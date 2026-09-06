// ============================================================
// CARD AUDIT — 2026_Stun (Dominus & Anti-Meta Dogmatika Stun)
// ============================================================
// | Card Name                     | Type       | OPT? | HOPT? | Cost | Effect Summary                              | Activate When                               | NEVER Activate When                           |
// |-------------------------------|------------|------|-------|------|---------------------------------------------|---------------------------------------------|-----------------------------------------------|
// | Inspector Boarder             | Monster L4 | No   | No    | None | Lock monster effect activations             | Turn 1 Normal Summon priority #1            | Already have Boarder on field                 |
// | Barrier Statue of Inferno     | Monster L4 | No   | No    | None | Neither player can SS except FIRE           | Normal Summon priority #2                    | Already controlling a statue                  |
// | Barrier Statue of Torrent     | Monster L4 | No   | No    | None | Neither player can SS except WATER          | Normal Summon priority #2                    | Already controlling a statue                  |
// | Dogmatika Ecclesia            | Monster L4 | Yes  | Yes   | None | NS/SS: Search Dogmatika Punishment          | In Hand / On Summon                         | Already have Punishment & Statue              |
// | Pot of Duality                | Spell      | Yes  | Yes   | None | Excavate 3, add 1, cannot SS this turn      | Main Phase early                            | After Special Summoning                       |
// | Nadir Servant                 | Spell      | Yes  | Yes   | Send | Send ED monster -> Add Ecclesia from Dk/GY  | Main Phase starter                          | Already have all pieces                       |
// | Decisive Battle of Golgonda   | ContSpell  | Yes  | No    | Send | Protects face-up cards by dumping Albaz ED  | On field / When card would be destroyed     | No Albaz Fusion in ED                         |
// | The Fallen & The Virtuous     | QuickSp    | Yes  | Yes   | Send | Send Albaz ED -> Destroy 1 face-up card     | Opponent has threat on field / Interrupt    | No targets                                    |
// | Dogmatika Punishment          | NormalTrap | Yes  | Yes   | Send | Send ED monster with >= ATK -> Pop monster  | Opponent summons threat monster             | No valid ED target with >= ATK                |
// | Dominus Impulse               | NormalTrap | Yes  | Yes   | None | Negate eff that includes SS & destroy       | Opponent activates SS effect                | Chain already resolved                        |
// | Dominus Purge                 | NormalTrap | Yes  | Yes   | None | Negate eff that adds from Deck & destroy    | Opponent activates search effect            | Chain already resolved                        |
// | Songs of the Dominators       | NormalTrap | Yes  | Yes   | None | Negate monster eff on field + search Dominus| Opponent activates monster eff on field     | Chain already resolved                        |
// | Dominus Spark                 | NormalTrap | Yes  | Yes   | None | Banish opp monster on hand/GY eff           | Opponent activates eff in hand/GY           | No opponent monster on field                  |
// | Solemn Judgment               | CounterTr  | No   | No    | LP/2 | Pay half LP -> Negate S/T or Summon         | Opponent activates board breaker or boss SS | LP < 1000                                     |
// | Solemn Warning                | CounterTr  | No   | No    | 2000 | Pay 2000 LP -> Negate Summon or SS eff      | Opponent summons boss or activates SS       | LP <= 2000                                    |
// | Solemn Report                 | CounterTr  | No   | No    | 1500 | Pay 1500/3000 LP -> Negate S/T & lock name  | Opponent activates S/T                      | LP <= 1500                                    |
// | Infinite Impermanence         | NormalTrap | No   | No    | None | Negate face-up monster effect               | Opponent activates key monster on field     | Monster already negated                       |
// | Elder Entity N'tss            | Fusion L4  | Yes  | No    | None | Sent to GY -> Destroy 1 card on field       | Sent as cost by Punishment / Nadir          | Field has no targets                          |
// | Garura                        | Fusion L6  | Yes  | Yes   | None | Sent to GY -> Draw 1 card                   | Sent as cost by Nadir                       | Deck empty                                    |
// | The Dragon that Devours Dogma | Fusion L8  | Yes  | Yes   | None | 3000 ATK send cost; End Phase search Dogma  | Sent as cost by Punishment / The Fallen     | Already in GY                                 |
// | Titaniklad                    | Fusion L8  | Yes  | Yes   | None | 2500 ATK send cost; End Phase search Dogma  | Sent as cost by Punishment / Nadir          | Already in GY                                 |
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Stun", "2026_Stun")]
    public class _2026_StunExecutor : ModernExecutor
    {
        public class CardId
        {
            // Floodgate & Engine Monsters
            public const int InspectorBoarder = 15397015;
            public const int BarrierStatueOfTheInferno = 47961808;
            public const int BarrierStatueOfTheTorrent = 10963799;
            public const int DogmatikaEcclesia = 60303688;
            public const int MaxxC = 23434538;

            // Spells
            public const int PotOfDuality = 98645731;
            public const int NadirServant = 1984618;
            public const int DecisiveBattleOfGolgonda = 70485614;
            public const int TheFallenAndTheVirtuous = 30271097;

            // Normal Traps & Dominus Traps
            public const int InfiniteImpermanence = 10045474;
            public const int DogmatikaPunishment = 82956214;
            public const int DominusImpulse = 40366667;
            public const int DominusPurge = 97045737;
            public const int SongsOfTheDominators = 58053438;
            public const int DominusSpark = 6325660;

            // Counter Traps
            public const int SolemnJudgment = 41420027;
            public const int SolemnWarning = 84749824;
            public const int SolemnReport = 78114463;

            // Extra Deck Targets
            public const int ElderEntityNtss = 80532587;
            public const int Garura = 11765832;
            public const int TitanikladTheAshDragon = 41373230;
            public const int SprindTheIrondashDragon = 1906812;
            public const int AlbionTheBrandedDragon = 87746184;
            public const int AlbaLenatusTheAbyssDragon = 3410461;
            public const int TheDragonThatDevoursTheDogma = 76666602;
            public const int EcclesiaAndTheDarkDragon = 78397661;
            public const int VallonTheSuperPsySkyblaster = 40673853;
            public const int ArtemisTheMagistusMoonMaiden = 34755994;
            public const int SPLittleKnight = 29301450;
        }

        // Standard OCG Hint IDs
        private const long HINT_SELECT_FACEUP = 500;
        private const long HINT_SELECT_TOGRAVE = 501;
        private const long HINT_SELECT_DESTROY = 502;
        private const long HINT_SELECT_DISCARD = 504;
        private const long HINT_SELECT_TOHAND = 506;
        private const long HINT_SELECT_SPSUMMON = 509;
        private const long HINT_SELECT_BANISH = 512;

        // Turn state flags
        private bool _potOfDualityUsed = false;
        private bool _nadirServantUsed = false;
        private bool _ecclesiaSummonUsed = false;
        private bool _fallenVirtuousUsed = false;
        private bool _impulseUsed = false;
        private bool _purgeUsed = false;
        private bool _songsUsed = false;
        private bool _sparkUsed = false;

        public _2026_StunExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ═══════════════════════════════════════════════════════════════
            //  EXECUTORS PIPELINE (Tiered Priority)
            // ═══════════════════════════════════════════════════════════════

            // ── Tier 0: Counter Traps & Hard Negates ──
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, DefaultSolemnJudgment);
            AddExecutor(ExecutorType.Activate, CardId.SolemnWarning, DefaultSolemnWarning);
            AddExecutor(ExecutorType.Activate, CardId.SolemnReport, DefaultSolemnReport);

            // Dominus Traps & Hand-Activated Interruptions
            AddExecutor(ExecutorType.Activate, CardId.DominusImpulse, DominusImpulseEffect);
            AddExecutor(ExecutorType.Activate, CardId.DominusPurge, DominusPurgeEffect);
            AddExecutor(ExecutorType.Activate, CardId.SongsOfTheDominators, SongsOfTheDominatorsEffect);
            AddExecutor(ExecutorType.Activate, CardId.DominusSpark, DominusSparkEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCEffect);

            // On-field Removal Traps & Spells
            AddExecutor(ExecutorType.Activate, CardId.DogmatikaPunishment, DogmatikaPunishmentEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheFallenAndTheVirtuous, TheFallenAndTheVirtuousEffect);

            // Sent to GY Triggers (N'tss, Garura, Dogma Dragon)
            AddExecutor(ExecutorType.Activate, CardId.ElderEntityNtss, NtssEffect);
            AddExecutor(ExecutorType.Activate, CardId.Garura, GaruraEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheDragonThatDevoursTheDogma, DogmaDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.TitanikladTheAshDragon, TitanikladEffect);
            AddExecutor(ExecutorType.Activate, CardId.VallonTheSuperPsySkyblaster, VallonEffect);

            // ── Tier 1: Searchers & Setup Spells ──
            AddExecutor(ExecutorType.Activate, CardId.PotOfDuality, PotOfDualityEffect);
            AddExecutor(ExecutorType.Activate, CardId.NadirServant, NadirServantEffect);
            AddExecutor(ExecutorType.Activate, CardId.DecisiveBattleOfGolgonda, GolgondaEffect);

            // ── Tier 2: Normal Summons (Strict Floodgate Hierarchy) ──
            AddExecutor(ExecutorType.Summon, CardId.InspectorBoarder, InspectorBoarderSummon);
            AddExecutor(ExecutorType.Summon, CardId.BarrierStatueOfTheInferno, BarrierStatueSummon);
            AddExecutor(ExecutorType.Summon, CardId.BarrierStatueOfTheTorrent, BarrierStatueSummon);
            AddExecutor(ExecutorType.Summon, CardId.DogmatikaEcclesia, EcclesiaSummon);
            AddExecutor(ExecutorType.Activate, CardId.DogmatikaEcclesia, EcclesiaEffect);

            // Fallback Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.BarrierStatueOfTheInferno, SimpleSummon);
            AddExecutor(ExecutorType.Summon, CardId.BarrierStatueOfTheTorrent, SimpleSummon);
            AddExecutor(ExecutorType.Summon, CardId.DogmatikaEcclesia, SimpleSummon);

            // ── Tier 3: Spell & Trap Sets ──
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnReport, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnJudgment, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnWarning, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.DogmatikaPunishment, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.TheFallenAndTheVirtuous, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.DominusImpulse, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.DominusPurge, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.SongsOfTheDominators, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.DominusSpark, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, DefaultSpellSet);

            // ── Tier 4: Repositioning ──
            AddExecutor(ExecutorType.Repos, MonsterReposOverride);
        }

        public override bool OnSelectHand() => true;

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _potOfDualityUsed = false;
            _nadirServantUsed = false;
            _ecclesiaSummonUsed = false;
            _fallenVirtuousUsed = false;
            _impulseUsed = false;
            _purgeUsed = false;
            _songsUsed = false;
            _sparkUsed = false;
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.InspectorBoarder
                || card.Id == CardId.BarrierStatueOfTheInferno
                || card.Id == CardId.BarrierStatueOfTheTorrent;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 0: COUNTER TRAPS & NEGATES
        // ═══════════════════════════════════════════════════════════════

        private bool DominusImpulseEffect()
        {
            if (_impulseUsed) return false;
            if (Card != null && Util.ChainContainsCard(Card.Id)) return false;
            if (Duel.LastChainPlayer == 1)
            {
                _impulseUsed = true;
                return true;
            }
            return false;
        }

        private bool DominusPurgeEffect()
        {
            if (_purgeUsed) return false;
            if (Card != null && Util.ChainContainsCard(Card.Id)) return false;
            if (Duel.LastChainPlayer == 1)
            {
                _purgeUsed = true;
                return true;
            }
            return false;
        }

        private bool SongsOfTheDominatorsEffect()
        {
            if (_songsUsed) return false;
            if (Card != null && Util.ChainContainsCard(Card.Id)) return false;
            if (Duel.LastChainPlayer == 1)
            {
                _songsUsed = true;
                AI.SelectCard(CardId.DominusImpulse, CardId.DominusPurge, CardId.DominusSpark);
                return true;
            }
            return false;
        }

        private bool DominusSparkEffect()
        {
            if (_sparkUsed) return false;
            if (Duel.LastChainPlayer == 0) return false;
            if (Card != null && Util.ChainContainsCard(Card.Id)) return false;
            var oppTarget = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup());
            if (oppTarget != null)
            {
                _sparkUsed = true;
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        private bool MaxxCEffect()
        {
            if (Card != null && Util.ChainContainsCard(Card.Id)) return false;
            if (Duel.LastChainPlayer == 1)
            {
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  REMOVAL TRAPS & SPELLS
        // ═══════════════════════════════════════════════════════════════

        private bool DogmatikaPunishmentEffect()
        {
            if (Card.Location != CardLocation.SpellZone) return false;
            // Target opponent face-up monster with highest ATK (ignoring immune Dark Magician if Eternal Soul is active)
            bool oppHasEternalSoul = Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.Id == 48680970);
            var oppMonster = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup() && (!oppHasEternalSoul || c.Id != 46986414));
            if (oppMonster == null) return false;

            // Pick Extra Deck monster with ATK >= oppMonster.Attack
            // Priority: N'tss (2500 ATK, pops another card!), Garura (1500 ATK, draws 1 card), Dogma Dragon (3000 ATK, searches in EP)
            int targetAtk = oppMonster.Attack;
            ClientCard edTarget = null;

            if (targetAtk <= 2500 && Bot.ExtraDeck.Any(c => c.Id == CardId.ElderEntityNtss))
            {
                edTarget = Bot.ExtraDeck.First(c => c.Id == CardId.ElderEntityNtss);
            }
            else if (targetAtk <= 3000 && Bot.ExtraDeck.Any(c => c.Id == CardId.TheDragonThatDevoursTheDogma))
            {
                edTarget = Bot.ExtraDeck.First(c => c.Id == CardId.TheDragonThatDevoursTheDogma);
            }
            else if (targetAtk <= 2500 && Bot.ExtraDeck.Any(c => c.Id == CardId.TitanikladTheAshDragon))
            {
                edTarget = Bot.ExtraDeck.First(c => c.Id == CardId.TitanikladTheAshDragon);
            }
            else if (targetAtk <= 1500 && Bot.ExtraDeck.Any(c => c.Id == CardId.Garura))
            {
                edTarget = Bot.ExtraDeck.First(c => c.Id == CardId.Garura);
            }
            else
            {
                edTarget = Bot.ExtraDeck.FirstOrDefault(c => c.Attack >= targetAtk);
            }

            if (edTarget != null)
            {
                AI.SelectCard(oppMonster);
                AI.SelectNextCard(edTarget);
                return true;
            }
            return false;
        }

        private bool TheFallenAndTheVirtuousEffect()
        {
            if (_fallenVirtuousUsed) return false;
            bool oppHasEternalSoul = Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.Id == 48680970);
            var oppTarget = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && (c.Id == 48680970 || c.Id == 47222536 || c.Id == 66399653 || CardIntelligence.IsFloodgateSpellTrap(c.Id)))
                         ?? Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && (c.Id == 4280258 || c.Id == 1561110 || c.Id == 21887175))
                         ?? Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup() && (!oppHasEternalSoul || c.Id != 46986414))
                         ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());

            if (oppTarget != null)
            {
                _fallenVirtuousUsed = true;
                // Option 0: Send Albaz Fusion from Extra Deck -> Destroy 1 face-up card on field
                AI.SelectOption(0);
                AI.SelectCard(CardId.TheDragonThatDevoursTheDogma, CardId.TitanikladTheAshDragon, CardId.AlbionTheBrandedDragon);
                AI.SelectNextCard(oppTarget);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXTRA DECK SENT TO GY TRIGGERS
        // ═══════════════════════════════════════════════════════════════

        private bool NtssEffect()
        {
            // Sent to GY -> Destroy 1 card on field!
            bool oppHasEternalSoul = Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.Id == 48680970);
            var oppTarget = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && (c.Id == 48680970 || c.Id == 47222536 || c.Id == 66399653 || CardIntelligence.IsFloodgateSpellTrap(c.Id)))
                         ?? Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && (c.Id == 4280258 || c.Id == 1561110 || c.Id == 21887175))
                         ?? Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup() && (!oppHasEternalSoul || c.Id != 46986414))
                         ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup())
                         ?? Enemy.GetMonsters().FirstOrDefault(c => c != null && (!oppHasEternalSoul || c.Id != 46986414))
                         ?? Enemy.GetSpells().FirstOrDefault();
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        private bool GaruraEffect() => true; // Draw 1 card!

        private bool DogmaDragonEffect()
        {
            // End Phase: Search Dogmatika card from Deck
            AI.SelectCard(CardId.DogmatikaEcclesia, CardId.DogmatikaPunishment);
            return true;
        }

        private bool TitanikladEffect()
        {
            // End Phase: Search Ecclesia from Deck
            AI.SelectCard(CardId.DogmatikaEcclesia);
            return true;
        }

        private bool VallonEffect()
        {
            // Sent to GY -> Destroy 1 face-down card on field
            var facedown = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFacedown())
                        ?? Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFacedown());
            if (facedown != null)
            {
                AI.SelectCard(facedown);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 1: SEARCHERS & SETUP SPELLS
        // ═══════════════════════════════════════════════════════════════

        private bool PotOfDualityEffect()
        {
            if (_potOfDualityUsed) return false;
            _potOfDualityUsed = true;
            // Pick best card: Boarder / Statue / Punishment / Solemn
            AI.SelectCard(CardId.InspectorBoarder, CardId.SolemnReport, CardId.BarrierStatueOfTheInferno, CardId.BarrierStatueOfTheTorrent, CardId.DogmatikaPunishment, CardId.NadirServant);
            return true;
        }

        private bool NadirServantEffect()
        {
            if (_nadirServantUsed) return false;
            _nadirServantUsed = true;

            // Send Garura (draw 1) or Dogma Dragon (3000 ATK, EP search) or N'tss (if opp has card to pop)
            if (Enemy.GetMonsterCount() > 0 && Bot.ExtraDeck.Any(c => c.Id == CardId.ElderEntityNtss))
            {
                AI.SelectCard(CardId.ElderEntityNtss);
            }
            else if (Bot.ExtraDeck.Any(c => c.Id == CardId.Garura))
            {
                AI.SelectCard(CardId.Garura);
            }
            else
            {
                AI.SelectCard(CardId.TheDragonThatDevoursTheDogma, CardId.TitanikladTheAshDragon);
            }

            // Search Dogmatika Ecclesia
            AI.SelectNextCard(CardId.DogmatikaEcclesia);
            return true;
        }

        private bool GolgondaEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return true;
            }
            if (Card.Location == CardLocation.SpellZone)
            {
                // When a card would be destroyed: send Albaz fusion from Extra Deck to GY instead!
                AI.SelectCard(CardId.TheDragonThatDevoursTheDogma, CardId.TitanikladTheAshDragon, CardId.SprindTheIrondashDragon, CardId.AlbionTheBrandedDragon);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 2: NORMAL SUMMONS (FLOODGATE HIERARCHY)
        // ═══════════════════════════════════════════════════════════════

        private bool InspectorBoarderSummon()
        {
            // Priority 1: Boarder locks everything if opponent has no Extra Deck monsters!
            return !Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.InspectorBoarder);
        }

        private bool BarrierStatueSummon()
        {
            // Priority 2: Statues lock Special Summons!
            return !Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Id == CardId.BarrierStatueOfTheInferno || c.Id == CardId.BarrierStatueOfTheTorrent || c.Id == CardId.InspectorBoarder));
        }

        private bool EcclesiaSummon()
        {
            if (_ecclesiaSummonUsed) return false;
            // Normal summon Ecclesia to search Dogmatika Punishment
            return !Bot.HasInHand(CardId.DogmatikaPunishment) && !Bot.GetSpells().Any(c => c != null && c.Id == CardId.DogmatikaPunishment);
        }

        private bool EcclesiaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_ecclesiaSummonUsed) return false;
                _ecclesiaSummonUsed = true;
                // Search Dogmatika Punishment
                AI.SelectCard(CardId.DogmatikaPunishment);
                return true;
            }
            return false;
        }

        private bool SimpleSummon() => true;

        private bool MonsterReposOverride()
        {
            if (Card.Attack < Card.Defense)
            {
                return DefaultMonsterRepos();
            }
            return false;
        }
    }
}
