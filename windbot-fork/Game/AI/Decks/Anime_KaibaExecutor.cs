// ============================================================================
// CARD AUDIT — Anime_Kaiba (Seto Kaiba's Blue-Eyes Jet & Ultimate Dragon Engine)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Blue-Eyes White Dragon             | Monster L8   | No   | No    | None    | 3000 ATK Core Vanilla Material                | Beatstick / Fusion / Synchro / Xyz Material  | None                                        |
// | Blue-Eyes Alternative White Dragon | Monster L8   | Yes  | Yes   | Reveal  | Reveal BEWD in hand to SS; Pop 1 monster      | Control BEWD in hand; target high threat mon | Bot has lethal and needs to attack with it  |
// | Blue-Eyes Jet Dragon               | Monster L8   | Yes  | Yes   | None    | Infinite revive on destruction; bounce on atk | Card destroyed on field and BEWD in field/GY | No BEWD on field or GY                      |
// | Blue-Eyes Abyss Dragon             | Monster L8   | Yes  | Yes   | None    | On SS: Search Ultimate Fusion; EP search L8   | Special Summoned while BEWD on field/GY       | Already activated this turn                 |
// | Dictator of D.                     | Monster L4   | Yes  | Yes   | Send    | Dump BEWD to SS; discard to revive BEWD fromGY| Have BEWD in deck or GY; need field presence | No BEWD targets                             |
// | Sage with Eyes of Blue             | Monster L1 T | Yes  | Yes   | Target  | On NS: Search L1 LIGHT Tuner (Stone/Veiler)   | Normal Summoned; or send fodder to SS BEWD   | Hand already full of stones and no targets  |
// | The White Stone of Ancients        | Monster L1 T | Yes  | Yes   | Banish  | EP: SS Blue-Eyes from deck; GY recycle BEWD   | Sent to GY; or need BEWD in hand from GY     | No Blue-Eyes in deck or GY                  |
// | The White Stone of Legend          | Monster L1 T | Yes  | No    | None    | On GY send: Add BEWD from Deck to hand        | Sent to GY                                   | No BEWD in deck                             |
// | Ash Blossom & Joyous Spring        | Monster L3 T | Yes  | Yes   | Discard | Handtrap: Negate search / SS from deck / dump | Opponent activates search/dump/SS from deck  | Chain not hostile                           |
// | Effect Veiler                      | Monster L1 T | Yes  | No    | Send    | Handtrap: Target & negate opp face-up monster | Opponent Main Phase, opp monster activates   | Target already disabled                     |
// | Nibiru, the Primal Being           | Monster L11  | Yes  | Yes   | Tribute | Tribute all monsters on field, SS + Token     | Opponent 5+ summons and has high threat      | Bot has established boss board              |
// | The Melody of Awakening Dragon     | Spell Normal | Yes  | No    | Discard | Discard 1, add 2 Dragons (Alt + Jet / BEWD)   | Main Phase search starter                    | Discard would lose key combo piece           |
// | Trade-In                           | Spell Normal | No   | No    | Discard | Discard L8 monster, draw 2 cards              | Have spare L8 (BEWD/Stone-searchable)        | Discarding only copy of boss needed on board|
// | Dragon Shrine                      | Spell Normal | Yes  | Yes   | None    | Send 1 Dragon (BEWD) + 1 more (Stone/Jet)     | Main Phase 1 dump starter                    | No Dragons in deck                          |
// | Foolish Burial                     | Spell Normal | Yes  | No    | None    | Send 1 monster from Deck to GY (Stone/Jet)    | Main Phase 1 setup                           | Deck empty of targets                       |
// | Triple Tactics Talent              | Spell Normal | Yes  | Yes   | None    | Draw 2 / Steal monster / Look at opp hand     | Opponent activated monster effect during MP  | Conditions not met                          |
// | Called by the Grave                | Spell Quick  | Yes  | No    | Target  | Banish monster from opp GY and negate         | Opponent handtrap or key GY trigger activates| Bot's own card with same name               |
// | Ultimate Fusion                    | Spell Quick  | Yes  | Yes   | Shuffle | Quick Fusion using GY/field/hand; pop cards   | Main Phase, have fusion boss materials       | Not enough materials in field/GY/hand       |
// | True Light                         | Trap Cont    | Yes  | Yes   | None    | Protect BEWD from targeting; SS BEWD or Set S | Main Phase, SS BEWD from GY/hand or set spell| Opponent might wipe backrow (self blow-up)  |
// | Infinite Impermanence              | Trap Normal  | Yes  | No    | Target  | Negate opp monster effect; column spell negate| Opponent activates or controls key threat    | Target already disabled                     |
// | Neo Blue-Eyes Ultimate Dragon      | Fusion L12   | Yes  | No    | Send    | 4500 ATK, attacks 3 times; GY protects BEWD  | Need overwhelming lethal damage / beatdown   | Opponent has blanket battle immunity        |
// | Blue-Eyes Tyrant Dragon            | Fusion L8    | Yes  | Yes   | None    | 3400 ATK, unaffected by traps, attacks all    | Opponent controls multiple monsters/traps    | No monsters on opp field                    |
// | Blue-Eyes Twin Burst Dragon        | Fusion L8    | No   | No    | Contact | 3000 ATK, cannot be destroyed by battle, ban  | Contact fuse with 2 BEWD; banish on battle   | Can make Draglubion OTK instead             |
// | Blue-Eyes Spirit Dragon            | Synchro L9   | Yes  | Yes   | Tribute | Stop multi-SS; Quick GY negate; Tag into Synch| Opponent multi-summons, or GY effect triggers| Bot needs beatstick                         |
// | Azure-Eyes Silver Dragon           | Synchro L9   | Yes  | Yes   | None    | Dragons cannot be targeted/destroyed; revive  | Tag out from Spirit Dragon on opp turn / MP2 | No Dragons on field                         |
// | Michael, the Arch-Lightsworn       | Synchro L7   | Yes  | No    | Pay1000 | Banish 1 card on the field                    | Tag out from Spirit Dragon to banish threat  | LP < 1000                                   |
// | Black Rose Moonlight Dragon        | Synchro L7   | Yes  | Yes   | Target  | Bounce 1 SS monster on field                  | Tag out from Spirit Dragon when opp SS mon   | No valid opp target                         |
// | Number 38: Hope Harbinger          | Xyz R8       | Yes  | No    | Detach  | Quick: Negate Spell & attach; redirect atk    | Opponent activates Spell card or effect      | Spell already negated                       |
// | Number 90: Photon Lord             | Xyz R8       | Yes  | Yes   | Detach  | Quick: Negate monster effect                  | Opponent activates monster effect            | Target already disabled                     |
// | Number 97: Draglubion              | Xyz R8       | Yes  | Yes   | Detach  | SS Numeron Dragon with Hope Harbinger attached| Main Phase 1, push for 9000-17000 ATK OTK    | Opponent has unaffected monster / no battle |
// | Number 100: Numeron Dragon         | Xyz R1       | Yes  | No    | Detach  | Gains combined Ranks x 1000 (9000-17000 ATK)  | Battle Phase lethal attack                   | Opponent has battle fader/negate            |
// | Hieratic Seal of Heavenly Spheres  | Link-2       | Yes  | Yes   | Tribute | Non-target bounce 1 face-up; SS Jet from deck | Quick disruption on opp turn / setup         | No tribute fodder                           |
// | Relinquished Anima                 | Link-1       | Yes  | No    | Target  | Equip opp monster pointed to                  | Opponent has monster in column facing EMZ    | No monster in column                        |
// | S:P Little Knight                  | Link-2       | Yes  | Yes   | Banish  | Banish 1 card on field/GY; Quick temp banish  | Opponent key card on field/GY or disruption  | No targets / Main Phase 1 direct attack     |
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
    [Deck("Anime_Kaiba", "Anime_Kaiba")]
    public class Anime_KaibaExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int BlueEyesWhiteDragon = 89631133;
            public const int BlueEyesAlternativeWhiteDragon = 38517737;
            public const int BlueEyesJetDragon = 30576089;
            public const int BlueEyesAbyssDragon = 64202399;
            public const int DictatorOfD = 66961194;
            public const int SageWithEyesOfBlue = 8240199;
            public const int TheWhiteStoneOfAncients = 71039903;
            public const int TheWhiteStoneOfLegend = 79814787;
            public const int AshBlossom = 14558127;
            public const int EffectVeiler = 97268402;
            public const int Nibiru = 27204311;

            // Spells
            public const int TheMelodyOfAwakeningDragon = 48800175;
            public const int TradeIn = 38120068;
            public const int DragonShrine = 41620959;
            public const int FoolishBurial = 81439173;
            public const int TripleTacticsTalent = 25311006;
            public const int CalledByTheGrave = 24224830;
            public const int UltimateFusion = 71143015;

            // Traps
            public const int TrueLight = 62089826;
            public const int InfiniteImpermanence = 10045474;

            // Extra Deck
            public const int NeoBlueEyesUltimateDragon = 56532353;
            public const int BlueEyesTyrantDragon = 11443677;
            public const int BlueEyesTwinBurstDragon = 2129638;
            public const int BlueEyesSpiritDragon = 59822133;
            public const int AzureEyesSilverDragon = 40908371;
            public const int MichaelTheArchLightsworn = 4779823;
            public const int BlackRoseMoonlightDragon = 33698022;
            public const int Number38HopeHarbinger = 63767246;
            public const int Number90PhotonLord = 8165596;
            public const int Number97Draglubion = 28400508;
            public const int Number100NumeronDragon = 57314798;
            public const int HieraticSealOfTheHeavenlySpheres = 24361622;
            public const int RelinquishedAnima = 94259633;
            public const int SPLittleKnight = 29301450;
        }

        public Anime_KaibaExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // 0. Connect Decoupled Domain Plugin Architecture (MANDATORY)
            DeckPlugin = new Anime_KaibaPlugin(this);

            RegisterExecutors();
        }

        public override bool OnSelectHand()
        {
            // True = First Turn (Set up Spirit Dragon / Hope Harbinger / True Light + Jet Dragon)
            return true;
        }

        private void RegisterExecutors()
        {
            // -------------------------------------------------------------
            // 1. High-Priority Negates & Disruptions (Chain / Enemy Turn)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomActivate);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceActivate);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, EffectVeilerActivate);
            AddExecutor(ExecutorType.Activate, CardId.Nibiru, NibiruActivate);

            // Boss Quick Negates & Interruption
            AddExecutor(ExecutorType.Activate, CardId.Number38HopeHarbinger, HopeHarbingerSpellNegate);
            AddExecutor(ExecutorType.Activate, CardId.Number90PhotonLord, PhotonLordMonsterNegate);
            AddExecutor(ExecutorType.Activate, CardId.BlueEyesSpiritDragon, SpiritDragonActivate);
            AddExecutor(ExecutorType.Activate, CardId.HieraticSealOfTheHeavenlySpheres, HeavenlySpheresActivate);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightActivate);

            // Immortal Boss Trigger: Blue-Eyes Jet Dragon
            AddExecutor(ExecutorType.Activate, CardId.BlueEyesJetDragon, JetDragonActivate);

            // Quick-Play Fusion: Ultimate Fusion
            AddExecutor(ExecutorType.Activate, CardId.UltimateFusion, UltimateFusionActivate);

            // Continuous Protection: True Light
            AddExecutor(ExecutorType.Activate, CardId.TrueLight, TrueLightActivate);

            // -------------------------------------------------------------
            // 2. Search & Draw Engines (Main Phase 1)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentActivate);
            AddExecutor(ExecutorType.Activate, CardId.TheMelodyOfAwakeningDragon, MelodyActivate);
            AddExecutor(ExecutorType.Activate, CardId.TradeIn, TradeInActivate);
            AddExecutor(ExecutorType.Activate, CardId.DragonShrine, DragonShrineActivate);
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurial, FoolishBurialActivate);

            // -------------------------------------------------------------
            // 3. Normal Summons
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Summon, CardId.SageWithEyesOfBlue, SageNormalSummon);
            AddExecutor(ExecutorType.Activate, CardId.SageWithEyesOfBlue, SageEffectActivate);
            AddExecutor(ExecutorType.Summon, CardId.TheWhiteStoneOfAncients, StoneNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.TheWhiteStoneOfLegend, StoneNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.DictatorOfD, DictatorNormalSummon);

            // -------------------------------------------------------------
            // 4. Extenders & Field Swarm (Main Phase 1)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.DictatorOfD, DictatorActivate);

            AddExecutor(ExecutorType.SpSummon, CardId.BlueEyesAlternativeWhiteDragon, AlternativeSpecialSummon);
            AddExecutor(ExecutorType.Activate, CardId.BlueEyesAlternativeWhiteDragon, AlternativePopActivate);

            AddExecutor(ExecutorType.Activate, CardId.TheWhiteStoneOfAncients, StoneOfAncientsGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheWhiteStoneOfLegend, StoneOfLegendGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.BlueEyesAbyssDragon, AbyssDragonActivate);

            // -------------------------------------------------------------
            // 5. Extra Deck Climbing & Boss Assembly
            // -------------------------------------------------------------
            // Relinquished Anima (Steal monster facing EMZ with L1 Tuner)
            AddExecutor(ExecutorType.SpSummon, CardId.RelinquishedAnima, RelinquishedAnimaSummon);
            AddExecutor(ExecutorType.Activate, CardId.RelinquishedAnima, RelinquishedAnimaActivate);

            // Rank 8 Xyz Powerhouses (PRIMARY BOSS SUMMON - Do not link away!)
            AddExecutor(ExecutorType.SpSummon, CardId.Number97Draglubion, DraglubionSummon);
            AddExecutor(ExecutorType.Activate, CardId.Number97Draglubion, DraglubionActivate);
            AddExecutor(ExecutorType.Activate, CardId.Number100NumeronDragon, NumeronDragonActivate);

            AddExecutor(ExecutorType.SpSummon, CardId.Number38HopeHarbinger, HopeHarbingerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Number90PhotonLord, PhotonLordSummon);

            // Synchro Bosses: Blue-Eyes Spirit Dragon (Level 9 = L8 BEWD + L1 Tuner)
            AddExecutor(ExecutorType.SpSummon, CardId.BlueEyesSpiritDragon, SpiritDragonSummon);
            AddExecutor(ExecutorType.Activate, CardId.AzureEyesSilverDragon, AzureEyesActivate);
            AddExecutor(ExecutorType.Activate, CardId.MichaelTheArchLightsworn, MichaelActivate);
            AddExecutor(ExecutorType.Activate, CardId.BlackRoseMoonlightDragon, MoonlightDragonActivate);

            // Contact Fusion: Blue-Eyes Twin Burst Dragon (2 BEWD on field)
            AddExecutor(ExecutorType.SpSummon, CardId.BlueEyesTwinBurstDragon, TwinBurstSummon);
            AddExecutor(ExecutorType.Activate, CardId.BlueEyesTwinBurstDragon, TwinBurstBanishActivate);

            // Link-2 Situational Toolboxes (ONLY with low-ATK fodder, NEVER sacrifice Bosses!)
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.HieraticSealOfTheHeavenlySpheres, HeavenlySpheresSummon);

            // Fusion Boss Effects
            AddExecutor(ExecutorType.Activate, CardId.NeoBlueEyesUltimateDragon, NeoBlueEyesEffect);
            AddExecutor(ExecutorType.Activate, CardId.BlueEyesTyrantDragon, TyrantDragonEffect);

            // -------------------------------------------------------------
            // 6. Backrow Support & Fallbacks
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.SpellSet, SpellSetStrategy);
            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        // =================================================================
        // EXECUTION LOGIC IMPLEMENTATIONS
        // =================================================================

        private bool HasBEWDInFieldOrGrave()
        {
            return Bot.GetMonsters().Any(m => m.Id == CardId.BlueEyesWhiteDragon || m.Id == CardId.BlueEyesAlternativeWhiteDragon)
                || Bot.Graveyard.Any(m => m.Id == CardId.BlueEyesWhiteDragon);
        }

        private bool JetDragonActivate()
        {
            if (Card.Location == CardLocation.Grave || Card.Location == CardLocation.Hand)
            {
                return HasBEWDInFieldOrGrave();
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard bestTarget = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled())
                                     ?? Enemy.GetSpells().FirstOrDefault(s => s.IsFaceup())
                                     ?? Enemy.GetMonsters().FirstOrDefault();
                if (bestTarget != null)
                {
                    AI.SelectCard(bestTarget);
                    return true;
                }
            }
            return true;
        }

        private bool SpiritDragonActivate()
        {
            // Effect 1: Negate GY activation (Quick Effect)
            if (Duel.LastChainPlayer == 1 && Duel.CurrentChain.Count > 0)
            {
                ClientCard lastCard = Duel.CurrentChain.Last();
                if (lastCard != null && lastCard.Location == CardLocation.Grave)
                {
                    return true;
                }
            }

            // Effect 2: Tag out if targeted or in danger, or during opponent end phase
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.End || Duel.LastChainPlayer == 1))
            {
                if (Enemy.GetMonsterCount() > 0)
                {
                    AI.SelectCard(CardId.BlackRoseMoonlightDragon, CardId.MichaelTheArchLightsworn, CardId.AzureEyesSilverDragon);
                }
                else
                {
                    AI.SelectCard(CardId.AzureEyesSilverDragon, CardId.BlackRoseMoonlightDragon);
                }
                return true;
            }

            return false;
        }

        private bool AzureEyesActivate()
        {
            AI.SelectCard(CardId.BlueEyesWhiteDragon);
            return true;
        }

        private bool MichaelActivate()
        {
            if (Bot.LifePoints <= 1000) return false;
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

        private bool MoonlightDragonActivate()
        {
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.IsSpecialSummoned);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool HopeHarbingerSpellNegate()
        {
            return Duel.LastChainPlayer == 1;
        }

        private bool PhotonLordMonsterNegate()
        {
            return Duel.LastChainPlayer == 1;
        }

        private bool HeavenlySpheresActivate()
        {
            if (Duel.Player == 1 && Enemy.GetMonsterCount() > 0)
            {
                ClientCard tributeTarget = Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.HieraticSealOfTheHeavenlySpheres || m.Id == CardId.SageWithEyesOfBlue || m.Id == CardId.TheWhiteStoneOfAncients);
                if (tributeTarget != null)
                {
                    AI.SelectCard(tributeTarget);
                    ClientCard enemyTarget = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup()) ?? Enemy.GetSpells().FirstOrDefault(s => s.IsFaceup());
                    if (enemyTarget != null)
                    {
                        AI.SelectNextCard(enemyTarget);
                        return true;
                    }
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(CardId.BlueEyesJetDragon, CardId.BlueEyesAbyssDragon, CardId.BlueEyesWhiteDragon);
                return true;
            }
            return false;
        }

        private bool TrueLightActivate()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                if (Bot.Hand.Any(c => c.Id == CardId.BlueEyesWhiteDragon) || Bot.Graveyard.Any(c => c.Id == CardId.BlueEyesWhiteDragon))
                {
                    AI.SelectOption(0);
                    AI.SelectCard(CardId.BlueEyesWhiteDragon);
                    return true;
                }
                AI.SelectOption(1);
                AI.SelectCard(CardId.UltimateFusion);
                return true;
            }
            return true;
        }

        private bool UltimateFusionActivate()
        {
            int bewdCount = Bot.Graveyard.Count(c => c.Id == CardId.BlueEyesWhiteDragon)
                          + Bot.Hand.Count(c => c.Id == CardId.BlueEyesWhiteDragon)
                          + Bot.GetMonsters().Count(c => c.Id == CardId.BlueEyesWhiteDragon);

            if (bewdCount >= 3)
            {
                AI.SelectCard(CardId.NeoBlueEyesUltimateDragon);
                return true;
            }
            else if (bewdCount >= 2)
            {
                AI.SelectCard(CardId.BlueEyesTwinBurstDragon, CardId.BlueEyesTyrantDragon);
                return true;
            }
            else if (bewdCount >= 1)
            {
                AI.SelectCard(CardId.BlueEyesTyrantDragon);
                return true;
            }
            return false;
        }

        private bool MelodyActivate()
        {
            ClientCard discard = DeckPlugin?.MaterialEvaluator?.PickDiscardTarget(Bot.Hand);
            if (discard == null || discard.Id == CardId.TheMelodyOfAwakeningDragon)
            {
                discard = Bot.Hand.FirstOrDefault(c => c.Id == CardId.TheWhiteStoneOfAncients || c.Id == CardId.TheWhiteStoneOfLegend)
                       ?? Bot.Hand.FirstOrDefault(c => c.Id != CardId.TheMelodyOfAwakeningDragon && c.Id != CardId.DictatorOfD && c.Id != CardId.SageWithEyesOfBlue);
            }

            if (discard != null)
            {
                AI.SelectCard(discard);
                if (!Bot.Hand.Any(c => c.Id == CardId.BlueEyesWhiteDragon))
                {
                    AI.SelectNextCard(CardId.BlueEyesWhiteDragon, CardId.BlueEyesAlternativeWhiteDragon);
                }
                else
                {
                    AI.SelectNextCard(CardId.BlueEyesAlternativeWhiteDragon, CardId.BlueEyesJetDragon, CardId.BlueEyesAbyssDragon);
                }
                return true;
            }
            return false;
        }

        private bool TradeInActivate()
        {
            ClientCard discard = Bot.Hand.FirstOrDefault(c => c.Level == 8 && c.Id != CardId.BlueEyesJetDragon)
                              ?? Bot.Hand.FirstOrDefault(c => c.Level == 8);
            if (discard != null)
            {
                AI.SelectCard(discard);
                return true;
            }
            return false;
        }

        private bool DragonShrineActivate()
        {
            AI.SelectCard(CardId.BlueEyesWhiteDragon);
            AI.SelectNextCard(CardId.TheWhiteStoneOfAncients, CardId.BlueEyesJetDragon, CardId.TheWhiteStoneOfLegend);
            return true;
        }

        private bool FoolishBurialActivate()
        {
            AI.SelectCard(CardId.TheWhiteStoneOfAncients, CardId.BlueEyesJetDragon, CardId.BlueEyesWhiteDragon);
            return true;
        }

        private bool TripleTacticsTalentActivate()
        {
            if (Enemy.GetMonsterCount() > 0 && Duel.Turn > 1)
            {
                AI.SelectOption(1);
                ClientCard target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
                if (target != null) AI.SelectCard(target);
                return true;
            }
            AI.SelectOption(0);
            return true;
        }

        private bool SageNormalSummon()
        {
            return true;
        }

        private bool IsHighValueBoss(ClientCard c)
        {
            if (c == null) return false;
            int cost = DeckPlugin?.MaterialEvaluator?.GetMaterialCost(c) ?? 0;
            return cost >= 60;
        }

        private bool SageEffectActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(CardId.TheWhiteStoneOfAncients, CardId.EffectVeiler);
                return true;
            }
            if (Card.Location == CardLocation.Hand)
            {
                ClientCard target = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && (m.Id == CardId.TheWhiteStoneOfAncients || m.Id == CardId.TheWhiteStoneOfLegend || m.Id == CardId.DictatorOfD || m.Id == CardId.RelinquishedAnima || m.Id == CardId.SageWithEyesOfBlue));
                if (target != null)
                {
                    AI.SelectCard(target);
                    AI.SelectNextCard(CardId.BlueEyesJetDragon, CardId.BlueEyesWhiteDragon, CardId.BlueEyesAbyssDragon);
                    return true;
                }
            }
            return false;
        }

        private bool StoneNormalSummon()
        {
            return Bot.GetMonsterCount() == 0 || Bot.GetMonsters().Any(m => m.Level == 8);
        }

        private bool DictatorNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool DictatorActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                AI.SelectCard(CardId.BlueEyesWhiteDragon);
                return true;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard discard = DeckPlugin?.MaterialEvaluator?.PickDiscardTarget(Bot.Hand)
                                  ?? Bot.Hand.FirstOrDefault(c => c.Id == CardId.TheWhiteStoneOfAncients || c.Id == CardId.TheWhiteStoneOfLegend || c.Id == CardId.BlueEyesWhiteDragon);
                if (discard != null && Bot.Graveyard.Any(c => c.Id == CardId.BlueEyesWhiteDragon || c.Id == CardId.BlueEyesJetDragon || c.Id == CardId.BlueEyesAbyssDragon))
                {
                    AI.SelectCard(discard);
                    AI.SelectNextCard(CardId.BlueEyesJetDragon, CardId.BlueEyesWhiteDragon, CardId.BlueEyesAbyssDragon);
                    return true;
                }
            }
            return false;
        }

        private bool AlternativeSpecialSummon()
        {
            AI.SelectCard(CardId.BlueEyesWhiteDragon);
            return true;
        }

        private bool AlternativePopActivate()
        {
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && (m.Attack >= 2000 || AntiFloodgateHelper.NegateMonsterIds.Contains(m.Id)))
                             ?? Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup())
                             ?? Enemy.GetMonsters().FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool StoneOfAncientsGraveEffect()
        {
            if (Duel.Phase == DuelPhase.End)
            {
                AI.SelectCard(CardId.BlueEyesJetDragon, CardId.BlueEyesAbyssDragon, CardId.BlueEyesWhiteDragon);
                return true;
            }
            if (Bot.Graveyard.Any(c => c.Id == CardId.BlueEyesWhiteDragon) && Bot.Hand.Count <= 2)
            {
                AI.SelectCard(CardId.BlueEyesWhiteDragon, CardId.BlueEyesAlternativeWhiteDragon);
                return true;
            }
            return false;
        }

        private bool StoneOfLegendGraveEffect()
        {
            AI.SelectCard(CardId.BlueEyesWhiteDragon);
            return true;
        }

        private bool AbyssDragonActivate()
        {
            AI.SelectCard(CardId.UltimateFusion);
            AI.SelectNextCard(CardId.BlueEyesJetDragon, CardId.BlueEyesAlternativeWhiteDragon);
            return true;
        }

        private bool RelinquishedAnimaSummon()
        {
            ClientCard tunerFodder = Bot.GetMonsters().FirstOrDefault(m => m.Level == 1 && (m.Id == CardId.SageWithEyesOfBlue || m.Id == CardId.TheWhiteStoneOfAncients || m.Id == CardId.TheWhiteStoneOfLegend));
            return tunerFodder != null && Enemy.GetMonsterCount() > 0;
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

        private bool SPLittleKnightSummon()
        {
            // CRITICAL RULE 350: NEVER summon S:P in Main Phase 1 if enemy has no monsters or if we can direct attack!
            // S:P prevents all monsters from attacking directly for the rest of the turn!
            if (Duel.Phase == DuelPhase.Main1 && (Enemy.GetMonsterCount() == 0 || Bot.GetMonsters().Any(m => m.Attack >= 2500)))
            {
                return false;
            }

            // Only summon if opponent has a critical card on field or GY that needs banishing
            bool oppHasTarget = Enemy.GetMonsters().Any(m => m.IsFaceup() && (m.Attack >= 2500 || AntiFloodgateHelper.NegateMonsterIds.Contains(m.Id) || !m.IsDisabled()))
                             || Enemy.GetSpells().Any(s => s.IsFaceup())
                             || Enemy.Graveyard.Any(c => c.IsMonster() && c.Attack >= 2000);

            if (!oppHasTarget) return false;

            // Fodder MUST be low cost (NEVER sacrifice BEWD, Jet, or Xyz/Synchro bosses!)
            var availableFodder = Bot.GetMonsters().Where(m => m.HasType(CardType.Effect) && !IsHighValueBoss(m)).ToList();
            if (availableFodder.Count >= 2) return true;

            return false;
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

        private bool HeavenlySpheresSummon()
        {
            // Rule: NEVER summon Heavenly Spheres in Main Phase 1 of an attacking turn!
            if (Duel.Phase == DuelPhase.Main1 && Duel.Turn > 1 && Bot.GetMonsters().Any(m => m.Attack >= 2500))
            {
                return false;
            }

            // Must NOT use 2 high-ATK bosses (e.g. 2 BEWD)!
            var fodderDragons = Bot.GetMonsters().Where(m => m.HasRace(CardRace.Dragon) && !IsHighValueBoss(m)).ToList();
            var allDragons = Bot.GetMonsters().Where(m => m.HasRace(CardRace.Dragon)).ToList();

            if (fodderDragons.Count >= 1 && allDragons.Count >= 2)
            {
                if (Duel.Turn == 1 || Duel.Phase == DuelPhase.Main2)
                {
                    return true;
                }
            }

            return false;
        }

        private bool SpiritDragonSummon()
        {
            return true;
        }

        private bool TwinBurstSummon()
        {
            int bewdOnField = Bot.GetMonsters().Count(m => m.Id == CardId.BlueEyesWhiteDragon || m.Id == CardId.BlueEyesAlternativeWhiteDragon);
            if (bewdOnField < 2) return false;

            // Only summon if opponent has a monster that cannot be destroyed by battle or high DEF wall
            bool oppHasBattleImmuneOrWall = Enemy.GetMonsters().Any(m => m.IsFaceup() && (m.Attack >= 3000 || m.IsDefense()));
            return oppHasBattleImmuneOrWall && Bot.ExtraDeck.Any(c => c.Id == CardId.BlueEyesTwinBurstDragon);
        }

        private bool TwinBurstBanishActivate()
        {
            return true;
        }

        private bool DraglubionSummon()
        {
            int l8Count = Bot.GetMonsters().Count(m => m.Level == 8);
            return l8Count >= 2;
        }

        private bool DraglubionActivate()
        {
            AI.SelectCard(CardId.Number100NumeronDragon);
            AI.SelectNextCard(CardId.Number38HopeHarbinger);
            return true;
        }

        private bool NumeronDragonActivate()
        {
            return true;
        }

        private bool HopeHarbingerSummon()
        {
            int l8Count = Bot.GetMonsters().Count(m => m.Level == 8);
            return l8Count >= 2;
        }

        private bool PhotonLordSummon()
        {
            int l8Count = Bot.GetMonsters().Count(m => m.Level == 8);
            return l8Count >= 2;
        }

        private bool NeoBlueEyesEffect()
        {
            return true;
        }

        private bool TyrantDragonEffect()
        {
            AI.SelectCard(CardId.InfiniteImpermanence, CardId.TrueLight);
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
            AI.SelectPosition(CardPosition.FaceUpDefence);
            return DefaultNibiru();
        }

        private bool SpellSetStrategy()
        {
            if (Card.IsTrap() && (Card.Id == CardId.InfiniteImpermanence || Card.Id == CardId.TrueLight))
            {
                return true;
            }
            if (Card.Id == CardId.UltimateFusion)
            {
                return true;
            }
            return false;
        }

        private bool RepositionStrategy()
        {
            if (Card.Attack >= 2500 && Card.IsDefense()) return true;
            if (Card.Attack < 1500 && Card.IsAttack()) return true;
            return false;
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Anti-Pattern Rule 1: Isolation of HINTMSG_ATOHAND (506)
            if (hint == 506)
            {
                var target = DeckPlugin?.Strategy?.PickSearchTarget(cards, Card);
                if (target != null && cards.Contains(target))
                {
                    return new List<ClientCard> { target };
                }
            }

            // Anti-Pattern Rule 2: Destruction (502) or Banish (503/504) must target enemy cards (c.Controller == 1)
            if (hint == 502 || hint == 503 || hint == 504)
            {
                var enemyTargets = cards.Where(c => c.Controller == 1).ToList();
                if (enemyTargets.Count >= min)
                {
                    return enemyTargets.OrderByDescending(c => c.Attack).Take(max).ToList();
                }
            }

            // Hint 500 (Release/Tribute) or Hint 507 (Material Selection) or Extra Deck Material Sort
            if (hint == 500 || hint == 507 || (Duel.Phase != DuelPhase.Battle && cards.All(c => c.Location == CardLocation.MonsterZone && c.Controller == 0)))
            {
                var sorted = DeckPlugin?.MaterialEvaluator?.SortMaterials(cards, min);
                if (sorted != null && sorted.Count >= min)
                {
                    return sorted.Take(max).ToList();
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
