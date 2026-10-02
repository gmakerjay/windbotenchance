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
    // ====================================================================================================
    // CARD AUDIT — FiendsmithSacred (Fiendsmith + Sacred Beasts Hybrid Engine)
    // 100% verified against cards.cdb and FiendsmithSacred.ydk
    // ====================================================================================================
    // | Card Name                               | ID       | Lv/Rk | ATK  | DEF  | Type   | Key Role                                  |
    // |-----------------------------------------|----------|-------|------|------|--------|-------------------------------------------|
    // | Calamity of the Sacred Beasts - Hamon   | 50251045 | 10    | 4000 | 4000 | Thund  | Hand: search SB Spell / Colossus enabler  |
    // | Infinity of the Sacred Beasts - Raviel  | 96345184 | 10    | 4000 | 4000 | Fiend  | Hand: search SB Monster / Boardwipe Quick |
    // | Inferno of the Sacred Beasts - Uria     | 23856331 | 10    | 0    | 0    | Pyro   | Hand: search SB Trap / S/T pop Quick      |
    // | Summoner of the Sacred Beasts           | 22734799 | 8     | 2300 | 3000 | Thund  | Hand/GY SS SB / Colossus Contact Tribute  |
    // | Martyr of the Sacred Beasts             | 59138498 | 1     | 0    | 0    | Fiend  | Place SB Field/Trap / SS 2 Martyrs        |
    // | Fabled Lurrie                           | 97651498 | 1     | 200  | 400  | Fiend  | Discard trigger: Special Summon self      |
    // | Lacrima the Crimson Tears               | 28803166 | 4     | 1200 | 1200 | Fiend  | Normal/SS: Foolish Fiendsmith Engraver    |
    // | Fiendsmith Engraver                     | 60764609 | 6     | 1800 | 2400 | Fiend  | Hand: Search Tract / GY: Revive self      |
    // | Kashtira Fenrir                         | 32909498 | 7     | 2400 | 2400 | Psych  | Inherent SS / Search Fenrir / Banish pop  |
    // | Sacred Beasts Released                  | 38776201 | 0     | 0    | 0    | Spell  | Add 3 SB monsters -> discard 2 (+1 setup) |
    // | Sacred Beasts Thunderclap               | 1259915  | 0     | 0    | 0    | CSpell | Place 2 Thunderclap + Fallen Paradise     |
    // | Fallen Paradise of the Sacred Beasts    | 65861210 | 0     | 0    | 0    | Field  | SS SB from Deck / Draw 2 cards            |
    // | Fiendsmith's Tract                      | 98567237 | 0     | 0    | 0    | Spell  | Search LIGHT Fiend -> discard 1           |
    // | Sacred Beasts Combined Assault          | 50147815 | 0     | 0    | 0    | Trap   | Quick SS SB + Negate & Pop / GY Fusion    |
    // |-----------------------------------------|----------|-------|------|------|--------|-------------------------------------------|
    // | The Chaotic Phantasmal Sacred Beasts    | 7894706  | 10    | 5000 | 5000 | Fusion | 3x Monster Negate Quick + Gain LP         |
    // | Thunder Dragon Colossus                 | 15291624 | 8     | 2600 | 2400 | Fusion | Search Lock Floodgate (Contact Fusion)    |
    // | Varudras, Final Bringer of End Times    | 70636044 | 10    | 3000 | 2000 | Xyz    | Rank 10 Omni-Negate & Destroy (Priority #1)|
    // | D/D/D Wave High King Caesar             | 79559912 | 6     | 2800 | 1800 | Xyz    | Rank 6 Special Summon Negate x2           |
    // | Fiendsmith's Desirae                    | 82135803 | 9     | 2800 | 2400 | Fusion | Face-up Cards Negate Quick Effect         |
    // | Fiendsmith's Lacrima                    | 46640168 | 6     | 2400 | 2400 | Fusion | Revive LIGHT Fiend / Burn 1200 on GY      |
    // | Fiendsmith's Agnumday                   | 32991300 | 3     | 1800 | Link | Link-3 | Quick Revive LIGHT Fiend non-link & Equip |
    // | Fiendsmith's Sequence                   | 49867899 | 2     | 1200 | Link | Link-2 | GY Shuffle Fusion Summon Fiend Boss       |
    // | Fiendsmith's Requiem                    | 2463794  | 1     | 600  | Link | Link-1 | Tribute: SS Fiendsmith / Equip from GY    |
    // | Moon of the Closed Heaven               | 71818935 | 2     | 1200 | Link | Link-2 | 2 Effect monsters -> LIGHT Fiend Bridge   |
    // | Relinquished Anima                      | 94259633 | 1     | 0    | Link | Link-1 | Lv1 -> Steal opposing face-up monster    |
    // | Gorgon of Zilofthonia                   | 12067160 | 3     | 100  | Link | Link-3 | Negate activated effects in pointed zones |
    // | I:P Masquerena                          | 65741786 | 2     | 800  | Link | Link-2 | Quick Link into S:P Little Knight         |
    // | S:P Little Knight                       | 29301450 | 2     | 1600 | Link | Link-2 | Banish 1 on summon / Quick Banish 2       |
    // ====================================================================================================

    [Deck("FiendsmithSacred", "FiendsmithSacred", "Modern")]
    [Deck("Fiendsmith Sacred", "FiendsmithSacred", "Modern")]
    [Deck("2026_FiendsmithSacred", "FiendsmithSacred", "Modern")]
    [Deck("Fiendsmith_Sacred", "FiendsmithSacred", "Modern")]
    public class FiendsmithSacredExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Deck Sacred Beasts Engine
            public const int CalamityHamon = 50251045;
            public const int InfinityRaviel = 96345184;
            public const int InfernoUria = 23856331;
            public const int SummonerOfTheSacredBeasts = 22734799;
            public const int MartyrOfTheSacredBeasts = 59138498;
            public const int SacredBeastsReleased = 38776201;
            public const int SacredBeastsThunderclap = 1259915;
            public const int FallenParadiseOfTheSacredBeasts = 65861210;
            public const int SacredBeastsCombinedAssault = 50147815;

            // Main Deck Fiendsmith Engine
            public const int FiendsmithEngraver = 60764609;
            public const int FiendsmithsTract = 98567237;
            public const int LacrimaTheCrimsonTears = 28803166;
            public const int FabledLurrie = 97651498;

            // Main Deck Staples & Handtraps
            public const int KashtiraFenrir = 32909498;
            public const int AshBlossom = 14558128;
            public const int AshBlossomAlt = 14558127;
            public const int GhostOgre = 59438930;
            public const int GhostBelle = 73642296;
            public const int DrollAndLockBird = 94145021;
            public const int TripleTacticsThrust = 35269904;
            public const int CalledByTheGrave = 24224830;
            public const int InfiniteImpermanence = 10045474;

            // Extra Deck Bosses & Links
            public const int TheChaoticPhantasmalSacredBeasts = 7894706;
            public const int ThunderDragonColossus = 15291624;
            public const int NecroquipPrincess = 93860227;
            public const int FiendsmithsDesirae = 82135803;
            public const int FiendsmithsLacrima = 46640168;
            public const int Varudras = 70636044;
            public const int DDDWaveHighKingCaesar = 79559912;
            public const int FiendsmithsAgnumday = 32991300;
            public const int FiendsmithsSequence = 49867899;
            public const int FiendsmithsRequiem = 2463794;
            public const int MoonOfTheClosedHeaven = 71818935;
            public const int RelinquishedAnima = 94259633;
            public const int GorgonOfZilofthonia = 12067160;
            public const int IPMasquerena = 65741786;
            public const int SPLittleKnight = 29301450;
        }

        private readonly FiendsmithSacredPlugin _plugin;

        public FiendsmithSacredExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            _plugin = new FiendsmithSacredPlugin(this);

            // ── 1. Register Ace Cards in ResourcePlanner ──
            ResourcePlan.RegisterAceCards(
                CardId.DDDWaveHighKingCaesar,
                CardId.Varudras,
                CardId.TheChaoticPhantasmalSacredBeasts,
                CardId.ThunderDragonColossus,
                CardId.FiendsmithsDesirae,
                CardId.SPLittleKnight,
                CardId.IPMasquerena,
                CardId.CalamityHamon,
                CardId.InfinityRaviel
            );

            // ── 2. Register Starters and Baits in BaitPlanner ──
            BaitPlanner.RegisterComboStarters(
                CardId.FiendsmithEngraver,
                CardId.FiendsmithsTract,
                CardId.LacrimaTheCrimsonTears,
                CardId.MartyrOfTheSacredBeasts,
                CardId.SacredBeastsReleased,
                CardId.CalamityHamon,
                CardId.InfinityRaviel,
                CardId.KashtiraFenrir
            );

            BaitPlanner.RegisterBaitCards(
                CardId.KashtiraFenrir,
                CardId.TripleTacticsThrust,
                CardId.SacredBeastsThunderclap
            );

            // Reset turn state on Phase/Turn changes
            AddExecutor(ExecutorType.SpSummon, ResetTurnStateCheck);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 0: QUICK NEGATES, HANDTRAPS & INTERRUPTIONS (HIGHEST PRIORITY)
            // ═══════════════════════════════════════════════════════════════
            // Preemptive Impermanence on Turn 2
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultPreemptiveImpermanence);

            // Varudras Rank 10: Omni-Negate & Destroy + Pop
            AddExecutor(ExecutorType.Activate, CardId.Varudras, ShouldVarudrasActivate);

            // D/D/D Wave High King Caesar: Negate Special Summon x2
            AddExecutor(ExecutorType.Activate, CardId.DDDWaveHighKingCaesar, ShouldCaesarActivate);

            // The Chaotic Phantasmal Sacred Beasts: 3x Monster Negate Quick + Gain LP
            AddExecutor(ExecutorType.Activate, CardId.TheChaoticPhantasmalSacredBeasts, ShouldChaoticPhantasmalActivate);

            // Fiendsmith's Desirae: Quick Negate Face-up Cards
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsDesirae, ShouldDesiraeActivate);

            // S:P Little Knight: Quick Banish 2 face-up monsters until EP
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, ShouldSPLittleKnightActivate);

            // I:P Masquerena: Quick Link into S:P Little Knight on opponent's turn
            AddExecutor(ExecutorType.Activate, CardId.IPMasquerena, ShouldIPMasquerenaActivate);

            // Inferno Uria: Quick Destroy 1 Spell/Trap (Spell speed 4!)
            AddExecutor(ExecutorType.Activate, CardId.InfernoUria, ShouldUriaQuickPop);

            // Sacred Beasts Combined Assault: Set Trap activation (SS SB + Negate & Pop)
            AddExecutor(ExecutorType.Activate, CardId.SacredBeastsCombinedAssault, ShouldCombinedAssaultTrapActivate);

            // Handtraps via Smart Interruption
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAlt, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, DefaultGhostBelleAndHauntedMansion);
            AddExecutor(ExecutorType.Activate, CardId.GhostOgre, DefaultGhostOgreAndSnowRabbit);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DefaultDrollAndLockBird);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsThrust, ShouldThrustActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: BOARD BREAKERS & REMOVAL (GOING SECOND)
            // ═══════════════════════════════════════════════════════════════
            // Kashtira Fenrir: Special Summon when no monsters
            AddExecutor(ExecutorType.SpSummon, CardId.KashtiraFenrir, ShouldFenrirSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.KashtiraFenrir, ShouldFenrirActivate);

            // Relinquished Anima: Link-1 using Martyr/Lurrie to steal enemy monster
            AddExecutor(ExecutorType.SpSummon, CardId.RelinquishedAnima, ShouldRelinquishedAnimaSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.RelinquishedAnima, ShouldRelinquishedAnimaActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: SEARCHERS & ENGINE STARTERS
            // ═══════════════════════════════════════════════════════════════
            // Sacred Beasts Released: Add 3 Sacred Beasts -> discard 2 (+1 advantage)
            AddExecutor(ExecutorType.Activate, CardId.SacredBeastsReleased, ShouldSacredBeastsReleasedActivate);

            // Infinity Raviel hand reveal: Add Martyr or Hamon -> discard 1
            AddExecutor(ExecutorType.Activate, CardId.InfinityRaviel, ShouldInfinityRavielHandActivate);

            // Calamity Hamon hand reveal: Add Released, Thunderclap, or Fallen Paradise -> discard 1
            AddExecutor(ExecutorType.Activate, CardId.CalamityHamon, ShouldCalamityHamonHandActivate);

            // Inferno Uria hand reveal: Add Combined Assault -> discard 1
            AddExecutor(ExecutorType.Activate, CardId.InfernoUria, ShouldInfernoUriaHandActivate);

            // Fiendsmith Engraver: Discard -> Add Tract
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithEngraver, ShouldEngraverHandActivate);

            // Fiendsmith's Tract: Add Lurrie/Engraver -> discard Lurrie
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsTract, ShouldTractActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: FIELD & CONTINUOUS SPELLS
            // ═══════════════════════════════════════════════════════════════
            // Sacred Beasts Thunderclap: Place 2 Thunderclap + Fallen Paradise
            AddExecutor(ExecutorType.Activate, CardId.SacredBeastsThunderclap, ShouldThunderclapActivate);

            // Fallen Paradise of the Sacred Beasts:
            // Hand: Place into Field Zone
            // Field: Draw 2 if control Lv10 SB / Send 3 to SS SB from Deck
            AddExecutor(ExecutorType.Activate, CardId.FallenParadiseOfTheSacredBeasts, ShouldFallenParadiseActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: NORMAL SUMMONS
            // ═══════════════════════════════════════════════════════════════
            // 1. Lacrima the Crimson Tears: Send Engraver to GY
            AddExecutor(ExecutorType.Summon, CardId.LacrimaTheCrimsonTears, ShouldLacrimaSummon);
            AddExecutor(ExecutorType.Activate, CardId.LacrimaTheCrimsonTears, ShouldLacrimaActivate);

            // 2. Martyr of the Sacred Beasts: Place Field/Trap + SS 2 Martyrs
            AddExecutor(ExecutorType.Summon, CardId.MartyrOfTheSacredBeasts, ShouldMartyrSummon);
            AddExecutor(ExecutorType.Activate, CardId.MartyrOfTheSacredBeasts, ShouldMartyrActivate);

            // 3. Fabled Lurrie (fallback normal summon if dead in hand)
            AddExecutor(ExecutorType.Summon, CardId.FabledLurrie, ShouldLurrieSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 5: IGNITION, GY TRIGGERS & EXTENDERS
            // ═══════════════════════════════════════════════════════════════
            // Fabled Lurrie discard trigger: SS self!
            AddExecutor(ExecutorType.Activate, CardId.FabledLurrie, ShouldLurrieDiscardTrigger);

            // Summoner of Sacred Beasts: Hand SS / GY Revive
            AddExecutor(ExecutorType.Activate, CardId.SummonerOfTheSacredBeasts, ShouldSummonerActivate);

            // Fiendsmith Engraver: GY revive by shuffling 1 LIGHT Fiend
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithEngraver, ShouldEngraverGYRevive);

            // Fiendsmith Engraver: Field removal (send equip + opp card)
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithEngraver, ShouldEngraverFieldRemoval);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 6: EXTRA DECK SUMMONS & ENGINE BRIDGES
            // ═══════════════════════════════════════════════════════════════
            // 1. Thunder Dragon Colossus: Contact Fusion by tributing Summoner (Thunder)
            AddExecutor(ExecutorType.SpSummon, CardId.ThunderDragonColossus, ShouldColossusSpSummon);

            // 2. Moon of the Closed Heaven: 2 Effect Monsters -> LIGHT Fiend bridge
            AddExecutor(ExecutorType.SpSummon, CardId.MoonOfTheClosedHeaven, ShouldMoonSpSummon);

            // 3. Fiendsmith's Requiem: Link-1 using LIGHT Fiend (Moon, Lurrie, Lacrima)
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsRequiem, ShouldRequiemSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsRequiem, ShouldRequiemActivate);

            // 4. Fiendsmith's Sequence: Link-2 using Engraver + Requiem/Lurrie
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsSequence, ShouldSequenceSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsSequence, ShouldSequenceActivate);

            // 5. Fiendsmith's Lacrima: Fusion trigger -> revive Engraver!
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsLacrima, ShouldFiendsmithLacrimaActivate);

            // 6. D/D/D Wave High King Caesar: Rank 6 Xyz using Engraver + Fiendsmith's Lacrima!
            AddExecutor(ExecutorType.SpSummon, CardId.DDDWaveHighKingCaesar, ShouldCaesarSpSummon);

            // 7. Rank 10: Varudras (Omni-negate Priority #1 for Lv10s)
            AddExecutor(ExecutorType.SpSummon, CardId.Varudras, ShouldVarudrasSpSummon);

            // 8. The Chaotic Phantasmal Sacred Beasts: Contact Fusion with 3 Lv10 non-normal summonables
            AddExecutor(ExecutorType.SpSummon, CardId.TheChaoticPhantasmalSacredBeasts, ShouldChaoticPhantasmalSpSummon);

            // 9. Fiendsmith's Agnumday & Desirae
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsAgnumday, ShouldAgnumdaySpSummon);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsAgnumday, ShouldAgnumdayActivate);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsTract, ShouldTractGYFusionActivate);

            // 10. I:P Masquerena & S:P Little Knight
            AddExecutor(ExecutorType.SpSummon, CardId.IPMasquerena, ShouldIPMasquerenaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, ShouldSPLittleKnightSpSummon);

            // 11. Gorgon of Zilofthonia
            AddExecutor(ExecutorType.SpSummon, CardId.GorgonOfZilofthonia, ShouldGorgonSpSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 7: TRAP SETTING & REPOSITIONS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.SacredBeastsCombinedAssault);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, ShouldImpermanenceSet);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, ShouldCalledBySet);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        private bool ResetTurnStateCheck()
        {
            _plugin.ResetTurnState();
            return false;
        }

        #region Tier 0: Quick Negates & Disruptions
        private bool ShouldVarudrasActivate()
        {
            // Detach 1: Negate any opponent activation & destroy it, then destroy 1 card on field!
            if (Duel.LastChainPlayer == 1)
            {
                _plugin.Strategy.VarudrasUsed = true;
                return true;
            }
            return false;
        }

        private bool ShouldCaesarActivate()
        {
            // Detach 1: Negate a Special Summon or an effect that includes Special Summoning
            if (Duel.LastChainPlayer == 1)
            {
                _plugin.Strategy.CaesarNegatesCount++;
                return true;
            }
            return false;
        }

        private bool ShouldChaoticPhantasmalActivate()
        {
            if (_plugin.Strategy.ChaoticPhantasmalNegateCount >= 3) return false;

            // Up to 3x per turn: Quick negate monster effect & gain LP equal to half ATK
            // Must have a valid face-up opponent effect/dangerous monster
            var dangerousOppMonster = Enemy.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && !m.IsDisabled())
                .OrderByDescending(m => (m.HasType(CardType.Effect) ? 10000 : 0) + m.Attack)
                .FirstOrDefault();

            if (dangerousOppMonster != null)
            {
                AI.SelectCard(dangerousOppMonster);
                _plugin.Strategy.ChaoticPhantasmalNegateCount++;
                return true;
            }
            return false;
        }

        private bool ShouldDesiraeActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_plugin.Strategy.DesiraeUsed) return false;

            // Quick effect: Negate face-up cards on field
            // Must have valid opponent face-up cards to negate
            var enemyTargets = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                .Where(c => c != null && c.IsFaceup() && !c.IsDisabled())
                .OrderByDescending(c => _plugin.ThreatEvaluator.EvaluateTargetPriority(c))
                .ToList();

            if (enemyTargets.Any())
            {
                AI.SelectCard(enemyTargets);
                _plugin.Strategy.DesiraeUsed = true;
                return true;
            }

            return false;
        }

        private bool ShouldSPLittleKnightActivate()
        {
            // On summon banish (ActivateDescription == -1 or trigger)
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Quick banish 2 monsters until End Phase
                if (Duel.LastChainPlayer == 1 || Duel.Phase == DuelPhase.Battle)
                {
                    var oppMonster = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup());
                    if (oppMonster != null)
                    {
                        AI.SelectCard(new[] { Card, oppMonster });
                        return true;
                    }
                }
            }
            return true;
        }

        private bool ShouldIPMasquerenaActivate()
        {
            // Quick Link on opponent's turn into S:P Little Knight
            if (Duel.Player == 1 && Card.Location == CardLocation.MonsterZone)
            {
                return Bot.ExtraDeck.Any(c => c.Id == CardId.SPLittleKnight);
            }
            return false;
        }

        private bool ShouldUriaQuickPop()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            var enemyST = _plugin.ThreatEvaluator.GetBestSpellTrapRemovalTarget();
            if (enemyST != null)
            {
                AI.SelectCard(enemyST);
                _plugin.Strategy.UriaHandUsed = true;
                return true;
            }
            return false;
        }

        private bool ShouldCombinedAssaultTrapActivate()
        {
            if (Card.Location != CardLocation.SpellZone) return false;
            // Special Summon SB monster from Hand or GY in DEF
            bool hasTarget = Bot.Hand.Any(c => FiendsmithSacredStrategy.IsSacredBeastMonster(c)) ||
                             Bot.Graveyard.Any(c => FiendsmithSacredStrategy.IsSacredBeastMonster(c));
            return hasTarget;
        }

        private bool ShouldThrustActivate()
        {
            return Duel.LastChainPlayer == 1 || Enemy.GetMonsterCount() > 0;
        }
        #endregion

        #region Tier 1: Board Breakers
        private bool ShouldFenrirSpSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool ShouldFenrirActivate()
        {
            // Search Fenrir on summon or banish face-down on battle/effect
            return true;
        }

        private bool ShouldRelinquishedAnimaSpSummon()
        {
            if (Duel.Turn == 1) return false;
            // Check if there is an opponent monster in front of EMZ
            return Enemy.GetMonsterCount() > 0 &&
                   Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && (m.Id == CardId.MartyrOfTheSacredBeasts || m.Id == CardId.FabledLurrie));
        }

        private bool ShouldRelinquishedAnimaActivate()
        {
            return true;
        }
        #endregion

        #region Tier 2: Searchers & Starters
        private bool ShouldSacredBeastsReleasedActivate()
        {
            if (DefaultCheckWhetherCardIsNegated(Card)) return false;
            if (_plugin.Strategy.ReleasedUsed) return false;

            // Hand activation: Add 3 SB monsters -> discard 2
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                _plugin.Strategy.ReleasedUsed = true;
                return true;
            }

            // GY effect: Banish to add Lv10 Pyro/Thunder/Fiend (except turn sent)
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }
            return false;
        }

        private bool ShouldInfinityRavielHandActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_plugin.Strategy.RavielHandUsed) return false;
            _plugin.Strategy.RavielHandUsed = true;
            return true;
        }

        private bool ShouldCalamityHamonHandActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_plugin.Strategy.HamonHandUsed) return false;
            _plugin.Strategy.HamonHandUsed = true;
            _plugin.Strategy.ThunderActivatedInHandThisTurn = true;
            return true;
        }

        private bool ShouldInfernoUriaHandActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_plugin.Strategy.UriaHandUsed) return false;
            _plugin.Strategy.UriaHandUsed = true;
            return true;
        }

        private bool ShouldEngraverHandActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_plugin.Strategy.EngraverHandUsed) return false;
            _plugin.Strategy.EngraverHandUsed = true;
            return true;
        }

        private bool ShouldTractActivate()
        {
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.SpellZone) return false;
            if (_plugin.Strategy.TractUsed) return false;
            _plugin.Strategy.TractUsed = true;
            return true;
        }
        #endregion

        #region Tier 3: Field & Continuous Spells
        private bool ShouldThunderclapActivate()
        {
            if (DefaultCheckWhetherCardIsNegated(Card)) return false;
            if (_plugin.Strategy.ThunderclapUsed) return false;
            _plugin.Strategy.ThunderclapUsed = true;
            return true;
        }

        private bool ShouldFallenParadiseActivate()
        {
            if (DefaultCheckWhetherCardIsNegated(Card)) return false;

            // Hand: Always place into Field Zone if not present
            if (Card.Location == CardLocation.Hand || ActivateDescription == -1)
            {
                return !Bot.HasInSpellZone(CardId.FallenParadiseOfTheSacredBeasts);
            }

            // Field Zone effect 1: If control Lv10 Sacred Beast, draw 2 cards!
            if (ActivateDescription == Util.GetStringId(CardId.FallenParadiseOfTheSacredBeasts, 1))
            {
                if (_plugin.Strategy.FallenParadiseDrawUsed) return false;
                if (_plugin.Strategy.HasLevel10SacredBeastOnField())
                {
                    _plugin.Strategy.FallenParadiseDrawUsed = true;
                    return true;
                }
                return false;
            }

            // Field Zone effect 0: Send 3 cards to SS Sacred Beast from Deck/GY
            if (ActivateDescription == Util.GetStringId(CardId.FallenParadiseOfTheSacredBeasts, 0))
            {
                if (_plugin.Strategy.FallenParadiseSSCount >= 3) return false;

                // 3+ Spells available (Thunderclap on field + hand spells)
                int availableSpells = Bot.GetSpells().Count(s => s != null && s.IsFaceup() && s != Card) + Bot.Hand.Count(c => c.IsSpell());
                if (availableSpells >= 3)
                {
                    _plugin.Strategy.FallenParadiseSSCount++;
                    return true;
                }

                // 3+ expendable monsters
                int expendables = Bot.GetMonsters().Count(m => m != null && !_plugin.MaterialScorer.IsProtectedBoss(m)) +
                                  Bot.Hand.Count(c => c.IsMonster() && !_plugin.MaterialScorer.IsProtectedBoss(c));
                if (expendables >= 3)
                {
                    _plugin.Strategy.FallenParadiseSSCount++;
                    return true;
                }
                return false;
            }

            // Fallback draw 2 check
            return _plugin.Strategy.HasLevel10SacredBeastOnField() && !_plugin.Strategy.FallenParadiseDrawUsed;
        }
        #endregion

        #region Tier 4: Normal Summons
        private bool ShouldLacrimaSummon()
        {
            return !_plugin.Strategy.LacrimaSummonUsed;
        }

        private bool ShouldLacrimaActivate()
        {
            _plugin.Strategy.LacrimaSummonUsed = true;
            return true;
        }

        private bool ShouldMartyrSummon()
        {
            return true;
        }

        private bool ShouldMartyrActivate()
        {
            _plugin.Strategy.MartyrSummonUsed = true;
            return true;
        }

        private bool ShouldLurrieSummon()
        {
            // Only normal summon Lurrie if we have no other normal summon and need a body for Requiem
            return Bot.GetMonsterCount() == 0 && !Bot.HasInHand(CardId.LacrimaTheCrimsonTears) && !Bot.HasInHand(CardId.MartyrOfTheSacredBeasts);
        }
        #endregion

        #region Tier 5: Ignition & Extenders
        private bool ShouldLurrieDiscardTrigger()
        {
            return Card.Location == CardLocation.Grave;
        }

        private bool ShouldSummonerActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                _plugin.Strategy.ThunderActivatedInHandThisTurn = true;
                return Bot.Hand.Count >= 2;
            }
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.Graveyard.Any(c => FiendsmithSacredStrategy.IsSacredBeastMonster(c) && c.Id != CardId.SummonerOfTheSacredBeasts);
            }
            return false;
        }

        private bool ShouldEngraverGYRevive()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_plugin.Strategy.EngraverGYUsed) return false;

            // Check if there is another LIGHT Fiend in GY to shuffle
            bool hasLightFiend = Bot.Graveyard.Any(c => c != Card && FiendsmithSacredStrategy.IsLightFiend(c));
            if (hasLightFiend)
            {
                _plugin.Strategy.EngraverGYUsed = true;
                return true;
            }
            return false;
        }

        private bool ShouldEngraverFieldRemoval()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // Check if we have an equip card and opponent has target
            bool hasEquip = Bot.GetSpells().Any(s => s != null && s.HasType(CardType.Equip));
            bool hasEnemyTarget = Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
            return hasEquip && hasEnemyTarget;
        }
        #endregion

        #region Tier 6: Extra Deck Summons
        private bool ShouldColossusSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.ThunderDragonColossus)) return false;
            // Condition: A Thunder monster activated its effect in hand this turn
            // AND we control 1 Thunder non-fusion monster (SummonerOfTheSacredBeasts)
            if (!_plugin.Strategy.ThunderActivatedInHandThisTurn) return false;

            var summonerOnField = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Id == CardId.SummonerOfTheSacredBeasts);
            if (summonerOnField != null)
            {
                AI.SelectCard(summonerOnField);
                _plugin.Strategy.ColossusSummonedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool ShouldMoonSpSummon()
        {
            // Summon Moon of the Closed Heaven if we have 2 expendable monsters (e.g. 2 Martyrs) and haven't run Fiendsmith yet
            if (Bot.HasInMonstersZone(CardId.DDDWaveHighKingCaesar)) return false;

            var expendables = Bot.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && !_plugin.MaterialScorer.IsProtectedBoss(m))
                .ToList();

            return expendables.Count >= 2;
        }

        private bool ShouldRequiemSpSummon()
        {
            if (_plugin.Strategy.RequiemTributeUsed) return false;
            // 1 LIGHT Fiend monster (Moon, Lurrie, Lacrima)
            var lightFiend = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && FiendsmithSacredStrategy.IsLightFiend(m) && !m.HasType(CardType.Link));
            if (lightFiend != null)
            {
                AI.SelectCard(lightFiend);
                return true;
            }
            var moon = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Id == CardId.MoonOfTheClosedHeaven);
            if (moon != null)
            {
                AI.SelectCard(moon);
                return true;
            }
            return false;
        }

        private bool ShouldRequiemActivate()
        {
            // Tribute effect: Special Summon Engraver from Deck
            if (Card.Location == CardLocation.MonsterZone)
            {
                _plugin.Strategy.RequiemTributeUsed = true;
                return true;
            }

            // GY effect: Equip to LIGHT Fiend non-link on field
            if (Card.Location == CardLocation.Grave)
            {
                var target = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && FiendsmithSacredStrategy.IsLightFiend(m) && !m.HasType(CardType.Link));
                if (target != null)
                {
                    AI.SelectCard(target);
                    _plugin.Strategy.RequiemEquipUsed = true;
                    return true;
                }
            }
            return true;
        }

        private bool ShouldSequenceSpSummon()
        {
            if (_plugin.Strategy.SequenceFusionUsed) return false;
            // If 2 Level 6 Fiends exist on field and Caesar is not made, let Caesar summon first!
            int lv6Fiends = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 6 && (c.Race & (int)CardRace.Fiend) != 0);
            if (lv6Fiends >= 2 && !Bot.HasInMonstersZone(CardId.DDDWaveHighKingCaesar)) return false;

            // Link-2: 2 monsters including 1 LIGHT Fiend
            var lightFiend = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && FiendsmithSacredStrategy.IsLightFiend(m) && !_plugin.MaterialScorer.IsProtectedBoss(m));
            var otherMonster = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m != lightFiend && !_plugin.MaterialScorer.IsProtectedBoss(m));
            return lightFiend != null && otherMonster != null;
        }

        private bool ShouldSequenceActivate()
        {
            // Main Phase Fusion: Shuffle materials from GY to summon Fiendsmith's Lacrima or Desirae
            if (Card.Location == CardLocation.MonsterZone)
            {
                _plugin.Strategy.SequenceFusionUsed = true;
                return true;
            }
            return false;
        }

        private bool ShouldFiendsmithLacrimaActivate()
        {
            // On Fusion Summon: Target 1 LIGHT Fiend in GY/banish -> Special Summon it (Engraver!)
            _plugin.Strategy.FiendsmithLacrimaUsed = true;
            var engraver = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.FiendsmithEngraver);
            if (engraver != null)
            {
                AI.SelectCard(engraver);
            }
            return true;
        }

        private bool ShouldCaesarSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.DDDWaveHighKingCaesar)) return false;
            // 2 Level 6 Fiend monsters (Engraver + Fiendsmith's Lacrima)
            var lv6Fiends = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && m.Level == 6 && (m.Race & (int)CardRace.Fiend) != 0).ToList();
            return lv6Fiends.Count >= 2;
        }

        private bool ShouldVarudrasSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.Varudras)) return false;
            // 2 Level 10 monsters (Hamon, Raviel, Uria)
            var lv10s = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && m.Level == 10 && m.Id != CardId.TheChaoticPhantasmalSacredBeasts).ToList();
            return lv10s.Count >= 2;
        }

        private bool ShouldChaoticPhantasmalSpSummon()
        {
            // Contact Fusion: Send 3 Lv10 non-normal summonable monsters from field to GY
            var candidates = Bot.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && FiendsmithSacredStrategy.IsLevel10SacredBeast(m) && m.Id != CardId.TheChaoticPhantasmalSacredBeasts)
                .ToList();
            return candidates.Count >= 3;
        }

        private bool ShouldAgnumdaySpSummon()
        {
            return Bot.GetMonsterCount() >= 3 && !Bot.HasInMonstersZone(CardId.DDDWaveHighKingCaesar);
        }

        private bool ShouldAgnumdayActivate()
        {
            _plugin.Strategy.AgnumdayUsed = true;
            return true;
        }

        private bool ShouldTractGYFusionActivate()
        {
            return Card.Location == CardLocation.Grave && Bot.Hand.Concat(Bot.GetMonsters()).Count(c => FiendsmithSacredStrategy.IsLightFiend(c)) >= 2;
        }

        private bool ShouldIPMasquerenaSpSummon()
        {
            if (Duel.Turn != 1) return false;
            if (Bot.HasInMonstersZone(CardId.IPMasquerena)) return false;
            var nonTunerExpendables = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && !_plugin.MaterialScorer.IsProtectedBoss(m)).ToList();
            return nonTunerExpendables.Count >= 2;
        }

        private bool ShouldSPLittleKnightSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.SPLittleKnight)) return false;
            var expendables = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && !_plugin.MaterialScorer.IsProtectedBoss(m)).ToList();
            return expendables.Count >= 2;
        }

        private bool ShouldGorgonSpSummon()
        {
            var expendables = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && !_plugin.MaterialScorer.IsProtectedBoss(m)).ToList();
            return expendables.Count >= 3;
        }
        #endregion

        #region Tier 7: Setting Traps
        private bool ShouldImpermanenceSet()
        {
            return Bot.GetMonsterCount() > 0 && (Duel.Turn == 1 || Duel.Phase == DuelPhase.Main2);
        }

        private bool ShouldCalledBySet()
        {
            return Duel.Turn == 1 || Duel.Phase == DuelPhase.Main2;
        }
        #endregion

        #region Universal OnSelect Handlers
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Hint 500: Release / Tribute
            if (hint == 500)
            {
                var sorted = cards.OrderByDescending(c => _plugin.MaterialScorer.ScoreSendCost(c)).ToList();
                return sorted.Take(min).ToList();
            }

            // Hint 501: Discard
            if (hint == 501)
            {
                var sorted = cards.OrderByDescending(c => _plugin.MaterialScorer.ScoreDiscardMaterial(c)).ToList();
                return sorted.Take(min).ToList();
            }

            // Hint 502: Destroy / Hint 503: Remove / Hint 505: Return to Hand
            if (hint == 502 || hint == 503 || hint == 505)
            {
                var oppCards = cards.Where(c => c.Controller == 1).ToList();
                if (oppCards.Any())
                {
                    var sorted = oppCards.OrderByDescending(c => _plugin.ThreatEvaluator.EvaluateTargetPriority(c)).ToList();
                    return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                }
                if (cancelable)
                {
                    return new List<ClientCard>();
                }
            }

            // Hint 572: Negate / Hint 575: Disable / Hint 550: Target / Hint 551: Effect / Hint 514: Face-up / Hint 556: Opponent
            if (hint == 572 || hint == 575 || hint == 550 || hint == 551 || hint == 514 || hint == 556)
            {
                var oppCards = cards.Where(c => c.Controller == 1).ToList();
                if (oppCards.Any())
                {
                    var sorted = oppCards.OrderByDescending(c => _plugin.ThreatEvaluator.EvaluateTargetPriority(c)).ToList();
                    return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                }
                if (cancelable)
                {
                    return new List<ClientCard>();
                }
            }

            // Hint 504: To Grave (Removal on opponent OR Cost for Fallen Paradise / Tract)
            if (hint == 504)
            {
                var oppCards = cards.Where(c => c.Controller == 1).ToList();
                if (oppCards.Any())
                {
                    var sorted = oppCards.OrderByDescending(c => _plugin.ThreatEvaluator.EvaluateTargetPriority(c)).ToList();
                    return sorted.Take(min).ToList();
                }
                // Our own send cost (Fallen Paradise / Tract): Thunderclap on field has highest score!
                var ownSorted = cards.OrderByDescending(c => _plugin.MaterialScorer.ScoreSendCost(c)).ToList();
                return ownSorted.Take(min).ToList();
            }

            // Hint 506: Add to Hand / Search
            if (hint == 506)
            {
                var selected = new List<ClientCard>();

                // Priority: Tract / Lurrie
                var lurrie = cards.FirstOrDefault(c => c.Id == CardId.FabledLurrie);
                if (lurrie != null && !selected.Contains(lurrie)) selected.Add(lurrie);

                var engraver = cards.FirstOrDefault(c => c.Id == CardId.FiendsmithEngraver);
                if (engraver != null && !selected.Contains(engraver)) selected.Add(engraver);

                var martyr = cards.FirstOrDefault(c => c.Id == CardId.MartyrOfTheSacredBeasts);
                if (martyr != null && !selected.Contains(martyr)) selected.Add(martyr);

                var hamon = cards.FirstOrDefault(c => c.Id == CardId.CalamityHamon);
                if (hamon != null && !selected.Contains(hamon)) selected.Add(hamon);

                var raviel = cards.FirstOrDefault(c => c.Id == CardId.InfinityRaviel);
                if (raviel != null && !selected.Contains(raviel)) selected.Add(raviel);

                var released = cards.FirstOrDefault(c => c.Id == CardId.SacredBeastsReleased);
                if (released != null && !selected.Contains(released)) selected.Add(released);

                var thunderclap = cards.FirstOrDefault(c => c.Id == CardId.SacredBeastsThunderclap);
                if (thunderclap != null && !selected.Contains(thunderclap)) selected.Add(thunderclap);

                var assault = cards.FirstOrDefault(c => c.Id == CardId.SacredBeastsCombinedAssault);
                if (assault != null && !selected.Contains(assault)) selected.Add(assault);

                var fenrir = cards.FirstOrDefault(c => c.Id == CardId.KashtiraFenrir);
                if (fenrir != null && !selected.Contains(fenrir)) selected.Add(fenrir);

                foreach (var c in cards)
                {
                    if (selected.Count >= min) break;
                    if (!selected.Contains(c)) selected.Add(c);
                }
                return selected.Take(min).ToList();
            }

            // Hint 507: Return to Deck (Sequence shuffle)
            if (hint == 507)
            {
                var selected = new List<ClientCard>();
                // Shuffle Requiem, Lurrie, Lacrima first (leave Engraver in GY to revive!)
                var requiem = cards.FirstOrDefault(c => c.Id == CardId.FiendsmithsRequiem);
                if (requiem != null) selected.Add(requiem);

                var lurrie = cards.FirstOrDefault(c => c.Id == CardId.FabledLurrie);
                if (lurrie != null) selected.Add(lurrie);

                var lacrima = cards.FirstOrDefault(c => c.Id == CardId.LacrimaTheCrimsonTears);
                if (lacrima != null) selected.Add(lacrima);

                foreach (var c in cards)
                {
                    if (selected.Count >= min) break;
                    if (!selected.Contains(c)) selected.Add(c);
                }
                return selected.Take(min).ToList();
            }

            // Hint 509: Special Summon
            if (hint == 509)
            {
                var selected = new List<ClientCard>();

                // Requiem tribute target: Lacrima the Crimson Tears triggers to dump Engraver!
                var lacrima = cards.FirstOrDefault(c => c.Id == CardId.LacrimaTheCrimsonTears);
                if (lacrima != null && !_plugin.Strategy.LacrimaSummonUsed)
                {
                    selected.Add(lacrima);
                }

                // Sequence Extra Deck Fusion Target:
                if (!Bot.HasInMonstersZone(CardId.DDDWaveHighKingCaesar))
                {
                    var fusionLacrima = cards.FirstOrDefault(c => c.Id == CardId.FiendsmithsLacrima);
                    if (fusionLacrima != null && !selected.Contains(fusionLacrima)) selected.Add(fusionLacrima);
                }
                var desirae = cards.FirstOrDefault(c => c.Id == CardId.FiendsmithsDesirae);
                if (desirae != null && !selected.Contains(desirae)) selected.Add(desirae);

                // Fallen Paradise / Combined Assault target:
                // If Thunder activated in hand, summon Summoner (to make Colossus!)
                if (_plugin.Strategy.ThunderActivatedInHandThisTurn && !Bot.HasInMonstersZone(CardId.ThunderDragonColossus))
                {
                    var summoner = cards.FirstOrDefault(c => c.Id == CardId.SummonerOfTheSacredBeasts);
                    if (summoner != null && !selected.Contains(summoner)) selected.Add(summoner);
                }

                var hamon = cards.FirstOrDefault(c => c.Id == CardId.CalamityHamon);
                if (hamon != null && !selected.Contains(hamon)) selected.Add(hamon);

                var raviel = cards.FirstOrDefault(c => c.Id == CardId.InfinityRaviel);
                if (raviel != null && !selected.Contains(raviel)) selected.Add(raviel);

                var engraver = cards.FirstOrDefault(c => c.Id == CardId.FiendsmithEngraver);
                if (engraver != null && !selected.Contains(engraver)) selected.Add(engraver);

                var martyr = cards.FirstOrDefault(c => c.Id == CardId.MartyrOfTheSacredBeasts);
                if (martyr != null && !selected.Contains(martyr)) selected.Add(martyr);

                foreach (var c in cards)
                {
                    if (selected.Count >= min) break;
                    if (!selected.Contains(c)) selected.Add(c);
                }
                return selected.Take(min).ToList();
            }

            // Hint 518: Equip
            if (hint == 518)
            {
                var ownMonsters = cards.Where(c => c.Controller == 0 && c.Location == CardLocation.MonsterZone).ToList();
                if (ownMonsters.Any())
                {
                    var desirae = ownMonsters.FirstOrDefault(c => c.Id == CardId.FiendsmithsDesirae);
                    if (desirae != null) return new[] { desirae };

                    var caesar = ownMonsters.FirstOrDefault(c => c.Id == CardId.DDDWaveHighKingCaesar);
                    if (caesar != null) return new[] { caesar };

                    var flacrima = ownMonsters.FirstOrDefault(c => c.Id == CardId.FiendsmithsLacrima);
                    if (flacrima != null) return new[] { flacrima };

                    var engraver = ownMonsters.FirstOrDefault(c => c.Id == CardId.FiendsmithEngraver);
                    if (engraver != null) return new[] { engraver };

                    return ownMonsters.OrderByDescending(c => c.Attack).Take(min).ToList();
                }
            }

            // Hint 513: Xyz Material / 533: Link Material / 511: Fusion Material
            if (hint == 513 || hint == 533 || hint == 511)
            {
                var sorted = cards.OrderByDescending(c => _plugin.MaterialScorer.ScoreTributeOrCostMaterial(c)).ToList();
                return sorted.Take(min).ToList();
            }

            // Universal fallback: Prioritize opponent targets whenever opponent cards are present
            // and the action is not an explicit summon material, cost, search, or discard
            var oppCardsGeneric = cards.Where(c => c.Controller == 1).ToList();
            if (oppCardsGeneric.Any() &&
                hint != 500 && // Release/Tribute
                hint != 501 && // Discard
                hint != 504 && // To Grave (if own cost)
                hint != 506 && // Search
                hint != 507 && // To Deck
                hint != 509 && // SpSummon
                hint != 511 && // Fusion material
                hint != 512 && // Synchro material
                hint != 513 && // Xyz material
                hint != 518 && // Equip
                hint != 533)   // Link material
            {
                var sorted = oppCardsGeneric.OrderByDescending(c => _plugin.ThreatEvaluator.EvaluateTargetPriority(c)).ToList();
                return sorted.Take(Math.Min(max, sorted.Count)).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int OnSelectOption(IList<long> options)
        {
            return base.OnSelectOption(options);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Defensive bosses (Varudras, Caesar, Hamon 4000 DEF, Colossus)
            if (cardId == CardId.CalamityHamon || cardId == CardId.SummonerOfTheSacredBeasts)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }

            // Attackers
            if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;

            return base.OnSelectPosition(cardId, positions);
        }
        #endregion
    }
}
