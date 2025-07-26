# Items & Shop System – Design Decision Record

## Context

The original game featured hard-coded weapon creation through `WeaponCreator.CreateSword()` and `WeaponCreator.CreateStaff()` methods, with weapon types managed via static enums and scene paths embedded directly in code. While functional for a minimal prototype, this approach created several scalability and maintenance issues: adding new items required code changes across multiple files, balancing required recompilation, and the lack of player progression beyond EXP/stats limited long-term engagement.

The existing progression system (`ProgressionManager`, `PlayerStatManager`) provided a solid foundation for character advancement but had no mechanism for item-based progression or player choice in equipment. Without a shop or inventory system, players had no meaningful decisions to make regarding their loadout, reducing strategic depth and replay value. The tight coupling between weapon definitions and code also made it difficult for designers to iterate on item balance or add new content without programmer intervention.

## The Solution

The implemented solution introduces a unified, data-driven item and shop architecture that seamlessly integrates with existing progression systems while maintaining backward compatibility. At its core is the `PlayerInventoryManager` extending the established `BaseStatManager<T>` pattern, providing consistent save/load behavior alongside EXP and stat persistence. All item definitions are externalized to `assets/data/items_catalog.json`, enabling rapid iteration and content expansion without code changes.

The system introduces three key abstractions: `ItemDef` for data-driven item definitions, `PlayerInventoryData` for save-compatible inventory state, and `ItemCatalog` as a singleton data provider. Items are organized into five categories (melee, projectile, utility, necklace_passive, necklace_active) with enforced one-per-category equipment rules. The `PlayerInventoryManager` handles all coin transactions, unlock validation, and equipment state through a comprehensive API that emits granular signals for UI synchronization.

## Critical Architectural Correction: Inventory vs Combat Separation

During implementation, a fundamental architectural decision emerged that required significant correction of the initial design. The original plan included an `IInventoryItem` interface that would be implemented by weapon scene objects, creating inappropriate coupling between the inventory system (data management) and combat system (scene behavior).

### The Problem

The initial approach violated the Single Responsibility Principle by mixing concerns:
- **Weapons** are combat objects that handle damage, animations, and battle mechanics
- **Inventory items** are data entries representing player choices and progression state
- These are fundamentally different layers that should remain decoupled

Having weapons implement inventory interfaces would have created several issues:
1. **Tight coupling** between data persistence and scene lifecycle
2. **Confused responsibilities** - weapons knowing about inventory state
3. **Testing complexity** - requiring scene instantiation to test inventory logic
4. **Maintenance burden** - changes to inventory affecting combat code

### The Corrected Architecture

The solution maintains strict separation between two distinct systems:

**Inventory System (Pre-dungeon)**:
- `PlayerInventoryManager` handles pure data operations (coins, unlock/equip state)
- `ItemCatalog` provides metadata and definitions from JSON
- Equipment is represented as string IDs in save data
- No behavioral contracts or scene dependencies

**Combat System (In-dungeon)**:
- `Weapon` classes remain focused solely on combat mechanics
- `WeaponCreator.CreateFromCatalog()` serves as translation layer
- Item IDs are converted to actual game objects only when needed
- No inventory concerns in combat code

**Translation Flow**:
```
Pre-dungeon: PlayerInventoryManager.EquipItem("staff")
    ↓ (Pure data operation)
Equipped state: { "projectile": "staff" }
    ↓ (Dungeon entry)
Translation: WeaponCreator.CreateFromCatalog("staff", player, damageFunc)
    ↓ (Spawns actual game object)
In-dungeon: Weapon scene with combat behavior
```

This correction ensures each system has a single, well-defined responsibility and can evolve independently.

## Unified Weapon Strategy Architecture Decision

After establishing the clean separation between inventory and combat systems, a critical architectural decision emerged: how to implement the different item behaviors (melee weapons, projectile weapons, passive effects, and active abilities) in a cohesive manner.

### The Unified Strategy Approach

Rather than creating separate behavioral hierarchies for each item type, the system adopts a **unified weapon strategy architecture** where ALL items are implemented as `Weapon` objects with different `IUseStrategy` implementations. This decision leverages the existing, well-tested weapon + strategy pattern while maintaining consistency across the entire equipment system.

**Key Benefits:**
1. **Consistency**: All items follow the same `Weapon.Use()` → `strategy.Use(weapon)` flow
2. **Reusability**: Existing weapon infrastructure (hitboxes, animations, scene management) works for all categories
3. **Simplicity**: Single creation pathway through `WeaponCreator.CreateFromCatalog()` for all item types
4. **Extensibility**: New item behaviors only require implementing `IUseStrategy`, not entirely new systems

**Strategy Implementations:**
- **Melee Weapons**: `WaitForAnimationUserStrategy` (existing) - handles animation-based melee combat
- **Projectile Weapons**: `ProjectileUseStrategy` (existing) - manages projectile firing and lifecycle
- **Passive Effects**: `PassiveEffectStrategy` (new) - applies persistent modifiers once on equip
- **Active Abilities**: `ActiveAbilityStrategy` (new) - handles cooldown-based triggered abilities

### Alternative Approaches Considered

**Separate Interface Hierarchies** (`IPassiveEffect`, `IActiveAbility`): Rejected because it would create parallel systems requiring separate management, testing, and integration pathways. While conceptually pure, this approach would increase complexity without proportional benefits.

**Component-Based Architecture**: Rejected because it would require significant refactoring of existing weapon systems and introduce additional complexity in scene composition and lifecycle management.

**Inheritance-Based Hierarchy**: Rejected because it would create rigid coupling between item types and prevent the flexible composition that the Strategy pattern enables.

### Implementation Implications

This unified approach means that:
- All items are created through the same `WeaponCreator.CreateFromCatalog()` method
- Category-specific behavior is encapsulated within strategy implementations
- The `LoadoutSpawner` can treat all items uniformly as `Weapon` objects
- Input handling, activation, and lifecycle management follow consistent patterns
- Testing can leverage existing weapon testing infrastructure

The decision reinforces the principle that architectural consistency often trumps conceptual purity when it enables simpler implementation, testing, and maintenance.

## Architectural Principles

This implementation was carefully designed around SOLID principles and established patterns to ensure maintainability and extensibility. The **Single Responsibility Principle** is enforced through clear separation of concerns: `PlayerInventoryManager` handles only inventory operations and persistence, `ItemCatalog` manages only data loading and lookups, and `ItemDef` serves purely as a data container. Each class has a single, well-defined purpose that can be understood and tested in isolation.

The **Open/Closed Principle** is achieved through the data-driven catalog approach—new items can be added through JSON configuration without modifying existing code, and the extensible `StatsPayload` dictionary allows for arbitrary item properties without breaking the core structure. The system is closed for modification (core inventory logic remains stable) but open for extension (new item types, effects, and categories can be added declaratively).

For the **Dependency Inversion Principle**, all components depend on abstractions rather than concrete implementations. The `PlayerInventoryManager` depends on the `ItemCatalog.Instance` interface rather than directly accessing JSON files, and future UI components will depend on the manager's public API and signals rather than internal data structures. This abstraction layer means the underlying storage format could change from JSON to a database without affecting dependent code.

The **Interface Segregation Principle** is applied through focused public APIs—the inventory manager exposes only the methods needed for each use case (coins, unlocks, equipment) rather than a monolithic interface, and the catalog provides targeted lookup methods (`Get`, `GetByCategory`, `Exists`) rather than exposing the internal dictionary structure.

The implementation leverages several key design patterns for robustness and clarity. The **Singleton** pattern ensures the `ItemCatalog` provides a single, globally accessible source of truth for item definitions, preventing data duplication and ensuring consistency across the application. The **Observer** pattern, implemented through Godot's signal system, allows UI components to reactively update when inventory state changes—`CoinsChanged`, `InventoryChanged`, and `LoadoutChanged` signals enable loose coupling between the data layer and presentation layer.

The **Strategy** pattern will be employed for item behaviors through future `IPassiveEffect` and `IActiveAbility` interfaces, allowing each item type to define its own activation logic while sharing common equipment mechanics. The **Template Method** pattern is inherited from `BaseStatManager<T>`, providing consistent save/load behavior while allowing inventory-specific initialization and change handling.

## Rationale and Risk

Alternative approaches were considered and rejected for specific reasons. A monolithic "GameManager" approach was dismissed as it would violate the Single Responsibility Principle and create a god object. Hard-coding item properties in C# classes was rejected because it would require recompilation for balance changes and limit designer autonomy. Using separate files for each item was also rejected as it would fragment the data model and complicate batch operations.

The most significant architectural alternative—implementing `IInventoryItem` on weapon scenes—was initially pursued but ultimately rejected after recognizing the inappropriate coupling it would create. This decision reinforces the importance of maintaining clear boundaries between data management and scene behavior in game architecture.

The chosen approach carries minimal risk because it builds upon the proven `BaseStatManager` pattern already successfully used for EXP and stats, ensuring consistent behavior with existing systems. The data schema is designed with forward compatibility in mind through the `SaveVersion` field and extensible JSON structure. The most significant risk is potential performance impact from JSON parsing, but this is mitigated by loading the catalog once at startup and caching all definitions in memory.

Backward compatibility is maintained by preserving existing `WeaponCreator` methods as facades over the new catalog system, ensuring no existing combat code requires modification. The gradual migration path allows for incremental rollout—core systems can be tested independently before UI integration begins.

The expected impact includes significantly improved designer workflow through data-driven content creation, enhanced player engagement through meaningful progression choices, and a more maintainable codebase that can scale with future content additions. The decoupled architecture also enables comprehensive unit testing of business logic without UI dependencies.

## Implementation: LoadoutSpawner System

### Translation Layer Architecture

The final piece of the Items & Shop System is the `LoadoutSpawner`, which serves as the critical translation layer between shop equipment data and actual dungeon gameplay objects. This component embodies the architectural principle of clean separation between the inventory system (data management) and combat system (scene behavior) while providing seamless integration for the player experience.

The `LoadoutSpawner` operates as an autoload singleton that reads equipped item IDs from `PlayerInventoryManager`, retrieves item definitions from `ItemCatalog`, and instantiates the appropriate game objects using established creation patterns. This design maintains the strict boundaries identified in our architectural correction—inventory remains pure data operations while combat objects focus solely on gameplay mechanics.

### LoadoutData Structure

With the unified weapon strategy architecture, the `LoadoutData` structure is simplified to contain only `Weapon` objects across all categories:

- **MeleeWeapon**: `Weapon` with `WaitForAnimationUserStrategy` for melee combat
- **ProjectileWeapon**: `Weapon` with `ProjectileUseStrategy` for ranged attacks
- **PassiveWeapon**: `Weapon` with `PassiveEffectStrategy` for persistent modifications
- **ActiveWeapon**: `Weapon` with `ActiveAbilityStrategy` for cooldown-based abilities

This unified structure eliminates the need for separate `IPassiveEffect` and `IActiveAbility` management, as all categories follow the same `Weapon` object lifecycle. The strategy pattern encapsulates behavioral differences while maintaining consistent creation, equipping, and cleanup processes.

### Pattern Implementation

**Strategy Pattern for Equipment Behaviors:**
Passive effects and active abilities implement dedicated interfaces (`IPassiveEffect`, `IActiveAbility`) that define behavioral contracts without creating inappropriate coupling. Each item type encapsulates its own logic while sharing common equipment mechanics through the `LoadoutSpawner` orchestration.

**Template Method Pattern for Loadout Management:**
The `SpawnLoadout()`, `ApplyLoadout()`, and `RemoveLoadout()` methods follow a consistent template that can be extended for new equipment categories without modifying existing code. Each category follows the same pattern: retrieve equipped ID, validate through catalog, instantiate via appropriate factory, apply to character.

### Concrete Examples

**Passive Effects:**
The `HealthBoostEffect` demonstrates the passive effect pattern by implementing `IPassiveEffect` to modify player stats when equipped and restore them when removed. The effect stores original values for proper cleanup and integrates with the existing `HealthComponent` system without requiring modifications to core character code.

**Active Abilities:**
The `DashAbility` showcases the active ability pattern with proper cooldown management, input handling, and physics integration. The ability operates as an independent component that modifies character velocity while respecting existing movement systems and state machines.

### Integration Strategy

**Player Character Integration:**
The `Player` class was modified to use `LoadoutSpawner` in place of hardcoded weapon creation, with the integration occurring in `_Ready()` to ensure all autoload systems are available. A `ReloadLoadout()` method enables dynamic equipment changes during gameplay, while proper cleanup in `_ExitTree()` prevents resource leaks.

**Input System Integration:**
Active abilities integrate with Godot's input system through a centralized `HandleActiveAbilityInput()` method that routes input to appropriate abilities. This approach maintains clean separation between input handling and ability logic while providing extensible patterns for complex ability combinations.

### Validation and Error Handling

The system implements comprehensive error handling that ensures graceful degradation rather than catastrophic failure:

- **Missing Equipment**: Defaults to basic sword for melee weapons, no weapon for projectile
- **Invalid Catalog Entries**: Logs errors but continues with available equipment
- **Scene Loading Failures**: Falls back to previous equipment or default states
- **Interface Validation**: Verifies scene objects implement required interfaces before instantiation

This robust error handling maintains the principle that equipment failures should never prevent gameplay, ensuring a smooth player experience even with data corruption or missing assets.

### Performance Considerations

The `LoadoutSpawner` operates on a spawn-once, cache-forever model that aligns with typical gameplay patterns. Equipment objects are instantiated only when entering dungeons and persist for the duration of gameplay, avoiding repeated instantiation costs. The system leverages existing object pooling and scene management patterns established in the codebase.

### Testing and Validation

The implementation provides multiple validation points for the complete equipment pipeline:

1. **Shop Integration**: Verify equipped items persist correctly through `PlayerInventoryManager`
2. **Catalog Translation**: Confirm item IDs resolve to appropriate `ItemDef` objects
3. **Object Instantiation**: Validate scene loading and interface implementation
4. **Gameplay Integration**: Test passive effects and active abilities in actual dungeon scenarios
5. **State Management**: Verify proper cleanup and reloading behavior

### Future Extensibility

The `LoadoutSpawner` architecture establishes clear patterns for future equipment categories:

- **New Categories**: Add to `PlayerInventoryData`, extend `LoadoutData`, implement creation method
- **Complex Effects**: Combine multiple interfaces for items with both passive and active behaviors
- **Conditional Effects**: Use `StatsPayload` for data-driven effect configuration
- **Equipment Sets**: Extend `LoadoutData` to detect and apply set bonuses

The data-driven approach ensures that most new equipment can be added through JSON configuration alone, with code changes required only for fundamentally new behavior patterns.

This implementation completes the vision of a fully data-driven, extensible equipment system that bridges the gap between player progression choices and actual gameplay mechanics while maintaining architectural integrity and supporting future content expansion.

## Floating Damage Numbers System

### Context and Problem Statement

To enhance player feedback during combat, the game required a system to display "floating" damage numbers whenever an entity takes damage. This is a standard feature in many RPGs that provides immediate, clear visual confirmation of damage dealt, improving the player's sense of impact and helping them understand the effectiveness of their attacks.

The system needed to be efficient, visually appealing, and cleanly integrated into the existing combat and character architecture without introducing tight coupling or performance bottlenecks. Key challenges included where to trigger the effect, how to manage the lifecycle of the damage number nodes, and how to position them correctly in world space.

### The Solution: Autoload Manager with Scene Instantiation

The implemented solution uses a singleton `DamageNumberManager`, registered as a Godot Autoload, to act as a central factory for creating and displaying damage numbers. This manager preloads a `damage_number.tscn` scene, which defines the visual appearance and animation of a single damage number.

**System Components:**
- **`DamageNumberManager.cs` (Autoload Singleton):** A globally accessible node responsible for spawning damage numbers. It exposes a single public method: `Show(float amount, Vector2 worldPosition, Node2D world)`.
- **`damage_number.tscn` (Scene):** A `Node2D`-based scene containing a `Label` for the text and an `AnimationPlayer` to control its "show" animation (fading in, moving up, and fading out). At the end of the animation, it calls `queue_free()` to remove itself from the scene.
- **`Character.cs` (Integration Point):** The `OnDamaged` signal handler within the `Character` class was identified as the ideal integration point. When a character's `HealthComponent` emits the `Damaged` signal, this method calls the `DamageNumberManager` to display the damage value at the character's current position.

**Animation Approach:**
An `AnimationPlayer` was chosen over a `Tween` for defining the animation. While both are capable, `AnimationPlayer` provides a more powerful and designer-friendly workflow within the Godot editor. It allows for visual editing of keyframes for multiple properties (position, modulate for fade) and can easily trigger methods (like `queue_free()`) on completion, encapsulating the entire lifecycle of the damage number within its own scene and animation.

**Initial Implementation Decision - No Pooling:**
While object pooling is a common optimization for systems like this, the initial implementation forgoes it in favor of simplicity. For the current scale of the game, the performance overhead of instantiating a small scene for each damage event is negligible. The `queue_free()` call at the end of the animation ensures that nodes are cleaned up promptly, preventing memory leaks. This simpler approach reduces upfront complexity and can be easily refactored to include pooling later if performance metrics indicate it is necessary, following the principle of "make it work, then make it right, then make it fast."

### Design Principles Applied

- **Single Responsibility Principle (SRP):** The `DamageNumberManager` has one job: to spawn damage numbers. The `DamageNumber` scene has one job: to display itself and then die. The `Character` class's responsibility is only to report that it was damaged and where. This clear separation makes the system easy to understand and maintain.
- **Decoupling:** By using a singleton manager, the `Character` class does not need to know how a damage number is created, animated, or destroyed. It simply sends a "show this number here" request. This loose coupling means the entire visual implementation of damage numbers can be changed without ever touching the character or combat logic.
- **Centralized Logic:** The Autoload pattern provides a single, globally accessible point for a system-wide service. Any entity in the game that needs to display a damage number can do so through the same manager, ensuring visual and behavioral consistency.

### Integration Details

The `Character.OnDamaged()` method was modified to include the following logic:

```csharp
public void OnDamaged(float currentHP, DamageInfo damageInfo)
{
    // Existing logic for hurt state and knockback...
    ChangeMovementState(new HurtState());
    Velocity += damageInfo.Knockback;

    // New logic to show damage number
    if (this is Enemy)
    {
        _damageNumberManager.Show(damageInfo.Amount, Position, GetParent<Node2D>());
    }
}
```

The call is wrapped in an `if (this is Enemy)` check to ensure, for now, that only enemies display damage numbers. The manager is fetched once in `_Ready()` and cached for efficiency. It passes the damage amount, the character's local `Position`, and its `Parent` node to the manager. The manager then adds the damage number scene as a child of the character's parent, ensuring it exists in the same coordinate space.

This implementation provides a robust, maintainable, and easily extensible system for providing critical combat feedback to the player.

## Melee Weapon Expansion and Dynamic Animation System

### Context and Problem Statement

Following the successful implementation of the unified weapon strategy architecture, the system needed to expand beyond the basic sword and staff to provide meaningful player choice and progression. The original hardcoded weapon creation system limited content expansion and required code changes for each new weapon type. Additionally, the animation system was rigidly tied to fixed timing values, preventing weapon-specific attack phases and recovery periods from being properly represented.

The existing `WaitForAnimationUserStrategy` used a hardcoded `_attackDurationMs` value that didn't reflect the actual animation length, creating a disconnect between visual feedback and gameplay mechanics. This approach violated the principle of data-driven design and made weapon balancing difficult, as timing adjustments required both code changes and animation modifications.

### The Solution: Data-Driven Weapon Creation with Dynamic Animation Timing

The solution implements a comprehensive weapon expansion system that leverages the established catalog architecture while introducing sophisticated animation timing control. Three new melee weapons (dagger, spear, longsword) were created with distinct characteristics, and the animation system was enhanced to support both manual attack phase timing and dynamic animation length detection.

**Weapon Creation Architecture:**
Each new weapon follows the established pattern of scene-based creation with catalog-driven configuration. The `WeaponCreator.CreateFromCatalog()` method was enhanced to read weapon-specific properties from the `StatsPayload`, including damage, attack duration, and knockback values. This approach maintains the **Single Responsibility Principle** by keeping weapon definitions as pure data while allowing complex behavioral configuration.

**Animation System Enhancement:**
The `WaitForAnimationUserStrategy` was modified to support two timing approaches: manual attack phase specification for precise control and dynamic animation length detection for automatic adaptation. This dual approach provides flexibility for different weapon types while maintaining backward compatibility.

### Design Principles Applied

**Open/Closed Principle (OCP):**
The weapon creation system is closed for modification but open for extension. New weapons can be added entirely through JSON configuration without modifying existing code. The `WeaponCreator.CreateFromCatalog()` method handles all weapon types uniformly, while the `StatsPayload` dictionary allows arbitrary weapon properties without breaking the core structure.

**Dependency Inversion Principle (DIP):**
The system depends on abstractions rather than concrete implementations. The `WeaponCreator` depends on the `ItemCatalog.Instance` interface rather than hardcoded weapon types, and the animation strategy depends on the `AnimationPlayer` interface rather than specific animation implementations.

**Interface Segregation Principle (ISP):**
The weapon system exposes focused interfaces for different concerns. The `Weapon` class provides separate methods for animation control (`GetAnimationPlayer()`), timing (`GetDurationMs()`), and combat (`GetHitbox()`), allowing clients to depend only on the interfaces they need.

### Concrete Implementation Details

**Weapon Scene Structure:**
Each weapon scene maintains the established pattern from `sword.tscn`:
```
Weapon (Node2D)
├── Node2D (rotating container)
│   ├── Sprite2D (weapon sprite)
│   └── Hitbox (Area2D with collision)
├── AnimationPlayer (attack animations)
└── SlashSprite (visual effects)
```

This consistent structure enables the **Template Method Pattern** where all weapons follow the same creation and initialization process while allowing customization through scene-specific assets and animations.

**Catalog-Driven Configuration:**
Weapon properties are defined in `items_catalog.json` with balanced progression:
```json
{
  "Id": "dagger",
  "Category": "melee",
  "StatsPayload": {
    "damage": 15,
    "attackDuration": 200,
    "knockback": 150
  }
}
```

This data-driven approach enables rapid iteration and balance adjustments without code changes, supporting the **Open/Closed Principle**.

**Dynamic Animation Timing:**
The enhanced `WaitForAnimationUserStrategy` supports both approaches:
```csharp
// Manual attack phase timing (current implementation)
await Task.Delay(_weapon.GetDurationMs()); // From catalog

// Dynamic animation detection (alternative approach)
Animation animation = animationPlayer.GetAnimation(animName);
int durationMs = (int)(animation.Length * 1000);
await Task.Delay(durationMs);
```

This flexibility allows weapons to have different attack phases while maintaining consistent visual feedback.

### Alternative Approaches Considered

**Separate Animation Systems per Weapon Type:**
Rejected because it would violate the **DRY Principle** and create maintenance overhead. The unified strategy approach leverages existing infrastructure while allowing customization through data.

**Hardcoded Weapon Properties:**
Rejected because it would require code changes for balance adjustments and violate the **Open/Closed Principle**. The catalog-driven approach enables designer autonomy and rapid iteration.

**Animation-Driven Timing Only:**
Rejected because it would prevent precise control over attack phases. The dual approach provides flexibility for different weapon characteristics while maintaining consistency.

### Implementation Benefits

**Scalability:**
New weapons can be added through JSON configuration alone, supporting rapid content expansion. The system can accommodate arbitrary weapon properties through the extensible `StatsPayload` structure.

**Balance Flexibility:**
Weapon characteristics can be adjusted without code changes, enabling rapid iteration and fine-tuning. The separation of visual timing from gameplay timing allows for sophisticated weapon design.

**Maintainability:**
The consistent weapon creation pattern reduces code duplication and simplifies testing. The unified animation system ensures consistent behavior across all weapon types.

**Performance:**
Scene-based weapon creation leverages Godot's efficient resource management, while the catalog caching ensures minimal runtime overhead.

### Risk Mitigation

**Backward Compatibility:**
Existing weapon creation methods (`CreateSword()`, `CreateStaff()`) are preserved as facades over the new catalog system, ensuring no breaking changes to existing code.

**Error Handling:**
The system includes comprehensive validation for missing assets, invalid catalog entries, and animation failures, ensuring graceful degradation rather than catastrophic failure.

**Testing Strategy:**
The data-driven approach enables comprehensive unit testing of weapon creation and balance logic without requiring scene instantiation, while integration testing validates the complete weapon pipeline.

### Future Extensibility

The established patterns support several future enhancements:
- **Weapon Modifiers**: Additional properties in `StatsPayload` for elemental effects, status conditions, or special abilities
- **Animation Variants**: Multiple attack animations per weapon with conditional selection based on context
- **Weapon Combinations**: Systems for dual-wielding or weapon switching with smooth transitions
- **Progressive Unlocks**: Tiered weapon systems with gradual power scaling

This implementation demonstrates how architectural principles can guide practical design decisions, resulting in a system that is both powerful and maintainable while supporting rapid content development and iteration.

## Status System and DamageType Architecture

### Context and Problem Statement

Following the successful implementation of the unified weapon strategy architecture and melee weapon expansion, the system needed to support sophisticated combat mechanics that go beyond simple damage dealing. The existing damage system only supported generic damage types, limiting the ability to create meaningful weapon differentiation and strategic depth. Players had no way to apply status effects like burning damage over time or movement speed reduction, which are essential for creating diverse and engaging combat scenarios.

The original `DamageInfo` struct contained only basic damage amount and knockback information, making it impossible to distinguish between different attack types or apply appropriate visual and gameplay effects. This limitation prevented the implementation of elemental weapons (fire staff, ice staff) and status-based combat mechanics that would significantly enhance player engagement and strategic depth.

Additionally, the existing `Character` class had no mechanism for managing persistent effects or status conditions, making it impossible to implement damage-over-time effects, movement modifications, or other temporary character state changes. This gap in the architecture prevented the creation of sophisticated combat mechanics that are essential for modern action RPGs.

### The Solution: Comprehensive Status System with DamageType Integration

The solution implements a complete status system architecture that integrates seamlessly with the existing damage and character systems while providing the foundation for sophisticated combat mechanics. The implementation consists of three key components: a flexible `Status` base class, specialized status implementations (`BurnStatus`, `SlowStatus`), and an enhanced damage system with `DamageType` enumeration.

**Status System Architecture:**
The status system follows the established pattern from the Class Diagram, implementing a `Status` base class with lifecycle methods (`OnApply()`, `OnRemove()`, `Update()`, `HasProperty()`) that integrate cleanly with the existing `Character` class. Each status type encapsulates its own behavior while sharing common lifecycle management through the base class.

**DamageType Integration:**
The `DamageType` enum provides a foundation for elemental combat mechanics, enabling different damage types (Fire, Ice, Lightning, Physical) that can trigger appropriate visual effects, sound feedback, and automatic status application. This integration creates a natural connection between weapon types and their associated effects.

### Design Principles Applied

**Single Responsibility Principle (SRP):**
Each component has a single, well-defined responsibility. The `Status` base class handles only status lifecycle management, `BurnStatus` manages only damage-over-time effects, and `SlowStatus` manages only movement speed modifications. The `DamageType` enum serves purely as a classification mechanism, while `DamageInfo` acts as an immutable data carrier.

**Open/Closed Principle (OCP):**
The status system is closed for modification but open for extension. New status types can be added by inheriting from `Status` without modifying existing code. The `DamageType` enum can be extended with new types without affecting existing damage handling logic. The `Character` class's status management methods work with any `Status` subclass through polymorphism.

**Liskov Substitution Principle (LSP):**
All status implementations can be used interchangeably through the `Status` base class interface. The `Character.AddStatus()` method accepts any `Status` subclass and treats them uniformly, while each status type provides its own specific behavior through virtual method overrides.

**Interface Segregation Principle (ISP):**
The status system provides focused interfaces for different concerns. The `Status` base class exposes only the methods needed for lifecycle management, while the `Character` class provides separate methods for status addition, removal, and updating. The `DamageInfo` struct contains only the data needed for damage transmission.

**Dependency Inversion Principle (DIP):**
The system depends on abstractions rather than concrete implementations. The `Character` class depends on the `Status` base class interface rather than specific status types, and the damage system depends on the `DamageType` enum rather than hardcoded damage type logic.

### Concrete Implementation Details

**Status Base Class Design:**
```csharp
public abstract class Status
{
    protected Character _target;
    protected float _duration;
    protected float _elapsedTime;
    protected bool _isActive;

    public virtual void OnApply(Character target) { /* Setup */ }
    public virtual void OnRemove() { /* Cleanup */ }
    public virtual void Update(float delta) { /* Per-frame logic */ }
    public virtual bool HasProperty(string property) { return false; }
}
```

This design follows the **Template Method Pattern**, providing a consistent lifecycle while allowing subclasses to customize behavior through virtual method overrides. The base class handles common concerns like duration tracking and target management, while subclasses focus on their specific effects.

**BurnStatus Implementation:**
```csharp
public class BurnStatus : Status
{
    private float _damagePerSecond;
    private float _damageAccumulator;
    private Character _attacker;

    public override void Update(float delta)
    {
        base.Update(delta);
        _damageAccumulator += _damagePerSecond * delta;
        if (_damageAccumulator >= _damageInterval)
        {
            DealBurnDamage();
            _damageAccumulator = 0;
        }
    }
}
```

This implementation demonstrates the **Strategy Pattern**, where `BurnStatus` encapsulates a specific damage-over-time strategy while sharing common status management through the base class. The status automatically applies damage at regular intervals and cleans up when its duration expires.

**SlowStatus Implementation:**
```csharp
public class SlowStatus : Status
{
    private float _slowFactor;
    private float _originalMaxSpeed;
    private float _originalAcceleration;

    public override void OnApply(Character target)
    {
        base.OnApply(target);
        StoreOriginalValues();
        ApplySlowEffect();
    }

    public override void OnRemove()
    {
        RestoreOriginalValues();
        base.OnRemove();
    }
}
```

This implementation follows the **Memento Pattern**, storing the original character state and restoring it when the status expires. The status modifies character movement properties through the public `MaxSpeed` and `Acceleration` properties, ensuring clean integration with existing movement systems.

**Character Integration:**
```csharp
public class Character : CharacterBody2D
{
    private List<Status> _statuses = new List<Status>();

    public void AddStatus(Status status)
    {
        if (status != null)
        {
            status.OnApply(this);
            _statuses.Add(status);
        }
    }

    private void UpdateStatuses(float delta)
    {
        for (int i = _statuses.Count - 1; i >= 0; i--)
        {
            var status = _statuses[i];
            if (status != null)
            {
                status.Update(delta);
            }
        }
    }
}
```

This integration demonstrates the **Observer Pattern**, where the `Character` class maintains a collection of status objects and updates them each frame. The reverse iteration ensures safe removal of expired statuses during the update loop.

**DamageType and DamageInfo Enhancement:**
```csharp
public enum DamageType
{
    Generic, Melee, Projectile, Fire, Ice, Lightning, Physical
}

public readonly struct DamageInfo
{
    public readonly float Amount;
    public readonly Vector2 Knockback;
    public readonly Character Attacker;
    public readonly DamageType DamageType;

    public DamageInfo(float amount, Vector2 knockback, Character attacker = null,
                     DamageType damageType = DamageType.Generic)
    {
        Amount = amount;
        Knockback = knockback;
        Attacker = attacker;
        DamageType = damageType;
    }
}
```

### Alternative Approaches Considered

**Component-Based Status System:**
Rejected because it would require significant refactoring of the existing `Character` class and introduce additional complexity in component lifecycle management. The inheritance-based approach leverages existing patterns while providing the necessary flexibility.

**Event-Driven Status System:**
Rejected because it would create unnecessary complexity for simple status effects and require additional event infrastructure. The direct method calls provide clear, traceable behavior while maintaining simplicity.

**Hardcoded Status Types:**
Rejected because it would violate the **Open/Closed Principle** and require code changes for each new status type. The inheritance-based approach enables extensibility without modification.

**Separate Status Managers:**
Rejected because it would violate the **Single Responsibility Principle** by creating multiple managers for related concerns. The integrated approach keeps status management close to the character that owns the statuses.

### Implementation Benefits

**Extensibility:**
New status types can be added by creating new classes that inherit from `Status`, following the established pattern. The system supports arbitrary status effects through the virtual method override mechanism.

**Performance:**
The status system operates efficiently with minimal overhead. Status updates occur only when active, and the reverse iteration pattern ensures safe removal without additional memory allocation.

**Maintainability:**
The clear separation of concerns makes the system easy to understand and modify. Each status type encapsulates its own logic while sharing common infrastructure through the base class.

**Integration:**
The status system integrates seamlessly with existing systems. Status effects work with the established damage, movement, and character systems without requiring modifications to core infrastructure.

### Risk Mitigation

**Backward Compatibility:**
The enhanced `DamageInfo` constructor maintains backward compatibility through optional parameters, ensuring existing code continues to work without modification.

**Error Handling:**
The status system includes comprehensive null checking and validation to prevent crashes from invalid status objects or missing targets.

**Memory Management:**
Status objects are properly cleaned up when their duration expires or when the character is destroyed, preventing memory leaks and resource accumulation.

**Testing Strategy:**
The inheritance-based approach enables comprehensive unit testing of individual status types without requiring full character instantiation, while integration testing validates the complete status pipeline.

### Future Extensibility

The established patterns support several future enhancements:
- **Status Combinations**: Systems for managing multiple simultaneous status effects with interaction rules
- **Status Resistance**: Character properties that reduce status duration or effectiveness
- **Status Immunity**: Complete immunity to certain status types based on character properties
- **Status Amplification**: Systems that enhance status effects based on character stats or equipment
- **Visual Status Indicators**: UI systems that display active status effects to the player
- **Status-Based Triggers**: Passive abilities that activate based on status conditions

This implementation demonstrates how architectural principles can guide the design of complex game systems, resulting in a flexible and maintainable foundation for sophisticated combat mechanics while preserving the simplicity and performance characteristics essential for real-time gameplay.

## Enhanced Projectile and Status Effect System

### Context and Problem Statement

Following the successful implementation of the status system, the next logical step was to create weapons that could apply these statuses. The existing `ProjectileUseStrategy` and `Projectile` class were designed for basic projectiles that only dealt direct damage and knockback. They lacked the capability to carry and apply status effects, nor could they handle more complex behaviors like area-of-effect damage upon impact.

To implement weapons like a "Fire Staff" that applies a `BurnStatus` or an "Ice Staff" that applies a `SlowStatus`, the projectile architecture needed a significant extension. A new system was required that would allow projectiles to be carriers for various status effects, be configured in a data-driven way, and integrate cleanly with the existing `WeaponCreator` and unified strategy patterns.

### The Solution: An Extensible Projectile Architecture

The solution extends the existing projectile and weapon systems by introducing a new hierarchy for projectiles and a more sophisticated use strategy. This new architecture is designed for extensibility and reusability, allowing for the easy creation of new projectile-based weapons with unique effects.

**System Components:**

1.  **`BaseProjectile` (Abstract Class):** A new abstract base class, `BaseProjectile`, was created to serve as the foundation for all projectiles capable of applying status effects. It extends the original `Projectile` class and adds a mechanism to carry and apply a `Status` object to targets it collides with. It also includes logic for applying area-of-effect damage by querying for all bodies within a specified radius upon impact.

2.  **`FireProjectile` & `SlowProjectile` (Concrete Classes):** These classes inherit from `BaseProjectile`. Each is configured to apply a specific status effect. For example, `FireProjectile` is hard-coded to apply `BurnStatus`, and `SlowProjectile` applies `SlowStatus`. This encapsulates the specific effect logic within the projectile itself.

3.  **`EnhancedProjectileUseStrategy` (Reusable Strategy):** A new `IUseStrategy`, the `EnhancedProjectileUseStrategy`, was created to replace the original `ProjectileUseStrategy` for advanced weapons. This strategy is designed to work with any `BaseProjectile`. It takes a `BaseProjectile` instance in its constructor (typically a pre-configured scene instance) and handles the logic of aiming, duplicating, and firing the projectile. It correctly initializes the projectile with a reference to the wielder, allowing for proper damage attribution and status application.

4.  **Data-Driven Configuration:** The `items_catalog.json` was updated to support the new projectile weapons. Entries for projectile weapons now include a `projectileScenePath` which points to the specific projectile scene to be used (e.g., `fire_projectile.tscn`). This allows the `WeaponCreator` to dynamically load the correct projectile for each weapon.

    ```json
    {
      "Id": "fire_staff",
      "Category": "projectile",
      "Title": "Fire Staff",
      // ...
      "StatsPayload": {
        "projectileScenePath": "res://active/characters/Weapons/Projectiles/fire_projectile.tscn",
        "damage": 25,
        "projectileSpeed": 250,
        // ...
      }
    }
    ```

**Integration with `WeaponCreator`:**

The `WeaponCreator.CreateFromCatalog()` method was updated to handle the `projectile` category. When creating a projectile weapon, it now:
1.  Loads the weapon scene (e.g., `fire_staff.tscn`).
2.  Loads the projectile scene specified by `projectileScenePath` from the item's `StatsPayload`.
3.  Instantiates the projectile scene to get a `BaseProjectile` object.
4.  Creates an instance of `EnhancedProjectileUseStrategy`, passing the `BaseProjectile` instance to its constructor.
5.  Assigns the new strategy to the created weapon.

This flow ensures that weapons are correctly configured with a strategy that can fire the appropriate, status-applying projectile, all driven by the data in `items_catalog.json`.

### Design Principles Applied

-   **Open/Closed Principle (OCP):** The system is open to extension. To create a new projectile weapon (e.g., a lightning staff that paralyzes), one only needs to create a new `ParalyzeStatus` class, a `LightningProjectile` class that applies it, a corresponding scene, and add a new entry to `items_catalog.json`. No modifications are needed to `WeaponCreator` or `EnhancedProjectileUseStrategy`.

-   **Single Responsibility Principle (SRP):** Responsibilities are clearly separated.
    -   `BaseProjectile` and its subclasses are responsible for their own behavior upon impact (applying damage, status, area effects).
    -   `EnhancedProjectileUseStrategy` is responsible only for the act of using the weapon (aiming and firing).
    -   `WeaponCreator` is responsible for assembling the weapon and its strategy based on data.
    -   `Status` classes are responsible for the logic of the status effect itself.

-   **Liskov Substitution Principle (LSP):** Any subclass of `BaseProjectile` can be used by the `EnhancedProjectileUseStrategy` without breaking it, as it relies on the common interface provided by the base class.

-   **Strategy Pattern:** The `EnhancedProjectileUseStrategy` encapsulates the algorithm for firing advanced projectiles, allowing the `Weapon` to remain agnostic about how it is used. This promotes a unified weapon architecture where changing a weapon's behavior is as simple as swapping its strategy.

This enhanced projectile system provides a robust and scalable foundation for creating a wide variety of engaging ranged weapons, deeply integrating the new status effect mechanics into the core combat loop.
