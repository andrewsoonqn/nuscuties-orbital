# Coin System Integration: Daily Quests & Passive Dungeons

## Context
- The project uses a modular, component-based architecture for player stats and progression.
- Coins are a separate meta-currency, not to be conflated with EXP.
- User requested:
  - Level-scaled coin rewards (higher level = more coins)
  - Coins for daily quest completion/incompletion
  - Exponential, random coin rewards for passive dungeons (more time = much more coins, but with run-to-run variance)
  - All coin logic to be separate from EXP logic

## Design Principles
- **Separation of Concerns:** Coins are managed by `PlayerInventoryManager`, EXP by `ProgressionManager`.
- **Level Scaling:** All coin rewards scale with player level using exponential or multiplicative formulas.
- **Randomness:** Passive dungeon coins use a random factor for run-to-run variance.
- **Observer Pattern:** Coin changes emit signals for UI updates.
- **Persistence:** Coins are saved in `player_inventory.json`, separate from EXP and stats.

## Implementation Steps

### 1. Extend PlayerInventoryManager
- Added methods:
  - `CalculateDailyQuestCoinReward()` — returns coins for daily quest, scales as `base * 1.15^(level-1)`
  - `CalculatePassiveDungeonCoinReward(timeMinutes)` — exponential with time, level, and random variance
  - `CalculateActiveDungeonEnemyCoinReward()` — for future use
- Added reference to `ProgressionManager` for level access

### 2. Daily Quest Integration
- In `CompletableQuestComponent.cs`:
  - Added `PlayerInventoryManager` reference
  - On quest completion: add EXP and coins
  - On quest uncompletion: subtract EXP and attempt to subtract coins (with safety check)
  - Uses new calculation method for level scaling

### 3. Passive Dungeon Integration
- In `PassiveSessionInfoManager.cs`:
  - Added coin tracking: `setAccumulatedCoins`, `getAccumulatedCoins`
- In `PassiveOngoing.cs`:
  - Added real-time coin calculation using `CalculatePassiveDungeonCoinReward`
  - Updates coin label in UI (if present)
  - Stores coins in session manager at end
- In `PassiveEndScene.cs`:
  - Reads coins from session manager
  - Awards coins to player inventory
  - Displays total coins gained in UI (if present)

### 4. UI/UX
- Coin rewards are displayed in passive dungeon UI and end screen (if label is present)
- Console prints for daily quest coin changes
- All changes are null-safe for missing UI elements

## Example Formulas
- **Daily Quest:** `50 * 1.15^(level-1)`
- **Passive Dungeon:** `5 * (1 + 0.1*level) * (minutes^1.2) * random(0.85, 1.15)`

## Code Changes Summary
- `frontend/tools/PlayerInventoryManager.cs`: Added coin calculation methods, level reference
- `frontend/daily/components/CompletableQuestComponent.cs`: Integrated coin rewards/penalties
- `frontend/passive/tools/PassiveSessionInfoManager.cs`: Added coin tracking
- `frontend/passive/PassiveOngoing.cs`: Real-time coin calculation and UI
- `frontend/passive/PassiveEndScene.cs`: Coin award and display

## Result
- Coins are now awarded for daily quest completion and passive dungeon time, scaling with level and including randomness for passive dungeons.
- All logic is modular, testable, and follows project architecture standards.
