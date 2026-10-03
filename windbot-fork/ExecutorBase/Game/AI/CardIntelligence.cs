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
    public static partial class CardIntelligence
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
            94977269,  // El Shaddoll Winda (1 SS per turn)
            94977270,  // El Shaddoll Winda (alt)
            19261966,  // El Shaddoll Anoyatyllis (no SpSummon from hand/GY by Spells/Traps)
            78193831,  // Vanity's Fiend
            47084486,  // Majesty's Fiend
            67922702,  // Archlord Kristya
            96015934,  // Vanity's Ruler
            14212200,  // Amano-Iwato
            71564252,  // Thunder King Rai-Oh
            15291624,  // Thunder Dragon Colossus
            90590303,  // Number 41: Bagooska the Terribly Tired Tapir
            90590304,  // Number 41: Bagooska (alt)
            26273196,  // Legacy fallback
            85359414,  // Legacy fallback
            3717252,   // Koa'ki Meiru Drago
            99916754,  // Naturia Exterio
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
            91279700,  // Evilswarm Ophion (Lv5+ SS Lock)
            93039339,  // Super Starslayer TY-PHON - Sky Crisis (>=3000 ATK effect lock)
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
            37818794,  // Red-Eyes Dark Dragoon
            37675907,  // Legacy ID mapping
            46396218,  // Azamina Ilia Silvia (Omni-Negate)
            4993187,   // W:P Fancy Ball (Monster Negate)
            90809975,  // Toadally Awesome
            73580471,  // Black Rose Dragon (wipe)
            46772449,  // Evilswarm Exciton Knight (wipe)
            1621413,   // Dark Requiem Xyz Dragon (3x monster effect negate + pop + revive)
            69946549,  // Predaplant Dragostapelia (Quick monster negate + counter)
            14529511,  // Mementomictlan Tecuhtlica - Creation King (Quick field pop)
            34909328,  // Ryzeal Detonator (Quick pop on opponent activation)
            85523502,  // Whisker Blitzclique (Quick monster negate + pop)
            23526128,  // Blitzclique Return Stroke (Quick spell negate + pop)
            63746411,  // Number 106: Giant Hand (Quick monster negate)
            27548199,  // Borreload Savage Dragon (Omni-negate)
            9940036,   // Mereologic Aggregator (GY send negation)
            70636044,  // Varudras, the Final Bringer of the End Times (Omni-negate & pop)
            79559912,  // D/D/D Wave High King Caesar (Special summon negate x2)
            7894706,   // The Chaotic Phantasmal Sacred Beasts (Monster negate x3 + LP gain)
            82135803,  // Fiendsmith's Desirae (Face-up cards negate)
            12067160,  // Gorgon of Zilofthonia (Activated effect negation)
            // Orcust Engine & WCQ Negators
            93854893,  // Dingirsu, the Orcust of the Evening Star (Send to GY / Board Protection)
            703897,    // Orcust Crescendo (Counter Trap Omni-Negate & Banish)
            76145142,  // Longirsu, the Orcust Orchestrator (Linked monster send to GY)
            74820316,  // Enlilgirsu, the Orcust Mekk-Knight (Take control / Field Send)
            27918365,  // World Legacy - "World Crown" (Extra Deck summon tribute negate)
            40366667,  // Dominus Impulse (Special Summon Negate & Destroy)
            6325660,   // Dominus Spark (Quick Pop on effect activation)
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
            48680970,  // Eternal Soul
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
            // Modern Meta Engines (Snake-Eye / Fiendsmith / Generic Staples)
            85106525,  // Bonfire
            80845034,  // WANTED: Seeker of Sinful Spoils
            89023486,  // Original Sinful Spoils - Snake-Eye
            9674034,   // Snake-Eye Ash
            90241276,  // Snake-Eyes Poplar
            2772337,   // Promethean Princess, Bestower of Flames
            60764609,  // Fiendsmith Engraver
            98567237,  // Fiendsmith's Tract
            49867899,  // Fiendsmith's Sequence
            29301450,  // S:P Little Knight
            29301451,  // S:P Little Knight (alt)
            // Memento Engine
            54550967,  // Mementotlan Angwitch (Search starter)
            18165869,  // Mementotlan Dark Blade (Summon starter)
            81677154,  // Mementotlan Tatsunootoshigo (Special Summon & multi-dump)
            80722024,  // Mementotlan Bone Party (Deck summon / search)
            66518509,  // Mementotlan Fusion (Grave fusion & S/T searcher)
            23288411,  // Mementoal Tecuhtlica - Combined Creation (5000 ATK Boss)
            29111045,  // Goblin Biker Grand Breakout (Goblin summoner)
            98476659,  // Pot of Sloth (Hand replenisher)
            // Ryzeal Engine
            8633261,   // Ice Ryzeal (Starter)
            35844557,  // Sword Ryzeal (Starter / Searcher)
            34022970,  // Ext Ryzeal (Extender / Searcher)
            // Blitzclique Engine
            37654623,  // Hideout in the Sky, Coulomb (Field Spell Searcher)
            22912101,  // Surge Blitzclique (Pop & Summon)
            64049762,  // Blitzclique - Breakaway (Trap placement / pop)
            433377,    // Blitzclique - Steppleader (Self-destruct summoner)
            // Fiendsmith Engine
            60764609,  // Fiendsmith Engraver (Search / GY revive)
            98567237,  // Fiendsmith's Tract (Search / Fusion)
            28803166,  // Lacrima the Crimson Tears (Foolish Fiendsmith)
            // Sacred Beasts Engine
            59138498,  // Martyr of the Sacred Beasts (Place Field/Trap + 2 SS)
            38776201,  // Sacred Beasts Released (Add 3 SB monsters)
            1259915,   // Sacred Beasts Thunderclap (Place 2 + Fallen Paradise)
            65861210,  // Fallen Paradise of the Sacred Beasts (SS SB + Draw 2)
            50147815,  // Sacred Beasts Combined Assault (SS + Negate & Pop)
            // Orcust Engine Chokepoints
            69811710,  // Girsu, the Orcust Mekk-Knight (Foolish / Token starter)
            4334811,   // Scrap Recycler (Foolish Machine starter)
            28985331,  // Armageddon Knight (Foolish DARK starter)
            14536035,  // Dark Grepher (Discard / Foolish DARK starter)
            57835716,  // Orcust Harp Horror (GY Special Summon Orcust from Deck)
            21441617,  // Orcust Cymbal Skeleton (GY Special Summon Orcust from GY)
            4055337,   // Orcust Knightmare (GY Foolish DARK Machine)
            93920420,  // World Legacy - "World Wand" (GY Special Summon banished Orcust)
            94046012,  // Orcust Brass Bombard (GY Special Summon Orcust from hand)
            30741503,  // Galatea, the Orcust Automaton (Shuffle banished -> Set Orcust S/T)
            48835607,  // Galatea-i, the Orcust Automaton (Link-1 Babel searcher / GY Revive)
            90351981,  // Orcustrated Babel (Field Spell Quick Effect enabler)
            26845680,  // Orcustrated Return (Draw 2)
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
            94145022,  // Droll & Lock Bird (alt)
            97268402,  // Effect Veiler
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
            24224831,  // Called by the Grave (alt)
            65681983,  // Crossout Designator (canonical)
            65681982,  // Crossout Designator (alt)
            24299458,  // Forbidden Droplet
            41420027,  // Solemn Judgment
            23002292,  // Red Reboot
            40366667,  // Dominus Impulse
            6325660,   // Dominus Spark
            89264428,  // Dominus Purge
            62015408,  // Ghost Reaper & Winter Cherries
            38814750,  // PSY-Framegear Gamma
            91800273,  // Dimension Shifter
        };

        // ═══════════════════════════════════════════════════════════════
        //  6. TARGET IMMUNITY & DANGEROUS BATTLE CARDS
        // ═══════════════════════════════════════════════════════════════
        private static readonly HashSet<int> TargetImmuneCards = new HashSet<int>
        {
            55410871,  // Blue-Eyes Chaos MAX Dragon
            41721210,  // Dark Magician the Dragon Knight (protects backrow)
            37818794,  // Red-Eyes Dark Dragoon (untargetable & indestructible)
            37675907,  // Legacy ID mapping
            70405001,  // Black Luster Soldier - Soldier of Light and Darkness (battle immune & activated unaffected)
            44001993,  // Magician of Dark Chaos - Black Chaos (protects S/T & activated unaffected)
            98684220,  // Black Chaos (activated unaffected with ritual spell in GY)
            21887175,  // Mekk-Knight Crusadia Avramax (untargetable by effects)
            88264978,  // Red-Eyes Flare Metal Dragon (destruction immune with mats)
        };

        // ═══════════════════════════════════════════════════════════════
        //  7. DANGEROUS BATTLE / DAMAGE REFLECTION MONSTERS
        //  Attacking these results in self-damage, destroyed attacker, or wasted attacks.
        // ═══════════════════════════════════════════════════════════════
        private static readonly HashSet<int> DangerousBattleMonsters = new HashSet<int>
        {
            // Mikanko Monsters (Reflects all battle damage to opponent)
            6327734,   // Hu-Li the Jewel Mikanko
            6327735,   // Hu-Li the Jewel Mikanko (alt)
            11161666,  // Sanaki the Mikanko Devotee
            18377261,  // Ha-Re the Sword Mikanko
            54862960,  // Ni-Ni the Mirror Mikanko
            57566760,  // Uzuhime the Manifested Mikanko
            75771170,  // Arahime the Manifested Mikanko
            81260679,  // Ohime the Manifested Mikanko
            81260680,  // Ohime the Manifested Mikanko (alt)

            // Yubel Engine (Reflects battle damage / destroys attacker)
            78371393,  // Yubel
            4779091,   // Yubel - Terror Incarnate
            31764700,  // Yubel - The Ultimate Nightmare
            47172959,  // Yubel - The Loving Defender Forever
            80453041,  // Phantom of Yubel
            90829280,  // Spirit of Yubel

            // Timelords (Cannot be destroyed by battle / takes 0 battle damage)
            28929131,  // Zaphion the Timelord
            65314286,  // Sadion the Timelord
            74530899,  // Metaion the Timelord
            91712985,  // Kamion the Timelord
            92435533,  // Lazion the Timelord
            7733560,   // Michion the Timelord
            34137269,  // Hailon the Timelord
            60222213,  // Raphion the Timelord
            6616912,   // Gabrion the Timelord
            33015627,  // Sandaion the Timelord

            // Other Damage Reflection / Dangerous Attack Targets
            54366836,  // Number 54: Lion Heart
            29552709,  // Daigusto Sphreeze
            20366274,  // El Shaddoll Construct (destroys special summoned monster)
            63845230,  // Eater of Millions (banishes battling monster face-down)
            46239604,  // Dupe Frog
        };

        // ═══════════════════════════════════════════════════════════════
        //  8. DRAW / STANDBY PHASE FLOODGATES
        //  Traps that should be flipped in Draw/Standby Phase to pre-empt opponent plays.
        // ═══════════════════════════════════════════════════════════════
        private static readonly HashSet<int> DrawStandbyFloodgates = new HashSet<int>
        {
            82732047,  // Skill Drain
            82732705,  // Skill Drain (alt)
            83326048,  // Dimensional Barrier
            58921041,  // Anti-Spell Fragrance
            2429943,   // There Can Be Only One
            34487429,  // Gozen Match
            53334641,  // Gozen Match (alt)
            22046459,  // Rivalry of Warlords
            90845713,  // Rivalry of Warlords (alt)
            47355498,  // Summon Limit
            92746535,  // Summon Limit (alt)
            34507039,  // Deck Lockdown
            68462976,  // Secret Village of the Spellcasters
            30241314,  // Macro Cosmos
            81674782,  // Dimensional Fissure
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

        public static bool IsDrawStandbyFloodgate(int cardId)
        {
            return DrawStandbyFloodgates.Contains(cardId);
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
            return TargetImmuneCards.Contains(cardId) || GeneratedTargetImmuneCards.Contains(cardId);
        }

        public static bool IsTargetImmune(ClientCard card)
        {
            if (card == null) return false;
            if (card.IsDisabled()) return false;
            if (IsTargetImmune(card.Id)) return true;
            if (card.IsShouldNotBeTarget()) return true;
            if (CardTextSemantics.GetProfile(card)?.IsTargetImmune == true) return true;
            return false;
        }

        public static bool IsDestructionImmune(ClientCard card)
        {
            if (card == null) return false;
            if (card.IsDisabled()) return false;
            return CardTextSemantics.GetProfile(card)?.IsDestructionImmune ?? false;
        }

        public static bool IsEngineGenerator(ClientCard card)
        {
            if (card == null) return false;
            var profile = CardTextSemantics.GetProfile(card);
            if (profile == null) return false;
            return profile.IsCounterGenerator || profile.IsContinuousSearcher || profile.IsContinuousSummoner;
        }

        /// <summary>
        /// Universal threat score computation combining O(1) intelligence databases with dynamic text semantics.
        /// </summary>
        public static int GetCardThreatScore(ClientCard card, long hint = 0)
        {
            if (card == null) return 0;
            int score = 0;

            int id = card.Id;
            int nonAltId = card.GetNonAltartCode();

            // 1. O(1) Central Intelligence Checks (Staples / Known Bosses)
            if (KnownNegators.Contains(id) || KnownNegators.Contains(nonAltId)) score += 10000;
            if (FloodgateMonsters.Contains(id) || FloodgateMonsters.Contains(nonAltId)) score += 9500;
            if (FloodgateSpellsTraps.Contains(id)) score += 9500;
            if (HighThreatChokepoints.Contains(id) || HighThreatChokepoints.Contains(nonAltId)) score += 8000;

            // 2. Dynamic Card Text Semantics (Counter engines, searchers, decoys, immunities)
            score += CardTextSemantics.EvaluateCardThreat(card, hint);

            // 3. Spell / Trap / Monster baseline
            if (card.IsSpell() || card.IsTrap())
            {
                if (card.IsFaceup())
                {
                    if (id == 48680970) score += 12000; // Eternal Soul
                    else if (id == 82732047) score += 11000; // Skill Drain
                    else if (id == 38009249) score += 9500;  // Runick Fountain
                    else if (id == 38033121) score += 9000;  // Dark Magical Circle
                    else if (card.HasType(CardType.Continuous) || card.HasType(CardType.Field)) score += 2000;
                    else score += 1000;
                }
                else
                {
                    score += 4500; // Unknown set backrow
                }
            }
            else if (card.IsMonster())
            {
                score += card.Attack;
            }

            return score;
        }

        public static bool IsInvincibleBattle(int cardId)
        {
            return GeneratedBattleImmuneCards.Contains(cardId);
        }

        public static bool IsFusionSpell(int cardId)
        {
            return GeneratedFusionSpells.Contains(cardId);
        }

        /// <summary>
        /// Check if attacking this defender is dangerous (damage reflection, battle immunity, instant destruction).
        /// </summary>
        public static bool IsDangerousBattleTarget(ClientCard defender, ClientCard attacker)
        {
            if (defender == null || defender.IsDisabled()) return false;

            int id = defender.Id;

            // 1. Direct match in dangerous battle monsters (Mikanko, Yubel, Timelords, etc.)
            if (DangerousBattleMonsters.Contains(id) || GeneratedDangerousBattleMonsters.Contains(id))
                return true;

            // 2. Mekk-Knight Crusadia Avramax: Gains ATK equal to Special Summoned monster's ATK during damage calc
            if (id == 21887175 && defender.IsAttack() && attacker != null && attacker.IsSpecialSummoned)
                return true;

            // 3. Crystal Wing Synchro Dragon: Gains ATK equal to Lv5+ monster's ATK during damage calc
            if (id == 50954680 && defender.IsAttack() && attacker != null && attacker.Level >= 5)
                return true;

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
