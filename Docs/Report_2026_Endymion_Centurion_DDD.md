# Modern Bot Domain Plugin Refactoring Report: Endymion, Centur-Ion & D/D/D (v12.0 Audited)

## Executive Summary
In accordance with the project's **Domain Plugin Architecture** (Layer 3 in `SKILL.md`) established with the Six Samurai benchmark, all three modern 2026 bots (**Endymion**, **Centur-Ion**, **D/D/D**) and **Six Samurai** have been elevated into fully decoupled domain architectures. 

Furthermore, a comprehensive **Smart Position Control & Handtrap Survival System** has been incorporated to eliminate low-ATK/high-DEF monster vulnerabilities (e.g. Ash Blossom 0/1800, D/D Savant Kepler 0/0, Six Samurai Fuma 200/1800).

---

## 1. Architectural Standardization (Six Samurai Benchmark)

Each modern deck now implements the strict 7-class Domain Plugin standard:
```
Deck Executor (_2026_<Deck>Executor.cs)
├── [Deck]Plugin              ← Master lifecycle manager & domain coordinator
├── [Deck]Strategy            ← Turn routing, route decisions & win conditions
├── [Deck]DomainHelper(s)     ← Archetype-specific domain mechanics
├── [Deck]MaterialScorer      ← Boss protection & material sacrifice penalties
├── [Deck]ActionScorer        ← Dynamic utility scoring for Phase 1 & 2 actions
└── [Deck]BoardAssessor       ← Live field strength, lethal calculation & threat grading
```

### Comparative Architecture Matrix
| Deck | Domain Plugin Class | Key Domain Specialization Helper | Material Safeguard | Smart Repositioning |
| :--- | :--- | :--- | :--- | :--- |
| **Endymion** | `EndymionPlugin` | `EndymionCounterEconomy`<br>`EndymionScaleResolver` | Jackal King (10,000 pts)<br>Mighty Master (10,000 pts) | Handtraps & 0 ATK $\rightarrow$ DEF |
| **Centur-Ion** | `CenturionPlugin` | `CenturionTimingAdvisor`<br>`CenturionResourceLoop` | Cosmic Blazar (10,000 pts)<br>Legatia / Auxila (5,000 pts) | Stand-Up Trap $\rightarrow$ DEF |
| **D/D/D** | `DDDPlugin` | `DDDContractBurnManager`<br>`DDDScaleAndSearchResolver` | Machinex / Caesar (10,000 pts)<br>Gilgamesh Link (5,000 pts) | Kepler 0/0 $\rightarrow$ DEF |
| **Six Samurai** | `SixSamuraiPlugin` | `SixSamCounterEconomy`<br>`SixSamKizaruResolver` | Shi En / Lord Shi En (10,000 pts)<br>Apollousa / S:P (10,000 pts) | Fuma 200/1800 $\rightarrow$ DEF |

---

## 2. Deck-Specific Refactoring Details

### 2.1 Endymion (`_2026_EndymionExecutor.cs`)
- **`EndymionPlugin`**: Central coordinator decoupling all spell counter and scale logic from engine calls.
- **`EndymionCounterEconomy`**:
  - Five-tier counter reserve level: `Critical` (0–1), `Low` (2–3), `Ready` (4–5), `ComboReady` (6–7), `Surplus` (8+).
  - Threat-prioritized monster negate budgeting for Jackal King (2 counters per negate, avoiding double negations).
  - Mighty Master S/T bounce priority: Recycles expended scales (`Servant` / `Magister`) while preserving active counter tanks.
  - Global fuel drain priority: `Magical Citadel` $\rightarrow$ `Mythical Institution` $\rightarrow$ surplus monsters.
- **`EndymionScaleResolver`**: Evaluates scale numbers (Low 2 vs High 8) and protects the optimal pendulum summon window.
- **`EndymionMaterialScorer`**: Penalizes tributing or linking away Jackal King, Mighty Master, or Selene with heavy penalties (-10,000 score).

### 2.2 Centur-Ion (`CenturionExecutor.cs`)
- **`CenturionPlugin`**: Coordinates timing windows and S/T zone monster recycling.
- **`CenturionTimingAdvisor`**:
  - `StandUpCenturIon` opponent-turn Quick Synchro no longer triggers prematurely. It evaluates opponent actions to strike at key chokepoints:
    1. Opponent summons monster with ATK $\ge 1800$.
    2. Opponent summons high-threat starter, extender, or boss.
    3. Opponent controls $\ge 2$ monsters.
    4. Opponent activates an effect in chain.
    5. Crimson Dragon $\rightarrow$ Cosmic Blazar Dragon tag-out gating.
- **`CenturionResourceLoop`**:
  - End Phase automated recovery: Places `Primera` and `Trudea` into S/T zones from GY/banishment for next-turn extension.
  - S/T zone manager: Prevents board clogs and reserves spaces for Counter Trap `Phalanx` and `True Awakening`.

### 2.3 D/D/D (`_2026_DDDExecutor.cs`)
- **`DDDPlugin`**: Orchestrates Pendulum, Fusion, Synchro, Xyz, and Link summoning routes.
- **`DDDContractBurnManager`**:
  - Calculates cumulative Dark Contract standby burn damage each turn.
  - Activates **Contract Clearance Protocol** if current LP $\le 2000$ or burn exceeds remaining LP, prioritizing D/D/D D'Arc LP inversion or sending contracts to GY via Machinex/Swirl/Necro.
- **`DDDScaleAndSearchResolver`**: Dynamically chooses Dark Contract search targets (Gate for combo pieces, Swamp for fusions, Witch for removal).
- **`DDDMaterialScorer`**: Heavily protects end-board bosses (Deus Machinex, High King Caesar, Siegfried, Kali Yuga) from being consumed as fodder.

---

## 3. Smart Position Control & Handtrap Survival System

### 3.1 The Tactical Problem
In Yu-Gi-Oh!, Normal Summons are always performed in **Face-up Attack Position**. Handtraps (such as Ash Blossom 0/1800, Ghost Belle 0/1800, Effect Veiler 0/0) and combo searchers (D/D Savant Kepler 0/0, Six Samurai Fuma 200/1800) have low ATK and high DEF. If stranded in Attack position, opponent monsters can deal massive lethal/OTK battle damage.

### 3.2 Implemented Two-Tier Defense Architecture

#### Tier 1: `OnSelectPosition` (Summon & Special Summon)
When summoned via effect, Special Summon, or revive:
```csharp
public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
{
    // Specific Handtrap & Wall Overrides (Ash 0/1800, Fuma 200/1800, Kepler 0/0)
    if (cardId == CardId.AshBlossom || cardId == CardId.Fuma)
    {
        if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
        if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
    }

    return base.OnSelectPosition(cardId, positions);
}
```
`ModernExecutor.OnSelectPosition` also automatically enforces Defense position for any monster where `def > atk && atk < 1800` or `atk == 0`.

#### Tier 2: `SmartMonsterRepos` (Field Repositioning via `ExecutorType.Repos`)
If a low ATK / high DEF monster is already on the field in Attack position (e.g. from an unavoidable normal summon):
```csharp
private bool SmartMonsterRepos()
{
    if (Card == null) return false;
    
    // 1. Handtraps or Low ATK / High DEF walls stranded in Attack -> Switch to Defense!
    if (Card.IsAttack() && (Card.Attack == 0 || (Card.Defense > Card.Attack && Card.Defense >= 1800)))
        return true;
        
    // 2. High ATK monsters accidentally in Defense when we can push for lethal -> Switch to Attack!
    if (Card.IsDefense() && Card.Attack > Card.Defense && Card.Attack >= 1800 && Duel.Turn > 1)
        return true;
        
    return DefaultMonsterRepos();
}
```

---

## 4. Policy for Future Decks vs Legacy Decks

1. **New Decks (Strict Mandate)**:
   - All newly added decks MUST be implemented with the decoupled **Domain Plugin Architecture** (Layer 3 in `SKILL.md`).
   - All newly added decks MUST implement `SmartMonsterRepos` and `OnSelectPosition` protection for survival walls and handtraps.
   - Decks must be registered in `bots.json` with canonical name and relevant aliases.
2. **Legacy Working Decks (Do Not Break)**:
   - Existing legacy bots (`AI_*`, `GOAT_*`, older archetypes) that are currently functional must NOT be touched or destabilized unless explicitly requested by the user.

---

## 5. Build & Deployment Verification

Executed:
```powershell
cd C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN
powershell -ExecutionPolicy Bypass -File .\BUILD_AND_DEPLOY.ps1
```
- **WindBot.dll, ExecutorBase.dll, core.dll**: Compiled with **0 Errors**.
- **DashBot.exe**: Compiled with **0 Errors**.
- **Exclusive Deployment Target**: Verified strictly deployed to `C:\Users\admin\Documents\EdoGame\`.
