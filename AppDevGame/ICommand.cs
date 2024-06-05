using System;
using System.IO;
using System.Numerics;

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

        public LoadWindowCommand(WindowManager windowManager, BaseWindow targetWindow)
        {
            this._windowManager = windowManager;
            this._targetWindow = targetWindow;
        }

        public void Execute()
        {
            this._windowManager.LoadWindow(this._targetWindow);
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
            Player newPlayer = new Player(newLevel, MainApp.GetInstance()._imageLoader.GetResource("character"), new Vector2(100, 100));
            MainApp.Log("setting the player to the level...");
            newLevel.SetPlayer(newPlayer);
            MainApp.Log("loading the window...");
            _windowManager.LoadWindow(newLevel);
            MainApp.Log("saving the state...");
            SaveLoadManager.SaveToDevice(newPlayer, _saveSlot);
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
                Player loadedPlayer = new Player(loadedLevel, MainApp.GetInstance()._imageLoader.GetResource("character"), gameState.playerPosition);
                loadedPlayer.SetHealth(gameState.playerHealth);
                loadedPlayer.SetCoins(gameState.coinsCollected);
                loadedPlayer.SetCurrentLevel(gameState.currentLevel);
                loadedLevel.SetPlayer(loadedPlayer);
                _windowManager.LoadWindow(loadedLevel);
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
}