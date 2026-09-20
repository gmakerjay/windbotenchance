// ============================================================================
// CARD AUDIT — Anime_Gong (Gong Strong / Gongenzaka Noboru — Steadfast Defense)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Ash Blossom & Joyous Spring        | Monster L3 T | Yes  | Yes   | Discard | Handtrap: Negate deck search/dump/SS          | Opponent activates deck-interacting effect    | Bot's own turn without threat               |
// | Ghost Ogre & Snow Rabbit           | Monster L3 T | Yes  | Yes   | Send    | Handtrap: Destroy card activating effect      | Opponent monster or cont S/T activates        | Card cannot be destroyed                    |
// | Effect Veiler                      | Monster L1 T | Yes  | Yes   | Send    | Handtrap: Negate opp face-up effect in opp MP | Opponent Main Phase threat monster            | Target already negated                      |
// | Droll & Lock Bird                  | Monster L1   | Yes  | No    | Send    | Handtrap: Lock searching/drawing after 1 add  | Opponent adds card from Deck to hand          | Bot needs to add cards this turn            |
// | Superheavy Samurai Motorbike       | Monster L2 T | Yes  | Yes   | Discard | Discard to search ANY Superheavy Samurai mon  | Main Phase 1: search Wakaushi starter         | Already have Wakaushi                       |
// | Superheavy Samurai Prodigy Wakaushi| Monster L4 P | Yes  | Yes   | None    | Scale: Place Monk Benkei & SS self from P-Zone| In hand/P-Zone: primary 1-card engine starter | S/T in GY (breaks Steadfast condition)      |
// | Superheavy Samurai Monk Big Benkei | Monster L8 P | Yes  | Yes   | None    | P-Scale: Search 1 Superheavy Soul monster     | In P-Zone: search Soulpiercer/Soulpeacemaker  | Already used this turn                      |
// | Superheavy Samurai Soulpiercer     | Monster L4   | No   | No    | None    | Equip to SHS; when sent to GY search SHS mon  | Hand/field: equip or synchro material         | Deck has no SHS monsters                    |
// | Superheavy Samurai Soulpeacemaker  | Monster L1   | Yes  | Yes   | Tribute | Equip; tribute equipped SHS to SS from Deck   | Equipped to SHS; cheat out Scales/Big Benkei  | S/T in GY                                   |
// | Superheavy Samurai Soulbuster Gaunt| Monster L1   | Yes  | Yes   | Discard | Hand DEF Honest: Double DEF during damage calc| Damage Step; double DEF (up to 9600 damage!)  | No battle / already lethal                  |
// | Superheavy Samurai Scales          | Monster L4   | Yes  | Yes   | None    | On NS/SS: Revive L4 or lower SHS from GY (DEF)| Normal or Special Summoned; revive Soulpiercer| No L4 or lower SHS in GY                    |
// | Superheavy Samurai Flutist         | Monster L3   | Yes  | Yes   | Banish  | Tribute to SS from hand; GY banish negate tar | In hand: extend; in GY: protect SHS from tar  | No targeting effect                         |
// | Superheavy Samurai Gigagloves      | Monster L3   | Yes  | Yes   | Banish  | GY banish on direct attack: drop ATK to 0 & dr| Opponent declares direct attack               | Bot has monsters on field                   |
// | Superheavy Samurai Trumpeter       | Monster L2 T | Yes  | Yes   | None    | SS from hand if no S/T in GY; synchro tuner   | In hand; need Tuner for climbing              | S/T in GY                                   |
// | Superheavy Samurai Fist            | Monster L2 T | Yes  | Yes   | Target  | SS from GY by reducing SHS Synchro level by 1 | In GY; need Tuner revival for Synchro climb   | S/T in GY                                   |
// | Superheavy Samurai Big Waraji      | Monster L5   | No   | No    | None    | SS from hand if no S/T in GY; counts as 2 trib| In hand; free Level 5 body                    | S/T in GY                                   |
// | Superheavy Samurai Big Benkei      | Monster L8   | No   | No    | None    | 3500 DEF; attacks while in Defense Position   | Field boss attacker using 3500 DEF            | Cannot attack                               |
// | Naturia Beast                      | Synchro L5   | No   | No    | Mill 2  | Quick Omni-Negate ANY Spell Card activation   | Opponent activates Spell Card                 | Opponent activated no Spell                 |
// | Superheavy Samurai Brave Masurawo  | Synchro L12  | Yes  | Yes   | None    | 4000 DEF; battles in DEF; draw up to 3 on S/T | Opponent activates S/T; draw cards + battle   | S/T in GY                                   |
// | Superheavy Samurai Steam Train King| Synchro L12  | Yes  | Yes   | Discard | 4800 DEF direct attack; pop 2 cards; burn     | Boss finisher with 4800 DEF direct attack     | Cannot attack                               |
// | Superheavy Samurai Warlord Susanowo| Synchro L10  | Yes  | Yes   | None    | 3800 DEF; Quick: Steal 1 opp S/T from their GY| Opponent GY has useful Spell/Trap             | Opponent GY has no S/T                      |
// | Superheavy Samurai Beast Kyubi     | Synchro L9   | No   | No    | None    | 2500 DEF + 900 per opponent SSed monster      | Opponent has 2+ Special Summoned monsters     | Opponent has 0 SSed monsters                |
// | Superheavy Samurai Ninja Sarutobi  | Synchro L8   | Yes  | Yes   | None    | 2800 DEF; Quick: Pop 1 opp S/T + 500 burn     | Opponent controls Spell/Trap                  | Opponent controls 0 S/T                     |
// | Superheavy Samurai Commander Shanawo| Synchro L8  | Yes  | Yes   | None    | Battle Phase Quick: change battle position    | Battle Phase; drop attacker ATK to 0          | Outside battle / already negated            |
// | Superheavy Samurai Ogre Shutendoji | Synchro L6   | Yes  | Yes   | None    | On Synchro Summon: Wipe all opp Spells/Traps  | Opponent controls 1+ Spells/Traps             | Opponent controls 0 S/T                     |
// | Superheavy Samurai Swordmaster Musa| Synchro L5   | Yes  | Yes   | None    | On Synchro Summon: Recover 1 Machine from GY  | Need Machine recovery from GY                 | GY has 0 Machines                          |
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
    [Deck("Anime_Gong", "Anime_Gong")]
    public class Anime_GongExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int Motorbike = 83334932;
            public const int ProdigyWakaushi = 82112494;
            public const int MonkBigBenkei = 19510093;
            public const int Soulpiercer = 90361010;
            public const int Soulpeacemaker = 95500396;
            public const int SoulbusterGauntlet = 35800511;
            public const int BigBenkei = 3117804;
            public const int BigWaraji = 36523152;
            public const int Trumpeter = 64373401;
            public const int Fist = 71386411;
            public const int Flutist = 27978707;
            public const int Gigagloves = 62017867;
            public const int Magnet = 89091772;
            public const int Scales = 78391364;
            public const int AshBlossom = 14558127;
            public const int GhostOgre = 59438930;
            public const int EffectVeiler = 97268402;
            public const int DrollAndLockBird = 94145021;

            // Extra Deck
            public const int SteamTrainKing = 17775525;
            public const int BraveMasurawo = 64193046;
            public const int WarlordSusanowo = 494922;
            public const int BeastKyubi = 85528209;
            public const int CommanderShanawo = 90587641;
            public const int NinjaSarutobi = 76471944;
            public const int StealthNinja = 50065971;
            public const int OgreShutendoji = 36953371;
            public const int SwordmasterMusashi = 75988594;
            public const int NaturiaBeast = 33198837;
        }

        public Anime_GongExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterComboLines();
            RegisterExecutors();
        }

        private void RegisterComboLines()
        {
            // ── Line 1: 1-Card Wakaushi Full Climb ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "SHS-Wakaushi-Full-Climb",
                RequiredCards = new List<int> { CardId.ProdigyWakaushi },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.ProdigyWakaushi, ActionType = ExecutorType.Activate, Description = "Place Wakaushi in Scale" },
                    new() { CardId = CardId.ProdigyWakaushi, ActionType = ExecutorType.Activate, Description = "Wakaushi place Monk Benkei & SS self" },
                    new() { CardId = CardId.MonkBigBenkei, ActionType = ExecutorType.Activate, Description = "Monk Benkei search Soulpiercer" }
                },
                FallbackLineName = "SHS-Motorbike-Starter"
            });

            // ── Line 2: Motorbike Searcher Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "SHS-Motorbike-Starter",
                RequiredCards = new List<int> { CardId.Motorbike },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.Motorbike, ActionType = ExecutorType.Activate, Description = "Discard Motorbike -> Search Wakaushi" },
                    new() { CardId = CardId.ProdigyWakaushi, ActionType = ExecutorType.Activate, Description = "Place Wakaushi in Scale" },
                    new() { CardId = CardId.ProdigyWakaushi, ActionType = ExecutorType.Activate, Description = "Wakaushi place Monk Benkei & SS self" },
                    new() { CardId = CardId.MonkBigBenkei, ActionType = ExecutorType.Activate, Description = "Monk Benkei search Soulpiercer" }
                }
            });
        }

        private void RegisterExecutors()
        {
            // ═══════════════════════════════════════════════════════════════
            //  TIER 0: QUICK DISRUPTIONS, NEGATES & COMBAT TRICKS
            // ═══════════════════════════════════════════════════════════════

            // Naturia Beast — Infinite Quick Spell Negate (Mill 2 to negate & destroy)
            AddExecutor(ExecutorType.Activate, CardId.NaturiaBeast, NaturiaBeastNegateActivate);

            // Warlord Susanowo — Quick steal Spell/Trap from opponent's GY
            AddExecutor(ExecutorType.Activate, CardId.WarlordSusanowo, SusanowoStealActivate);

            // Ninja Sarutobi — Quick pop opponent Spell/Trap + 500 burn
            AddExecutor(ExecutorType.Activate, CardId.NinjaSarutobi, SarutobiPopActivate);

            // Commander Shanawo — Battle Phase position changer & attack negator
            AddExecutor(ExecutorType.Activate, CardId.CommanderShanawo, ShanawoActivate);

            // Soulbuster Gauntlet — Hand DEF Honest (Double DEF in Damage Step)
            AddExecutor(ExecutorType.Activate, CardId.SoulbusterGauntlet, SoulbusterActivate);

            // Flutist — GY banish to negate effect targeting SHS monster
            AddExecutor(ExecutorType.Activate, CardId.Flutist, FlutistGYProtectActivate);

            // Gigagloves — GY banish on direct attack: drop ATK to 0 & draw 1
            AddExecutor(ExecutorType.Activate, CardId.Gigagloves, GigaglovesActivate);

            // Handtraps
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomActivate);
            AddExecutor(ExecutorType.Activate, CardId.GhostOgre, GhostOgreActivate);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, EffectVeilerActivate);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: MAIN PHASE STARTERS & PENDULUM SETUPS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.Motorbike, MotorbikeActivate);
            AddExecutor(ExecutorType.Activate, CardId.ProdigyWakaushi, WakaushiActivate);
            AddExecutor(ExecutorType.Activate, CardId.MonkBigBenkei, MonkBenkeiActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: NORMAL SUMMONS & FIELD TRIGGERS
            // ═══════════════════════════════════════════════════════════════
            // Scales Normal Summon (revives L4 or lower SHS from GY)
            AddExecutor(ExecutorType.Summon, CardId.Scales, ScalesNormalSummon);
            AddExecutor(ExecutorType.Activate, CardId.Scales, ScalesActivate);

            // Soulpiercer Normal Summon (4) or Trigger in GY
            AddExecutor(ExecutorType.Summon, CardId.Soulpiercer, SoulpiercerSummon);
            AddExecutor(ExecutorType.Activate, CardId.Soulpiercer, SoulpiercerEffect);

            // Magnet Normal Summon (SS L4 or lower SHS from hand)
            AddExecutor(ExecutorType.Summon, CardId.Magnet, MagnetSummon);
            AddExecutor(ExecutorType.Activate, CardId.Magnet, MagnetEffect);

            // Flutist Normal Summon & Tribute from field
            AddExecutor(ExecutorType.Summon, CardId.Flutist, FlutistSummon);
            AddExecutor(ExecutorType.Activate, CardId.Flutist, FlutistEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: SPECIAL SUMMON EXTENDERS & EQUIPS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.Scales, ScalesInherentSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Trumpeter, TrumpeterSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BigWaraji, BigWarajiSpSummon);

            // Fist GY Revival
            AddExecutor(ExecutorType.Activate, CardId.Fist, FistEffect);

            // Soulpeacemaker: Equip from hand/field & Tribute equipped monster to SS from deck
            AddExecutor(ExecutorType.Activate, CardId.Soulpeacemaker, SoulpeacemakerEffect);

            // Fallback Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.Fist, FistNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.Trumpeter, FallbackNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.Gigagloves, FallbackNormalSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: EXTRA DECK SYNCHRO PROGRESSION
            // ═══════════════════════════════════════════════════════════════

            // Level 5 Naturia Beast (Infinite Spell Negate against Spell decks)
            AddExecutor(ExecutorType.SpSummon, CardId.NaturiaBeast, NaturiaBeastSpSummon);

            // Level 6 Shutendoji (Wipes all opponent Spells/Traps on Synchro summon)
            AddExecutor(ExecutorType.SpSummon, CardId.OgreShutendoji, ShutendojiSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.OgreShutendoji, ShutendojiEffect);

            // Level 5 Musashi (Recycles Machine from GY)
            AddExecutor(ExecutorType.SpSummon, CardId.SwordmasterMusashi, MusashiSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SwordmasterMusashi, MusashiEffect);

            // Level 7 Stealth Ninja (2800 DEF direct attacker & float on destruction)
            AddExecutor(ExecutorType.SpSummon, CardId.StealthNinja, StealthNinjaSpSummon);

            // Level 8 Sarutobi & Shanawo
            AddExecutor(ExecutorType.SpSummon, CardId.NinjaSarutobi, SarutobiSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CommanderShanawo, ShanawoSpSummon);

            // Level 9 Beast Kyubi (Massive DEF scaling vs opponent Special Summons)
            AddExecutor(ExecutorType.SpSummon, CardId.BeastKyubi, BeastKyubiSpSummon);

            // Level 10 Warlord Susanowo (3800 DEF + Quick steal opponent S/T)
            AddExecutor(ExecutorType.SpSummon, CardId.WarlordSusanowo, SusanowoSpSummon);

            // Level 12 Brave Masurawo (4000 DEF, destruction immune, draws up to 3)
            AddExecutor(ExecutorType.SpSummon, CardId.BraveMasurawo, MasurawoSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BraveMasurawo, MasurawoEffect);

            // Level 12 Steam Train King (4800 DEF direct attacker, pop 2, S/T burn)
            AddExecutor(ExecutorType.SpSummon, CardId.SteamTrainKing, SteamTrainKingSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SteamTrainKing, SteamTrainKingEffect);

            // Reposition: Switch SHS monsters to Defense Position to utilize DEF combat
            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        // ═══════════════════════════════════════════════════════════════
        //  DISRUPTIONS & COMBAT TRICKS
        // ═══════════════════════════════════════════════════════════════

        private bool NaturiaBeastNegateActivate()
        {
            // Quick Negate: mill 2 cards to negate and destroy any Spell Card
            ClientCard lastCard = LastChainCard;
            return lastCard != null && lastCard.Controller == 1 && lastCard.IsSpell();
        }

        private bool SusanowoStealActivate()
        {
            // Steal 1 Spell/Trap from opponent's GY
            if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard target = Enemy.Graveyard.FirstOrDefault(c => c.IsSpell() || c.IsTrap());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool SarutobiPopActivate()
        {
            // Quick pop 1 opponent Spell/Trap + 500 burn
            if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard target = Enemy.GetSpells().FirstOrDefault(s => s.IsFaceup())
                                 ?? Enemy.GetSpells().FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool ShanawoActivate()
        {
            // P-Zone trigger when opponent declares attack: SS self, drop ATK to 0, negate effects
            if (Card.Location == CardLocation.SpellZone)
            {
                return true;
            }
            // Monster Zone Quick Effect during Battle Phase: change battle position
            if (Card.Location == CardLocation.MonsterZone && Duel.Phase >= DuelPhase.BattleStart && Duel.Phase <= DuelPhase.Battle)
            {
                ClientCard oppMon = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.IsAttack() && m.Attack > 0);
                if (oppMon != null)
                {
                    AI.SelectCard(oppMon);
                    return true;
                }
            }
            return false;
        }

        private bool SoulbusterActivate()
        {
            // Damage Step DEF Honest: Double DEF of battling SHS monster in Defense Position
            if (Card.Location == CardLocation.Hand && Duel.Phase == DuelPhase.Damage)
            {
                ClientCard battling = Bot.BattlingMonster;
                ClientCard enemyMon = Enemy.BattlingMonster;
                if (battling != null && enemyMon != null && battling.HasSetcode(0x9a) && battling.IsDefense())
                {
                    // Trigger if opponent attack would beat or tie defense, or to inflict massive lethal reflection
                    if (battling.Defense <= enemyMon.Attack || (battling.Defense * 2 - enemyMon.Attack >= Enemy.LifePoints))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool FlutistGYProtectActivate()
        {
            // GY Quick Effect: Banish to negate and destroy an effect targeting an SHS monster
            if (Card.Location == CardLocation.Grave && Duel.LastChainPlayer == 1)
            {
                return Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasSetcode(0x9a));
            }
            return false;
        }

        private bool GigaglovesActivate()
        {
            // GY trigger on opponent direct attack: banish to drop ATK to 0 and add excavated SHS
            return Card.Location == CardLocation.Grave;
        }

        private bool AshBlossomActivate()
        {
            ClientCard lastCard = LastChainCard;
            return lastCard != null && lastCard.Controller == 1;
        }

        private bool GhostOgreActivate()
        {
            ClientCard lastCard = LastChainCard;
            return lastCard != null && lastCard.Controller == 1;
        }

        private bool EffectVeilerActivate()
        {
            ClientCard target = Enemy.GetMonsters()
                .Where(m => m.IsFaceup() && !m.IsDisabled() && m.HasType(CardType.Effect))
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool DrollAndLockBirdActivate()
        {
            return Duel.Player == 1;
        }

        // ═══════════════════════════════════════════════════════════════
        //  STARTERS & MONSTER EFFECTS
        // ═══════════════════════════════════════════════════════════════

        private bool MotorbikeActivate()
        {
            // Discard to search SHS monster: Wakaushi first, or Soulpiercer/Scales
            if (Card.Location == CardLocation.Hand)
            {
                if (!Bot.Hand.Any(c => c.Id == CardId.ProdigyWakaushi))
                    AI.SelectCard(CardId.ProdigyWakaushi);
                else
                    AI.SelectCard(CardId.Soulpiercer, CardId.Scales, CardId.SoulbusterGauntlet);
                return true;
            }
            return false;
        }

        private bool WakaushiActivate()
        {
            // If in Hand: place in P-Zone
            if (Card.Location == CardLocation.Hand)
            {
                return true;
            }
            // If in P-Zone: activate P-Scale effect (place Monk Big Benkei and SS self)
            if (Card.Location == CardLocation.SpellZone)
            {
                AI.SelectCard(CardId.MonkBigBenkei);
                return true;
            }
            // If in Extra Deck (used as Synchro material): place back in P-Zone
            if (Card.Location == CardLocation.Extra)
            {
                return true;
            }
            return false;
        }

        private bool MonkBenkeiActivate()
        {
            // If in Extra Deck: place back in P-Zone
            if (Card.Location == CardLocation.Extra)
            {
                return true;
            }

            // If in Hand:
            // 1. Place in P-Zone if Scale 1 is empty to enable search
            // 2. Or SS self by dumping Big Benkei from deck/hand to GY
            if (Card.Location == CardLocation.Hand)
            {
                bool hasScale = Bot.GetSpells().Any(s => s.IsFaceup() && s.Id == CardId.MonkBigBenkei);
                if (!hasScale)
                {
                    return true;
                }
                if (Bot.HasInHand(CardId.BigBenkei) || GetRemainingCount(CardId.BigBenkei) > 0)
                {
                    AI.SelectCard(CardId.BigBenkei);
                    return true;
                }
                return true;
            }

            // P-Scale effect: search 1 Superheavy Soul monster
            if (Card.Location == CardLocation.SpellZone)
            {
                if (!Bot.Hand.Any(c => c.Id == CardId.Soulpiercer))
                    AI.SelectCard(CardId.Soulpiercer);
                else if (!Bot.Hand.Any(c => c.Id == CardId.SoulbusterGauntlet))
                    AI.SelectCard(CardId.SoulbusterGauntlet);
                else
                    AI.SelectCard(CardId.Soulpeacemaker);
                return true;
            }
            return false;
        }

        private bool ScalesNormalSummon()
        {
            // Normal Summon Scales if we have targets in GY to revive
            return Bot.Graveyard.Any(c => c.IsMonster() && c.HasSetcode(0x9a) && c.Level <= 4 && c.Id != CardId.Scales);
        }

        private bool ScalesInherentSpSummon()
        {
            // Inherent SS from hand if opponent has 2+ monsters and bot has 0
            return Enemy.GetMonsterCount() >= 2 && Bot.GetMonsterCount() == 0;
        }

        private bool ScalesActivate()
        {
            // Revive Level 4 or lower SHS from GY in Defense Position
            ClientCard target = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.Soulpiercer)
                             ?? Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.Motorbike)
                             ?? Bot.Graveyard.FirstOrDefault(c => c.IsMonster() && c.HasSetcode(0x9a) && c.Level <= 4 && c.Id != CardId.Scales);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SoulpiercerSummon()
        {
            // Normal Summon Soulpiercer if we have a Tuner, extender, or if board is empty
            return Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasType(CardType.Tuner))
                || Bot.Hand.Any(c => c.Id == CardId.Trumpeter)
                || Bot.Hand.Any(c => c.Id == CardId.Soulpeacemaker)
                || Bot.GetMonsterCount() == 0;
        }

        private bool SoulpiercerEffect()
        {
            // Trigger in GY: Search SHS monster from Deck
            if (!Bot.Hand.Any(c => c.Id == CardId.SoulbusterGauntlet))
                AI.SelectCard(CardId.SoulbusterGauntlet);
            else if (!Bot.Hand.Any(c => c.Id == CardId.Trumpeter) && !Bot.GetMonsters().Any(m => m.Id == CardId.Trumpeter))
                AI.SelectCard(CardId.Trumpeter);
            else if (!Bot.Hand.Any(c => c.Id == CardId.Scales))
                AI.SelectCard(CardId.Scales);
            else
                AI.SelectCard(CardId.BigWaraji, CardId.BigBenkei);
            return true;
        }

        private bool MagnetSummon()
        {
            return Bot.Hand.Any(c => c.IsMonster() && c.HasSetcode(0x9a) && c.Level <= 4 && c.Id != CardId.Magnet);
        }

        private bool MagnetEffect()
        {
            ClientCard target = Bot.Hand.FirstOrDefault(c => c.IsMonster() && c.HasSetcode(0x9a) && c.Level <= 4 && c.Id != CardId.Magnet);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool FlutistSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool FlutistEffect()
        {
            // Tribute from field to SS SHS from hand
            if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard target = Bot.Hand.FirstOrDefault(c => c.IsMonster() && c.HasSetcode(0x9a));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool TrumpeterSpSummon()
        {
            return Bot.GetMonsters().Any(m => m.HasSetcode(0x9a));
        }

        private bool BigWarajiSpSummon()
        {
            return Bot.GetMonsterCount() == 0 || Bot.GetMonsters().Any(m => m.HasType(CardType.Tuner));
        }

        private bool FistEffect()
        {
            // GY Effect: Target 1 SHS Synchro on field, reduce Level by 1, SS Fist from GY
            if (Card.Location == CardLocation.Grave)
            {
                ClientCard synchro = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasType(CardType.Synchro) && m.HasSetcode(0x9a) && m.Level >= 6);
                if (synchro != null)
                {
                    AI.SelectCard(synchro);
                    return true;
                }
            }
            return false;
        }

        private bool SoulpeacemakerEffect()
        {
            // In Hand: Equip to face-up SHS monster (prefer Soulpiercer for search trigger on tribute)
            if (Card.Location == CardLocation.Hand)
            {
                ClientCard target = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Id == CardId.Soulpiercer)
                                 ?? Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasSetcode(0x9a));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
                return false;
            }
            // On Field (equipped): Tribute equipped monster to SS SHS from deck
            if (Card.Location == CardLocation.SpellZone)
            {
                AI.SelectCard(CardId.Scales, CardId.ProdigyWakaushi, CardId.BigBenkei, CardId.Trumpeter, CardId.BigWaraji);
                return true;
            }
            return false;
        }

        private bool FistNormalSummon()
        {
            // Normal Summon Fist if we can equip Soulpeacemaker or extend with BigWaraji, or board is empty
            return Bot.Hand.Any(c => c.Id == CardId.Soulpeacemaker)
                || Bot.Hand.Any(c => c.Id == CardId.BigWaraji)
                || Bot.GetMonsterCount() == 0;
        }

        private bool FallbackNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXTRA DECK SYNCHROS
        // ═══════════════════════════════════════════════════════════════

        private bool NaturiaBeastSpSummon()
        {
            // Level 5 EARTH Synchro: 1 EARTH Tuner + 1+ EARTH non-Tuner
            // Make Naturia Beast to lock down spells
            return !Bot.GetMonsters().Any(m => m.Id == CardId.NaturiaBeast);
        }

        private bool ShutendojiSpSummon()
        {
            // Synchro Level 6: Wipe all opponent Spells/Traps
            return Enemy.GetSpellCount() > 0;
        }

        private bool ShutendojiEffect()
        {
            return true;
        }

        private bool MusashiSpSummon()
        {
            return Bot.Graveyard.Any(c => c.IsMonster() && c.HasRace(CardRace.Machine) && c.Id != CardId.SwordmasterMusashi);
        }

        private bool MusashiEffect()
        {
            ClientCard target = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.Soulpiercer)
                             ?? Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.SoulbusterGauntlet)
                             ?? Bot.Graveyard.FirstOrDefault(c => c.IsMonster() && c.HasRace(CardRace.Machine));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool StealthNinjaSpSummon()
        {
            // Level 7: 2800 DEF direct attacker & revives on effect destruction
            return true;
        }

        private bool SarutobiSpSummon()
        {
            return true;
        }

        private bool ShanawoSpSummon()
        {
            return true;
        }

        private bool BeastKyubiSpSummon()
        {
            // Level 9: Gains 900 DEF per opponent Special Summoned monster
            return Enemy.GetMonsters().Count(m => m.IsSpecialSummoned) >= 2;
        }

        private bool SusanowoSpSummon()
        {
            return true;
        }

        private bool MasurawoSpSummon()
        {
            return true;
        }

        private bool MasurawoEffect()
        {
            // Draw until 3 cards in hand
            return true;
        }

        private bool SteamTrainKingSpSummon()
        {
            return true;
        }

        private bool SteamTrainKingEffect()
        {
            // Discard up to 2 cards to destroy up to 2 opponent cards
            var targets = Enemy.GetMonsters().Concat(Enemy.GetSpells()).ToList();
            if (targets.Count > 0 && Bot.Hand.Count > 0)
            {
                var viable = targets.Where(c => !IsTargetImmune(c)).ToList();
                var chosen = (viable.Count > 0 ? viable : targets).Take(Math.Min(2, Bot.Hand.Count)).ToList();
                AI.SelectCard(chosen);
                return true;
            }
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  HEURISTICS: RESOURCE STOP, COMBAT & SELECTION
        // ═══════════════════════════════════════════════════════════════

        protected override bool ShouldStopExtending()
        {
            // Don't stop if we have a Tuner and another monster ready to Synchro climb
            if (Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasType(CardType.Tuner)) && Bot.GetMonsterCount() >= 2)
            {
                return false;
            }
            // Don't stop if we don't have a major boss presence on field
            if (!Bot.GetMonsters().Any(m => m.IsFaceup() && (m.Id == CardId.BraveMasurawo || m.Id == CardId.SteamTrainKing || m.Id == CardId.WarlordSusanowo || m.Id == CardId.NaturiaBeast || m.Id == CardId.StealthNinja)))
            {
                return false;
            }
            return base.ShouldStopExtending();
        }

        private bool RepositionStrategy()
        {
            // Superheavy Samurai monsters fight using DEF in Defense Position!
            if (Card.HasSetcode(0x9a) && Card.Defense >= Card.Attack && Card.IsAttack())
            {
                return true; // Switch to Defense Position!
            }
            return false;
        }

        public override ClientCard OnSelectAttacker(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            if (attackers == null || attackers.Count == 0) return null;
            // Order attackers by effective battle power (DEF if defense attacker, else ATK)
            var sorted = attackers
                .Where(a => a != null && (a.IsAttack() || a.IsMonsterAttackWhileInDefPos()))
                .OrderByDescending(a => a.GetAttackPower())
                .ToList();
            return sorted.Count > 0 ? sorted.First() : base.OnSelectAttacker(attackers, defenders);
        }

        public override bool OnPreBattleBetween(ClientCard attacker, ClientCard defender)
        {
            if (attacker.IsMonsterAttackWhileInDefPos() || (attacker.HasSetcode(0x9a) && attacker.IsDefense()))
            {
                attacker.RealPower = attacker.Defense;
                if (Bot.Hand.Any(c => c.Id == CardId.SoulbusterGauntlet))
                {
                    attacker.RealPower *= 2;
                }
            }
            if (attacker.RealPower <= 0)
                return false;
            return base.OnPreBattleBetween(attacker, defender);
        }

        public override BattlePhaseAction OnBattle(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            if (attackers == null || attackers.Count == 0) return null;

            if (defenders == null || defenders.Count == 0)
            {
                var directAttacker = attackers
                    .Where(c => c != null && c.IsFaceup() && (c.IsAttack() || c.IsMonsterAttackWhileInDefPos()) && (c.GetAttackPower() > 0 || c.Attack > 0))
                    .OrderByDescending(c => c.GetAttackPower())
                    .FirstOrDefault();

                if (directAttacker != null)
                {
                    return AI.Attack(directAttacker, null);
                }
            }
            else
            {
                // Prioritize attacking dangerous/negator defenders or clearing monsters we can beat
                foreach (var attacker in attackers.Where(a => a != null && a.IsFaceup()))
                {
                    int attackerPower = (attacker.IsMonsterAttackWhileInDefPos() || (attacker.HasSetcode(0x9a) && attacker.IsDefense()))
                        ? attacker.Defense : attacker.Attack;
                    if (Bot.Hand.Any(c => c.Id == CardId.SoulbusterGauntlet))
                        attackerPower *= 2;

                    var target = defenders
                        .Where(d => d != null && !d.IsMonsterInvincible() && !CardIntelligence.IsDangerousBattleTarget(d, attacker))
                        .OrderByDescending(d => CardIntelligence.IsKnownNegator(d.Id) ? 10000 : d.GetDefensePower())
                        .FirstOrDefault(d => attackerPower > d.GetDefensePower() || (attackerPower >= d.GetDefensePower() && d.IsAttack()));

                    if (target != null)
                    {
                        return AI.Attack(attacker, target);
                    }
                }
            }

            return base.OnBattle(attackers, defenders);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Superheavy Samurai bosses and defense fighters must be placed in Defense Position!
            if (cardId == CardId.BraveMasurawo || cardId == CardId.SteamTrainKing ||
                cardId == CardId.WarlordSusanowo || cardId == CardId.NinjaSarutobi ||
                cardId == CardId.CommanderShanawo || cardId == CardId.OgreShutendoji ||
                cardId == CardId.SwordmasterMusashi || cardId == CardId.BeastKyubi ||
                cardId == CardId.StealthNinja || cardId == CardId.BigBenkei)
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

            // Rule 1: Isolation of HINTMSG_ATOHAND (506) - Deck search
            if (hint == 506)
            {
                var preferred = new List<int>
                {
                    CardId.ProdigyWakaushi,
                    CardId.Soulpiercer,
                    CardId.SoulbusterGauntlet,
                    CardId.Trumpeter,
                    CardId.Scales,
                    CardId.Soulpeacemaker,
                    CardId.MonkBigBenkei,
                    CardId.BigWaraji,
                    CardId.BigBenkei
                };

                var matches = cards.Where(c => preferred.Contains(c.Id))
                                   .OrderBy(c => preferred.IndexOf(c.Id))
                                   .ToList();

                if (matches.Count >= min)
                    return matches.Take(max).ToList();
            }

            // Rule 2: Destruction (502) or Banish (504) must target enemy cards (c.Controller == 1)
            if (hint == 502 || hint == 504)
            {
                var enemyTargets = cards.Where(c => c.Controller == 1 && !IsTargetImmune(c)).ToList();
                if (enemyTargets.Count >= min)
                {
                    return enemyTargets.OrderByDescending(c => GetCardThreatScore(c)).Take(max).ToList();
                }
            }

            // Rule 3: Special Summon (509)
            if (hint == 509)
            {
                var preferredSS = new List<int>
                {
                    CardId.BraveMasurawo,
                    CardId.SteamTrainKing,
                    CardId.WarlordSusanowo,
                    CardId.NaturiaBeast,
                    CardId.NinjaSarutobi,
                    CardId.StealthNinja,
                    CardId.CommanderShanawo,
                    CardId.Scales,
                    CardId.Soulpiercer,
                    CardId.Trumpeter,
                    CardId.Motorbike
                };

                var matches = cards.Where(c => preferredSS.Contains(c.Id))
                                   .OrderBy(c => preferredSS.IndexOf(c.Id))
                                   .ToList();

                if (matches.Count >= min)
                    return matches.Take(max).ToList();
            }

            // Rule 4: Discard / Send to GY / Tribute (500, 501, 508)
            if (hint == 500 || hint == 501 || hint == 508)
            {
                var discardFodder = new List<int>
                {
                    CardId.Soulpiercer,   // triggers search on send
                    CardId.Gigagloves,    // active in GY
                    CardId.Flutist,       // active in GY
                    CardId.Fist,          // active in GY
                    CardId.Motorbike,
                    CardId.BigBenkei,
                    CardId.BigWaraji
                };

                var matches = cards.Where(c => c.Controller == 0 && discardFodder.Contains(c.Id))
                                   .OrderBy(c => discardFodder.IndexOf(c.Id))
                                   .ToList();

                if (matches.Count >= min)
                    return matches.Take(min).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
