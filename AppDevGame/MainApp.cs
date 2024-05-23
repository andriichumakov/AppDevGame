using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AppDevGame
{
    public class MainApp : Game
    {
        // Main class for the game, controls all framework-level functionality

        private static MainApp _instance; // Singleton; global access to this instance for better integration of UI and game engine.
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private WindowManager _windowManager;
        private Texture2D _backgroundTexture;

        internal FontLoader _fontLoader;
        internal ImageLoader _imageLoader;

        private const bool _isDebugMode = true;

        public MainMenu MainMenu { get; private set; }
        public SettingsMenu SettingsMenu { get; private set; }
        public LanguageMenu LanguageMenu { get; private set; }
        public GameTime GameTime { get; private set; }

        private MainApp()
        {
            // Private constructor, so that no one can create a new instance of this class.
            Log("Starting Application...");
            this._graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Log("Setup complete.");
        }

        public static MainApp GetInstance()
        {
            // The only way to get an instance of this class is to call this method.
            if (_instance == null)
            {
                _instance = new MainApp();
            }
            return _instance;
        }

        public static void Log(string message)
        {
            if (_isDebugMode)
            {
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

            // Initialize the MainMenu, SettingsMenu, and LanguageMenu
            LanguageMenu = new LanguageMenu(800, 600, _backgroundTexture, _windowManager);
            SettingsMenu = new SettingsMenu(800, 600, _backgroundTexture, _windowManager);
            MainMenu = new MainMenu(800, 600, _backgroundTexture, _windowManager, SettingsMenu);

            _windowManager.LoadWindow(MainMenu);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            GameTime = gameTime; // Store the game time to be accessible by other classes
            _windowManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _windowManager.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}