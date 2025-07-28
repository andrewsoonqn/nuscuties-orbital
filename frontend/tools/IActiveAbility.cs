using Godot;

namespace nuscutiesapp.tools
{
    public enum ActiveAbilityPhase
    {
        Ready,
        Loading,
        InProgress,
        Cooldown
    }

    public interface IActiveAbility
    {
        bool Activate();
        bool IsOnCooldown();
        float GetCooldownRemaining();
        ActiveAbilityPhase GetCurrentPhase();
        float GetPhaseCompletionPercentage();
    }
}