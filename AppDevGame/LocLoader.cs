using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace AppDevGame
{
    public class LocLoader
    {
        private Dictionary<string, string> _localizationDict;
        private string _currentLanguage;
        private const string LocalizationFolder = "Localization";

        public LocLoader()
        {
            _localizationDict = new Dictionary<string, string>();
        }

        // Load localization data from a JSON file
        public void LoadLocalization(string language, ContentManager content)
        {
            _currentLanguage = language;
            string filePath = Path.Combine(content.RootDirectory, LocalizationFolder, $"{language}.json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                _localizationDict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            }
        }

        public string GetString(string key)
        {
            return _localizationDict.ContainsKey(key) ? _localizationDict[key] : key;
        }

        public string GetCurrentLanguage()
        {
            return _currentLanguage;
        }

         public void ChangeLanguage(string language, ContentManager content)
        {
            LoadLocalization(language, content);

            // Update the text of all buttons and UI elements
            var mainApp = MainApp.GetInstance();
            mainApp.MainMenu.Setup();
            mainApp.SettingsMenu.Setup();
            mainApp.LanguageMenu.Setup();
            mainApp.SoundMenu.Setup();
            mainApp.ModMenu.Setup();
            mainApp.StartMenu.Setup();
            mainApp.SelectSaveSlotMenu.Setup();
            mainApp.LoadSaveMenu.Setup();
        }
    }
}
