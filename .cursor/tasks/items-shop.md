- [ ] Objective
  - [ ] Implement the unified, data-driven item, skills & shop system defined in `plans/items-shop` (v2 design).
    Includes new save manager, catalogue, shop UI, load-out workflow, reward hooks, refactors, tests and documentation.

- [x] PlayerInventoryManager (coins, inventory, load-out)
  - [x] Define `PlayerInventoryData` struct (coins, unlockedIds, equippedIds, saveVersion) - **4 categories: melee, projectile, necklace_passive, necklace_active**.
  - [x] Implement `PlayerInventoryManager : BaseStatManager<PlayerInventoryData>` as autoload node.
  - [x] Add API methods: `AddCoins`, `SpendCoins`, `UnlockItem`, `EquipItem`, `UnequipItem`, `IsUnlocked`, `IsEquipped`.
  - [x] Emit signals: `CoinsChanged`, `InventoryChanged`, `LoadoutChanged`.
  - [x] Implement versioned migration skeleton.
  - [x] Guard checks: non-negative coins, one equip per category, catalogue validation.
  - [ ] Update `PlayerInventoryData` to remove utility category.
  - [ ] Unit tests covering coins, unlock/equip logic and migrations.

- [x] ItemCatalogue
  - [x] Create `assets/data/items_catalog.json` with starter items (sword, staff) & placeholders for others.
  - [x] Implement `ItemCatalog` singleton that loads JSON into `Dictionary<string, ItemDef>`.
  - [x] Define `ItemDef` class/struct (id, category, title, desc, price, iconPath, scenePath, statsPayload).
  - [x] Provide lookup helpers `Get(id)` and `GetByCategory(cat)`.
  - [ ] Add new melee weapons (dagger, spear, longsword) to catalog.
  - [ ] Add armor & bomb items to passive/active necklace categories.
  - [ ] Unit test JSON load and retrieval helpers.

- [x] Unified Weapon Strategy Architecture
  - [x] ~~Define `IInventoryItem` (OnEquip(Node player), OnUnequip()).~~ **Removed - inventory items are pure data**
  - [x] ~~Define `IPassiveEffect` and `IActiveAbility` interfaces.~~ **Replaced with unified weapon strategy approach**
  - [x] Refactor `WeaponCreator` to accept `ItemDef` and create `Weapon` objects for ALL categories.
  - [x] Map existing sword & staff scenes to new catalogue; update creation calls.
  - [ ] Implement `PassiveEffectStrategy` for passive necklaces (armor, multi-shot, etc.).
  - [ ] Implement `ActiveAbilityStrategy` for active necklaces (dash, bomb, etc.).

- [x] Shop UI
  - [x] Create `ui/shop/shop.tscn` (Control ➔ TabContainer ➔ GridContainer) - **4 tabs: Melee, Projectile, Passive, Active**.
  - [x] Build `ItemCard.tscn` with icon, title, price/desc, action button.
  - [x] Implement `ShopController` script: populate tabs from `ItemCatalog`, manage state machine (Locked, Affordable, Unlocked, Equipped).
  - [x] Connect buttons to `PlayerInventoryManager` for purchases & equips.
  - [x] Listen to `CoinsChanged` & `InventoryChanged` signals for live refresh.
  - [x] Display coin total HUD element.
  - [ ] Update shop UI to remove utility tab.
  - [ ] Integration test purchase & equip flow.

- [ ] Load-out Management
  - [ ] Update `LoadoutSpawner` to create `Weapon` objects for ALL categories via `WeaponCreator.CreateFromCatalog()`.
  - [ ] Update `LoadoutData` structure to contain only `Weapon` objects (remove `IPassiveEffect`/`IActiveAbility` lists).
  - [ ] Implement hot-swap for melee/projectile weapons (`1`, `2` keys) using existing `Player.SwitchWeapon()`.
  - [ ] Ensure passive weapons auto-activate on spawn; active weapons respond to `E` key input.

- [ ] Reward Integration
  - [ ] Implement `RewardCalculator` formulas (daily, passive, active drops scaled by level).
  - [ ] Hook coin rewards in DailyQuest, PassiveDungeon clear, ActiveDungeon enemy death.
  - [ ] Ensure `AddCoins` and level-up sequencing order (coins update after EXP signals).

- [ ] Error Handling & Validation
  - [ ] Disable shop buttons when validation fails (insufficient coins, already equipped, etc.).
  - [ ] Log invalid states with `GD.PrintErr` without crashing.

- [ ] Migration from Prototype Assets
  - [ ] Remove hard-coded weapon path dictionary in `Weapon` once catalogue fully adopted.
  - [ ] Update existing scenes/scripts to use new interfaces where applicable.

- [ ] Testing Suite
  - [ ] Write unit tests: `PlayerProgressManagerTest`, `ItemCatalogLoadTest`, `ShopTransactionTest`.
  - [ ] Write integration tests: dungeon load-out spawn & hot-swap flow.
  - [ ] Regression tests: coin spend edge cases, save/load persistence.

- [ ] Documentation & Cleanup
  - [ ] Update README with new progression loop, shop screenshots, keybinds.
  - [ ] Add code comments where necessary and generate XML docs (tooling permissible).
  - [ ] Remove deprecated paths/constants.
