using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ============================================================
    // 2026_Dracotail Executor (ModernExecutor Architecture)
    // ============================================================
    // Dracotail is a powerhouse Fusion engine utilizing Hand, Field, and DECK
    // materials to summon high-stat Fusion dragons (Arthalion 3300, Gulamel 2800,
    // Shaulas 2500) while triggering material effects in GY:
    // - Urgula: Set S/T from Deck + Destroy 1 S/T on field
    // - Pan: Set S/T from Deck + Destroy 1 Monster on field
    // - Mululu: Set S/T from Deck + Negate 1 face-up Monster
    // - Lukias: Add Dracotail monster / Set S/T from Deck
    // - Faimena: Quick Fusion / Set S/T from Deck / Recycle from GY
    //
    // Synergies:
    // - Branded: Fallen of the White Dragon dumps Albion -> searches Branded Fusion / The Fallen & The Virtuous
    // - Magistus: Spenta searches Crowley/Zoroa -> free Special Summons & Fusions
    // ============================================================

    [Deck("2026_Dracotail", "2026_Dracotail")]
    public class _2026_DracotailExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck — Dracotail Monsters
            public const int DracotailLukias = 75003700;
            public const int DracotailFaimena = 1498449;
            public const int DracotailPhryxul = 84477320;
            public const int DracotailMululu = 7375867;
            public const int DracotailUrgula = 70871153;
            public const int DracotailPan = 44482554;

            // Main Deck — Dracotail Spells/Traps
            public const int KetuDracotail = 6153210;
            public const int RahuDracotail = 32548318;
            public const int DracotailSting = 80208225;
            public const int DracotailHorn = 69932023;
            public const int DracotailFlame = 5431722;

            // Main Deck — Branded Engine
            public const int FallenOfTheWhiteDragon = 73819701;
            public const int BlazingCartesiaTheVirtuous = 95515789;
            public const int IncredibleEcclesiaTheVirtuous = 55273560;
            public const int BrandedFusion = 44362883;
            public const int TheFallenAndTheVirtuous = 30271097;

            // Main Deck — Magistus Engine
            public const int ZoroaTheMagistusOfFlame = 36099130;
            public const int CrowleyTheGiftedOfMagistus = 875572;
            public const int SpoonTheSealOfMagistus = 42544773; // Spenta, the Magistus Sealer

            // Main Deck — Staples / Handtraps
            public const int MulcharmyFuwalos = 42141493;
            public const int AshBlossom = 14558128;
            public const int GhostOgre = 59438930;
            public const int GhostBelle = 73642296;
            public const int DrollAndLockBird = 94145021;
            public const int BystialMagnamhut = 33854624;
            public const int PotOfProsperity = 84211599;

            // Extra Deck — Dracotail Fusions
            public const int DracotailArthalion = 33760966;
            public const int DracotailGulamel = 79755671;
            public const int DracotailShaulas = 42125140;

            // Extra Deck — Branded Fusions & Synchro
            public const int AlbionTheBrandedDragon = 87746184;
            public const int AlbaLenatusTheAbyssDragon = 3410461;
            public const int SecreterionDragon = 89851827;
            public const int EcclesiaAndTheDarkDragon = 78397661;
            public const int KhaosStarsourceDragon = 72578374;

            // Extra Deck — Magistus Fusions, Synchro & Link
            public const int MagistusChorozo = 66532962;
            public const int InvokedMagistusOmega = 38423248;
            public const int ZoroaVerethragna = 37260677;
            public const int AiwassMagistusSpellSpirit = 35877582;
            public const int ZoroaConflagrantCalamity = 95911373;
            public const int ArtemisMagistusMoonMaiden = 34755994;

            // Side Deck
            public const int CalledByTheGrave = 24224830;
            public const int ForbiddenDroplet = 24299458;
        }

        private static readonly int[] HandTraps = {
            CardId.AshBlossom,
            CardId.MulcharmyFuwalos,
            CardId.GhostOgre,
            CardId.GhostBelle,
            CardId.DrollAndLockBird,
            CardId.CalledByTheGrave,
            CardId.BystialMagnamhut,
            CardId.ForbiddenDroplet
        };

        // Once-per-turn trackers
        private bool _lukiasSearchUsed = false;
        private bool _lukiasMatUsed = false;
        private bool _faimenaQuickUsed = false;
        private bool _faimenaGYUsed = false;
        private bool _phryxulSummonUsed = false;
        private bool _mululuQuickUsed = false;
        private bool _mululuMatUsed = false;
        private bool _urgulaMatUsed = false;
        private bool _urgulaGYUsed = false;
        private bool _panMatUsed = false;
        private bool _panGYUsed = false;
        private bool _ketuUsed = false;
        private bool _rahuUsed = false;
        private bool _stingUsed = false;
        private bool _hornUsed = false;
        private bool _flameUsed = false;
        private bool _arthalionUsed = false;
        private bool _gulamelQuickUsed = false;
        private bool _shaulasUsed = false;
        private bool _brandedFusionUsed = false;
        private bool _fallenUsed = false;
        private bool _fallenWhiteDragonSSUsed = false;
        private bool _albionGYUsed = false;
        private bool _potProsperityUsed = false;
        private bool _spentaDiscardUsed = false;
        private bool _spentaGYUsed = false;
        private bool _crowleyFuseUsed = false;
        private bool _zoroaEquipUsed = false;
        private bool _secreterionUsed = false;
        private bool _cartesiaUsed = false;

        private bool _fusionLockActive = false;

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.DracotailArthalion
                || card.Id == CardId.DracotailGulamel
                || card.Id == CardId.DracotailShaulas
                || card.Id == CardId.AlbionTheBrandedDragon
                || card.Id == CardId.AlbaLenatusTheAbyssDragon
                || card.Id == CardId.EcclesiaAndTheDarkDragon
                || card.Id == CardId.InvokedMagistusOmega
                || card.Id == CardId.ZoroaVerethragna
                || card.Id == CardId.SecreterionDragon
                || card.Id == CardId.KhaosStarsourceDragon;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c) && c.Location == CardLocation.MonsterZone) return 900;
            if (HandTraps.Contains(c.Id)) return 800;
            if (c.Location == CardLocation.Deck)
            {
                if (c.Id == CardId.DracotailUrgula) return 10;
                if (c.Id == CardId.DracotailPan) return 11;
                if (c.Id == CardId.DracotailLukias) return 12;
                if (c.Id == CardId.DracotailFaimena) return 13;
                if (c.Id == CardId.FallenOfTheWhiteDragon) return 15;
            }
            if (c.Location == CardLocation.Hand)
            {
                if (c.Id == CardId.DracotailPan) return 20;
                if (c.Id == CardId.DracotailUrgula) return 21;
                if (c.Id == CardId.FallenOfTheWhiteDragon) return 22;
                if (c.Id == CardId.DracotailMululu) return 23;
                if (c.Id == CardId.DracotailPhryxul) return 24;
            }
            return base.GetMaterialPriority(c);
        }

        public _2026_DracotailExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(
                CardId.DracotailArthalion,
                CardId.DracotailGulamel,
                CardId.DracotailShaulas,
                CardId.AlbionTheBrandedDragon,
                CardId.AlbaLenatusTheAbyssDragon,
                CardId.EcclesiaAndTheDarkDragon,
                CardId.InvokedMagistusOmega,
                CardId.ZoroaVerethragna,
                CardId.SecreterionDragon,
                CardId.KhaosStarsourceDragon
            );

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Rahu-Deck-Fusion-OTK",
                RequiredCards = new List<int> { CardId.RahuDracotail },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.RahuDracotail, ActionType = ExecutorType.Activate, Description = "Rahu Fuses Arthalion using Urgula + Pan from Deck" },
                    new() { CardId = CardId.DracotailArthalion, ActionType = ExecutorType.SpSummon, Description = "Summon 3300 ATK Arthalion" }
                },
                EndBoardScore = 95
            });

            BaitPlanner.RegisterComboStarters(CardId.RahuDracotail, CardId.DracotailLukias, CardId.KetuDracotail, CardId.BrandedFusion);
            BaitPlanner.RegisterBaitCards(CardId.PotOfProsperity, CardId.KetuDracotail, CardId.SpoonTheSealOfMagistus);
            ChainAdvisor.RegisterHighValueTargets(CardId.RahuDracotail, CardId.DracotailLukias, CardId.BrandedFusion, CardId.DracotailArthalion);

            // ============================================================
            // TIER 1: Hand Traps & Reactive Negations (DIRECT, NO HOLDING)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.GhostOgre, GhostOgreEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, GhostBelleEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.BystialMagnamhut, BystialMagnamhutEffect);

            // ============================================================
            // TIER 2: Dracotail Traps (Disruption & Resource Loop)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.DracotailSting, DracotailStingEffect);
            AddExecutor(ExecutorType.Activate, CardId.DracotailHorn, DracotailHornEffect);
            AddExecutor(ExecutorType.Activate, CardId.DracotailFlame, DracotailFlameEffect);

            // ============================================================
            // TIER 3: Fusion Boss Trigger & Quick Effects
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.DracotailArthalion, ArthalionEffect);
            AddExecutor(ExecutorType.Activate, CardId.DracotailGulamel, GulamelQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.DracotailShaulas, ShaulasEffect);
            AddExecutor(ExecutorType.Activate, CardId.ZoroaVerethragna, ZoroaVerethragnaEffect);
            AddExecutor(ExecutorType.Activate, CardId.AiwassMagistusSpellSpirit, AiwassEffect);
            AddExecutor(ExecutorType.Activate, CardId.SecreterionDragon, SecreterionEffect);
            AddExecutor(ExecutorType.Activate, CardId.EcclesiaAndTheDarkDragon, EcclesiaAndDarkDragonEffect);

            // ============================================================
            // TIER 4: Quick Fusions (Rahu, Faimena, Mululu, Cartesia)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.RahuDracotail, RahuEffect);
            AddExecutor(ExecutorType.Activate, CardId.DracotailFaimena, FaimenaQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.DracotailMululu, MululuQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.BlazingCartesiaTheVirtuous, CartesiaEffect);

            // ============================================================
            // TIER 5: Dracotail GY Trigger Effects (Set S/T + Pop/Negate)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.DracotailLukias, LukiasMatEffect);
            AddExecutor(ExecutorType.Activate, CardId.DracotailPhryxul, PhryxulMatEffect);
            AddExecutor(ExecutorType.Activate, CardId.DracotailMululu, MululuMatEffect);
            AddExecutor(ExecutorType.Activate, CardId.DracotailUrgula, UrgulaMatEffect);
            AddExecutor(ExecutorType.Activate, CardId.DracotailPan, PanMatEffect);
            AddExecutor(ExecutorType.Activate, CardId.DracotailFaimena, FaimenaGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.DracotailUrgula, UrgulaGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.DracotailPan, PanGYEffect);

            // ============================================================
            // TIER 6: Starter Spells & Searchers (Rahu, Ketu, Branded Fusion, White Dragon)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, PotOfProsperityEffect);
            AddExecutor(ExecutorType.Activate, CardId.RahuDracotail, RahuEffect);
            AddExecutor(ExecutorType.Activate, CardId.BrandedFusion, BrandedFusionEffect);
            AddExecutor(ExecutorType.Activate, CardId.KetuDracotail, KetuEffect);
            AddExecutor(ExecutorType.Activate, CardId.FallenOfTheWhiteDragon, FallenWhiteDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.SpoonTheSealOfMagistus, SpentaEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheFallenAndTheVirtuous, TheFallenAndTheVirtuousEffect);

            // ============================================================
            // TIER 7: On-Summon Triggers & Ignitions (Lukias, Phryxul, Crowley, Zoroa)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.DracotailLukias, LukiasSearchEffect);
            AddExecutor(ExecutorType.Activate, CardId.DracotailPhryxul, PhryxulSummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.CrowleyTheGiftedOfMagistus, CrowleyEffect);
            AddExecutor(ExecutorType.Activate, CardId.ZoroaTheMagistusOfFlame, ZoroaFlameEffect);
            AddExecutor(ExecutorType.Activate, CardId.IncredibleEcclesiaTheVirtuous, EcclesiaTributeEffect);
            AddExecutor(ExecutorType.Activate, CardId.AlbionTheBrandedDragon, AlbionGYEffect);

            // ============================================================
            // TIER 8: Normal Summons
            // ============================================================
            AddExecutor(ExecutorType.Summon, CardId.DracotailLukias, ShouldSummonLukias);
            AddExecutor(ExecutorType.Summon, CardId.DracotailPhryxul, ShouldSummonPhryxul);
            AddExecutor(ExecutorType.Summon, CardId.DracotailMululu, ShouldSummonMululu);
            AddExecutor(ExecutorType.Summon, CardId.ZoroaTheMagistusOfFlame, ShouldSummonMagistus);
            AddExecutor(ExecutorType.Summon, CardId.CrowleyTheGiftedOfMagistus, ShouldSummonMagistus);
            AddExecutor(ExecutorType.Summon, CardId.FallenOfTheWhiteDragon, ShouldSummonBranded);
            AddExecutor(ExecutorType.Summon, CardId.BlazingCartesiaTheVirtuous, ShouldSummonBranded);
            AddExecutor(ExecutorType.Summon, CardId.IncredibleEcclesiaTheVirtuous, ShouldSummonBranded);
            AddExecutor(ExecutorType.Summon, CardId.DracotailFaimena, ShouldSummonFaimena);

            // Fallback Summons
            AddExecutor(ExecutorType.Summon, CardId.DracotailLukias);
            AddExecutor(ExecutorType.Summon, CardId.DracotailPhryxul);
            AddExecutor(ExecutorType.Summon, CardId.DracotailMululu);
            AddExecutor(ExecutorType.Summon, CardId.ZoroaTheMagistusOfFlame);

            // ============================================================
            // TIER 9: Extra Deck Special Summons (SAFE ONLY - NO ACCIDENTAL FUSION SACRIFICE)
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.DracotailArthalion);
            AddExecutor(ExecutorType.SpSummon, CardId.DracotailGulamel);
            AddExecutor(ExecutorType.SpSummon, CardId.DracotailShaulas);
            AddExecutor(ExecutorType.SpSummon, CardId.AlbaLenatusTheAbyssDragon);
            AddExecutor(ExecutorType.SpSummon, CardId.AlbionTheBrandedDragon);
            AddExecutor(ExecutorType.SpSummon, CardId.EcclesiaAndTheDarkDragon);
            AddExecutor(ExecutorType.SpSummon, CardId.SecreterionDragon);
            AddExecutor(ExecutorType.SpSummon, CardId.KhaosStarsourceDragon);
            AddExecutor(ExecutorType.SpSummon, CardId.InvokedMagistusOmega, InvokedOmegaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ZoroaVerethragna, ZoroaVerethragnaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MagistusChorozo, ChorozoSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ArtemisMagistusMoonMaiden, ArtemisSummon);

            // ============================================================
            // TIER 10: Spell/Trap Sets
            // ============================================================
            AddExecutor(ExecutorType.SpellSet, CardId.DracotailSting);
            AddExecutor(ExecutorType.SpellSet, CardId.DracotailHorn);
            AddExecutor(ExecutorType.SpellSet, CardId.DracotailFlame);
            AddExecutor(ExecutorType.SpellSet, CardId.TheFallenAndTheVirtuous);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet);

            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand() => true; // Prefer Going 1st for Traps & Arthalion

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _lukiasSearchUsed = false;
            _lukiasMatUsed = false;
            _faimenaQuickUsed = false;
            _faimenaGYUsed = false;
            _phryxulSummonUsed = false;
            _mululuQuickUsed = false;
            _mululuMatUsed = false;
            _urgulaMatUsed = false;
            _urgulaGYUsed = false;
            _panMatUsed = false;
            _panGYUsed = false;
            _ketuUsed = false;
            _rahuUsed = false;
            _stingUsed = false;
            _hornUsed = false;
            _flameUsed = false;
            _arthalionUsed = false;
            _gulamelQuickUsed = false;
            _shaulasUsed = false;
            _brandedFusionUsed = false;
            _fallenUsed = false;
            _fallenWhiteDragonSSUsed = false;
            _albionGYUsed = false;
            _potProsperityUsed = false;
            _spentaDiscardUsed = false;
            _spentaGYUsed = false;
            _crowleyFuseUsed = false;
            _zoroaEquipUsed = false;
            _secreterionUsed = false;
            _cartesiaUsed = false;
            _fusionLockActive = false;
        }

        private int GetThreatScore(ClientCard card)
        {
            if (card == null) return 0;
            int score = 0;
            if (card.IsMonster())
            {
                score += 1000 + card.Attack;
                if (card.IsExtraCard()) score += 3000;
                if (card.IsFaceup() && !card.IsDisabled()) score += 5000;
                if (card.Id == 1561110) score += 8000; // ABC-Dragon Buster
                if (card.Id == 21887175) score += 6000; // Avramax
                if (card.Id == 4280258) score += 7000; // Apollousa
                if (card.Id == 83152482) score += 5000; // Union Carrier
                if (card.Id == 10443957) score += 8500; // Cyber Dragon Infinity
            }
            else if (card.IsSpell() || card.IsTrap())
            {
                score += 800;
                if (card.IsFaceup()) score += 2000;
                if (card.Id == 66399653) score += 6000; // Union Hangar
            }
            return score;
        }

        private bool MonsterRepos()
        {
            if (Card == null) return DefaultMonsterRepos();
            bool enemyEmpty = Enemy.GetMonsterCount() == 0;
            if (Card.IsAttack())
            {
                if (!enemyEmpty && Card.Attack < 1000 && Card.Defense >= 1000)
                    return true;
            }
            else
            {
                if (enemyEmpty || (Card.Attack >= 1500 && !Card.HasType(CardType.Token)))
                    return true;
            }
            return false;
        }

        // ============================================================
        // TIER 1 Handtraps (DIRECT INTERRUPTIONS)
        // ============================================================
        private bool GhostOgreEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return DefaultGhostOgreAndSnowRabbit();
        }

        private bool AshBlossomEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool GhostBelleEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return DefaultGhostBelleAndHauntedMansion();
        }

        private bool MulcharmyEffect()
        {
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool DrollAndLockBirdEffect()
        {
            return Duel.Player == 1 && Duel.LastChainPlayer == 1;
        }

        private bool CalledByTheGraveEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return DefaultCalledByTheGrave();
        }

        private bool BystialMagnamhutEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (Duel.Player == 1)
            {
                bool oppHasLightDark = Enemy.Graveyard.Any(c => c != null && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)));
                return oppHasLightDark;
            }
            return false;
        }

        // ============================================================
        // TIER 2 Dracotail Traps
        // ============================================================
        private bool DracotailStingEffect()
        {
            if (_stingUsed) return false;
            // Target 1 monster and/or 1 S/T in opp GY; banish them, recycle Dracotail, draw 1
            bool oppGYTarget = Enemy.Graveyard.Any(c => c != null && (c.IsMonster() || c.IsSpell() || c.IsTrap()));
            if (!oppGYTarget) return false;

            if (Duel.Player == 1 || Bot.Hand.Count < 5 || Duel.LastChainPlayer == 1)
            {
                _stingUsed = true;
                return true;
            }
            return false;
        }

        private bool DracotailHornEffect()
        {
            if (_hornUsed) return false;
            // STRICT RULE: ONLY activate Horn if opponent controls an Attack position monster that is TARGETABLE!
            bool oppHasTargetableAtk = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsAttack() && !c.IsShouldNotBeTarget());
            if (!oppHasTargetableAtk) return false;

            if (Duel.Player == 1 || Duel.Phase == DuelPhase.BattleStart)
            {
                _hornUsed = true;
                return true;
            }
            return false;
        }

        private bool DracotailFlameEffect()
        {
            if (_flameUsed) return false;
            // Target 1 face-up Spell on field; negate effects, recycle Dracotail, draw 1
            bool oppHasFaceupSpell = Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsSpell() && !c.IsShouldNotBeTarget());
            if (oppHasFaceupSpell || (Duel.LastChainPlayer == 1 && LastChainCard != null && LastChainCard.IsSpell() && LastChainCard.Controller == 1))
            {
                _flameUsed = true;
                return true;
            }
            return false;
        }

        // ============================================================
        // TIER 3 Fusion Boss Activations
        // ============================================================
        private bool ArthalionEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // GY trigger: Revive self when 2+ monsters sent to GY
                return true;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_arthalionUsed) return false;
                // On summon bounce: return opponent threats or recycle friendly GY monsters to hand
                bool hasOppTarget = Enemy.GetFieldCount() > 0 || Enemy.Graveyard.Count > 0;
                if (hasOppTarget)
                {
                    _arthalionUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool GulamelQuickEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // GY trigger: Revive self when 2+ monsters sent to GY
                return true;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_gulamelQuickUsed) return false;
                // Quick effect: When Dracotail card/effect is activated -> pop 1 opp card
                if (Enemy.GetFieldCount() > 0)
                {
                    _gulamelQuickUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool ShaulasEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // GY trigger: Revive self when 2+ monsters sent to GY
                return true;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_shaulasUsed) return false;
                int dracoGY = Bot.Graveyard.Count(c => c != null && c.HasSetcode(0x1c0));
                if (dracoGY >= 2 && Enemy.GetFieldCount() > 0)
                {
                    _shaulasUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool ZoroaVerethragnaEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // Quick effect: Negate monster effect + destroy 1 opp card
            if (Duel.LastChainPlayer == 1) return true;
            // Ignition: Equip 1 effect monster from GY/opp field
            return Bot.SpellZone.Count(c => c == null) > 0;
        }

        private bool AiwassEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // Quick effect: Equip to opp monster and TAKE CONTROL!
            var oppTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
            return oppTarget != null;
        }

        private bool SecreterionEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_secreterionUsed) return false;
            // Target 1 Dragon + 1 Spellcaster in GY -> Special summon 1
            bool hasDragon = Bot.Graveyard.Any(c => c != null && c.HasRace(CardRace.Dragon));
            bool hasSpellcaster = Bot.Graveyard.Any(c => c != null && c.HasRace(CardRace.SpellCaster));
            if (hasDragon && hasSpellcaster && !IsSpecialSummonBlocked())
            {
                _secreterionUsed = true;
                return true;
            }
            return false;
        }

        private bool EcclesiaAndDarkDragonEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Quick effect: banish self to SS Fallen of Albaz / Ecclesia from Deck/GY
                return Duel.Player == 1 || Duel.Phase == DuelPhase.BattleStart;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // In GY: target Level 8 Fusion in GY/banish + 1 card on field -> shuffle both + this card into deck
                bool hasLv8Fusion = Bot.Graveyard.Any(c => c != null && c.HasType(CardType.Fusion) && c.Level == 8)
                    || Bot.Banished.Any(c => c != null && c.HasType(CardType.Fusion) && c.Level == 8);
                bool hasFieldTarget = Enemy.GetFieldCount() > 0;
                return hasLv8Fusion && hasFieldTarget;
            }
            return false;
        }

        // ============================================================
        // TIER 4 Quick Fusions (Faimena, Mululu, Cartesia)
        // ============================================================
        private bool FaimenaQuickEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_faimenaQuickUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (!Duel.IsMainPhase()) return false;

            // Faimena sends itself to GY as cost, so we need at least 2 other non-Ace materials in hand/field!
            int availableNonAce = Bot.Hand.Count(c => c != null && c != Card && c.IsMonster())
                + Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            if (availableNonAce < 2) return false;

            bool hasDracotailMat = Bot.Hand.Any(c => c != null && c != Card && c.HasSetcode(0x1c0))
                || Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasSetcode(0x1c0) && !IsAceCard(c));
            if (!hasDracotailMat) return false;

            // On Opponent's turn: ONLY fuse if opponent has cards on field/GY to disrupt!
            if (Duel.Player == 1)
            {
                if (Enemy.GetFieldCount() == 0 && Enemy.Graveyard.Count == 0) return false;
            }

            _faimenaQuickUsed = true;
            return true;
        }

        private bool MululuQuickEffect()
        {
            if (_mululuQuickUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (!Duel.IsMainPhase()) return false;

            bool hasMaterial = Bot.Hand.Any(c => c != null && c != Card && c.IsMonster())
                || Bot.GetMonsters().Any(c => c != null && c != Card && c.IsFaceup() && !IsAceCard(c));

            if (hasMaterial)
            {
                _mululuQuickUsed = true;
                _fusionLockActive = true;
                return true;
            }
            return false;
        }

        private bool CartesiaEffect()
        {
            if (_cartesiaUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (!Duel.IsMainPhase()) return false;

            bool hasMaterial = Bot.Hand.Any(c => c != null && c != Card && c.IsMonster())
                || Bot.GetMonsters().Any(c => c != null && c != Card && c.IsFaceup() && !IsAceCard(c));

            if (hasMaterial)
            {
                _cartesiaUsed = true;
                return true;
            }
            return false;
        }

        // ============================================================
        // TIER 5 GY Triggers (Set S/T + Pop/Negate)
        // ============================================================
        private bool LukiasMatEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_lukiasMatUsed) return false;
            _lukiasMatUsed = true;
            return true;
        }

        private bool PhryxulMatEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            return true;
        }

        private bool MululuMatEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_mululuMatUsed) return false;
            _mululuMatUsed = true;
            return true;
        }

        private bool UrgulaMatEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_urgulaMatUsed) return false;
            // STRICT RULE: Urgula destroys 1 S/T on field; do NOT activate if opponent has no S/T (would destroy friendly cards)
            if (Enemy.GetSpellCount() == 0) return false;
            _urgulaMatUsed = true;
            return true;
        }

        private bool UrgulaGYEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_urgulaGYUsed) return false;
            // Target 1 Spellcaster Dracotail in GY -> add to hand, place Urgula on bottom of deck
            bool hasSpellcaster = Bot.Graveyard.Any(c => c != null && c != Card && c.HasSetcode(0x1c0) && c.HasRace(CardRace.SpellCaster));
            if (hasSpellcaster)
            {
                _urgulaGYUsed = true;
                return true;
            }
            return false;
        }

        private bool PanMatEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_panMatUsed) return false;
            // STRICT RULE: Pan destroys 1 Monster on field; do NOT activate if opponent has no monsters (would destroy friendly Boss)
            if (Enemy.GetMonsterCount() == 0) return false;
            _panMatUsed = true;
            return true;
        }

        private bool PanGYEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_panGYUsed) return false;
            _panGYUsed = true;
            return true;
        }

        private bool FaimenaGYEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_faimenaGYUsed) return false;
            _faimenaGYUsed = true;
            return true;
        }

        // ============================================================
        // TIER 6 Spells & Starters (Rahu, Ketu, Branded, Spenta, White Dragon)
        // ============================================================
        private bool PotOfProsperityEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_potProsperityUsed) return false;
            if (Bot.ExtraDeck.Count < 6 || Bot.Deck.Count < 6) return false;
            _potProsperityUsed = true;
            return true;
        }

        private bool RahuEffect()
        {
            if (_rahuUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            // Rahu fuses directly from DECK!
            bool hasEDBoss = GetRemainingCount(CardId.DracotailArthalion) > 0
                || GetRemainingCount(CardId.DracotailGulamel) > 0
                || GetRemainingCount(CardId.DracotailShaulas) > 0;

            if (hasEDBoss)
            {
                _rahuUsed = true;
                _fusionLockActive = true;
                return true;
            }
            return false;
        }

        private bool BrandedFusionEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_brandedFusionUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            bool hasTarget = GetRemainingCount(CardId.AlbionTheBrandedDragon) > 0
                || GetRemainingCount(CardId.AlbaLenatusTheAbyssDragon) > 0
                || GetRemainingCount(CardId.SecreterionDragon) > 0;

            if (hasTarget)
            {
                _brandedFusionUsed = true;
                return true;
            }
            return false;
        }

        private bool KetuEffect()
        {
            if (_ketuUsed) return false;
            _ketuUsed = true;
            return true;
        }

        private bool TheFallenAndTheVirtuousEffect()
        {
            if (_fallenUsed) return false;

            // STRICT RULE: ONLY activate if opponent controls a TARGETABLE face-up card!
            bool oppHasTargetable = Enemy.GetMonsters().Concat(Enemy.GetSpells()).Any(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
            if (!oppHasTargetable) return false;

            _fallenUsed = true;
            return true;
        }

        private bool SpentaEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_spentaDiscardUsed) return false;
                // Discard to search Crowley or Zoroa
                _spentaDiscardUsed = true;
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                if (_spentaGYUsed) return false;
                // Banish to equip Magistus from Extra Deck
                bool hasFaceupMonster = Bot.GetMonsters().Any(c => c != null && c.IsFaceup());
                if (hasFaceupMonster && Bot.SpellZone.Count(c => c == null) > 0)
                {
                    _spentaGYUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool FallenWhiteDragonEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_fallenWhiteDragonSSUsed) return false;
                if (IsSpecialSummonBlocked()) return false;
                // Send Albion from Extra Deck to SS this card!
                bool hasAlbionInED = Bot.ExtraDeck.Any(c => c != null && c.Id == CardId.AlbionTheBrandedDragon);
                if (hasAlbionInED)
                {
                    _fallenWhiteDragonSSUsed = true;
                    return true;
                }
            }
            return false;
        }

        // ============================================================
        // TIER 7 On-Summon Triggers & Ignitions
        // ============================================================
        private bool LukiasSearchEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_lukiasSearchUsed) return false;
            _lukiasSearchUsed = true;
            return true;
        }

        private bool PhryxulSummonEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_phryxulSummonUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            bool hasGYDracotail = Bot.Graveyard.Any(c => c != null && c.HasSetcode(0x1c0) && c.IsMonster());
            if (hasGYDracotail)
            {
                _phryxulSummonUsed = true;
                return true;
            }
            return false;
        }

        private bool CrowleyEffect()
        {
            if (_crowleyFuseUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            // On summon: Fusion Summon Magistus/Invoked Fusion using hand/field
            bool hasMaterial = Bot.Hand.Any(c => c != null && c != Card && c.IsMonster())
                || Bot.GetMonsters().Any(c => c != null && c != Card && c.IsFaceup() && !IsAceCard(c));

            if (hasMaterial)
            {
                _crowleyFuseUsed = true;
                return true;
            }
            return false;
        }

        private bool ZoroaFlameEffect()
        {
            if (_zoroaEquipUsed) return false;
            if (Bot.SpellZone.Count(c => c == null) == 0) return false;
            _zoroaEquipUsed = true;
            return true;
        }

        private bool EcclesiaTributeEffect()
        {
            // Tribute Incredible Ecclesia to SS Fallen of Albaz from Deck
            if (Card.Location != CardLocation.MonsterZone) return false;
            return !IsSpecialSummonBlocked();
        }

        private bool AlbionGYEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_albionGYUsed) return false;
            if (Duel.Phase != DuelPhase.End) return false;
            _albionGYUsed = true;
            return true;
        }

        // ============================================================
        // TIER 8 Normal Summon Conditions
        // ============================================================
        private bool ShouldSummonLukias() => true;

        private bool ShouldSummonPhryxul()
        {
            return Bot.Graveyard.Any(c => c != null && c.HasSetcode(0x1c0) && c.IsMonster() && c.IsCanRevive());
        }

        private bool ShouldSummonMululu() => true;

        private bool ShouldSummonFaimena()
        {
            return _faimenaQuickUsed || !Bot.Hand.Any(c => c != null && c.Id == CardId.DracotailLukias);
        }

        private bool ShouldSummonBranded()
        {
            return Bot.HasInHand(CardId.BrandedFusion) || Bot.HasInHand(CardId.TheFallenAndTheVirtuous) || Enemy.GetMonsterCount() > 0;
        }

        private bool ShouldSummonMagistus() => true;

        // ============================================================
        // TIER 9 Extra Deck Contact Fusion & Link Safety
        // ============================================================
        private bool ChorozoSummon()
        {
            // STRICT RULE: NEVER use Ace monsters as material for Chorozo!
            if (IsSpecialSummonBlocked()) return false;
            var monsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
            bool hasExtraType = monsters.Any(c => c.IsExtraCard() || c.HasType(CardType.Fusion | CardType.Synchro | CardType.Xyz | CardType.Link));
            bool hasSpellcaster = monsters.Any(c => c.HasRace(CardRace.SpellCaster));
            return hasExtraType && hasSpellcaster;
        }

        private bool InvokedOmegaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return !Bot.GetMonsters().Any(c => c != null && IsAceCard(c));
        }

        private bool ZoroaVerethragnaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return !Bot.GetMonsters().Any(c => c != null && IsAceCard(c));
        }

        private bool ArtemisSummon()
        {
            if (IsSpecialSummonBlocked() || _fusionLockActive) return false;

            // STRICT RULE: ONLY summon Artemis using Magistus monsters (Spoon/Spenta, Crowley, Zoroa)
            bool hasMagistusMat = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() &&
                (c.Id == CardId.SpoonTheSealOfMagistus ||
                 c.Id == CardId.CrowleyTheGiftedOfMagistus ||
                 c.Id == CardId.ZoroaTheMagistusOfFlame));

            return hasMagistusMat;
        }

        // ============================================================
        // OnSelectCard: Master Hint Handling, Safe Targeting & Material Optimization
        // ============================================================
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // ============================================================
            // 1. Pot of Prosperity Banish Cost Selection (From Extra Deck)
            // ============================================================
            if (LastChainCard != null && LastChainCard.Id == CardId.PotOfProsperity && (hint == 503 || hint == 504 || hint == 511))
            {
                var extraCards = cards.Where(c => c.Location == CardLocation.Extra).ToList();
                if (extraCards.Count >= min)
                {
                    var banishOrder = extraCards.OrderBy(c =>
                    {
                        if (c.Id == CardId.ArtemisMagistusMoonMaiden) return 1;
                        if (c.Id == CardId.AiwassMagistusSpellSpirit) return 2;
                        if (c.Id == CardId.ZoroaConflagrantCalamity) return 3;
                        if (c.Id == CardId.InvokedMagistusOmega) return 4;
                        if (c.Id == CardId.MagistusChorozo) return 5;
                        if (c.Id == CardId.KhaosStarsourceDragon) return 6;
                        if (c.Id == CardId.ZoroaVerethragna) return 7;
                        if (c.Id == CardId.SecreterionDragon) return 8;
                        if (c.Id == CardId.DracotailShaulas) return 9;
                        if (c.Id == CardId.AlbionTheBrandedDragon) return 10;
                        if (c.Id == CardId.AlbaLenatusTheAbyssDragon) return 11;
                        if (c.Id == CardId.DracotailGulamel) return 12;
                        if (c.Id == CardId.DracotailArthalion) return 13;
                        return 20;
                    }).ToList();

                    return banishOrder.Take(max).ToList();
                }
            }

            // ============================================================
            // 2. Search / Add Targets (Hint 503 / 506 / 510)
            // ============================================================
            if (hint == 503 || hint == 506 || hint == 510)
            {
                if (LastChainCard != null)
                {
                    // Ketu Dracotail -> search Lukias > Faimena > Rahu > Mululu
                    if (LastChainCard.Id == CardId.KetuDracotail || LastChainCard.Id == CardId.DracotailLukias)
                    {
                        var searchOrder = cards.OrderBy(c =>
                        {
                            if (c.Id == CardId.RahuDracotail && !Bot.HasInHand(CardId.RahuDracotail)) return 1;
                            if (c.Id == CardId.DracotailLukias && !Bot.HasInHand(CardId.DracotailLukias)) return 2;
                            if (c.Id == CardId.DracotailFaimena) return 3;
                            if (c.Id == CardId.DracotailUrgula) return 4;
                            if (c.Id == CardId.DracotailPan) return 5;
                            if (c.Id == CardId.DracotailMululu) return 6;
                            if (c.Id == CardId.DracotailPhryxul) return 7;
                            if (c.IsTrap()) return 8;
                            return 20;
                        }).ToList();
                        return searchOrder.Take(max).ToList();
                    }

                    // Urgula / Pan / Mululu Set Traps from Deck
                    if (LastChainCard.Id == CardId.DracotailUrgula || LastChainCard.Id == CardId.DracotailPan || LastChainCard.Id == CardId.DracotailMululu)
                    {
                        var trapOrder = cards.OrderBy(c =>
                        {
                            if (c.Id == CardId.RahuDracotail && !Bot.HasInHand(CardId.RahuDracotail) && !Bot.HasInSpellZone(CardId.RahuDracotail)) return 1;
                            if (c.Id == CardId.DracotailHorn && !Bot.HasInSpellZone(CardId.DracotailHorn)) return 2;
                            if (c.Id == CardId.DracotailSting && !Bot.HasInSpellZone(CardId.DracotailSting)) return 3;
                            if (c.Id == CardId.DracotailFlame && !Bot.HasInSpellZone(CardId.DracotailFlame)) return 4;
                            return 10;
                        }).ToList();
                        return trapOrder.Take(max).ToList();
                    }

                    // Spenta / Artemis search Magistus
                    if (LastChainCard.Id == CardId.SpoonTheSealOfMagistus || LastChainCard.Id == CardId.ArtemisMagistusMoonMaiden)
                    {
                        var magOrder = cards.OrderBy(c =>
                        {
                            if (c.Id == CardId.CrowleyTheGiftedOfMagistus && !Bot.HasInHand(CardId.CrowleyTheGiftedOfMagistus)) return 1;
                            if (c.Id == CardId.ZoroaTheMagistusOfFlame && !Bot.HasInHand(CardId.ZoroaTheMagistusOfFlame)) return 2;
                            return 10;
                        }).ToList();
                        return magOrder.Take(max).ToList();
                    }

                    // Albion GY search
                    if (LastChainCard.Id == CardId.AlbionTheBrandedDragon)
                    {
                        var albionOrder = cards.OrderBy(c =>
                        {
                            if (c.Id == CardId.BrandedFusion && !Bot.HasInHand(CardId.BrandedFusion)) return 1;
                            if (c.Id == CardId.TheFallenAndTheVirtuous && !Bot.HasInHand(CardId.TheFallenAndTheVirtuous)) return 2;
                            return 10;
                        }).ToList();
                        return albionOrder.Take(max).ToList();
                    }
                }
            }

            // ============================================================
            // 3. Rahu Dracotail Fusion Materials (Fuses from Deck / Hand / Field)
            // ============================================================
            if (LastChainCard != null && LastChainCard.Id == CardId.RahuDracotail && (hint == 501 || hint == 505 || hint == 511))
            {
                // Priority 1: Deck Materials (Urgula + Pan trigger BOTH GY set+pops simultaneously!)
                var deckMaterials = cards.Where(c => c.Location == CardLocation.Deck).OrderBy(c =>
                {
                    if (c.Id == CardId.DracotailUrgula) return 1;
                    if (c.Id == CardId.DracotailPan) return 2;
                    if (c.Id == CardId.DracotailLukias) return 3;
                    if (c.Id == CardId.DracotailFaimena) return 4;
                    if (c.Id == CardId.DracotailMululu) return 5;
                    return 10;
                }).ToList();

                if (deckMaterials.Count >= min)
                    return deckMaterials.Take(max).ToList();

                // Priority 2: Hand Materials
                var handMaterials = cards.Where(c => c.Location == CardLocation.Hand && !IsAceCard(c)).ToList();
                if (handMaterials.Count >= min)
                    return handMaterials.Take(max).ToList();
            }

            // ============================================================
            // 4. Arthalion On-Summon Target Selection (Field & GYs)
            // ============================================================
            if (LastChainCard != null && LastChainCard.Id == CardId.DracotailArthalion && (hint == 505 || hint == 502))
            {
                var targets = new List<ClientCard>();

                var oppFieldThreats = cards.Where(c => c.Controller == 1 && (c.Location == CardLocation.MonsterZone || c.Location == CardLocation.SpellZone))
                    .OrderByDescending(c => GetThreatScore(c)).ToList();
                targets.AddRange(oppFieldThreats);

                var oppGYCards = cards.Where(c => c.Controller == 1 && c.Location == CardLocation.Grave)
                    .OrderByDescending(c => (c.Id == 30012506 || c.Id == 77411244 || c.Id == 3405259) ? 100 : 10).ToList();
                targets.AddRange(oppGYCards);

                var friendlyGYRecycle = cards.Where(c => c.Controller == 0 && c.Location == CardLocation.Grave && c.HasSetcode(0x1c0))
                    .OrderBy(c => c.Id == CardId.DracotailLukias ? 1 : 2).ToList();
                targets.AddRange(friendlyGYRecycle);

                if (targets.Count >= min)
                {
                    return targets.Take(max).ToList();
                }
            }

            // ============================================================
            // 5. Opponent Removal / Negation / Bouncing (Pan, Urgula, Sting, Horn, Flame, The Fallen)
            // ============================================================
            if (hint == 502 || hint == 505 || hint == 511 || hint == 501 || hint == 504 || hint == 575)
            {
                var oppTargets = cards.Where(c => c.Controller == 1).OrderByDescending(c => GetThreatScore(c)).ToList();
                if (oppTargets.Count >= min)
                {
                    return oppTargets.Take(max).ToList();
                }
                if (cancelable) return null;
                // If forced, NEVER select our own Ace cards!
                var safeTargets = cards.Where(c => !IsAceCard(c)).OrderBy(c => c.Attack).ToList();
                if (safeTargets.Count >= min)
                    return safeTargets.Take(max).ToList();
            }

            // ============================================================
            // 6. Special Summon Targets (Hint 509)
            // ============================================================
            if (hint == 509)
            {
                var summonOrder = cards.OrderByDescending(c =>
                {
                    if (c.Id == CardId.DracotailArthalion) return 100;
                    if (c.Id == CardId.DracotailGulamel) return 95;
                    if (c.Id == CardId.AlbaLenatusTheAbyssDragon) return 90;
                    if (c.Id == CardId.AlbionTheBrandedDragon) return 85;
                    if (c.Id == CardId.SecreterionDragon) return 80;
                    if (c.Id == CardId.DracotailLukias) return 70;
                    if (c.Id == CardId.DracotailFaimena) return 60;
                    return 10;
                }).ToList();

                return summonOrder.Take(max).ToList();
            }

            // ============================================================
            // 7. Material Selection Protection (Hint 533 / 511 / Link / Fusion)
            // ============================================================
            if (hint == 533 || hint == 511)
            {
                var safeMaterials = cards.Where(c => !(c.Controller == 0 && c.Location == CardLocation.MonsterZone && IsAceCard(c)))
                    .OrderBy(c => GetMaterialPriority(c)).ToList();

                if (safeMaterials.Count >= min)
                {
                    return safeMaterials.Take(max).ToList();
                }
            }

            // ============================================================
            // 8. Generic Safe Discard / Cost Protection
            // ============================================================
            var safe = cards.Where(c => c.Controller == 0 && !IsAceCard(c) && !HandTraps.Contains(c.Id)).ToList();
            if (safe.Count >= min)
            {
                var sortedSafe = safe.OrderBy(c =>
                {
                    if (c.Location == CardLocation.Hand) return 1;
                    if (c.Location == CardLocation.Grave) return 2;
                    return 10;
                }).ToList();
                return sortedSafe.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override bool OnSelectYesNo(long desc)
        {
            // Pan: "Destroy 1 monster on the field?"
            if (desc == Util.GetStringId(CardId.DracotailPan, 2))
            {
                return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
            }

            // Urgula: "Destroy 1 Spell/Trap on the field?"
            if (desc == Util.GetStringId(CardId.DracotailUrgula, 2))
            {
                return Enemy.GetSpells().Any(c => c != null);
            }

            return true;
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            if (card == null) return null;
            if (card.IsCode(CardId.DracotailArthalion, CardId.DracotailGulamel, CardId.DracotailShaulas,
                            CardId.DracotailUrgula, CardId.DracotailPan, CardId.DracotailMululu,
                            CardId.DracotailLukias, CardId.DracotailFaimena, CardId.AlbionTheBrandedDragon,
                            CardId.AlbaLenatusTheAbyssDragon, CardId.DracotailSting, CardId.DracotailHorn, CardId.DracotailFlame))
                return true;
            return base.OnSelectEffectYn(card, desc);
        }

        public override IList<ClientCard> OnSelectFusionMaterial(IList<ClientCard> cards, int min, int max)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectFusionMaterial(cards, min, max);
            var protectedCards = cards.Where(c => !(c.Location == CardLocation.MonsterZone && IsAceCard(c))).ToList();
            if (protectedCards.Count >= min)
            {
                var sorted = protectedCards.OrderBy(c => GetMaterialSacrificePriority(c)).ToList();
                return sorted.Take(min).ToList();
            }
            return base.OnSelectFusionMaterial(cards, min, max);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (HandTraps.Contains(cardId) || cardId == CardId.SpoonTheSealOfMagistus || cardId == CardId.CrowleyTheGiftedOfMagistus)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }
            if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            return base.OnSelectPosition(cardId, positions);
        }
    }

    [Deck("Expert_2026_Dracotail", "2026_Dracotail")]
    public class ExpertDracotailExecutor : _2026_DracotailExecutor
    {
        public ExpertDracotailExecutor(GameAI ai, Duel duel) : base(ai, duel) { }
    }
}
