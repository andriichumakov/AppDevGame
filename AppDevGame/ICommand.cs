using System;
using Microsoft.Xna.Framework.Audio;

namespace AppDevGame
{
    public interface ICommand 
    {
        void Execute();
    }

    public class QuitCommand : ICommand 
    {
        public void Execute() {
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

        public void Execute() {
            MainApp.Log(_message);
        }
    }

    public class LoadWindowCommand : ICommand 
    {
        private WindowManager _windowManager;
        private BaseWindow _targetWindow;
        private bool _unpause;

        public LoadWindowCommand(WindowManager windowManager, BaseWindow targetWindow, bool unpause = false) {
            this._windowManager = windowManager;
            this._targetWindow = targetWindow;
            this._unpause = unpause;
        }

        public void Execute() {
            this._windowManager.LoadWindow(this._targetWindow);
            if (_unpause) {
                MainApp.GetInstance().TogglePause(); // Unpause the game if the flag is set
            }
        }
    }

    public class UnpauseCommand : ICommand
    {
        public void Execute() 
        {
            // Example: toggle pause state
            // This should interface with your game's main state management to resume gameplay
            MainApp.GetInstance().TogglePause(); // You need to define this method according to your game's architecture
            
            // Optionally, close/hide the current pause menu
            // WindowManager.GetInstance().HideCurrentWindow(); // You might need to implement this method in WindowManager
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
            // Assuming there's a way to reset the level
            _level.Setup();
            MainApp.GetInstance().TogglePause();
        }
    }
}