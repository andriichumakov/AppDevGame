using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        private ModMenu _modMenu;

        private const bool _isDebugMode = true;

        private MainApp()
        {
            // Private constructor for singleton pattern
            Log("Starting Application...");
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set the game to fullscreen
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

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
        public ModMenu ModMenu => _modMenu;

        protected override void Initialize()
        {
            try
            {
                Log($"Fullscreen: {_graphics.IsFullScreen}");
                Log($"Screen Width: {_graphics.PreferredBackBufferWidth}");
                Log($"Screen Height: {_graphics.PreferredBackBufferHeight}");

                _windowManager = WindowManager.GetInstance();
                _imageLoader = new ImageLoader(GraphicsDevice);
                _fontLoader = new FontLoader(Content);

                base.Initialize();
            }
            catch (Exception ex)
            {
                Log($"Exception during Initialize: {ex.Message}");
                throw;
            }
        }

        protected override void LoadContent()
        {
            try
            {
                _spriteBatch = new SpriteBatch(GraphicsDevice);
                _imageLoader.LoadContent();
                _fontLoader.LoadContent();
                _backgroundTexture = _imageLoader.GetResource("PlaceholderBackground");

                var font = _fontLoader.GetResource("Default");

                // Initialize menus
                _settingsMenu = new SettingsMenu(800, 600, _backgroundTexture, _windowManager, font);
                _languageMenu = new LanguageMenu(800, 600, _backgroundTexture, _windowManager, font, GraphicsDevice);
                _soundMenu = new SoundMenu(800, 600, _backgroundTexture, _windowManager, font, GraphicsDevice);
                _modMenu = new ModMenu(800, 600, _backgroundTexture, _windowManager, font);
                _mainMenu = new MainMenu(800, 600, _backgroundTexture, _windowManager, _settingsMenu, font);

                _windowManager.LoadWindow(_mainMenu);

                // Additional logging
                Log($"LoadContent called. Fullscreen: {_graphics.IsFullScreen}");
                Log($"Screen Width: {_graphics.PreferredBackBufferWidth}");
                Log($"Screen Height: {_graphics.PreferredBackBufferHeight}");

                base.LoadContent();
            }
            catch (Exception ex)
            {
                Log($"Exception during LoadContent: {ex.Message}");
                throw;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            try
            {
                _windowManager.Update(gameTime);
                base.Update(gameTime);
            }
            catch (Exception ex)
            {
                Log($"Exception during Update: {ex.Message}");
                throw;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            try
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                _spriteBatch.Begin();

                _windowManager.Draw(_spriteBatch);
                _spriteBatch.End();
                base.Draw(gameTime);
            }
            catch (Exception ex)
            {
                Log($"Exception during Draw: {ex.Message}");
                throw;
            }
        }
    }
}