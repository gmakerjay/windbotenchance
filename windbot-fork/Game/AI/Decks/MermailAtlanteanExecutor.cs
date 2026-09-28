using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Decks;
using WindBot.Game.AI.DecisionEngine;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("MermailAtlantean", "MermailAtlantean")]
    public class MermailAtlanteanExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Deck Monsters
            public const int NeptabyssTheAtlanteanPrince = 21565445;
            public const int AtlanteanDragoons = 74311226;
            public const int AtlanteanHeavyInfantry = 37104630;
            public const int AbyssrhineTheAtlanteanSpirit = 17080584;
            public const int MermailAbyssteus = 22446869;
            public const int MermailShadowSquad = 53085623;
            public const int MermailAbysspike = 58471134;
            public const int MermailAbyssocea = 28577986;
            public const int PoseidraTheStormingAtlantean = 99193444;
            public const int MoulinglaciaTheElementalLord = 13959634;
            public const int SuperancientDeepseaKingCoelacanth = 88307361;
            public const int DeepSeaMinstrel = 71978434;
            public const int FirstPenguin = 12975671;
            public const int GluttonousReptolphinGreethys = 8576764;
            public const int AshBlossom = 14558127;
            public const int AshBlossomAlt = 14558128;
            public const int DDCrow = 24508238;
            public const int DrollAndLockBird = 94145021;

            // Spells
            public const int OneForOne = 2295440;
            public const int CalledByTheGrave = 24224830;
            public const int CalledByTheGraveAlt = 24224831;
            public const int CrossoutDesignator = 65681983;
            public const int ForbiddenCrown = 98829635;

            // Traps
            public const int InfiniteImpermanence = 10045474;
            public const int DominusImpulse = 40366667;

            // Extra Deck
            public const int PoseidraAbyssTheAtlanteanDragonLord = 60517697;
            public const int MermailAbyssgaios = 74371660;
            public const int AbysstriteTheAtlanteanSpirit = 9453320;
            public const int LeVirtueDragon = 33113958;
            public const int MermailKingNeptabyss = 69385019;
            public const int MarincessCoralAnemone = 79130389;
            public const int MistarBoy = 65170459;
            public const int HaggardLizardose = 9763474;
            public const int IcejadeGymirAegirine = 86682165;
            public const int SwordsoulSupremeSovereignChengying = 96633955;
            public const int TrishulaDragonOfTheIceBarrier = 52687916;
            public const int AdamancipatorRisenDragite = 9464441;
            public const int WhiteAuraMonoceros = 63731062;
            public const int DeepSeaPrimaDonna = 50793215;
            public const int DeepSeaRepetiteur = 33467872;
        }

        private static readonly int[] BossMonsters = {
            CardId.PoseidraAbyssTheAtlanteanDragonLord,
            CardId.MermailAbyssgaios,
            CardId.AdamancipatorRisenDragite,
            CardId.IcejadeGymirAegirine,
            CardId.SwordsoulSupremeSovereignChengying,
            CardId.TrishulaDragonOfTheIceBarrier,
            CardId.MoulinglaciaTheElementalLord,
            CardId.AbysstriteTheAtlanteanSpirit,
            CardId.MermailKingNeptabyss
        };

        internal MermailAtlanteanPlugin Plugin { get; private set; }

        private bool _neptabyssUsedThisTurn = false;
        private bool _abyssteusUsedThisTurn = false;
        private bool _abyssrhineUsedThisTurn = false;
        private bool _moulinglaciaSummonedThisTurn = false;
        private bool _poseidraAbyssBounceUsed = false;

        public MermailAtlanteanExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            Plugin = new MermailAtlanteanPlugin(this);
            DeckPlugin = Plugin;

            RegisterHelperModules();
            RegisterExecutors();
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _neptabyssUsedThisTurn = false;
            _abyssteusUsedThisTurn = false;
            _abyssrhineUsedThisTurn = false;
            _moulinglaciaSummonedThisTurn = false;
            _poseidraAbyssBounceUsed = false;
            Plugin?.ResetTurnState();
        }

        public override bool OnSelectHand()
        {
            // Mermail Atlantean sets up board control Going 1st
            return true;
        }

        private void RegisterHelperModules()
        {
            // 1. Ace Card Protection
            ResourcePlan.RegisterAceCards(BossMonsters);
            HeuristicGuard.RegisterAceCards(BossMonsters);

            // 2. Combo Starters & Bait
            BaitPlanner.RegisterComboStarters(
                CardId.NeptabyssTheAtlanteanPrince,
                CardId.AbyssrhineTheAtlanteanSpirit,
                CardId.OneForOne,
                CardId.MermailAbyssteus
            );
            BaitPlanner.RegisterBaitCards(
                CardId.CrossoutDesignator,
                CardId.CalledByTheGrave
            );

            // 3. Negators & High Value Targets
            ChainAdvisor.RegisterHighValueTargets(
                CardId.PoseidraAbyssTheAtlanteanDragonLord,
                CardId.MermailAbyssgaios,
                CardId.AdamancipatorRisenDragite,
                CardId.IcejadeGymirAegirine,
                CardId.SwordsoulSupremeSovereignChengying,
                CardId.DominusImpulse,
                CardId.InfiniteImpermanence,
                CardId.AshBlossom,
                CardId.CalledByTheGrave,
                CardId.CrossoutDesignator
            );
        }

        private void RegisterExecutors()
        {
            // ═══════════════════════════════════════════════════════════════
            // TIER 0: COUNTER & QUICK NEGATORS / HANDTRAPS
            // ═══════════════════════════════════════════════════════════════

            // Called by the Grave: Negate opponent Handtraps or GY Bosses
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);

            // Crossout Designator: Declare Ash / Imperm / Droll
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, DefaultCrossoutDesignator);

            // Dominus Impulse: Handtrap trap - Negates monster Special Summon effect + destroy!
            AddExecutor(ExecutorType.Activate, CardId.DominusImpulse, DominusImpulseActivate);

            // Infinite Impermanence: Handtrap or set trap negate
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);

            // Ash Blossom: High-priority search/draw denial
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAlt, DefaultAshBlossomAndJoyousSpring);

            // D.D. Crow: Banish key opponent GY card
            AddExecutor(ExecutorType.Activate, CardId.DDCrow, DDCrowActivate);

            // Droll & Lock Bird: Lock searches when opponent searches
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DefaultDrollAndLockBird);

            // Forbidden Crown: Quick-Play monster negate
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenCrown, ForbiddenCrownActivate);

            // Adamancipator Risen - Dragite: Quick Effect Spell/Trap Negate
            AddExecutor(ExecutorType.Activate, CardId.AdamancipatorRisenDragite, DragiteActivate);

            // Icejade Gymir Aegirine: Quick Effect Board Protection & Banish
            AddExecutor(ExecutorType.Activate, CardId.IcejadeGymirAegirine, GymirAegirineActivate);

            // Mermail Abyssgaios: Quick Effect Negate monsters with ATK < Abyssgaios (2800)
            AddExecutor(ExecutorType.Activate, CardId.MermailAbyssgaios, AbyssgaiosActivate);

            // Poseidra Abyss, the Atlantean Dragon Lord: Bounce up to 3 cards opponent controls!
            AddExecutor(ExecutorType.Activate, CardId.PoseidraAbyssTheAtlanteanDragonLord, PoseidraAbyssActivate);

            // Atlantean Heavy Infantry: Destroy 1 face-up card when sent to GY for WATER effect
            AddExecutor(ExecutorType.Activate, CardId.AtlanteanHeavyInfantry, HeavyInfantryActivate);

            // Atlantean Dragoons: Search Sea Serpent when sent to GY for WATER effect
            AddExecutor(ExecutorType.Activate, CardId.AtlanteanDragoons, DragoonsActivate);

            // Mermail Shadow Squad: Special Summon from Deck when sent to GY for WATER effect
            AddExecutor(ExecutorType.Activate, CardId.MermailShadowSquad, ShadowSquadActivate);

            // ═══════════════════════════════════════════════════════════════
            // TIER 1: COMBO STARTERS & EXTENDERS
            // ═══════════════════════════════════════════════════════════════

            // One for One: Send monster to SS Neptabyss
            AddExecutor(ExecutorType.Activate, CardId.OneForOne, OneForOneActivate);

            // Neptabyss the Atlantean Prince: Send Dragoons as cost -> Search Atlantean
            AddExecutor(ExecutorType.Summon, CardId.NeptabyssTheAtlanteanPrince, NeptabyssSummon);
            AddExecutor(ExecutorType.Activate, CardId.NeptabyssTheAtlanteanPrince, NeptabyssActivate);

            // Abyssrhine, the Atlantean Spirit: Tribute itself and another WATER -> Add/SS Level 7 from Deck
            AddExecutor(ExecutorType.Activate, CardId.AbyssrhineTheAtlanteanSpirit, AbyssrhineActivate);

            // Mermail Abyssteus: Discard WATER -> SS from hand -> Search Level 4 or lower Mermail
            AddExecutor(ExecutorType.Activate, CardId.MermailAbyssteus, AbyssteusActivate);

            // Mermail Abysspike: NS or SS -> Discard WATER -> Search Level 3 WATER
            AddExecutor(ExecutorType.Summon, CardId.MermailAbysspike, AbysspikeSummon);
            AddExecutor(ExecutorType.Activate, CardId.MermailAbysspike, AbysspikeActivate);

            // Mermail Abyssocea: Target Mermail -> SS Mermails whose total Levels equal target
            AddExecutor(ExecutorType.Activate, CardId.MermailAbyssocea, AbyssoceaActivate);

            // Moulinglacia the Elemental Lord: SS from hand when exactly 5 WATER in GY -> Hand rip 2!
            AddExecutor(ExecutorType.SpSummon, CardId.MoulinglaciaTheElementalLord, MoulinglaciaSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.MoulinglaciaTheElementalLord, MoulinglaciaActivate);

            // First Penguin: SS from hand
            AddExecutor(ExecutorType.SpSummon, CardId.FirstPenguin, FirstPenguinSpSummon);

            // Gluttonous Reptolphin Greethys: SS from GY
            AddExecutor(ExecutorType.Activate, CardId.GluttonousReptolphinGreethys, ReptolphinActivate);

            // Poseidra, the Storming Atlantean: Summon from GY or hand
            AddExecutor(ExecutorType.Activate, CardId.PoseidraTheStormingAtlantean, PoseidraActivate);

            // Superancient Deepsea King Coelacanth: Discard 1 -> SS Fish from Deck
            AddExecutor(ExecutorType.Activate, CardId.SuperancientDeepseaKingCoelacanth, CoelacanthActivate);

            // ═══════════════════════════════════════════════════════════════
            // TIER 2: EXTRA DECK SUMMONS (XYZ, SYNCHRO, LINK)
            // ═══════════════════════════════════════════════════════════════

            // Poseidra Abyss, the Atlantean Dragon Lord (Rank 7)
            AddExecutor(ExecutorType.SpSummon, CardId.PoseidraAbyssTheAtlanteanDragonLord, XyzPoseidraAbyss);

            // Mermail Abyssgaios (Rank 7)
            AddExecutor(ExecutorType.SpSummon, CardId.MermailAbyssgaios, XyzAbyssgaios);

            // Abysstrite, the Atlantean Spirit (Rank 7)
            AddExecutor(ExecutorType.SpSummon, CardId.AbysstriteTheAtlanteanSpirit, XyzAbysstrite);
            AddExecutor(ExecutorType.Activate, CardId.AbysstriteTheAtlanteanSpirit, AbysstriteActivate);

            // LeVirtue Dragon (Rank 3)
            AddExecutor(ExecutorType.SpSummon, CardId.LeVirtueDragon, XyzLeVirtue);
            AddExecutor(ExecutorType.Activate, CardId.LeVirtueDragon, LeVirtueActivate);

            // Icejade Gymir Aegirine (Lv 10 Synchro)
            AddExecutor(ExecutorType.SpSummon, CardId.IcejadeGymirAegirine, SynchroGymirAegirine);

            // Swordsoul Supreme Sovereign - Chengying (Lv 10 Synchro)
            AddExecutor(ExecutorType.SpSummon, CardId.SwordsoulSupremeSovereignChengying, SynchroChengying);

            // Trishula, Dragon of the Ice Barrier (Lv 9 Synchro)
            AddExecutor(ExecutorType.SpSummon, CardId.TrishulaDragonOfTheIceBarrier, SynchroTrishula);
            AddExecutor(ExecutorType.Activate, CardId.TrishulaDragonOfTheIceBarrier, TrishulaActivate);

            // Adamancipator Risen - Dragite (Lv 8 Synchro)
            AddExecutor(ExecutorType.SpSummon, CardId.AdamancipatorRisenDragite, SynchroDragite);

            // White Aura Monoceros (Lv 7 Synchro)
            AddExecutor(ExecutorType.SpSummon, CardId.WhiteAuraMonoceros, SynchroMonoceros);

            // Deep Sea Prima Donna (Lv 7 Synchro)
            AddExecutor(ExecutorType.SpSummon, CardId.DeepSeaPrimaDonna, SynchroPrimaDonna);
            AddExecutor(ExecutorType.Activate, CardId.DeepSeaPrimaDonna, PrimaDonnaActivate);

            // Deep Sea Repetiteur (Lv 5 Synchro)
            AddExecutor(ExecutorType.SpSummon, CardId.DeepSeaRepetiteur, SynchroRepetiteur);

            // Mermail King - Neptabyss (Link 3)
            AddExecutor(ExecutorType.SpSummon, CardId.MermailKingNeptabyss, LinkMermailKing);
            AddExecutor(ExecutorType.Activate, CardId.MermailKingNeptabyss, MermailKingActivate);

            // Marincess Coral Anemone (Link 2)
            AddExecutor(ExecutorType.SpSummon, CardId.MarincessCoralAnemone, LinkCoralAnemone);
            AddExecutor(ExecutorType.Activate, CardId.MarincessCoralAnemone, CoralAnemoneActivate);

            // Mistar Boy (Link 2)
            AddExecutor(ExecutorType.SpSummon, CardId.MistarBoy, LinkMistarBoy);

            // Haggard Lizardose (Link 2)
            AddExecutor(ExecutorType.SpSummon, CardId.HaggardLizardose, LinkLizardose);

            // ═══════════════════════════════════════════════════════════════
            // TIER 3: TRAP SETTING & POSITION CONTROL
            // ═══════════════════════════════════════════════════════════════

            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);
            AddExecutor(ExecutorType.SpellSet, CardId.DominusImpulse);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenCrown);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);
            AddExecutor(ExecutorType.SpellSet, CardId.CrossoutDesignator);

            AddExecutor(ExecutorType.Repos, SmartMonsterRepos);
        }

        // ═══════════════════════════════════════════════════════════════
        private bool DDCrowActivate()
        {
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard target = Enemy.Graveyard.OrderByDescending(c => c.Attack).FirstOrDefault(c => c.IsMonster());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool DominusImpulseActivate()
        {
            // Negate monster Special Summon effect
            return Duel.LastChainPlayer == 1;
        }

        private bool ForbiddenCrownActivate()
        {
            if (Card == null) return false;
            // Negate high threat face-up monster
            var threat = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled() && IsTargetable(c));
            return threat != null;
        }

        private bool DragiteActivate()
        {
            // Negate opponent Spell/Trap activation if WATER in GY
            if (Duel.LastChainPlayer == 1)
            {
                bool hasWaterInGy = Bot.Graveyard.Any(c => c != null && c.HasAttribute(CardAttribute.Water));
                return hasWaterInGy;
            }
            return false;
        }

        private bool GymirAegirineActivate()
        {
            // Quick effect: Activate in response to opponent card effect to grant board immunity & banish
            return Duel.LastChainPlayer == 1;
        }

        private bool AbyssgaiosActivate()
        {
            // Negate all monsters with less ATK than Abyssgaios (2800)
            if (Duel.LastChainPlayer == 1)
            {
                return true;
            }
            if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
            {
                return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.Attack < 2800);
            }
            return false;
        }

        private bool PoseidraAbyssActivate()
        {
            if (Card == null) return false;

            // Xyz Detach effect: send 1 WATER from hand/deck -> bounce up to 3 opponent cards!
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (!_poseidraAbyssBounceUsed && Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0)
                {
                    _poseidraAbyssBounceUsed = true;
                    return true;
                }
            }

            // GY trigger: discard 1 -> SS up to 3 Fish/Sea Serpent/Aqua Level 3 or lower
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.GetMonsterCount() < 5;
            }

            return false;
        }

        private bool HeavyInfantryActivate()
        {
            // Destroy 1 face-up card opponent controls
            return Enemy.GetMonsters().Concat(Enemy.GetSpells()).Any(c => c != null && c.IsFaceup() && IsTargetable(c));
        }

        private bool DragoonsActivate()
        {
            // Always search Sea Serpent when sent to GY for WATER effect
            return true;
        }

        private bool ShadowSquadActivate()
        {
            // Special summon Level 4 or lower Atlantean or Mermail from Deck
            return Bot.GetMonsterCount() < 5;
        }

        private bool OneForOneActivate()
        {
            // Pitch fodder to summon Neptabyss from Deck
            return Bot.GetMonsterCount() < 5 && Bot.Deck.Any(c => c != null && c.IsCode(CardId.NeptabyssTheAtlanteanPrince));
        }

        private bool NeptabyssSummon()
        {
            return Bot.GetMonsterCount() < 5 && !_neptabyssUsedThisTurn;
        }

        private bool NeptabyssActivate()
        {
            if (Card == null) return false;

            // Ignition: Send Atlantean from deck as cost -> Add Atlantean to hand
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (!_neptabyssUsedThisTurn)
                {
                    _neptabyssUsedThisTurn = true;
                    return true;
                }
            }

            // GY trigger when sent for WATER effect: Revive Atlantean from GY
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.GetMonsterCount() < 5;
            }

            return false;
        }

        private bool AbyssrhineActivate()
        {
            if (Card == null) return false;

            // Hand/Field effect: Tribute itself and another WATER -> Add/SS Level 7 from Deck
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.MonsterZone)
            {
                if (!_abyssrhineUsedThisTurn)
                {
                    bool hasTributeFodder = Bot.Hand.Concat(Bot.GetMonsters()).Any(c => c != null && c != Card && (IsAtlantean(c) || IsMermail(c)));
                    if (hasTributeFodder)
                    {
                        _abyssrhineUsedThisTurn = true;
                        return true;
                    }
                }
            }

            // GY effect in opponent turn: banish itself and discard 1 -> draw 1
            if (Card.Location == CardLocation.Grave && Duel.Player == 1)
            {
                return Bot.Hand.Count > 0;
            }

            return false;
        }

        private bool AbyssteusActivate()
        {
            if (Card == null) return false;

            // Hand effect: Discard 1 WATER -> Special Summon itself
            if (Card.Location == CardLocation.Hand)
            {
                bool hasWaterDiscard = Bot.Hand.Any(c => c != null && c != Card && c.HasAttribute(CardAttribute.Water));
                return hasWaterDiscard && Bot.GetMonsterCount() < 5 && !_abyssteusUsedThisTurn;
            }

            // On Special Summon: Search Level 4 or lower Mermail
            if (Card.Location == CardLocation.MonsterZone)
            {
                _abyssteusUsedThisTurn = true;
                return true;
            }

            return false;
        }

        private bool AbysspikeSummon()
        {
            return Bot.GetMonsterCount() < 5;
        }

        private bool AbysspikeActivate()
        {
            // Discard 1 WATER -> search Level 3 WATER
            return Bot.Hand.Any(c => c != null && c.HasAttribute(CardAttribute.Water));
        }

        private bool AbyssoceaActivate()
        {
            // Target Mermail -> SS Mermails from deck
            return Bot.GetMonsterCount() < 5;
        }

        private bool MoulinglaciaSpSummon()
        {
            // Exactly 5 WATER in GY
            int waterInGy = Bot.Graveyard.Count(c => c != null && c.HasAttribute(CardAttribute.Water));
            return waterInGy == 5 && Bot.GetMonsterCount() < 5 && !_moulinglaciaSummonedThisTurn;
        }

        private bool MoulinglaciaActivate()
        {
            _moulinglaciaSummonedThisTurn = true;
            return true;
        }

        private bool FirstPenguinSpSummon()
        {
            return Bot.GetMonsterCount() < 5;
        }

        private bool ReptolphinActivate()
        {
            return Bot.GetMonsterCount() < 5;
        }

        private bool PoseidraActivate()
        {
            return true;
        }

        private bool CoelacanthActivate()
        {
            // Discard 1 -> summon Fish
            return Bot.Hand.Count > 0 && Bot.GetMonsterCount() < 5;
        }

        private bool XyzPoseidraAbyss()
        {
            // Rank-up over Abysstrite or Abyssgaios or 3 Level 7s
            return true;
        }

        private bool XyzAbyssgaios()
        {
            return true;
        }

        private bool XyzAbysstrite()
        {
            return true;
        }

        private bool AbysstriteActivate()
        {
            // On Xyz: Revive Fish/Sea Serpent/Aqua
            return Bot.GetMonsterCount() < 5;
        }

        private bool XyzLeVirtue()
        {
            return true;
        }

        private bool LeVirtueActivate()
        {
            // Add Fish/Sea Serpent/Aqua from GY to hand
            return true;
        }

        private bool SynchroGymirAegirine()
        {
            return true;
        }

        private bool SynchroChengying()
        {
            return true;
        }

        private bool SynchroTrishula()
        {
            return _isGoingSecond || Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0;
        }

        private bool TrishulaActivate()
        {
            return true;
        }

        private bool SynchroDragite()
        {
            return true;
        }

        private bool SynchroMonoceros()
        {
            return true;
        }

        private bool SynchroPrimaDonna()
        {
            return true;
        }

        private bool PrimaDonnaActivate()
        {
            return true;
        }

        private bool SynchroRepetiteur()
        {
            return true;
        }

        private bool LinkMermailKing()
        {
            return Bot.GetMonsterCount() >= 3 && !HasAceOnBoard();
        }

        private bool MermailKingActivate()
        {
            return true;
        }

        private bool LinkCoralAnemone()
        {
            // Revive WATER with 1500 or less ATK (e.g. Neptabyss)
            return Bot.GetMonsterCount() >= 2 && !HasAceOnBoard();
        }

        private bool CoralAnemoneActivate()
        {
            return Bot.GetMonsterCount() < 5;
        }

        private bool LinkMistarBoy()
        {
            return Bot.GetMonsterCount() >= 2 && !HasAceOnBoard();
        }

        private bool LinkLizardose()
        {
            return Bot.GetMonsterCount() >= 2 && !HasAceOnBoard();
        }

        // ═══════════════════════════════════════════════════════════════
        // REPOSITIONING & UTILITIES
        // ═══════════════════════════════════════════════════════════════

        private bool SmartMonsterRepos()
        {
            if (Card == null) return false;

            // 0 ATK or Handtraps in Attack -> switch to Defense
            if (Card.IsAttack() && (Card.Attack == 0 || (Card.Defense > Card.Attack && Card.Defense >= 1800)))
                return true;

            // High ATK bosses in Defense -> switch to Attack to deal lethal
            if (Card.IsDefense() && Card.Attack > Card.Defense && Card.Attack >= 1800 && Duel.Turn > 1)
                return true;

            return DefaultMonsterRepos();
        }

        private bool IsAtlantean(ClientCard card)
        {
            if (card == null) return false;
            return card.IsCode(
                CardId.NeptabyssTheAtlanteanPrince,
                CardId.AtlanteanDragoons,
                CardId.AtlanteanHeavyInfantry,
                CardId.AbyssrhineTheAtlanteanSpirit,
                CardId.PoseidraTheStormingAtlantean,
                CardId.MermailShadowSquad
            );
        }

        private bool IsMermail(ClientCard card)
        {
            if (card == null) return false;
            return card.IsCode(
                CardId.MermailAbyssteus,
                CardId.MermailAbysspike,
                CardId.MermailAbyssocea,
                CardId.AbyssrhineTheAtlanteanSpirit,
                CardId.MermailShadowSquad,
                CardId.MermailKingNeptabyss,
                CardId.MermailAbyssgaios,
                CardId.AbysstriteTheAtlanteanSpirit
            );
        }

        private bool HasAceOnBoard()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && BossMonsters.Contains(c.Id));
        }

        private bool IsTargetable(ClientCard card)
        {
            if (card == null) return false;
            return !card.IsShouldNotBeTarget();
        }

        // ═══════════════════════════════════════════════════════════════
        // CALLBACK OVERRIDES
        // ═══════════════════════════════════════════════════════════════

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return 0;

            for (int i = 0; i < options.Count; i++)
            {
                long cardId = options[i] >> 4;
                if (cardId == 0 && Card != null) cardId = Card.Id;
                long optIndex = options[i] & 0xf;

                // Abyssrhine: Option 1: Special Summon, Option 0: Add to Hand
                if (cardId == CardId.AbyssrhineTheAtlanteanSpirit)
                {
                    if (optIndex == 1 && Bot.GetMonsterCount() < 5) return i;
                    if (optIndex == 0) return i;
                }

                // LeVirtue Dragon: Option 0: Add Fish/Sea Serpent/Aqua from GY to hand
                if (cardId == CardId.LeVirtueDragon)
                {
                    if (optIndex == 0) return i;
                }
            }

            return base.OnSelectOption(options);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (positions == null || positions.Count == 0) return CardPosition.FaceUpAttack;
            if (positions.Count == 1) return positions[0];

            var cardData = YGOSharp.OCGWrapper.NamedCard.Get(cardId);
            if (cardData != null)
            {
                if (cardData.HasType(CardType.Link))
                    return CardPosition.FaceUpAttack;

                // Handtraps (Ash, Crow, 0 ATK) -> Defense
                if (cardData.Attack == 0 || CardIntelligence.IsHandtrap(cardId))
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                    if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                }

                // High ATK -> Attack
                if (cardData.Attack >= 1800 && positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return new List<ClientCard>();

            // 1. Hint 506: ATOHAND (Search target)
            if (hint == 506)
            {
                var target = Plugin.Strategy.PickSearchTarget(cards, Card);
                if (target != null)
                {
                    return new List<ClientCard> { target };
                }
            }

            // 2. Hint 509: SPSUMMON (Special Summon target)
            if (hint == 509)
            {
                var target = Plugin.Strategy.PickSpecialSummonTarget(cards);
                if (target != null)
                {
                    return new List<ClientCard> { target };
                }
            }

            // 3. Hint 501 / 504: DISCARD / TOGRAVE
            if (hint == 501 || hint == 504)
            {
                // If Neptabyss is sending Atlantean as cost -> always send Dragoons!
                if (Card != null && Card.IsCode(CardId.NeptabyssTheAtlanteanPrince))
                {
                    var dragoons = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.AtlanteanDragoons));
                    if (dragoons != null)
                    {
                        return new List<ClientCard> { dragoons };
                    }
                }

                var discardTarget = Plugin.MaterialEvaluator.PickDiscardTarget(cards, min);
                if (discardTarget != null)
                {
                    return new List<ClientCard> { discardTarget };
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
