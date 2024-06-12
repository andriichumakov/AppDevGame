using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
        public ImageLoader _imageLoader;
        public LevelWindow _currentLevel;
        public LocLoader LocLoader { get; private set; }

        private MainMenu _mainMenu;
        private SettingsMenu _settingsMenu;
        private LanguageMenu _languageMenu;
        private SoundMenu _soundMenu;
        private ModMenu _modMenu;
        private StartMenu _startMenu;
        private SelectSaveSlotMenu _selectSaveSlotMenu;
        private LoadSaveMenu _loadSaveMenu;

        private const bool _isDebugMode = true;
        private static readonly string LogFilePath = "game_log.txt";

        private static DateTime _lastActionTime = DateTime.MinValue;
        private static readonly TimeSpan ActionDelay = TimeSpan.FromSeconds(1.5); // 1.5 seconds delay

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
                Console.WriteLine(message); // Log to console
                try
                {
                    using (StreamWriter writer = new StreamWriter(LogFilePath, true))
                    {
                        writer.WriteLine($"{DateTime.Now}: {message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to log message: {ex.Message}");
                }
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
        public StartMenu StartMenu => _startMenu;
        public SelectSaveSlotMenu SelectSaveSlotMenu => _selectSaveSlotMenu;
        public LoadSaveMenu LoadSaveMenu => _loadSaveMenu;

        protected override void Initialize()
        {
            _windowManager = WindowManager.GetInstance();
            _imageLoader = new ImageLoader(GraphicsDevice);
            _fontLoader = new FontLoader(Content);
            LocLoader = new LocLoader();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _imageLoader.LoadContent();
            _fontLoader.LoadContent();
            LocLoader.LoadLocalization("en", Content);
            _backgroundTexture = _imageLoader.GetResource("PlaceholderBackground");

            var font = _fontLoader.GetResource("Default");

            // Load the portal textures specifically
            _imageLoader.LoadSpecificResource("Content/PortalActive.png", "PortalActive");
            _imageLoader.LoadSpecificResource("Content/PortalInactive.png", "PortalInactive");

            // Load the coin texture
            _imageLoader.LoadSpecificResource("Content/coin.png", "Coin");

            // Initialize menus
            _settingsMenu = new SettingsMenu(800, 600, _backgroundTexture, _windowManager, font);
            _languageMenu = new LanguageMenu(800, 600, _backgroundTexture, _windowManager, font, GraphicsDevice);
            _soundMenu = new SoundMenu(800, 600, _backgroundTexture, _windowManager, font, GraphicsDevice);
            _modMenu = new ModMenu(800, 600, _backgroundTexture, _windowManager, font);
            _mainMenu = new MainMenu(800, 600, _backgroundTexture, _windowManager, _settingsMenu, font);
            _startMenu = new StartMenu(800, 600, _backgroundTexture, _windowManager, font);
            _selectSaveSlotMenu = new SelectSaveSlotMenu(800, 600, _backgroundTexture, _windowManager, font);
            _loadSaveMenu = new LoadSaveMenu(800, 600, _backgroundTexture, _windowManager, font);

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

        public LevelWindow GetCurrentLevel()
        {
            return _currentLevel;
        }

        public static bool CanPerformAction()
        {
            return DateTime.Now - _lastActionTime >= ActionDelay;
        }

        public static void RecordAction()
        {
            _lastActionTime = DateTime.Now;
        }

        public void ChangeLanguage(string newLanguage)
        {
            LocLoader.ChangeLanguage(newLanguage, Content);
            
            // Update text for all elements in the current window
            if (_windowManager.CurrentWindow is MenuWindow currentMenu)
            {
                currentMenu.UpdateTexts();
            }
        }
    }
}