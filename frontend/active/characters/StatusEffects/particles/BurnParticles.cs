using Godot;

namespace nuscutiesapp.active.characters.StatusEffects.particles
{
    public partial class BurnParticles : CpuParticles2D
    {
        public override void _Ready()
        {
            Emitting = true;
        }

        public void StartBurning()
        {
            Emitting = true;
        }

        public void StopBurning()
        {
            Emitting = false;
        }
    }
}
