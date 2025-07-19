using Godot;
using nuscutiesapp.active.characters.DamageSystem;
using nuscutiesapp.active.characters.Weapons.UseStrategies;
using nuscutiesapp.tools;
using System;
using System.Collections.Generic;

namespace nuscutiesapp.active.characters.Weapons
{
    public class WeaponCreator
    {
        public static Weapon CreateFromCatalog(string itemId, Character wielder, Func<float, float> damageMultiplier)
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
                    var damage = GetStatValue(statsPayload, "damage", 20);
                    var attackDuration = GetStatValue(statsPayload, "attackDuration", 200);
                    var knockback = GetStatValue(statsPayload, "knockback", 200);

                    var meleeDamageFunction = new DamageFunction(() => damageMultiplier(damage));

                    weapon.Initialize(wielder, meleeDamageFunction, knockback, attackDuration, new WaitForAnimationUserStrategy());
                    return weapon;

                case "projectile":
                    var projectileDamage = GetStatValue(statsPayload, "damage", 20);
                    var projectileKnockback = GetStatValue(statsPayload, "knockback", 150);
                    var cooldown = GetStatValue(statsPayload, "cooldown", 750);

                    var projectileDamageFunction = new DamageFunction(() => damageMultiplier(projectileDamage));
                    var useStrategy = new ProjectileUseStrategy(
                        itemDef.StatsPayload["projectileScenePath"].ToString(),
                        wielder,
                        projectileDamageFunction,
                        projectileKnockback
                    );

                    weapon.Initialize(wielder, null, 0, cooldown, useStrategy);
                    return weapon;

                default:
                    GD.PrintErr($"Unhandled weapon category: {itemDef.Category}");
                    return null;
            }
        }

        public static Weapon CreateSword(Character wielder, DamageFunction damageFunction)
        {
            var weapon = CreateFromCatalog("sword", wielder, (dmg) => dmg); // Pass a dummy multiplier
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
    }
}
