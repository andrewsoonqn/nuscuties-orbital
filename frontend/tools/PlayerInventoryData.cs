using System.Collections.Generic;

namespace nuscutiesapp.tools
{
    public class PlayerInventoryData
    {
        public int Coins { get; set; } = 10000;
        public Dictionary<string, List<string>> UnlockedItems { get; set; } = new();
        public Dictionary<string, string> EquippedItems { get; set; } = new();
        public int SaveVersion { get; set; } = 1;

        public PlayerInventoryData()
        {
            UnlockedItems = new Dictionary<string, List<string>>
            {
                { "melee", new List<string> { "sword" } },
                { "projectile", new List<string>() },
                { "necklace_passive", new List<string>() },
                { "necklace_active", new List<string>() }
            };

            EquippedItems = new Dictionary<string, string>
            {
                { "melee", "sword" },
                { "projectile", null },
                { "necklace_passive", null },
                { "necklace_active", null }
            };
        }
    }
}