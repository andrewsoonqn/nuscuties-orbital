using Godot;

namespace nuscutiesapp.active.characters.StatusEffects.particles
{
    public partial class ForcefieldParticles : CpuParticles2D
    {
        public override void _Ready()
        {
            Emitting = true;
        }

        public void StartForcefield()
        {
            Emitting = true;
        }

        public void StopForcefield()
        {
            Emitting = false;
        }
    }
}