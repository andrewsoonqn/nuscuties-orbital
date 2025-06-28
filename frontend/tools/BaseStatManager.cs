using Godot;
using System;
using System.IO;
using System.Text.Json;

namespace nuscutiesapp.tools
{
    public abstract partial class BaseStatManager<TData> : Node where TData : class, new()
    {
        protected TData Data;
        protected abstract string SaveFilePath { get; }
        protected abstract void OnDataChanged();

        public override void _Ready()
        {
            LoadData();
            InitializeDefaults();
        }

        protected virtual void InitializeDefaults() { }

        protected void SaveData()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(Data, options);

            try
            {
                var dirPath = Path.GetDirectoryName(SaveFilePath);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                File.WriteAllText(SaveFilePath, jsonString);
                GD.Print($"Saved data to {SaveFilePath}");
            }
            catch (Exception e)
            {
                GD.PrintErr($"Error saving to {SaveFilePath}: {e.Message}");
            }
        }

        protected void LoadData()
        {
            try
            {
                if (!File.Exists(SaveFilePath))
                {
                    Data = new TData();
                    return;
                }

                string jsonString = File.ReadAllText(SaveFilePath);
                Data = JsonSerializer.Deserialize<TData>(jsonString) ?? new TData();
                GD.Print($"Loaded data from {SaveFilePath}");
            }
            catch (Exception e)
            {
                GD.PrintErr($"Error loading from {SaveFilePath}: {e.Message}");
                Data = new TData();
            }
        }

        protected void NotifyDataChanged()
        {
            OnDataChanged();
            SaveData();
        }

        // Save stats automatically when game window closed
        public override void _Notification(int what)
        {
            if (what == NotificationWMCloseRequest)
            {
                SaveData();
            }
        }
    }
}