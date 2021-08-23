using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RunescapeQuestsBackend
{
    public class Settings
    {
        public List<string> SavedUsers { get; set; }
        public string LastUser { get; set; }
        private readonly string settingsFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "/app.settings";
        public Settings()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (!File.Exists(settingsFilePath))
            {
                File.Create(settingsFilePath);
                return;
            }
            string settings = File.ReadAllText(settingsFilePath);
            if (settings.Length == 0) return;
            SettingsJson settingsJson = JsonSerializer.Deserialize<SettingsJson>(settings);
            SavedUsers = settingsJson.SavedUsers;
            LastUser = settingsJson.LastUser;
        }
        public void SaveSettings()
        {
            SettingsJson saveSettings = new();
            saveSettings.SavedUsers = SavedUsers;
            saveSettings.LastUser = LastUser;
            string jsonString = JsonSerializer.Serialize(saveSettings);
            File.WriteAllText(settingsFilePath, jsonString);
        }
    }
    class SettingsJson
    {
        public List<string> SavedUsers { get; set; }
        public string LastUser { get; set; }
    }
}
