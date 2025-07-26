# Slow Effect Refactor, Blue Modulation, and Bugfixes (July 2025)

## Context
The slow effect in the game was originally implemented to reduce a character's speed and tint them blue. However, several issues were discovered:

- The slow effect's duration was not always correct (sometimes only 3 seconds instead of the intended 8 seconds).
- The blue modulation (color tint) was not persistent: it could be overridden by other effects (like the red damage flash) and would not always be restored immediately.
- The blue modulation was previously reapplied only on status effect ticks (every 1 second), which left gaps where the character was not blue.

## Problem
1. **Duration Bug:**
   - The slow effect's `_duration` was set in `_Ready()` or in a custom `Apply()` method, but the base `Apply()` method (from `StatusEffect`) was called before `_duration` was set, so `_remainingDuration` was initialized with the wrong value (default 3.0f).
   - This caused the effect to expire early, calling `OnRemoved()` and restoring speed and color before the intended duration.

2. **Blue Modulation Not Persistent:**
   - The blue color was set in `OnApplied()` and sometimes on ticks, but could be overridden by other effects (e.g., the red flash from damage in `HealthComponent`).
   - If the effect was removed between ticks, the character could remain non-blue until the next tick or until the effect was reapplied.

## Investigation
- Traced the order of operations in `StatusEffectManager`, `StatusEffect`, and `SlowStatusEffect`.
- Confirmed that `Apply()` was called before `_duration` was set, causing the duration bug.
- Confirmed that blue modulation was not being enforced every frame, so it could be lost between ticks.

## Solution
### 1. **Duration Bug Fix**
- Set `_duration = 8.0f` directly in the constructor of `SlowStatusEffect` (or before calling `base.Apply(target)`), ensuring the correct value is used when `_remainingDuration` is set.
- Removed redundant or conflicting duration assignments in `_Ready()` and custom `Apply()`.

### 2. **Persistent Blue Modulation**
- Overrode the `_Process(double delta)` method in `SlowStatusEffect`.
- On every frame, if the effect is active, forcibly set the character's `AnimatedSprite.Modulate` to blue.
- This ensures that any other color changes (e.g., from damage) are immediately overridden, and the character remains blue for the entire duration of the slow effect.
- On removal, restore the color to white.

## Final Implementation (as of July 2025)

```csharp
public partial class SlowStatusEffect : StatusEffect
{
    [Export] private float _speedMultiplier = 0.5f;
    private float _originalMaxSpeed;
    private bool _speedModified = false;
    public override string StatusName => "Slow";

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (_isActive && _target?.AnimatedSprite != null)
        {
            _target.AnimatedSprite.Modulate = new Color(0.7f, 0.7f, 1.0f);
        }
    }

    protected override void OnApplied()
    {
        if (_target != null)
        {
            _originalMaxSpeed = _target.MaxSpeed;
            float newMaxSpeed = _originalMaxSpeed * _speedMultiplier;
            _target.MaxSpeed = newMaxSpeed;
            _speedModified = true;
            GD.Print($"Slowed {_target.Name}: {_originalMaxSpeed} -> {newMaxSpeed}");
            if (_target.AnimatedSprite != null)
            {
                _target.AnimatedSprite.Modulate = new Color(0.7f, 0.7f, 1.0f);
            }
        }
    }

    protected override void OnRemoved()
    {
        if (_target != null && _speedModified)
        {
            _target.MaxSpeed = _originalMaxSpeed;
            GD.Print($"Restored speed for {_target.Name}: {_originalMaxSpeed}");
            if (_target.AnimatedSprite != null)
            {
                _target.AnimatedSprite.Modulate = new Color(1.0f, 1.0f, 1.0f);
            }
        }
    }
}
```

## Outcome
- The slow effect now always lasts the intended duration (8 seconds).
- The blue modulation is visually persistent for the entire effect duration, regardless of other effects.
- The code is robust against timing and color conflicts.

## Lessons Learned
- Always ensure effect durations are set before calling base logic that depends on them.
- For visual effects that must persist, use per-frame enforcement rather than relying on event/tick-based updates.
- Document the order of operations and side effects in status effect systems to avoid subtle bugs.
