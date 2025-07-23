using Godot;

namespace nuscutiesapp.tools
{
    public interface IPassiveEffect
    {
        void ApplyEffect(Node player);
        void RemoveEffect(Node player);
    }
}