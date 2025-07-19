using Godot;
using nuscutiesapp.active.characters.DamageSystem;
using nuscutiesapp.active.characters.Weapons.UseStrategies;
using nuscutiesapp.tools;
using System.Collections.Generic;

namespace nuscutiesapp.active.characters.Weapons
{
    public class WeaponCreator
    {
        private static readonly Dictionary<string, Weapon.WeaponType> _categoryToWeaponType = new()
        {
            { "melee", Weapon.WeaponType.Sword },
            { "projectile", Weapon.WeaponType.Staff }
        };

        public static Weapon CreateFromCatalog(string itemId, Character wielder, DamageFunction damageFunction)
        {
            var itemDef = ItemCatalog.Instance?.Get(itemId);
            if (itemDef == null)
            {
                GD.PrintErr($"Item not found in catalog: {itemId}");
                return null;
            }

            if (string.IsNullOrEmpty(itemDef.ScenePath))
            {
                GD.PrintErr($"No scene path specified for item: {itemId}");
                return null;
            }

            var weapon = ResourceLoader.Load<PackedScene>(itemDef.ScenePath)?.Instantiate<Weapon>();
            if (weapon == null)
            {
                GD.PrintErr($"Failed to load weapon scene: {itemDef.ScenePath}");
                return null;
            }

            var statsPayload = itemDef.StatsPayload;

            switch (itemDef.Category)
            {
                case "melee":
                    var damage = GetStatValue(statsPayload, "damage", 200);
                    var attackDuration = GetStatValue(statsPayload, "attackDuration", 200);
                    var knockback = GetStatValue(statsPayload, "knockback", 200);
                    weapon.Initialize(wielder, damageFunction, knockback, attackDuration, new WaitForAnimationUserStrategy());
                    return weapon;

                case "projectile":
                    var projectileSpeed = GetStatValue(statsPayload, "projectileSpeed", 200);
                    var cooldown = GetStatValue(statsPayload, "cooldown", 750);
                    var projectileKnockback = GetStatValue(statsPayload, "knockback", 150);

                    Projectile projectile = ResourceLoader.
                        Load<PackedScene>(Paths.StaffProjectile).Instantiate<Projectile>();
                    projectile.InitializeHitbox(wielder, damageFunction, projectileKnockback);

                    weapon.Initialize(wielder, null, 0, cooldown, new ProjectileUseStrategy(projectile));
                    return weapon;

                default:
                    GD.PrintErr($"Unhandled weapon category: {itemDef.Category}");
                    return null;
            }
        }

        private static int GetStatValue(Dictionary<string, object> statsPayload, string key, int defaultValue)
        {
            if (statsPayload?.TryGetValue(key, out object value) == true)
            {
                if (value is System.Text.Json.JsonElement jsonElement && jsonElement.ValueKind == System.Text.Json.JsonValueKind.Number)
                {
                    return jsonElement.GetInt32();
                }
                if (value is int intValue)
                {
                    return intValue;
                }
            }
            return defaultValue;
        }
        public static Weapon CreateStaff(Character wielder, DamageFunction damageFunction)
        {
            var weapon = CreateFromCatalog("staff", wielder, damageFunction);
            if (weapon == null)
            {
                GD.PrintErr("Failed to create staff from catalog, falling back to hardcoded creation");
                Projectile projectile = ResourceLoader.
                    Load<PackedScene>(Paths.StaffProjectile).Instantiate<Projectile>();

                projectile.InitializeHitbox(wielder, damageFunction, 200);
                return Weapon.CreateWeapon(
                    Weapon.WeaponType.Staff,
                    wielder,
                    null,
                    0,
                    750,
                    new ProjectileUseStrategy(projectile)
                );
            }
            return weapon;
        }

        public static Weapon CreateSword(Character wielder, DamageFunction damageFunction)
        {
            var weapon = CreateFromCatalog("sword", wielder, damageFunction);
            if (weapon == null)
            {
                GD.PrintErr("Failed to create sword from catalog, falling back to hardcoded creation");
                return Weapon.CreateWeapon(
                    Weapon.WeaponType.Sword,
                    wielder,
                    damageFunction,
                    200,
                    250,
                    new WaitForAnimationUserStrategy()
                );
            }
            return weapon;
        }
    }
}
