# Executor + Plugin Template (ลงทะเบียนครบทุกโมดูล)

> ใช้เป็นจุดเริ่มต้นของเด็คใหม่ทุกเด็ค · ทุก `CardId` ต้องตรวจจาก `cards.cdb` · ทุกเอฟเฟกต์ตรวจจาก `script/cXXXX.lua`
> Signature ทั้งหมดในไฟล์นี้ตรวจกับ source จริงแล้ว (ExecutorBase 2026-10-03)
> โครงที่ลงทะเบียน: Plugin (Strategy/Material/Threat) · Ace registry ×3 · BaitPlanner · ChainAdvisor · ComboRouter ·
> Optional-removal guard · OPT flags · Pending-Select · Core-pitfall handlers

## 1. `windbot-fork/Game/AI/Decks/SampleDeckExecutor.cs`

```csharp
using System.Collections.Generic;
using System.Linq;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("SampleDeck", "SampleDeck")]            // Name ต้องตรง bots.json "deck"; File = ชื่อ .ydk
    public class SampleDeckExecutor : ModernExecutor
    {
        public static class CardId
        {
            // ── ตรวจทุกตัวจาก cards.cdb ──
            public const int StarterA = 0;
            public const int StarterB = 0;
            public const int ExtenderA = 0;        // ใช้เป็น Bait ได้
            public const int SearchSpell = 0;
            public const int BossA = 0;
            public const int LinkFodder = 0;
            public const int AshBlossom = 14558127;
            public const int MaxxC = 23434538;
            public const int Impermanence = 10045474;
            public const int CalledByTheGrave = 24224830;
        }

        private static readonly int[] AceIds = { CardId.BossA };

        internal SampleDeckPlugin Plugin { get; }

        // ── OPT / Hard OPT flags (reset ใน OnNewTurn) ──
        private bool _starterAEffectUsed;
        private bool _searchSpellUsed;

        // ── Pending-Select (แทน AI.SelectCard สำหรับ hint ที่ core จัดการ) ──
        private int[] _pendingSelect;
        private long _pendingHint;

        public SampleDeckExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            Plugin = new SampleDeckPlugin(this);
            DeckPlugin = Plugin;                    // 🔒 ทุกเด็ค
            RegisterCoreModules();
            RegisterComboLines();
            RegisterExecutors();
        }

        // ═════════════ 1. CORE MODULE REGISTRATION ═════════════
        private void RegisterCoreModules()
        {
            // Ace registry: ต้องตรงกัน 3 ที่ + override IsAceCard ด้านล่าง
            ResourcePlan.RegisterAceCards(AceIds);
            HeuristicGuard.RegisterAceCards(AceIds);

            // Bait: register อย่างเดียวไม่พอ ต้องเรียก GetBaitIfNeeded(Card) ใน func ของ starter
            BaitPlanner.RegisterComboStarters(CardId.StarterA, CardId.StarterB);
            BaitPlanner.RegisterBaitCards(CardId.ExtenderA);
            foreach (int id in AceIds) BaitPlanner.RegisterNeverBait(id);

            // Chain timing: การ์ด "ศัตรู" ที่คุ้มตัดด้วย handtrap (chokepoint ของ meta ที่เด็คนี้เจอบ่อย)
            ChainAdvisor.RegisterHighValueTargets(/* opponent starter IDs */);

            // การ์ดเราที่มี optional destroy/banish/bounce → OnSelectYesNo ปฏิเสธเมื่อสนามศัตรูว่าง
            RegisterOptionalFieldRemovalCards(/* our optional-removal card IDs */);
        }

        public override bool IsAceCard(ClientCard card) => card != null && card.IsCode(AceIds);

        public override bool OnSelectHand() => true;   // true = ไปก่อน (เด็คไปหลัง → false)

        public override void OnNewTurn()
        {
            base.OnNewTurn();   // reset ComboRouter / BaitPlanner / DeckPlugin ให้แล้ว — ❌ ห้ามเรียก Plugin.ResetTurnState() ซ้ำ
            _starterAEffectUsed = false;
            _searchSpellUsed = false;
            _pendingSelect = null;
        }

        // ═════════════ 2. COMBO LINES (เฉพาะ step ที่ปลอดภัยเสมอ) ═════════════
        private void RegisterComboLines()
        {
            // ⚠️ Step ถูก execute ตรงๆ ไม่ผ่าน func ด้านล่าง → ใส่แค่ action ที่ไม่มีทางผิด + กรองด้วย Condition
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "SampleDeck-StarterA",
                RequiredCards = new List<int> { CardId.StarterA },
                EndBoardScore = 80,
                Priority = 1,
                Condition = () => !_starterAEffectUsed && !IsSpecialSummonBlocked() && !Bot.HasInMonstersZone(CardId.StarterA),
                Steps = new List<ComboRouter.ComboStep>
                {
                    new ComboRouter.ComboStep { CardId = CardId.StarterA, ActionType = ExecutorType.Summon, Description = "NS Starter A" },
                }
            });
        }

        // ═════════════ 3. EXECUTORS (ลำดับ = Priority) ═════════════
        private void RegisterExecutors()
        {
            // Tier 0 — Handtrap / Negate
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomActivate);
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, DefaultMaxxC);
            AddExecutor(ExecutorType.Activate, CardId.Impermanence, DefaultInfiniteImpermanence);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);

            // Tier 1 — Board breakers (ไปหลัง)
            // Tier 2 — Bait (ต้องอยู่ก่อน starter เพื่อให้ GetBaitIfNeeded มีผล)
            AddExecutor(ExecutorType.Activate, CardId.ExtenderA, ExtenderAActivate);

            // Tier 3 — Starters / Searchers
            AddExecutor(ExecutorType.Activate, CardId.SearchSpell, SearchSpellActivate);
            AddExecutor(ExecutorType.Summon, CardId.StarterA, StarterASummon);
            AddExecutor(ExecutorType.Activate, CardId.StarterA, StarterAEffect);   // รับทั้ง ignition + optional trigger

            // Tier 4 — Extra Deck (🔒 ห้าม AddExecutor(SpSummon, id) แบบไม่มี func)
            AddExecutor(ExecutorType.SpSummon, CardId.LinkFodder, LinkFodderSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BossA, BossASummon);

            // Tier 5 — Set / Repos
            AddExecutor(ExecutorType.SpellSet, DefaultSpellSet);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        // ═════════════ 4. ACTIVATION FUNCTIONS ═════════════
        private bool AshBlossomActivate()
        {
            if (Duel.CurrentChain.Any(c => c != null && c.Controller == 0 && c.IsCode(CardId.AshBlossom))) return false; // ไม่ซ้อนชื่อเดิม
            if (IsChainAlreadyNeutralized()) return false;                                                            // 1 Negate = 1 Problem
            return SmartHandTrapChain() && DefaultAshBlossomAndJoyousSpring();
        }

        private bool ExtenderAActivate()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool SearchSpellActivate()
        {
            if (_searchSpellUsed) return false;                                            // Hard OPT
            if (Duel.Phase == DuelPhase.Main2 && IsBoardStrongEnough()) return false;      // core MP2 guard กัน ResourceGain ให้แล้ว (v0.086) — คงไว้ให้อ่านง่าย/ไม่พึ่ง ClassifyAction
            int target = PickSearchFor();
            if (target == 0) return false;                                                // Can Activate ≠ Should Activate
            SetPendingSelect(506, target);
            _searchSpellUsed = true;
            DecisionTracer.TraceActivate(nameof(SearchSpellActivate), $"search {target}");
            return true;
        }

        private bool StarterASummon()
        {
            var bait = GetBaitIfNeeded(Card);                                              // 🎯 Bait ก่อน Core
            if (bait != null && bait.Id != Card.Id) return false;
            return !Bot.HasInMonstersZone(CardId.StarterA);
        }

        private bool StarterAEffect()
        {
            // func นี้ถูกเรียกทั้งตอนเปิดใช้ปกติ และตอน optional trigger (OnSelectEffectYn)
            // ดู n จาก lua: e:SetDescription(aux.Stringid(id, n))
            if (ActivateDescription == Util.GetStringId(CardId.StarterA, 0))
            {
                if (_starterAEffectUsed) return false;
                int target = PickSearchFor();
                if (target == 0) return false;
                SetPendingSelect(506, target);
                _starterAEffectUsed = true;
                return true;
            }
            return false;                                                                 // เอฟเฟกต์ที่ยังไม่ออกแบบ → ไม่เปิด
        }

        private bool LinkFodderSummon()
        {
            if (ShouldSkipLinkSummon()) return false;
            int fodder = Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && !IsAceCard(m));
            if (fodder < 2) return false;                                                 // ไม่ใช้ Ace เป็น material
            return Bot.HasInExtra(CardId.BossA);                                          // ต้องมีเหตุผล: เปิดทางไป Boss
        }

        private bool BossASummon()
        {
            if (ShouldAvoidGenericExtraDeckSummon(2)) return false;
            AI.SelectPosition(CardPosition.FaceUpAttack);
            return true;
        }

        // ═════════════ 5. HELPERS ═════════════
        private int PickSearchFor()
        {
            int[] priority = { CardId.StarterA, CardId.StarterB, CardId.ExtenderA };
            foreach (int id in priority)
            {
                if (Bot.HasInHand(id)) continue;
                if (Bot.GetRemainingCount(id, 3) <= 0) continue;                          // initialCount = จำนวนใน .ydk
                return id;
            }
            return 0;
        }

        private bool IsChainAlreadyNeutralized()
        {
            ClientCard last = Util.GetLastChainCard();
            if (last == null || last.Controller != 1) return true;
            if (last.IsDisabled()) return true;
            return Duel.NegatedChainIndexList.Contains(Duel.CurrentChain.Count);
        }

        private void SetPendingSelect(long hint, params int[] ids)
        {
            _pendingHint = hint;
            _pendingSelect = ids;
        }

        // ═════════════ 6. SELECTION ═════════════
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectCard(cards, min, max, hint, cancelable);

            // (1) Pending intent จาก activate func — ชนะ core routing
            if (_pendingSelect != null && hint == _pendingHint)
            {
                int[] ids = _pendingSelect;
                _pendingSelect = null;
                var pick = cards.Where(c => c != null && c.IsCode(ids)).Take(max).ToList();
                if (pick.Count >= min) return pick;
            }

            // (2) Custom hint ของการ์ด: if (hint == Util.GetStringId(CardId.X, n)) { ... }

            // (3) Deck→GY เจาะจง: core ส่ง 504 (Deck ทั้งหมด) ไป `Strategy.PickSearchTarget` ซึ่งใช้ priority ของ search
            //     ถ้าเด็คต้องการ "ตัวที่มีผลใน GY" (คนละชุดกับ search) ให้ตัดสินที่นี่
            //     (507/504 cost ฝั่งเรา core ส่งไป `MaterialEvaluator.PickDiscardTarget` ให้แล้ว — ไม่ต้องเขียนซ้ำ)
            if (hint == 504 && cards.All(c => c != null && c.Location == CardLocation.Deck))
            {
                var gy = Plugin.StrategyImpl.PickDeckToGrave(cards);
                if (gy != null) return new List<ClientCard> { gy };
            }

            // (4) ที่เหลือให้ core: hint routing → DeckPlugin → HeuristicGuard
            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
```

## 2. `windbot-fork/Game/AI/Plugins/SampleDeckPlugin.cs`

```csharp
using System.Collections.Generic;
using System.Linq;
using WindBot.Game.AI.Decks;
using WindBot.Game.AI.Plugin;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Plugins
{
    public class SampleDeckPlugin : DeckPluginBase
    {
        public override string DeckName => "SampleDeck";

        public SampleDeckStrategy StrategyImpl { get; }
        public SampleDeckMaterialEvaluator MaterialImpl { get; }
        public SampleDeckThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;
        // เพิ่มเมื่อจำเป็น: ResourceEvaluator (Counter) / ScaleResolver (Pendulum) / ActionScorer

        public SampleDeckPlugin(SampleDeckExecutor exec)
        {
            StrategyImpl = new SampleDeckStrategy(exec);
            MaterialImpl = new SampleDeckMaterialEvaluator(exec);
            ThreatImpl = new SampleDeckThreatEvaluator(exec);
        }
        // ไม่ต้อง override ResetTurnState — base เรียก Strategy.Reset() + ResourceEvaluator.Reset() ให้แล้ว
    }

    public class SampleDeckStrategy : IDeckStrategy
    {
        private readonly SampleDeckExecutor _exec;
        public SampleDeckStrategy(SampleDeckExecutor exec) { _exec = exec; }

        public void Reset() { }

        // ถูกเรียกเมื่อ hint 506, 505(ฝั่งเรา) และ 504 Deck→GY (ถ้า executor ไม่ตัดสินก่อน) — คืน null = ให้ core heuristic ตัดสิน
        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            int[] priority = { SampleDeckExecutor.CardId.StarterA, SampleDeckExecutor.CardId.StarterB, SampleDeckExecutor.CardId.ExtenderA };
            foreach (int id in priority)
            {
                if (_exec.Bot.HasInHand(id)) continue;                                     // ไม่เสิร์ชตัวซ้ำ
                var c = candidates.FirstOrDefault(x => x != null && x.IsCode(id));
                if (c != null) return c;
            }
            return null;
        }

        // hint 509
        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            return candidates.FirstOrDefault(c => c != null && c.IsCode(SampleDeckExecutor.CardId.BossA));
        }

        // ใช้เองใน executor OnSelectCard (3) — เลือกตัวที่มีผลใน GY แทน priority ของ search
        public ClientCard PickDeckToGrave(IList<ClientCard> candidates)
        {
            return candidates.FirstOrDefault(c => c != null && c.IsCode(SampleDeckExecutor.CardId.ExtenderA)); // ตัวที่มีผลในสุสาน
        }
    }

    public class SampleDeckMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly SampleDeckExecutor _exec;
        public SampleDeckMaterialEvaluator(SampleDeckExecutor exec) { _exec = exec; }

        // ต่ำ = ใช้ก่อน: Token → Fodder → ใช้ผลแล้ว → ซ้ำ → สำคัญ → Ace
        public int GetMaterialCost(ClientCard c)
        {
            if (c == null) return int.MaxValue;
            if (c.HasType(CardType.Token)) return 0;
            if (_exec.IsAceCard(c)) return 10000;
            if (CardIntelligence.IsKnownNegator(c.Id) || CardIntelligence.IsHandtrap(c.Id)) return 5000;
            return 100 + c.Attack / 10;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
            => candidates.Where(c => c != null).OrderBy(GetMaterialCost).ToList();

        // hint 500/501/504(ฝั่งเรา ไม่ใช่ Deck)/507(ฝั่งเรา) — ทิ้งตัวที่มีผลในสุสาน/ตัวซ้ำก่อน
        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
            => SortMaterials(candidates, min).FirstOrDefault();

        // hint 502 เมื่อศัตรูมีไม่พอ (pop ตัวเอง) — ห้ามเลือก Ace
        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
            => candidates.Where(c => c != null && !_exec.IsAceCard(c)).OrderBy(GetMaterialCost).FirstOrDefault();
    }

    public class SampleDeckThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly SampleDeckExecutor _exec;
        public SampleDeckThreatEvaluator(SampleDeckExecutor exec) { _exec = exec; }

        // บวกเพิ่มจาก CardIntelligence สำหรับการ์ดที่ตัดเด็คนี้โดยเฉพาะ (เช่น floodgate ที่ล็อกกลไกหลัก)
        public int EvaluateThreatScore(ClientCard card) => 0;

        // core ไม่เรียก — ใช้เองใน executor เพื่อปลด Tier 0
        public bool IsEmergencyThreat(ClientCard card) => false;
    }
}
```

## 3. Registration Checklist (ลงทะเบียนเด็คใหม่)

| # | ที่ไหน | ทำอะไร |
|---|---|---|
| 1 | `windbot-fork/Decks/<Deck>.ydk` | ID ครบ + ผ่าน banlist |
| 2 | `Game/AI/Decks/<Deck>Executor.cs` | `[Deck("<Deck>", "<ydk name>")]` |
| 3 | `Game/AI/Plugins/<Deck>Plugin.cs` | Strategy + Material + Threat |
| 4 | `windbot-fork/bots.json` | `{ "name", "deck" (= Deck.Name), "difficulty": 3, "dialog": "default", "flags": ["OCG","TCG"], "masterRules": [4,5] }` |
| 5 | `dashbot/MainWindow.xaml.cs` | เพิ่มชื่อใน `ModernArchetypes` (หรือ prefix `Anime_` / `GOAT_` / `Special_`) |
| 6 | `CardIntelligence.cs` | เพิ่ม floodgate / negator / handtrap / chokepoint ใหม่ที่เด็คนี้มี (ถ้ายังไม่มี) |
| 7 | Build | `BUILD_AND_DEPLOY.ps1` 0 Errors |
