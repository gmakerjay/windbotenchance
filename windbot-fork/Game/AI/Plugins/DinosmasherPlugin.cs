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

            // 1. Double Evolution Pill: Prioritize Ultimate Conductor Tyranno UNCONDITIONALLY!
            var uct = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.UltimateConductorTyranno);
            if (uct != null) return uct;

            // 2. Souleating Oviraptor: ABSOLUTE HIGHEST PRIORITY for Baby float triggers!
            // Searches or dumps a Dinosaur on Normal OR Special Summon!
            var ovi = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.SouleatingOviraptor);
            if (ovi != null) return ovi;

            // 3. Giant Rex (2000 ATK Level 4 beatstick / Rank 4 material with Oviraptor)
            var rex = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.GiantRex);
            if (rex != null) return rex;

            // 4. Pankratops for 2600 ATK / Quick spot removal
            var pank = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.Pankratops);
            if (pank != null) return pank;

            // 5. Dogoran, the Mad Flame Kaiju (3000 ATK Dinosaur beatstick)
            var dogo = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.Dogoran);
            if (dogo != null) return dogo;

            // 6. Animadorned Archosaur if Pill not in hand yet
            if (!_exec.Bot.HasInHand(DinosmasherExecutor.CardId.DoubleEvolutionPill) &&
                !_exec.Bot.HasInMonstersZone(DinosmasherExecutor.CardId.AnimadornedArchosaur))
            {
                var arch = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.AnimadornedArchosaur);
                if (arch != null) return arch;
            }

            // 7. Xeno Meteorus for Level 6 Tuner / Rank 6 Lars climb
            var xeno = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.XenoMeteorus);
            if (xeno != null && !_exec.Bot.HasInMonstersZone(DinosmasherExecutor.CardId.XenoMeteorus))
                return xeno;

            // 8. Frostosaurus (Level 6 2600 ATK Normal Dino)
            var frosto = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.Frostosaurus);
            if (frosto != null) return frosto;

            // 9. Babycerasaurus / Petiteranodon (lowest priority fallback)
            var baby = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.Babycerasaurus || c.Id == DinosmasherExecutor.CardId.Petiteranodon);
            if (baby != null) return baby;

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

            // 0. Emergency Kaiju Search: If opponent controls an untargetable tower / boss (Dragon Knight, Crystal Wing, Buster)
            // and we do not have Dogoran in hand, search Dogoran Kaiju immediately!
            if (candidates.Any(c => c.Id == DinosmasherExecutor.CardId.Dogoran) &&
                !_exec.Bot.HasInHand(DinosmasherExecutor.CardId.Dogoran) &&
                _exec.Enemy.GetMonsters().Any(m => m.IsFaceup() && (_exec.Plugin.ThreatImpl.EvaluateThreatScore(m) >= 70 || m.Attack >= 3000)))
            {
                var dogo = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.Dogoran);
                if (dogo != null) return dogo;
            }

            // 1. Archosaur search: Double Evolution Pill
            var pill = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.DoubleEvolutionPill);
            if (pill != null) return pill;

            // 1.5 Ground Xeno search (always prioritize Xeno Meteorus over Frostosaurus):
            if (candidates.Any(c => c.Id == DinosmasherExecutor.CardId.XenoMeteorus))
            {
                var xeno = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.XenoMeteorus);
                if (xeno != null && !_exec.Bot.HasInHand(DinosmasherExecutor.CardId.XenoMeteorus)) return xeno;
                var frosto = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.Frostosaurus);
                if (frosto != null) return frosto;
            }

            // 1.8 Souleating Oviraptor contextual search:
            if (contextCard != null && contextCard.Id == DinosmasherExecutor.CardId.SouleatingOviraptor)
            {
                // If Lost World is active, Babycerasaurus is best so Oviraptor can pop Token and deck-pop Baby!
                if (_exec.Bot.HasInSpellZone(DinosmasherExecutor.CardId.LostWorld))
                {
                    var baby = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.Babycerasaurus);
                    if (baby != null) return baby;
                }
                // Otherwise Misc is highest priority to protect Dinos and summon Archosaur
                if (!hasMisc)
                {
                    var misc = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.Miscellaneousaurus);
                    if (misc != null) return misc;
                }
                var baby2 = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.Babycerasaurus);
                if (baby2 != null) return baby2;
                var rex = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.GiantRex);
                if (rex != null) return rex;
            }

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
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Lost World destruction substitute: STRICTLY destroy Babycerasaurus or Petiteranodon from DECK!
            var deckBaby = candidates.FirstOrDefault(c => c.Location == CardLocation.Deck && 
                (c.Id == DinosmasherExecutor.CardId.Babycerasaurus || c.Id == DinosmasherExecutor.CardId.Petiteranodon));
            if (deckBaby != null) return deckBaby;

            // 2. Token on field (Jurraegg Token, Ojama Token)
            var token = candidates.FirstOrDefault(c => c.HasType(CardType.Token));
            if (token != null) return token;

            // 3. Babycerasaurus or Petiteranodon anywhere (Hand or Field - triggers float upon destruction!)
            var baby = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.Babycerasaurus || c.Id == DinosmasherExecutor.CardId.Petiteranodon);
            if (baby != null) return baby;

            // 4. Giant Rex (can be banished later)
            var rex = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.GiantRex);
            if (rex != null) return rex;

            // 5. Survival's End on field
            var surv = candidates.FirstOrDefault(c => c.Id == DinosmasherExecutor.CardId.SurvivalEnd);
            if (surv != null) return surv;

            // 6. Safe fallbacks: NEVER destroy Frostosaurus, UCT, or Extra Deck Bosses!
            var safe = candidates.Where(c => c.Id != DinosmasherExecutor.CardId.Frostosaurus && 
                                             c.Id != DinosmasherExecutor.CardId.UltimateConductorTyranno &&
                                             c.Id != DinosmasherExecutor.CardId.EvolzarLars &&
                                             c.Id != DinosmasherExecutor.CardId.EvolzarDolkka &&
                                             c.Id != DinosmasherExecutor.CardId.EvolzarLaggia &&
                                             c.Id != DinosmasherExecutor.CardId.XenoMeteorus)
                                 .OrderBy(GetMaterialCost)
                                 .FirstOrDefault();
            if (safe != null) return safe;

            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
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

            // Continuous Floodgates & Engine Pillars
            // 48680970 = Eternal Soul (DM: wipes all DM monsters when popped!)
            // 82732705 = Skill Drain
            // 66399653 = Union Hangar
            // 47222536 = Dark Magical Circle
            if (c.Id == 48680970) score += 120;
            if (c.Id == 82732705) score += 110;
            if (c.Id == 66399653) score += 95;
            if (c.Id == 47222536) score += 80;
            if (c.Id == 82732705 || c.Id == 30241314) score += 85;

            // Mass Backrow Wipes
            if (c.Id == 18144506 || c.Id == 14532163 || c.Id == 15693423) score += 90;

            if (c.IsFaceup() && c.HasType(CardType.Monster))
            {
                // Bosses & Towers
                if (c.Id == 41721210) score += 150; // Dark Magician the Dragon Knight (lockdown)
                if (c.Id == 50954680) score += 130; // Crystal Wing Synchro Dragon (indestructible negator)
                if (c.Id == 01561110) score += 140; // ABC-Dragon Buster (quick banish)
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
