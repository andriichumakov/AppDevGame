using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class WindowManager
    {
         private static WindowManager _instance;
        private BaseWindow _currentWindow;
        private BaseWindow _previousWindow;
        private WindowManager() { }
        public BaseWindow CurrentWindow => _currentWindow;

        public static WindowManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new WindowManager();
            }
            return _instance;
        }

        public BaseWindow PreviousWindow => _previousWindow;

        public BaseWindow GetCurrentWindow()
        {
            return _currentWindow;
        }

         public void SetCurrentWindow(BaseWindow window)
        {
            _currentWindow = window;
        }

        public void SetLevelWindow(LevelWindow levelWindow)
        {
            SetCurrentWindow(levelWindow);
        }


        public void LoadWindow(BaseWindow window)
        {
            _currentWindow?.Clear();
            _currentWindow = window;
            _currentWindow.Setup();
        }

        public void HideCurrentWindow()
        {
            if (_currentWindow != null)
            {
                _currentWindow.Hide();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_currentWindow != null)
            {
                _currentWindow?.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_currentWindow != null)
            {
                _currentWindow?.Draw(spriteBatch);
            }
        }
    }
}