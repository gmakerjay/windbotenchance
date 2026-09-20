# Progress Log: 2026_Branded, 2026_DarkTime, 2026_Runick, 2026_RyuGe, 2026_AFS, 2026_Spright, GOD-01, Demise, 2026_Darklord, 2026_DarkWorld, 2026_Hecahand & Anime ModernExecutors

## 0.010. Anime_Pegasus (Pegasus) S:P Little Knight Audit & Self-Targeting Removal Fix (2026-09-20)
- **Problem Statement**:
  - The user observed "pegasus ดีดการ์ดตัวเองลงหลุมหรอ" (Why does Pegasus send his own cards to GY?) and requested "ดู log ล่าสุดเกี่ยวกับการกระทำของ SP knight" (Analyze recent logs regarding S:P Little Knight's actions).
- **Log Analysis & Root Cause Findings (`WindBot/logs/Blue_Angel_vs_Pegasus_25690920_123602` & `123537`)**:
  1. **Comic Hand Waste on S:P Link Summon (Turn 2)**:
     - Pegasus activated `Comic Hand` (33453260) to take control of Blue Angel's `Trickstar Lilybell` (98700941) and Normal Summoned `Toon Cyber Dragon` (83629030).
     - Because `SPLittleKnightSpSummon()` returned `true` unconditionally, the bot sacrificed both the stolen monster and `Toon Cyber Dragon` as Link materials to summon `S:P Little Knight` (29301450).
     - As a result, the stolen monster left the field, sending `Comic Hand` straight to the GY ("ดีดลงหลุม"), destroying Pegasus's own advantage and replacing high-ATK direct attackers under `Toon Kingdom` with a 1600 ATK link monster.
  2. **S:P Little Knight Quick Effect Dodge (Turn 3 & Turn 5)**:
     - On Turn 3 (12:36:16) and Turn 5 (12:36:17), Blue Angel activated monster effects. `SPLittleKnightActivate` responded with Quick Effect (Effect 2: target 2 face-up monsters including 1 bot controls to banish until End Phase).
     - The bot correctly targeted Blue Angel's monster (`98700941` / `37683441`) and itself (`29301450`), causing both to fly out of the field temporarily until the End Phase.
  3. **Toon Black Luster Soldier Self-Banish Bug (Turn 2 in 123537)**:
     - In the earlier duel, `Toon Black Luster Soldier` activated its once-per-turn banish effect (`hint=503`).
     - In `OnSelectCard()`, the bot unconditionally matched `preferred` cards containing `ToonBlackLusterSoldier`.
     - Because hint type wasn't checked, the bot selected **its own Toon BLS as the target to banish**, removing its own boss monsters twice!
- **Architectural Fixes in `Anime_PegasusExecutor.cs`**:
  - **`SPLittleKnightSpSummon` Hard Guards**: Added strict conditions:
    - NEVER Link Summon S:P if Pegasus controls any monster equipped with `Comic Hand`.
    - NEVER sacrifice high-ATK Toons under `Toon Kingdom` in Main Phase 1 (preserves direct attack win condition).
    - Only Link Summon S:P in Main Phase 2 as an end-board piece or in Main Phase 1 if opponent has high threats and bot has small non-Toon bodies.
  - **`OnSelectCard` Hint-Specific Segregation**:
    - Restricted `preferred` search list solely to Deck searches (`hint == 506` / `HINTMSG_ATOHAND` or all cards located in Deck).
    - For removal hints (`hint == 503` / `HINTMSG_REMOVE`, `hint == 502` / `HINTMSG_DESTROY`, `hint == 504` / `HINTMSG_TOGRAVE`), bot strictly prioritizes enemy cards (`c.Controller == 1`) and never targets friendly cards.
  - **`ComicHandActivate` Target Validation**: Requires face-up opponent monsters (`Controller == 1`) that are not tokens and not already equipped with `Comic Hand`.
  - **`SPLittleKnightActivate` Dual-Branch Handling**:
    - Quick Effect dodge: targets highest-ATK opponent monster + S:P itself only when opponent activates effects.
    - On-Summon banish: targets highest-ATK enemy monster, enemy backrow, or opponent GY monster.
  - **Extra Deck Safeguards**: Added guards to `BigEyeSpSummon`, `RelinquishedAnimaSpSummon`, and `HopeHarbingerSpSummon` to avoid throwing away Toon direct lethal push.
- **Build & Exclusive Deployment**:
  - Compiled with 0 errors via `BUILD_AND_DEPLOY.ps1`; deployed new `WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `bots.json`, and deck assets to `C:\Users\admin\Documents\EdoGame\`.

## 0.009. Anime_Yugi (Yugi Muto) AI Freeze Fix & Deck Optimization (2026-09-20)
- **Problem Statement**: The user reported "เด็ค yugi muto ไม่ยอมเล่นการ์ด" (Yugi Muto deck refuses to play cards / freezes / passes turns doing nothing).
- **Root Causes Identified from Duel Logs (`WindBot/logs/Yugi_Muto_vs_Shark_...`)**:
  1. **Impossible Tribute Condition Bug**: In `Anime_YugiExecutor.cs`, Normal/Tribute summons for `DarkMagicianGirl`, `SkullArchfiendOfChaos`, and `PharaohsServant` had condition `return Bot.GetMonsterCount() == 0;`. Because Level 6 & 7 monsters require 1-2 tributes, they could NEVER be summoned when the board had monsters, and the game engine couldn't offer them when the board was empty, locking the bot out of normal summons 100% of the time.
  2. **Searcher Self-Sabotage in `OnSelectCard`**: When `Illusion of Chaos` activated its hand search, `OnSelectCard` lacked priority handling and frequently selected the searched `Pharaoh's Servant` to put immediately back on top of the deck!
  3. **Starter Starvation & Severe Deck Bricks**: The original 46-card `Anime_Yugi.ydk` had only 1 copy of `Dark Magician, the Pharaoh's Servant` and 0 copies of original `Dark Magician`. As a result, 3x `Dark Magical Curtain` frequently had zero targets in Deck and failed to trigger its search; 3x `Preparation of Rites` bricked because there was only 1 Level <= 7 Ritual in the deck; and 3x `Pre-Preparation of Rites` bricked because there were only 2 Ritual monsters total.
  4. **Pot of Prosperity Sabotage**: `PotOfProsperityActivate` was banishing all 3 copies of `Dark Magician of Destruction`, severing the bot's primary Extra Deck route into `Red-Eyes Dark Dragoon`.
  5. **Handtrap Exposure**: `SpellSetStrategy` was setting `Dominus Impulse` face-down immediately on Turn 1, exposing it to removal instead of keeping it in hand as an active handtrap.
- **Architectural Solutions & Fixes**:
  - **`Anime_Yugi.ydk` Optimization**: Rebalanced main deck to 45 cards: added 2x original `Dark Magician` (46986414) enabling full search trigger on `Dark Magical Curtain`, increased `Pharaoh's Servant` to 2x, `Illusion of Chaos` to 2x, and `Magician of Dark Chaos` to 2x so all searchers (`Pre-Prep`, `Prep`, `Curtain`, `Soul Servant`) remain live throughout the match.
  - **`Anime_YugiExecutor.cs` Overhaul**:
    - Rewrote `OnSelectCard` with intelligent rules: never returns searched starters with `Illusion of Chaos`; intelligently reveals disposable spells for `Pharaoh's Servant`; picks top starters from `Pot of Prosperity` excavations.
    - Fixed all summon methods: `DarkMagicianGirlSummon` and `SkullArchfiendSummon` now properly check `Bot.GetMonsterCount() > 0` for 1-tribute lines.
    - Restructured priority pipeline so Turn 1 starters (`Illusion of Chaos` -> `Preparation of Rites` -> `Pre-Prep` -> `Griffoh` -> `Black Chaos` discard -> `Pharaoh's Servant` SS -> `Dark Magician of Destruction` -> `The Gaze of Timaeus` into `Red-Eyes Dark Dragoon`) activate sequentially without bottlenecking.
    - Updated `Pot of Prosperity` banish list to preserve core fusion bosses (`Dragoon`, `Dragon Knight`, `Master of Chaos`, and `Dark Magician of Destruction`).
  - **Compilation & Exclusive Deployment**: Rebuilt with 0 errors via `BUILD_AND_DEPLOY.ps1`; deployed to `C:\Users\admin\Documents\EdoGame\`. Synchronized `Anime_Yugi.ydk` to both `WindBot/Decks/` and `deck/`.

## 0.008. Elite Anime 5-Deck Expansion (2026-09-20)
- **Concept & Request**: Developed, ported, and deployed 5 elite rule-based anime executors representing 5 distinct Yu-Gi-Oh! eras (ZEXAL, VRAINS, 5D's, ARC-V, DM) with 100% verified real cards, deterministic starters, and lethal disruptions:
  1. **`Anime_Shark` (Reginald Kastle / Nash — Water Xyz & Armored Xyz)**:
     - Deck: 40 Main / 15 Extra (`Anime_Shark.ydk`).
     - Engine: `Buzzsaw Shark` (1-card Rank 3-5 Xyz), `Lantern Shark`, `Crystal Shark`, `Armored Shark`.
     - Bosses & Disruptions: `N.As.H. Knight` (Non-targeting absorption of enemy monster), `CXyz N.As.Ch. Knight` (immune to monster effects), `Full Armored Crystalzero Lancer` (Quick Effect negates all face-up opponent monsters on field), `Number C101: Silent Honor DARK` (absorbs Special Summoned monsters and floats with HP gain), `Virtue Stream` (pops 2 cards).
  2. **`Anime_BlueAngel` (Skye Zaizen / Blue Angel — Trickstar Burn & Hand Control)**:
     - Deck: 41 Main / 15 Extra (`Anime_BlueAngel.ydk`).
     - Engine: `Trickstar Candina` (searches any Trickstar), `Trickstar Light Stage` (searches, locks backrow, adds burn), `Trickstar Aqua Angel`, `Trickstar Hoody`.
     - Bosses & Disruptions: `Trickstar Reincarnation` (banishes opponent's entire hand and forces redraw, triggering massive burn with `Trickstar Lycoris`), `Trickstar Corobane` (Honest handtrap doubling ATK during damage calculation), `Trickstar Bella Madonna` (Link-4 2800 ATK tower completely unaffected by activated card effects + burns 500 per Trickstar in GY every turn).
  3. **`Anime_Crow` (Crow Hogan — Blackwing Synchro Swarm & Burn)**:
     - Deck: 40 Main / 15 Extra (`Anime_Crow.ydk`).
     - Engine: `Blackwing - Sudri the Phantom Glimmer` (searches Blackwing card + spawns tokens), `Blackwing - Simoon the Poison Wind` (sets Black Whirlwind from Deck for extra NS), `Blackwing - Shamal the Sandstorm`, `Blackwing - Zephyros the Elite`.
     - Bosses & Disruptions: `Blackbird Close` (Counter Trap activatable from hand: negates monster effect, destroys it, and cheats out `Black-Winged Dragon`), `Blackwing Full Armor Master` (3000 ATK tower immune to all effects + steals enemy monsters with Wedge Counters), `Black-Winged Assault Dragon` (3200 ATK, burns 700 every time opponent activates monster effect + quick field nuke), `Raikiri` (board wipe), `Hawk Joe` (resurrects Level 5+ Winged Beasts).
  4. **`Anime_Gong` (Gong Strong / Noboru Gongenzaka — Superheavy Samurai Steadfast)**:
     - Deck: 40 Main (100% monsters, 0 Spells/Traps) / 15 Extra (`Anime_Gong.ydk`).
     - Engine: `Superheavy Samurai Motorbike` (discards to search any SHS), `Superheavy Samurai Prodigy Wakaushi` (1-card scale setup + Special Summon), `Monk Big Benkei` (searches Soul monsters), `Soulpiercer` (non-OPT search on GY send).
     - Bosses & Disruptions: `Superheavy Samurai Brave Masurawo` (4000 DEF, draws up to 3 cards when opponent activates S/T, battles in DEF), `Baronne de Fleur` (Omni-negate + pop), `Warlord Susanowo` (3800 DEF, Quick Effect steals Spell/Trap from opponent's GY), `Ninja Sarutobi` (Quick pop S/T + 500 burn), `Soulbuster Gauntlet` (doubles DEF in damage calc up to 9600 DEF!), `Flutist` (GY target negation), `Gigagloves` (drops direct attack to 0 and draws).
  5. **`Anime_Pegasus` (Maximillion Pegasus — Modern Toon Kingdom Control)**:
     - Deck: 40 Main / 15 Extra (`Anime_Pegasus.ydk`).
     - Engine: `Toon Bookmark` (searches Toon Kingdom + GY protection), `Toon Table of Contents` (searches any Toon card), `Toon Kingdom` (blanket targeting and destruction immunity for all Toons).
     - Bosses & Disruptions: `Toon Terror` (Omni-Negate Counter Trap), `Toon Briefcase` (spins enemy summon into Deck), `Comic Hand` (steals any enemy monster and makes it attack directly), `Toon Black Luster Soldier` (3000 ATK direct attack + banishes 1 card face-up every turn), `Toon Dark Magician` (swarms and searches), `Relinquished Anima` (steals pointed monster), `Number 11: Big Eye` (permanently takes control of enemy monster), `Hope Harbinger` (Spell negate).
- **Compilation & Exclusive Deployment**: All 5 C# executors compiled with 0 errors; deployed via `BUILD_AND_DEPLOY.ps1` directly to `C:\Users\admin\Documents\EdoGame\`. All 5 decks registered in `bots.json` under the **Anime** and **Other Decks** categories in DashBot Launcher.

## 0.007. New Anime Series Rule-Based ModernExecutor: Anime_Bruno (2026-09-20)
- **Source & Concept**: Ported from YGOPRODeck Anime category (`Bruno / Antinomy: ultimate deck`). Features Bruno / Antinomy's modern T.G. (Tech Genus) Accel and Delta Accel Synchro engine with 100% deterministic combo routes, multiple high-stat bosses, and lethal disruptions.
- **Deck Composition (`Anime_Bruno.ydk`)**:
  - Main Deck (40 cards): 3x T.G. Rocket Salamander, 3x T.G. Screw Serpent, 3x T.G. Warwolf, 2x T.G. Striker, 2x T.G. Tank Grub, 2x T.G. Gear Zombie, 2x T.G. Booster Raptor, 1x T.G. Drill Fish, 1x T.G. Rush Rhino, 3x Ash Blossom & Joyous Spring, 3x Infinite Impermanence, 3x T.G. All Clear, 3x T.G. Limiter Removal, 2x Bonfire, 1x One for One, 2x Called by the Grave, 1x Harpie's Feather Duster, 3x T.G. Close.
  - Extra Deck (15 cards): 1x T.G. Trident Launcher, 2x T.G. Mighty Striker, 1x T.G. Over Dragonar, 1x T.G. Star Guardian, 1x T.G. Hyper Librarian, 1x T.G. Wonder Magician, 1x T.G. Recipro Dragonfly, 1x T.G. Power Gladiator, 1x T.G. Blade Blaster, 1x Shooting Star Dragon T.G. EX, 2x T.G. Glaive Blaster, 2x T.G. Halberd Cannon.
- **Architecture & Intelligent Executor (`Anime_BrunoExecutor.cs`)**:
  - **Deterministic Starter Engine**: 1-Card starter via `T.G. Rocket Salamander` (tributes self to Special Summon `Screw Serpent` from Deck; `Screw Serpent` on summon revives `Rocket Salamander` from GY; synchros into `T.G. Over Dragonar`). Also fully searchable via 2x `Bonfire` and 1x `One for One`.
  - **T.G. Over Dragonar Mass Revival**: Level 5 Dragon Synchro resurrects any number of T.G. monsters from GY upon Synchro Summon, forming an unstoppable swarm of materials for higher climbing.
  - **T.G. Mighty Striker & All Clear**: Level 2 Synchro Tuner searches `T.G. All Clear` (grants additional Normal Summon and pops cards to search) or `T.G. Close`; foolishes on sent to GY.
  - **Lethal End Board & Disruptions**:
    - `T.G. Glaive Blaster` (4000 ATK Delta Accel Synchro): Quick Effect banishes monsters Special Summoned from the Extra Deck (up to 2-3 times/turn) + triggers to steal any face-up banished monster onto our field ignoring summon conditions.
    - `T.G. Halberd Cannon` (4000 ATK Delta Accel Synchro): Negates any opponent monster summon (Summon Negate) and destroys it + floats on send.
    - `T.G. Close` (In-archetype Counter Trap): Omni-negates Monster effects, Spells, or Traps while controlling a Machine T.G., and automatically re-sets itself from GY when a Synchro monster is banished.
    - `Shooting Star Dragon T.G. EX` (3300 ATK Accel Synchro): Negates effects targeting friendly monsters by banishing a Tuner from GY + negates attacks.
  - **Heuristics & Target Selection**: Overrode `OnSelectCard` and `OnSelectYesNo` with smart priority ordering (Screw Serpent, Rocket Salamander, Close, All Clear, Warwolf) to prevent accidental self-disruption or miss-timing.
- **Bot Registration & Deployment**: Registered as `Anime_Bruno` in `bots.json` under the **Anime** and **Other Decks** categories; compiled and deployed exclusively to `C:\Users\admin\Documents\EdoGame\` via `BUILD_AND_DEPLOY.ps1`.

## 0.006. DashBot Launcher Frontend Revamp (2026-09-20)
- **Concept & Request**: Redesigned DashBot Launcher UI according to user requirements: removed all version numbers (`v2.3`, `2026_`), implemented a tournament-style Pill Card Grid deck selector (without card art), categorized decks (`All`, `Modern`, `Anime`, `Legacy`, `Special`), added real-time live search, added credits (`By EDO Team | Custom Deck By Jaynesiz`), and eliminated all emojis.
- **Architecture & UI Updates (`dashbot/MainWindow.xaml`, `dashbot/MainWindow.xaml.cs`)**:
  - **Clean Naming & Zero Versioning**: Removed `v2.3` from Window title, Header, Footer, and console greeting. Automated `CleanDeckDisplayName` strips prefixes (`2026_`, `Expert_2026_`, `Neural_2026_`, `AI_`, `Anime_`, `GOAT_`) while mapping proper spacing for archetype readability (e.g. `Red Dragon Archfiend`, `Dark Magician`, `Jack Atlas`).
  - **Pill Grid Deck Library**: Replaced legacy ComboBoxes with a scrollable 2-column WrapPanel grid of tournament pill badges with distinct category tags (Amber for Modern, Pink for Anime, Royal Blue for Legacy, Emerald for Special).
  - **Category Tabs & Real-Time Filter**: Instant category switching tabs with total deck counts and a live search box filtering across both display names and filenames.
  - **Duel Matchup & Dual Bot Assignment**: Added interactive toggles for Bot 1 (Player) and Bot 2 (Opponent) assignment with live matchup preview card (`[BOT 1: Deck A] VS [BOT 2: Deck B]`) and right-click shortcuts.
  - **Credits & No Emoji Rule**: Added `By EDO Team` and `Custom Deck By Jaynesiz` in both header and footer; strictly zero unicode emojis across the UI.
- **Build & Exclusive Deployment**: Built and deployed exclusively to `C:\Users\admin\Documents\EdoGame\DashBot.exe` via `BUILD_AND_DEPLOY.ps1`.

## 0.005. New Anime Series Rule-Based ModernExecutor: Anime_JackAtlas (2026-09-20)
- **Source & Concept**: Ported from YGOPRODeck Anime category (`Anime Battlebox - Jack Atlas`). Features Jack Atlas's Resonator, Red Dragon Archfiend, and Bystial Synchro engine with devastating Level 8, 9, 10, and 12 boss lines.
- **Deck Composition (`Anime_JackAtlas.ydk`)**:
  - Main Deck (45 cards): 3x Nibiru, the Primal Being, 1x The Bystial Lubellion, 3x Fidraulis Harmonia, 1x Bystial Baldrake, 1x Bystial Druiswurm, 1x Fiend Piece Golem, 3x Power Vice Dragon, 3x Bone Archfiend, 1x Wandering King Wildwind, 3x Red Lotus King, Flame Crime, 3x Darkness Resonator, 3x Soul Resonator, 1x Crimson Resonator, 2x Vision Resonator, 2x Synkron Resonator, 1x Crimson Call, 3x Resonator Call, 3x Crimson Gaia, 3x Dominus Impulse, 1x Red Reign, 1x Red Dragon Archfiend's Chain, 1x Red Zone, 1x The Ruler's Rumbling.
  - Extra Deck (15 cards): 1x Red Hypernova Dragon, 1x Hot Red Dragon Archfiend King Calamity, 1x Red Supernova Dragon, 1x Red Nova Dragon - Burning Soul, 1x Hot Red Dragon Archfiend Bane, 1x Hot Red Dragon Archfiend Abyss, 2x Red Dragon Archfiend, 1x Scarred Dragon Archfiend, 1x The Crimson King, 1x Kuibelt the Blade Dragon, 1x Crimson Blade Dragon, 3x Red Rising Dragon.
- **Architecture & Intelligent Executor (`Anime_JackAtlasExecutor.cs`)**:
  - **Soul Resonator & Bone Archfiend Engine**: Normal Summon `Soul Resonator` searches `Bone Archfiend`; `Bone Archfiend` discards to Special Summon itself and dumps `Vision Resonator` or `Crimson Resonator` from Deck to adjust levels; `Vision Resonator` adds `Crimson Gaia` to hand to set up `Red Zone` or search follow-ups.
  - **Red Rising Dragon Ladder**: Level 6 Dragon Synchro revives `Crimson Resonator` or `Soul Resonator` from GY upon Synchro Summon; banishes itself from GY to revive 2 Level 1 Resonators (`Synkron Resonator`).
  - **Crimson Resonator Multi-Summon**: When controlling exactly 1 DARK Dragon Synchro (`Red Rising Dragon` or `Scarred Dragon Archfiend`), summons 2 Resonators (`Vision Resonator` + `Synkron Resonator`) directly from Deck for immediate double or triple tuner climbing.
  - **Scarred Dragon Archfiend Float & Wipe**: When sent to GY as Synchro Material for higher DARK Dragon Synchros, automatically cheats out `Red Dragon Archfiend` from Extra Deck and destroys all opponent Attack Position monsters.
  - **Boss Lineups**:
    - `Hot Red Dragon Archfiend Abyss`: Quick Effect target negation of face-up opponent threat/floodgate cards.
    - `Hot Red Dragon Archfiend Bane`: Tributes lower monster to revive RDA bosses; revives 2 Tuners on battle damage.
    - `Red Supernova Dragon`: 4000+ ATK, effect destruction immunity; Quick Effect banishes all opponent cards on monster effect activation or attack declaration.
    - `Red Nova Dragon - Burning Soul` & `Hot Red Dragon Archfiend King Calamity`: Ultimate beatdown and turn shutdown.
  - **Disruption Grid & Handtraps**: `Dominus Impulse`, `Nibiru, the Primal Being`, `Bystial Druiswurm`, `Bystial Baldrake`, `Red Zone` (pops card when RDA triggers; revives banished Dragon Synchro), and `Red Reign` (non-targeting banish all monsters except highest level Synchro with total effect immunity).
  - **Self-Harm Guards**: Safe targeting on `Red Zone`, `Kuibelt`, and `OnSelectYesNo` empty-field protection.
  - **Lua Script Bug Fix**: Fixed `Attempting to access deleted object` at line 72 in `c70088809.lua` (`Fydraulis Harmonia`) caused by storing a temporary C++ `Group` across phase callbacks without `KeepAlive`. Resolved by storing a native Lua table of persistent `Card` objects and recreating a fresh `Group` in `effop`. Synchronized across all 4 script locations (`script/`, `script/official/`, `repositories/local-patches/`, `repositories/official-scripts/`).
- **Bot Registration & Deployment**: Registered as `Anime_JackAtlas` in `bots.json` under DashBot's **Other Decks** category; built and deployed exclusively to `C:\Users\admin\Documents\EdoGame\` via `BUILD_AND_DEPLOY.ps1`.

## 0.004. New Anime Series Rule-Based ModernExecutor: Anime_Yusei (2026-09-20)
- **Source & Concept**: Ported from YGOPRODeck Anime category (`Anime Battlebox - Yusei`). Features Yusei Fudo's Synchron, Junk, and Stardust Synchro laddering engine with massive Extra Deck boss options.
- **Deck Composition (`Anime_Yusei.ydk`)**:
  - Main Deck (45 cards): 1x Wheel Synchron, 3x Stardust Synchron, 3x Junk Meister, 2x Junk Synchron, 1x Starjunk Synchron, 3x Ash Blossom & Joyous Spring, 3x Ghost Belle & Haunted Mansion, 3x Ghost Mourner & Moonlit Chill, 3x Ghost Ogre & Snow Rabbit, 1x Full-Speed Warrior, 1x Anchorbolt Hedgehog, 1x Assault Synchron, 3x Junk Converter, 1x Scrap Synchron, 1x Jet Synchron, 1x Crossroad Sonic Chick, 3x Effect Veiler, 3x Synchro Fellowship, 3x Tuning, 3x Junk Signal, 1x Scrap-Iron Sacred Statue, 1x Majestic Mirage.
  - Extra Deck (15 cards): Shooting Quasar Dragon, Crimson Dragon, Crimson Dragon Quetzacoatl, Shooting Star Dragon, Stardust Warrior, Satellite Warrior, Accel Synchro Stardust Dragon, Stardust Dragon, Stardust Dragon - Victim Sanctuary, Shooting Riser Dragon, Stardust Charge Warrior, Junk Speeder, Scrap Warrior, Formula Synchron, Majestic Star Dragon.
- **Architecture & Intelligent Executor (`Anime_YuseiExecutor.cs`)**:
  - **Synchro Fellowship Engine**: Main Phase search for Junk Synchron + Stardust Synchron / Junk Meister; GY banish effect reduces Synchro level by 1 and grants an additional Normal Summon for Synchron monsters.
  - **Junk Speeder Engine**: Level 5 Synchro engine that summons multiple Synchron tuners with different levels directly from deck (Stardust, Junk, Assault, Jet, Wheel), enabling immediate multi-branch Synchro climbing into Level 8, 10, and 12 bosses in a single turn.
  - **Shooting Quasar Dragon Boss**: 4000 ATK Level 12 ultimate boss with multi-attack, Omni-Negate + destroy, and floats into 3300 ATK `Shooting Star Dragon` upon leaving field.
  - **Crimson Dragon Tag-Out**: Quick effect tags out with target dragon of the same level to summon Dragon Synchro bosses without ordinary requirements.
  - **Satellite Warrior Board Break**: Clears opponent cards based on the number of Synchros in the GY (wiping 3-5 cards) while gaining +1000 ATK per destroyed card (pushing past 5500+ ATK).
  - **Defensive & Handtrap Grid**: Full suite of 15 handtraps (Ash, Belle, Mourner, Ogre, Veiler, Stardust Victim Sanctuary) combined with `Stardust Warrior` (negates Special Summons) and `Junk Signal` (negates responses to Synchro activations or cheats out Stardust/Junk).
- **Bot Registration & Deployment**: Registered as `Anime_Yusei` in `bots.json`; built and deployed exclusively to `C:\Users\admin\Documents\EdoGame\` via `BUILD_AND_DEPLOY.ps1`.

## 0.003. New Anime Series Rule-Based ModernExecutor: Anime_Yugi (2026-09-20)
- **Source & Concept**: Ported from YGOPRODeck Anime category (`Anime Battlebox - Yugi`). Features modern Yugi Mutos/Atem-inspired Black Chaos and Dark Magician Ritual/Fusion support.
- **Deck Composition (`Anime_Yugi.ydk`)**:
  - Main Deck (46 cards): 3x Black Chaos, 1x Dark Magician Pharaoh's Servant, 1x Skull Archfiend of Chaos, 1x Dark Magician Girl, 1x Detonating Kuriboh, 3x Griffoh, 3x Multiplying Kuriboh!, 1x BLS Soldier of Light and Darkness, 1x Magician of Dark Chaos Black Chaos, 1x Illusion of Chaos, 1x Pot of Prosperity, 3x Preparation of Rites, 1x Triple Tactics Talent, 1x Chaos Magical Hats, 1x Chaos Mystic Box, 3x Dark Magical Curtain, 3x Forbidden Crown, 1x Soul Servant, 1x Spell Shattering Sword (BETB), 1x The Gaze of Timaeus, 2x Swords of Concealing Light, 2x Light and Darkness Ritual, 3x Pre-Preparation of Rites, 1x Secrets of Dark Magic, 1x Chaos Space, 1x Dark Magic Talisman, 3x Dominus Impulse, 1x Mind Shuffle.
  - Extra Deck (15 cards): Timaeus the United Magical Dragon, Guardian Chimera, Dark Magician the Dragon Knight, Master of Chaos, Red-Eyes Dark Dragoon, Dark Cavalry, 3x Dark Magician of Destruction, The Dark Magicians, Magi Magi ☆ Magician Gal, Black Luster Soldier - Soldier of Chaos, Day-Breaker the Shining Magical Warrior, Linkuriboh, Ebon High Magician.
- **CDB & Asset Synchronization**:
  - Registered `Spell Shattering Sword` (`101402064` / `77456448`) across all game and WindBot `cards.cdb` databases.
  - Mirrored lua script `c101402064.lua` to `c77456448.lua` and pic `101402064.jpg` to `77456448.jpg`.
  - Fixed YGOPRODeck builder quirk where Normal Spell `Chaos Space` was incorrectly listed in Extra Deck by placing it cleanly into Main Deck.
  - **Full Asset Ingestion**: Downloaded high-resolution artwork and thumbnails from official CDN for all missing cards (`Chaos Magical Hats`, `Skull Archfiend of Chaos`, `Mind Shuffle`, `Magician of Dark Chaos - Black Chaos`, `Chaos Mystic Box`, `Griffoh`, `Black Chaos`). All 41 unique cards now have 100% full-size and thumbnail artwork in `pics/` and `pics/thumbnail/`.
  - **Comprehensive Thai Translation**: Translated 100% of untranslated card effects in `Anime_Yugi.ydk` into accurate, standard Thai Yu-Gi-Oh! terminology while preserving original English card names in `texts.name` to prevent any code or logic breaking. Synchronized across all CDB locations (`cards.cdb`, `config/languages/Thai/cards.delta.cdb`, `WindBot/cards.cdb`, etc.).
  - **Lua Script Bug Fixes**: Fixed `Attempting to access deleted object` in `c98684220.lua` (Black Chaos) by passing `Card` object directly to `SetLabelObject`; fixed `Parameter 2 should be 'Effect' but is 'nil'` in `c24088928.lua` (Skull Archfiend of Chaos) and `proc_workaround.lua` by passing `e` into `:Filter(Card.IsCanBeEffectTarget, nil, e)`.
- **Architecture & Intelligent Executor (`Anime_YugiExecutor.cs`)**:
  - **Griffoh Hand Disruption**: Hand Quick effect discards to search and Set `Mind Shuffle`, `Chaos Mystic Box`, `Chaos Magical Hats`, or `Spell Shattering Sword` from deck, activatable in the same turn.
  - **Pharaoh's Servant Engine**: Reveals spell in hand to Special Summon and Set DM Spell/Trap; Quick Effect discards spell to wipe all opponent backrow (Feather Duster).
  - **Dark Magician of Destruction**: Alternative Extra Deck Summon during a turn a Spell is activated by banishing Level 6+ DARK Spellcaster; searches DM support on summon.
  - **The Gaze of Timaeus Fusion**: Recycles DM/DMG from field or GY into deck to summon `Red-Eyes Dark Dragoon` (omni-negate + pop/burn), `Dark Magician the Dragon Knight` (backrow protection), or `Master of Chaos` (revives and banishes all enemy monsters).
  - **Light & Darkness Ritual**: Recursively rituals `Magician of Dark Chaos` (recovers Spells, face-down banishes opponent card) or `BLS Soldier of Light & Darkness` (non-targeting banish, +1500 ATK double attack); infinite recursion via GY return effect.
  - **Black Chaos Tower**: Shuffles back used Rituals to SS a 3000 ATK body immune to opponent activated effects (when Ritual Spell in GY) and ignitions a non-targeting double banish.
  - **Handtraps & Disruption**: Real-time chain interaction with `Dominus Impulse`, `Detonating Kuriboh`, `Forbidden Crown`, `Dark Magic Talisman`, and `Multiplying Kuriboh`.
- **Bot Registration & Deployment**: Registered as `Anime_Yugi` in `bots.json`; built and deployed exclusively to `C:\Users\admin\Documents\EdoGame\` via `BUILD_AND_DEPLOY.ps1`.

## 0.002. New Rule-Based ModernExecutor: 2026_DogmaStun (2026-09-20)
- **Deck Strategy**: Dogmatika Anti-Meta Stun hybrid combining Inspector Boarder, Dogmatika Ecclesia, Nadir Servant, Decisive Battle of Golgonda, The Fallen & The Virtuous, Necrovalley, Super Polymerization, Card of Demise, Pot of Prosperity, and heavy Counter Traps (Solemn Strike, Solemn Judgment, Crackdown, Skill Drain, Dogmatika Punishment).
- **Architecture**: Implemented in `_2026_DogmaStunExecutor.cs` inheriting `ModernExecutor`.
  - Registered `ResourcePlan.RegisterAceCards` for Inspector Boarder, The Dragon that Devours the Dogma, Starving Venom Fusion Dragon, S:P Little Knight, TY-PHON.
  - Registered `BaitPlanner.RegisterComboStarters` (Pot of Prosperity, Terraforming, Nadir Servant).
  - Registered `ChainAdvisor.RegisterHighValueTargets` (Inspector Boarder, Skill Drain, Necrovalley).
  - Registered `_optionalFieldRemovalCards` for `TheFallenAndTheVirtuous` to prevent self-destruction on empty enemy fields.
  - Smart Extra Deck dumping for `DogmatikaPunishment` & `NadirServant` (N'tss for 2nd pop, Garura for draw, Devours Dogma for 3000 ATK send & End Phase search, Titaniklad/Albion for End Phase search/set).
  - Unrespondable `SuperPolymerization` logic targeting Starving Venom, Mudragon, or Garura using materials across both fields.
  - High-IQ disruption timing: `SolemnJudgment` prioritizes board-wipes (`Harpie's`, `Lightning Storm`, `Raigeki`, `Evenly`) and major starters (`Branded Fusion`); `SolemnStrike` targets Extra Deck summons and Chokepoint/Negator activations; `DogmatikaPunishment` and `Crackdown` intercept on-field monster effects, Extra Deck climbs, and Battle Phase pushes.
  - Harmonized `CardOfDemise` and `PotOfProsperity` to prevent conflicting activation in the same turn, ensuring all Traps are deployed before Demise.
- **Deck & Bot Registration**: Copied `2026_DogmaStun.ydk` to both `windbot-fork\Decks` and `WindBot\Decks`; registered `2026_DogmaStun` in `bots.json`.
- **Exclusive Deployment**: Built and deployed to `C:\Users\admin\Documents\EdoGame\` via `BUILD_AND_DEPLOY.ps1`.

## 0.001. Non-Legacy Archetypes Comprehensive Optimization & Engine Fixes (2026-09-20)
- **cards.cdb Merge**: 174 missing 2026 cards merged from expansions to SQLite; resolved `2026_Angelechy` missing card failure.
- **bots.json Path Fix**: Aligned 4 broken deck path filenames (`Rank5`, `Level8`, `ToadallyAwesome`, `ZexalWeapons`).
- **ModernExecutor Central Protection**:
  - Registered `_optionalFieldRemovalCards`: blocks self-board wipes via `OnSelectYesNo` when enemy has 0 cards.
  - Hardened `GetMaterialSacrificePriority`: Ace cards are assigned priority 100000 to prevent sacrificing boss monsters as link/xyz materials.
- **Archetype Fixes & Modernization**:
  - `2026_Maliss`: Allowed 1-card Link climbing into Link-1 (Link Disciple / Linguriboh) from expendable Maliss monsters.
  - `2026_Fireking`: Prevented Fire King Island from wiping bot's field; guarded Kirin from self-destructing with no enemy cards; prioritized enemy targets on destroy hint.
  - `2026_Exosister`: Enforced strict Martha Xyz condition + Elis in deck check; fixed Magnifica field-only banish; added Mikailis GY disruption.
  - `2026_KaijuCrusadia`: Implemented `IsAceCard` override, ComboRouter lines, and `IsBoardStrongEnough`.
  - `2026_Tearla`: Integrated ComboRouter sequencing (Reinoheart-Kitkallos & Fiendsmith lines), BaitPlanner, and ChainAdvisor.
  - `2026_Yummy`: Integrated ComboRouter lines (Marshmao-Cupsy & Cooky-Lollipo-Borreload), BaitPlanner, and ChainAdvisor.
  - `2026_Purrely`: Integrated BaitPlanner, ChainAdvisor, and ResourcePlan.
  - `2026_Stun`: Registered ResourcePlan, BaitPlanner, ChainAdvisor, and OptionalFieldRemoval for The Fallen & The Virtuous.
  - Blind `cards[0]` selection replaced with smart target/threat selection in Archfiend, Dreadnought, Regenesis, Darklord, and Doomz.
- **Deployment**: Compiled and deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.

## 0.000. Critical Freeze & Redundant Chain Prevention Hotfix (2026-09-06)

### Root Cause Analysis & Problem Statement
1. **Engine Freeze / Infinite Loop**:
   - In live duels (e.g. `2026_AFS vs ABC`), the engine generated over 58,000 log lines in 2 seconds and completely froze.
   - When a primary combo line (`AFS-TripleEngine-OmniBoard`) failed midway at Engraver, `ComboRouter` switched to fallback `Fiendsmith-Caesar-Line`.
   - When `Fiendsmith-Caesar-Line` failed at Poplar, `ComboRouter` reset and switched right back to `AFS-TripleEngine-OmniBoard`.
   - Inside `ModernExecutor.cs`: `while (ComboRouter.HasActiveCombo)` ping-ponged between the two lines in an infinite while loop without passing control back to the game engine.
2. **Redundant Chains & "Player Not Reading Card Text" Behavior**:
   - The bot repeatedly chained multiple copies of `Infinite Impermanence (10045474)` or `Effect Veiler (97268402)` on a single monster that was **already disabled/negated** (e.g. ABC-Dragon Buster).
   - In `CardContainer.cs` (`GetShouldBeDisabledBeforeItUseEffectMonster`), it did NOT check `!card.IsDisabled()`. Because ABC-Dragon Buster matched the high-threat list, the function kept returning it as an active target even after its effects were already negated.
   - In `DefaultExecutor.cs` (`DefaultDisableMonster`), it lacked checks preventing duplicate negation cards in the current chain link.

### Core Fixes Implemented
1. **`ComboRouter.cs` (Turn-scoped Failed Line Blacklist)**:
   - Added `_failedLinesThisTurn = new HashSet<string>()`.
   - When any combo line fails execution, it is added to `_failedLinesThisTurn` and cannot be reselected during the same turn.
   - Clear blacklist automatically on each new turn (`Reset()`).
2. **`ModernExecutor.cs` (Combo Loop Safety Limiter)**:
   - Added hard iteration cap (`int maxIterations = 15; while (ComboRouter.HasActiveCombo && --maxIterations > 0)`) preventing runaway while-loops.
   - Hardened `DefaultEffectVeiler` with `!lastChain.IsDisabled()` and duplicate chain check.
3. **`CardContainer.cs` (`GetShouldBeDisabledBeforeItUseEffectMonster`)**:
   - Added `!card.IsDisabled()` check: will never return an opponent monster that is already negated.
4. **`DefaultExecutor.cs` (`DefaultDisableMonster`)**:
   - Added check ensuring `!target.IsDisabled()` and blocking activation if `Duel.CurrentChain` already contains an active negation targeting that monster.
5. **`GameAI.cs` (`ShouldExecute`)**:
   - Added universal Central Core Handtrap/Negator guards:
     - Block activating identical handtraps/negators in the same chain link.
     - Block chaining targeted negators (Imperm/Veiler) if `Duel.LastChainPlayer == 0` (bot's own card).
     - Block chaining targeted negators if `Duel.CurrentChain.LastOrDefault()` is already disabled.

### Headless Duel Verification (2026_AFS vs ABC)
- 3/3 duels completed with **Status: OK**, 0 Violations, 0 Crashes, 0 Freezes (average 12.9s per duel).
- Redundant chains onto disabled monsters completely eliminated.

---

## 0.00. Central Core Intelligence & ModernExecutor Overhaul (2026-09-06)

### Objective
Systemic overhaul of Central Core AI (`ExecutorBase`, `GameAI`, `ModernExecutor`, `DefaultExecutor`, `CardIntelligence`, `ComboRouter`) to eliminate bot misplays, suicide plays, designation corruption, and ensure genuine tournament-grade Player vs CPU intelligence without touching individual deck files.

### 6 Core Architecture Upgrades
1. **Permanent Match-Level Designation (`IsGoingFirst` / `IsGoingSecond`)**:
   - **Root Cause**: `_isGoingSecond = (Duel.Turn > 1)` was executed every turn across dozens of 2026 executors and `ModernExecutor`. On Turn 3 (bot went first), `_isGoingSecond` flipped to `true`, destroying Turn 1 board setups and triggering going-second board breaker code inappropriately. Furthermore, on Turn 1 when opponent went first, `IsGoingSecond` was unset until Turn 2.
   - **Fix**: Replaced `_isGoingSecond` field in `ModernExecutor` with a property backed by `IsGoingSecond`. Locked match designation permanently on Turn 1 in `Executor.PreNewTurn()` based on `(Duel.Turn % 2)` and `Duel.Player`.
2. **Chain Link 3 Defense & Pre-emptive Draw/Standby Floodgates**:
   - **Root Cause**: When opponent responded with handtraps to interrupt bot combos, bots did not automatically chain `Called by the Grave` or `Crossout Designator` as Chain Link 3. Additionally, continuous floodgates (`Skill Drain`, `There Can Be Only One`, `Anti-Spell Fragrance`, `Dimensional Barrier`) were held until Main Phase when opponent already cast board breakers (Harpie's, Lightning Storm, Evenly Matched).
   - **Fix**: Implemented `CheckChainLink3Defense` and `CheckDrawStandbyFloodgate` in `Executor.cs`, and wired them directly into `GameAI.OnSelectChain` so CL3 protection and Draw/Standby flips occur reliably across all bots.
3. **Universal `OnSelectYesNo` Self-Harm Guard**:
   - **Root Cause**: `Executor.OnSelectYesNo` unconditionally returned `true`. Cards with optional removal targeting "on the field" (e.g. `Dracotail Pan`, `Dracotail Urgula`, `Epurrely Plump`) prompted "Do you want to destroy 1 card on the field?". On Turn 1 or empty opponent fields, answering Yes forced the bot to destroy/banish its own cards.
   - **Fix**: Implemented `OnSelectYesNo` override in `ModernExecutor` guarding against empty opponent fields and specific string IDs, refusing optional removal when enemy has 0 valid targets.
4. **ComboRouter Dynamic Fallback & Plan B Execution**:
   - **Root Cause**: When a mandatory combo step was negated or unplayable, `ComboRouter` aborted the entire combo and passed the turn without checking alternative lines.
   - **Fix**: Added `TrySwitchToFallback(ClientField bot)` in `ComboRouter.cs`. When a step fails in `ModernExecutor.OnSelectIdleCmd`, the engine immediately attempts the registered `FallbackLineName` or re-evaluates available cards to seamlessly switch to Plan B/C.
5. **Universal Threat-Weighted Battle Targeting & Baiting**:
   - **Root Cause**: Bots attacked face-up high-ATK targets blindly, crashed into damage reflection monsters (Mikanko, Yubel, Timelords), or walked into face-down backrow with high-ATK bosses first.
   - **Fix**: Added `IsDangerousBattleTarget()` in `CardIntelligence.cs` (blocking Mikanko, Yubel, Timelords, Lion Heart, Sphreeze). Rewrote `OnSelectAttackTarget()` with threat scoring (negators +120, floodgates +100, lethal push bonus). Added face-down backrow baiting in `GameAI.InternalOnSelectBattleCmd` (attacking with lowest ATK monster first unless lethal is assured).
6. **Effect Veiler Canonical ID Correction & CardIntelligence Unification**:
   - **Root Cause**: `63845230` (Eater of Millions) was mistakenly hardcoded as Effect Veiler across `BoardScorer`, `ChainTimingAdvisor`, `ModernExecutor`, `StateRepresentation`, `DynamicValueEvaluator`, `BeliefState`, and `2026_GemKnight.ydk`. Real Effect Veiler (`97268402`) was ignored, and drawing Eater of Millions caused false handtrap holding.
   - **Fix**: Corrected all references to `97268402` for Effect Veiler, separated `63845230` as Eater of Millions, corrected `Crossout Designator` canonical ID to `65681983`, and added `Dominus Impulse`, `Dominus Purge`, and `PSY-Framegear Gamma` to `CardIntelligence`.

### Headless Duel Simulation Verification
- **`2026_Dracotail vs Blue-Eyes`**: 4 Wins / 1 Loss (**80.0% Win Rate**) | 0 Violations, 0 Crashes
- **`2026_Dracotail vs ABC`**: 1 Win / 2 Losses (**33.3% Win Rate**) | 0 Violations, 0 Crashes
- **`2026_Dracotail vs DarkMagician`**: 2 Wins / 1 Loss (**66.7% Win Rate**) | 0 Violations, 0 Crashes
- **`2026_Dracotail vs Altergeist`**: 2 Wins / 1 Loss (**66.7% Win Rate**) | 0 Violations, 0 Crashes
- **`2026_Stun vs Blue-Eyes`**: 2 Wins / 1 Loss (**66.7% Win Rate**) | 0 Violations, 0 Crashes
- **Overall**: 11 Wins / 6 Losses (**64.7% Win Rate**), **0 Engine Crashes, 0 MSG_RETRY Violations**.

---

## 0.0. Central Core Architecture Upgrade: Duplicate Chain & Self-Negation Prevention (2026-09-06)

### Root Cause Analysis & Problem Statement
- **Issue**: In `2026_Stun` duels, logs revealed that the bot frequently performed redundant self-chains with Counter Traps (specifically `Solemn Report 78114463`), paying 1500 LP twice in the same chain (e.g. Dogmatika Punishment CL1 → Solemn Report CL2 → Solemn Report CL3).
- **Core Defects Found**:
  1. `DefaultExecutor.cs` defined `DefaultSolemnJudgment`, `DefaultSolemnWarning`, and `DefaultSolemnStrike`, but lacked `DefaultSolemnReport`.
  2. `_2026_StunExecutor.cs` used an ad-hoc method `SolemnReportEffect()` with `(Duel.LastChainPlayer == 1 || Duel.Player == 1)`. In opponent's turn (`Duel.Player == 1`), this was always true even when `Duel.LastChainPlayer == 0` (the bot itself just activated a card), causing Solemn Report to negate the bot's own traps and double-chain.
  3. No central guard in `DefaultExecutor` or `GameAI.ShouldExecute` prevented Counter Traps from double-activating in the same chain or negating cards controlled by the bot.

### Core Architecture Enhancements Implemented
1. **`DefaultExecutor.cs`**:
   - Added `SolemnReport = 78114463;` to `_CardId`.
   - Added `DefaultSolemnReport()`: Enforces `Bot.LifePoints > 1500`, `!Util.ChainContainsCard(Card.Id)`, `Duel.LastChainPlayer == 1`, and ensures the target is an active opponent Spell/Trap card.
   - Enhanced `DefaultSolemnJudgment()`, `DefaultSolemnWarning()`, and `DefaultSolemnStrike()` with `!Util.ChainContainsCard(Card.Id)`, `Duel.LastChainPlayer != 0`, and checks that the last chain card is not bot-controlled or already disabled.
2. **`GameAI.cs` (`ShouldExecute`)**:
   - Implemented universal Central Core Counter Trap guards for `card.HasType(CardType.Counter)`:
     - Automatically rejects activation if `Duel.LastChainPlayer == 0` (immediately preceding chain link is bot's own card).
     - Automatically rejects activation if `Duel.LastChainPlayer == -1 && Duel.LastSummonPlayer == 0` (bot's own summon).
     - Automatically rejects activation if `Duel.CurrentChain.Any(c => c.IsCode(card.Id))` (duplicate counter-trap in the same chain).
     - Automatically rejects activation if `Duel.CurrentChain.LastOrDefault()?.Controller == 0`.
3. **`_2026_StunExecutor.cs`**:
   - Delegated `SolemnReport` to `DefaultSolemnReport`.
   - Hardened `DominusSparkEffect` with `Duel.LastChainPlayer != 0` guard.
   - Hardened `MaxxCEffect` against duplicate chains.

### Headless Duel Simulation Verification (40 Duels)
- **ABC**: 6 Wins / 4 Losses (**60.0%**, jumped from 30% baseline!) | 0 Violations, 0 Crashes
- **Altergeist**: 6 Wins / 4 Losses (**60.0%**) | 0 Violations, 0 Crashes
- **BlueEyes**: 5 Wins / 5 Losses (**50.0%**) | 0 Violations, 0 Crashes
- **DarkMagician**: 7 Wins / 3 Losses (**70.0%**) | 0 Violations, 0 Crashes
- **Total**: 24 Wins / 16 Losses (**60.0% Win Rate**), **0 MSG_RETRY Errors, 0 Crashes**, and **0 redundant double-chains / 0 self-negations**.


---

## 📚 Historical Development Archives
> ประวัติการพัฒนาและบันทึกการทดสอบเวอร์ชันก่อนหน้า (ตั้งแต่ 0.000 ย้อนหลังไป) ได้รับการย้ายไปจัดเก็บอย่างเป็นหมวดหมู่ที่:
> [Docs/PROGRESS_ARCHIVE.md](Docs/PROGRESS_ARCHIVE.md)
