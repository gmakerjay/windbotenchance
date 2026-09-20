// ============================================================================
// CARD AUDIT — Anime_Gong (Gong Strong's Steadfast Superheavy Samurai Defense Army)
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
// | Superheavy Samurai Soulpeacemaker  | Monster L1   | Yes  | Yes   | Tribute | Equip; tribute equipped SHS to SS from Deck   | Equipped to SHS; cheat out Big Benkei         | S/T in GY                                   |
// | Superheavy Samurai Soulbuster Gaunt| Monster L1   | Yes  | Yes   | Discard | Hand DEF Honest: Double DEF during damage calc| Damage Step; double DEF (up to 9600 damage!)  | No battle / already lethal                  |
// | Superheavy Samurai Flutist         | Monster L3   | Yes  | Yes   | Tribute | Tribute to SS from hand; GY banish negate tar | In hand: extend; in GY: protect SHS from tar  | No targeting effect                         |
// | Superheavy Samurai Gigagloves      | Monster L3   | Yes  | Yes   | Banish  | GY banish on direct attack: drop ATK to 0 & dr| Opponent declares direct attack               | Bot has monsters on field                   |
// | Superheavy Samurai Trumpeter       | Monster L2 T | Yes  | Yes   | None    | SS from hand if no S/T in GY; synchro tuner   | In hand; need Tuner for climbing              | S/T in GY                                   |
// | Superheavy Samurai Fist            | Monster L2 T | Yes  | Yes   | Target  | SS from GY by bouncing Synchro; adjust level  | In GY; need Tuner revival                     | S/T in GY                                   |
// | Superheavy Samurai Big Waraji      | Monster L5   | No   | No    | None    | SS from hand if no S/T in GY; counts as 2 trib| In hand; free Level 5 body                    | S/T in GY                                   |
// | Superheavy Samurai Big Benkei      | Monster L8   | No   | No    | None    | 3500 DEF; attacks while in Defense Position   | Field boss attacker using 3500 DEF            | Cannot attack                               |
// | Baronne de Fleur                   | Synchro L10  | Yes  | Yes   | None    | 3000 ATK; Quick Omni-negate; once target pop  | Opponent activates card/effect or threat pop  | Already negated this duel                   |
// | Superheavy Samurai Brave Masurawo  | Synchro L10  | Yes  | Yes   | None    | 4000 DEF; battles in DEF; draw up to 3 on S/T | Opponent activates S/T; draw cards + battle   | S/T in GY                                   |
// | Superheavy Samurai Warlord Susanowo| Synchro L10  | Yes  | Yes   | None    | 3800 DEF; Quick: Steal 1 opp S/T from their GY| Opponent GY has useful Spell/Trap (Reborn etc)| Opponent GY has no S/T                      |
// | Superheavy Samurai Ninja Sarutobi  | Synchro L8   | Yes  | Yes   | None    | 2800 DEF; Quick: Pop 1 opp S/T + 500 burn     | Opponent controls Spell/Trap                  | Opponent controls 0 S/T                     |
// | Superheavy Samurai Ogre Shutendoji | Synchro L6   | Yes  | Yes   | None    | On Synchro Summon: Wipe all opp Spells/Traps  | Opponent controls 1+ Spells/Traps             | Opponent controls 0 S/T                     |
// | Superheavy Samurai Steam Train King| Synchro L12  | Yes  | Yes   | Discard | 4800 DEF direct attack; pop 2 cards; draw     | Boss finisher with 4800 DEF direct attack     | Cannot attack                               |
// | Superheavy Samurai Scarecrow       | Link-1       | Yes  | Yes   | Discard | Discard 1 to revive Superheavy from GY in DEF | Need GY revival to continue climbing          | Hand empty / GY empty                       |
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
            public const int Stealthy = 78391364;
            public const int AshBlossom = 14558127;
            public const int GhostOgre = 59438930;
            public const int EffectVeiler = 97268402;
            public const int DrollAndLockBird = 94145021;

            // Extra Deck
            public const int SteamTrainKing = 17775525;
            public const int BraveMasurawo = 64193046;
            public const int BaronneDeFleur = 84815190;
            public const int WarlordSusanowo = 494922;
            public const int CommanderShanawo = 90587641;
            public const int NinjaSarutobi = 76471944;
            public const int OgreShutendoji = 36953371;
            public const int SwordmasterMusashi = 75988594;
            public const int Scarecrow = 33918636;
        }

        public Anime_GongExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterComboLines();
            RegisterExecutors();
        }

        private void RegisterComboLines()
        {
            // ── Line 1: 1-Card Wakaushi Engine Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "SHS-Wakaushi-Full-Climb",
                RequiredCards = new List<int> { CardId.ProdigyWakaushi },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.ProdigyWakaushi, ActionType = ExecutorType.Activate, Description = "Place Wakaushi in Pendulum Scale" },
                    new() { CardId = CardId.ProdigyWakaushi, ActionType = ExecutorType.Activate, Description = "Wakaushi place Monk Benkei & Special Summon self" },
                    new() { CardId = CardId.MonkBigBenkei, ActionType = ExecutorType.Activate, Description = "Monk Benkei search Soulpiercer/Soulbuster" }
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
                    new() { CardId = CardId.ProdigyWakaushi, ActionType = ExecutorType.Activate, Description = "Place Wakaushi in Scale" }
                }
            });
        }

        private void RegisterExecutors()
        {
            // ═══════════════════════════════════════════════════════════════
            //  TIER 0: QUICK DISRUPTIONS, NEGATES & COMBAT TRICKS
            // ═══════════════════════════════════════════════════════════════

            // Baronne de Fleur — Quick Omni-Negate
            AddExecutor(ExecutorType.Activate, CardId.BaronneDeFleur, BaronneNegateActivate);

            // Warlord Susanowo — Quick steal Spell/Trap from opponent's GY!
            AddExecutor(ExecutorType.Activate, CardId.WarlordSusanowo, SusanowoStealActivate);

            // Ninja Sarutobi — Quick pop opponent Spell/Trap + 500 burn
            AddExecutor(ExecutorType.Activate, CardId.NinjaSarutobi, SarutobiPopActivate);

            // Soulbuster Gauntlet — Hand DEF Honest (Double DEF in damage calc)
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
            //  TIER 2: NORMAL SUMMONS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Summon, CardId.Soulpiercer, SoulpiercerSummon);
            AddExecutor(ExecutorType.Activate, CardId.Soulpiercer, SoulpiercerEffect);

            AddExecutor(ExecutorType.Summon, CardId.Magnet, MagnetSummon);
            AddExecutor(ExecutorType.Activate, CardId.Magnet, MagnetEffect);

            AddExecutor(ExecutorType.Summon, CardId.Flutist, FlutistSummon);
            AddExecutor(ExecutorType.Activate, CardId.Flutist, FlutistEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: SPECIAL SUMMON EXTENDERS & EQUIPS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.Trumpeter, TrumpeterSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Stealthy, StealthySpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BigWaraji, BigWarajiSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Fist, FistEffect);

            // Soulpeacemaker equip & tribute
            AddExecutor(ExecutorType.Activate, CardId.Soulpeacemaker, SoulpeacemakerEffect);

            // Fallback Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.Trumpeter, FallbackNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.Gigagloves, FallbackNormalSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: EXTRA DECK SYNCHRO & LINK PROGRESSIONS
            // ═══════════════════════════════════════════════════════════════

            // Link-1 Scarecrow (discard 1 to revive SHS from GY)
            AddExecutor(ExecutorType.SpSummon, CardId.Scarecrow, ScarecrowSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Scarecrow, ScarecrowEffect);

            // Level 6 Shutendoji (Wipe opponent Spells/Traps)
            AddExecutor(ExecutorType.SpSummon, CardId.OgreShutendoji, ShutendojiSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.OgreShutendoji, ShutendojiEffect);

            // Level 5 Musashi (Recycle Machine)
            AddExecutor(ExecutorType.SpSummon, CardId.SwordmasterMusashi, MusashiSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SwordmasterMusashi, MusashiEffect);

            // Level 8 Sarutobi & Shanawo
            AddExecutor(ExecutorType.SpSummon, CardId.NinjaSarutobi, SarutobiSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CommanderShanawo, ShanawoSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.CommanderShanawo, ShanawoEffect);

            // Level 10 Bosses: Baronne, Brave Masurawo, Warlord Susanowo
            AddExecutor(ExecutorType.SpSummon, CardId.BaronneDeFleur, BaronneSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BraveMasurawo, MasurawoSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BraveMasurawo, MasurawoEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.WarlordSusanowo, SusanowoSpSummon);

            // Level 12 Steam Train King (4800 DEF direct attack)
            AddExecutor(ExecutorType.SpSummon, CardId.SteamTrainKing, SteamTrainKingSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SteamTrainKing, SteamTrainKingEffect);

            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        // ═══════════════════════════════════════════════════════════════
        //  DISRUPTIONS & COMBAT TRICKS
        // ═══════════════════════════════════════════════════════════════

        private bool BaronneNegateActivate()
        {
            ClientCard lastCard = LastChainCard;
            return lastCard != null && lastCard.Controller == 1;
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
            // Pop 1 opponent Spell/Trap + 500 burn
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

        private bool SoulbusterActivate()
        {
            // Damage calc DEF Honest
            if (Card.Location == CardLocation.Hand && Duel.Phase == DuelPhase.Damage)
            {
                ClientCard battling = Bot.BattlingMonster;
                ClientCard enemyMon = Enemy.BattlingMonster;
                if (battling != null && enemyMon != null && battling.HasSetcode(0x9a))
                {
                    if (battling.Defense <= enemyMon.Attack || enemyMon.Attack >= 2500)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool FlutistGYProtectActivate()
        {
            return Card.Location == CardLocation.Grave;
        }

        private bool GigaglovesActivate()
        {
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
            AI.SelectCard(CardId.ProdigyWakaushi, CardId.Soulpiercer);
            return true;
        }

        private bool WakaushiActivate()
        {
            return true;
        }

        private bool MonkBenkeiActivate()
        {
            AI.SelectCard(CardId.Soulpiercer, CardId.SoulbusterGauntlet, CardId.Soulpeacemaker);
            return true;
        }

        private bool SoulpiercerSummon()
        {
            return true;
        }

        private bool SoulpiercerEffect()
        {
            // Search SHS monster
            AI.SelectCard(CardId.SoulbusterGauntlet, CardId.Trumpeter, CardId.BigBenkei);
            return true;
        }

        private bool MagnetSummon()
        {
            return true;
        }

        private bool MagnetEffect()
        {
            ClientCard target = Bot.Hand.FirstOrDefault(c => c.IsMonster() && c.HasSetcode(0x9a) && c.Level <= 4);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool FlutistSummon()
        {
            return true;
        }

        private bool FlutistEffect()
        {
            ClientCard target = Bot.Hand.FirstOrDefault(c => c.IsMonster() && c.HasSetcode(0x9a) && c.Level >= 5);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool TrumpeterSpSummon()
        {
            return Bot.GetMonsters().Any(m => m.HasSetcode(0x9a));
        }

        private bool StealthySpSummon()
        {
            return Bot.GetMonsters().Any(m => m.HasSetcode(0x9a));
        }

        private bool BigWarajiSpSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool FistEffect()
        {
            ClientCard synchro = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasType(CardType.Synchro) && m.HasSetcode(0x9a));
            if (synchro != null)
            {
                AI.SelectCard(synchro);
                return true;
            }
            return false;
        }

        private bool SoulpeacemakerEffect()
        {
            ClientCard target = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasSetcode(0x9a));
            if (target != null)
            {
                AI.SelectCard(target);
                AI.SelectNextCard(CardId.BigBenkei);
                return true;
            }
            return false;
        }

        private bool FallbackNormalSummon()
        {
            return Bot.GetMonsterCount() < 2;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXTRA DECK SYNCHROS & LINK
        // ═══════════════════════════════════════════════════════════════

        private bool ScarecrowSpSummon()
        {
            return Bot.Graveyard.Any(c => c.IsMonster() && c.HasSetcode(0x9a));
        }

        private bool ScarecrowEffect()
        {
            ClientCard discard = Bot.Hand.FirstOrDefault(c => c.Id == CardId.Soulpiercer)
                             ?? Bot.Hand.FirstOrDefault();
            if (discard != null)
            {
                AI.SelectCard(discard);
                ClientCard revive = Bot.Graveyard.FirstOrDefault(c => c.IsMonster() && c.HasSetcode(0x9a) && c.Id != discard.Id);
                if (revive != null)
                {
                    AI.SelectNextCard(revive);
                    return true;
                }
            }
            return false;
        }

        private bool ShutendojiSpSummon()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool ShutendojiEffect()
        {
            return true;
        }

        private bool MusashiSpSummon()
        {
            return true;
        }

        private bool MusashiEffect()
        {
            ClientCard target = Bot.Graveyard.FirstOrDefault(c => c.IsMonster() && c.HasRace(CardRace.Machine));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SarutobiSpSummon()
        {
            return true;
        }

        private bool ShanawoSpSummon()
        {
            return true;
        }

        private bool ShanawoEffect()
        {
            return true;
        }

        private bool BaronneSpSummon()
        {
            return true;
        }

        private bool MasurawoSpSummon()
        {
            return true;
        }

        private bool MasurawoEffect()
        {
            return true;
        }

        private bool SusanowoSpSummon()
        {
            return true;
        }

        private bool SteamTrainKingSpSummon()
        {
            return true;
        }

        private bool SteamTrainKingEffect()
        {
            // Pop up to 2 cards
            var targets = Enemy.GetMonsters().Concat(Enemy.GetSpells()).Take(2).ToList();
            if (targets.Count > 0 && Bot.Hand.Count > 0)
            {
                AI.SelectCard(targets);
                return true;
            }
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  HEURISTICS: STEADFAST DEFENSE POSITION COMBAT
        // ═══════════════════════════════════════════════════════════════

        private bool RepositionStrategy()
        {
            // Superheavy Samurai with high DEF battle in Defense Position!
            if (Card.HasSetcode(0x9a) && Card.Defense >= Card.Attack && Card.IsAttack())
            {
                return true; // Switch to Defense Position!
            }
            return false;
        }

        public override bool OnSelectYesNo(long desc)
        {
            return base.OnSelectYesNo(desc);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards != null && cards.Count > 0)
            {
                var preferred = cards.Where(c => c.Id == CardId.ProdigyWakaushi ||
                                                c.Id == CardId.Soulpiercer ||
                                                c.Id == CardId.SoulbusterGauntlet ||
                                                c.Id == CardId.MonkBigBenkei).ToList();
                if (preferred.Count >= min)
                {
                    return preferred.Take(max).ToList();
                }
            }
            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
