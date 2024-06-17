using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Input;


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

        private Song _backgroundMusic;
        private Song _levelMusic;
        private SoundEffect _playerAttackSound;
        //private SoundEffect _playerDamageSound;
        //private SoundEffect _playerDieSound;
        //private SoundEffect _enemyAttackSound;
        //private SoundEffect _enemyDamageSound;
        //private SoundEffect _enemyDieSound;

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

            _imageLoader.LoadSpecificResource("Content/PortalActive.png", "PortalActive");
            _imageLoader.LoadSpecificResource("Content/PortalInactive.png", "PortalInactive");
            _imageLoader.LoadSpecificResource("Content/coin.png", "Coin");

            _backgroundMusic = Content.Load<Song>("background_music");
            _levelMusic = Content.Load<Song>("level_music");
            _playerAttackSound = Content.Load<SoundEffect>("player_attack");
            //_playerDamageSound = Content.Load<SoundEffect>("player_damage");
            //_playerDieSound = Content.Load<SoundEffect>("player_die");
            //_enemyAttackSound = Content.Load<SoundEffect>("enemy_attack");
            //_enemyDamageSound = Content.Load<SoundEffect>("enemy_damage");
            //_enemyDieSound = Content.Load<SoundEffect>("enemy_die");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_backgroundMusic);

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
            MediaPlayer.Stop();
            MediaPlayer.Play(_backgroundMusic);
            MediaPlayer.IsRepeating = true; // Loop the song
        }

        public void PlayLevelMusic()
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(_levelMusic);
            MediaPlayer.IsRepeating = true; // Loop the song
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

        /*public SoundEffect GetSoundEffect(string soundName)
        {
            switch (soundName)
            {
                case "player_attack":
                    return _playerAttackSound;
                case "player_damage":
                    return _playerDamageSound;
                case "player_die":
                    return _playerDieSound;
                case "enemy_attack":
                    return _enemyAttackSound;
                case "enemy_damage":
                    return _enemyDamageSound;
                case "enemy_die":
                    return _enemyDieSound;
                default:
                    return null;
            }
        }*/

        public void PlayAttackSound()
        {
            _playerAttackSound.Play(SoundEffect.MasterVolume, 0.0f, 0.0f);
        }

        public void SetMasterVolume(float volume)
        {
            MediaPlayer.Volume = volume;
            SoundEffect.MasterVolume = volume;
        }
    }
}
