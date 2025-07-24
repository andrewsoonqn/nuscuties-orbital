# Drop System and Base Number System Refactor

**Date**: January 2025
**Status**: Implemented
**Components**: Active Dungeon Gameplay, Visual Feedback System

## Context and Problem

The game needed:
1. **Reward drops** when enemies die (coins, health potions, buff potions, forcefield potions)
2. **Level-based coin scaling** using existing progression system
3. **Specialized visual feedback** for different reward types (damage, health gain, coin gain)
4. **Victory condition improvement** - players should actively complete levels rather than auto-ending

The existing system only had basic damage numbers with no drop rewards or specialized feedback.

## Design Decisions

### 1. Drop System Architecture (Phase 1)

#### **Decision**: Abstract Base Class Pattern
- **`BaseDropItem`** - Abstract class handling collision, pickup logic, and magnetic attraction
- **Specialized classes** inherit behavior: `CoinDrop`, `HealthPotion`, `BuffPotion`, `ForcefieldPotion`, `VictoryOrb`

**Rationale**:
- **Code Reuse** - All drops share collision detection, animation, and magnetic pickup behavior
- **Extensibility** - New drop types only need to implement `OnPickup(Player player)`
- **Consistency** - Uniform pickup experience across all drop types

#### **Decision**: Centralized Drop Management
- **`DropManager`** singleton handles spawning logic and probabilities
- **Probability-based system**: Coins (100%), Health (15%), Buff (8%), Forcefield (5%)
- **Scene loading** handled centrally with error fallbacks

**Rationale**:
- **Single Responsibility** - Drop spawning logic centralized
- **Easy tuning** - Probability constants easily adjustable
- **Performance** - Scene caching in dictionary for efficiency

#### **Decision**: Reuse Existing Systems
- **Level scaling** - Leveraged existing `PlayerInventoryManager.CalculateActiveDungeonEnemyCoinReward()`
- **Status effects** - Reused existing `StatusEffect` base class and `StatusEffectManager`
- **Event system** - Integrated with existing `ActiveDungeonEventManager`

**Benefits**:
- **No duplication** - Avoided reimplementing progression formulas
- **Consistency** - Coin rewards scale identically to other game systems
- **Reliability** - Leveraged tested status effect framework

### 2. Base Number System Refactor (Phase 2)

#### **Decision**: Extract Abstract Base Class
- **`BaseNumber`** - Abstract class with common animation and display logic
- **Specialized implementations**: `DamageNumber`, `HealthGainNumber`, `CoinGainNumber`
- **Template Method Pattern** - Subclasses define `GetTextColor()` and `FormatText()`

```csharp
public abstract partial class BaseNumber : Node2D
{
    protected abstract Color GetTextColor();
    protected abstract string FormatText(string value);

    public virtual void Show(string text)
    {
        _label.Text = FormatText(text);
        _label.Modulate = GetTextColor();
        _animationPlayer.Play("show");
    }
}
```

**Rationale**:
- **DRY Principle** - Eliminated code duplication between number types
- **Visual Consistency** - All numbers use same animation timing and behavior
- **Type Safety** - Specialized formatting prevents display errors

#### **Decision**: Unified Manager with Backward Compatibility
- **`BaseNumberManager`** - Handles all number types with specialized methods
- **`DamageNumberManager`** - Kept as legacy wrapper inheriting from base
- **Project.godot** - Updated autoload to use BaseNumberManager

**Benefits**:
- **Gradual Migration** - Existing code continues working
- **Centralized Management** - Single point for all number displays
- **Easy Extension** - Adding new number types requires minimal changes

#### **Decision**: Color-Coded Visual Language
- **Damage**: Red-orange (`1.0f, 0.4f, 0.2f`) - Danger/Loss
- **Health Gain**: Bright green (`0.2f, 1.0f, 0.3f`) - Healing/Restoration
- **Coin Gain**: Gold (`1.0f, 0.8f, 0.2f`) - Reward/Value

**Rationale**:
- **Instant Recognition** - Players immediately understand feedback type
- **Industry Standards** - Follows common game UI conventions
- **Accessibility** - High contrast colors for visibility

### 3. Victory Condition Enhancement

#### **Decision**: Victory Orb Collection
- **Last enemy** spawns `VictoryOrb` instead of normal drops
- **Player agency** - Must actively collect orb to complete level
- **UI feedback** - "Find the Victory Orb!" message replaces auto-completion

**Rationale**:
- **Player Satisfaction** - Active completion more engaging than auto-trigger
- **Clear Completion** - Visual indication of victory state
- **Reward Anticipation** - Final collection moment creates satisfaction peak

## Implementation Highlights

### **Reuse Patterns Applied**

1. **Inheritance Hierarchies**:
   ```
   BaseDropItem → CoinDrop, HealthPotion, BuffPotion, etc.
   BaseNumber → DamageNumber, HealthGainNumber, CoinGainNumber
   StatusEffect → BuffStatusEffect, ForcefieldStatusEffect
   ```

2. **System Integration**:
   - Leveraged existing `EnemyTracker` for last enemy detection
   - Reused `PlayerInventoryManager` coin scaling formulas
   - Integrated with existing `StatusEffectManager` architecture

3. **Scene Template Reuse**:
   - All number scenes share animation structure
   - All drop scenes share collision and node setup
   - Only styling and scripts differ between variants

### **Extensibility Examples**

Adding a new drop type:
```csharp
public partial class ExperienceOrb : BaseDropItem
{
    protected override void OnPickup(Player player)
    {
        var progressionManager = GetNode<ProgressionManager>("/root/ProgressionManager");
        progressionManager.AddExp(100);

        var numberManager = GetNode<BaseNumberManager>("/root/BaseNumberManager");
        numberManager.ShowExpGain(100, Position, GetParent<Node2D>());
    }
}
```

Adding a new number type:
```csharp
public partial class ExpGainNumber : BaseNumber
{
    protected override Color GetTextColor() => new Color(0.7f, 0.3f, 1.0f); // Purple
    protected override string FormatText(string value) => "+" + value + " XP";
}
```

## Performance Considerations

1. **Scene Caching** - DropManager loads scenes once in `_Ready()`
2. **Object Pooling Ready** - BaseNumber structure supports future pooling
3. **Event-Driven** - No polling, everything responds to enemy death events
4. **Minimal Allocations** - Reuses existing nodes and managers

## Testing and Validation

- **Drop Probabilities** - Verified balanced gameplay experience
- **Level Scaling** - Confirmed coin rewards match progression curve
- **Visual Feedback** - Tested color accessibility and readability
- **Performance** - No frame drops with multiple simultaneous drops
- **Backward Compatibility** - Existing damage numbers continue working

## Future Extensions

This architecture enables:
- **New drop types** (XP orbs, temporary weapons, keys)
- **New number types** (critical hits, combo multipliers, level ups)
- **Advanced effects** (screen shakes, particle systems, sound cues)
- **Drop conditions** (boss-only drops, streak bonuses, player level gates)

## Lessons Learned

1. **Extract Early** - Abstract base classes pay off quickly when adding variants
2. **Leverage Existing** - Reusing progression formulas ensured consistency
3. **Template Method** - Perfect pattern for shared behavior with customizable details
4. **Gradual Migration** - Keeping old interfaces during refactoring reduces risk
5. **Visual Language** - Consistent color coding significantly improves UX

This refactor demonstrates how thoughtful abstraction and system reuse can deliver rich features while maintaining clean, extensible code.
