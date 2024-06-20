using System;
using System.IO;
using Microsoft.Xna.Framework;

namespace AppDevGame
{
    public interface ICommand
    {
        void Execute();
    }

    public class QuitCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("quitting");
            Environment.Exit(0);
        }
    }

    public class PrintCommand : ICommand
    {
        private string _message;

        public PrintCommand(string message)
        {
            _message = message;
        }

        public void Execute()
        {
            MainApp.Log(_message);
        }
    }

    public class LoadWindowCommand : ICommand
    {
        private WindowManager _windowManager;
        private BaseWindow _targetWindow;
        private bool _unpause;

        public LoadWindowCommand(WindowManager windowManager, BaseWindow targetWindow, bool unpause = false)
        {
            this._windowManager = windowManager;
            this._targetWindow = targetWindow;
            this._unpause = unpause;
        }

        public void Execute()
        {
            this._windowManager.LoadWindow(this._targetWindow);

            // Check if the target window is the main menu
            if (_targetWindow is MainMenu)
            {
                MainApp.GetInstance().PlayMainMenuMusic();
            }
            else if (_targetWindow is LevelWindow)
            {
                MainApp.GetInstance().PlayLevelMusic();
            }

            if (_unpause)
            {
                MainApp.GetInstance().TogglePause(); // Unpause the game if the flag is set
            }
        }
    }

    public class UnpauseCommand : ICommand
    {
        public void Execute()
        {
            MainApp.GetInstance().TogglePause();
            Console.WriteLine("Game unpaused");
        }
    }

    public class RestartLevelCommand : ICommand
    {
        private LevelWindow _level;

        public RestartLevelCommand(LevelWindow level)
        {
            _level = level;
        }

        public void Execute()
        {
            _level.Setup();
            MainApp.GetInstance().TogglePause();
        }
    }

    public class StartNewGameCommand : ICommand
    {
        private WindowManager _windowManager;
        private int _saveSlot;

        public StartNewGameCommand(WindowManager windowManager, int saveSlot)
        {
            _windowManager = windowManager;
            _saveSlot = saveSlot;
        }

        public void Execute()
        {
            MainApp.Log("trying to actually create a save file...");
            MainApp.Log("loading the level...");
            Level1 newLevel = new Level1(800, 600, 2372, 3063, MainApp.GetInstance()._imageLoader.GetResource("BackgroundLevel1"));
            MainApp.Log("loading the player...");
            Player newPlayer = new Player(newLevel, MainApp.GetInstance()._imageLoader.GetResource("character"), new Vector2(100, 100), MainApp.GetInstance().BackgroundTexture);
            MainApp.Log("setting the player to the level...");
            newLevel.SetPlayer(newPlayer);
            MainApp.Log("loading the window...");
            _windowManager.LoadWindow(newLevel);
            MainApp.Log("saving the state...");
            SaveLoadManager.SaveToDevice(newPlayer, _saveSlot);

            MainApp.GetInstance().PlayLevelMusic(); // Ensure level music is played
        }
    }

    public class LoadGameCommand : ICommand
    {
        private WindowManager _windowManager;
        private int _saveSlot;

        public LoadGameCommand(WindowManager windowManager, int saveSlot)
        {
            _windowManager = windowManager;
            _saveSlot = saveSlot;
        }

        public void Execute()
        {
            GameState gameState = SaveLoadManager.LoadFromDevice(_saveSlot);
            if (gameState != null)
            {
                Level1 loadedLevel = new Level1(800, 600, 2372, 3063, MainApp.GetInstance()._imageLoader.GetResource("BackgroundLevel1"));
                Player loadedPlayer = new Player(loadedLevel, MainApp.GetInstance()._imageLoader.GetResource("character"), gameState.playerPosition, MainApp.GetInstance().BackgroundTexture);
                loadedPlayer.SetHealth(gameState.playerHealth);
                loadedPlayer.SetCoins(gameState.coinsCollected);
                loadedPlayer.SetCurrentLevel(gameState.currentLevel);
                loadedLevel.SetPlayer(loadedPlayer);
                _windowManager.LoadWindow(loadedLevel);

                MainApp.GetInstance().PlayLevelMusic(); // Ensure level music is played
            }
        }
    }

    public class DeleteSaveCommand : ICommand
    {
        private int _saveSlot;

        public DeleteSaveCommand(int saveSlot)
        {
            _saveSlot = saveSlot;
        }

        public void Execute()
        {
            string saveFilePath = Path.Combine("saves", $"savegame_{_saveSlot}.json");
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
                MainApp.Log("Save file deleted.");
                SaveLoadManager.SaveSlotsEmpty[_saveSlot] = true;
            }
        }
    }

    public class ApplyLanguageCommand : ICommand
    {
        public void Execute()
        {
            MainApp mainApp = MainApp.GetInstance();
            string selectedLanguage = mainApp.LanguageMenu.GetSelectedLanguage();
            string currentLanguage = mainApp.LocLoader.GetCurrentLanguage();

            if (selectedLanguage != currentLanguage)
            {
                mainApp.LocLoader.ChangeLanguage(selectedLanguage, mainApp.Content);

                // Update the text of buttons and other UI elements to the selected language
                mainApp.MainMenu.SetupElements();
                mainApp.SettingsMenu.SetupElements();
                mainApp.LanguageMenu.SetupElements();
                mainApp.SoundMenu.SetupElements();
                mainApp.ModMenu.SetupElements();
                mainApp.StartMenu.SetupElements();
                mainApp.SelectSaveSlotMenu.SetupElements();
                mainApp.LoadSaveMenu.SetupElements();

                MainApp.Log("Language changed and UI updated.");
            }
            else
            {
                MainApp.Log("Selected language is the same as the current language. No changes made.");
            }
        }
    }

    public class ToggleAllModsCommand : ICommand
    {
        private ModMenu _modMenu;

        public ToggleAllModsCommand(ModMenu modMenu)
        {
            _modMenu = modMenu;
        }

        public void Execute()
        {
            _modMenu.ToggleAllMods(!_modMenu._toggleAll.IsToggled);
        }
    }

    public class ToggleModCommand : ICommand
    {
        private ModMenu _modMenu;
        private int _modIndex;

        public ToggleModCommand(ModMenu modMenu, int modIndex)
        {
            _modMenu = modMenu;
            _modIndex = modIndex;
        }

        public void Execute()
        {
            // Toggle the state of the specific mod checkbox
            var modStates = _modMenu.GetModStates();
            modStates[_modIndex] = !modStates[_modIndex];
        }
    }
}