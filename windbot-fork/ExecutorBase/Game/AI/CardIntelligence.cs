using System.Collections.Generic;
using YGOSharp.OCGWrapper.Enums;
using WindBot.Game;

namespace WindBot.Game.AI
{
    /// <summary>
    /// Centralized Universal Card Intelligence Database.
    /// Consolidates threat categories, floodgates, negators, chokepoints, handtraps,
    /// and immunities previously duplicated and hardcoded across dozens of executors.
    /// Provides high-performance O(1) lookups for all Core engines and Deck executors.
    /// </summary>
    public static class CardIntelligence
    {
        // ═══════════════════════════════════════════════════════════════
        //  1. SPECIAL SUMMON & FLOODGATE MONSTERS
        // ═══════════════════════════════════════════════════════════════
        private static readonly HashSet<int> FloodgateMonsters = new HashSet<int>
        {
            42009023,  // Fossil Dyna Pachycephalo
            42009836,  // Fossil Dyna (alt)
            7902349,   // Jowgen the Spiritualist
            15397015,  // Inspect Boarder
            19261966,  // El Shaddoll Winda (1 SS per turn)
            78193831,  // Vanity's Fiend
            47084486,  // Majesty's Fiend
            67922702,  // Archlord Kristya
            96015934,  // Vanity's Ruler
            14212200,  // Amano-Iwato
            71564252,  // Thunder King Rai-Oh
            15291624,  // Thunder Dragon Colossus
            26273196,  // Number 41: Bagooska the Terribly Tired Tapir
            85359414,  // Number 41: Bagooska (alt)
            3717252,   // Koa'ki Meiru Drago
            94977269,  // Naturia Exterio
            33198837,  // Naturia Beast
            72634965,  // Denko Sekka
            59509952,  // Lose 1 Turn (monster)
            // Barrier Statues
            10963799,  // Barrier Statue of the Stormwinds
            19740112,  // Barrier Statue of the Drought
            47961808,  // Barrier Statue of the Inferno
            73356503,  // Barrier Statue of the Abyss
            84478195,  // Barrier Statue of the Torrent
            86325573,  // Barrier Statue of the Heavens
        };

        // ═══════════════════════════════════════════════════════════════
        //  2. FLOODGATE & CONTINUOUS LOCK SPELLS / TRAPS
        // ═══════════════════════════════════════════════════════════════
        private static readonly HashSet<int> FloodgateSpellsTraps = new HashSet<int>
        {
            5851097,   // Vanity's Emptiness
            4514109,   // Kaiser Colosseum
            22046459,  // Rivalry of Warlords
            90845713,  // Rivalry of Warlords (alt)
            34487429,  // Gozen Match
            53334641,  // Gozen Match (alt)
            2429943,   // There Can Be Only One
            3188710,   // Summon Breaker
            47355498,  // Summon Limit
            92746535,  // Summon Limit (alt)
            81674782,  // Dimensional Fissure
            30241314,  // Macro Cosmos
            82732047,  // Skill Drain
            82732705,  // Skill Drain (alt)
            61740673,  // Imperial Order
            58921041,  // Anti-Spell Fragrance
            68462976,  // Secret Village of the Spellcasters
            10833828,  // Mistake
            34507039,  // Deck Lockdown
            83326048,  // Dimensional Barrier
            4149689,   // Mistaken Arrest
            67616300,  // Chicken Game
            48680970,  // Eternal Soul
            38009249,  // Runick Fountain
        };

        // ═══════════════════════════════════════════════════════════════
        //  3. KNOWN NEGATORS & DISRUPTIONS (Bosses / Quick Effects)
        // ═══════════════════════════════════════════════════════════════
        private static readonly HashSet<int> KnownNegators = new HashSet<int>
        {
            84815190,  // Baronne de Fleur
            4280258,   // Apollousa, Bow of the Goddess
            50954680,  // Crystal Wing Synchro Dragon
            10443957,  // Cyber Dragon Infinity
            86066372,  // Herald of Ultimateness
            44665365,  // Herald of Perfection
            31801517,  // Evolzar Dolkka
            42752141,  // Evolzar Laggia
            57793869,  // Borreload Savage Dragon
            17330115,  // Hot Red Dragon Archfiend Abyss
            21522601,  // Witchcrafter Madame Verre
            84523092,  // Witchcrafter Haine
            1508649,   // Altergeist Hexstia
            1561110,   // ABC-Dragon Buster
            63767246,  // Number 38: Hope Harbinger Dragon Titanic Galaxy
            44146295,  // Mirrorjade the Iceblade Dragon
            29301450,  // S:P Little Knight
            92892239,  // Borreload Furious Dragon
            51409648,  // Rindbrumm the Striking Dragon
            53971455,  // Despian Luluwalilith
            78397661,  // Ecclesia and the Dark Dragon
            76666602,  // The Dragon That Devours the Dogma
            37675907,  // Red-Eyes Dark Dragoon
            90809975,  // Toadally Awesome
            73580471,  // Black Rose Dragon (wipe)
            46772449,  // Evilswarm Exciton Knight (wipe)
        };

        // ═══════════════════════════════════════════════════════════════
        //  4. HIGH-THREAT CHOKEPOINTS (Starters / Key Enablers)
        // ═══════════════════════════════════════════════════════════════
        private static readonly HashSet<int> HighThreatChokepoints = new HashSet<int>
        {
            // Altergeist engine
            42790071,  // Altergeist Multifaker
            53936268,  // Personal Spoofing
            25533642,  // Altergeist Meluseek
            27541563,  // Altergeist Protocol
            35146019,  // Altergeist Manifestation
            // ABC engine
            66970002,  // Union Hangar
            77411244,  // B-Buster Drake
            99249638,  // Union Driver
            46659709,  // Galaxy Soldier
            // Dark Magician engine
            47222536,  // Dark Magical Circle
            38033121,  // Dark Magical Circle (alt)
            41721210,  // Dark Magician the Dragon Knight
            97077563,  // Call of the Haunted
            // Blue-Eyes engine
            71039903,  // The White Stone of Ancients
            79814787,  // The White Stone of Legend
            8240199,   // Sage with Eyes of Blue
            48800175,  // The Melody of Awakening Dragon
            // Modern Meta Engines
            44362883,  // Branded Fusion
            62962630,  // Aluber the Jester of Despia
            1984618,   // Nadir Servant
            25311006,  // Triple Tactics Talent
            48130397,  // Super Polymerization
            35261759,  // Pot of Desires
            72426662,  // Pot of Extravagance
            49238328,  // Pot of Extravagance (alt)
            55144522,  // Pot of Greed
            79571449,  // Graceful Charity
            44763025,  // Delinquent Duo
            32807846,  // Reinforcement of the Army
            18144506,  // Harpie's Feather Duster
            18144507,  // Harpie's Feather Duster (alt)
            14532163,  // Lightning Storm
            12580477,  // Raigeki
            5318639,   // Mystical Space Typhoon
            8267140,   // Cosmic Cyclone
            35269904,  // Cosmic Cyclone (alt)
        };

        // ═══════════════════════════════════════════════════════════════
        //  5. UNIVERSAL HANDTRAPS
        // ═══════════════════════════════════════════════════════════════
        private static readonly HashSet<int> UniversalHandtraps = new HashSet<int>
        {
            14558127,  // Ash Blossom & Joyous Spring
            14558128,  // Ash Blossom (alt art)
            23434538,  // Maxx "C"
            94145021,  // Droll & Lock Bird
            97268402,  // Effect Veiler
            63845230,  // Effect Veiler (alt)
            10045474,  // Infinite Impermanence
            42141493,  // Mulcharmy Fuwalos
            84192580,  // Mulcharmy Purulia
            87126721,  // Mulcharmy Nyalus
            73642296,  // Ghost Belle & Haunted Mansion
            59438930,  // Ghost Ogre & Snow Rabbit
            29726552,  // Ghost Sister & Spooky Dogwood
            34267821,  // Artifact Lancea
            27204311,  // Nibiru, the Primal Being
            24224830,  // Called by the Grave
            65681982,  // Crossout Designator
            24299458,  // Forbidden Droplet
            41420027,  // Solemn Judgment / Red Reboot
        };

        // ═══════════════════════════════════════════════════════════════
        //  6. TARGET IMMUNITY & DANGEROUS BATTLE CARDS
        // ═══════════════════════════════════════════════════════════════
        private static readonly HashSet<int> TargetImmuneCards = new HashSet<int>
        {
            55410871,  // Blue-Eyes Chaos MAX Dragon
            41721210,  // Dark Magician the Dragon Knight (protects backrow)
            37675907,  // Red-Eyes Dark Dragoon
            21887175,  // Mekk-Knight Crusadia Avramax (untargetable by effects)
            88264978,  // Red-Eyes Flare Metal Dragon (destruction immune with mats)
        };

        // ═══════════════════════════════════════════════════════════════
        //  PUBLIC QUERY API (O(1) lookups)
        // ═══════════════════════════════════════════════════════════════

        public static bool IsFloodgate(int cardId)
        {
            return FloodgateMonsters.Contains(cardId) || FloodgateSpellsTraps.Contains(cardId);
        }

        public static bool IsFloodgateMonster(int cardId)
        {
            return FloodgateMonsters.Contains(cardId);
        }

        public static bool IsFloodgateSpellTrap(int cardId)
        {
            return FloodgateSpellsTraps.Contains(cardId);
        }

        public static bool IsKnownNegator(int cardId)
        {
            return KnownNegators.Contains(cardId);
        }

        public static bool IsHighThreatChokepoint(int cardId)
        {
            return HighThreatChokepoints.Contains(cardId) || KnownNegators.Contains(cardId) || FloodgateMonsters.Contains(cardId);
        }

        public static bool IsHandtrap(int cardId)
        {
            return UniversalHandtraps.Contains(cardId);
        }

        public static bool IsTargetImmune(int cardId)
        {
            return TargetImmuneCards.Contains(cardId);
        }

        public static bool IsTargetImmune(ClientCard card)
        {
            if (card == null) return false;
            if (TargetImmuneCards.Contains(card.Id)) return true;
            if (card.IsShouldNotBeTarget()) return true;
            return false;
        }

        public static bool IsSpecialSummonBlocked(ClientField enemy, ClientField bot = null)
        {
            if (enemy == null) return false;

            // Check enemy monsters for SS locks
            foreach (var m in enemy.GetMonsters())
            {
                if (m != null && m.IsFaceup() && !m.IsDisabled() && FloodgateMonsters.Contains(m.Id))
                    return true;
            }

            // Check enemy spells/traps for SS locks
            foreach (var s in enemy.GetSpells())
            {
                if (s != null && s.IsFaceup() && !s.IsDisabled() && FloodgateSpellsTraps.Contains(s.Id))
                    return true;
            }

            // Check our own field if provided (e.g. self-inflicted Vanity's or Winda)
            if (bot != null)
            {
                foreach (var m in bot.GetMonsters())
                {
                    if (m != null && m.IsFaceup() && !m.IsDisabled() && (m.Id == 78193831 || m.Id == 42009023)) // Vanity's / Fossil Dyna
                        return true;
                }
            }

            return false;
        }

        public static bool OpponentHasActiveNegator(ClientField enemy)
        {
            if (enemy == null) return false;
            foreach (var m in enemy.GetMonsters())
            {
                if (m != null && m.IsFaceup() && !m.IsDisabled() && KnownNegators.Contains(m.Id))
                    return true;
            }
            return false;
        }
    }
}
