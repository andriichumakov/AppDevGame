using AppDevGame;

namespace AppDevGame
{
    public interface ICommand 
    {
        // represents a command that can be executed
        public void Execute();
    }

    public class QuitCommand: ICommand 
    {
        public void Execute() {
            Console.WriteLine("quitting");
            Environment.Exit(0);
        }
    }

    public class LoadWindowCommand: ICommand 
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