using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AppDevGame
{
    public class MainApp : Game
    {
        // main class for the game, controls all framework-level functionality

        private static MainApp _instance; // singleton; we need global access to this instance to better integrate the UI and the game engine.
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private WindowManager _windowManager;
        private Texture2D _backgroundTexture;

        internal FontLoader _fontLoader;
        internal ImageLoader _imageLoader;

        private const bool _isDebugMode = true;

        private MainApp()
        {
            // private constructor, so that no one can create a new instance of this class.
            Log("Starting Application...");
            this._graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Log("Setup complete.");
        }

        public static MainApp GetInstance()
        {
            // the only way to get an instance of this class is to call this method.
            if (_instance == null)
            {
                _instance = new MainApp();
            }
            return _instance;
        }

        public static void Log(string message) {
            if (_isDebugMode) {
                Console.WriteLine(message);
            }
        }

        public GraphicsDeviceManager GetGraphicsManager()
        {
            return _graphics;
        }

        protected override void Initialize()
        {
            _windowManager = WindowManager.GetInstance();
            _imageLoader = new ImageLoader(GraphicsDevice);
            _fontLoader = new FontLoader(Content);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _imageLoader.LoadContent();
            _fontLoader.LoadContent();
            _backgroundTexture = _imageLoader.GetResource("PlaceholderBackground");
            _windowManager.LoadWindow(new MainMenu(800, 600, _backgroundTexture));
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            _windowManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            //_spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);

            _windowManager.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}