using System.ComponentModel;
using AppDevGame;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppDevGame 
{
    public class MainApp: Game
    {
        private static MainApp _instance; // singleton (thank god for software quality)
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private WindowManager _windowManager;
        private ImageLoader _imageLoader;

        private MainApp() 
        {
            this._graphics = new GraphicsDeviceManager(this);
            this._windowManager = WindowManager.GetInstance();
            this._spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this._imageLoader = new ImageLoader(this.GraphicsDevice, "backup.png");
        }

        public static MainApp GetInstance() 
        {
            if (_instance == null) {
                _instance = new MainApp();
            }
            return _instance;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
        
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this._imageLoader.LoadContent();
        }

        protected Texture2D GetTexture(string key) {
            return this._imageLoader.GetResource(key);
        }
    }
}