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
        
        public LoadWindowCommand(WindowManager windowManager, BaseWindow targetWindow) {
            this._windowManager = windowManager;
            this._targetWindow = targetWindow;
        }
            
        public void Execute() {
            this._windowManager.LoadWindow(this._targetWindow);
        }
    }
}