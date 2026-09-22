using System.Collections.Generic;

namespace WindBot.Game.AI
{
    /// <summary>
    /// Auto-generated Card Intelligence Database extracted from official Lua scripts & cards.cdb.
    /// Run tools/scan_card_intelligence.py to update whenever new cards are released.
    /// </summary>
    public static partial class CardIntelligence
    {
        // ═══════════════════════════════════════════════════════════════
        //  AUTO-GENERATED: TARGET-IMMUNE MONSTERS
        // ═══════════════════════════════════════════════════════════════
        private static readonly HashSet<int> GeneratedTargetImmuneCards = new HashSet<int>
        {
            645794,  // Majespecter Toad - Ogama
            1409474,  // Hazy Flame Sphynx
            1686814,  // Ultimaya Tzolkin
            2521011,  // Brotherhood of the Fire Fist - Swallow
            3149401,  // Ultimate Dragon of Pride and Soul
            3611830,  // 'Endymion, the Mighty Master of Magic'
            4167084,  // The First Darklord
            5206415,  // Thunder Dragonlord
            5506791,  // Majespecter Cat - Nekomata
            6165656,  // Number C88: Gimmick Puppet Disaster Leo
            6247535,  // Borreload eXcharge Dragon
            6327734,  // Hu-Li the Jewel Mikanko
            6511113,  // Traptrix Rafflesia
            7574904,  // Myutant Arsenal
            8062132,  // Vennominaga the Deity of Poisonous Snakes
            8102334,  // Gate Blocker
            8540986,  // Veidos the Dragon of Endless Darkness
            8561192,  // Leo, the Keeper of the Sacred Tree
            8696773,  // Hazy Flame Hydra
            9264485,  // Horus' Servant
            9275482,  // UFOLight
            10000000,  // Obelisk the Tormentor
            10000080,  // The Winged Dragon of Ra - Sphere Mode
            10136446,  // Darklord Eveningstar
            11321089,  // Guardian Chimera
            12977245,  // Altergeist Fifinellag
            13331639,  // Supreme King Z-ARC
            13756293,  // King Dragun
            14512825,  // Pumpkin Carriage
            14541657,  // Twilight Ninja Shingetsu
            14970113,  // Zoodiac Hammerkong
            15419596,  // 'Ghost Bird of Bewitchment'
            16259549,  // Number 49: Fortune Tune
            16643334,  // Starliege Photon Blast Dragon
            17943271,  // Mementotlan Goblin
            18386170,  // Dante, Pilgrim of the Burning Abyss
            19438484,  // Ilios the Black Sun Dragon
            20011655,  // Beast of Talwar - The Sword Summit
            20654247,  // Blue-Eyes Chaos Dragon
            20849090,  // Kozmo Forerunner
            20938824,  // Maliss <P> March Hare
            21887175,  // Mekk-Knight Crusadia Avramax
            22723778,  // Clorless, Chaos King of Dark World
            23776077,  // Hazy Flame Basiltrice
            23920796,  // 'Mimighoul Cerberus'
            23965033,  // 'Amazoness Augusta'
            24550676,  // Lunalight Leo Dancer
            24573625,  // Deskbot 008
            24731391,  // Cyberse Magician
            25451652,  // Darklord Morningstar
            26873574,  // 'Chaos Daedalus'
            27381364,  // Spright Elf
            27784944,  // The Weather Painter Aurora
            28346136,  // Galaxy-Eyes Cipher X Dragon
            28400508,  // Number 97: Draglubion
            28454232,  // Enneacraft - Asta.PIXEA
            28981598,  // Nine-Lives Cat
            30698243,  // Red Hypernova Dragon
            31178212,  // Majespecter Unicorn - Kirin
            31303283,  // Hazy Flame Hyppogrif
            31833038,  // Borreload Dragon
            31991800,  // Majespecter Raccoon - Bunbuku
            32289031,  // Tellarknight Constellar Delteros
            33145233,  // 'Djinn Demolisher of Rituals'
            33206889,  // Performage Trapeze Witch
            33545259,  // Ancient Warriors - Ambitious Cao De
            34093683,  // Revendread Executor
            34541940,  // Artmage Finmel
            34550857,  // Lyrilusc - Cobalt Sparrow
            34695290,  // Myutant Beast
            34848821,  // Brigrand the Glory Dragon
            35103106,  // Evolzar Lars
            37354507,  // 'Ninja Grandmaster Saizo'
            37433748,  // 'SPYRAL GEAR - Last Resort'
            37552929,  // Maiden of the Millennium Moon
            37617348,  // Rescue-ACE Hydrant
            37683547,  // Albaz the Ashen
            37803172,  // Hazy Flame Peryton
            37818794,  // Red-Eyes Dark Dragoon
            38026562,  // Vola-Chemicritter Methydraco
            38030232,  // Tenyi Spirit - Sahasrara
            38229962,  // Giant Beetrooper Invincible Atlas
            38502358,  // 'Mekk-Knight Spectrum Supreme'
            38525760,  // 'Hazy Flame Cerbereus'
            38811586,  // Albion the Sanctifire Dragon
            39016067,  // Wind Unicorn Parallel, the Dracoslayer
            39890958,  // Heavy Mech Support Armor
            40908371,  // Azure-Eyes Silver Dragon
            41069676,  // Kewl Tune Loudness War
            41456841,  // Metamorphosed Insect Queen
            41522092,  // Number F0: Utopic Future Zexal
            41721210,  // Dark Magician the Dragon Knight
            42052439,  // Fire Flint Lady
            42166000,  // Egyptian God Slime
            42291297,  // Flower Cardian Lightshower
            42717221,  // Cyberse Clock Dragon
            43202238,  // Yazi, Evil of the Yang Zing
            43228023,  // Blue-Eyes Alternative Ultimate Dragon
            43803845,  // Duck Dummy
            45420955,  // Groza, Tyrant of Thunder
            46195773,  // Turbo Warrior
            46804536,  // Battlin' Boxer King Dempsey
            46947713,  // Transcode Talker
            49202162,  // Black Luster Soldier - Soldier of Chaos
            49394035,  // Vendread Core
            49451215,  // Ukanomitsune-no-Onari
            49867899,  // Fiendsmith's Sequence
            50383626,  // 'Darkest Diabolos, Lord of the Lair'
            50750868,  // T.G. Trident Launcher
            51073802,  // Majespecter Porcupine - Yamarashi
            52159691,  // Raider's Wing
            52707042,  // Libromancer Mystigirl
            52738610,  // Dance Princess of the Nekroz
            53550467,  // Noble Knight Drystan
            53618293,  // Sunvine Maiden
            54358015,  // Galaxy Stealth Dragon
            54757758,  // Mudragon of the Swamp
            55410871,  // Blue-Eyes Chaos MAX Dragon
            55787576,  // World Legacy - "World Shield"
            55885348,  // Kozmo Dark Destroyer
            58036229,  // Protectcode Talker
            58346901,  // Infernoble Knight Oliver
            58601383,  // Gaia Drake, the Universal Force
            58720904,  // Pendransaction
            58931850,  // Dragon Master Lords
            60025883,  // Duel Link Dragon, the Duel Dragon
            61089209,  // Myutant Mist
            61380658,  // Watthopper
            61641818,  // Virtual World Dragon - Longlong
            61764082,  // Dinowrestler Rambrachio
            61888819,  // Steelswarm Origin
            62133026,  // Vernusylph of the Flowering Fields
            63533837,  // Cyberse Quantum Dragon
            64063868,  // Kozmo Dark Eclipser
            64635042,  // Archfiend's Call
            65025250,  // Yosenju Shinchu L
            66069967,  // Prediction Princess Bibliomuse
            68295149,  // Linkmail Archfiend
            68395509,  // Majespecter Crow - Yata
            69385019,  // Mermail King - Neptabyss
            69718652,  // Ritual Beast Ulti-Nochiudrago
            69815951,  // Drytron Meteonis Draconids
            70333910,  // Noctovision Dragon
            71209500,  // Amazoness Scouts
            71797713,  // 'Elementsaber Lapauila Mana'
            72330894,  // Simorgh, Bird of Sovereignty
            72402069,  // D/D/D Super Doom King Bright Armageddon
            72664875,  // Crimson Nova Trinity the Dark Cubic Lord
            73082255,  // The Zombie Vampire
            73121813,  // Tenyi Spirit - Mula Adhara
            73490417,  // Jungle Dweller
            74010769,  // Hazy Flame Griffin
            74665150,  // Decode Talker Integration
            74725513,  // Qebehsenuef, Protection of Horus
            75059201,  // F.A. Turbo Charger
            75574498,  // Princess Cologne
            76416959,  // Ancient Warriors - Loyal Guan Yun
            76937326,  // Daigusto Laplampilica
            77610772,  // 'Ib the World Chalice Priestess'
            78225596,  // Appliancer Celtopus
            79086452,  // Gimmick Puppet Bisque Doll
            79656239,  // Aromaseraphy Sweet Marjoram
            80208158,  // D.D. Esper Star Sparrow
            81471108,  // ZW - Tornado Bringer
            81497285,  // Lady Labrynth of the Silver Castle
            82103466,  // Divine Serpent Geh
            82315403,  // Cyber Eternity Dragon
            82627406,  // Kiwi Magician Girl
            84523092,  // Witchcrafter Haine
            85028288,  // Jurrac Titano
            85065943,  // Saint Azamina
            85080444,  // Artifact Aegis
            85908279,  // Invoked Cocytus
            85909450,  // Harpie's Pet Phantasmal Dragon
            85991529,  // Kozmo Dark Planet
            87054946,  // Nephthys, the Sacred Flame
            87475570,  // Crystal Master
            88753594,  // Lunalight Sabre Dancer
            88754763,  // CXyz Coach Lord Ultimatrainer
            90207654,  // Gullveig of the Nordic Ascendant
            90465153,  // Mannadium Prime-Heart
            90590303,  // Number 41: Bagooska the Terribly Tired Tapir
            91025875,  // K9-ØØ Lupis
            91215724,  // Junk Armor
            91397409,  // Penguin Brave
            91718579,  // Gogogo Aristera & Dexia
            92332424,  // Majesty Pegasus, the Dracoslayer
            93413793,  // 'Ukanomitsune-no-Tamayura'
            93738004,  // Sagitta, Maverick Fur Hire
            94130731,  // Transcendosaurus Glaciasaurus
            94151981,  // Xyz Armor Torpedo
            94292987,  // Fabled Gamygyn
            94641726,  // Storm-Bane Dragon Destorbim
            94784213,  // Majespecter Fox - Kyubi
            95192919,  // Simorgh, Lord of the Storm
            95209656,  // Drytron Meteonis Quadrantids
            95974848,  // S-Force Orrafist
            96051150,  // Hazy Flame Mantikor
            96150936,  // Madolche Fresh Sistart
            98630720,  // Borrelend Dragon
            99000107,  // Gendo the Ascetic Monk
            99217226,  // Paladins of Bonds and Unity
        };

        // ═══════════════════════════════════════════════════════════════
        //  AUTO-GENERATED: BATTLE-IMMUNE MONSTERS
        // ═══════════════════════════════════════════════════════════════
        private static readonly HashSet<int> GeneratedBattleImmuneCards = new HashSet<int>
        {
            1035143,  // Darkuriboh
            1528054,  // Silhouhatte Rabbit
            1872843,  // Morpheus, the Dream Mirror White Knight
            2129638,  // Blue-Eyes Twin Burst Dragon
            2204038,  // Valkyrie Brunhilde
            2250266,  // Morphtronic Staplen
            2414168,  // Interrupt Resistor
            2857636,  // Knightmare Phoenix
            2980764,  // Consecrated Light
            3134857,  // Orcustrion
            4019153,  // Number 4: Numeron Gate Catvari
            4026187,  // Chronicler of Fairy Tail Tales
            4055337,  // Orcust Knightmare
            4417407,  // Mecha Phantom Beast Blackfalcon
            4591250,  // Amazoness Empress
            4779091,  // Yubel - Terror Incarnate
            4786063,  // Superheavy Samurai Soulfire Suit
            5524387,  // Marincess Marbled Rock
            5846183,  // Appliancer Breakerbuncle
            6142213,  // Turbo Rocket
            6327734,  // Hu-Li the Jewel Mikanko
            6330307,  // DZW - Chimera Clad
            6511113,  // Traptrix Rafflesia
            6616912,  // Gabrion, the Timelord
            6622715,  // Encode Talker
            6659193,  // Performage Trapeze High Magician
            7020743,  // Tantrum Toddler
            7279373,  // Abyss Actor - Twinkle Little Star
            7540107,  // Gouki Guts
            7622360,  // Matador Archfiend
            7733560,  // Michion, the Timelord
            7864030,  // Superheavy Samurai Blowtorch
            8198620,  // Dragonecro Nethersoul Dragon
            8384771,  // Performapal Gumgumouton
            8445808,  // Maiden in Love
            8594079,  // Jurrac Brachis
            8700633,  // Undaunted Bumpkin Beast
            8763963,  // Beelzeus of the Diabolic Dragons
            8841431,  // 'Centur-Ion Primera Primus'
            9097866,  // Cross Debug
            9275482,  // UFOLight
            9634146,  // Signal Warrior
            10248389,  // Cyber Blader
            10669138,  // Five-Headed Link Dragon
            10796448,  // Ace★Spades Speculation
            10817524,  // First of the Dragons
            11375683,  // Tindangle Trinity
            11449436,  // Glacier Aqua Madoor
            11662742,  // Gellenduo
            11677278,  // Mimighoul Armor
            11722335,  // Worm Xex
            11755663,  // Dinowrestler Martial Anga
            11845050,  // Right-Hand Shark
            12067160,  // Gorgon of Zilofthonia
            12423762,  // Gagaga Gardna
            12600382,  // Exodia Necross
            12890860,  // Denial Deity Dotan
            13173832,  // Salamangreat Wolvie
            13256226,  // Elemental HERO Spirit of Neos
            13474291,  // Cloudian - Storm Dragon
            13708888,  // Evil HERO Neos Lord
            13760677,  // P.M. Captor
            14148099,  // B.E.S. Big Core
            15232745,  // Number 1: Numeron Gate Ekam
            15317640,  // B.E.S. Covered Core
            15335853,  // 'Mecha Phantom Beast Sabre Hawk'
            15495787,  // Superheavy Samurai Prepped Defense
            15610297,  // Vijam the Cubic Seed
            15744417,  // Orgoth the Relentless
            15914410,  // Mechquipped Angineer
            15982593,  // Centur-Ion Legatia
            16197610,  // Cloudian - Turbulence
            16516630,  // Blackwing - Boreas the Sharp
            16617334,  // Performapal Rain Goat
            16922142,  // Radiant Typhoon Krosea
            16943770,  // Mecha Phantom Beast Aerosguin
            17810268,  // Cloudian - Acid Cloud
            18377261,  // Ha-Re the Sword Mikanko
            19434243,  // Power Vice Dragon
            20003527,  // Cloudian - Nimbusman
            20246864,  // Windwitch - Freeze Bell
            20368763,  // Mecha Phantom Beast Harrliard
            20700531,  // Pilgrim of the Ice Barrier
            21435914,  // Dragocytos Corrupted Nethersoul Dragon
            21452275,  // Aroma Jar
            22093873,  // Masked HERO Divine Wind
            22110647,  // Mecha Phantom Beast Dracossack
            22510667,  // Gouki The Solid Ogre
            22638495,  // Dinoster Power, the Mighty Dracoslayer
            22790789,  // B.E.S. Crystal Core
            22850702,  // Chaos Angel
            22916418,  // Mantman the Ultrahuman
            23093604,  // X-Saber Pashuul
            23187256,  // Number 93: Utopia Kaiser
            23205979,  // Spirit Reaper
            23421244,  // Reborn Zombie
            23656668,  // Gravity Controller
            23770284,  // Strong Wind Dragon
            23935886,  // 'Draco Masters of the Tenyi'
            23971061,  // 'Doublebyte Dragon'
            24232799,  // Storagepod
            25904894,  // 'Karakuri Super Shogun mdl 00N "Bureibu"'
            26746975,  // 'Dark Guardian'
            26949946,  // 'Mecha Phantom Beast Jaculuslan'
            26973555,  // 'Number F0: Utopic Draco Future'
            27143874,  // Dino-Sewing
            27240101,  // Kikinagashi Fucho
            27352108,  // Chobham Armor Dragon
            27416701,  // Shiba-Warrior Taro
            27618634,  // The Unhappy Girl
            28798938,  // Dual Avatar - Manifested A-Un
            28929131,  // Zaphion, the Timelord
            29357956,  // Gladiator Beast Nerokius
            29552709,  // Daigusto Sphreez
            29913783,  // Super Anti-Kaiju War Machine Mecha-Thunder-King
            29996433,  // Dinowrestler Capoeiraptor
            30138615,  // Nightmare-Eyes Restrict
            30276969,  // Reese the Ice Mistress
            30607616,  // Oboro-Guruma, the Wheeled Mayakashi
            30741503,  // Galatea, the Orcust Automaton
            30811116,  // Mecha Phantom Beast Stealthray
            30860696,  // Rocket Warrior
            31053337,  // Blackwing - Abrolhos the Megaquake
            31305911,  // Marshmallon
            31480215,  // Mecha Phantom Beast Warbluran
            31533704,  // Mecha Phantom Beast Megaraptor
            31539614,  // Antidote Nurse
            31764700,  // Yubel - The Ultimate Nightmare
            31887806,  // Tobari the Sky Ninja
            31919988,  // Dark Diviner
            32138660,  // Reptilianne Melusine
            32216688,  // R.B. The Brute Blues
            32448765,  // Trickstar Holly Angel
            32453837,  // Number 2: Ninja Shadow Mosquito
            32759190,  // Hecahands Yadel
            32975247,  // Divine Dragon Titanomakhia
            33015627,  // Sandaion, the Timelord
            33296432,  // Dogmatika Adin, the Enlightened
            33883834,  // Shien's Squire
            34137269,  // Hailon, the Timelord
            34198387,  // Meowseclick
            34408491,  // 'Beelze of the Diabolic Dragons'
            34475451,  // Construction Train Signal Red
            34541543,  // Master Tao the Chanter
            34620088,  // Gimmick Puppet Shadow Feeler
            34680482,  // Madolche Anjelly
            34848821,  // Brigrand the Glory Dragon
            34876719,  // N.As.H. Knight
            34961968,  // Phantom Beast Thunder-Pegasus
            35252119,  // Virtual World Beast - Jiujiu
            35494087,  // Speedroid Skull Marbles
            35770983,  // Dinowrestler Martial Ankylo
            36322312,  // Boot-Up Admiral - Destroyer Dynamo
            36472900,  // Sonic Chick
            36776089,  // 'Sky Cavalry Centaurea'
            36931229,  // 'Castle Gate'
            36974120,  // 'Vouiburial, the Dragon Undertaker'
            37115973,  // Numeral Hunter
            37310367,  // Lockout Gardna
            37405032,  // 'Trickstar Aqua Angel'
            37433748,  // 'SPYRAL GEAR - Last Resort'
            37552929,  // Maiden of the Millennium Moon
            38264974,  // 'Chimera the Illusion Beast'
            38505587,  // 'Moremarshmallon'
            39520293,  // 'Junk Mail'
            39643167,  // Bunny Ear Enthusiast
            39972129,  // Number 64: Ronin Raccoon Sandayu
            39998992,  // 'X-Krawler Synaphysis'
            40028305,  // 'Superheavy Samurai Soulclaw'
            40221691,  // Nightmare Magician
            40227329,  // Go! - D/D/D Divine Zero King Rage
            40945356,  // Twilight Ninja Nichirin, the Chunin
            40991587,  // The Lady in Wight
            41329458,  // Mecha Phantom Beast Kalgriffin
            41436536,  // Elemental HERO Phoenix Enforcer
            41522092,  // Number F0: Utopic Future Zexal
            41628550,  // Superheavy Samurai Blue Brawler
            42166000,  // Egyptian God Slime
            42230449,  // Number 2: Numeron Gate Dve
            42741437,  // Exosister Mikailis
            43268675,  // Opera the Melodious Diva
            43318266,  // Cloudian - Cirrostratus
            43378048,  // 'Armityle the Chaos Phantasm'
            43490025,  // Number F0: Utopic Future Slash
            43730887,  // Holding Arms
            44026393,  // Mecha Phantom Beast Raiten
            44509529,  // Prank-Kids Weather Washer
            44694191,  // Transient Masquerader of Illusion
            44954628,  // B.E.S. Tetran
            44968687,  // The Legendary Fisherman III
            45420955,  // Groza, Tyrant of Thunder
            45445571,  // The Duke of Demise
            45488703,  // NT8000 - SIRIUS
            46132282,  // Powered Inzektron
            46136942,  // Performapal Odd-Eyes Dissolver
            46939151,  // Earthbound Prisoner Ground Keeper
            47017574,  // Number C92: Heart-eartH Chaos Dragon
            47172959,  // Yubel - The Loving Defender Forever
            47946130,  // Gouki The Giant Ogre
            48608796,  // Lyrilusc - Assembled Nightingale
            48654323,  // White Relic of Dogmatika
            48928529,  // Number 83: Galaxy Queen
            49776811,  // Shining Piecephilia
            50400231,  // 'Satellite Cannon'
            50789693,  // Armored Kappa
            50907446,  // El Shaddoll Apkallone
            50939127,  // Different Dimension Dragon
            51497409,  // D/D/D Stone King Darius
            51566770,  // Infernity Guardian
            51777272,  // Lunalight Cat Dancer
            52077741,  // Obnoxious Celtic Guard
            52085072,  // Dystopia the Despondent
            52254878,  // Dimensional Allotrope Varis
            52698008,  // Cyberse Wicckid
            52768390,  // Dark Creator
            52824910,  // Kaiser Glider
            53413628,  // Code Talker
            53451824,  // Mecha Phantom Beast Concoruda
            53466722,  // First of the Dragonlords
            53490455,  // Salamangreat Raccoon
            54366836,  // Number 54: Lion Heart
            54446813,  // Dinowrestler Martial Ampelo
            54862960,  // Ni-Ni the Mirror Mikanko
            54919528,  // K9-ØØ "Hound"
            55537983,  // Mimighoul Master
            56043446,  // Viser Des
            56051086,  // Number 43: Manipulator of Souls
            56198785,  // Guard Ghost
            56292140,  // Number 51: Finisher the Strong Arm
            57566760,  // Uzuhime the Manifested Mikanko
            57610714,  // Cloudian - Eye of the Typhoon
            58036229,  // Protectcode Talker
            58143852,  // Nightmare Apprentice
            58153103,  // Armed Dragon Thunder LV10
            59042331,  // Hedge Guard
            59281922,  // Cyber Dragon Drei
            59369430,  // Golgoil the Steel Seismic Smasher
            59627393,  // Number 105: Battlin' Boxer Star Cestus
            59913418,  // Demise, Supreme King of Armageddon
            60033398,  // Ancient Warriors - Fearsome Zhang Yuan
            60222213,  // Raphion, the Timelord
            60303688,  // Dogmatika Ecclesia, the Virtuous
            60349525,  // Cracking Dragon
            60417395,  // Darkness Neosphere
            61888819,  // Steelswarm Origin
            62038047,  // Shiranui Smith
            62411042,  // Ibicella Lutea
            62892347,  // Arcana Force 0 - The Fool
            63504681,  // Number 86: Heroic Champion - Rhongomyniad
            63947968,  // Fiend Reflection of the Millennium
            64104037,  // Ghost Lancer, the Underworld Spearman
            64591429,  // Astral Kuriboh
            65305468,  // Number F0: Utopic Future
            65314286,  // Sadion, the Timelord
            65326118,  // Tenpai Dragon Fadra
            65815684,  // Vicious Astraloud
            66200210,  // Mecha Phantom Beast Hamstrat
            66262416,  // Destiny HERO - Dreamer
            66393507,  // X-Krawler Neurogos
            66401502,  // Vanquish Soul Pantera
            66500065,  // Bi'an, Earth of the Yang Zing
            67045745,  // F.A. Sonic Meister
            67385964,  // Gladiator Beast Noxious
            67489919,  // บลูอิมพาลา 'Mecha Phantom Beast'
            67508932,  // วอร์ปเกทผู้ให้กำเนิด 'Timelord'
            67712104,  // Marincess Crystal Heart
            67922702,  // Mecha Phantom Beast Tetherwolf
            68144894,  // Hecahands Godos
            68823957,  // Subterror Nemesis Defender
            69031175,  // Blackwing Armor Master
            69058960,  // Number 13: Embodiment of Crime
            69073023,  // Infinitrack Fortress Megaclops
            69228245,  // Performapal Changeraffe
            69526976,  // Cupid Dunk
            70083723,  // Naturia Dragonfly
            70271583,  // Karakuri Watchdog mdl 313 "Saizan"
            71645463,  // Dragunity Quirinus
            71768839,  // 'I.A.S. -Invasive Alien Species-'
            71797713,  // 'Elementsaber Lapauila Mana'
            72006609,  // Mekk-Knight of the Morning Star
            72171665,  // Magical Broker
            72246674,  // Gladiator Beast Dareios
            72291078,  // Mecha Phantom Beast O-Lion
            72427512,  // Mimicking Man-Eater Bug
            72566043,  // Litmus Doom Swordsman
            74009824,  // El Shaddoll Wendigo
            74122412,  // Nekroz of Gungnir
            74163487,  // Restoration Point Guard
            74530899,  // Metaion, the Timelord
            74889525,  // Eldlich the Mad Golden Lord
            75083197,  // Dragonlark Pairen
            75676192,  // F.A. Motorhome Transport
            75771170,  // Arahime the Manifested Mikanko
            75874514,  // 'Shining Star Dragon'
            75937826,  // B.E.S. Big Core MK-2
            76218643,  // Black Potan
            76504386,  // Number 39: Utopia the Envoy of Light
            76833149,  // Melffy Mommy
            76902476,  // Mecha Phantom Beast Turtletracer
            77205367,  // Number C96: Dark Storm
            77571454,  // Number 69: Heraldry Crest - Dark Matter Demolition
            77610772,  // 'Ib the World Chalice Priestess'
            77679716,  // Superheavy Samurai Soulbreaker Armor
            77700347,  // Necro Defender
            78135071,  // Exosister Kaspitell
            78371393,  // Yubel
            78625448,  // Number 3: Numeron Gate Trini
            79491903,  // Reptilianne Naga
            79703905,  // Cloudian - Altus
            80088625,  // Binary Blader
            80453041,  // Phantom of Yubel
            80889750,  // Frightfur Sabre-Tooth
            80896940,  // Nirvana High Paladin
            81035362,  // Fiendish Rhino Warrior
            81260679,  // Ohime the Manifested Mikanko
            81782101,  // Reptia Egg
            82821760,  // B.E.S. Big Core MK-3
            83121692,  // Elemental HERO Tempest
            83135907,  // Scrap Goblin
            83812099,  // Flint Lock
            84224627,  // Cat Shark
            84257883,  // B.E.S. Blaster Cannon Core
            84330567,  // Tearlaments Rulkallos
            84339249,  // Protecting Spirit Loagaeth
            84472026,  // Ghostrick Yeti
            84988419,  // Bloom Diva the Melodious Choir
            85289965,  // Borrelsword Dragon
            85401123,  // Mokomoko
            85684223,  // Reaper on the Nightmare
            85771019,  // Darklord Asmodeus
            86038337,  // Faisan, Hunting Scout of the Deep Forest
            86165817,  // Evil HERO Malicious Bane
            87054946,  // Nephthys, the Sacred Flame
            87390067,  // Blackwing - Jetstream the Blue Sky
            87462901,  // Emissary from the House of Wax
            87468732,  // Clock Arc
            87804747,  // Ultimate Great Insect
            88106656,  // Libromancer Fireburst
            88820235,  // Elemental HERO Shining Phoenix Enforcer
            88926295,  // Evigishki Neremanas
            89127526,  // Barrier Resonator
            89132148,  // Photon Orbital
            89194103,  // The Fabled Kudabbi
            89571015,  // Storm Cipher
            90664857,  // Virtual World Shell - Jaja
            90726340,  // Queen Dragun Djinn
            90829280,  // Spirit of Yubel
            90835938,  // Starry Night, Starry Dragon
            91712985,  // Kamion, the Timelord
            92015800,  // Number 76: Harmonizer Gradielle
            92435533,  // Lazion, the Timelord
            92644052,  // 'Performapal Duelist Extraordinaire'
            92932860,  // Performapal Miss Director
            93302695,  // Kozmoll Wickedwitch
            93353691,  // Driven Daredevil
            93581434,  // 'Gouki Ringtrainer'
            93657021,  // 'Destiny HERO - Dusktopia'
            93730230,  // Chronomaly Crystal Chrononaut
            93920420,  // World Legacy - "World Wand"
            94130731,  // Transcendosaurus Glaciasaurus
            94136469,  // Checksum Dragon
            94207108,  // Marincess Wonder Heart
            94410955,  // Hecahands Xeno
            94730900,  // Infernoble Knight Maugis
            94973028,  // Mecha Phantom Beast Coltwing
            95360850,  // Shield Warrior
            95365081,  // Hecahands Ibtel
            95442074,  // Number 31: Embodiment of Punishment
            95506252,  // Black Jack the Shadow-Armored Knight
            95825679,  // Archfiend's Awakening
            96708940,  // Speedroid Marble Machine
            96891787,  // 'Dogmatika Theo, the Iron Punch'
            97403510,  // 'Number 92: Heart-eartH Dragon'
            97522863,  // The Man with the Mark
            98024118,  // 'White Potan'
            98630720,  // Borrelend Dragon
            99267150,  // Five-Headed Dragon
        };

        // ═══════════════════════════════════════════════════════════════
        //  AUTO-GENERATED: DANGEROUS BATTLE MONSTERS (Reflect / Avoid)
        // ═══════════════════════════════════════════════════════════════
        private static readonly HashSet<int> GeneratedDangerousBattleMonsters = new HashSet<int>
        {
            3918345,  // Magical Reflect Slime
            4779091,  // Yubel - Terror Incarnate
            6327734,  // Hu-Li the Jewel Mikanko
            6616912,  // Gabrion, the Timelord
            7733560,  // Michion, the Timelord
            10474647,  // Depresspard
            17285476,  // Naturia Mosquito
            18377261,  // Ha-Re the Sword Mikanko
            28929131,  // Zaphion, the Timelord
            29552709,  // Daigusto Sphreez
            31764700,  // Yubel - The Ultimate Nightmare
            33015627,  // Sandaion, the Timelord
            34031284,  // Ojama Emperor
            34137269,  // Hailon, the Timelord
            35494087,  // Speedroid Skull Marbles
            45025640,  // Boycotton
            47172959,  // Yubel - The Loving Defender Forever
            54366836,  // Number 54: Lion Heart
            54862960,  // Ni-Ni the Mirror Mikanko
            57566760,  // Uzuhime the Manifested Mikanko
            59627393,  // Number 105: Battlin' Boxer Star Cestus
            60222213,  // Raphion, the Timelord
            65314286,  // Sadion, the Timelord
            67508932,  // วอร์ปเกทผู้ให้กำเนิด 'Timelord'
            69058960,  // Number 13: Embodiment of Crime
            74530899,  // Metaion, the Timelord
            74578720,  // Time Thief Chronocorder
            75771170,  // Arahime the Manifested Mikanko
            78371393,  // Yubel
            80453041,  // Phantom of Yubel
            81260679,  // Ohime the Manifested Mikanko
            85065943,  // Saint Azamina
            90829280,  // Spirit of Yubel
            91712985,  // Kamion, the Timelord
            92435533,  // Lazion, the Timelord
            93730230,  // Chronomaly Crystal Chrononaut
            94004268,  // Amazoness Swords Woman
            95442074,  // Number 31: Embodiment of Punishment
            97403510,  // 'Number 92: Heart-eartH Dragon'
            97637162,  // Handigallop
        };

        // ═══════════════════════════════════════════════════════════════
        //  AUTO-GENERATED: FUSION SPELLS
        // ═══════════════════════════════════════════════════════════════
        private static readonly HashSet<int> GeneratedFusionSpells = new HashSet<int>
        {
            458748,  // The Book of the Law
            1122030,  // Artmage Vandalism -Assault-
            1264319,  // Gem-Knight Fusion
            1784686,  // The Eye of Timaeus
            1845204,  // Instant Fusion
            3259760,  // Spellbound
            3496543,  // Sinful Spoils of the White Forest
            3659803,  // Overload Fusion
            6077601,  // Frightfur Fusion
            6153210,  // Ketu Dracotail
            6172122,  // Red-Eyes Fusion
            6417578,  // El Shaddoll Fusion
            7394770,  // Brilliant Fusion
            8148322,  // Predaprime Fusion
            8778267,  // Penetration Fusion
            9102835,  // Salamandra Fusion
            9113513,  // Ostinato
            10218411,  // Double Fusion
            10833828,  // Forbidden Dark Contract with the Swamp King
            11493868,  // Fortissimo
            11827244,  // Magicalized Fusion
            11911336,  // Spellbook of the Grand Circle
            12071500,  // Dark Calling
            13234975,  // Beetrooper Landing
            14088859,  // Neos Fusion
            14283055,  // Concours de Cuisine (Culinary Confrontation)
            15543940,  // Tyrant Dino Fusion
            16269385,  // Prank-Kids Place
            17236839,  // Flash Fusion
            17725109,  // Roar of the Blue-Eyed Dragons
            18795635,  // GMX Applied Experiment #55
            18973184,  // Branded Lost
            20934683,  // Azamina Debtors
            21862633,  // Piercing the Darkness
            22283204,  // The Gaze of Timaeus
            23299957,  // Vehicroid Connection Zone
            24220368,  // Gem-Knight Dispersion
            24845628,  // Magicalized Duston Mop
            25800447,  // 'Fusion of Fire'
            25861589,  // 'Aroma Blend'
            29062925,  // Face Card Fusion
            29143457,  // Fire Formation - Ingen
            31444249,  // Void Imagination
            31458630,  // Melodious Concerto
            32548318,  // Rahu Dracotail
            33099732,  // 'Over Future Fusion'
            33550694,  // Fusion Gate
            34813545,  // Naturia Blessing
            34933456,  // Magistus Invocation
            34950192,  // Lev Shaddoll Fusion
            34995106,  // Branded in White
            35098357,  // Witchcrafter Confusion Confession
            35167375,  // Surprise Fusion
            35255456,  // Miracle Contact
            35705817,  // Ghost Fusion
            36484016,  // Miracle Synchro Fusion
            36494597,  // Teleport Fusion
            37517035,  // Artmage Masterwork -Succession-
            37630732,  // Power Bond
            38129297,  // Double Interlock
            38590361,  // Spiral Fusion
            39261576,  // Particle Fusion
            39564736,  // Fullmetalfoes Fusion
            40003819,  // 'Guardragon Reincarnation'
            40110009,  // Dragonmaid Changeover
            40597694,  // Scatter Fusion
            41940225,  // Destruction Swordsman Fusion
            42577802,  // 'Myutant Fusion'
            43698897,  // Frightfur Factory
            44227727,  // Plunder Patroll Shipshape Ships Shipping
            44362883,  // Branded Fusion
            44394295,  // Shaddoll Fusion
            44771289,  // The Terminus of the Burning Abyss
            44886582,  // Apex Polymerization
            45906428,  // Miracle Fusion
            47679935,  // Magical Meltdown
            48130397,  // 'Super Polymerization'
            48144509,  // 'Odd-Eyes Fusion'
            52553471,  // Over Fusion
            52947044,  // Fusion Destiny
            54283059,  // Parallel World Fusion
            55421040,  // Hunting Horn
            55704856,  // Cyberload Fusion
            57425061,  // Vision Fusion
            57809669,  // Hecahands Tartaros
            58199906,  // Cybernetic Fusion Support
            58549532,  // Parametalfoes Fusion
            59332125,  // A.I. Love Fusion
            59419719,  // Fossil Fusion
            59432181,  // Fusion Tag
            59514116,  // Secrets of Dark Magic
            60226558,  // Nephe Shaddoll Fusion
            63136489,  // 'Chimera Fusion'
            63854005,  // Ready Fusion
            64061284,  // Ancient Gear Fusion
            65514302,  // Magnet Bonding
            65646587,  // Pendulum Fusion
            65801012,  // Cynet Fusion
            65956182,  // Dark World Accession
            66290900,  // Gladiator Beast United
            66518509,  // Mementotlan Fusion
            67523044,  // กราวด์ 'Xeno'
            67526112,  // Rapid Trigger
            71143015,  // Ultimate Fusion
            71422989,  // Absorb Fusion
            71490127,  // Dragon's Mirror
            71593652,  // Mutiny in the Sky
            71939275,  // Spirit Illusion
            72490637,  // Dowsing Fusion
            73360025,  // Dark Contract with the Swamp King
            73594093,  // Metalfoes Fusion
            73714736,  // Flame Swordsrealm
            74063034,  // Invocation
            74335036,  // Fusion Substitute
            76647978,  // Ultra Polymerization
            77124096,  // Dark Contact
            77565204,  // 'Future Fusion'
            78063197,  // Relinquished Fusion
            79059098,  // Prank-Kids Pandemonium
            80033124,  // Cyberdark Impact!
            81788994,  // Curse of the Shadow Prison
            82119326,  // Tales of Fairy Tail
            82738008,  // Branded in Red
            85808813,  // Time Stream
            86758746,  // Amazoness Secret Arts
            87669904,  // Dual Avatar Invitation
            87931906,  // Lunalight Fusion
            88693151,  // Trickstar Fusion
            92058902,  // Future Fusion Nova
            94820406,  // Dark Fusion
            94845588,  // The Hallowed Azamina
            95034141,  // Seven Cities of the Golden Land
            95238394,  // Thunder Dragon Fusion
            95286165,  // De-Fusion
            96239878,  // Strength in Unity
            96687733,  // Defense of the Temple
            98567237,  // Fiendsmith's Tract
            98570539,  // Dream Mirror of Chaos
            98828338,  // Extinguishing the Ashened
            98829635,  // Forbidden Crown
            99161253,  // Primite Fusion
            99426088,  // Magikey Maftea
            99543666,  // Despia, Theater of the Branded
            99599062,  // Earthbound Fusion
            99941223,  // Darklord Dance
        };
    }
}
