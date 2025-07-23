using Godot;

namespace nuscutiesapp.tools
{
    public interface IActiveAbility
    {
        bool Activate();
        bool IsOnCooldown();
        float GetCooldownRemaining();
    }
}