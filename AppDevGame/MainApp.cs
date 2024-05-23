using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AppDevGame
{
    public class MainApp : Game
    {
        private static MainApp _instance; // Singleton instance
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private WindowManager _windowManager;
        private Texture2D _backgroundTexture;

        internal FontLoader _fontLoader;
        internal ImageLoader _imageLoader;

        private MainMenu _mainMenu;
        private SettingsMenu _settingsMenu;
        private LanguageMenu _languageMenu;
        private SoundMenu _soundMenu;
        private ModMenu _modMenu; // Add ModMenu here

        private const bool _isDebugMode = true;

        private MainApp()
        {
            // Private constructor for singleton pattern
            Log("Starting Application...");
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Log("Setup complete.");
        }

        public static MainApp GetInstance()
        {
            // Singleton instance accessor
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

        public MainMenu MainMenu => _mainMenu;
        public SettingsMenu SettingsMenu => _settingsMenu;
        public LanguageMenu LanguageMenu => _languageMenu;
        public SoundMenu SoundMenu => _soundMenu;
        public ModMenu ModMenu => _modMenu; // Add getter for ModMenu

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

            // Initialize menus
            _settingsMenu = new SettingsMenu(800, 600, _backgroundTexture, _windowManager);
            _languageMenu = new LanguageMenu(800, 600, _backgroundTexture, _windowManager);
            _soundMenu = new SoundMenu(800, 600, _backgroundTexture, _windowManager);
            _modMenu = new ModMenu(800, 600, _backgroundTexture, _windowManager); // Initialize ModMenu here
            _mainMenu = new MainMenu(800, 600, _backgroundTexture, _windowManager, _settingsMenu);

            _windowManager.LoadWindow(_mainMenu);

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

            _windowManager.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}