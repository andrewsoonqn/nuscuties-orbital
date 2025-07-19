# LoadoutSpawner System

The `LoadoutSpawner` is a core system that bridges the gap between the shop's equipment data and the actual in-game objects. It reads equipped items from the `PlayerInventoryManager` and translates them into functional game objects like weapons, passive effects, and active abilities.

## Architecture

### Core Components

1. **LoadoutSpawner** (`tools/LoadoutSpawner.cs`)
   - Autoload singleton that handles loadout creation
   - Reads equipped items from `PlayerInventoryManager`
   - Creates actual game objects from `ItemCatalog` data

2. **LoadoutData** (nested class)
   - Data structure containing:
     - `MeleeWeapon`: Equipped melee weapon
     - `ProjectileWeapon`: Equipped projectile weapon
     - `UtilityItem`: Equipped utility item node
     - `PassiveEffects`: List of passive effects
     - `ActiveAbilities`: List of active abilities

### Integration Points

#### Shop System Integration
- Reads equipped item IDs from `PlayerInventoryManager.GetEquipped(category)`
- Uses `ItemCatalog.Get(itemId)` to retrieve item definitions
- Maps item categories to appropriate creation methods

#### Weapon System Integration
- Uses `WeaponCreator.CreateFromCatalog()` for weapon creation
- Provides fallback to default weapons if catalog fails
- Integrates with existing weapon switching logic

#### Character Integration
- `Player.cs` uses LoadoutSpawner in `_Ready()` to spawn initial loadout
- `ReloadLoadout()` method allows dynamic equipment changes
- Proper cleanup in `_ExitTree()`

## Equipment Categories

### Weapons
- **Melee**: Always equipped (defaults to sword if none specified)
- **Projectile**: Optional secondary weapon

### Utility Items
- Scene-based items loaded from `ItemDef.ScenePath`
- Added as child nodes to the character

### Passive Effects
- Implement `IPassiveEffect` interface
- Applied once when loadout is equipped
- Automatically removed when loadout changes

### Active Abilities
- Implement `IActiveAbility` interface
- Added as child nodes for processing
- Handle their own input and cooldown logic

## Usage Examples

### Basic Weapon Spawning
```csharp
var loadout = _loadoutSpawner.SpawnLoadout(player);
if (loadout.MeleeWeapon != null)
{
    player.EquipWeapon(loadout.MeleeWeapon);
}
```

### Passive Effect Implementation
```csharp
public partial class HealthBoostEffect : Node, IPassiveEffect
{
    public void ApplyEffect(Node player)
    {
        // Increase player's max health
    }

    public void RemoveEffect(Node player)
    {
        // Restore original max health
    }
}
```

### Active Ability Implementation
```csharp
public partial class DashAbility : Node, IActiveAbility
{
    public bool Activate()
    {
        if (IsOnCooldown()) return false;
        // Perform dash logic
        return true;
    }

    public bool IsOnCooldown() => !_cooldownTimer.IsStopped();
    public float GetCooldownRemaining() => _cooldownTimer.TimeLeft;
}
```

## Item Catalog Format

Items in `assets/data/items_catalog.json` should include:

```json
{
  "Id": "health_necklace",
  "Category": "necklace_passive",
  "Title": "Necklace of Vitality",
  "Description": "Increases maximum health by 20 points",
  "Price": 500,
  "IconPath": "res://assets/dawn_like/Items/Necklace0.png",
  "ScenePath": "res://active/characters/PassiveEffects/health_boost_effect.tscn",
  "StatsPayload": {
    "healthBonus": 20
  }
}
```

## Input Handling

Active abilities can be triggered through input:
- Current implementation uses "dash" action (Space key)
- Abilities can implement custom input handling in their scenes
- Player class routes input to active abilities via `HandleActiveAbilityInput()`

## Error Handling

The system includes robust error handling:
- Missing items fall back to defaults (sword for melee)
- Invalid scene paths are logged but don't crash the game
- Failed item creation uses graceful degradation

## Extension Points

### Adding New Equipment Types
1. Add new category to `PlayerInventoryData`
2. Extend `LoadoutData` with new fields
3. Add creation method to `LoadoutSpawner`
4. Update `ApplyLoadout` and `RemoveLoadout` methods

### Creating New Passive Effects
1. Create class implementing `IPassiveEffect`
2. Create .tscn file with the script attached
3. Add item to catalog with correct `ScenePath`
4. Configure `StatsPayload` as needed

### Creating New Active Abilities
1. Create class implementing `IActiveAbility`
2. Create .tscn file with the script attached
3. Add `Initialize(Character)` method if needed
4. Handle physics/input processing in the ability class
5. Add item to catalog with correct `ScenePath`

## Dependencies

- `PlayerInventoryManager`: Equipment state
- `ItemCatalog`: Item definitions
- `WeaponCreator`: Weapon instantiation
- `DerivedStatCalculator`: Damage calculations
- `IPassiveEffect`: Passive effect interface
- `IActiveAbility`: Active ability interface

## Testing

The system can be tested by:
1. Equipping items in the shop
2. Starting an active dungeon
3. Verifying equipped items appear in-game
4. Testing passive effects (health boost)
5. Testing active abilities (dash with Space key)
