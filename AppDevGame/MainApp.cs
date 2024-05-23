using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame 
{
    public class MainApp : Game
    {
        private static MainApp _instance; // Singleton
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private WindowManager _windowManager;
        private ImageLoader _imageLoader;

        private MainApp() 
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _windowManager = WindowManager.GetInstance();
            _imageLoader = new ImageLoader(GraphicsDevice, "backup.png");
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
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _imageLoader.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _windowManager.Draw(_spriteBatch);
            base.Draw(gameTime);
        }

        public Texture2D GetTexture(string key) 
        {
            return _imageLoader.GetResource(key);
        }
    }
}
