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
    // ═══════════════════════════════════════════════════════════════════════════
    // DECOUPLED DOMAIN PLUGIN ARCHITECTURE: DinosmasherPlugin
    // Implements Strategy, MaterialEvaluator, ThreatEvaluator, and TokenTactics
    // for Modernized Dinosaur (Lost World, Token Clog, Target-Lock, UCT OTK)
    // ═══════════════════════════════════════════════════════════════════════════
    public class DinosmasherPlugin : DeckPluginBase
    {
        private readonly DinosmasherExecutor _exec;

        public override string DeckName => "Dinosmasher";

        public DinosmasherStrategy StrategyImpl { get; }
        public DinosmasherMaterialEvaluator MaterialImpl { get; }
        public DinosmasherThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public DinosmasherPlugin(DinosmasherExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new DinosmasherStrategy(exec);
            MaterialImpl = new DinosmasherMaterialEvaluator(exec);
            ThreatImpl = new DinosmasherThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class DinosmasherStrategy : IDeckStrategy
    {
        private readonly DinosmasherExecutor _exec;
        public DinosmasherStrategy(DinosmasherExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Double Evolution Pill: Prioritize Ultimate Conductor Tyranno
            var uct = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.UltimateConductorTyranno);
            if (uct != null && !_exec.Bot.HasInMonstersZone(DinosmasherExecutor.CardId.UltimateConductorTyranno))
                return uct;

            // 2. Babycerasaurus / Petiteranodon float triggers:
            // If no Oviraptor on field -> summon Oviraptor immediately!
            if (!_exec.Bot.HasInMonstersZone(DinosmasherExecutor.CardId.SouleatingOviraptor))
            {
                var ovi = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.SouleatingOviraptor);
                if (ovi != null) return ovi;
            }

            // 3. Animadorned Archosaur if Pill not searched yet
            if (!_exec.Bot.HasInHand(DinosmasherExecutor.CardId.DoubleEvolutionPill) &&
                !_exec.Bot.HasInMonstersZone(DinosmasherExecutor.CardId.AnimadornedArchosaur))
            {
                var arch = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.AnimadornedArchosaur);
                if (arch != null) return arch;
            }

            // 4. Xeno Meteorus for Level 6 Tuner / Rank 6 climb into Lars
            var xeno = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.XenoMeteorus);
            if (xeno != null && !_exec.Bot.HasInMonstersZone(DinosmasherExecutor.CardId.XenoMeteorus))
                return xeno;

            // 5. Frostosaurus (Level 6 Normal Dino summoned by Xeno Meteorus to make Lars)
            var frosto = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.Frostosaurus);
            if (frosto != null && _exec.Bot.HasInMonstersZone(DinosmasherExecutor.CardId.XenoMeteorus))
                return frosto;

            // 6. Giant Rex for Level 4 material / 2000 beatstick
            var rex = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.GiantRex);
            if (rex != null) return rex;

            // 7. Megalosmasher X (Level 4 Normal Dino for Rank 4 Dolkka/Laggia)
            var mega = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.MegalosmasherX);
            if (mega != null) return mega;

            // 8. Babycerasaurus (chainable fodder)
            var baby = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.Babycerasaurus);
            if (baby != null) return baby;

            // 9. Pankratops for spot removal
            var pank = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.Pankratops);
            if (pank != null) return pank;

            return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard contextCard)
        {
            if (candidates == null || candidates.Count == 0) return null;

            bool hasOviraptor = _exec.Bot.HasInHand(DinosmasherExecutor.CardId.SouleatingOviraptor) ||
                                _exec.Bot.HasInMonstersZone(DinosmasherExecutor.CardId.SouleatingOviraptor);
            bool hasMisc = _exec.Bot.HasInHand(DinosmasherExecutor.CardId.Miscellaneousaurus);
            bool hasBaby = _exec.Bot.HasInHand(DinosmasherExecutor.CardId.Babycerasaurus) ||
                           _exec.Bot.HasInMonstersZone(DinosmasherExecutor.CardId.Babycerasaurus);
            bool hasPill = _exec.Bot.HasInHand(DinosmasherExecutor.CardId.DoubleEvolutionPill);
            bool hasUCT = _exec.Bot.HasInHand(DinosmasherExecutor.CardId.UltimateConductorTyranno) ||
                          _exec.Bot.HasInMonstersZone(DinosmasherExecutor.CardId.UltimateConductorTyranno);

            // 1. Archosaur search: Double Evolution Pill
            var pill = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.DoubleEvolutionPill);
            if (pill != null) return pill;

            // 2. Souleating Oviraptor (Primary normal summon / engine starter)
            if (!hasOviraptor)
            {
                var ovi = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.SouleatingOviraptor);
                if (ovi != null) return ovi;
            }

            // 3. Miscellaneousaurus (Hand protection shield during Main Phase 1)
            if (!hasMisc)
            {
                var misc = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.Miscellaneousaurus);
                if (misc != null) return misc;
            }

            // 4. Babycerasaurus (Fodder to pop with Oviraptor / Archosaur / UCT)
            if (!hasBaby)
            {
                var baby = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.Babycerasaurus);
                if (baby != null) return baby;
            }

            // 5. Ultimate Conductor Tyranno (Boss finisher)
            if (!hasUCT && hasPill)
            {
                var boss = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.UltimateConductorTyranno);
                if (boss != null) return boss;
            }

            // 6. Pankratops (Quick spot removal)
            var pank = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.Pankratops);
            if (pank != null) return pank;

            return candidates.FirstOrDefault();
        }
    }

    public class DinosmasherMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly DinosmasherExecutor _exec;
        public DinosmasherMaterialEvaluator(DinosmasherExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard c)
        {
            if (c == null) return 0;

            // Tokens: Zero cost, preferred fodder
            if (c.HasType(CardType.Token) || c.Id == DinosmasherExecutor.CardId.JurraeggToken || c.Id == DinosmasherExecutor.CardId.OjamaToken)
                return 1;

            // Archosaur after effect used: Low cost
            if (c.Id == DinosmasherExecutor.CardId.AnimadornedArchosaur)
                return 2;

            // Giant Rex: Low cost (triggers when banished)
            if (c.Id == DinosmasherExecutor.CardId.GiantRex)
                return 2;

            // Normal Dinosaurs (Frostosaurus, Megalosmasher)
            if (c.Id == DinosmasherExecutor.CardId.Frostosaurus || c.Id == DinosmasherExecutor.CardId.MegalosmasherX)
                return 3;

            // Babies: Destroying them is actually beneficial (Cost = 4)
            if (c.Id == DinosmasherExecutor.CardId.Babycerasaurus || c.Id == DinosmasherExecutor.CardId.Petiteranodon)
                return 4;

            // Oviraptor: High cost if we still need its effect
            if (c.Id == DinosmasherExecutor.CardId.SouleatingOviraptor)
                return 8;

            // Extra Deck Bosses & Ace Monsters: NEVER sacrifice or destroy!
            if (c.Id == DinosmasherExecutor.CardId.UltimateConductorTyranno ||
                c.Id == DinosmasherExecutor.CardId.EvolzarLars ||
                c.Id == DinosmasherExecutor.CardId.EvolzarLaggia ||
                c.Id == DinosmasherExecutor.CardId.EvolzarDolkka ||
                c.Id == DinosmasherExecutor.CardId.AccesscodeTalker ||
                c.Id == DinosmasherExecutor.CardId.SPLittleKnight)
                return 100;

            return 10;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.OrderBy(GetMaterialCost).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Miscellaneousaurus (immediately active in GY)
            var misc = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.Miscellaneousaurus);
            if (misc != null) return misc;

            // 2. Giant Rex (can be banished later for free revival)
            var rex = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.GiantRex);
            if (rex != null) return rex;

            // 3. Survival's End (has graveyard banish-pop effect)
            var surv = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.SurvivalEnd);
            if (surv != null) return surv;

            // 4. Babycerasaurus / Petiteranodon
            var baby = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.Babycerasaurus || c.Id == DinosmasherExecutor.CardId.Petiteranodon);
            if (baby != null) return baby;

            // 5. Frostosaurus / Megalosmasher
            var normal = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.Frostosaurus || c.Id == DinosmasherExecutor.CardId.MegalosmasherX);
            if (normal != null) return normal;

            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            // Lost World destruction substitute: STRICTLY destroy Babycerasaurus or Petiteranodon from DECK!
            // This triggers their GY effect to Special Summon a new Dinosaur from deck!
            var deckBaby = candidates?.FirstOrDefault(c => c.Location == CardLocation.Deck && c.Id == DinosmasherExecutor.CardId.Babycerasaurus);
            if (deckBaby != null) return deckBaby;

            var deckPetite = candidates?.FirstOrDefault(c => c.Location == CardLocation.Deck && c.Id == DinosmasherExecutor.CardId.Petiteranodon);
            if (deckPetite != null) return deckPetite;

            var handBaby = candidates?.FirstOrDefault(c => c.Location == CardLocation.Hand &&
                (c.Id == DinosmasherExecutor.CardId.Babycerasaurus || c.Id == DinosmasherExecutor.CardId.Petiteranodon));
            if (handBaby != null) return handBaby;

            return candidates?.OrderBy(GetMaterialCost).FirstOrDefault();
        }

        public ClientCard PickUCTPopTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Hand Babycerasaurus or Petiteranodon (triggers float without losing field presence!)
            var handBaby = candidates.FirstOrDefault(c => c.Location == CardLocation.Hand &&
                (c.Id == DinosmasherExecutor.CardId.Babycerasaurus || c.Id == DinosmasherExecutor.CardId.Petiteranodon));
            if (handBaby != null) return handBaby;

            // 2. Field Babycerasaurus or Petiteranodon
            var fieldBaby = candidates.FirstOrDefault(c => c.Location == CardLocation.MonsterZone &&
                (c.Id == DinosmasherExecutor.CardId.Babycerasaurus || c.Id == DinosmasherExecutor.CardId.Petiteranodon));
            if (fieldBaby != null) return fieldBaby;

            // 3. Jurraegg Token or Archosaur whose effect has resolved
            var fodder = candidates.FirstOrDefault(c => c.Location == CardLocation.MonsterZone &&
                (c.Id == DinosmasherExecutor.CardId.AnimadornedArchosaur || c.HasType(CardType.Token)));
            if (fodder != null) return fodder;

            // NEVER pop Evolzar bosses or UCT itself!
            return null;
        }

        public IList<ClientCard> PickDoubleEvolutionPillBanish(IList<ClientCard> candidates)
        {
            if (candidates == null) return new List<ClientCard>();

            // Must pick 1 Dinosaur and 1 Non-Dinosaur
            ClientCard dinoTarget = null;
            ClientCard nonDinoTarget = null;

            // Dinosaur priority: Giant Rex in GY (floats on banish!) > Archosaur in GY > Normal Dino in GY
            var gyDinos = candidates.Where(c => c.HasRace(CardRace.Dinosaur)).ToList();
            dinoTarget = gyDinos.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.GiantRex && c.Location == CardLocation.Grave)
                      ?? gyDinos.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.AnimadornedArchosaur && c.Location == CardLocation.Grave)
                      ?? gyDinos.FirstOrDefault(c => c.Location == CardLocation.Grave && (c.Id == DinosmasherExecutor.CardId.Frostosaurus || c.Id == DinosmasherExecutor.CardId.MegalosmasherX))
                      ?? gyDinos.FirstOrDefault(c => c.Location == CardLocation.Grave && c.Id != DinosmasherExecutor.CardId.UltimateConductorTyranno)
                      ?? gyDinos.FirstOrDefault(c => c.Id != DinosmasherExecutor.CardId.UltimateConductorTyranno);

            // Non-Dinosaur priority: Linkuriboh in GY > Secure Gardna in GY > Ash Blossom in GY
            var nonDinos = candidates.Where(c => !c.HasRace(CardRace.Dinosaur)).ToList();
            nonDinoTarget = nonDinos.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.Linkuriboh && c.Location == CardLocation.Grave)
                         ?? nonDinos.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.SecureGardna && c.Location == CardLocation.Grave)
                         ?? nonDinos.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.AshBlossom && c.Location == CardLocation.Grave)
                         ?? nonDinos.FirstOrDefault(c => c.Location == CardLocation.Grave)
                         ?? nonDinos.FirstOrDefault();

            var result = new List<ClientCard>();
            if (dinoTarget != null) result.Add(dinoTarget);
            if (nonDinoTarget != null) result.Add(nonDinoTarget);

            return result;
        }
    }

    public class DinosmasherThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly DinosmasherExecutor _exec;
        public DinosmasherThreatEvaluator(DinosmasherExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard c)
        {
            if (c == null) return 0;
            int score = 0;

            // Mass Backrow Wipes
            if (c.Id == 18144506 || c.Id == 14532163 || c.Id == 15693423) score += 90;

            // Continuous Floodgates
            if (c.Id == 82732047 || c.Id == 82732705 || c.Id == 30241314) score += 85;

            if (c.IsFaceup() && c.HasType(CardType.Monster))
            {
                if (c.Attack >= 3000) score += 30;
                if (CardIntelligence.IsKnownNegator(c.Id)) score += 50;
                if (CardIntelligence.IsHighThreatChokepoint(c.Id)) score += 40;
            }

            return score;
        }

        public bool IsEmergencyThreat(ClientCard c)
        {
            if (c == null) return false;
            return EvaluateThreatScore(c) >= 70;
        }
    }
}
