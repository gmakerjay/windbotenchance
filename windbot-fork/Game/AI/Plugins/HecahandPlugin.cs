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
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: HecahandPlugin
    //  Illusion Control & Board Steal Direct OTK Engine
    // ═══════════════════════════════════════════════════════════════
    public class HecahandPlugin : DeckPluginBase
    {
        private readonly HecahandExecutor _exec;

        public override string DeckName => "Hecahand";

        public HecahandStrategy StrategyImpl { get; }
        public HecahandMaterialEvaluator MaterialImpl { get; }
        public HecahandThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public HecahandPlugin(HecahandExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new HecahandStrategy(exec);
            MaterialImpl = new HecahandMaterialEvaluator(exec);
            ThreatImpl = new HecahandThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class HecahandStrategy : IDeckStrategy
    {
        private readonly HecahandExecutor _exec;

        public HecahandStrategy(HecahandExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Extra Deck Fusion targets
            if (candidates.Any(c => c != null && c.Location == CardLocation.Extra))
            {
                // Dandalos: Steals monster + gives ALL Heca fusions and stolen monsters direct attack!
                if (!_exec.Bot.HasInMonstersZone(HecahandExecutor.CardId.HecahandsDandalos))
                {
                    var dandalos = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsDandalos);
                    if (dandalos != null) return dandalos;
                }

                // Xeno: 3400 ATK, quick banish Extra Deck monster
                if (!_exec.Bot.HasInMonstersZone(HecahandExecutor.CardId.HecahandsXeno))
                {
                    var xeno = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsXeno);
                    if (xeno != null) return xeno;
                }

                var jauzah = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsJauzah);
                if (jauzah != null) return jauzah;

                var dandalosBackup = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsDandalos);
                if (dandalosBackup != null) return dandalosBackup;
            }

            // Deck Summons (via Hecahands Ibtel e1)
            if (candidates.Any(c => c != null && c.Location == CardLocation.Deck))
            {
                // Prefer Gaigas (excavate 3 and steal) > Breus (steal from hand) > Yadel
                var gaigas = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsGaigas);
                if (gaigas != null) return gaigas;

                var breus = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsBreus);
                if (breus != null) return breus;

                var yadel = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsYadel);
                if (yadel != null) return yadel;
            }

            // GY Revives (via Ibtel e2, Monster Reborn, Illusion Gate)
            if (candidates.Any(c => c != null && c.Location == CardLocation.Grave))
            {
                // First: Dandalos for direct attack lethal push
                var dandalos = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsDandalos);
                if (dandalos != null) return dandalos;

                var xeno = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsXeno);
                if (xeno != null) return xeno;

                // High ATK opponent monster in GY
                var oppBoss = candidates.Where(c => c != null && c.Controller == 1 && c.Attack >= 2500)
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();
                if (oppBoss != null) return oppBoss;

                var jauzah = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsJauzah);
                if (jauzah != null) return jauzah;

                var gaigas = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsGaigas);
                if (gaigas != null) return gaigas;

                var breus = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsBreus);
                if (breus != null) return breus;
            }

            return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Triple Tactics Thrust search:
            if (context != null && context.IsCode(HecahandExecutor.CardId.TripleTacticsThrust))
            {
                bool oppHasOwned = _exec.Enemy.GetMonsters().Any(c => c != null && c.Owner == 0);
                bool hasKaijuInHand = _exec.Bot.HasInHand(HecahandExecutor.CardId.LavaGolem) || _exec.Bot.HasInHand(HecahandExecutor.CardId.Gameciel);

                if (oppHasOwned || hasKaijuInHand)
                {
                    var botHerder = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.BotHerder);
                    if (botHerder != null) return botHerder;
                }

                if (_exec.Enemy.GetMonsterCount() > 0)
                {
                    var coh = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.ChangeOfHeart);
                    if (coh != null) return coh;
                }

                var hiddenHeca = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.TheHiddenHecahands);
                if (hiddenHeca != null) return hiddenHeca;

                var gate = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.IllusionGate);
                if (gate != null) return gate;
            }

            // The Hidden Hecahands search:
            if (context != null && context.IsCode(HecahandExecutor.CardId.TheHiddenHecahands))
            {
                bool hasMakibel = _exec.Bot.HasInHand(HecahandExecutor.CardId.HecahandsMakibel);
                bool hasIbtel = _exec.Bot.HasInHand(HecahandExecutor.CardId.HecahandsIbtel);
                bool hasYadel = _exec.Bot.HasInHand(HecahandExecutor.CardId.HecahandsYadel);
                bool hasMaterials = _exec.Bot.Hand.Concat(_exec.Bot.MonsterZone).Any(c => c != null && (c.Id == HecahandExecutor.CardId.HecahandsIbtel || c.Id == HecahandExecutor.CardId.HecahandsYadel || c.Id == HecahandExecutor.CardId.NightmareApprentice || (c.Location == CardLocation.MonsterZone && c.Owner == 1)));

                if (hasMaterials && !hasMakibel)
                {
                    var makibel = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsMakibel);
                    if (makibel != null) return makibel;
                }

                if (!hasIbtel)
                {
                    var ibtel = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsIbtel);
                    if (ibtel != null) return ibtel;
                }

                if (!hasYadel)
                {
                    var yadel = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsYadel);
                    if (yadel != null) return yadel;
                }

                var gaigas = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsGaigas);
                if (gaigas != null) return gaigas;

                var breus = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsBreus);
                if (breus != null) return breus;
            }

            // Nightmare Apprentice search:
            if (context != null && context.IsCode(HecahandExecutor.CardId.NightmareApprentice))
            {
                if (!_exec.Bot.HasInHand(HecahandExecutor.CardId.HecahandsIbtel))
                {
                    var ibtel = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsIbtel);
                    if (ibtel != null) return ibtel;
                }

                if (!_exec.Bot.HasInHand(HecahandExecutor.CardId.HecahandsMakibel))
                {
                    var makibel = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsMakibel);
                    if (makibel != null) return makibel;
                }

                if (!_exec.Bot.HasInHand(HecahandExecutor.CardId.HecahandsYadel))
                {
                    var yadel = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsYadel);
                    if (yadel != null) return yadel;
                }

                var gaigas = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsGaigas);
                if (gaigas != null) return gaigas;
            }

            // Hecahands Yadel search (Spell/Trap):
            if (context != null && context.IsCode(HecahandExecutor.CardId.HecahandsYadel))
            {
                if (!_exec.Bot.HasInHand(HecahandExecutor.CardId.TheHiddenHecahands))
                {
                    var hiddenHeca = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.TheHiddenHecahands);
                    if (hiddenHeca != null) return hiddenHeca;
                }

                var tartaros = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsTartaros);
                if (tartaros != null) return tartaros;
            }

            // Hecahands Jauzah search (Deck/GY):
            if (context != null && context.IsCode(HecahandExecutor.CardId.HecahandsJauzah))
            {
                if (!_exec.Bot.HasInHand(HecahandExecutor.CardId.TheHiddenHecahands))
                {
                    var hiddenHeca = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.TheHiddenHecahands);
                    if (hiddenHeca != null) return hiddenHeca;
                }

                if (!_exec.Bot.HasInHand(HecahandExecutor.CardId.HecahandsMakibel))
                {
                    var makibel = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsMakibel);
                    if (makibel != null) return makibel;
                }

                if (!_exec.Bot.HasInHand(HecahandExecutor.CardId.HecahandsTartaros) && !_exec.Bot.HasInSpellZone(HecahandExecutor.CardId.HecahandsTartaros))
                {
                    var tartaros = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsTartaros);
                    if (tartaros != null) return tartaros;
                }

                var ibtel = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsIbtel);
                if (ibtel != null) return ibtel;
            }

            return candidates.FirstOrDefault();
        }

        public ClientCard PickFoolishGraveTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Ibtel: Has GY effect to revive an Illusion monster!
            var ibtel = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsIbtel);
            if (ibtel != null) return ibtel;

            // 2. Yadel: Has GY effect to set Hecahands Spell/Trap!
            var yadel = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsYadel);
            if (yadel != null) return yadel;

            // 3. Makibel: Recycles to hand when fusion monster is sent to GY!
            var makibel = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsMakibel);
            if (makibel != null) return makibel;

            return candidates.FirstOrDefault();
        }
    }

    public class HecahandMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly HecahandExecutor _exec;

        public HecahandMaterialEvaluator(HecahandExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard c)
        {
            if (c == null) return 999;

            // Stolen monster on field: BEST material to fuse away (removes from opponent permanently)
            if (c.Location == CardLocation.MonsterZone && c.Owner == 1) return 10;

            // Makibel: Recycles to hand
            if (c.Id == HecahandExecutor.CardId.HecahandsMakibel) return 20;

            // Ibtel: Has GY revive
            if (c.Id == HecahandExecutor.CardId.HecahandsIbtel) return 30;

            // Yadel: Has GY set S/T
            if (c.Id == HecahandExecutor.CardId.HecahandsYadel) return 40;

            // Nightmare Apprentice: Already used on field
            if (c.Id == HecahandExecutor.CardId.NightmareApprentice) return 50;

            // Gaigas / Breus
            if (c.Id == HecahandExecutor.CardId.HecahandsGaigas || c.Id == HecahandExecutor.CardId.HecahandsBreus) return 80;

            // Handtraps: avoid using as material
            if (c.IsCode(HecahandExecutor.CardId.MaxxC, HecahandExecutor.CardId.MulcharmyFuwalos, HecahandExecutor.CardId.MulcharmyPurulia, HecahandExecutor.CardId.DrollAndLockBird))
                return 800;

            // Ace cards: heavily protected
            if (_exec.IsAceCard(c)) return 950;

            return 100;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.Where(c => c != null).OrderBy(GetMaterialCost).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Preferred cards to send to GY as cost:
            // 1. Ibtel (has GY revive)
            var ibtel = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsIbtel);
            if (ibtel != null) return ibtel;

            // 2. Yadel (has GY set S/T)
            var yadel = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsYadel);
            if (yadel != null) return yadel;

            // 3. Makibel (recycles to hand on fusion sent to GY)
            var makibel = candidates.FirstOrDefault(c => c != null && c.Id == HecahandExecutor.CardId.HecahandsMakibel);
            if (makibel != null) return makibel;

            // 4. Stolen monster on our field with low value
            var stolenFodder = candidates.FirstOrDefault(c => c != null && c.Location == CardLocation.MonsterZone && c.Owner == 1 && c.Attack < 2000 && !c.IsFloodgate());
            if (stolenFodder != null) return stolenFodder;

            // 5. Nightmare Apprentice already on field
            var apprentice = candidates.FirstOrDefault(c => c != null && c.Location == CardLocation.MonsterZone && c.Id == HecahandExecutor.CardId.NightmareApprentice);
            if (apprentice != null) return apprentice;

            var safe = candidates.Where(c => c != null &&
                !_exec.IsAceCard(c) &&
                !c.IsCode(HecahandExecutor.CardId.MaxxC, HecahandExecutor.CardId.MulcharmyFuwalos, HecahandExecutor.CardId.MulcharmyPurulia, HecahandExecutor.CardId.DrollAndLockBird) &&
                !c.IsCode(HecahandExecutor.CardId.ChangeOfHeart, HecahandExecutor.CardId.BotHerder)
            ).OrderBy(c => c.Attack).FirstOrDefault();

            return safe ?? candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }
    }

    public class HecahandThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly HecahandExecutor _exec;

        public HecahandThreatEvaluator(HecahandExecutor exec) => _exec = exec;

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
