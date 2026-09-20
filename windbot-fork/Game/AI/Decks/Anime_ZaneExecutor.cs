// ============================================================================
// CARD AUDIT — Anime_Zane (Zane Truesdale / Hell Kaiser — Cyber Dragon & Clockwork Contact)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Cyber Dragon                       | Monster L5   | No   | No    | None    | SS from hand if only opp controls monster     | Only opp controls monster; rank 5 material    | Bot controls monsters                       |
// | Cyber Dragon Core                  | Monster L2   | Yes  | Yes   | Banish  | On NS: Search Cyber S/T; GY: SS Cydra from Dk | Normal Summoned; or in GY when field empty   | Already used effect this turn               |
// | Cyber Dragon Herz                  | Monster L1   | Yes  | Yes   | None    | On GY send: Add Cydra from Deck/GY to hand    | Sent to GY (by Rampage/discard/link)         | No Cydra in deck or GY                      |
// | Cyber Dragon Nachster              | Monster L1   | Yes  | Yes   | Discard | Discard 1 to SS; revive 2100 ATK/DEF Machine  | In hand, have discard fodder & GY target     | No valid GY targets                         |
// | Jizukiru, Star Destroying Kaiju    | Monster L10  | No   | No    | Tribute | Tribute 1 opp monster to SS to opp field      | Opponent has high threat / tower monster     | Opponent has no monsters                   |
// | Therion "King" Regulus             | Monster L8   | Yes  | Yes   | Equip   | SS from hand by equipping Machine; Omni-negate| Have Machine in GY; Quick negate on opp turn | No Machine in GY                            |
// | Clockwork Night                    | Spell Cont   | Yes  | Yes   | Banish  | Field becomes Machine; +500/-1000 ATK; GY srch| Main Phase 1: Turn opp monsters to Machines  | Already face-up                             |
// | Chimeratech Fortress Dragon        | Fusion L8    | No   | No    | Contact | Devour Cydra + ANY Machine on EITHER field    | Opponent controls Machines (Clockwork Night) | Opponent has 0 Machine monsters             |
// | Chimeratech Megafleet Dragon       | Fusion L10   | No   | No    | Contact | Devour Cydra + monster in Extra Monster Zone  | Opponent controls monster in EMZ             | Opponent has no monster in EMZ              |
// | Cyber Dragon Nova                  | Xyz R5       | Yes  | No    | Detach  | Revive Cydra; +2100 ATK on battle; float boss | Main Phase rank climb into Infinity           | Already converted to Infinity               |
// | Cyber Dragon Infinity              | Xyz R6       | Yes  | Yes   | Detach  | Xyz over Nova; absorb face-up mon; Omni-negate| Quick negate card/effect; absorb opp monster | Detach would lose only protection           |
// | Cyber Dragon Sieger                | Link-2       | Yes  | Yes   | None    | Quick: +2100 ATK/DEF to 2100+ Machine         | Damage Step / Battle Phase beatdown push      | Card already has lethal                     |
// | Chimeratech Rampage Dragon         | Fusion L5    | Yes  | No    | Send    | Pop S/T up to materials; dump 2 for 3 attacks | Fusion Summoned; Battle Phase OTK             | No backrow to pop and already 3 attacks     |
// | Constellar Pleiades                | Xyz R5       | Yes  | No    | Detach  | Quick: Target 1 card on field, return to hand | Opponent turn disruption or clearing threat  | No valid opponent targets                   |
// | Cyber Emergency                    | Spell Normal | Yes  | No    | None    | Search Cyber Dragon monster or LIGHT Machine  | Main Phase 1 search Core/Herz/Nachster        | No targets in deck                          |
// | Cyber Repair Plant                 | Spell Normal | Yes  | Yes   | None    | Search LIGHT Machine if Cydra in GY           | Have Cydra in GY; search Regulus/Jizukiru    | No Cydra in GY                              |
// | Cyber Revsystem                    | Spell Normal | No   | No    | None    | SS Cydra from hand/GY with destruction protect| Need extender on field                        | No Cydra in hand or GY                      |
// | Cyberload Fusion                   | Spell Quick  | Yes  | Yes   | Shuffle | Quick Fusion from field/banished into Rampage | Battle Phase OTK push or dodging removal      | No valid banished/field materials           |
// | Overload Fusion                    | Spell Normal | No   | No    | Banish  | Banish materials from field/GY for Rampage    | Need Rampage OTK and have materials in GY     | Materials needed for other plays            |
// | Machine Duplication                | Spell Normal | No   | No    | Target  | Target Core/Herz (treated as Cydra), SS 2 Cydr| Control Core or Herz face-up on field         | No original Cydra in Deck                   |
// | Cybernetic Overflow                | Trap Normal  | Yes  | Yes   | Banish  | Banish Cydra from hand/field/GY; non-tgt pop  | Opponent establishes board; chain disruption  | No Cydras with different levels             |
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("Anime_Zane", "Anime_Zane")]
    public class Anime_ZaneExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int CyberDragon = 70095154;
            public const int CyberDragonCore = 23893227;
            public const int CyberDragonHerz = 56364287;
            public const int CyberDragonNachster = 1142880;
            public const int JizukiruTheStarDestroyingKaiju = 63941210;
            public const int TherionKingRegulus = 10604644;
            public const int AshBlossom = 14558127;
            public const int EffectVeiler = 97268402;
            public const int Nibiru = 27204311;

            // Spells
            public const int CyberEmergency = 60600126;
            public const int ClockworkNight = 84797028;
            public const int CyberRepairPlant = 86686671;
            public const int CyberRevsystem = 33041277;
            public const int CyberloadFusion = 55704856;
            public const int OverloadFusion = 3659803;
            public const int MachineDuplication = 63995093;
            public const int FoolishBurial = 81439173;
            public const int CalledByTheGrave = 24224830;
            public const int TripleTacticsTalent = 25311006;

            // Traps
            public const int InfiniteImpermanence = 10045474;
            public const int CyberneticOverflow = 82428674;

            // Extra Deck
            public const int CyberEndDragon = 1546123;
            public const int ChimeratechRampageDragon = 84058253;
            public const int ChimeratechFortressDragon = 79229522;
            public const int ChimeratechMegafleetDragon = 87116928;
            public const int CyberDragonNova = 58069384;
            public const int CyberDragonInfinity = 10443957;
            public const int CyberDragonSieger = 46724542;
            public const int ConstellarPleiades = 73964868;
            public const int SPLittleKnight = 29301450;
            public const int LynaTheLightCharmer = 9839945;
            public const int RelinquishedAnima = 94259633;
            public const int SalamangreatAlmiraj = 60303245;
        }

        public Anime_ZaneExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterExecutors();
        }

        private void RegisterExecutors()
        {
            // -------------------------------------------------------------
            // 1. High-Priority Negates & Disruption (Chain / Enemy Turn)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomActivate);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceActivate);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, EffectVeilerActivate);
            AddExecutor(ExecutorType.Activate, CardId.Nibiru, NibiruActivate);

            // Boss Quick Negates & Disruptions
            AddExecutor(ExecutorType.Activate, CardId.CyberDragonInfinity, InfinityOmniNegateActivate);
            AddExecutor(ExecutorType.Activate, CardId.TherionKingRegulus, RegulusOmniNegateActivate);
            AddExecutor(ExecutorType.Activate, CardId.ConstellarPleiades, PleiadesBounceActivate);
            AddExecutor(ExecutorType.Activate, CardId.CyberneticOverflow, CyberneticOverflowActivate);
            AddExecutor(ExecutorType.Activate, CardId.CyberDragonSieger, SiegerAtkBoostActivate);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightActivate);

            // Nova Floating into Cyber End Dragon
            AddExecutor(ExecutorType.Activate, CardId.CyberDragonNova, NovaFloatActivate);

            // -------------------------------------------------------------
            // 2. Kaiju & Contact Fusion Board Eating (Break Opponent Board)
            // -------------------------------------------------------------
            // Jizukiru Kaiju tribute
            AddExecutor(ExecutorType.SpSummon, CardId.JizukiruTheStarDestroyingKaiju, KaijuSpecialSummon);

            // Clockwork Night: Field spell / Continuous Spell & GY Banish search
            AddExecutor(ExecutorType.Activate, CardId.ClockworkNight, ClockworkNightActivate);

            // Megafleet Contact Fusion (Devour Extra Monster Zone)
            AddExecutor(ExecutorType.SpSummon, CardId.ChimeratechMegafleetDragon, MegafleetSummon);

            // Fortress Contact Fusion (Devour opponent Machines + Cydra)
            AddExecutor(ExecutorType.SpSummon, CardId.ChimeratechFortressDragon, FortressSummon);

            // -------------------------------------------------------------
            // 3. Searchers & Starters (Main Phase 1)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.CyberEmergency, CyberEmergencyActivate);
            AddExecutor(ExecutorType.Activate, CardId.CyberRepairPlant, CyberRepairPlantActivate);
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurial, FoolishBurialActivate);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentActivate);

            // Inherent Cyber Dragon SS
            AddExecutor(ExecutorType.SpSummon, CardId.CyberDragon, CyberDragonInherentSummon);

            // -------------------------------------------------------------
            // 4. Normal Summons
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Summon, CardId.CyberDragonCore, CoreNormalSummon);
            AddExecutor(ExecutorType.Activate, CardId.CyberDragonCore, CoreActivate);

            AddExecutor(ExecutorType.Summon, CardId.CyberDragonHerz, HerzNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.CyberDragonNachster, NachsterNormalSummon);

            // Machine Duplication on Core or Herz (Checked against remaining Cydras in Deck)
            AddExecutor(ExecutorType.Activate, CardId.MachineDuplication, MachineDuplicationActivate);

            // -------------------------------------------------------------
            // 5. Extenders & GY Triggers
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.SpSummon, CardId.CyberDragonNachster, NachsterSpecialSummon);
            AddExecutor(ExecutorType.Activate, CardId.CyberDragonNachster, NachsterReviveActivate);

            AddExecutor(ExecutorType.SpSummon, CardId.TherionKingRegulus, RegulusSpecialSummon);
            AddExecutor(ExecutorType.Activate, CardId.CyberRevsystem, RevsystemActivate);
            AddExecutor(ExecutorType.Activate, CardId.CyberDragonHerz, HerzGraveActivate);

            // -------------------------------------------------------------
            // 6. Extra Deck Climbing & Boss Assembly
            // -------------------------------------------------------------
            // Almiraj / Anima using Core or Herz
            AddExecutor(ExecutorType.SpSummon, CardId.RelinquishedAnima, RelinquishedAnimaSummon);
            AddExecutor(ExecutorType.Activate, CardId.RelinquishedAnima, RelinquishedAnimaActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.SalamangreatAlmiraj, AlmirajSummon);

            // Xyz Summon Cyber Dragon Nova (2 Level 5 Machines)
            AddExecutor(ExecutorType.SpSummon, CardId.CyberDragonNova, NovaSummon);
            AddExecutor(ExecutorType.Activate, CardId.CyberDragonNova, NovaReviveActivate);

            // Xyz Summon Cyber Dragon Infinity over Nova!
            AddExecutor(ExecutorType.SpSummon, CardId.CyberDragonInfinity, InfinitySummon);
            AddExecutor(ExecutorType.Activate, CardId.CyberDragonInfinity, InfinityAbsorbActivate);

            // Rank 5 Constellar Pleiades (2 Level 5 LIGHT monsters — Quick bounce)
            AddExecutor(ExecutorType.SpSummon, CardId.ConstellarPleiades, PleiadesSummon);

            // Link Summon Cyber Dragon Sieger
            AddExecutor(ExecutorType.SpSummon, CardId.CyberDragonSieger, SiegerSummon);

            // S:P Little Knight & Lyna (With Ace protection safeguards)
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LynaTheLightCharmer, LynaSummon);
            AddExecutor(ExecutorType.Activate, CardId.LynaTheLightCharmer, LynaActivate);

            // Fusion Summon Rampage Dragon (Overload / Cyberload Fusion)
            AddExecutor(ExecutorType.Activate, CardId.OverloadFusion, OverloadFusionActivate);
            AddExecutor(ExecutorType.Activate, CardId.CyberloadFusion, CyberloadFusionActivate);
            AddExecutor(ExecutorType.Activate, CardId.ChimeratechRampageDragon, RampageDragonActivate);

            // -------------------------------------------------------------
            // 7. Backrow Support & Fallbacks
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.SpellSet, SpellSetStrategy);
            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        // =================================================================
        // EXECUTION LOGIC IMPLEMENTATIONS
        // =================================================================

        private bool InfinityOmniNegateActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            // Quick Effect: When a card or effect is activated, detach 1 to negate & destroy
            return Duel.LastChainPlayer == 1;
        }

        private bool InfinityAbsorbActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            // Target 1 face-up Attack Position monster to attach as material (non-destruction removal!)
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.IsAttack() && (m.Attack >= 2000 || AntiFloodgateHelper.NegateMonsterIds.Contains(m.Id)))
                             ?? Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.IsAttack() && !IsTargetImmune(m));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool RegulusOmniNegateActivate()
        {
            return Duel.LastChainPlayer == 1;
        }

        private bool PleiadesSummon()
        {
            // 2 Level 5 LIGHT monsters (e.g. Cyber Dragon + Nova/Cyber Dragon)
            // Make Pleiades if we already have Infinity or need a quick bounce
            int l5LightCount = Bot.GetMonsters().Count(m => m.Level == 5 && m.HasAttribute(CardAttribute.Light));
            return l5LightCount >= 2 && Bot.GetMonsters().Any(m => m.Id == CardId.CyberDragonInfinity);
        }

        private bool PleiadesBounceActivate()
        {
            // Quick Effect: Detach 1 material to target 1 card on the field; return it to the hand
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            if (Duel.LastChainPlayer == 1 || Duel.Player == 1)
            {
                ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && (m.Attack >= 2000 || AntiFloodgateHelper.NegateMonsterIds.Contains(m.Id)) && !IsTargetImmune(m))
                                 ?? Enemy.GetSpells().FirstOrDefault(s => s.IsFaceup() && !IsTargetImmune(s))
                                 ?? Enemy.GetMonsters().FirstOrDefault(m => !IsTargetImmune(m));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool CyberneticOverflowActivate()
        {
            // Banish Cyber Dragons with different levels from hand/field/GY to pop enemy cards
            if ((Enemy.GetMonsterCount() + Enemy.GetSpellCount()) > 0)
            {
                ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && (m.Attack >= 2000 || AntiFloodgateHelper.NegateMonsterIds.Contains(m.Id)))
                                 ?? Enemy.GetSpells().FirstOrDefault(s => s.IsFaceup())
                                 ?? Enemy.GetMonsters().FirstOrDefault()
                                 ?? Enemy.GetSpells().FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool SiegerAtkBoostActivate()
        {
            // Quick Effect in Damage Step or Battle Phase: Target 2100+ ATK Machine, +2100 ATK/DEF
            if (Duel.Phase >= DuelPhase.BattleStart && Duel.Phase <= DuelPhase.Damage)
            {
                ClientCard target = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Attack >= 2100 && m.Id != CardId.CyberDragonSieger)
                                 ?? Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.CyberDragonSieger);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool NovaFloatActivate()
        {
            // When sent to GY by opp effect: SS Cyber End Dragon from Extra Deck!
            AI.SelectCard(CardId.CyberEndDragon);
            return true;
        }

        private bool KaijuSpecialSummon()
        {
            // Tribute opponent monster to SS Jizukiru on their field
            // CRITICAL SAFEGUARD: Only give opponent Jizukiru (3300 ATK) if we can immediately contact fuse it
            // into Chimeratech Fortress Dragon on THIS SAME TURN! Otherwise Jizukiru will attack and kill us!
            if (!Bot.ExtraDeck.Any(c => c.Id == CardId.ChimeratechFortressDragon))
                return false;

            bool hasCydraOnField = Bot.GetMonsters().Any(m => m.IsFaceup() &&
                (m.Id == CardId.CyberDragon || m.Id == CardId.CyberDragonCore || m.Id == CardId.CyberDragonHerz || m.Id == CardId.CyberDragonNachster));

            bool canSummonCydra = Bot.Hand.Any(c => c.Id == CardId.CyberDragonCore || c.Id == CardId.CyberDragonHerz || c.Id == CardId.CyberDragonNachster ||
                (c.Id == CardId.CyberDragon && Bot.GetMonsterCount() == 0));

            if (!hasCydraOnField && !canSummonCydra)
                return false;

            ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && (m.Attack >= 2500 || AntiFloodgateHelper.NegateMonsterIds.Contains(m.Id)))
                              ?? Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ClockworkNightActivate()
        {
            // In Hand: Activate continuous spell (turns all monsters into Machines)
            if (Card.Location == CardLocation.Hand)
            {
                return true;
            }
            // In GY: Banish to search EARTH Machine (Jizukiru Kaiju!)
            if (Card.Location == CardLocation.Grave)
            {
                if (Bot.Hand.Count >= 1 && GetRemainingCount(CardId.JizukiruTheStarDestroyingKaiju) > 0)
                {
                    ClientCard discard = Bot.Hand.FirstOrDefault(c => c.Id == CardId.CyberDragonHerz || c.Id == CardId.CyberDragon)
                                      ?? Bot.Hand.FirstOrDefault();
                    if (discard != null)
                    {
                        AI.SelectCard(discard);
                        AI.SelectNextCard(CardId.JizukiruTheStarDestroyingKaiju);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool MegafleetSummon()
        {
            // Contact Fuse using 1 Cydra + monster in Extra Monster Zone
            ClientCard emzEnemy = Enemy.GetMonsters().FirstOrDefault(m => m.Location == CardLocation.MonsterZone && (m.Sequence == 5 || m.Sequence == 6));
            return emzEnemy != null && Bot.GetMonsters().Any(m => m.Id == CardId.CyberDragon || m.Id == CardId.CyberDragonCore || m.Id == CardId.CyberDragonHerz);
        }

        private bool FortressSummon()
        {
            // Contact Fuse using Cydra + ALL Machines on either field (Clockwork Night makes all opp monsters Machines!)
            bool hasClockwork = Bot.GetSpells().Any(s => s.Id == CardId.ClockworkNight && s.IsFaceup());
            int oppMachineCount = Enemy.GetMonsters().Count(m => m.HasRace(CardRace.Machine) || hasClockwork);
            return oppMachineCount >= 1 && Bot.GetMonsters().Any(m => m.Id == CardId.CyberDragon || m.Id == CardId.CyberDragonCore || m.Id == CardId.CyberDragonHerz);
        }

        private bool CyberEmergencyActivate()
        {
            AI.SelectCard(CardId.CyberDragonCore, CardId.CyberDragonHerz, CardId.CyberDragonNachster, CardId.CyberDragon);
            return true;
        }

        private bool CyberRepairPlantActivate()
        {
            bool hasCydraInGrave = Bot.Graveyard.Any(c => c.Id == CardId.CyberDragon || c.Id == CardId.CyberDragonCore || c.Id == CardId.CyberDragonHerz || c.Id == CardId.CyberDragonSieger);
            if (!hasCydraInGrave) return false;

            // Priority: Regulus (Omni-negate) > Core (starter) > Jizukiru (Kaiju) > Nachster (reviver)
            if (!Bot.Hand.Any(c => c.Id == CardId.TherionKingRegulus) && Bot.Graveyard.Any(c => c.HasRace(CardRace.Machine)))
                AI.SelectCard(CardId.TherionKingRegulus);
            else if (!Bot.Hand.Any(c => c.Id == CardId.CyberDragonCore))
                AI.SelectCard(CardId.CyberDragonCore);
            else if (Enemy.GetMonsterCount() > 0 && !Bot.Hand.Any(c => c.Id == CardId.JizukiruTheStarDestroyingKaiju))
                AI.SelectCard(CardId.JizukiruTheStarDestroyingKaiju);
            else
                AI.SelectCard(CardId.CyberDragonNachster, CardId.CyberDragon);

            return true;
        }

        private bool FoolishBurialActivate()
        {
            AI.SelectCard(CardId.CyberDragonHerz, CardId.CyberDragonCore);
            return true;
        }

        private bool TripleTacticsTalentActivate()
        {
            if (Enemy.GetMonsterCount() > 0 && Duel.Turn > 1)
            {
                AI.SelectOption(1); // Take control of opponent's monster
                ClientCard target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
                if (target != null) AI.SelectCard(target);
                return true;
            }
            AI.SelectOption(0); // Draw 2 cards
            return true;
        }

        private bool CyberDragonInherentSummon()
        {
            return Bot.GetMonsterCount() == 0 && Enemy.GetMonsterCount() > 0;
        }

        private bool CoreNormalSummon()
        {
            return true;
        }

        private bool CoreActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // On NS: Search Cyber S/T
                if (Bot.Deck.Count(c => c.Id == CardId.CyberDragon) >= 2 && Bot.Hand.Any(c => c.Id == CardId.MachineDuplication))
                    AI.SelectCard(CardId.CyberRepairPlant, CardId.CyberneticOverflow);
                else
                    AI.SelectCard(CardId.CyberEmergency, CardId.CyberRepairPlant, CardId.CyberRevsystem, CardId.CyberloadFusion);
                return true;
            }
            if (Card.Location == CardLocation.Grave && Bot.GetMonsterCount() == 0 && Enemy.GetMonsterCount() > 0)
            {
                // Banish from GY to SS Cydra from Deck
                AI.SelectCard(CardId.CyberDragon);
                return true;
            }
            return false;
        }

        private bool HerzNormalSummon()
        {
            return Bot.GetMonsterCount() == 0 && Bot.Hand.Any(c => c.Id == CardId.MachineDuplication);
        }

        private bool NachsterNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool MachineDuplicationActivate()
        {
            // CRITICAL CHECK: Verify at least 1 original Cyber Dragon remains in Deck!
            int cydraInDeck = Bot.Deck.Count(c => c.Id == CardId.CyberDragon);
            if (cydraInDeck < 1) return false;

            // Target Core (400 ATK) or Herz (100 ATK) on field to summon original Cyber Dragons from deck!
            ClientCard target = Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.CyberDragonCore || m.Id == CardId.CyberDragonHerz);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool NachsterSpecialSummon()
        {
            // Discard 1 monster to SS Nachster
            ClientCard discard = Bot.Hand.FirstOrDefault(c => c.Id == CardId.CyberDragonHerz || c.Id == CardId.CyberDragon)
                              ?? Bot.Hand.FirstOrDefault(c => c.Id != CardId.CyberDragonNachster && c.IsMonster());
            if (discard != null)
            {
                AI.SelectCard(discard);
                return true;
            }
            return false;
        }

        private bool NachsterReviveActivate()
        {
            // Revive 2100 ATK/DEF Machine (Infinity, Nova, Cydra, Sieger, Rampage)
            AI.SelectCard(CardId.CyberDragonInfinity, CardId.CyberDragonNova, CardId.CyberDragon, CardId.CyberDragonSieger);
            return true;
        }

        private bool RegulusSpecialSummon()
        {
            // Target Machine in GY to equip and SS Regulus from hand
            ClientCard equipTarget = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.CyberDragon || c.Id == CardId.CyberDragonCore || c.Id == CardId.CyberDragonHerz);
            if (equipTarget != null)
            {
                AI.SelectCard(equipTarget);
                return true;
            }
            return false;
        }

        private bool RevsystemActivate()
        {
            AI.SelectCard(CardId.CyberDragon, CardId.CyberDragonCore);
            return true;
        }

        private bool HerzGraveActivate()
        {
            // Add Cydra from Deck or GY to hand
            AI.SelectCard(CardId.CyberDragon, CardId.CyberDragonCore);
            return true;
        }

        private bool RelinquishedAnimaSummon()
        {
            ClientCard fodder = Bot.GetMonsters().FirstOrDefault(m => m.Level == 1 && (m.Id == CardId.CyberDragonHerz || m.Id == CardId.CyberDragonNachster));
            return fodder != null && Enemy.GetMonsterCount() > 0;
        }

        private bool RelinquishedAnimaActivate()
        {
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool AlmirajSummon()
        {
            // Send Core or Herz to GY to enable Repair Plant or trigger Herz!
            ClientCard fodder = Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.CyberDragonCore || m.Id == CardId.CyberDragonHerz);
            return fodder != null && Bot.GetMonsterCount() == 1 && Bot.Hand.Any(c => c.Id == CardId.CyberRepairPlant || c.Id == CardId.CyberDragonNachster);
        }

        private bool NovaSummon()
        {
            // 2 Level 5 Machine monsters
            int l5Count = Bot.GetMonsters().Count(m => m.Level == 5 && m.HasRace(CardRace.Machine));
            return l5Count >= 2;
        }

        private bool NovaReviveActivate()
        {
            AI.SelectCard(CardId.CyberDragon);
            return true;
        }

        private bool InfinitySummon()
        {
            // Xyz summon over Nova
            return Bot.GetMonsters().Any(m => m.Id == CardId.CyberDragonNova);
        }

        private bool SiegerSummon()
        {
            // Summon Sieger using expendable Cydra / Core / Herz
            return Bot.GetMonsterCount() >= 2 && Bot.GetMonsters().Any(m => m.Id == CardId.CyberDragon || m.Id == CardId.CyberDragonCore);
        }

        private bool SPLittleKnightSummon()
        {
            // Rule 2 Safeguard: NEVER sacrifice Infinity, Nova, Regulus, Sieger, or Rampage to summon S:P Little Knight!
            int expendable = Bot.GetMonsters().Count(m => m.Id != CardId.CyberDragonInfinity &&
                                                          m.Id != CardId.TherionKingRegulus &&
                                                          m.Id != CardId.CyberDragonNova &&
                                                          m.Id != CardId.CyberDragonSieger &&
                                                          m.Id != CardId.ChimeratechRampageDragon);
            return expendable >= 2 && (Enemy.GetMonsterCount() + Enemy.GetSpellCount()) > 0;
        }

        private bool SPLittleKnightActivate()
        {
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard botCard = Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.SPLittleKnight);
                ClientCard oppCard = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup());
                if (botCard != null && oppCard != null)
                {
                    AI.SelectCard(botCard);
                    AI.SelectNextCard(oppCard);
                    return true;
                }
            }
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && (m.Attack >= 2500 || AntiFloodgateHelper.NegateMonsterIds.Contains(m.Id)))
                             ?? Enemy.GetSpells().FirstOrDefault(s => s.IsFaceup())
                             ?? Enemy.GetMonsters().FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool LynaSummon()
        {
            int expendable = Bot.GetMonsters().Count(m => m.Id != CardId.CyberDragonInfinity && m.Id != CardId.TherionKingRegulus);
            return expendable >= 2 && Enemy.Graveyard.Any(c => c.HasAttribute(CardAttribute.Light) && c.IsMonster());
        }

        private bool LynaActivate()
        {
            ClientCard target = Enemy.Graveyard.FirstOrDefault(c => c.HasAttribute(CardAttribute.Light) && c.IsMonster());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool OverloadFusionActivate()
        {
            // Fusion Summon Chimeratech Rampage Dragon using GY materials
            AI.SelectCard(CardId.ChimeratechRampageDragon);
            return true;
        }

        private bool CyberloadFusionActivate()
        {
            AI.SelectCard(CardId.ChimeratechRampageDragon);
            return true;
        }

        private bool RampageDragonActivate()
        {
            // Effect 1: Pop Spells/Traps
            if (Enemy.GetSpellCount() > 0)
            {
                ClientCard target = Enemy.GetSpells().FirstOrDefault(s => s.IsFaceup()) ?? Enemy.GetSpells().FirstOrDefault();
                if (target != null) AI.SelectCard(target);
                return true;
            }
            // Effect 2: Dump up to 2 LIGHT Machines to gain 2 additional attacks (3 attacks total!)
            AI.SelectCard(CardId.CyberDragonHerz, CardId.CyberDragonCore);
            return true;
        }

        // =================================================================
        // UNIVERSAL TACTICS & ANTI-PATTERNS
        // =================================================================

        private bool CalledByTheGraveActivate()
        {
            if (Duel.LastChainPlayer == 1 && Duel.CurrentChain.Count > 0)
            {
                ClientCard target = Enemy.Graveyard.LastOrDefault(c => c.IsMonster());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool AshBlossomActivate()
        {
            return Duel.LastChainPlayer == 1;
        }

        private bool InfiniteImpermanenceActivate()
        {
            if (Duel.LastChainPlayer == 1 || Duel.Player == 1)
            {
                ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled() && (m.Attack >= 1500 || AntiFloodgateHelper.NegateMonsterIds.Contains(m.Id)))
                                 ?? Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool EffectVeilerActivate()
        {
            if (Duel.LastChainPlayer == 1 || Duel.Player == 1)
            {
                ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled() && (m.Attack >= 1500 || AntiFloodgateHelper.NegateMonsterIds.Contains(m.Id)))
                                 ?? Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool NibiruActivate()
        {
            // Only on opponent's turn in Main Phase
            if (Duel.Player != 1 || (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2))
                return false;

            if (Enemy.GetMonsterCount() < 2) return false;

            // Do not tribute our own board if we control active boss monsters, unless opponent has lethal
            bool weHaveBoss = Bot.GetMonsters().Any(m => m.IsFaceup() && (m.Id == CardId.CyberDragonInfinity || m.Id == CardId.TherionKingRegulus || m.Id == CardId.CyberDragonSieger));
            int enemyTotalAtk = Enemy.GetMonsters().Where(m => m.IsFaceup() && m.IsAttack()).Sum(m => m.Attack);
            if (weHaveBoss && enemyTotalAtk < Bot.LifePoints)
            {
                return false;
            }

            AI.SelectPosition(CardPosition.FaceUpDefence);
            return true;
        }

        private bool SpellSetStrategy()
        {
            if (Card.IsTrap() && (Card.Id == CardId.InfiniteImpermanence || Card.Id == CardId.CyberneticOverflow))
            {
                return true;
            }
            if (Card.Id == CardId.CyberloadFusion)
            {
                return true;
            }
            return false;
        }

        private bool RepositionStrategy()
        {
            if (Card.Attack >= 2100 && Card.IsDefense()) return true;
            if (Card.Attack < 1000 && Card.IsAttack()) return true;
            return false;
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Anti-Pattern Rule 1: Isolation of HINTMSG_ATOHAND (506)
            if (hint == 506)
            {
                var preferred = new List<int>
                {
                    CardId.CyberEmergency,
                    CardId.CyberDragonCore,
                    CardId.TherionKingRegulus,
                    CardId.MachineDuplication,
                    CardId.CyberRepairPlant,
                    CardId.CyberDragon,
                    CardId.CyberDragonHerz,
                    CardId.CyberDragonNachster,
                    CardId.CyberloadFusion,
                    CardId.ClockworkNight,
                    CardId.JizukiruTheStarDestroyingKaiju
                };

                var matches = cards.Where(c => preferred.Contains(c.Id))
                                   .OrderBy(c => preferred.IndexOf(c.Id))
                                   .ToList();

                if (matches.Count >= min)
                    return matches.Take(max).ToList();
            }

            // Anti-Pattern Rule 2: Destruction (502) or Banish (504) must target enemy cards (c.Controller == 1)
            if (hint == 502 || hint == 504)
            {
                var enemyTargets = cards.Where(c => c.Controller == 1 && !IsTargetImmune(c)).ToList();
                if (enemyTargets.Count >= min)
                {
                    return enemyTargets.OrderByDescending(c => GetCardThreatScore(c)).Take(max).ToList();
                }
            }

            // Contact Fusion / Material Selection: Devour opponent cards first!
            if (cards.Any(c => c.Controller == 1))
            {
                // If this is Megafleet or Fortress material selection, prioritize enemy monsters
                var enemyMaterials = cards.Where(c => c.Controller == 1).ToList();
                if (enemyMaterials.Count > 0)
                {
                    var ourCydra = cards.Where(c => c.Controller == 0 && (c.Id == CardId.CyberDragon || c.Id == CardId.CyberDragonCore || c.Id == CardId.CyberDragonHerz)).ToList();
                    var combined = enemyMaterials.Concat(ourCydra).ToList();
                    if (combined.Count >= min)
                        return combined.Take(Math.Min(max, combined.Count)).ToList();
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
