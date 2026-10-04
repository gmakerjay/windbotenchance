using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Decks;
using WindBot.Game.AI.Plugin;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Plugins
{
    // ═══════════════════════════════════════════════════════════════
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: LunaPlugin
    //  Lunalight Fusion Turbo, Pendulum Re-scale & OTK Engine
    // ═══════════════════════════════════════════════════════════════
    public class LunaPlugin : DeckPluginBase
    {
        private readonly LunaExecutor _exec;

        public override string DeckName => "Luna";

        public LunaStrategy StrategyImpl { get; }
        public LunaMaterialEvaluator MaterialImpl { get; }
        public LunaThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public LunaPlugin(LunaExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new LunaStrategy(exec);
            MaterialImpl = new LunaMaterialEvaluator(exec);
            ThreatImpl = new LunaThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class LunaStrategy : IDeckStrategy
    {
        private readonly LunaExecutor _exec;

        public LunaStrategy(LunaExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Extra Deck Fusion Summons
            if (candidates.Any(c => c != null && c.Location == CardLocation.Extra))
            {
                // Liger Dancer (3800 ATK, double attack, immune, SS board wipe)
                var liger = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.LigerDancer);
                if (liger != null && (_exec.CanDealLethalCheck() || _exec.Enemy.GetMonsters().Any(m => m != null && m.IsSpecialSummoned)))
                    return liger;

                // Leo Dancer (3500 ATK, untargetable, indestructible, double attack, SS board wipe)
                var leo = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.LeoDancer);
                if (leo != null) return leo;

                // Perfume Dancer: Key starter! 2 Lunalight monsters -> searches Luna Light Perfume on summon!
                if (!_exec.Bot.HasInMonstersZone(LunaExecutor.CardId.PerfumeDancer))
                {
                    var perfume = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.PerfumeDancer);
                    if (perfume != null) return perfume;
                }

                // Sabre Dancer: 3000+ ATK untargetable beater
                var sabre = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.SabreDancer);
                if (sabre != null) return sabre;

                // Panther Dancer: Fodder for Leo Dancer or double attacker
                var panther = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.PantherDancer);
                if (panther != null) return panther;

                // Rank 4 Xyz
                if (_exec.Duel.Turn == 1 || _exec.Duel.Phase == DuelPhase.Main2)
                {
                    var bagooska = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.Bagooska);
                    if (bagooska != null) return bagooska;
                }

                var tigerKing = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.TigerKing);
                if (tigerKing != null) return tigerKing;

                var dugares = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.Dugares);
                if (dugares != null) return dugares;
            }

            // Deck Summons (via Silver Hound e1 when sent by effect):
            if (candidates.Any(c => c != null && c.Location == CardLocation.Deck))
            {
                if (!_exec.Bot.HasInMonstersZone(LunaExecutor.CardId.KaleidoChick))
                {
                    var chick = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.KaleidoChick);
                    if (chick != null) return chick;
                }

                if (!_exec.Bot.HasInMonstersZone(LunaExecutor.CardId.GoldLeo))
                {
                    var leo = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.GoldLeo);
                    if (leo != null) return leo;
                }

                var marten = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.YellowMarten);
                if (marten != null) return marten;

                var bird = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.EmeraldBird);
                if (bird != null) return bird;

                var sheep = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.BlackSheep);
                if (sheep != null) return sheep;
            }

            // GY Revives (via Tiger Pendulum / Luna Light Perfume / Dugares):
            if (candidates.Any(c => c != null && c.Location == CardLocation.Grave))
            {
                var chick = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.KaleidoChick);
                if (chick != null) return chick;

                var leo = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.LeoDancer && c.IsCanRevive());
                if (leo != null) return leo;

                var perfume = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.PerfumeDancer && c.IsCanRevive());
                if (perfume != null) return perfume;

                var sabre = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.SabreDancer && c.IsCanRevive());
                if (sabre != null) return sabre;

                var marten = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.YellowMarten);
                if (marten != null) return marten;

                var bird = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.EmeraldBird);
                if (bird != null) return bird;

                var leoMain = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.GoldLeo);
                if (leoMain != null) return leoMain;

                var hound = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.SilverHound);
                if (hound != null) return hound;
            }

            return candidates.FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Fraktall dump to GY:
            if (context != null && context.IsCode(LunaExecutor.CardId.TriBrigadeFraktall))
            {
                var hound = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.SilverHound);
                if (hound != null) return hound;

                var sheep = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.BlackSheep);
                if (sheep != null) return sheep;
            }

            // Kaleido Chick Extra/Deck dump:
            if (context != null && context.IsCode(LunaExecutor.CardId.KaleidoChick))
            {
                var panther = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.PantherDancer);
                if (panther != null) return panther;

                var marten = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.YellowMarten);
                if (marten != null) return marten;

                var bird = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.EmeraldBird);
                if (bird != null) return bird;
            }

            // Tenki search:
            if (context != null && context.IsCode(LunaExecutor.CardId.Tenki))
            {
                if (!_exec.HasNormalSummonedThisTurn && !_exec.Bot.HasInHand(LunaExecutor.CardId.GoldLeo))
                {
                    var leo = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.GoldLeo);
                    if (leo != null) return leo;
                }

                if (!_exec.Bot.HasInHand(LunaExecutor.CardId.Tiger) && !_exec.Bot.HasInSpellZone(LunaExecutor.CardId.Tiger))
                {
                    var tiger = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.Tiger);
                    if (tiger != null) return tiger;
                }

                if (!_exec.Bot.HasInHand(LunaExecutor.CardId.KaleidoChick))
                {
                    var chick = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.KaleidoChick);
                    if (chick != null) return chick;
                }

                var marten = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.YellowMarten);
                if (marten != null) return marten;

                var sheep = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.BlackSheep);
                if (sheep != null) return sheep;
            }

            // Gold Leo search (from Deck):
            if (context != null && context.IsCode(LunaExecutor.CardId.GoldLeo))
            {
                if (!_exec.Bot.HasInHand(LunaExecutor.CardId.Tiger) && !_exec.Bot.HasInSpellZone(LunaExecutor.CardId.Tiger))
                {
                    var tiger = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.Tiger);
                    if (tiger != null) return tiger;
                }

                if (!_exec.Bot.HasInHand(LunaExecutor.CardId.KaleidoChick))
                {
                    var chick = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.KaleidoChick);
                    if (chick != null) return chick;
                }

                if (!_exec.Bot.HasInHand(LunaExecutor.CardId.YellowMarten))
                {
                    var marten = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.YellowMarten);
                    if (marten != null) return marten;
                }

                var hound = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.SilverHound);
                if (hound != null) return hound;

                if (!_exec.Bot.HasInHand(LunaExecutor.CardId.Wolf) && !_exec.Bot.HasInSpellZone(LunaExecutor.CardId.Wolf))
                {
                    var wolf = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.Wolf);
                    if (wolf != null) return wolf;
                }

                var sheep = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.BlackSheep);
                if (sheep != null) return sheep;
            }

            // Yellow Marten GY search (Spell/Trap):
            if (context != null && context.IsCode(LunaExecutor.CardId.YellowMarten))
            {
                if (!_exec.Bot.HasInHand(LunaExecutor.CardId.LunaLightPerfume))
                {
                    var perfume = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.LunaLightPerfume);
                    if (perfume != null) return perfume;
                }

                var fusion = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.LunalightFusion);
                if (fusion != null) return fusion;

                var masquerade = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.LunalightMasquerade);
                if (masquerade != null) return masquerade;
            }

            // Perfume Dancer search (Luna Light Perfume):
            if (context != null && context.IsCode(LunaExecutor.CardId.PerfumeDancer))
            {
                var perfume = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.LunaLightPerfume);
                if (perfume != null) return perfume;
            }

            // Luna Light Perfume GY Banish search:
            if (context != null && context.IsCode(LunaExecutor.CardId.LunaLightPerfume))
            {
                if (!_exec.Bot.HasInHand(LunaExecutor.CardId.Wolf) && !_exec.Bot.HasInSpellZone(LunaExecutor.CardId.Wolf))
                {
                    var wolf = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.Wolf);
                    if (wolf != null) return wolf;
                }

                if (!_exec.Bot.HasInHand(LunaExecutor.CardId.Tiger) && !_exec.Bot.HasInSpellZone(LunaExecutor.CardId.Tiger))
                {
                    var tiger = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.Tiger);
                    if (tiger != null) return tiger;
                }

                var chick = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.KaleidoChick);
                if (chick != null) return chick;

                var marten = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.YellowMarten);
                if (marten != null) return marten;
            }

            return candidates.FirstOrDefault();
        }

        public ClientCard PickFoolishGraveTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Kaleido Chick Extra/Deck dump: Panther Dancer enables Leo Dancer!
            if (context != null && context.IsCode(LunaExecutor.CardId.KaleidoChick))
            {
                var panther = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.PantherDancer);
                if (panther != null) return panther;

                var marten = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.YellowMarten);
                if (marten != null) return marten;

                var bird = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.EmeraldBird);
                if (bird != null) return bird;
            }

            // 2. Fraktall dump: MUST dump Silver Hound!
            if (context != null && context.IsCode(LunaExecutor.CardId.TriBrigadeFraktall))
            {
                var hound = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.SilverHound);
                if (hound != null) return hound;

                var sheep = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.BlackSheep);
                if (sheep != null) return sheep;
            }

            // 3. Foolish Burial dump: Silver Hound (SS from deck) > Yellow Marten (search S/T)
            if (context != null && context.IsCode(LunaExecutor.CardId.FoolishBurial))
            {
                var hound = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.SilverHound);
                if (hound != null) return hound;

                var marten = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.YellowMarten);
                if (marten != null) return marten;

                var bird = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.EmeraldBird);
                if (bird != null) return bird;
            }

            // 4. Silver Hound on-field trigger dump: Yellow Marten > Emerald Bird > Kaleido Chick
            if (context != null && context.IsCode(LunaExecutor.CardId.SilverHound))
            {
                var marten = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.YellowMarten);
                if (marten != null) return marten;

                var bird = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.EmeraldBird);
                if (bird != null) return bird;

                var chick = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.KaleidoChick);
                if (chick != null) return chick;

                var sheep = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.BlackSheep);
                if (sheep != null) return sheep;
            }

            // 5. Lunalight Masquerade ignition dump: Silver Hound > Yellow Marten > Emerald Bird
            if (context != null && context.IsCode(LunaExecutor.CardId.LunalightMasquerade))
            {
                var hound = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.SilverHound);
                if (hound != null) return hound;

                var marten = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.YellowMarten);
                if (marten != null) return marten;

                var bird = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.EmeraldBird);
                if (bird != null) return bird;
            }

            // 6. Lunalight Fusion (materials from Extra/Deck):
            if (context != null && context.IsCode(LunaExecutor.CardId.LunalightFusion))
            {
                var panther = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.PantherDancer);
                if (panther != null) return panther;

                var liger = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.LigerDancer);
                if (liger != null) return liger;

                var leo = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.LeoDancer);
                if (leo != null) return leo;

                var marten = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.YellowMarten);
                if (marten != null) return marten;
            }

            // General fallback
            var defaultHound = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.SilverHound);
            if (defaultHound != null) return defaultHound;

            var defaultMarten = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.YellowMarten);
            if (defaultMarten != null) return defaultMarten;

            return candidates.FirstOrDefault();
        }
    }

    public class LunaMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly LunaExecutor _exec;

        public LunaMaterialEvaluator(LunaExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard c)
        {
            if (c == null) return 999;

            // Super Poly / Opponent cards
            if (c.Controller == 1) return 5;

            // In GY (Miracle Fusion via Wolf):
            if (c.Location == CardLocation.Grave)
            {
                if (c.Id == LunaExecutor.CardId.YellowMarten) return 15;
                if (c.Id == LunaExecutor.CardId.EmeraldBird) return 16;
                if (c.Id == LunaExecutor.CardId.BlackSheep) return 17;
                if (c.Id == LunaExecutor.CardId.SilverHound) return 18;
                if (c.Id == LunaExecutor.CardId.KaleidoChick) return 20;
                if (c.Id == LunaExecutor.CardId.PantherDancer) return 25;
                return 30;
            }

            // In Hand:
            if (c.Location == CardLocation.Hand)
            {
                if (c.Id == LunaExecutor.CardId.BlackSheep) return 30;
                if (c.Id == LunaExecutor.CardId.SilverHound) return 32;
                if (c.Id == LunaExecutor.CardId.EmeraldBird) return 34;
                if (c.Id == LunaExecutor.CardId.YellowMarten) return 36;
                if (c.Id == LunaExecutor.CardId.GoldLeo) return 38;
                if (c.Id == LunaExecutor.CardId.KaleidoChick) return 40;
                return 45;
            }

            // On Field:
            if (c.Location == CardLocation.MonsterZone)
            {
                if (c.Id == LunaExecutor.CardId.LeoDancer || c.Id == LunaExecutor.CardId.LigerDancer) return 999;
                if (c.Id == LunaExecutor.CardId.Bagooska) return 900;
                if (c.Id == LunaExecutor.CardId.SabreDancer) return 800;
                if (c.Id == LunaExecutor.CardId.PerfumeDancer) return 500; // Protect Perfume Dancer from being fused away!

                // Panther Dancer can be used if making Leo Dancer
                if (c.Id == LunaExecutor.CardId.PantherDancer) return 150;

                if (c.Id == LunaExecutor.CardId.BlackSheep) return 45;
                if (c.Id == LunaExecutor.CardId.SilverHound) return 50;
                if (c.Id == LunaExecutor.CardId.GoldLeo) return 55;
                if (c.Id == LunaExecutor.CardId.EmeraldBird) return 60;
                if (c.Id == LunaExecutor.CardId.YellowMarten) return 65;
                if (c.Id == LunaExecutor.CardId.KaleidoChick) return 70;

                if (_exec.IsAceCard(c)) return 950;
                return 100;
            }

            // Handtraps
            if (c.IsCode(LunaExecutor.CardId.AshBlossom, LunaExecutor.CardId.AshBlossomAlt, LunaExecutor.CardId.DrollAndLockBird, LunaExecutor.CardId.MulcharmyFuwalos, LunaExecutor.CardId.MulcharmyPurulia))
                return 850;

            return 200;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.Where(c => c != null).OrderBy(GetMaterialCost).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Preferred discard targets:
            // 1. Silver Hound: Special Summons a Lunalight from Deck when sent by effect!
            var hound = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.SilverHound);
            if (hound != null) return hound;

            // 2. Yellow Marten: Searches Lunalight Spell/Trap when sent by effect!
            var marten = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.YellowMarten);
            if (marten != null) return marten;

            // 3. Emerald Bird: Special Summons Lv4 or lower Lunalight from GY!
            var bird = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.EmeraldBird);
            if (bird != null) return bird;

            // 4. Black Sheep
            var sheep = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.BlackSheep);
            if (sheep != null) return sheep;

            // 5. Luna Light Perfume: Has GY banish effect to search!
            var perfume = candidates.FirstOrDefault(c => c != null && c.Id == LunaExecutor.CardId.LunaLightPerfume);
            if (perfume != null) return perfume;

            var safe = candidates.Where(c => c != null &&
                !_exec.IsAceCard(c) &&
                !c.IsCode(LunaExecutor.CardId.AshBlossom, LunaExecutor.CardId.AshBlossomAlt, LunaExecutor.CardId.DrollAndLockBird, LunaExecutor.CardId.MulcharmyFuwalos, LunaExecutor.CardId.MulcharmyPurulia, LunaExecutor.CardId.DominusImpulse) &&
                !c.IsCode(LunaExecutor.CardId.Polymerization, LunaExecutor.CardId.LunalightFusion, LunaExecutor.CardId.ApexPolymerization)
            ).OrderBy(c => c.Attack).FirstOrDefault();

            return safe ?? candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }
    }

    public class LunaThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly LunaExecutor _exec;

        public LunaThreatEvaluator(LunaExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null || card.Controller == 0) return 0;
            int score = 0;

            int id = card.Id;
            if (CardIntelligence.IsKnownNegator(id)) score += 10000;
            if (CardIntelligence.IsFloodgate(id)) score += 8000;

            if (card.HasType(CardType.Fusion | CardType.Synchro | CardType.Xyz | CardType.Link)) score += 5000;
            if (card.Attack >= 3000) score += 4000;
            else if (card.Attack >= 2500) score += 3000;
            else if (card.Attack >= 2000) score += 2000;

            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null || card.Controller == 0) return false;
            return card.Attack >= 3000 || CardIntelligence.IsKnownNegator(card.Id);
        }
    }
}
