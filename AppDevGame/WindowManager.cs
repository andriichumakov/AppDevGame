using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using AppDevGame;

namespace AppDevGame
{

    public abstract class BaseWindow 
    {
        private int _width;
        private int _height;
        private Texture2D _background;

        public BaseWindow(int width, int height, Texture2D background=null)
        {
            this._width = width;
            this._height = height;
            this._background = background;
        }

        public void Setup()
        {
            MainApp.Log("Setting up window...");
            MainApp.GetInstance().GetGraphicsManager().PreferredBackBufferWidth = this._width;
            MainApp.GetInstance().GetGraphicsManager().PreferredBackBufferHeight = this._height;
            MainApp.GetInstance().GetGraphicsManager().IsFullScreen = false;
            MainApp.GetInstance().GetGraphicsManager().ApplyChanges();
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        public void Clear()
        {
            MainApp.Log("Clearing window...");
            MainApp.GetInstance().GraphicsDevice.Clear(Color.Black);
        }
    }

    public interface IDrawable {
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
    }

    public abstract class UIElement {
        protected Rectangle _bounds { get; }
        protected Texture2D _texture { get; }
        protected Color _color { get; }
        protected string _text { get; }
    }

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