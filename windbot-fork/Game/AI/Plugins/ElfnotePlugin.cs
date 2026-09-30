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
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: ElfnotePlugin
    //  Decouples Domain Rules, Strategy, and Material Evaluation
    //  for Elfnote / Power Patron / Ars Magna / Theorealize Strategy
    // ═══════════════════════════════════════════════════════════════
    public class ElfnotePlugin : DeckPluginBase
    {
        private readonly ElfnoteExecutor _exec;

        public override string DeckName => "Elfnote";

        public ElfnoteStrategy StrategyImpl { get; }
        public ElfnoteMaterialEvaluator MaterialImpl { get; }
        public ElfnoteThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public ElfnotePlugin(ElfnoteExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new ElfnoteStrategy(exec);
            MaterialImpl = new ElfnoteMaterialEvaluator(exec);
            ThreatImpl = new ElfnoteThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class ElfnoteStrategy : IDeckStrategy
    {
        private readonly ElfnoteExecutor _exec;
        public ElfnoteStrategy(ElfnoteExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Context: Regina summoning from Deck to field
            if (_exec.CurrentCard != null && _exec.CurrentCard.IsCode(ElfnoteExecutor.CardId.ElfnoteRegina))
            {
                // Must summon Tuner (ElfnotePowerPatron) first if we don't have one on field!
                bool hasTuner = _exec.Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.IsTuner() || c.IsCode(ElfnoteExecutor.CardId.ElfnotePowerPatron)));
                if (!hasTuner)
                {
                    var tuner = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ElfnotePowerPatron));
                    if (tuner != null) return tuner;
                }
            }

            int[] priorities = {
                ElfnoteExecutor.CardId.ArtmageDiactorus,
                ElfnoteExecutor.CardId.BaronneDeFleur,
                ElfnoteExecutor.CardId.JunoraThePowerPatronOfTuning,
                ElfnoteExecutor.CardId.ElfnoteJunePride,
                ElfnoteExecutor.CardId.MedicuriusThePowerPatronOfIllusions,
                ElfnoteExecutor.CardId.CrystalWingSynchroDragon,
                ElfnoteExecutor.CardId.ChaosAngel,
                ElfnoteExecutor.CardId.PSYFramelordOmega,
                ElfnoteExecutor.CardId.RavenousCrocodragonArchethys,
                ElfnoteExecutor.CardId.ElfnoteSeraphimStrelitzia,
                ElfnoteExecutor.CardId.FADawnDragster,
                ElfnoteExecutor.CardId.SPLittleKnight,
                ElfnoteExecutor.CardId.ElfnotePowerPatron,
                ElfnoteExecutor.CardId.ArsMagnaOfInfinityAndFinity,
                ElfnoteExecutor.CardId.ArsMagnaOfPurificationAndCorruption,
                ElfnoteExecutor.CardId.TheorealizeMedius,
                ElfnoteExecutor.CardId.MediusThePure,
                ElfnoteExecutor.CardId.ElfnoteRegina,
                ElfnoteExecutor.CardId.ElfnoteLucina,
                ElfnoteExecutor.CardId.ElfnoteTinia,
                ElfnoteExecutor.CardId.ElfnoteFortuna,
                ElfnoteExecutor.CardId.JetSynchron
            };

            foreach (int id in priorities)
            {
                var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id));
                if (match != null) return match;
            }

            return candidates.FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Context 1: Terminus (Requires DARK Fairy monster)
            if (context != null && context.IsCode(ElfnoteExecutor.CardId.UnleashedPowerPatronPortalTerminus))
            {
                // Medius the Pure is DARK Fairy and summons Power Patron / Tuner from Deck!
                var pureMedius = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.MediusThePure));
                if (pureMedius != null) return pureMedius;

                var tm = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.TheorealizeMedius));
                if (tm != null) return tm;
            }

            // Context 2: Medius the Pure (Searches or Summons Power Patron monster)
            if (context != null && context.IsCode(ElfnoteExecutor.CardId.MediusThePure))
            {
                // If we have Ars Magna in hand or banished, prioritize Theorealize Medius to cheat Diactorus!
                bool hasArsMagna = _exec.Bot.Hand.Any(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ArsMagnaOfInfinityAndFinity))
                                || _exec.Bot.Banished.Any(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ArsMagnaOfInfinityAndFinity));
                if (hasArsMagna)
                {
                    var tm = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.TheorealizeMedius));
                    if (tm != null) return tm;
                }

                var junordo = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.PowerPatronShadowSpiritJunordo));
                if (junordo != null) return junordo;

                var elfPatron = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ElfnotePowerPatron));
                if (elfPatron != null) return elfPatron;
            }

            // Context 3: Theorealize Medius (Searches Theorealize Spell/Trap)
            if (context != null && context.IsCode(ElfnoteExecutor.CardId.TheorealizeMedius))
            {
                var pastLull = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.TheorealizePastLull));
                if (pastLull != null) return pastLull;
            }

            // Context 4: Ars Magna of Infinity and Finity (Searches non-Warrior Ars Magna)
            if (context != null && context.IsCode(ElfnoteExecutor.CardId.ArsMagnaOfInfinityAndFinity))
            {
                var purif = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ArsMagnaOfPurificationAndCorruption));
                if (purif != null) return purif;
            }

            // Context 5: Ars Magna of Purification and Corruption (Searches Ars Magna Spell/Trap)
            if (context != null && context.IsCode(ElfnoteExecutor.CardId.ArsMagnaOfPurificationAndCorruption))
            {
                var citrinitas = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ArsMagnaCitrinitas));
                if (citrinitas != null) return citrinitas;
            }

            // Context 6: Ars Magna - "Citrinitas" (Searches Medius or Ars Magna monster)
            if (context != null && context.IsCode(ElfnoteExecutor.CardId.ArsMagnaCitrinitas))
            {
                bool hasTheorealizeMedius = _exec.Bot.Hand.Any(c => c != null && c.IsCode(ElfnoteExecutor.CardId.TheorealizeMedius))
                                         || _exec.Bot.GetMonsters().Any(c => c != null && c.IsCode(ElfnoteExecutor.CardId.TheorealizeMedius));
                if (!hasTheorealizeMedius)
                {
                    var tm = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.TheorealizeMedius));
                    if (tm != null) return tm;
                }

                var pureMedius = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.MediusThePure));
                if (pureMedius != null) return pureMedius;

                var infinity = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ArsMagnaOfInfinityAndFinity));
                if (infinity != null) return infinity;
            }

            // Context 7: Purification Power Patron (Searches Theorealize card)
            if (context != null && context.IsCode(ElfnoteExecutor.CardId.PurificationPowerPatron))
            {
                var pastLull = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.TheorealizePastLull));
                if (pastLull != null) return pastLull;
            }

            // Context 8: Lucina (Searches Elfnote monster)
            if (context != null && context.IsCode(ElfnoteExecutor.CardId.ElfnoteLucina))
            {
                bool hasTuner = _exec.Bot.Hand.Any(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ElfnotePowerPatron))
                             || _exec.Bot.GetMonsters().Any(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ElfnotePowerPatron));
                if (!hasTuner)
                {
                    var tuner = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ElfnotePowerPatron));
                    if (tuner != null) return tuner;
                }

                bool hasRegina = _exec.Bot.Hand.Any(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ElfnoteRegina));
                if (!hasRegina)
                {
                    var regina = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ElfnoteRegina));
                    if (regina != null) return regina;
                }

                bool hasTinia = _exec.Bot.Hand.Any(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ElfnoteTinia));
                if (!hasTinia)
                {
                    var tinia = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ElfnoteTinia));
                    if (tinia != null) return tinia;
                }

                var fortuna = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ElfnoteFortuna));
                if (fortuna != null) return fortuna;
            }

            // Context 9: Elfnote Power Patron (Searches ANY Elfnote card)
            if (context != null && context.IsCode(ElfnoteExecutor.CardId.ElfnotePowerPatron))
            {
                bool hasRhapsodia = _exec.Bot.GetSpells().Any(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ElfnotesRhapsodiaOfMadness))
                                 || _exec.Bot.Hand.Any(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ElfnotesRhapsodiaOfMadness));
                if (!hasRhapsodia)
                {
                    var rhapsodia = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ElfnotesRhapsodiaOfMadness));
                    if (rhapsodia != null) return rhapsodia;
                }

                bool hasWelcome = _exec.Bot.GetSpells().Any(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ElfnotesWelcomeHome))
                               || _exec.Bot.Hand.Any(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ElfnotesWelcomeHome));
                if (!hasWelcome)
                {
                    var welcome = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ElfnotesWelcomeHome));
                    if (welcome != null) return welcome;
                }

                var lucina = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ElfnoteLucina));
                if (lucina != null) return lucina;

                var regina = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ElfnoteRegina));
                if (regina != null) return regina;
            }

            // General Priority Fallback
            int[] generalPriorities = {
                ElfnoteExecutor.CardId.ElfnotePowerPatron,
                ElfnoteExecutor.CardId.ElfnoteRegina,
                ElfnoteExecutor.CardId.ElfnoteLucina,
                ElfnoteExecutor.CardId.TheorealizeMedius,
                ElfnoteExecutor.CardId.MediusThePure,
                ElfnoteExecutor.CardId.ArsMagnaCitrinitas,
                ElfnoteExecutor.CardId.ArsMagnaOfInfinityAndFinity,
                ElfnoteExecutor.CardId.ArsMagnaOfPurificationAndCorruption,
                ElfnoteExecutor.CardId.TheorealizePastLull,
                ElfnoteExecutor.CardId.ElfnoteTinia,
                ElfnoteExecutor.CardId.ElfnotesRhapsodiaOfMadness,
                ElfnoteExecutor.CardId.ElfnotesWelcomeHome,
                ElfnoteExecutor.CardId.PowerPatronShadowSpiritJunordo,
                ElfnoteExecutor.CardId.ElfnoteFortuna
            };

            foreach (int id in generalPriorities)
            {
                var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id));
                if (match != null) return match;
            }

            return candidates.FirstOrDefault();
        }

        public ClientCard PickDumpTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Context: Terminus (Dumps Power Patron from Deck/Extra Deck)
            if (context != null && context.IsCode(ElfnoteExecutor.CardId.UnleashedPowerPatronPortalTerminus))
            {
                // Dump Medicurius or Junora or Purification to set up GY Power Patron
                var medicurius = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.MedicuriusThePowerPatronOfIllusions));
                if (medicurius != null) return medicurius;

                var junora = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.JunoraThePowerPatronOfTuning));
                if (junora != null) return junora;

                var linkPatron = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.PurificationPowerPatron));
                if (linkPatron != null) return linkPatron;
            }

            return candidates.FirstOrDefault();
        }
    }

    public class ElfnoteMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly ElfnoteExecutor _exec;
        public ElfnoteMaterialEvaluator(ElfnoteExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 0;

            // 1. Absolute Boss Protection - DO NOT TRIBUTE OR SEND TO GY UNLESS REQUIRED
            if (card.IsCode(ElfnoteExecutor.CardId.ArtmageDiactorus,
                            ElfnoteExecutor.CardId.BaronneDeFleur,
                            ElfnoteExecutor.CardId.JunoraThePowerPatronOfTuning,
                            ElfnoteExecutor.CardId.ElfnoteJunePride,
                            ElfnoteExecutor.CardId.MedicuriusThePowerPatronOfIllusions,
                            ElfnoteExecutor.CardId.CrystalWingSynchroDragon,
                            ElfnoteExecutor.CardId.ChaosAngel,
                            ElfnoteExecutor.CardId.PSYFramelordOmega,
                            ElfnoteExecutor.CardId.FADawnDragster,
                            ElfnoteExecutor.CardId.RavenousCrocodragonArchethys,
                            ElfnoteExecutor.CardId.SPLittleKnight))
            {
                return 1000;
            }

            // 2. Continuous Traps & Spells / High Value Board Locks
            if (card.IsCode(ElfnoteExecutor.CardId.ElfnotesRhapsodiaOfMadness,
                            ElfnoteExecutor.CardId.ArsMagnaCitrinitas,
                            ElfnoteExecutor.CardId.SolemnJudgment))
            {
                return 800;
            }

            // 3. Center Zone Castle Defender
            if (card.IsFaceup() && card.Sequence == 2)
            {
                return 700;
            }

            // 4. Regina has self-recursion when used as Synchro Material
            if (card.IsCode(ElfnoteExecutor.CardId.ElfnoteRegina))
            {
                return 10;
            }

            // 5. GY-trigger or self-reviving cards
            if (card.IsCode(ElfnoteExecutor.CardId.JetSynchron,
                            ElfnoteExecutor.CardId.MediusThePure))
            {
                return 20;
            }

            // 6. Generic low-priority fodder
            if (card.IsCode(ElfnoteExecutor.CardId.PurificationPowerPatron,
                            ElfnoteExecutor.CardId.TheorealizeMedius))
            {
                return 30;
            }

            return 50;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.OrderBy(c => GetMaterialCost(c)).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Prioritize discarding Regina (adds herself back to hand on Synchro)
            var regina = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.ElfnoteRegina));
            if (regina != null) return regina;

            // Prioritize Jet Synchron (can revive from GY)
            var jet = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.JetSynchron));
            if (jet != null) return jet;

            // Prioritize Medius (can revive itself from GY)
            var medius = candidates.FirstOrDefault(c => c != null && c.IsCode(ElfnoteExecutor.CardId.MediusThePure));
            if (medius != null) return medius;

            // Prioritize duplicate Elfnote cards
            var duplicate = candidates.GroupBy(c => c.Id).FirstOrDefault(g => g.Count() > 1)?.FirstOrDefault();
            if (duplicate != null) return duplicate;

            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }
    }

    public class ElfnoteThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly ElfnoteExecutor _exec;
        public ElfnoteThreatEvaluator(ElfnoteExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null) return 0;
            return CardIntelligence.GetCardThreatScore(card);
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null) return false;
            return CardIntelligence.GetCardThreatScore(card) >= 9000 || (card.IsMonster() && card.Attack >= 3000);
        }
    }
}
