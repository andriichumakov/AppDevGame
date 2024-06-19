using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO;

namespace AppDevGame
{
    public class MainApp : Game
    {
        private static MainApp _instance;
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

        private bool _isPaused = false;  // Track whether the game is paused

        // Property to access the background texture
        public Texture2D BackgroundTexture => _backgroundTexture;
        public EscapeMenu EscapeMenu { get; private set; }
        public bool IsPaused => _isPaused;  // Public property to check if the game is paused

        private StartMenu _startMenu;
        private SelectSaveSlotMenu _selectSaveSlotMenu;
        private LoadSaveMenu _loadSaveMenu;

        private const bool _isDebugMode = true;
        private static readonly string LogFilePath = "game_log.txt";

        private static DateTime _lastActionTime = DateTime.MinValue;
        private static readonly TimeSpan ActionDelay = TimeSpan.FromSeconds(1.5);

        private AudioManager _audioManager;

        private MainApp()
        {
            Log("Starting Application...");
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Log("Setup complete.");
        }

        public static MainApp GetInstance()
        {
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
            _fontLoader = new FontLoader(Content);
            LocLoader = new LocLoader();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _imageLoader = new ImageLoader(GraphicsDevice);
            _imageLoader.LoadContent();
            _fontLoader.LoadContent();
            LocLoader.LoadLocalization("en", Content);
            _backgroundTexture = _imageLoader.GetResource("PlaceholderBackground");

            var font = _fontLoader.GetResource("Default");

            _imageLoader.LoadSpecificResource("Images/PortalActive.png", "PortalActive");
            _imageLoader.LoadSpecificResource("Images/PortalInactive.png", "PortalInactive");
            _imageLoader.LoadSpecificResource("Images/coin.png", "Coin");

            _audioManager = AudioManager.GetInstance(Content);
            _audioManager.PlaySong("background_music");

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

        public void PlayMainMenuMusic()
        {
            _audioManager.StopSong();
            _audioManager.PlaySong("background_music");
        }

        public void PlayLevelMusic()
        {
            _audioManager.StopSong();
            _audioManager.PlaySong("level_music");
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

            // If the game is paused, draw the escape menu overlay
            if (_isPaused)
            {
                EscapeMenu.Draw(_spriteBatch);  // Draw the pause menu overlay
            }

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

            if (_windowManager.CurrentWindow is MenuWindow current_menu)
            {
                current_menu.UpdateTexts();
            }
        }

        public void SetMasterVolume(float volume)
        {
            _audioManager.SetMasterVolume(volume);
        }
    }
}