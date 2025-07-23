using Godot;
using nuscutiesapp.active.characters.DamageSystem;
using nuscutiesapp.active.characters.Weapons;
using nuscutiesapp.tools;
using System.Collections.Generic;

namespace nuscutiesapp.tools
{
    public partial class LoadoutSpawner : Node
    {
        private PlayerInventoryManager _inventoryManager;
        private ItemCatalog _itemCatalog;
        private DerivedStatCalculator _statCalculator;

        public override void _Ready()
        {
            _inventoryManager = GetNode<PlayerInventoryManager>("/root/PlayerInventoryManager");
            _itemCatalog = GetNode<ItemCatalog>("/root/ItemCatalog");
            _statCalculator = GetNode<DerivedStatCalculator>("/root/DerivedStatCalculator");
        }

        public class LoadoutData
        {
            public Weapon MeleeWeapon { get; set; }
            public Weapon ProjectileWeapon { get; set; }
            public Node UtilityItem { get; set; }
            public List<IPassiveEffect> PassiveEffects { get; set; } = new();
            public List<IActiveAbility> ActiveAbilities { get; set; } = new();
        }

        public LoadoutData SpawnLoadout(Character targetCharacter)
        {
            var loadout = new LoadoutData();

            loadout.MeleeWeapon = SpawnMeleeWeapon(targetCharacter);
            loadout.ProjectileWeapon = SpawnProjectileWeapon(targetCharacter);
            loadout.UtilityItem = SpawnUtilityItem(targetCharacter);
            loadout.PassiveEffects = SpawnPassiveEffects(targetCharacter);
            loadout.ActiveAbilities = SpawnActiveAbilities(targetCharacter);

            return loadout;
        }

        private Weapon SpawnMeleeWeapon(Character targetCharacter)
        {
            string equippedMeleeId = _inventoryManager.GetEquipped("melee");
            if (string.IsNullOrEmpty(equippedMeleeId))
            {
                return CreateDefaultMeleeWeapon(targetCharacter);
            }

            var weapon = WeaponCreator.CreateFromCatalog(equippedMeleeId, targetCharacter, (dmg) => dmg * _statCalculator.CalcAttackDamageMultiplier());

            if (weapon == null)
            {
                GD.PrintErr($"Failed to create melee weapon from catalog: {equippedMeleeId}, using default");
                return CreateDefaultMeleeWeapon(targetCharacter);
            }

            return weapon;
        }

        private Weapon SpawnProjectileWeapon(Character targetCharacter)
        {
            string equippedProjectileId = _inventoryManager.GetEquipped("projectile");
            if (string.IsNullOrEmpty(equippedProjectileId))
            {
                return null;
            }

            var weapon = WeaponCreator.CreateFromCatalog(equippedProjectileId, targetCharacter, (dmg) => dmg * _statCalculator.CalcAttackDamageMultiplier());

            if (weapon == null)
            {
                GD.PrintErr($"Failed to create projectile weapon from catalog: {equippedProjectileId}");
                return null;
            }

            return weapon;
        }

        private Node SpawnUtilityItem(Character targetCharacter)
        {
            string equippedUtilityId = _inventoryManager.GetEquipped("utility");
            if (string.IsNullOrEmpty(equippedUtilityId))
            {
                return null;
            }

            var itemDef = _itemCatalog.Get(equippedUtilityId);
            if (itemDef == null)
            {
                GD.PrintErr($"Utility item not found in catalog: {equippedUtilityId}");
                return null;
            }

            if (!string.IsNullOrEmpty(itemDef.ScenePath))
            {
                var scene = GD.Load<PackedScene>(itemDef.ScenePath);
                if (scene != null)
                {
                    return scene.Instantiate();
                }
            }

            GD.PrintErr($"Failed to load utility item scene: {itemDef.ScenePath}");
            return null;
        }

        private List<IPassiveEffect> SpawnPassiveEffects(Character targetCharacter)
        {
            var effects = new List<IPassiveEffect>();

            string equippedPassiveId = _inventoryManager.GetEquipped("necklace_passive");
            if (!string.IsNullOrEmpty(equippedPassiveId))
            {
                var effect = CreatePassiveEffect(equippedPassiveId, targetCharacter);
                if (effect != null)
                {
                    effects.Add(effect);
                }
            }

            return effects;
        }

        private List<IActiveAbility> SpawnActiveAbilities(Character targetCharacter)
        {
            var abilities = new List<IActiveAbility>();

            string equippedActiveId = _inventoryManager.GetEquipped("necklace_active");
            if (!string.IsNullOrEmpty(equippedActiveId))
            {
                var ability = CreateActiveAbility(equippedActiveId, targetCharacter);
                if (ability != null)
                {
                    abilities.Add(ability);
                }
            }

            return abilities;
        }

        private IPassiveEffect CreatePassiveEffect(string itemId, Character targetCharacter)
        {
            var itemDef = _itemCatalog.Get(itemId);
            if (itemDef == null)
            {
                GD.PrintErr($"Passive item not found in catalog: {itemId}");
                return null;
            }

            if (!string.IsNullOrEmpty(itemDef.ScenePath))
            {
                var scene = GD.Load<PackedScene>(itemDef.ScenePath);
                if (scene != null)
                {
                    var instance = scene.Instantiate();
                    if (instance is IPassiveEffect passiveEffect)
                    {
                        return passiveEffect;
                    }
                    else
                    {
                        GD.PrintErr($"Scene {itemDef.ScenePath} does not implement IPassiveEffect");
                        instance.QueueFree();
                    }
                }
            }

            return null;
        }

        private IActiveAbility CreateActiveAbility(string itemId, Character targetCharacter)
        {
            var itemDef = _itemCatalog.Get(itemId);
            if (itemDef == null)
            {
                GD.PrintErr($"Active item not found in catalog: {itemId}");
                return null;
            }

            if (!string.IsNullOrEmpty(itemDef.ScenePath))
            {
                var scene = GD.Load<PackedScene>(itemDef.ScenePath);
                if (scene != null)
                {
                    var instance = scene.Instantiate();
                    if (instance is IActiveAbility activeAbility)
                    {
                        if (instance.HasMethod("Initialize"))
                        {
                            instance.Call("Initialize", targetCharacter);
                        }
                        return activeAbility;
                    }
                    else
                    {
                        GD.PrintErr($"Scene {itemDef.ScenePath} does not implement IActiveAbility");
                        instance.QueueFree();
                    }
                }
            }

            return null;
        }

        private Weapon CreateDefaultMeleeWeapon(Character targetCharacter)
        {
            var damageFunction = new DamageFunction(() => _statCalculator.CalcTotalDamage());
            return WeaponCreator.CreateSword(targetCharacter, damageFunction);
        }

        public void ApplyLoadout(Character targetCharacter, LoadoutData loadout)
        {
            if (loadout.PassiveEffects != null)
            {
                foreach (var effect in loadout.PassiveEffects)
                {
                    effect.ApplyEffect(targetCharacter);
                }
            }

            if (loadout.ActiveAbilities != null)
            {
                foreach (var ability in loadout.ActiveAbilities)
                {
                    if (ability is Node abilityNode)
                    {
                        targetCharacter.AddChild(abilityNode);
                    }
                }
            }
        }

        public void RemoveLoadout(Character targetCharacter, LoadoutData loadout)
        {
            if (loadout.PassiveEffects != null)
            {
                foreach (var effect in loadout.PassiveEffects)
                {
                    effect.RemoveEffect(targetCharacter);
                }
            }

            if (loadout.ActiveAbilities != null)
            {
                foreach (var ability in loadout.ActiveAbilities)
                {
                    if (ability is Node abilityNode)
                    {
                        abilityNode.QueueFree();
                    }
                }
            }

            loadout.UtilityItem?.QueueFree();
        }
    }
}