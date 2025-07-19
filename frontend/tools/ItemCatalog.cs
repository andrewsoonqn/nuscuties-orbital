using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace nuscutiesapp.tools
{
    public partial class ItemCatalog : Node
    {
        private static ItemCatalog _instance;
        public static ItemCatalog Instance => _instance;

        private Dictionary<string, ItemDef> _items = new();
        private const string CatalogPath = "res://assets/data/items_catalog.json";

        public override void _Ready()
        {
            if (_instance == null)
            {
                _instance = this;
                LoadCatalog();
            }
        }

        private void LoadCatalog()
        {
            try
            {
                if (!FileAccess.FileExists(CatalogPath))
                {
                    GD.PrintErr($"Item catalog not found at {CatalogPath}");
                    return;
                }

                using var file = FileAccess.Open(CatalogPath, FileAccess.ModeFlags.Read);
                if (file == null)
                {
                    GD.PrintErr($"Failed to open item catalog at {CatalogPath}");
                    return;
                }

                string jsonContent = file.GetAsText();
                var items = JsonSerializer.Deserialize<ItemDef[]>(jsonContent);

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        _items[item.Id] = item;
                    }
                    GD.Print($"Loaded {_items.Count} items from catalog");
                }
            }
            catch (System.Exception e)
            {
                GD.PrintErr($"Error loading item catalog: {e.Message}");
            }
        }

        public ItemDef Get(string id)
        {
            _items.TryGetValue(id, out ItemDef item);
            return item;
        }

        public IEnumerable<ItemDef> GetByCategory(string category)
        {
            return _items.Values.Where(item => item.Category == category);
        }

        public IEnumerable<ItemDef> GetAll()
        {
            return _items.Values;
        }

        public bool Exists(string id)
        {
            return _items.ContainsKey(id);
        }
    }
}
