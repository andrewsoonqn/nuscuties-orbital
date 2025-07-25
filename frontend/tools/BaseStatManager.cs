using Godot;
using System;
using System.Text.Json;

namespace nuscutiesapp.tools
{
    public abstract partial class BaseStatManager<TData> : Node where TData : class, new()
    {
        protected TData Data;
        protected abstract string BaseSaveFileName { get; }
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
                var dir = DirAccess.Open(GetUserDirectory());
                if (dir == null)
                {
                    DirAccess.MakeDirRecursiveAbsolute(GetUserDirectory());
                }
                using var file = FileAccess.Open(SaveFilePath, FileAccess.ModeFlags.Write);
                if (file != null)
                {
                    file.StoreString(jsonString);
                    GD.Print($"Saved data to {SaveFilePath}");
                }
                else
                {
                    GD.PrintErr($"Error saving to {SaveFilePath}: Could not open file for writing");
                }
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
                if (!FileAccess.FileExists(SaveFilePath))
                {
                    Data = new TData();
                    return;
                }

                using var file = FileAccess.Open(SaveFilePath, FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string jsonString = file.GetAsText();
                    Data = JsonSerializer.Deserialize<TData>(jsonString) ?? new TData();
                    GD.Print($"Loaded data from {SaveFilePath}");
                }
                else
                {
                    GD.PrintErr($"Error loading from {SaveFilePath}: Could not open file for reading");
                    Data = new TData();
                }
            }
            catch (Exception e)
            {
                GD.PrintErr($"Error loading from {SaveFilePath}: {e.Message}");
                Data = new TData();
            }
        }

        public void NotifyDataChanged()
        {
            OnDataChanged();
            SaveData();
        }

        public void ReloadData()
        {
            LoadData();
            InitializeDefaults();
        }

        private string GetUserDirectory()
        {
            var userManager = GetNode<UserManager>("/root/UserManager");
            string username = userManager?.GetCurrentUser() ?? "DefaultUser";
            return $"user://saves/{username}";
        }

        private string GetUserFilePath()
        {
            return $"{GetUserDirectory()}/{BaseSaveFileName}";
        }

        protected string SaveFilePath => GetUserFilePath();

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
