using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("Rank5", "AI_Rank5")]
    [Deck("R5NK", "AI_Rank5")]
    [Deck("Rank V", "AI_Rank5")]
    public class Rank5Executor : ModernExecutor
    {
        public class CardId
        {
            public const int MistArchfiend = 28601770;
            public const int CyberDragon = 70095154;
            public const int ZWEagleClaw = 29353756;
            public const int SolarWindJammer = 33911264;
            public const int QuickdrawSynchron = 20932152;
            public const int WindUpSoldier = 12299841;
            public const int StarDrawing = 24610207;
            public const int ChronomalyGoldenJet = 88552992;

            public const int InstantFusion = 1845204;
            public const int DoubleSummon = 43422537;
            public const int MysticalSpaceTyphoon = 5318639;
            public const int BookOfMoon = 14087893;
            public const int XyzUnit = 13032689;
            public const int XyzReborn = 26708437;
            public const int MirrorForce = 44095762;
            public const int TorrentialTribute = 53582587;
            public const int XyzVeil = 96457619;

            public const int PanzerDragon = 72959823;
            public const int GaiaDragonTheThunderCharger = 91949988;
            public const int CyberDragonInfinity = 10443957;
            public const int TirasKeeperOfGenesis = 31386180;
            public const int Number61Volcasaurus = 29669359;
            public const int SharkFortress = 50449881;
            public const int CyberDragonNova = 58069384;
        }

        public ClientCard CurrentExecutingCard => Card;

        private bool NormalSummoned = false;
        private bool InstantFusionUsed = false;
        private bool DoubleSummonUsed = false;
        private bool CyberDragonInfinitySummoned = false;
        private bool Number61VolcasaurusUsed = false;

        public Rank5Executor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            DeckPlugin = new Rank5Plugin(this);

            // Tier 0: Quick spells & disruptions
            AddExecutor(ExecutorType.Activate, CardId.BookOfMoon, DefaultBookOfMoon);
            AddExecutor(ExecutorType.Activate, CardId.MysticalSpaceTyphoon, DefaultMysticalSpaceTyphoon);

            // Tier 1: Cyber Dragon Infinity (Negate & Absorb)
            AddExecutor(ExecutorType.SpSummon, CardId.CyberDragonNova, CyberDragonNovaSummon);
            AddExecutor(ExecutorType.Activate, CardId.CyberDragonNova, CyberDragonNovaEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.CyberDragonInfinity, CyberDragonInfinitySummon);
            AddExecutor(ExecutorType.Activate, CardId.CyberDragonInfinity, CyberDragonInfinityEffect);

            // Tier 2: Level 5 Extenders & Free Summons
            AddExecutor(ExecutorType.SpSummon, CardId.CyberDragon);
            AddExecutor(ExecutorType.SpSummon, CardId.ZWEagleClaw);
            AddExecutor(ExecutorType.SpSummon, CardId.SolarWindJammer, SolarWindJammerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.QuickdrawSynchron, QuickdrawSynchronSummon);
            AddExecutor(ExecutorType.Activate, CardId.InstantFusion, InstantFusionEffect);

            // Tier 3: Normal Summons & Level Modifications
            AddExecutor(ExecutorType.Summon, CardId.ChronomalyGoldenJet, NormalSummon);
            AddExecutor(ExecutorType.Activate, CardId.ChronomalyGoldenJet, ChronomalyGoldenJetEffect);
            AddExecutor(ExecutorType.Summon, CardId.StarDrawing, NormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.WindUpSoldier, NormalSummon);
            AddExecutor(ExecutorType.Activate, CardId.WindUpSoldier, WindUpSoldierEffect);
            AddExecutor(ExecutorType.Summon, CardId.MistArchfiend, MistArchfiendSummon);

            // Tier 4: Rank 5 Xyz Monsters
            AddExecutor(ExecutorType.SpSummon, CardId.Number61Volcasaurus, Number61VolcasaurusSummon);
            AddExecutor(ExecutorType.Activate, CardId.Number61Volcasaurus, Number61VolcasaurusEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.TirasKeeperOfGenesis);
            AddExecutor(ExecutorType.Activate, CardId.TirasKeeperOfGenesis, TirasKeeperOfGenesisEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.SharkFortress);
            AddExecutor(ExecutorType.Activate, CardId.SharkFortress);

            // Tier 5: Rank 7 Overlay
            AddExecutor(ExecutorType.SpSummon, CardId.GaiaDragonTheThunderCharger, GaiaDragonTheThunderChargerSummon);

            // Tier 6: Spells & Traps Support
            AddExecutor(ExecutorType.Activate, CardId.DoubleSummon, DoubleSummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.XyzUnit, XyzUnitEffect);
            AddExecutor(ExecutorType.Activate, CardId.XyzReborn, XyzRebornEffect);
            AddExecutor(ExecutorType.Activate, CardId.PanzerDragon, PanzerDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.XyzVeil, XyzVeilEffect);
            AddExecutor(ExecutorType.Activate, CardId.TorrentialTribute, DefaultTorrentialTribute);
            AddExecutor(ExecutorType.Activate, CardId.MirrorForce, DefaultTrap);

            // Reposition & Backrow Sets
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
            AddExecutor(ExecutorType.SpellSet, DefaultSpellSet);
        }

        public override bool OnSelectHand() => false;

        public override void OnNewTurn()
        {
            NormalSummoned = false;
            InstantFusionUsed = false;
            DoubleSummonUsed = false;
            CyberDragonInfinitySummoned = false;
            Number61VolcasaurusUsed = false;
            base.OnNewTurn();
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (DeckPlugin is Rank5Plugin r5Plugin)
            {
                var pluginSelected = r5Plugin.SelectCardLogic(cards, min, max, hint, cancelable);
                if (pluginSelected != null && pluginSelected.Count >= min)
                    return pluginSelected;
            }
            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            IList<ClientCard> result = Util.SelectPreferredCards(new[] {
                CardId.StarDrawing,
                CardId.PanzerDragon,
                CardId.MistArchfiend,
                CardId.SolarWindJammer
            }, cards, min, max);
            return Util.CheckSelectCount(result, cards, min, max);
        }

        private bool NormalSummon()
        {
            NormalSummoned = true;
            return true;
        }

        private bool SolarWindJammerSummon()
        {
            if (!NeedLV5())
                return false;
            AI.SelectPosition(CardPosition.FaceUpDefence);
            return true;
        }

        private bool QuickdrawSynchronSummon()
        {
            if (!NeedLV5())
                return false;
            AI.SelectCard(
                CardId.ZWEagleClaw,
                CardId.SolarWindJammer,
                CardId.QuickdrawSynchron,
                CardId.CyberDragon,
                CardId.MistArchfiend,
                CardId.WindUpSoldier,
                CardId.StarDrawing,
                CardId.ChronomalyGoldenJet
                );
            return true;
        }

        private bool MistArchfiendSummon()
        {
            if (!NeedLV5())
                return false;
            AI.SelectOption(1);
            NormalSummoned = true;
            return true;
        }

        private bool InstantFusionEffect()
        {
            // Rule 16: LP Safety check (1000 LP cost)
            if (Bot.LifePoints <= 1000)
                return false;

            if (!NeedLV5())
                return false;
            InstantFusionUsed = true;
            return true;
        }

        private bool NeedLV5()
        {
            if (HaveOtherLV5OnField())
                return true;
            if (Util.GetBotAvailZonesFromExtraDeck() == 0)
                return false;
            int lv5Count = 0;
            foreach (ClientCard card in Bot.Hand)
            {
                if (card.IsCode(CardId.SolarWindJammer) && Bot.GetMonsterCount() == 0)
                    ++lv5Count;
                if (card.IsCode(CardId.InstantFusion) && !InstantFusionUsed && Bot.LifePoints > 1000)
                    ++lv5Count;
                if (card.IsCode(CardId.QuickdrawSynchron) && Bot.Hand.ContainsMonsterWithLevel(4))
                    ++lv5Count;
                if (card.IsCode(CardId.MistArchfiend) && !NormalSummoned)
                    ++lv5Count;
                if (card.IsCode(CardId.DoubleSummon) && DoubleSummonEffect())
                    ++lv5Count;
            }
            if (lv5Count >= 2)
                return true;
            return false;
        }

        private bool WindUpSoldierEffect()
        {
            return HaveOtherLV5OnField();
        }

        private bool ChronomalyGoldenJetEffect()
        {
            return Card.Level == 4;
        }

        private bool DoubleSummonEffect()
        {
            if (!NormalSummoned || DoubleSummonUsed)
                return false;
            if (Bot.HasInHand(new[]
                {
                    CardId.WindUpSoldier,
                    CardId.StarDrawing,
                    CardId.ChronomalyGoldenJet,
                    CardId.MistArchfiend
                }))
            {
                NormalSummoned = false;
                DoubleSummonUsed = true;
                return true;
            }
            return false;
        }

        private bool CyberDragonNovaSummon()
        {
            return !CyberDragonInfinitySummoned;
        }

        private bool CyberDragonNovaEffect()
        {
            if (ActivateDescription == Util.GetStringId(CardId.CyberDragonNova, 0))
            {
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CyberDragonInfinitySummon()
        {
            CyberDragonInfinitySummoned = true;
            return true;
        }

        private bool CyberDragonInfinityEffect()
        {
            // Negate effect
            if (Duel.CurrentChain.Count > 0)
            {
                return Duel.LastChainPlayer == 1;
            }
            else
            {
                // Absorb effect - Rule 15: Only activate if face-up attack monster exists
                ClientCard bestmonster = null;
                foreach (ClientCard monster in Enemy.GetMonsters())
                {
                    if (monster.IsFaceup() && monster.IsAttack() && (bestmonster == null || monster.Attack >= bestmonster.Attack))
                        bestmonster = monster;
                }
                if (bestmonster != null)
                {
                    AI.SelectCard(bestmonster);
                    return true;
                }
            }
            return false;
        }

        private bool Number61VolcasaurusSummon()
        {
            return Util.IsOneEnemyBetterThanValue(2000, false);
        }

        private bool Number61VolcasaurusEffect()
        {
            // Rule 15: Target Verification Safeguard
            ClientCard target = Util.GetProblematicEnemyMonster(2000);
            if (target != null)
            {
                AI.SelectCard(CardId.CyberDragon);
                AI.SelectNextCard(target);
                Number61VolcasaurusUsed = true;
                return true;
            }
            return false;
        }

        private bool TirasKeeperOfGenesisEffect()
        {
            // Rule 15: Target Verification Safeguard
            ClientCard target = Util.GetProblematicEnemyCard();
            if (target == null)
                target = Util.GetBestEnemyCard();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool GaiaDragonTheThunderChargerSummon()
        {
            if (Number61VolcasaurusUsed && Bot.HasInMonstersZone(CardId.Number61Volcasaurus))
            {
                AI.SelectCard(CardId.Number61Volcasaurus);
                return true;
            }
            foreach (ClientCard monster in Bot.GetMonsters())
            {
                if (monster.HasType(CardType.Xyz) && !monster.HasXyzMaterial())
                {
                    AI.SelectCard(monster);
                    return true;
                }
            }
            return false;
        }

        private bool XyzRebornEffect()
        {
            if (!UniqueFaceupSpell())
                return false;
            AI.SelectCard(
                CardId.CyberDragonInfinity,
                CardId.CyberDragonNova,
                CardId.TirasKeeperOfGenesis,
                CardId.SharkFortress,
                CardId.Number61Volcasaurus
                );
            return true;
        }

        private bool XyzUnitEffect()
        {
            foreach (ClientCard monster in Bot.GetMonsters())
            {
                if (monster.HasType(CardType.Xyz))
                {
                    AI.SelectCard(monster);
                    return true;
                }
            }
            return false;
        }

        private bool PanzerDragonEffect()
        {
            // Rule 15: Target Verification Safeguard
            ClientCard target = Util.GetBestEnemyCard();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool XyzVeilEffect()
        {
            if (!UniqueFaceupSpell())
                return false;
            foreach (ClientCard monster in Bot.GetMonsters())
            {
                if (monster.HasType(CardType.Xyz))
                    return true;
            }
            return false;
        }

        private bool HaveOtherLV5OnField()
        {
            foreach (ClientCard monster in Bot.GetMonsters())
            {
                if (monster.HasType(CardType.Monster) &&
                    !monster.HasType(CardType.Xyz) &&
                    Util.GetBotAvailZonesFromExtraDeck(monster) > 0 &&
                    (monster.Level == 5
                    || monster.IsCode(CardId.StarDrawing)
                    || monster.IsCode(CardId.WindUpSoldier) && !monster.Equals(Card)))
                    return true;
            }
            return false;
        }
    }
}
