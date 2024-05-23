using System;

using AppDevGame;

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
            MainApp.Log("Quit");
            System.Environment.Exit(0);
        }
    }

    public class ConsoleLogCommand : ICommand
    {
        public void Execute()
        {
            MainApp.Log("We're doing something!");
        }
    }

    public class LoadWindowCommand : ICommand
    {
        private BaseWindow _targetWindow;

        public LoadWindowCommand(BaseWindow targetWindow)
        {
            _targetWindow = targetWindow;
        }

        public void Execute()
        {
            WindowManager.GetInstance().LoadWindow(_targetWindow);
        }
    }
}