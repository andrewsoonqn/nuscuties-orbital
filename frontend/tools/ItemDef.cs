using System.Collections.Generic;

namespace nuscutiesapp.tools
{
    public class ItemDef
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string IconPath { get; set; }
        public string ScenePath { get; set; }
        public Dictionary<string, object> StatsPayload { get; set; } = new();
    }
}