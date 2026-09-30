// ============================================================================
// CARD AUDIT — LairOfDarkness (Modernized STR41 Lair of Darkness - Tribute Lock & Viruses)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Lair of Darkness                   | Spell Field  | No   | No    | None    | Field is DARK; Tribute opp monster as cost; TT| Priority 1: Activate before any play         | Already active on field                     |
// | Darkest Diabolos, Lord of Lair     | Monster L8   | Yes  | Yes   | Tribute | SS from Hand/GY when DARK tributed; Hand rip  | When DARK tributed / Main Phase ignition     | No DARK to tribute / Opp hand empty         |
// | Lilith, Lady of Lament             | Monster L3   | Yes  | No    | Tribute | Quick: Tribute 1 DARK -> Reveal 3 Normal Traps| Opponent turn or during combo to disrupt     | No traps in deck                            |
// | Ahrima, the Wicked Warden          | Monster L4   | Yes  | Yes   | Discard | Discard -> Search Lair; Tribute DARK -> Search| Need Lair / Tribute opp monster to search Dia| No Lair in deck                             |
// | Malice, Lady of Lament             | Monster L3   | Yes  | No    | Tribute | Quick: Tribute 2 mons -> Set GY/Banished Trap | When high impact trap in GY/Banished         | Less than 2 monsters available               |
// | Radian, the Multidimensional Kaiju | Monster L7   | No   | No    | Opp Mon | Tribute opp monster to SS Radian to their side| Opponent has big threat monster              | Opp field empty                             |
// | Super Polymerization               | Spell Quick  | No   | No    | Discard | Unchainable Fusion using monsters either field| Opp has 2+ monsters under Lair               | No valid fusion in Extra                    |
// | Eradicator Epidemic Virus (EEV)    | Trap Normal  | No   | No    | Tribute | Tribute 2500+ DARK -> Declare Spell/Trap check| Have 2500+ DARK (Diabolos/Radian/opp monster)| No 2500+ DARK available                     |
// | Deck Devastation Virus (DDV)       | Trap Normal  | No   | No    | Tribute | Tribute 2000+ DARK -> Destroy <=1500 ATK mons| Have 2000+ DARK                              | No 2000+ DARK available                     |
// | Full Force Virus (FFV)             | Trap Normal  | No   | No    | Tribute | Tribute 2000+ DEF DARK -> Destroy <=1500 DEF | Have 2000+ DEF (Lilith/Malice/Diabolos)      | No 2000+ DEF available                     |
// | Share the Pain                     | Spell Normal | No   | No    | Tribute | Tribute 1 monster -> Opp must tribute 1 mon   | Under Lair: Tribute opp mon -> opp loses 2!  | No monsters to tribute                      |
// | Trap Trick                         | Trap Normal  | Yes  | Yes   | Banish  | Banish 1 Trap -> Set 1 from deck, usable now  | Opponent turn / Need Virus or IDP            | Only 1 copy left in deck                    |
// | Ojama Trio                         | Trap Normal  | No   | No    | None    | SS 3 Ojama Tokens to opp field; clog board    | Opp turn: Clog board, tokens become DARK     | Opp has < 3 free zones                      |
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
    [Deck("LairOfDarkness", "LairOfDarkness")]
    public class LairOfDarknessExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int AhrimaTheWickedWarden = 23898021;
            public const int DarkestDiabolos = 85555787;
            public const int LilithLadyOfLament = 50383626;
            public const int MaliceLadyOfLament = 12766474;
            public const int RadianTheMultidimensionalKaiju = 28601770;
            public const int LordOfTheHeavenlyPrison = 9822220;
            public const int AshBlossom = 14558127;

            // Spells
            public const int LairOfDarkness = 59160188;
            public const int SuperPolymerization = 48130397;
            public const int PotOfExtravagance = 49238328;
            public const int ShareThePain = 56830749;
            public const int FoolishBurial = 81439173;
            public const int HarpiesFeatherDuster = 18144506;

            // Traps
            public const int EradicatorEpidemicVirus = 54974237;
            public const int DeckDevastationVirus = 57728570;
            public const int FullForceVirus = 29876529;
            public const int TrapTrick = 80101899;
            public const int IceDragonsPrison = 15800838;
            public const int OjamaTrio = 29843091;
            public const int InfiniteImpermanence = 10045474;
            public const int TormentToken = 59160189;

            // Extra Deck
            public const int StarvingVenomFusionDragon = 41209827;
            public const int MudragonOfTheSwamp = 43892408;
            public const int GaruraWingsOfResonantLife = 11765832;
            public const int PredaplantDragostapelia = 69946549;
            public const int Typhon = 93039339;
            public const int DharcTheDarkCharmerGloomy = 8264361;
            public const int WeeWitchsApprentice = 71384012;
            public const int KnightmarePhoenix = 2857636;
            public const int KnightmareUnicorn = 38342335;
            public const int SPLittleKnight = 29301450;
            public const int AccesscodeTalker = 86066372;
            public const int Linkuriboh = 41999284;
        }

        internal LairOfDarknessPlugin Plugin { get; private set; }

        public LairOfDarknessExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            Plugin = new LairOfDarknessPlugin(this);
            DeckPlugin = Plugin;

            RegisterHelperModules();
            RegisterExecutors();
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            Plugin?.ResetTurnState();
        }

        private void RegisterHelperModules()
        {
            // Register high-priority handtraps & interruptions
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);
        }

        private void RegisterExecutors()
        {
            // ── Tier 1: Field Spell & Hand Protection ──
            AddExecutor(ExecutorType.Activate, CardId.LairOfDarkness, LairOfDarknessActivate);
            AddExecutor(ExecutorType.Activate, CardId.LordOfTheHeavenlyPrison, LordOfTheHeavenlyPrisonActivate);
            AddExecutor(ExecutorType.Activate, CardId.PotOfExtravagance, PotOfExtravaganceActivate);
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, HarpiesFeatherDusterActivate);

            // ── Tier 2: Board Breaking & Super Poly ──
            AddExecutor(ExecutorType.SpSummon, CardId.RadianTheMultidimensionalKaiju, RadianKaijuSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationActivate);
            AddExecutor(ExecutorType.Activate, CardId.ShareThePain, ShareThePainActivate);
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurial, FoolishBurialActivate);

            // ── Tier 3: Searchers & Disruption Traps ──
            AddExecutor(ExecutorType.Activate, CardId.AhrimaTheWickedWarden, AhrimaActivate);
            AddExecutor(ExecutorType.Activate, CardId.TrapTrick, TrapTrickActivate);
            AddExecutor(ExecutorType.Activate, CardId.EradicatorEpidemicVirus, EradicatorEpidemicVirusActivate);
            AddExecutor(ExecutorType.Activate, CardId.DeckDevastationVirus, DeckDevastationVirusActivate);
            AddExecutor(ExecutorType.Activate, CardId.FullForceVirus, FullForceVirusActivate);
            AddExecutor(ExecutorType.Activate, CardId.IceDragonsPrison, IceDragonsPrisonActivate);
            AddExecutor(ExecutorType.Activate, CardId.OjamaTrio, OjamaTrioActivate);

            // ── Tier 4: Boss Monster Summons & Ignitions ──
            AddExecutor(ExecutorType.Activate, CardId.DarkestDiabolos, DarkestDiabolosActivate);
            AddExecutor(ExecutorType.Activate, CardId.LilithLadyOfLament, LilithActivate);
            AddExecutor(ExecutorType.Activate, CardId.MaliceLadyOfLament, MaliceActivate);

            // ── Tier 5: Extra Deck Summons ──
            AddExecutor(ExecutorType.SpSummon, CardId.Typhon, TyphonSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Typhon, TyphonActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, AccesscodeTalkerSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker, AccesscodeTalkerActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareUnicorn, KnightmareUnicornSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.KnightmareUnicorn, KnightmareUnicornActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmarePhoenix, KnightmarePhoenixSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.KnightmarePhoenix, KnightmarePhoenixActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.DharcTheDarkCharmerGloomy, DharcSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.DharcTheDarkCharmerGloomy, DharcActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.WeeWitchsApprentice, WeeWitchSpSummon);

            // ── Tier 6: Normal Summons ──
            AddExecutor(ExecutorType.Summon, CardId.LilithLadyOfLament, LilithSummon);
            AddExecutor(ExecutorType.Summon, CardId.AhrimaTheWickedWarden, AhrimaSummon);
            AddExecutor(ExecutorType.Summon, CardId.MaliceLadyOfLament, MaliceSummon);

            // ── Tier 7: Spells & Traps Setting ──
            AddExecutor(ExecutorType.SpellSet, CardId.TrapTrick);
            AddExecutor(ExecutorType.SpellSet, CardId.EradicatorEpidemicVirus);
            AddExecutor(ExecutorType.SpellSet, CardId.DeckDevastationVirus);
            AddExecutor(ExecutorType.SpellSet, CardId.FullForceVirus);
            AddExecutor(ExecutorType.SpellSet, CardId.IceDragonsPrison);
            AddExecutor(ExecutorType.SpellSet, CardId.SuperPolymerization);
            AddExecutor(ExecutorType.SpellSet, CardId.OjamaTrio);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);
        }

        #region Card Logic

        private bool LairOfDarknessActivate()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
                return false;

            return !Bot.HasInSpellZone(CardId.LairOfDarkness);
        }

        private bool LordOfTheHeavenlyPrisonActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Reveal from hand during Main Phase to protect set cards
                return Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2;
            }

            // When a set card is activated: SS itself to field!
            return true;
        }

        private bool PotOfExtravaganceActivate()
        {
            return Duel.Phase == DuelPhase.Main1 && Bot.ExtraDeck.Count >= 6;
        }

        private bool HarpiesFeatherDusterActivate()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool RadianKaijuSpSummon()
        {
            // Tribute opponent's biggest threat
            var oppThreat = Enemy.GetMonsters().OrderByDescending(c => Plugin.ThreatImpl.EvaluateThreatScore(c)).FirstOrDefault();
            if (oppThreat != null)
            {
                AI.SelectCard(oppThreat);
                return true;
            }
            return false;
        }

        private bool SuperPolymerizationActivate()
        {
            if (Bot.Hand.Count == 0) return false;

            bool isLairActive = Bot.HasInSpellZone(CardId.LairOfDarkness);
            int oppMonsters = Enemy.GetMonsterCount();

            // Under Lair of Darkness: All opp monsters are DARK!
            if (isLairActive && oppMonsters >= 2)
            {
                // Can fuse into Starving Venom (2 DARK non-tokens), Garura, or Mudragon
                return true;
            }

            // Normal Super Poly check: Opponent has 2 monsters of same attribute/type
            return oppMonsters >= 2;
        }

        private bool ShareThePainActivate()
        {
            // Under Lair of Darkness: Can tribute 1 opponent monster as cost!
            // Opponent then must tribute another monster! (Opponent loses 2 monsters!)
            bool isLairActive = Bot.HasInSpellZone(CardId.LairOfDarkness);
            if (isLairActive && Enemy.GetMonsterCount() >= 2)
                return true;

            // Otherwise, tribute a token or low cost monster
            return Bot.GetMonsters().Any(c => c.HasType(CardType.Token)) && Enemy.GetMonsterCount() > 0;
        }

        private bool FoolishBurialActivate()
        {
            // Dump Darkest Diabolos to GY
            if (!Bot.HasInHand(CardId.DarkestDiabolos) && !Bot.Graveyard.Any(c => c.Id == CardId.DarkestDiabolos))
            {
                AI.SelectCard(CardId.DarkestDiabolos);
                return true;
            }
            return false;
        }

        private bool AhrimaActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Discard to search Lair of Darkness
                return !Bot.HasInSpellZone(CardId.LairOfDarkness) && !Bot.HasInHand(CardId.LairOfDarkness);
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                // Under Lair: Tribute opponent's monster as cost to search Darkest Diabolos!
                bool isLairActive = Bot.HasInSpellZone(CardId.LairOfDarkness);
                if (isLairActive && Enemy.GetMonsterCount() > 0)
                    return true;

                // Or tribute own token
                return Bot.GetMonsters().Any(c => c.HasType(CardType.Token));
            }

            return false;
        }

        private bool LilithActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Quick effect: Tribute 1 DARK monster (can be opponent's under Lair!) -> Reveal 3 Normal Traps
            bool isLairActive = Bot.HasInSpellZone(CardId.LairOfDarkness);
            if (isLairActive && Enemy.GetMonsterCount() > 0)
                return true;

            // Or tribute own token / self if targeted
            if (Duel.Player == 1 || Bot.GetMonsters().Any(c => c.HasType(CardType.Token)) || DefaultCheckWhetherCardIsNegated(Card))
                return true;

            return false;
        }

        private bool MaliceActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Quick effect: Tribute 2 monsters (1 opp under Lair + 1 token) -> Set normal trap from GY/banished
            bool hasTargetTrap = Bot.Graveyard.Concat(Bot.Banished).Any(c => c.HasType(CardType.Trap));
            if (!hasTargetTrap) return false;

            bool isLairActive = Bot.HasInSpellZone(CardId.LairOfDarkness);
            int availableTributes = Bot.GetMonsterCount() + (isLairActive ? Enemy.GetMonsterCount() : 0);

            return availableTributes >= 2;
        }

        private bool DarkestDiabolosActivate()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave)
            {
                // Trigger: Special Summon when a DARK monster is tributed!
                return Bot.GetMonsterCount() < 5;
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                // Main Phase ignition: Tribute 1 DARK monster (opp under Lair or token) -> Hand rip!
                if (Enemy.Hand.Count == 0) return false;

                bool isLairActive = Bot.HasInSpellZone(CardId.LairOfDarkness);
                if (isLairActive && Enemy.GetMonsterCount() > 0)
                    return true;

                return Bot.GetMonsters().Any(c => c.HasType(CardType.Token));
            }

            return false;
        }

        private bool EradicatorEpidemicVirusActivate()
        {
            // Requires 2500+ ATK DARK monster
            bool isLairActive = Bot.HasInSpellZone(CardId.LairOfDarkness);
            bool hasTribute = Bot.GetMonsters().Any(c => c.IsFaceup() && (c.HasAttribute(CardAttribute.Dark) || isLairActive) && c.Attack >= 2500) ||
                             (isLairActive && Enemy.GetMonsters().Any(c => c.IsFaceup() && c.Attack >= 2500));

            return hasTribute;
        }

        private bool DeckDevastationVirusActivate()
        {
            // Requires 2000+ ATK DARK monster
            bool isLairActive = Bot.HasInSpellZone(CardId.LairOfDarkness);
            bool hasTribute = Bot.GetMonsters().Any(c => c.IsFaceup() && (c.HasAttribute(CardAttribute.Dark) || isLairActive) && c.Attack >= 2000) ||
                             (isLairActive && Enemy.GetMonsters().Any(c => c.IsFaceup() && c.Attack >= 2000));

            return hasTribute;
        }

        private bool FullForceVirusActivate()
        {
            // Requires 2000+ DEF DARK monster (Lilith: 2000 DEF, Malice: 2000 DEF, Diabolos: 2000 DEF)
            bool isLairActive = Bot.HasInSpellZone(CardId.LairOfDarkness);
            bool hasTribute = Bot.GetMonsters().Any(c => c.IsFaceup() && (c.HasAttribute(CardAttribute.Dark) || isLairActive) && c.Defense >= 2000) ||
                             (isLairActive && Enemy.GetMonsters().Any(c => c.IsFaceup() && c.Defense >= 2000));

            return hasTribute;
        }

        private bool TrapTrickActivate()
        {
            // Banish 1 Normal Trap -> Set 1 with same name from deck, usable this turn!
            return Duel.Player == 1 || Duel.Phase == DuelPhase.Main2;
        }

        private bool IceDragonsPrisonActivate()
        {
            return Enemy.Graveyard.Any(c => c.HasType(CardType.Monster)) && Enemy.GetMonsterCount() > 0;
        }

        private bool OjamaTrioActivate()
        {
            int oppFreeZones = 5 - Enemy.GetMonsterCount();
            if (oppFreeZones < 3) return false;

            return Duel.Player == 1 || Duel.Phase == DuelPhase.Main1;
        }

        private bool LilithSummon()
        {
            return true;
        }

        private bool AhrimaSummon()
        {
            bool isLairActive = Bot.HasInSpellZone(CardId.LairOfDarkness);
            return isLairActive && Enemy.GetMonsterCount() > 0;
        }

        private bool MaliceSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool TyphonSpSummon()
        {
            return Enemy.GetMonsters().Any(c => c.IsFaceup() && c.Attack >= 3000) && Bot.GetMonsterCount() >= 2;
        }

        private bool TyphonActivate()
        {
            var target = Enemy.GetMonsters().OrderByDescending(c => Plugin.ThreatImpl.EvaluateThreatScore(c)).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool AccesscodeTalkerSpSummon()
        {
            int linkRating = Bot.GetMonsters().Where(c => c.IsFaceup()).Sum(c => c.HasType(CardType.Link) ? c.LinkMarker : 1);
            return linkRating >= 4 && (Duel.Turn >= 2 || Duel.Player == 1);
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

        private bool DharcSpSummon()
        {
            return Bot.GetMonsterCount() >= 2 && Enemy.Graveyard.Any(c => c.HasAttribute(CardAttribute.Dark));
        }

        private bool DharcActivate()
        {
            return true;
        }

        private bool WeeWitchSpSummon()
        {
            int darkMonsters = Bot.GetMonsters().Count(c => c.IsFaceup() && (c.HasAttribute(CardAttribute.Dark) || Bot.HasInSpellZone(CardId.LairOfDarkness)));
            return darkMonsters >= 2 && !Bot.HasInMonstersZone(CardId.WeeWitchsApprentice);
        }

        #endregion

        #region Selection Handlers

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // 1. Tribute Cost Under Lair of Darkness: ALWAYS pick opponent's monster first!
            bool isTributeContext = cards.Any(c => c.Controller == 1);
            if (isTributeContext)
            {
                var tributeTarget = Plugin.MaterialImpl.PickTributeTarget(cards);
                if (tributeTarget != null && cards.Contains(tributeTarget))
                {
                    var result = new List<ClientCard> { tributeTarget };
                    if (result.Count < min)
                    {
                        foreach (var c in cards)
                        {
                            if (!result.Contains(c))
                            {
                                result.Add(c);
                                if (result.Count >= min) break;
                            }
                        }
                    }
                    return result;
                }
            }

            // 2. Discard Target
            var discardTarget = Plugin.MaterialImpl.PickDiscardTarget(cards, min);
            if (discardTarget != null && cards.Contains(discardTarget))
            {
                var result = new List<ClientCard> { discardTarget };
                if (result.Count < min)
                {
                    foreach (var c in cards)
                    {
                        if (!result.Contains(c))
                        {
                            result.Add(c);
                            if (result.Count >= min) break;
                        }
                    }
                }
                return result;
            }

            // 3. Special Summon Target
            var ssTarget = Plugin.StrategyImpl.PickSpecialSummonTarget(cards);
            if (ssTarget != null && cards.Contains(ssTarget))
            {
                var result = new List<ClientCard> { ssTarget };
                if (result.Count < min)
                {
                    foreach (var c in cards)
                    {
                        if (!result.Contains(c))
                        {
                            result.Add(c);
                            if (result.Count >= min) break;
                        }
                    }
                }
                return result;
            }

            // 4. Search Target
            var searchTarget = Plugin.StrategyImpl.PickSearchTarget(cards, Card);
            if (searchTarget != null && cards.Contains(searchTarget))
            {
                var result = new List<ClientCard> { searchTarget };
                if (result.Count < min)
                {
                    foreach (var c in cards)
                    {
                        if (!result.Contains(c))
                        {
                            result.Add(c);
                            if (result.Count >= min) break;
                        }
                    }
                }
                return result;
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return base.OnSelectOption(options);

            // For Eradicator Epidemic Virus:
            // Option 0 is Spells, Option 1 is Traps (Declare Spells in meta!)
            if (Card != null && Card.Id == CardId.EradicatorEpidemicVirus)
            {
                // If opponent controls face-up continuous/field spells, or in turn 1/2: choose Spells
                return 0;
            }

            return base.OnSelectOption(options);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.LilithLadyOfLament || cardId == CardId.MaliceLadyOfLament)
                return CardPosition.FaceUpDefence; // 2000 DEF walls

            if (cardId == CardId.TormentToken)
                return CardPosition.FaceUpDefence; // 1000 DEF / 1000 ATK

            return base.OnSelectPosition(cardId, positions);
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            if (card == null) return null;

            // Always activate Darkest Diabolos summon when DARK is tributed
            if (card.Id == CardId.DarkestDiabolos) return true;

            // Always activate Lord of the Heavenly Prison when set card activates
            if (card.Id == CardId.LordOfTheHeavenlyPrison) return true;

            return base.OnSelectEffectYn(card, desc);
        }

        #endregion
    }
}
