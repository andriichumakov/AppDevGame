using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private bool _isPaused = false;  // Track whether the game is paused
        public LevelWindow _currentLevel;

        // Property to access the background texture
        public Texture2D BackgroundTexture => _backgroundTexture;
        public EscapeMenu EscapeMenu { get; private set; }
        public bool IsPaused => _isPaused;  // Public property to check if the game is paused

        private const bool _isDebugMode = true;
        private static readonly string LogFilePath = "game_log.txt";

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

        protected override void Initialize()
        {
            _windowManager = WindowManager.GetInstance();
            _fontLoader = new FontLoader(Content);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _imageLoader = new ImageLoader(GraphicsDevice);
            _imageLoader.LoadContent();
            _fontLoader.LoadContent();
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

            // Remove the following line:
            // EscapeMenu = new EscapeMenu(800, 600, _backgroundTexture, currentLevel);

            _windowManager.LoadWindow(_mainMenu);

            base.LoadContent();
        }


       public void TogglePause()
        {
            _isPaused = !_isPaused;

            if (_isPaused)
            {
                LevelWindow currentLevel = _windowManager.GetCurrentWindow() as LevelWindow;

                if (currentLevel != null)
                {
                    EscapeMenu = new EscapeMenu(800, 600, _backgroundTexture, currentLevel);
                }
                else
                {
                    throw new InvalidOperationException("Current window is not a LevelWindow.");
                }
            }
        }




       protected override void Update(GameTime gameTime)
       {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape) && !_isPaused)
            {
                TogglePause();
            }
            else if (state.IsKeyDown(Keys.U) && _isPaused)
            {
                TogglePause();
            }

            if (_isPaused)
            {
                // Log that we're updating the pause menu
                MainApp.Log("Updating the pause menu...");
                EscapeMenu.Update(gameTime);
            }
            else
            {
                _windowManager.Update(gameTime);
                base.Update(gameTime);
            }
       }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _windowManager.Draw(_spriteBatch);
            _spriteBatch.End();

            // If the game is paused, draw the escape menu overlay
            if (_isPaused)
            {
                _spriteBatch.Begin();
                EscapeMenu.Draw(_spriteBatch);  // Draw the pause menu overlay
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
