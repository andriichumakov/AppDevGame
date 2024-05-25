using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Diagnostics;

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
        private ModMenu _modMenu;

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
                Debug.WriteLine(message); // Added Debug output
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
        public ModMenu ModMenu => _modMenu;

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
            
            // Load the background texture
            _backgroundTexture = _imageLoader.GetResource("PlaceholderBackground");

            if (_backgroundTexture == null)
            {
                Log("Error: Background texture not loaded.");
            }
            else
            {
                Log("Background texture loaded successfully.");
            }

            // Initialize menus
            _settingsMenu = new SettingsMenu(800, 600, _backgroundTexture, _windowManager, _fontLoader.GetResource("Default"));
            _languageMenu = new LanguageMenu(800, 600, _backgroundTexture, _windowManager, _fontLoader.GetResource("Default"));
            _soundMenu = new SoundMenu(800, 600, _backgroundTexture, _windowManager, _fontLoader.GetResource("Default"));
            _modMenu = new ModMenu(800, 600, _backgroundTexture, _windowManager, _fontLoader.GetResource("Default"));
            _mainMenu = new MainMenu(800, 600, _backgroundTexture, _windowManager, _settingsMenu, _fontLoader.GetResource("Default"));

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