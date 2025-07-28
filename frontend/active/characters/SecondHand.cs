using Godot;
using nuscutiesapp.active.characters.DamageSystem;
using nuscutiesapp.active.characters.Weapons;
using nuscutiesapp.active.characters.Weapons.UseStrategies;
using System.Threading.Tasks;

namespace nuscutiesapp.active.characters
{
    public partial class SecondHand : Node2D
    {
        private Weapon _duplicatedWeapon;
        private Character _owner;
        private Vector2 _offset = new Vector2(10, 0);

        public override void _Ready()
        {
            Visible = false;
        }

        public void Initialize(Character owner, Weapon originalWeapon)
        {
            _owner = owner;

            if (originalWeapon != null)
            {
                DuplicateWeapon(originalWeapon);
            }
        }

        private void DuplicateWeapon(Weapon originalWeapon)
        {
            if (_duplicatedWeapon != null)
            {
                _duplicatedWeapon.QueueFree();
            }

            _duplicatedWeapon = originalWeapon.Duplicate() as Weapon;

            if (_duplicatedWeapon != null)
            {
                AddChild(_duplicatedWeapon);
                _duplicatedWeapon.Position = _offset;
                _duplicatedWeapon.UseStrategy = originalWeapon.UseStrategy.Copy();
            }
        }

        public async void Attack()
        {
            if (_duplicatedWeapon != null)
            {
                await Task.Delay(100);
                _duplicatedWeapon.Use();
            }
        }

        public void UpdateWeaponRotation(Vector2 direction)
        {
            if (_duplicatedWeapon != null)
            {
                _duplicatedWeapon.Rotation = direction.Angle();

                if (direction.X < 0 && _duplicatedWeapon.Scale.Y > 0)
                {
                    _duplicatedWeapon.ApplyScale(new Vector2(1, -1));
                }
                else if (direction.X > 0 && _duplicatedWeapon.Scale.Y < 0)
                {
                    _duplicatedWeapon.ApplyScale(new Vector2(1, -1));
                }
            }
        }

        public void SetVisible(bool visible)
        {
            Visible = visible;
            if (_duplicatedWeapon != null)
            {
                _duplicatedWeapon.Visible = visible;
            }
        }

        public void UpdateWeapon(Weapon newWeapon)
        {
            DuplicateWeapon(newWeapon);
        }
    }
}