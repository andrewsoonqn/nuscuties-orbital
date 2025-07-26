using nuscutiesapp.active.characters.DamageSystem;
using nuscutiesapp.active.characters.StatusEffects;

namespace nuscutiesapp.active.characters.StatusEffects
{
    public partial class ForcefieldPlayerHealthComponent : PlayerHealthComponent
    {
        public override void TakeDamage(in DamageInfo damageInfo)
        {
            if (_owner?.StatusEffects != null)
            {
                var forcefield = _owner.StatusEffects.GetStatusEffect<ForcefieldStatusEffect>();
                if (forcefield != null && forcefield.IsActive)
                {
                    if (!forcefield.HasBlocked)
                    {
                        forcefield.BlockDamage();
                        return;
                    }
                }
            }

            base.TakeDamage(damageInfo);
        }
    }
}