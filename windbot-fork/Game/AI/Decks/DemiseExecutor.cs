using YGOSharp.OCGWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ====================================================================================================
    // CARD AUDIT — Demise & Ruin (Ritual Stun, Board Wipe & OTK)
    // | Card Name                     | Type      | OPT? | Cost        | Effect                     | Activate When            | NEVER When           |
    // |-------------------------------|-----------|------|-------------|----------------------------|--------------------------|----------------------|
    // | Demise, Supreme King (Boss)   | Monster   | HOPT | 2000 LP/0   | Wipe all other + burn 200  | Enemy has cards to wipe  | Already wiped/Immune |
    // | Ruin, Supreme Queen (Boss)    | Monster   | No   | None        | Effect protection + 2x Atk | On field / Battle        | Not Ritual Summoned  |
    // | Demise, King of Armageddon    | Monster   | No   | 2000 LP     | Destroy all other cards    | Enemy has cards to wipe  | Already wiped/Immune |
    // | Ruin, Queen of Oblivion       | Monster   | No   | None        | Chain Attack (on battle)   | Destroy monster in battle| Enemy field empty    |
    // | Demise, Agent of Armageddon   | Monster   | HOPT | None        | Pop 1 monster / GY anti-ch | Ritual Summon / Sent GY  | No target            |
    // | Ruin, Angel of Oblivion       | Monster   | HOPT | None        | 2x monster atk / GY anti-at| Ritual Summon / Sent GY  | No target            |
    // | Diviner of the Herald         | Monster   | HOPT | None        | Send Herald from Extra->GY | Normal Summoned          | Extra empty          |
    // | Herald of the Arc Light       | Synchro M | No   | None        | Search any Ritual M / S    | Sent to GY               | No targets in deck   |
    // | Impcantation Chalislime       | Ritual M  | HOPT | Discard 1   | SS Impcantation from Deck  | Hand has discard fodder  | Hand count < 2       |
    // | Impcantation Talismandra      | Monster   | HOPT | Reveal Rit M| SS Candoll + Search Rit M  | Hand has Ritual Monster  | No Rit M in hand     |
    // | Impcantation Candoll          | Monster   | HOPT | Reveal Rit S| SS Talismandra + Search Sp | Hand has Ritual Spell    | No Rit S in hand     |
    // | Ash Blossom & Joyous Spring   | Monster   | HOPT | Discard     | Negate deck search/ss/dump | Opponent searches/draws  | Chain already blocked|
    // | Forbidden Droplet             | QuickSp   | No   | Send cards  | Negate & halve ATK quick   | Opponent has threat mons | No fodder in hand/fld|
    // | Red Reboot                    | Trap/Hand | No   | Half LP/None| Negate opponent trap card  | Opponent activates trap  | Opponent trap = 0    |
    // | Pre-Preparation of Rites      | Spell     | HOPT | None        | Search Ritual Sp + Named M | In Hand (Activable)      | None                 |
    // | Preparation of Rites          | Spell     | No   | None        | Search Lv<=7 Rit M + GY Sp | In Hand (Activable)      | None                 |
    // | End of the World              | RitualSp  | No   | Hand/Field  | Ritual summon King/Queen   | Exact Level 8/10 ready   | Materials != level   |
    // | Cycle of the World            | RitualSp  | HOPT | Field/GY    | Rit summon / GY recycle    | Field materials ready    | Field level < ritual |
    // | Turning of the World          | QuickRitSp| HOPT | Hand Rit    | Rit summon from Hand/Deck  | Hand ritual levels >= 8  | Hand ritual lv < rit |
    // | Breaking of the World         | FieldSp   | HOPT | None        | Level modulate / Draw/Pop  | Main Phase setup         | Already modulated    |
    // | Called by the Grave           | QuickSp   | HOPT | None        | Banish & negate GY monster | Opponent activates GY/HT | No enemy target      |
    // | Number 97: Draglubion         | Xyz M     | HOPT | Detach 1    | SS Numeron Dragon + attach | 2 Level 8s on field      | Already summoned Ace |
    // | Number 100: Numeron Dragon    | Xyz M     | HOPT | Detach 1    | Gains Ranks x 1000 ATK     | Battle Phase OTK (9000+) | Non-combat phase     |
    // | Gustav Max                    | Xyz M     | HOPT | Detach 1    | 2000 Burn Damage           | 2 Level 10s on field     | Enemy LP > 2000 in MP1|
    // | Juggernaut Liebe              | Xyz M     | No   | Detach 1    | 6000 ATK Multi-Attacker    | On top of Gustav Max     | Non-combat phase     |
    // | Number 38: Hope Harbinger     | Xyz M     | OPT  | None        | Negate Spell & attach      | Opponent activates Spell | Going 2nd OTK ready  |
    // | Number 90: Photon Lord        | Xyz M     | OPT  | Detach 1    | Negate monster effect & pop| Opponent activates threat| Going 2nd OTK ready  |
    // | Number 41: Bagooska           | Xyz M     | OPT  | Detach 1    | DEF stall & monster negate | Going 1st with 2 Level 4s| Going 2nd breakboard |
    // | Dyna Mondo                    | Link M    | HOPT | Tribute/GY  | Quick shuffle GY Rit + opp | Opponent turn disruption | No GY Ritual         |
    // | Cross-Sheep                   | Link M    | HOPT | None        | Draw 2, Discard 2 on Rit   | Before Ritual Summon     | No Link materials    |
    // | Accesscode Talker             | Link M    | No   | Banish Link | Gain ATK & destroy cards   | Link-3/2 on field to push| Field already empty  |
    // ====================================================================================================

    [Deck("Demise", "Demise")]
    public class _2026_DemiseExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Ritual Monsters
            public const int DemiseSupremeKingOfArmageddon = 59913418;
            public const int RuinSupremeQueenOfOblivion = 13518809;
            public const int DemiseKingOfArmageddon = 72426662;
            public const int RuinQueenOfOblivion = 46427957;
            public const int DemiseAgentOfArmageddon = 86124104;
            public const int RuinAngelOfOblivion = 50139096;
            public const int ImpcantationChalislime = 65877963;

            // Main Deck Non-Ritual Monsters
            public const int ImpcantationTalismandra = 80701178;
            public const int ImpcantationCandoll = 53303460;
            public const int DivinerOfTheHerald = 92919429;
            public const int AshBlossom = 14558127;

            // Main Deck Spells
            public const int PrePreparationOfRites = 13048472;
            public const int PreparationOfRites = 96729612;
            public const int EndOfTheWorld = 8198712;
            public const int CycleOfTheWorld = 32828635;
            public const int TurningOfTheWorld = 95612049;
            public const int BreakingOfTheWorld = 69217334;
            public const int ForbiddenDroplet = 24299458;
            public const int CalledByTheGrave = 24224830;

            // Main Deck Traps
            public const int RedReboot = 23002292;

            // Extra Deck Monsters
            public const int HeraldOfTheArcLight = 79606837;
            public const int Draglubion = 28400508;
            public const int NumeronDragon = 57314798;
            public const int HopeHarbinger = 63767246;
            public const int PhotonLord = 8165596;
            public const int GustavMax = 56910167;
            public const int Liebe = 26096328;
            public const int Zeus = 90448279;
            public const int Bagooska = 90590303;
            public const int AbyssDweller = 21044178;
            public const int TornadoDragon = 6983839;
            public const int DynaMondo = 73898890;
            public const int CrossSheep = 50277355;
            public const int AccesscodeTalker = 86066372;
        }

        private bool _breakingOfTheWorldUsedThisTurn = false;
        private bool _cycleGyUsedThisTurn = false;
        private int _ritualSummonCountThisTurn = 0;
        private bool _fieldWipeUsedThisTurn = false;

        public _2026_DemiseExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── 1. Register Ace Cards in ResourcePlanner ──
            ResourcePlan.RegisterAceCards(
                CardId.DemiseSupremeKingOfArmageddon,
                CardId.RuinSupremeQueenOfOblivion,
                CardId.DemiseKingOfArmageddon,
                CardId.RuinQueenOfOblivion,
                CardId.NumeronDragon,
                CardId.Draglubion,
                CardId.Liebe,
                CardId.GustavMax,
                CardId.HopeHarbinger,
                CardId.PhotonLord,
                CardId.Zeus,
                CardId.Bagooska,
                CardId.AccesscodeTalker,
                CardId.DynaMondo
            );

            // ── 2. Register Starters and Baits in BaitPlanner ──
            BaitPlanner.RegisterComboStarters(
                CardId.PrePreparationOfRites,
                CardId.PreparationOfRites,
                CardId.DivinerOfTheHerald,
                CardId.ImpcantationChalislime,
                CardId.ImpcantationCandoll,
                CardId.ImpcantationTalismandra
            );
            BaitPlanner.RegisterBaitCards(
                CardId.BreakingOfTheWorld,
                CardId.PreparationOfRites
            );

            // ── 3. Register Combo Lines ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Demise-Supreme-Wipe-OTK",
                RequiredCards = new List<int> { CardId.PrePreparationOfRites },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.PrePreparationOfRites, ActionType = ExecutorType.Activate, Description = "Search End of the World + Demise King" },
                    new() { CardId = CardId.EndOfTheWorld, ActionType = ExecutorType.Activate, Description = "Ritual Summon Demise King" },
                    new() { CardId = CardId.DemiseKingOfArmageddon, ActionType = ExecutorType.Activate, Description = "Wipe entire field" }
                },
                FallbackLineName = "Going-1st-Lock"
            });

            // ═══════════════════════════════════════════════════════════════
            //  EXECUTORS PIPELINE (Tiered Priority)
            // ═══════════════════════════════════════════════════════════════

            // ── Tier 0: Quick Negates, Red Reboot & Handtraps ──
            AddExecutor(ExecutorType.Activate, CardId.RedReboot, RedRebootEffect);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.HopeHarbinger, HopeHarbingerEffect);
            AddExecutor(ExecutorType.Activate, CardId.PhotonLord, PhotonLordEffect);
            AddExecutor(ExecutorType.Activate, CardId.AbyssDweller, AbyssDwellerEffect);
            AddExecutor(ExecutorType.Activate, CardId.TornadoDragon, TornadoDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.DynaMondo, DynaMondoEffect);
            AddExecutor(ExecutorType.Activate, CardId.Zeus, ZeusEffect);

            // ── Tier 1: Searchers & Impcantations (Play BEFORE Normal Summon to bait negates) ──
            AddExecutor(ExecutorType.Activate, CardId.PrePreparationOfRites, PrePreparationOfRitesEffect);
            AddExecutor(ExecutorType.Activate, CardId.PreparationOfRites, PreparationOfRitesEffect);
            AddExecutor(ExecutorType.Activate, CardId.ImpcantationChalislime, ImpcantationChalislimeEffect);
            AddExecutor(ExecutorType.Activate, CardId.ImpcantationCandoll, ImpcantationCandollEffect);
            AddExecutor(ExecutorType.Activate, CardId.ImpcantationTalismandra, ImpcantationTalismandraEffect);
            AddExecutor(ExecutorType.Activate, CardId.BreakingOfTheWorld, BreakingOfTheWorldEffect);

            // ── Tier 1.5: Normal Summon Searchers (Diviner) ──
            AddExecutor(ExecutorType.Summon, CardId.DivinerOfTheHerald, DivinerSummon);
            AddExecutor(ExecutorType.Activate, CardId.DivinerOfTheHerald, DivinerEffect);
            AddExecutor(ExecutorType.Activate, CardId.HeraldOfTheArcLight, HeraldOfTheArcLightEffect);

            // ── Tier 2: Ritual Spells & Ritual Summoning ──
            AddExecutor(ExecutorType.Activate, CardId.EndOfTheWorld, EndOfTheWorldEffect);
            AddExecutor(ExecutorType.Activate, CardId.CycleOfTheWorld, CycleOfTheWorldEffect);
            AddExecutor(ExecutorType.Activate, CardId.TurningOfTheWorld, TurningOfTheWorldEffect);

            // ── Tier 2.5: Ritual Monster Trigger Effects ──
            AddExecutor(ExecutorType.Activate, CardId.DemiseAgentOfArmageddon, DemiseAgentEffect);
            AddExecutor(ExecutorType.Activate, CardId.RuinAngelOfOblivion, RuinAngelEffect);
            AddExecutor(ExecutorType.Activate, CardId.RuinSupremeQueenOfOblivion, RuinSupremeQueenEffect);
            AddExecutor(ExecutorType.Activate, CardId.RuinQueenOfOblivion, RuinQueenEffect);

            // ── Tier 3: Board Wipe Activations (Supreme King / King) ──
            AddExecutor(ExecutorType.Activate, CardId.DemiseSupremeKingOfArmageddon, DemiseSupremeKingEffect);
            AddExecutor(ExecutorType.Activate, CardId.DemiseKingOfArmageddon, DemiseKingEffect);

            // ── Tier 4: Extra Deck Xyz & Link Extensions (Only when no Impcantations on field) ──
            AddExecutor(ExecutorType.SpSummon, CardId.GustavMax, GustavMaxSummon);
            AddExecutor(ExecutorType.Activate, CardId.GustavMax, GustavMaxEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Liebe, LiebeSummon);
            AddExecutor(ExecutorType.Activate, CardId.Liebe, LiebeEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.Draglubion, DraglubionSummon);
            AddExecutor(ExecutorType.Activate, CardId.Draglubion, DraglubionEffect);
            AddExecutor(ExecutorType.Activate, CardId.NumeronDragon, NumeronDragonEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.HopeHarbinger, HopeHarbingerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.PhotonLord, PhotonLordSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Bagooska, BagooskaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AbyssDweller, AbyssDwellerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TornadoDragon, TornadoDragonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Zeus, ZeusSummon);

            AddExecutor(ExecutorType.SpSummon, CardId.DynaMondo, DynaMondoSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrossSheep, CrossSheepSummon);
            AddExecutor(ExecutorType.Activate, CardId.CrossSheep, CrossSheepEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, AccesscodeTalkerSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker, AccesscodeTalkerEffect);

            // ── Tier 5: Direct Special Summon Calls ──
            AddExecutor(ExecutorType.SpSummon, CardId.DemiseSupremeKingOfArmageddon, RitualSummonGuard);
            AddExecutor(ExecutorType.SpSummon, CardId.RuinSupremeQueenOfOblivion, RitualSummonGuard);
            AddExecutor(ExecutorType.SpSummon, CardId.DemiseKingOfArmageddon, RitualSummonGuard);
            AddExecutor(ExecutorType.SpSummon, CardId.RuinQueenOfOblivion, RitualSummonGuard);
            AddExecutor(ExecutorType.SpSummon, CardId.DemiseAgentOfArmageddon);
            AddExecutor(ExecutorType.SpSummon, CardId.RuinAngelOfOblivion);

            // ── Tier 6: Spell/Trap Set ──
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.TurningOfTheWorld, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.RedReboot, DefaultSpellSet);

            // ── Tier 7: Monster Reposition ──
            AddExecutor(ExecutorType.Repos, MonsterReposOverride);
        }

        public override bool OnSelectHand()
        {
            return false; // Favor going second for board wipe & OTK
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _breakingOfTheWorldUsedThisTurn = false;
            _cycleGyUsedThisTurn = false;
            _ritualSummonCountThisTurn = 0;
            _fieldWipeUsedThisTurn = false;
        }

        public override void OnChaining(int player, ClientCard card)
        {
            base.OnChaining(player, card);
            if (player == 0 && card != null)
            {
                if (card.Id == CardId.BreakingOfTheWorld)
                    _breakingOfTheWorldUsedThisTurn = true;
            }
        }

        // ═══════════════════════════════════════════════════════════════
        //  ACE CARDS & MATERIAL PRIORITY
        // ═══════════════════════════════════════════════════════════════

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.DemiseSupremeKingOfArmageddon
                || card.Id == CardId.RuinSupremeQueenOfOblivion
                || card.Id == CardId.DemiseKingOfArmageddon 
                || card.Id == CardId.RuinQueenOfOblivion
                || card.Id == CardId.NumeronDragon
                || card.Id == CardId.Draglubion
                || card.Id == CardId.Liebe
                || card.Id == CardId.GustavMax
                || card.Id == CardId.HopeHarbinger
                || card.Id == CardId.PhotonLord
                || card.Id == CardId.Zeus
                || card.Id == CardId.Bagooska
                || card.Id == CardId.AccesscodeTalker;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900; // Strictly protect Aces
            if (c.Id == CardId.ImpcantationTalismandra || c.Id == CardId.ImpcantationCandoll) return 5; // Best tribute (unlocks Extra Deck)
            if (c.Id == CardId.DemiseAgentOfArmageddon || c.Id == CardId.RuinAngelOfOblivion) return 10; // Triggers GY effects
            if (c.Id == CardId.DivinerOfTheHerald) return 30; // Searcher on field
            return base.GetMaterialPriority(c);
        }

        // ═══════════════════════════════════════════════════════════════
        //  HANDTRAPS, FORBIDDEN DROPLET & RED REBOOT
        // ═══════════════════════════════════════════════════════════════

        private bool RedRebootEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                var chainCard = LastChainCard;
                if (chainCard != null && chainCard.HasType(CardType.Trap))
                    return true;
            }
            return false;
        }

        private bool ForbiddenDropletEffect()
        {
            if (Duel.Player == 0)
            {
                var oppMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && IsViableEffectTarget(c)).ToList();
                if (oppMonsters.Count > 0)
                {
                    int sendable = Bot.Hand.Count(c => c != null && c.Id != CardId.ForbiddenDroplet)
                                 + Bot.GetMonsters().Count(c => c != null && c.IsFaceup());
                    return sendable >= 1;
                }
            }
            else if (Duel.Player == 1)
            {
                if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep)
                {
                    var oppMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && c.IsAttack() && !c.IsDisabled()).ToList();
                    return oppMonsters.Count > 0;
                }
                if (Duel.LastChainPlayer == 1)
                {
                    var chainCard = LastChainCard;
                    if (chainCard != null && chainCard.IsMonster() && chainCard.Location == CardLocation.MonsterZone)
                        return true;
                }
            }
            return false;
        }

        private bool AshBlossomEffect()
        {
            if (Duel.Player == 0) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool CalledByTheGraveEffect()
        {
            if (Duel.Player == 0 && Duel.LastChainPlayer == 1)
            {
                return DefaultCalledByTheGrave();
            }
            if (Duel.Player == 1)
            {
                var targets = Enemy.Graveyard.Where(c => c != null && c.IsMonster()).ToList();
                if (targets.Any(c => c.Id == 1561110 || c.Id == 30012506 || c.Id == 77411244 || c.Id == 3405259 || // ABC
                                     c.Id == 71039903 || c.Id == 79814787 || c.Id == 45467446 || c.Id == 89631139 || // Blue-Eyes
                                     c.Id == 46986414 || c.Id == 7084129  || c.Id == 30603688))                       // Dark Magician
                {
                    return DefaultCalledByTheGrave();
                }
            }
            return DefaultCalledByTheGrave();
        }

        private bool HopeHarbingerEffect() => Duel.LastChainPlayer == 1;
        private bool PhotonLordEffect() => Duel.LastChainPlayer == 1;

        private bool AbyssDwellerEffect()
        {
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Draw || Duel.Phase == DuelPhase.Standby || Duel.Phase == DuelPhase.Main1))
            {
                return true;
            }
            return false;
        }

        private bool TornadoDragonEffect()
        {
            if (Enemy.GetSpellCount() == 0) return false;
            var target = Enemy.GetSpells().FirstOrDefault(c => c != null && (c.IsFaceup() || c.IsFacedown()));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool DynaMondoEffect()
        {
            if (Duel.Player == 1)
            {
                bool hasRitualInGrave = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasType(CardType.Ritual));
                bool enemyHasCards = Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
                if (hasRitualInGrave && enemyHasCards)
                {
                    return true;
                }
            }
            return false;
        }

        private bool ZeusEffect()
        {
            return Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2;
        }

        // ═══════════════════════════════════════════════════════════════
        //  SEARCHERS & RITUAL SUMMONING
        // ═══════════════════════════════════════════════════════════════

        private bool PrePreparationOfRitesEffect() => true;
        private bool PreparationOfRitesEffect() => true;

        // ── IMPCANTATION ENGINES ──
        private bool ImpcantationChalislimeEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return Bot.Hand.Count >= 2;
            }
            return false;
        }

        private bool ImpcantationCandollEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return Bot.Hand.Any(c => c != null && (c.Id == CardId.EndOfTheWorld || c.Id == CardId.CycleOfTheWorld || c.Id == CardId.TurningOfTheWorld));
            }
            return true;
        }

        private bool ImpcantationTalismandraEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return Bot.Hand.Any(c => c != null && c.IsMonster() && c.HasType(CardType.Ritual));
            }
            return true;
        }

        // ── DIVINER OF THE HERALD ──
        private bool DivinerSummon()
        {
            bool hasKingOnField = Bot.HasInMonstersZone(CardId.DemiseKingOfArmageddon) || Bot.HasInMonstersZone(CardId.DemiseSupremeKingOfArmageddon);
            bool enemyHasCards = Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
            bool canWipe = Bot.LifePoints > 2000 && !_fieldWipeUsedThisTurn;
            if (hasKingOnField && enemyHasCards && canWipe) return false;
            return true;
        }

        private bool DivinerEffect()
        {
            AI.SelectCard(CardId.HeraldOfTheArcLight);
            return true;
        }

        private bool HeraldOfTheArcLightEffect()
        {
            bool hasSpell = Bot.Hand.Any(c => c != null && (c.Id == CardId.CycleOfTheWorld || c.Id == CardId.EndOfTheWorld || c.Id == CardId.TurningOfTheWorld || c.Id == CardId.PrePreparationOfRites));
            bool hasBoss = Bot.Hand.Any(c => c != null && (c.Id == CardId.DemiseSupremeKingOfArmageddon || c.Id == CardId.DemiseKingOfArmageddon || c.Id == CardId.RuinSupremeQueenOfOblivion || c.Id == CardId.RuinQueenOfOblivion));

            if (!hasSpell && hasBoss)
            {
                AI.SelectCard(CardId.EndOfTheWorld, CardId.CycleOfTheWorld, CardId.PrePreparationOfRites);
            }
            else if (hasSpell && !hasBoss)
            {
                AI.SelectCard(CardId.DemiseSupremeKingOfArmageddon, CardId.DemiseKingOfArmageddon, CardId.RuinSupremeQueenOfOblivion);
            }
            else
            {
                AI.SelectCard(CardId.DemiseSupremeKingOfArmageddon, CardId.DemiseAgentOfArmageddon, CardId.EndOfTheWorld);
            }
            return true;
        }

        private bool BreakingOfTheWorldEffect()
        {
            if (_breakingOfTheWorldUsedThisTurn) return false;

            if (Bot.HasInSpellZone(CardId.BreakingOfTheWorld))
            {
                if (Duel.CurrentChain.Count > 0 || Duel.Player != 0 || 
                    (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2))
                {
                    return true;
                }

                var rituals = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && 
                    (c.Id == CardId.DemiseAgentOfArmageddon || c.Id == CardId.RuinAngelOfOblivion)).ToList();
                
                bool hasHighRitualInHand = Bot.Hand.Any(c => c != null && (c.Id == CardId.DemiseSupremeKingOfArmageddon ||
                                                                            c.Id == CardId.RuinSupremeQueenOfOblivion ||
                                                                            c.Id == CardId.DemiseKingOfArmageddon || 
                                                                            c.Id == CardId.RuinQueenOfOblivion));
                if (rituals.Count > 0 && hasHighRitualInHand)
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  RITUAL SUMMON SPELLS & WIPES
        // ═══════════════════════════════════════════════════════════════

        private bool DemiseSupremeKingEffect()
        {
            if (_fieldWipeUsedThisTurn) return false;
            if (Bot.LifePoints <= 2000) return false;

            int oppCount = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            if (oppCount == 0) return false;

            bool onlyUnaffected = Enemy.GetMonsters().All(c => c != null && c.IsFaceup() && (c.Id == 86221741 || c.IsShouldNotBeTarget()));
            if (onlyUnaffected && Enemy.GetSpellCount() == 0) return false;

            _fieldWipeUsedThisTurn = true;
            return true;
        }

        private bool DemiseKingEffect()
        {
            if (_fieldWipeUsedThisTurn) return false;
            if (Bot.LifePoints <= 2000) return false;

            int oppCount = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            if (oppCount == 0) return false;

            bool onlyUnaffected = Enemy.GetMonsters().All(c => c != null && c.IsFaceup() && (c.Id == 86221741 || c.IsShouldNotBeTarget()));
            if (onlyUnaffected && Enemy.GetSpellCount() == 0) return false;

            _fieldWipeUsedThisTurn = true;
            return true;
        }

        private bool EndOfTheWorldEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            return CanEndOfTheWorldSummon();
        }

        private bool CycleOfTheWorldEffect()
        {
            if (IsSpecialSummonBlocked()) return false;

            if (Card.Location == CardLocation.Grave)
            {
                if (_cycleGyUsedThisTurn) return false;
                _cycleGyUsedThisTurn = true;
                return true;
            }

            return CanCycleOfTheWorldSummon();
        }

        private bool TurningOfTheWorldEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Duel.Player == 0 && Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            return true;
        }

        private bool CanEndOfTheWorldSummon()
        {
            int[] targets = { CardId.DemiseSupremeKingOfArmageddon, CardId.RuinSupremeQueenOfOblivion, CardId.DemiseKingOfArmageddon, CardId.RuinQueenOfOblivion, CardId.DemiseAgentOfArmageddon, CardId.RuinAngelOfOblivion };
            foreach (int targetId in targets)
            {
                int reqLevel = (targetId == CardId.DemiseSupremeKingOfArmageddon || targetId == CardId.RuinSupremeQueenOfOblivion) ? 10 :
                               (targetId == CardId.DemiseKingOfArmageddon || targetId == CardId.RuinQueenOfOblivion) ? 8 : 4;
                if (CheckExactMaterialsForTarget(targetId, reqLevel))
                    return true;
            }
            return false;
        }

        private bool CanCycleOfTheWorldSummon()
        {
            int[] targets = { CardId.DemiseSupremeKingOfArmageddon, CardId.RuinSupremeQueenOfOblivion, CardId.DemiseKingOfArmageddon, CardId.RuinQueenOfOblivion, CardId.DemiseAgentOfArmageddon, CardId.RuinAngelOfOblivion };
            int fieldLevel = Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Sum(m => m.Level);
            foreach (int targetId in targets)
            {
                if (Bot.Hand.Any(c => c != null && c.Id == targetId))
                {
                    int reqLevel = (targetId == CardId.DemiseSupremeKingOfArmageddon || targetId == CardId.RuinSupremeQueenOfOblivion) ? 10 :
                                   (targetId == CardId.DemiseKingOfArmageddon || targetId == CardId.RuinQueenOfOblivion) ? 8 : 4;
                    if (fieldLevel >= reqLevel) return true;
                }
            }
            return false;
        }

        private bool CheckExactMaterialsForTarget(int targetId, int targetLevel)
        {
            bool hasTargetInHand = Bot.Hand.Any(c => c != null && c.Id == targetId);
            if (!hasTargetInHand) return false;

            int totalAvailable = 0;
            bool targetSkipped = false;
            foreach (var m in Bot.GetMonsters().Where(m => m != null && m.IsFaceup()))
                totalAvailable += m.Level;
            foreach (var m in Bot.Hand.Where(m => m != null && m.IsMonster()))
            {
                if (m.Id == targetId && !targetSkipped)
                {
                    targetSkipped = true;
                    continue;
                }
                totalAvailable += m.Level;
            }
            return totalAvailable >= targetLevel;
        }

        private bool RitualSummonGuard()
        {
            if (IsSpecialSummonBlocked()) return false;
            _ritualSummonCountThisTurn++;
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TRIGGER & CONTINUOUS EFFECTS
        // ═══════════════════════════════════════════════════════════════

        private bool DemiseAgentEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && IsViableEffectTarget(c));
                if (target != null)
                {
                    AI.SelectCard(target);
                }
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                bool hasRitualOnField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Ritual));
                if (hasRitualOnField)
                {
                    AI.SelectCard(CardId.DemiseSupremeKingOfArmageddon, CardId.DemiseKingOfArmageddon, CardId.RuinSupremeQueenOfOblivion, CardId.RuinQueenOfOblivion);
                    return true;
                }
            }
            return false;
        }

        private bool RuinAngelEffect()
        {
            if (Card.Location == CardLocation.MonsterZone) return true;
            if (Card.Location == CardLocation.Grave)
            {
                bool hasRitualOnField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Ritual));
                if (hasRitualOnField)
                {
                    AI.SelectCard(CardId.DemiseSupremeKingOfArmageddon, CardId.DemiseKingOfArmageddon, CardId.RuinSupremeQueenOfOblivion, CardId.RuinQueenOfOblivion);
                    return true;
                }
            }
            return false;
        }

        private bool RuinSupremeQueenEffect() => true;
        private bool RuinQueenEffect() => true;

        // ═══════════════════════════════════════════════════════════════
        //  EXTRA DECK SUMMONS & TACTICS (Rank 10 Gustav Max, Draglubion, etc.)
        // ═══════════════════════════════════════════════════════════════

        private bool HasImpcantationOnField()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && 
                (c.Id == CardId.ImpcantationCandoll || c.Id == CardId.ImpcantationTalismandra || c.Id == CardId.ImpcantationChalislime));
        }

        // ── RANK 10 GUSTAV MAX & JUGGERNAUT LIEBE ──
        private bool GustavMaxSummon()
        {
            if (IsSpecialSummonBlocked() || HasImpcantationOnField()) return false;
            int lv10Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 10);
            if (lv10Count >= 2)
            {
                return Duel.Phase == DuelPhase.Main2 || Enemy.LifePoints <= 2000;
            }
            return false;
        }

        private bool GustavMaxEffect() => true;

        private bool LiebeSummon()
        {
            if (IsSpecialSummonBlocked() || HasImpcantationOnField()) return false;
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.GustavMax);
        }

        private bool LiebeEffect() => true;

        // ── RANK 8 DRAGLUBION & NUMERON DRAGON ──
        private bool DraglubionSummon()
        {
            if (IsSpecialSummonBlocked() || HasImpcantationOnField()) return false;
            int lv8Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 8);
            return lv8Count >= 2;
        }

        private bool DraglubionEffect() => true;
        private bool NumeronDragonEffect() => true;

        private bool HopeHarbingerSummon()
        {
            if (IsSpecialSummonBlocked() || HasImpcantationOnField()) return false;
            if (Duel.Turn == 1 || !_isGoingSecond)
            {
                int lv8Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 8);
                return lv8Count >= 2;
            }
            return false;
        }

        private bool PhotonLordSummon()
        {
            if (IsSpecialSummonBlocked() || HasImpcantationOnField()) return false;
            if (Duel.Turn == 1 || !_isGoingSecond)
            {
                int lv8Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 8);
                return lv8Count >= 2;
            }
            return false;
        }

        private bool BagooskaSummon()
        {
            if (IsSpecialSummonBlocked() || HasImpcantationOnField()) return false;
            if (Duel.Turn == 1 || !_isGoingSecond)
            {
                int lv4Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 4 && !IsAceCard(c));
                return lv4Count >= 2;
            }
            return false;
        }

        private bool AbyssDwellerSummon()
        {
            if (IsSpecialSummonBlocked() || HasImpcantationOnField()) return false;
            int lv4Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 4 && !IsAceCard(c));
            return lv4Count >= 2;
        }

        private bool TornadoDragonSummon()
        {
            if (IsSpecialSummonBlocked() || HasImpcantationOnField()) return false;
            if (Enemy.GetSpellCount() > 0)
            {
                int lv4Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 4 && !IsAceCard(c));
                return lv4Count >= 2;
            }
            return false;
        }

        private bool ZeusSummon()
        {
            if (IsSpecialSummonBlocked() || HasImpcantationOnField()) return false;
            return Duel.Phase == DuelPhase.Main2;
        }

        private bool CrossSheepSummon()
        {
            if (IsSpecialSummonBlocked() || HasImpcantationOnField()) return false;
            bool hasRitualReady = Bot.Hand.Any(c => c != null && (c.Id == CardId.EndOfTheWorld || c.Id == CardId.CycleOfTheWorld || c.Id == CardId.PrePreparationOfRites));
            if (hasRitualReady)
            {
                var monsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
                if (monsters.Count >= 2 && monsters.Select(m => m.Id).Distinct().Count() >= 2)
                    return true;
            }
            return false;
        }

        private bool CrossSheepEffect() => true;

        private bool DynaMondoSummon()
        {
            if (IsSpecialSummonBlocked() || HasImpcantationOnField()) return false;
            var monsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            bool hasRitual = monsters.Any(m => m.HasType(CardType.Ritual));
            return monsters.Count >= 2 && hasRitual && (Duel.Turn == 1 || !_isGoingSecond);
        }

        private bool AccesscodeTalkerSummon()
        {
            if (IsSpecialSummonBlocked() || HasImpcantationOnField()) return false;
            int totalLink = Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).Sum(c => c.HasType(CardType.Link) ? c.LinkMarker : 1);
            return totalLink >= 4;
        }

        private bool AccesscodeTalkerEffect() => true;

        // ═══════════════════════════════════════════════════════════════
        //  TACTICAL DECISION OVERRIDES (OnSelectCard / Options / Position)
        // ═══════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // 0. Evenly Matched resolution protection
            if (LastChainCard != null && LastChainCard.IsCode(15693423))
            {
                return cards.OrderByDescending(c => IsAceCard(c) ? 100 : (c.IsMonster() ? c.Attack : 0))
                            .Take(max)
                            .ToList();
            }

            // 0.5 Forbidden Droplet resolution
            if (LastChainCard != null && LastChainCard.Id == CardId.ForbiddenDroplet)
            {
                if (hint == 501 || hint == 504 || hint == 500)
                {
                    return cards.OrderBy(c => IsAceCard(c) ? 100 : (c.Id == CardId.ImpcantationTalismandra ? 1 : c.Id == CardId.ImpcantationCandoll ? 2 : c.Id == CardId.CycleOfTheWorld ? 3 : 10))
                                .Take(min)
                                .ToList();
                }
                if (hint == 575 || hint == 507 || hint == 506)
                {
                    return cards.Where(c => c != null && c.Controller == 1 && c.IsFaceup())
                                .OrderByDescending(c => c.Attack)
                                .Take(max)
                                .ToList();
                }
            }

            // 0.6 Dyna Mondo resolution
            if (LastChainCard != null && LastChainCard.Id == CardId.DynaMondo)
            {
                if (cards.Any(c => c.Location == CardLocation.Grave && c.Controller == 0))
                {
                    return SelectPreferredCard(cards, min, max, CardId.DemiseSupremeKingOfArmageddon, CardId.DemiseKingOfArmageddon, CardId.RuinSupremeQueenOfOblivion);
                }
                if (cards.Any(c => c.Controller == 1))
                {
                    return cards.Where(c => c != null && c.Controller == 1)
                                .OrderByDescending(c => c.IsMonster() ? c.Attack : 1000)
                                .Take(max)
                                .ToList();
                }
            }

            // 1. Draglubion Effect Selection: Summon Numeron Dragon (57314798) & Attach Hope Harbinger (63767246)
            if (LastChainCard != null && LastChainCard.Id == CardId.Draglubion)
            {
                if (cards.Any(c => c.Id == CardId.NumeronDragon))
                {
                    return SelectPreferredCard(cards, min, max, CardId.NumeronDragon);
                }
                if (cards.Any(c => c.Id == CardId.HopeHarbinger || c.Id == CardId.PhotonLord))
                {
                    return SelectPreferredCard(cards, min, max, CardId.HopeHarbinger, CardId.PhotonLord);
                }
            }

            // 2. Pre-Preparation of Rites Resolution: End of the World + Demise King
            if (LastChainCard != null && LastChainCard.Id == CardId.PrePreparationOfRites)
            {
                return SelectPreferredCard(cards, min, max, CardId.EndOfTheWorld, CardId.DemiseKingOfArmageddon, CardId.RuinQueenOfOblivion);
            }

            // 3. Preparation of Rites Resolution: Chalislime / Demise Agent + Ritual Spell in GY
            if (LastChainCard != null && LastChainCard.Id == CardId.PreparationOfRites)
            {
                bool hasBoss = Bot.Hand.Any(c => c != null && (c.Id == CardId.DemiseSupremeKingOfArmageddon || c.Id == CardId.DemiseKingOfArmageddon || c.Id == CardId.RuinSupremeQueenOfOblivion || c.Id == CardId.RuinQueenOfOblivion));
                if (hasBoss)
                {
                    return SelectPreferredCard(cards, min, max, CardId.DemiseAgentOfArmageddon, CardId.RuinAngelOfOblivion, CardId.ImpcantationChalislime, CardId.EndOfTheWorld, CardId.CycleOfTheWorld);
                }
                else
                {
                    return SelectPreferredCard(cards, min, max, CardId.ImpcantationChalislime, CardId.DemiseAgentOfArmageddon, CardId.RuinAngelOfOblivion, CardId.EndOfTheWorld, CardId.CycleOfTheWorld);
                }
            }

            // 4. Diviner of the Herald -> Send Herald of the Arc Light to GY
            if (LastChainCard != null && LastChainCard.Id == CardId.DivinerOfTheHerald)
            {
                return SelectPreferredCard(cards, min, max, CardId.HeraldOfTheArcLight);
            }

            // 5. Impcantation Chalislime discard fodder & summon selection
            if (LastChainCard != null && LastChainCard.Id == CardId.ImpcantationChalislime)
            {
                if (hint == 501 || hint == 504)
                {
                    return cards.OrderBy(c => IsAceCard(c) ? 100 : (c.Id == CardId.ImpcantationTalismandra ? 1 : c.Id == CardId.ImpcantationCandoll ? 2 : c.Id == CardId.CycleOfTheWorld ? 3 : 10))
                                .Take(max)
                                .ToList();
                }
                if (hint == 509)
                {
                    bool hasSpell = Bot.Hand.Any(c => c != null && (c.Id == CardId.EndOfTheWorld || c.Id == CardId.CycleOfTheWorld || c.Id == CardId.TurningOfTheWorld || c.Id == CardId.PrePreparationOfRites));
                    if (!hasSpell)
                        return SelectPreferredCard(cards, min, max, CardId.ImpcantationCandoll, CardId.ImpcantationTalismandra);
                    else
                        return SelectPreferredCard(cards, min, max, CardId.ImpcantationTalismandra, CardId.ImpcantationCandoll);
                }
            }

            // 6. Impcantation Candoll Resolution
            if (LastChainCard != null && LastChainCard.Id == CardId.ImpcantationCandoll)
            {
                if (hint == 526)
                {
                    return SelectPreferredCard(cards, min, max, CardId.EndOfTheWorld, CardId.CycleOfTheWorld, CardId.TurningOfTheWorld);
                }
                if (hint == 509)
                {
                    return SelectPreferredCard(cards, min, max, CardId.ImpcantationTalismandra, CardId.ImpcantationChalislime);
                }
                if (hint == 506)
                {
                    return SelectPreferredCard(cards, min, max, CardId.EndOfTheWorld, CardId.CycleOfTheWorld, CardId.TurningOfTheWorld);
                }
            }

            // 7. Impcantation Talismandra Resolution
            if (LastChainCard != null && LastChainCard.Id == CardId.ImpcantationTalismandra)
            {
                if (hint == 526)
                {
                    return SelectPreferredCard(cards, min, max, CardId.ImpcantationChalislime, CardId.DemiseAgentOfArmageddon, CardId.RuinAngelOfOblivion, CardId.DemiseSupremeKingOfArmageddon, CardId.DemiseKingOfArmageddon);
                }
                if (hint == 509)
                {
                    return SelectPreferredCard(cards, min, max, CardId.ImpcantationCandoll, CardId.ImpcantationChalislime);
                }
                if (hint == 506)
                {
                    return SelectPreferredCard(cards, min, max, CardId.DemiseSupremeKingOfArmageddon, CardId.DemiseKingOfArmageddon, CardId.RuinSupremeQueenOfOblivion, CardId.RuinQueenOfOblivion);
                }
            }

            // 8. Ritual Summon Target Selection (Hint 509): ALWAYS PRIORITIZE BOSSES (Supreme King / King)
            if (hint == 509)
            {
                if (cards.Any(c => c.Id == CardId.DemiseSupremeKingOfArmageddon || c.Id == CardId.DemiseKingOfArmageddon || c.Id == CardId.RuinSupremeQueenOfOblivion || c.Id == CardId.RuinQueenOfOblivion))
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.DemiseSupremeKingOfArmageddon,
                        CardId.DemiseKingOfArmageddon,
                        CardId.RuinSupremeQueenOfOblivion,
                        CardId.RuinQueenOfOblivion,
                        CardId.DemiseAgentOfArmageddon,
                        CardId.RuinAngelOfOblivion);
                }
            }

            // 9. Ritual Materials Selection Prioritizer (Hints 500, 503, 511, 512, 513, 533)
            if (hint == 500 || hint == 503 || hint == 511 || hint == 512 || hint == 513 || hint == 533)
            {
                var sortedCards = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                var selected = new List<ClientCard>();

                foreach (var c in sortedCards)
                {
                    if (IsAceCard(c) && selected.Count >= min)
                        continue;

                    selected.Add(c);
                    if (selected.Count >= max) break;
                }

                if (selected.Count < min)
                {
                    foreach (var c in sortedCards)
                    {
                        if (!selected.Contains(c))
                        {
                            selected.Add(c);
                            if (selected.Count >= min) break;
                        }
                    }
                }
                return selected;
            }

            // 10. Cycle of the World Graveyard effect
            if (LastChainCard != null && LastChainCard.Id == CardId.CycleOfTheWorld)
            {
                bool hasDeckcards = cards.Any(c => c != null && c.Location == CardLocation.Deck);
                if (hasDeckcards)
                {
                    return SelectPreferredCard(cards, min, max, CardId.EndOfTheWorld);
                }
                bool hasGYcards = cards.Any(c => c != null && c.Location == CardLocation.Grave);
                if (hasGYcards)
                {
                    return SelectPreferredCard(cards, min, max, CardId.DemiseSupremeKingOfArmageddon, CardId.DemiseKingOfArmageddon, CardId.RuinSupremeQueenOfOblivion, CardId.RuinQueenOfOblivion);
                }
            }

            // 11. Turning of the World target selection
            if (LastChainCard != null && LastChainCard.Id == CardId.TurningOfTheWorld)
            {
                return SelectPreferredCard(cards, min, max, CardId.DemiseSupremeKingOfArmageddon, CardId.DemiseKingOfArmageddon, CardId.RuinSupremeQueenOfOblivion, CardId.RuinQueenOfOblivion);
            }

            // 12. Demise Agent of Armageddon targeting
            if (LastChainCard != null && LastChainCard.Id == CardId.DemiseAgentOfArmageddon)
            {
                bool isGYEffect = cards.All(c => c != null && c.Controller == 0);
                if (isGYEffect)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.DemiseSupremeKingOfArmageddon, CardId.DemiseKingOfArmageddon,
                        CardId.RuinSupremeQueenOfOblivion, CardId.RuinQueenOfOblivion);
                }

                var oppMonsters = cards.Where(c => c != null && c.Controller == 1 && c.IsFaceup())
                                       .OrderByDescending(c => c.Attack)
                                       .Take(max)
                                       .ToList();
                if (oppMonsters.Count > 0) return oppMonsters;
            }

            // 13. Ruin Angel of Oblivion targeting
            if (LastChainCard != null && LastChainCard.Id == CardId.RuinAngelOfOblivion)
            {
                bool isGYEffect = cards.All(c => c != null && c.Controller == 0);
                if (isGYEffect)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.DemiseSupremeKingOfArmageddon, CardId.DemiseKingOfArmageddon,
                        CardId.RuinSupremeQueenOfOblivion, CardId.RuinQueenOfOblivion);
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (Card != null)
            {
                if (Card.Id == CardId.BreakingOfTheWorld)
                {
                    long drawOpt = Util.GetStringId(CardId.BreakingOfTheWorld, 0);
                    long destroyOpt = Util.GetStringId(CardId.BreakingOfTheWorld, 1);

                    int drawIndex = options.IndexOf(drawOpt);
                    int destroyIndex = options.IndexOf(destroyOpt);

                    if (destroyIndex >= 0 && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0))
                    {
                        return destroyIndex;
                    }
                    if (drawIndex >= 0)
                    {
                        return drawIndex;
                    }
                }
            }
            return base.OnSelectOption(options);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.Bagooska)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }
            if (cardId == CardId.DemiseSupremeKingOfArmageddon || cardId == CardId.DemiseKingOfArmageddon
                || cardId == CardId.RuinSupremeQueenOfOblivion || cardId == CardId.RuinQueenOfOblivion
                || cardId == CardId.NumeronDragon || cardId == CardId.Liebe || cardId == CardId.GustavMax
                || cardId == CardId.AccesscodeTalker)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }
            return base.OnSelectPosition(cardId, positions);
        }

        private IList<ClientCard> SelectPreferredCard(IList<ClientCard> cards, int min, int max, params int[] preferredIds)
        {
            var result = new List<ClientCard>();
            foreach (int id in preferredIds)
            {
                var matches = cards.Where(c => c != null && c.Id == id && !result.Contains(c)).ToList();
                foreach (var m in matches)
                {
                    result.Add(m);
                    if (result.Count >= max) break;
                }
                if (result.Count >= max) break;
            }
            if (result.Count < min)
            {
                foreach (var card in cards)
                {
                    if (card != null && !result.Contains(card))
                    {
                        result.Add(card);
                        if (result.Count >= min) break;
                    }
                }
            }
            return result;
        }

        private bool MonsterReposOverride()
        {
            if (Card == null) return false;
            if (Card.Id == CardId.Bagooska) return false;

            if (IsAceCard(Card))
            {
                if (Card.IsAttack()) return false;
                return true;
            }

            bool enemyEmpty = Enemy.GetMonsterCount() == 0;
            if (Card.IsAttack())
            {
                if (!enemyEmpty && Card.Attack < 1500) return true;
            }
            else
            {
                if (enemyEmpty || Card.Defense < Card.Attack) return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  BATTLE & OTK LOGIC
        // ═══════════════════════════════════════════════════════════════

        public override BattlePhaseAction OnBattle(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            if (attackers == null || attackers.Count == 0)
                return null;

            // 1. Numeron Dragon (9000+ ATK) & Juggernaut Liebe (6000 ATK) OTK swings
            var bossAttacker = attackers.FirstOrDefault(c => c != null && (c.Id == CardId.NumeronDragon || c.Id == CardId.Liebe) && c.IsFaceup() && c.IsAttack() && c.Attack >= 6000);
            if (bossAttacker != null)
            {
                if (defenders == null || defenders.Count == 0)
                    return AI.Attack(bossAttacker, null);
                
                var target = defenders.FirstOrDefault(d => d != null && (d.IsAttack() ? bossAttacker.Attack > d.Attack : bossAttacker.Attack > d.Defense));
                if (target != null)
                    return AI.Attack(bossAttacker, target);
            }

            // 2. Direct attack scenario
            if (defenders == null || defenders.Count == 0)
            {
                var directAttacker = attackers
                    .Where(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack > 0)
                    .OrderBy(c => IsAceCard(c) ? 1 : 0)
                    .ThenBy(c => c.Attack)
                    .FirstOrDefault();

                if (directAttacker != null)
                {
                    return AI.Attack(directAttacker, null);
                }
            }

            // 3. Combat against defenders
            foreach (var attacker in attackers.Where(c => c != null && c.IsFaceup() && c.IsAttack()).OrderByDescending(c => c.Attack))
            {
                foreach (var defender in defenders.Where(d => d != null))
                {
                    if (defender.IsFaceup() && defender.IsAttack() && attacker.Attack > defender.Attack)
                        return AI.Attack(attacker, defender);
                    if (defender.IsFaceup() && defender.IsDefense() && attacker.Attack > defender.Defense)
                        return AI.Attack(attacker, defender);
                    if (defender.IsFacedown() && attacker.Attack >= 2000)
                        return AI.Attack(attacker, defender);
                }
            }

            // 4. Ruin Supreme Queen & Ruin Queen chain attack priority
            var queen = attackers.FirstOrDefault(c => c != null && (c.Id == CardId.RuinSupremeQueenOfOblivion || c.Id == CardId.RuinQueenOfOblivion) && c.IsFaceup() && c.IsAttack());
            if (queen != null)
            {
                var beatable = defenders.FirstOrDefault(d => d != null && d.IsFaceup() && d.IsAttack() && queen.Attack > d.Attack);
                if (beatable != null)
                    return AI.Attack(queen, beatable);
            }

            return null;
        }

        protected override bool IsBoardStrongEnough()
        {
            if (Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && (m.Id == CardId.NumeronDragon || m.Id == CardId.Liebe) && m.Attack >= 6000))
                return true;

            int disruption = Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && (m.Id == CardId.HopeHarbinger || m.Id == CardId.PhotonLord || m.Id == CardId.Bagooska || m.Id == CardId.DynaMondo));
            if (disruption >= 2) return true;

            int atk = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && m.IsAttack()).Sum(m => m.Attack);
            if (atk >= 8000 && Enemy.GetMonsterCount() == 0) return true;

            return false;
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough()) return true;
            return false;
        }
    }
}
