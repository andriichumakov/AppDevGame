using AppDevGame;

namespace AppDevGame
{
    public class WindowManager
    {
        private static WindowManager _instance { get; private set; }
        public BaseWindow _window { get; private set; }

        private WindowManager() {
            this._window = null;
        }

        public static WindowManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new WindowManager();
            }
            return _instance;
        }

        public void LoadWindow(BaseWindow window) 
        {
            this.Clear();
            this._window = window;
            this._window.Setup();
        }

        public void Clear() 
        {
            this._window.Clear();
            this._window = null;
        }
    }
}