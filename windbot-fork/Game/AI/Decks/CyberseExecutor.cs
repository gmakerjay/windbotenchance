using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Plugin;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("Cyberse", "AI_ST1732")]
    [Deck("ST1732", "AI_ST1732")]
    public class CyberseExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Monsters
            public const int LadyDebug = 16188701;
            public const int CyberseGadget = 645087;
            public const int Draconnet = 62706865;
            public const int Bitron = 36211150;
            public const int BalancerLord = 8567955;
            public const int Linkslayer = 35595518;
            public const int ROMCloudia = 44956694;
            public const int DotScaper = 18789533;
            public const int BootStagguard = 70950698;
            public const int Backlinker = 71172240;
            public const int DualAssembloom = 7445307;
            public const int AshBlossom = 14558127;
            public const int EffectVeiler = 97268402;

            // Spells
            public const int CynetMining = 57160136;
            public const int CynetBackdoor = 43839002;
            public const int MonsterReborn = 83764718;
            public const int DarkHole = 53129443;
            public const int Raigeki = 12580477;
            public const int HarpiesFeatherDuster = 18144506;
            public const int MindControl = 37520316;
            public const int CalledByTheGrave = 24224830;
            public const int MoonMirrorShield = 19508728;

            // Traps
            public const int InfiniteImpermanence = 10045474;
            public const int SolemnStrike = 40605147;
            public const int CompulsoryEvacuationDevice = 94192409;

            // Extra Deck
            public const int AccesscodeTalker = 86066372;
            public const int TranscodeTalker = 46947713;
            public const int DecodeTalker = 1861629;
            public const int EncodeTalker = 6622715;
            public const int TriGateWizard = 32617464;
            public const int SplashMage = 59859086;
            public const int UpdateJammer = 88093706;
            public const int Honeybot = 34472920;
            public const int BinarySorceress = 79016563;
            public const int LinkSpider = 98978921;

            // Tokens
            public const int GadgetToken = 645088;
            public const int StagToken = 70950699;
        }

        public bool BalancerLordUsed { get; set; } = false;
        public ClientCard CurrentExecutingCard => Card;

        public CyberseExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            DeckPlugin = new CybersePlugin(this);

            // Tier 0: Handtraps & Quick Disruptions
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, OnAshBlossom);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, OnEffectVeiler);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, OnInfiniteImpermanence);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, OnCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.SolemnStrike, OnSolemnStrike);
            AddExecutor(ExecutorType.Activate, CardId.DecodeTalker, OnDecodeTalkerActivate);
            AddExecutor(ExecutorType.Activate, CardId.TriGateWizard, OnTriGateWizardActivate);

            // Tier 1: Board Breakers
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, OnHarpiesFeatherDuster);
            AddExecutor(ExecutorType.Activate, CardId.Raigeki, OnRaigeki);
            AddExecutor(ExecutorType.Activate, CardId.DarkHole, OnDarkHole);
            AddExecutor(ExecutorType.Activate, CardId.MindControl, OnMindControl);
            AddExecutor(ExecutorType.SpSummon, CardId.Backlinker, OnBacklinkerSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Backlinker, OnBacklinkerActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.Linkslayer, OnLinkslayerSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Linkslayer, OnLinkslayerActivate);
            AddExecutor(ExecutorType.Activate, CardId.CompulsoryEvacuationDevice, OnCompulsoryEvacuationDevice);

            // Tier 2: Searchers & Starters
            AddExecutor(ExecutorType.Activate, CardId.CynetMining, OnCynetMining);
            AddExecutor(ExecutorType.Summon, CardId.LadyDebug, OnLadyDebugSummon);
            AddExecutor(ExecutorType.Activate, CardId.LadyDebug, OnLadyDebugActivate);
            AddExecutor(ExecutorType.Summon, CardId.CyberseGadget, OnCyberseGadgetSummon);
            AddExecutor(ExecutorType.Activate, CardId.CyberseGadget, OnCyberseGadgetActivate);
            AddExecutor(ExecutorType.Summon, CardId.BalancerLord, OnBalancerLordSummon);
            AddExecutor(ExecutorType.Activate, CardId.BalancerLord, OnBalancerLordActivate);
            AddExecutor(ExecutorType.Summon, CardId.Draconnet, OnDraconnetSummon);
            AddExecutor(ExecutorType.Activate, CardId.Draconnet, OnDraconnetActivate);
            AddExecutor(ExecutorType.Summon, CardId.ROMCloudia, OnROMCloudiaSummon);
            AddExecutor(ExecutorType.Activate, CardId.ROMCloudia, OnROMCloudiaActivate);

            // Tier 3: Free Extenders & Spells
            AddExecutor(ExecutorType.Activate, CardId.BootStagguard, OnBootStagguardActivate);
            AddExecutor(ExecutorType.Activate, CardId.DotScaper, OnDotscaperActivate);
            AddExecutor(ExecutorType.Activate, CardId.DualAssembloom, OnDualAssembloomActivate);
            AddExecutor(ExecutorType.Activate, CardId.MonsterReborn, OnMonsterReborn);
            AddExecutor(ExecutorType.Activate, CardId.MoonMirrorShield, OnMoonMirrorShield);
            AddExecutor(ExecutorType.Activate, CardId.CynetBackdoor, OnCynetBackdoor);
            AddExecutor(ExecutorType.SummonOrSet, CardId.Backlinker, OnNormalSummonFodder);
            AddExecutor(ExecutorType.SummonOrSet, CardId.Bitron, OnNormalSummonFodder);

            // Tier 4: Link Climbing Pipeline (Top-down priority)
            // 4a. Link-4 Finisher: Accesscode Talker!
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, OnAccesscodeTalkerSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker, OnAccesscodeTalkerActivate);

            // 4b. Link-3 Bosses: Transcode Talker, Decode Talker, Tri-Gate, Encode Talker
            AddExecutor(ExecutorType.SpSummon, CardId.TranscodeTalker, OnTranscodeTalkerSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TranscodeTalker, OnTranscodeTalkerActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.DecodeTalker, OnDecodeTalkerSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TriGateWizard, OnTriGateWizardSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EncodeTalker, OnEncodeTalkerSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.EncodeTalker, OnEncodeTalkerActivate);

            // 4c. Link-2 Stepping Stones: Splash Mage & Update Jammer
            AddExecutor(ExecutorType.SpSummon, CardId.SplashMage, OnSplashMageSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SplashMage, OnSplashMageActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.UpdateJammer, OnUpdateJammerSpSummon);

            // 4d. Link-1: Link Spider
            AddExecutor(ExecutorType.SpSummon, CardId.LinkSpider, OnLinkSpiderSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.LinkSpider, OnLinkSpiderActivate);

            // 4e. Fallback Link-2
            AddExecutor(ExecutorType.SpSummon, CardId.Honeybot, OnHoneybotSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BinarySorceress, OnBinarySorceressSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BinarySorceress, OnBinarySorceressActivate);

            // Tier 5: Backrow Sets
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, OnSpellSetDefault);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnStrike, OnSpellSetDefault);
            AddExecutor(ExecutorType.SpellSet, CardId.CompulsoryEvacuationDevice, OnSpellSetDefault);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, OnSpellSetDefault);
            AddExecutor(ExecutorType.SpellSet, CardId.CynetBackdoor, OnSpellSetDefault);

            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override bool OnSelectHand() => false;

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            BalancerLordUsed = false;
        }

        public override int OnSelectOption(IList<long> options) => options.Count == 2 ? 1 : 0;

        public override bool OnSelectYesNo(long desc)
        {
            if (desc == 210) return false; // Continue selecting materials?
            if (desc == 31) return true;   // Direct attack?
            if (Card != null && Card.Controller == 1) return false;
            return base.OnSelectYesNo(desc);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.Bitron || cardId == CardId.DotScaper || cardId == CardId.BootStagguard || cardId == CardId.AshBlossom || cardId == CardId.EffectVeiler)
            {
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }
            return base.OnSelectPosition(cardId, positions);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // 533: Link Material selection
            if (hint == 533)
            {
                var sorted = DeckPlugin?.MaterialEvaluator?.SortMaterials(cards, min);
                if (sorted != null && sorted.Count >= min)
                    return sorted.Take(min).ToList();
            }

            // 506: ATOHAND (Search / Retrieve)
            if (hint == 506)
            {
                var target = DeckPlugin?.Strategy?.PickSearchTarget(cards, Card);
                if (target != null)
                    return new List<ClientCard> { target };
            }

            // 509: SPSUMMON (Special Summon)
            if (hint == 509)
            {
                var target = DeckPlugin?.Strategy?.PickSpecialSummonTarget(cards);
                if (target != null)
                    return new List<ClientCard> { target };
            }

            // 501: DISCARD (Cost Discard)
            if (hint == 501)
            {
                var list = new List<ClientCard>();
                var pool = new List<ClientCard>(cards);
                for (int i = 0; i < min; i++)
                {
                    var target = DeckPlugin?.MaterialEvaluator?.PickDiscardTarget(pool, 1);
                    if (target != null)
                    {
                        list.Add(target);
                        pool.Remove(target);
                    }
                    else break;
                }
                if (list.Count >= min) return list;
            }

            // 502: DESTROY / 503: REMOVE / 505: RTOHAND (Removal) -> Opponent cards only!
            if (hint == 502 || hint == 503 || hint == 505 || hint == 507)
            {
                var enemyCards = cards.Where(c => c.Controller == 1).ToList();
                if (enemyCards.Count > 0)
                {
                    var sorted = enemyCards.OrderByDescending(c => c.Attack).ToList();
                    return sorted.Take(min).ToList();
                }
            }

            // 500: RELEASE (Tribute for Decode Talker / RAM Clouder)
            if (hint == 500)
            {
                var fodder = cards.Where(c => c.Id != CardId.AccesscodeTalker && c.Id != CardId.TranscodeTalker && c.Id != CardId.DecodeTalker)
                                  .OrderBy(c => DeckPlugin?.MaterialEvaluator?.GetMaterialCost(c) ?? 50)
                                  .ToList();
                if (fodder.Count >= min)
                    return fodder.Take(min).ToList();
            }

            // 551: TARGET
            if (hint == 551)
            {
                if (Card != null && (Card.Id == CardId.CynetBackdoor || Card.Id == CardId.MoonMirrorShield))
                {
                    var friendly = cards.Where(c => c.Controller == 0).ToList();
                    if (friendly.Count > 0) return friendly.Take(min).ToList();
                }
                var enemy = cards.Where(c => c.Controller == 1).ToList();
                if (enemy.Count > 0) return enemy.OrderByDescending(c => c.Attack).Take(min).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // EXECUTOR ACTION HANDLERS
        // ═══════════════════════════════════════════════════════════════════════

        private bool OnAshBlossom() => Duel.LastChainPlayer != 0 && DefaultAshBlossomAndJoyousSpring();

        private bool OnEffectVeiler() => Duel.LastChainPlayer != 0 && DefaultEffectVeiler();

        private bool OnInfiniteImpermanence() => Duel.LastChainPlayer != 0 && DefaultInfiniteImpermanence();

        private bool OnCalledByTheGrave() => DefaultCalledByTheGrave();

        private bool OnSolemnStrike()
        {
            if (Bot.LifePoints <= 2000) return false;
            return DefaultSolemnStrike();
        }

        private bool OnDecodeTalkerActivate() => true;

        private bool OnTriGateWizardActivate() => true;

        private bool OnHarpiesFeatherDuster() => Enemy.GetSpells().Count > 0;

        private bool OnRaigeki() => Enemy.GetMonsters().Any(m => !m.HasType(CardType.Token));

        private bool OnDarkHole() => DefaultDarkHole();

        private bool OnMindControl() => Util.GetBestEnemyMonster(true) != null;

        private bool OnBacklinkerSpSummon()
        {
            return (Bot.MonsterZone[5] == null && Bot.MonsterZone[6] == null)
                && (Enemy.MonsterZone[5] != null || Enemy.MonsterZone[6] != null);
        }

        private bool OnBacklinkerActivate() => Enemy.MonsterZone[5] != null || Enemy.MonsterZone[6] != null;

        private bool OnLinkslayerSpSummon() => Bot.GetMonsterCount() == 0;

        private bool OnLinkslayerActivate() => Enemy.GetSpells().Count > 0 && Bot.Hand.Count >= 2;

        private bool OnCompulsoryEvacuationDevice() => DefaultCompulsoryEvacuationDevice();

        private bool OnCynetMining() => Bot.Hand.Count >= 2;

        private bool OnLadyDebugSummon() => true;

        private bool OnLadyDebugActivate() => true;

        private bool OnCyberseGadgetSummon() => true;

        private bool OnCyberseGadgetActivate() => true;

        private bool OnBalancerLordSummon() => !BalancerLordUsed;

        private bool OnBalancerLordActivate()
        {
            if (Card.Location == CardLocation.Removed) return true;
            if (Bot.LifePoints <= 1000) return false;

            bool hasOtherCyberseInHand = Bot.Hand.Any(c => c != Card && c.IsMonster());
            if (hasOtherCyberseInHand && !BalancerLordUsed)
            {
                BalancerLordUsed = true;
                return true;
            }
            return false;
        }

        private bool OnDraconnetSummon() => Bot.GetRemainingCount(CardId.Bitron, 1) > 0;

        private bool OnDraconnetActivate() => true;

        private bool OnROMCloudiaSummon() => Bot.Graveyard.Any(c => c.IsMonster() && c.HasRace(CardRace.Cyberse));

        private bool OnROMCloudiaActivate() => true;

        private bool OnNormalSummonFodder() => true;

        private bool OnBootStagguardActivate() => true;

        private bool OnDotscaperActivate() => true;

        private bool OnDualAssembloomActivate()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave)
            {
                int cyberseAvailable = Bot.Hand.Count(c => c != Card && c.IsMonster() && c.HasRace(CardRace.Cyberse))
                                     + Bot.GetMonsters().Count(c => c.Attack < 2000 && c.HasRace(CardRace.Cyberse));
                return cyberseAvailable >= 2;
            }
            return Enemy.GetMonsters().Any(m => m.IsFaceup() && m.Attack < Card.Attack);
        }

        private bool OnMonsterReborn()
        {
            var targets = new[] {
                CardId.AccesscodeTalker, CardId.TranscodeTalker, CardId.DecodeTalker,
                CardId.DualAssembloom, CardId.SplashMage, CardId.BalancerLord
            };
            return Bot.HasInGraveyard(targets);
        }

        private bool OnMoonMirrorShield() => Bot.GetMonsters().Any(m => m.IsFaceup());

        private bool OnCynetBackdoor()
        {
            if (Card.Location == CardLocation.Hand && Duel.Phase != DuelPhase.Main2 && Duel.Phase != DuelPhase.Main1)
                return false;

            var target = Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.BalancerLord || m.Id == CardId.DotScaper || m.Attack >= 1700);
            return target != null;
        }

        private bool OnLinkSpiderSpSummon()
        {
            return Bot.GetMonsters().Any(m => m.Id == CardId.Bitron || m.Id == CardId.GadgetToken);
        }

        private bool OnLinkSpiderActivate() => Bot.Hand.Any(c => c.Id == CardId.Bitron);

        private bool OnSplashMageSpSummon()
        {
            // Do not eat Link-3 or Link-4 boss monsters
            int fodder = Bot.GetMonsters().Count(m => m.Id != CardId.TranscodeTalker && m.Id != CardId.AccesscodeTalker && m.Id != CardId.DecodeTalker);
            return fodder >= 2;
        }

        private bool OnSplashMageActivate() => true;

        private bool OnUpdateJammerSpSummon()
        {
            int fodder = Bot.GetMonsters().Count(m => m.Level >= 2 && m.Id != CardId.TranscodeTalker && m.Id != CardId.AccesscodeTalker && m.Id != CardId.DecodeTalker);
            return fodder >= 2;
        }

        private bool OnHoneybotSpSummon()
        {
            if (Bot.GetMonsters().Any(m => m.Id == CardId.TranscodeTalker || m.Id == CardId.AccesscodeTalker))
                return false;
            int fodder = Bot.GetMonsters().Count(m => m.Id != CardId.TranscodeTalker && m.Id != CardId.AccesscodeTalker && m.Id != CardId.DecodeTalker && m.Attack < 1900);
            return fodder >= 2 && Util.GetBestAttack(Bot) < 1900 && Bot.GetMonsters().Count(m => m.IsFaceup() && m.HasType(CardType.Link)) == 0;
        }

        private bool OnBinarySorceressSpSummon()
        {
            if (Bot.GetMonsters().Any(m => m.Id == CardId.TranscodeTalker || m.Id == CardId.AccesscodeTalker))
                return false;
            int fodder = Bot.GetMonsters().Count(m => m.Id != CardId.TranscodeTalker && m.Id != CardId.AccesscodeTalker && m.Id != CardId.DecodeTalker && m.Attack < 1600);
            return fodder >= 2 && Util.GetBestAttack(Bot) < 1600 && Bot.GetMonsters().Count(m => m.IsFaceup() && m.HasType(CardType.Link)) == 0;
        }

        private bool OnBinarySorceressActivate() => true;

        private bool OnTranscodeTalkerSpSummon()
        {
            // Summon Transcode Talker using 2+ Effect Monsters (Link-2 + 1 or 3 monsters)
            // Do NOT eat Accesscode Talker!
            int materials = Bot.GetMonsters().Count(m => m.Id != CardId.AccesscodeTalker);
            bool hasLink2 = Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasType(CardType.Link) && m.LinkMarker == 2);
            return (hasLink2 && materials >= 2) || materials >= 3;
        }

        private bool OnTranscodeTalkerActivate() => true;

        private bool OnDecodeTalkerSpSummon()
        {
            int materials = Bot.GetMonsters().Count(m => m.Id != CardId.AccesscodeTalker && m.Id != CardId.TranscodeTalker && m.Attack < 2300);
            return materials >= 2 && (Util.IsTurn1OrMain2() || Util.IsOneEnemyBetter());
        }

        private bool OnTriGateWizardSpSummon()
        {
            int materials = Bot.GetMonsters().Count(m => m.Id != CardId.AccesscodeTalker && m.Id != CardId.TranscodeTalker && m.Attack < 2200);
            return materials >= 2 && Bot.GetMonsterCount() >= 3;
        }

        private bool OnEncodeTalkerSpSummon()
        {
            int materials = Bot.GetMonsters().Count(m => m.Id != CardId.AccesscodeTalker && m.Id != CardId.TranscodeTalker && m.Attack < 2300);
            return materials >= 2 && Util.IsOneEnemyBetter();
        }

        private bool OnEncodeTalkerActivate() => true;

        private bool OnAccesscodeTalkerSpSummon()
        {
            bool hasLink3 = Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasType(CardType.Link) && m.LinkMarker == 3);
            bool hasLink2 = Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasType(CardType.Link) && m.LinkMarker == 2);
            int totalMonsters = Bot.GetMonsterCount();

            if (hasLink3 && totalMonsters >= 2) return true;
            if (hasLink2 && totalMonsters >= 3) return true;
            if (totalMonsters >= 4) return true;

            return false;
        }

        private bool OnAccesscodeTalkerActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
            }
            return true;
        }

        private bool OnSpellSetDefault() => DefaultSpellSet();
    }
}
