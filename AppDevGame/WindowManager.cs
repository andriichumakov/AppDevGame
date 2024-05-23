using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using AppDevGame;

namespace AppDevGame
{
    public class WindowManager
    {
        private static WindowManager _instance;
        private BaseWindow _currentWindow;

        private WindowManager() { }

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
            _currentWindow?.Clear();
            _currentWindow = window;
            _currentWindow.Setup();
        }

        public void Update(GameTime gameTime)
        {
            _currentWindow?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentWindow?.Draw(spriteBatch);
        }
    }
}