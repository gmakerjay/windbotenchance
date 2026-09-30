// ============================================================================
// CARD AUDIT — OjamaLock (Modernized Ojama 5-Zone Complete Lock & ABC Engine)
// ============================================================================
// | Card Name                | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                         |
// |--------------------------|--------------|------|-------|---------|-----------------------------------------------|---------------------------------------|
// | Ojama King (90140980)    | Fusion L6    | No   | No    | None    | Select up to 3 opp monster zones; disable them| On Fusion Summon                      |
// | Ojama Knight (40391316)  | Fusion L5    | No   | No    | None    | Select up to 2 opp monster zones; disable them| On Fusion Summon                      |
// | Ground Collapse(90502999)| Spell Cont   | No   | No    | None    | Select 2 MMZ; neither player can use them     | Main Phase (Target opponent's zones!) |
// | Ojama Country (90011152) | Spell Field  | No   | No    | Discard | Pitch 1 Ojama -> SS 1 Ojama from GY; swap P/D | Main Phase (Revive Boss / Enable 3000)|
// | Ojama Trio (29843091)    | Trap Normal  | No   | No    | None    | SS 3 Ojama Tokens to opp field; clog zones    | Opp turn when opp has 3+ free zones   |
// | Ojama Duo (14470845)     | Trap Normal  | No   | No    | None    | SS 2 Ojama Tokens to opp field; GY banish SS 2| Opp turn / GY banish SS 2 from Deck   |
// | Ojama Pajama (75884822)  | Trap Cont    | Yes  | Yes   | Discard | Quick: Search Ojama, discard 1; float if sent | Main Phase (discard Ojamagic for +3!) |
// | Ojamassimilation(2390019)| Spell Normal | Yes  | Yes   | Banish  | Reveal ABC -> Banish Ojamas -> SS A, B, C     | Main Phase (1 card into 3 ABC pieces!)|
// | Ojamatch (38395123)      | Spell Quick  | Yes  | Yes   | Send GY | Send Ojama -> Add Ojama + Armed Dragon -> NS  | Main Phase (Send Ojamagic for +3!)    |
// | Ojamagic (24643836)      | Spell Normal | No   | No    | None    | Sent to GY -> Add Green, Yellow, Black (+3!)  | Triggered automatically on discard/send|
// | Polymerization(24094653) | Spell Normal | No   | No    | Hand/Fld| Fuse into Ojama King (3) or Knight (2)        | Have required materials in hand/field |
// | Tri-Wight (96383838)     | Spell Normal | No   | No    | Target  | Target 3 Level 2 Normal in GY -> SS them      | Have 3 Normal Ojamas in GY            |
// | ABC-Dragon Buster(1561110)| Fusion L8   | Yes  | No    | Discard | Quick: Discard 1 -> Banish 1 card; Tag out opp| Threat on field / Opponent turn tag out|
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("OjamaLock", "OjamaLock")]
    public class OjamaLockExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Deck - Ojamas & Normal Materials
            public const int OjamaGreen = 12482652;
            public const int OjamaYellow = 42941100;
            public const int OjamaBlack = 79335209;
            public const int OjamaBlue = 64627453;
            public const int OjamaRed = 37132349;
            public const int OjamaPink = 42517468;

            // ABC Machine Pieces
            public const int AAssaultCore = 30012506;
            public const int BBusterDrake = 77411244;
            public const int CCrushWyvern = 3405259;

            // Armed Dragon Thunder Engine
            public const int ArmedDragonThunderLV5 = 21546416;
            public const int ArmedDragonThunderLV3 = 57030525;

            // Handtraps
            public const int AshBlossom = 14558127;
            public const int InfiniteImpermanence = 10045474;

            // Spells
            public const int Ojamagic = 24643836;
            public const int Ojamassimilation = 2390019;
            public const int Ojamatch = 38395123;
            public const int Polymerization = 24094653;
            public const int TriWight = 96383838;
            public const int GroundCollapse = 90502999;
            public const int OjamaCountry = 90011152;

            // Traps
            public const int OjamaPajama = 75884822;
            public const int OjamaTrio = 29843091;
            public const int OjamaDuo = 14470845;
            public const int OjamaToken = 29843092;

            // Extra Deck
            public const int OjamaKing = 90140980;
            public const int OjamaKnight = 40391316;
            public const int ABCDragonBuster = 1561110;
            public const int OjamaEmperor = 34031284;
            public const int RoninRaccoonSandayu = 39972129;
            public const int SkyCavalryCentaurea = 36776089;
            public const int LinkSpider = 98978921;
            public const int IPMasquerena = 65741786;
            public const int SPLittleKnight = 29301450;
            public const int KnightmarePhoenix = 2857636;
            public const int KnightmareUnicorn = 38342335;
            public const int AccesscodeTalker = 86066372;
        }

        internal OjamaLockPlugin Plugin { get; private set; }
        private bool _ojamaPajamaSearchUsedThisTurn = false;
        private bool _ojamaCountryReviveUsedThisTurn = false;
        private bool _ojamassimilationUsedThisTurn = false;
        private bool _ojamatchUsedThisTurn = false;
        private bool _ojamaDuoGYUsedThisTurn = false;

        public OjamaLockExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            Plugin = new OjamaLockPlugin(this);
            DeckPlugin = Plugin;

            RegisterHandtrapsAndEmergency();
            RegisterExecutors();
        }

        public override bool OnSelectHand() => true; // Always choose to go first to establish 5-zone lock!

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _ojamaPajamaSearchUsedThisTurn = false;
            _ojamaCountryReviveUsedThisTurn = false;
            _ojamassimilationUsedThisTurn = false;
            _ojamatchUsedThisTurn = false;
            _ojamaDuoGYUsedThisTurn = false;
            Plugin?.ResetTurnState();
        }

        private void RegisterHandtrapsAndEmergency()
        {
            // Tier 0: Emergency Handtraps & Interruptions
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);

            // ABC Tag-Out (Emergency evasion / Opponent End Phase tag)
            AddExecutor(ExecutorType.Activate, CardId.ABCDragonBuster, ABCDragonBusterTagOut);
        }

        private void RegisterExecutors()
        {
            // ── Tier 1: Zone Locking, Continuous Traps & Field Spells ──
            AddExecutor(ExecutorType.Activate, CardId.OjamaPajama, OjamaPajamaActivate);
            AddExecutor(ExecutorType.Activate, CardId.GroundCollapse, GroundCollapseActivate);
            AddExecutor(ExecutorType.Activate, CardId.OjamaCountry, OjamaCountryActivate);
            AddExecutor(ExecutorType.Activate, CardId.OjamaTrio, OjamaTrioActivate);
            AddExecutor(ExecutorType.Activate, CardId.OjamaDuo, OjamaDuoActivate);

            // ── Tier 2: Searchers, Starters & Engines ──
            AddExecutor(ExecutorType.Activate, CardId.Ojamatch, OjamatchActivate);
            AddExecutor(ExecutorType.Activate, CardId.Ojamassimilation, OjamassimilationActivate);

            // ── Tier 3: Boss Monster Summons ──
            AddExecutor(ExecutorType.SpSummon, CardId.ABCDragonBuster, ABCDragonBusterSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Polymerization, PolymerizationActivate);
            AddExecutor(ExecutorType.Activate, CardId.TriWight, TriWightActivate);

            // ── Tier 4: On-Field Ignition & Quick Effects ──
            AddExecutor(ExecutorType.Activate, CardId.ABCDragonBuster, ABCDragonBusterBanish);
            AddExecutor(ExecutorType.Activate, CardId.ArmedDragonThunderLV3, ArmedDragonThunderLV3Activate);
            AddExecutor(ExecutorType.Activate, CardId.ArmedDragonThunderLV5, ArmedDragonThunderLV5Activate);
            AddExecutor(ExecutorType.Activate, CardId.RoninRaccoonSandayu, RoninRaccoonSandayuActivate);
            AddExecutor(ExecutorType.Activate, CardId.SkyCavalryCentaurea, SkyCavalryCentaureaActivate);

            // ── Tier 5: Union Pieces GY Trigger Effects ──
            AddExecutor(ExecutorType.Activate, CardId.BBusterDrake, UnionGYTrigger);
            AddExecutor(ExecutorType.Activate, CardId.AAssaultCore, UnionGYTrigger);
            AddExecutor(ExecutorType.Activate, CardId.CCrushWyvern, UnionGYTrigger);

            // ── Tier 6: Extra Deck Link Climbing ──
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, AccesscodeTalkerSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker, AccesscodeTalkerActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareUnicorn, KnightmareUnicornSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.KnightmareUnicorn, KnightmareUnicornActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmarePhoenix, KnightmarePhoenixSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.KnightmarePhoenix, KnightmarePhoenixActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.OjamaEmperor, OjamaEmperorSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.OjamaEmperor, OjamaEmperorActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.LinkSpider, LinkSpiderSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.IPMasquerena, IPMasquerenaSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.IPMasquerena, IPMasquerenaActivate);

            // ── Tier 7: Xyz Summons ──
            AddExecutor(ExecutorType.SpSummon, CardId.RoninRaccoonSandayu, RoninRaccoonSandayuSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SkyCavalryCentaurea, SkyCavalryCentaureaSpSummon);

            // ── Tier 8: Normal Summons & Battle Crash ──
            AddExecutor(ExecutorType.Summon, CardId.OjamaRed, OjamaRedSummon);
            AddExecutor(ExecutorType.Activate, CardId.OjamaRed, OjamaRedActivate);
            AddExecutor(ExecutorType.Summon, CardId.OjamaBlue, OjamaBlueSummon);
            AddExecutor(ExecutorType.MonsterSet, CardId.OjamaBlue, OjamaBlueSet);
            AddExecutor(ExecutorType.Activate, CardId.OjamaBlue, OjamaBlueActivate);
            AddExecutor(ExecutorType.Activate, CardId.OjamaPink, OjamaPinkActivate);

            // Normal Summons of ABC pieces if needed to establish board presence
            AddExecutor(ExecutorType.Summon, CardId.BBusterDrake, NormalMonsterSummon);
            AddExecutor(ExecutorType.Summon, CardId.AAssaultCore, NormalMonsterSummon);
            AddExecutor(ExecutorType.Summon, CardId.CCrushWyvern, NormalMonsterSummon);
            AddExecutor(ExecutorType.Summon, CardId.OjamaGreen, NormalMonsterSummon);
            AddExecutor(ExecutorType.Summon, CardId.OjamaYellow, NormalMonsterSummon);
            AddExecutor(ExecutorType.Summon, CardId.OjamaBlack, NormalMonsterSummon);

            // ── Tier 9: Spell / Trap Sets & Smart Repositioning ──
            AddExecutor(ExecutorType.SpellSet, CardId.OjamaPajama, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.OjamaTrio, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.OjamaDuo, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, SpellSetStrategy);
            AddExecutor(ExecutorType.Repos, SmartMonsterRepos);
        }

        #region Card Logic

        private bool OjamaPajamaActivate()
        {
            if (Card.Location == CardLocation.SpellZone)
            {
                // In SpellZone: Trigger the search & discard effect during Main Phase (HOPT)
                if (Card.IsFaceup())
                {
                    if (_ojamaPajamaSearchUsedThisTurn) return false;
                    _ojamaPajamaSearchUsedThisTurn = true;
                    return true;
                }
                // Flipping face up: only if no other face-up Ojama Pajama already exists!
                bool alreadyHasFaceup = Bot.SpellZone.Any(c => c != null && c != Card && c.Id == CardId.OjamaPajama && c.IsFaceup());
                if (alreadyHasFaceup) return false;
                return true;
            }
            if (Card.Location == CardLocation.Hand)
            {
                // From hand: do not activate if we already have one active
                bool alreadyActive = Bot.SpellZone.Any(c => c != null && c.Id == CardId.OjamaPajama);
                return Bot.GetSpellCount() < 4 && !alreadyActive;
            }
            return false;
        }

        private bool IsCurrentCardNegated()
        {
            return Card != null && DefaultCheckWhetherCardIsNegated(Card);
        }

        private bool GroundCollapseActivate()
        {
            if (IsCurrentCardNegated()) return false;
            // ANTI-SELF-HARM & STACK-AWARE:
            // Ground Collapse requires locking 2 monster zones.
            // ONLY activate if opponent has at least 2 unoccupied, unlocked monster zones!
            // If opponent has <= 1 free zone, the effect would force us to lock our OWN monster zones!
            int oppFreeZones = 5 - Enemy.GetMonsterCount();
            return oppFreeZones >= 2;
        }

        private bool OjamaCountryActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return !Bot.HasInSpellZone(CardId.OjamaCountry);
            }

            // On-field ignition effect: Send 1 Ojama card from hand to GY -> Special Summon 1 Ojama from GY (OPT)
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                if (_ojamaCountryReviveUsedThisTurn) return false;
                bool hasOjamaInHand = Bot.Hand.Any(c => IsOjamaCard(c.Id));
                bool hasOjamaInGY = Bot.Graveyard.Any(c => IsOjamaMonster(c.Id));
                if (hasOjamaInHand && hasOjamaInGY && Bot.GetMonsterCount() < 5)
                {
                    _ojamaCountryReviveUsedThisTurn = true;
                    return true;
                }
            }

            return false;
        }

        private bool OjamaTrioActivate()
        {
            // Clog opponent zones: Need opponent to have at least 3 free monster zones
            int oppFreeZones = 5 - Enemy.GetMonsterCount();
            if (oppFreeZones < 3) return false;

            // Opponent turn: activate to disrupt plays
            if (Duel.Player == 1) return true;

            // Our turn: lock remaining zones
            return oppFreeZones >= 3;
        }

        private bool OjamaDuoActivate()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // GY effect: Banish to SS 2 Ojamas with different names from deck! (HOPT)
                if (_ojamaDuoGYUsedThisTurn) return false;
                if (Bot.GetMonsterCount() <= 3)
                {
                    _ojamaDuoGYUsedThisTurn = true;
                    return true;
                }
                return false;
            }

            int oppFreeZones = 5 - Enemy.GetMonsterCount();
            if (oppFreeZones < 2) return false;

            if (Duel.Player == 1) return true;
            return oppFreeZones >= 2;
        }

        private bool OjamatchActivate()
        {
            if (_ojamatchUsedThisTurn) return false;

            if (Card.Location == CardLocation.Grave)
            {
                // GY effect: Banish to shuffle 3 banished Ojamas and draw 1! (HOPT)
                if (Bot.Banished.Count(c => IsOjamaCard(c.Id)) >= 3)
                {
                    _ojamatchUsedThisTurn = true;
                    return true;
                }
                return false;
            }

            // Hand/Field effect: Send 1 Ojama card to search Ojama + Armed Dragon, then Normal Summon (HOPT)
            bool hasOjamaToSend = Bot.Hand.Any(c => IsOjamaCard(c.Id)) ||
                                  Bot.GetMonsters().Any(c => IsOjamaCard(c.Id)) ||
                                  Bot.GetSpells().Any(c => IsOjamaCard(c.Id) && c.IsFaceup());
            if (hasOjamaToSend)
            {
                _ojamatchUsedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool OjamassimilationActivate()
        {
            if (_ojamassimilationUsedThisTurn) return false;

            if (Card.Location == CardLocation.Grave)
            {
                // GY effect: Banish to shuffle 3 banished Ojamas and draw 1! (HOPT)
                if (Bot.Banished.Count(c => IsOjamaCard(c.Id)) >= 3)
                {
                    _ojamassimilationUsedThisTurn = true;
                    return true;
                }
                return false;
            }

            // Reveal ABC-Dragon Buster, banish Ojamas, SS A, B, C! (HOPT)
            bool hasABCInExtra = Bot.ExtraDeck.Any(c => c.Id == CardId.ABCDragonBuster);
            if (!hasABCInExtra) return false;

            int ojamaCount = Bot.Hand.Count(c => IsOjamaMonster(c.Id)) +
                             Bot.GetMonsters().Count(c => IsOjamaMonster(c.Id)) +
                             Bot.Graveyard.Count(c => IsOjamaMonster(c.Id));

            if (ojamaCount >= 1 && Bot.GetMonsterCount() < 5)
            {
                _ojamassimilationUsedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool SpellSetStrategy()
        {
            if (Card == null) return false;

            // STACK-AWARE GUARD: Never set duplicate Spells/Traps if one is already set/active in SpellZone
            if (Bot.SpellZone.Any(c => c != null && c.Id == Card.Id))
                return false;

            // STACK-AWARE GUARD: Preserve at least 1-2 free Spell/Trap zones for Normal Spells (Ojamassimilation, Ojamatch, Poly, Tri-Wight)
            if (Bot.GetSpellCount() >= 4)
                return false;

            // Ojama Trio requires 3 empty monster zones to activate
            if (Card.Id == CardId.OjamaTrio && Enemy.GetMonsterCount() > 2)
                return false;

            // Ojama Duo requires 2 empty monster zones to activate
            if (Card.Id == CardId.OjamaDuo && Enemy.GetMonsterCount() > 3)
                return false;

            if (Card.IsTrap()) return true;
            if (Card.IsSpell() && Card.HasType(CardType.QuickPlay)) return Duel.Phase == DuelPhase.Main2;
            return false;
        }

        private bool ABCDragonBusterSpSummon()
        {
            // Contact fusion: Banish A, B, C from Field or GY!
            bool hasA = Bot.GetMonsters().Any(c => c.Id == CardId.AAssaultCore) || Bot.Graveyard.Any(c => c.Id == CardId.AAssaultCore);
            bool hasB = Bot.GetMonsters().Any(c => c.Id == CardId.BBusterDrake) || Bot.Graveyard.Any(c => c.Id == CardId.BBusterDrake);
            bool hasC = Bot.GetMonsters().Any(c => c.Id == CardId.CCrushWyvern) || Bot.Graveyard.Any(c => c.Id == CardId.CCrushWyvern);

            return hasA && hasB && hasC && Bot.ExtraDeck.Any(c => c.Id == CardId.ABCDragonBuster);
        }

        private bool ABCDragonBusterBanish()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Quick effect: Discard 1 card -> Banish 1 card on the field
            var enemyCards = Enemy.GetMonsters().Concat(Enemy.GetSpells()).ToList();
            if (enemyCards.Count > 0 && Bot.Hand.Count > 0)
            {
                var target = enemyCards.OrderByDescending(c => Plugin.ThreatImpl.EvaluateThreatScore(c)).FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }

            return false;
        }

        private bool ABCDragonBusterTagOut()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Opponent turn tag-out: Tag out if targeted, negated, or in End Phase to float into 3 pieces!
            if (Duel.Player == 1)
            {
                if (Duel.Phase == DuelPhase.End || DefaultCheckWhetherCardIsNegated(Card) || Duel.LastChainPlayer == 1)
                {
                    int banishedPieces = Bot.Banished.Count(c =>
                        c.Id == CardId.AAssaultCore || c.Id == CardId.BBusterDrake || c.Id == CardId.CCrushWyvern);
                    return banishedPieces >= 3;
                }
            }

            return false;
        }

        private bool PolymerizationActivate()
        {
            // Ojama King check: Need Green, Yellow, Black in hand/field
            bool hasGreen = Bot.Hand.Any(c => c.Id == CardId.OjamaGreen) || Bot.GetMonsters().Any(c => c.Id == CardId.OjamaGreen);
            bool hasYellow = Bot.Hand.Any(c => c.Id == CardId.OjamaYellow) || Bot.GetMonsters().Any(c => c.Id == CardId.OjamaYellow);
            bool hasBlack = Bot.Hand.Any(c => c.Id == CardId.OjamaBlack) || Bot.GetMonsters().Any(c => c.Id == CardId.OjamaBlack);

            if (hasGreen && hasYellow && hasBlack && Bot.ExtraDeck.Any(c => c.Id == CardId.OjamaKing))
            {
                AI.SelectCard(CardId.OjamaKing);
                return true;
            }

            // Ojama Knight check: Need any 2 Ojamas in hand/field
            int ojamaMaterials = Bot.Hand.Count(c => IsOjamaMonster(c.Id)) + Bot.GetMonsters().Count(c => IsOjamaMonster(c.Id));
            if (ojamaMaterials >= 2 && Bot.ExtraDeck.Any(c => c.Id == CardId.OjamaKnight))
            {
                AI.SelectCard(CardId.OjamaKnight);
                return true;
            }

            return false;
        }

        private bool TriWightActivate()
        {
            int normalOjamasInGY = Bot.Graveyard.Count(c =>
                c.Id == CardId.OjamaGreen || c.Id == CardId.OjamaYellow || c.Id == CardId.OjamaBlack);
            return normalOjamasInGY >= 3 && Bot.GetMonsterCount() <= 2;
        }

        private bool ArmedDragonThunderLV3Activate()
        {
            return Bot.Hand.Count > 0 && Bot.GetMonsterCount() < 5;
        }

        private bool ArmedDragonThunderLV5Activate()
        {
            return Enemy.GetMonsters().Any(c => c.IsFaceup() && c.Attack <= 2400) && Bot.Hand.Count > 0;
        }

        private bool RoninRaccoonSandayuActivate()
        {
            return Bot.GetMonsterCount() < 5;
        }

        private bool SkyCavalryCentaureaActivate()
        {
            // Detach to bounce enemy monster that it battled
            return true;
        }

        private bool UnionGYTrigger()
        {
            // Automatically accept A, B, C GY trigger effects
            return true;
        }

        private bool AccesscodeTalkerSpSummon()
        {
            int linkRating = Bot.GetMonsters().Where(c => c.IsFaceup()).Sum(c => c.HasType(CardType.Link) ? c.LinkMarker : 1);
            return linkRating >= 4 && (Duel.Turn >= 2 || Enemy.GetMonsterCount() > 0);
        }

        private bool AccesscodeTalkerActivate()
        {
            return true;
        }

        private bool KnightmareUnicornSpSummon()
        {
            return Bot.GetMonsterCount() >= 3 && Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0;
        }

        private bool KnightmareUnicornActivate()
        {
            var target = Enemy.GetMonsters().Concat(Enemy.GetSpells()).OrderByDescending(c => Plugin.ThreatImpl.EvaluateThreatScore(c)).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool KnightmarePhoenixSpSummon()
        {
            return Bot.GetMonsterCount() >= 2 && Enemy.GetSpellCount() > 0;
        }

        private bool KnightmarePhoenixActivate()
        {
            var target = Enemy.GetSpells().OrderByDescending(c => Plugin.ThreatImpl.EvaluateThreatScore(c)).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SPLittleKnightSpSummon()
        {
            return Bot.GetMonsterCount() >= 2 && Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0;
        }

        private bool SPLittleKnightActivate()
        {
            var target = Enemy.GetMonsters().Concat(Enemy.GetSpells()).OrderByDescending(c => Plugin.ThreatImpl.EvaluateThreatScore(c)).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool OjamaEmperorSpSummon()
        {
            // Requires 3 Beast monsters, including an Ojama
            int beasts = Bot.GetMonsters().Count(c => c.IsFaceup() && c.HasRace(CardRace.Beast));
            bool hasOjama = Bot.GetMonsters().Any(c => c.IsFaceup() && IsOjamaMonster(c.Id));
            return beasts >= 3 && hasOjama;
        }

        private bool OjamaEmperorActivate()
        {
            // Target 1 non-Link Ojama monster in GY -> Special Summon it
            return Bot.GetMonsterCount() < 5 && Bot.Graveyard.Any(c => IsOjamaMonster(c.Id));
        }

        private bool LinkSpiderSpSummon()
        {
            // Convert a Normal Ojama into Effect Monster for Link climbing
            return Bot.GetMonsters().Any(c =>
                c.Id == CardId.OjamaGreen || c.Id == CardId.OjamaYellow || c.Id == CardId.OjamaBlack);
        }

        private bool IPMasquerenaSpSummon()
        {
            return Bot.GetMonsterCount() >= 2 && !Bot.HasInMonstersZone(CardId.IPMasquerena);
        }

        private bool IPMasquerenaActivate()
        {
            // Quick effect on opponent's turn: Link Summon into Knightmare Unicorn or S:P Little Knight
            return Duel.Player == 1 && Bot.GetMonsterCount() >= 2;
        }

        private bool RoninRaccoonSandayuSpSummon()
        {
            int level2Beasts = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 2 && c.HasRace(CardRace.Beast));
            return level2Beasts >= 2;
        }

        private bool SkyCavalryCentaureaSpSummon()
        {
            int level2 = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 2);
            return level2 >= 2 && Enemy.GetMonsters().Any(c => c.IsFaceup() && c.Attack >= 2000);
        }

        private bool OjamaRedSummon()
        {
            return true;
        }

        private bool OjamaRedActivate()
        {
            // Special Summon up to 4 Ojamas from hand
            return Bot.Hand.Any(c => IsOjamaMonster(c.Id));
        }

        private bool OjamaBlueSummon()
        {
            // Attack into stronger enemy monster to crash and trigger search 2 Ojama cards!
            var oppStronger = Enemy.GetMonsters().FirstOrDefault(c => c.IsFaceup() && c.Attack > 0);
            return oppStronger != null && Duel.Phase == DuelPhase.Main1;
        }

        private bool OjamaBlueSet()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool OjamaBlueActivate()
        {
            return true;
        }

        private bool OjamaPinkActivate()
        {
            return true;
        }

        private bool NormalMonsterSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        #endregion

        #region Zone Locking & Selection Handlers

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            // Zone Lock for Opponent: Ojama King, Ojama Knight, Ground Collapse, Ojama Pink
            if (player == 1 && location == CardLocation.MonsterZone)
            {
                return Plugin.ZoneLockManager.SelectLockZone(available, location);
            }

            return base.OnSelectPlace(cardId, player, location, available);
        }

        public override uint OnSelectDisfield(long hint, int count, uint available)
        {
            // Lock Opponent Monster Zones: Center (Zone 2 = 0x40000), Left-Center (Zone 1 = 0x20000),
            // Right-Center (Zone 3 = 0x80000), Left-Edge (Zone 0 = 0x10000), Right-Edge (Zone 4 = 0x100000)
            uint[] oppMonsterZones = { 0x40000, 0x20000, 0x80000, 0x10000, 0x100000, 0x200000, 0x400000 };
            uint selected = 0;
            int picked = 0;

            foreach (uint mask in oppMonsterZones)
            {
                if ((available & mask) != 0 && (selected & mask) == 0)
                {
                    selected |= mask;
                    picked++;
                    if (picked >= count) return selected;
                }
            }

            return base.OnSelectDisfield(hint, count, available);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // 1. Ojamassimilation: Extra Deck Reveal -> Select ABC-Dragon Buster
            if (cards.Any(c => c.Location == CardLocation.Extra && c.Id == CardId.ABCDragonBuster))
            {
                var abc = cards.FirstOrDefault(c => c.Id == CardId.ABCDragonBuster);
                if (abc != null) return new List<ClientCard> { abc };
            }

            // 2. Removal / Spin / Bounce / Banish / Destroy (Hints: 502, 503, 505, 507) -> TARGET OPPONENT ONLY!
            if (hint == 502 || hint == 503 || hint == 505 || hint == 507)
            {
                var oppCards = cards.Where(c => c.Controller == 1).ToList();
                if (oppCards.Count >= min)
                {
                    var sorted = oppCards.OrderByDescending(c => Plugin.ThreatImpl.EvaluateThreatScore(c))
                                         .ThenByDescending(c => c.Attack)
                                         .ToList();
                    return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                }
            }

            // 3. Discard Target (Hint 501 or Discard Prompt) -> Prioritize Ojamagic
            if (hint == 501 || cards.All(c => c.Location == CardLocation.Hand))
            {
                var discardTarget = Plugin.MaterialImpl.PickDiscardTarget(cards, min);
                if (discardTarget != null && cards.Contains(discardTarget))
                {
                    var result = new List<ClientCard> { discardTarget };
                    while (result.Count < min)
                    {
                        var next = cards.FirstOrDefault(c => !result.Contains(c));
                        if (next == null) break;
                        result.Add(next);
                    }
                    return result;
                }
            }

            // 4. Ojamassimilation Banish Selection (from Hand / Field / GY)
            if (Util.GetLastChainCard() != null && Util.GetLastChainCard().Id == CardId.Ojamassimilation && cards.All(c => IsOjamaMonster(c.Id)))
            {
                return Plugin.MaterialImpl.PickOjamassimilationBanish(cards, Math.Min(max, 3));
            }

            // 5. Special Summon Target (Hint 509)
            if (hint == 509)
            {
                var ssTarget = Plugin.StrategyImpl.PickSpecialSummonTarget(cards);
                if (ssTarget != null && cards.Contains(ssTarget))
                {
                    var result = new List<ClientCard> { ssTarget };
                    while (result.Count < min)
                    {
                        var next = cards.FirstOrDefault(c => !result.Contains(c));
                        if (next == null) break;
                        result.Add(next);
                    }
                    return result;
                }
            }

            // 6. Search Target (Hint 506)
            if (hint == 506 || cards.All(c => c.Location == CardLocation.Deck))
            {
                var searchTarget = Plugin.StrategyImpl.PickSearchTarget(cards, Card);
                if (searchTarget != null && cards.Contains(searchTarget))
                {
                    var result = new List<ClientCard> { searchTarget };
                    while (result.Count < min)
                    {
                        var next = cards.FirstOrDefault(c => !result.Contains(c));
                        if (next == null) break;
                        result.Add(next);
                    }
                    return result;
                }
            }

            // 7. Recycle banished Ojamas for Ojamatch / Ojamassimilation GY draw effect
            if (cards.All(c => c.Location == CardLocation.Removed && IsOjamaMonster(c.Id)))
            {
                return cards.Take(Math.Min(max, 3)).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectFusionMaterial(IList<ClientCard> cards, int min, int max)
        {
            if (cards == null) return base.OnSelectFusionMaterial(cards, min, max);

            // Prioritize normal Ojamas (Green, Yellow, Black) from hand/field, protect Bosses
            var sorted = cards.OrderBy(c => Plugin.MaterialImpl.GetMaterialCost(c)).ToList();
            return sorted.Take(min).ToList();
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Ojama Blue: FaceUpAttack (crash strategy)
            if (cardId == CardId.OjamaBlue)
                return CardPosition.FaceUpAttack;

            // Ojama King & Knight:
            // If Ojama Country is on field: ATK and DEF are swapped -> 3000 ATK / 2500 ATK (Attack Mode!)
            // If Ojama Country is NOT active: 3000 DEF / 2500 DEF (Defense Wall Mode!)
            if (cardId == CardId.OjamaKing || cardId == CardId.OjamaKnight)
            {
                bool countryActive = Bot.HasInSpellZone(CardId.OjamaCountry);
                if (countryActive && positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }

            // Attackers & Bosses -> FaceUpAttack
            int[] attackers = {
                CardId.ABCDragonBuster,
                CardId.AccesscodeTalker,
                CardId.OjamaEmperor,
                CardId.SPLittleKnight,
                CardId.KnightmareUnicorn,
                CardId.KnightmarePhoenix,
                CardId.AAssaultCore,
                CardId.BBusterDrake,
                CardId.CCrushWyvern,
                CardId.ArmedDragonThunderLV5
            };
            if (attackers.Contains(cardId) && positions.Contains(CardPosition.FaceUpAttack))
            {
                return CardPosition.FaceUpAttack;
            }

            // Normal Ojamas & Walls -> FaceUpDefence
            if (cardId == CardId.OjamaGreen || cardId == CardId.OjamaYellow || cardId == CardId.OjamaBlack ||
                cardId == CardId.AshBlossom || cardId == CardId.OjamaRed || cardId == CardId.OjamaPink)
            {
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            if (card == null) return null;

            // Reject opponent effects by default
            if (card.Controller == 1) return false;

            // Always activate Ojamagic (+3 card advantage)
            if (card.Id == CardId.Ojamagic) return true;

            // Always activate Ojama Blue (+2 card search)
            if (card.Id == CardId.OjamaBlue) return true;

            // Always activate Ojama Pajama floating effect
            if (card.Id == CardId.OjamaPajama) return true;

            // Always activate Ojama Pink (draw 1, discard 1, lock 1 zone)
            if (card.Id == CardId.OjamaPink) return true;

            // Always activate A, B, C floating triggers
            if (card.Id == CardId.AAssaultCore || card.Id == CardId.BBusterDrake || card.Id == CardId.CCrushWyvern)
                return true;

            // Accept Ojamatch bonus Normal Summon
            if (card.Id == CardId.Ojamatch) return true;

            return base.OnSelectEffectYn(card, desc);
        }

        private bool SmartMonsterRepos()
        {
            if (Card == null) return false;

            // 1. Ojama King / Knight under Ojama Country: Switch to Attack to deal 3000 / 2500 damage!
            bool countryActive = Bot.HasInSpellZone(CardId.OjamaCountry);
            if ((Card.Id == CardId.OjamaKing || Card.Id == CardId.OjamaKnight))
            {
                if (countryActive && Card.IsDefense() && Duel.Phase == DuelPhase.Main1 && Duel.Player == 0)
                    return true;
                if (!countryActive && Card.IsAttack())
                    return true;
            }

            // 2. High ATK monsters stranded in Defense -> Switch to Attack for battle pushes
            if (Card.IsDefense() && Card.Attack >= 2400 && Duel.Phase == DuelPhase.Main1 && Duel.Player == 0)
            {
                return true;
            }

            // 3. 0 ATK Ojamas stranded in Attack -> Switch to Defense!
            if (Card.IsAttack() && Card.Attack == 0 && !countryActive)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Helper Methods

        private bool IsOjamaMonster(int cardId)
        {
            return cardId == CardId.OjamaGreen ||
                   cardId == CardId.OjamaYellow ||
                   cardId == CardId.OjamaBlack ||
                   cardId == CardId.OjamaBlue ||
                   cardId == CardId.OjamaRed ||
                   cardId == CardId.OjamaPink;
        }

        private bool IsOjamaCard(int cardId)
        {
            return IsOjamaMonster(cardId) ||
                   cardId == CardId.Ojamagic ||
                   cardId == CardId.Ojamassimilation ||
                   cardId == CardId.Ojamatch ||
                   cardId == CardId.OjamaPajama ||
                   cardId == CardId.OjamaTrio ||
                   cardId == CardId.OjamaDuo ||
                   cardId == CardId.OjamaCountry;
        }

        #endregion
    }
}
