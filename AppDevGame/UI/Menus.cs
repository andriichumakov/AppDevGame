using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace AppDevGame
{
    public class MainMenu : MenuWindow
    {
        private WindowManager _windowManager;
        private BaseWindow _settingsWindow;
        private SpriteFont _font;

        public MainMenu(int width, int height, Texture2D background, WindowManager windowManager, BaseWindow settingsWindow, SpriteFont font)
            : base(width, height, background)
        {
            _windowManager = windowManager;
            _settingsWindow = settingsWindow;
            _font = font;
        }

        public override void Setup()
        {
            base.Setup();
            int buttonWidth = 200;
            int buttonHeight = 50;
            int buttonSpacing = 10;

            Vector2 buttonPos = CalcButtonPosition(3, buttonWidth, buttonHeight, buttonSpacing);
            int x = (int)buttonPos.X;
            int y = (int)buttonPos.Y;

            AddElement(new Button(new Rectangle(x, y, buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("Start"), new LoadWindowCommand(WindowManager.GetInstance(), MainApp.GetInstance().StartMenu), _font));
            AddElement(new Button(new Rectangle(x, (y + buttonHeight + buttonSpacing), buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("ChangeSettings"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().SettingsMenu), _font));
            AddElement(new Button(new Rectangle(x, (y + 2 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("Quit"), new QuitCommand(), _font));
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
        }
    }

    public class SettingsMenu : MenuWindow
    {
        private WindowManager _windowManager;
        private SpriteFont _font;

        public SettingsMenu(int width, int height, Texture2D background, WindowManager windowManager, SpriteFont font)
            : base(width, height, background)
        {
            _windowManager = windowManager;
            _font = font;
        }

        public override void Setup()
        {
            base.Setup();
            int buttonWidth = 200;
            int buttonHeight = 50;
            int buttonSpacing = 10;

            Vector2 buttonPos = CalcButtonPosition(3, buttonWidth, buttonHeight, buttonSpacing);
            int x = (int)buttonPos.X;
            int y = (int)buttonPos.Y;

            AddElement(new Button(new Rectangle(x, y, buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("LanguageMenu"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().LanguageMenu), _font));
            AddElement(new Button(new Rectangle(x, (y + buttonHeight + buttonSpacing), buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("SoundMenu"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().SoundMenu), _font));
            AddElement(new Button(new Rectangle(x, (y + 2 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("ModMenu"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().ModMenu), _font));

            // Position the "Apply Changes" button in the bottom-right corner
            int applyButtonX = _width - buttonWidth - 10;
            int applyButtonY = _height - buttonHeight - 10;
            AddElement(new Button(new Rectangle(applyButtonX, applyButtonY, buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("ApplyChanges"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().MainMenu), _font));
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
        }
    }

    public class LanguageMenu : MenuWindow
    {
        private WindowManager _windowManager;
        private Button backButton;
        private Button applyButton;
        private DropdownMenu languageDropdown;
        private SpriteFont _font;
        private GraphicsDevice _graphicsDevice;
        private string _selectedLanguage;

        // Constructor for the language menu
        public LanguageMenu(int width, int height, Texture2D background, WindowManager windowManager, SpriteFont font, GraphicsDevice graphicsDevice)
            : base(width, height, background)
        {
            _windowManager = windowManager;
            _font = font;
            _graphicsDevice = graphicsDevice;
            _selectedLanguage = "en"; // Default language
        }

        // Setup the elements in the language menu
        public override void Setup()
        {
            base.Setup();
            int buttonWidth = 200;
            int buttonHeight = 50;
            int dropdownWidth = 300;
            int dropdownHeight = 50;

            Vector2 backButtonPos = new Vector2(10, 10);
            backButton = new Button(new Rectangle((int)backButtonPos.X, (int)backButtonPos.Y, buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("GoBack"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().SettingsMenu), _font);

            Vector2 dropdownPos = new Vector2((_width - dropdownWidth) / 2, (_height - dropdownHeight) / 2);
            languageDropdown = new DropdownMenu(_graphicsDevice, new Rectangle((int)dropdownPos.X, (int)dropdownPos.Y, dropdownWidth, dropdownHeight), Color.Green, Color.White, "English", new List<string> { "English", "Dutch" }, _font);

            Vector2 applyButtonPos = new Vector2(_width - buttonWidth - 10, _height - buttonHeight - 10);
            applyButton = new Button(new Rectangle((int)applyButtonPos.X, (int)applyButtonPos.Y, buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("ApplyChanges"), new ApplyLanguageCommand(), _font);

            AddElement(backButton);
            AddElement(languageDropdown);
            AddElement(applyButton);
        }

        // Load content for the language menu
        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
            backButton.LoadContent(graphicsDevice, content);
            languageDropdown.LoadContent(graphicsDevice, content);
            applyButton.LoadContent(graphicsDevice, content);
        }

        // Update the language menu
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                languageDropdown.HandleClick(new Point(mouseState.X, mouseState.Y), gameTime);
                string selectedLanguage = languageDropdown.GetSelectedItem();
                _selectedLanguage = selectedLanguage == "English" ? "en" : "nl";
            }
        }

        public string GetSelectedLanguage()
        {
            return _selectedLanguage;
        }
    }

    public class SoundMenu : MenuWindow
    {
        private WindowManager _windowManager;
        private Button backButton;
        private Slider masterVolumeSlider;
        private Slider musicSlider;
        private Slider soundEffectsSlider;
        private SpriteFont _font;
        private GraphicsDevice _graphicsDevice;

        public SoundMenu(int width, int height, Texture2D background, WindowManager windowManager, SpriteFont font, GraphicsDevice graphicsDevice)
            : base(width, height, background)
        {
            _windowManager = windowManager;
            _font = font;
            _graphicsDevice = graphicsDevice;
        }

        public override void Setup()
        {
            base.Setup();
            int buttonWidth = 200;
            int buttonHeight = 50;
            int sliderWidth = 300;
            int sliderHeight = 20;
            int sliderSpacing = 50;

            var loc = MainApp.GetInstance().LocLoader;

            Vector2 backButtonPos = new Vector2(10, 10);
            backButton = new Button(new Rectangle((int)backButtonPos.X, (int)backButtonPos.Y, buttonWidth, buttonHeight), Color.Green, Color.White, loc.GetString("GoBack"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().SettingsMenu), _font);

            int centerX = _width / 2;
            int startY = _height / 2 - sliderSpacing;

            Vector2 masterVolumePos = new Vector2(centerX - sliderWidth / 2, startY);
            masterVolumeSlider = new Slider(_graphicsDevice, new Rectangle((int)masterVolumePos.X, (int)masterVolumePos.Y, sliderWidth, sliderHeight), Color.Gray, Color.Black, loc.GetString("MasterVolume"), 0.5f, _font);

            Vector2 musicPos = new Vector2(centerX - sliderWidth / 2, startY + sliderSpacing);
            musicSlider = new Slider(_graphicsDevice, new Rectangle((int)musicPos.X, (int)musicPos.Y, sliderWidth, sliderHeight), Color.Gray, Color.Black, loc.GetString("MusicVolume"), 0.5f, _font);

            Vector2 soundEffectsPos = new Vector2(centerX - sliderWidth / 2, startY + 2 * sliderSpacing);
            soundEffectsSlider = new Slider(_graphicsDevice, new Rectangle((int)soundEffectsPos.X, (int)soundEffectsPos.Y, sliderWidth, sliderHeight), Color.Gray, Color.Black, loc.GetString("SoundEffectsVolume"), 0.5f, _font);

            AddElement(backButton);
            AddElement(masterVolumeSlider);
            AddElement(musicSlider);
            AddElement(soundEffectsSlider);
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
            backButton.LoadContent(graphicsDevice, content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            masterVolumeSlider.Update(gameTime);
            musicSlider.Update(gameTime);
            soundEffectsSlider.Update(gameTime);

            // Sync music and sound effects volume with master volume
            MediaPlayer.Volume = masterVolumeSlider.Value * musicSlider.Value;
            SoundEffect.MasterVolume = masterVolumeSlider.Value * soundEffectsSlider.Value;

            // Ensure music and sound effects volume do not exceed master volume
            if (musicSlider.Value > masterVolumeSlider.Value)
            {
                musicSlider.SetValue(masterVolumeSlider.Value);
            }

            if (soundEffectsSlider.Value > masterVolumeSlider.Value)
            {
                soundEffectsSlider.SetValue(masterVolumeSlider.Value);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            masterVolumeSlider.Draw(spriteBatch);
            musicSlider.Draw(spriteBatch);
            soundEffectsSlider.Draw(spriteBatch);
        }
    }

    public class ModMenu : MenuWindow
    {
        private WindowManager _windowManager;
        private SpriteFont _font;
        private GraphicsDevice _graphicsDevice;
        public Toggle _toggleAll;
        private List<Checkbox> _modCheckboxes;
        private Button _goBackButton;
        private const int debounceTime = 300; // 300 milliseconds debounce time
        private DateTime lastClickTime;

        public ModMenu(int width, int height, Texture2D background, WindowManager windowManager, SpriteFont font, GraphicsDevice graphicsDevice)
            : base(width, height, background)
        {
            _windowManager = windowManager;
            _font = font;
            _graphicsDevice = graphicsDevice;
            _modCheckboxes = new List<Checkbox>();
            lastClickTime = DateTime.MinValue;
        }

        // Setup the elements in the mod menu
        public override void Setup()
        {
            base.Setup();

            int buttonWidth = 70;
            int buttonHeight = 50;
            int spacing = 10;
            int textOffset = 20;

            // Go Back button
            _goBackButton = new Button(new Rectangle(10, 10, buttonWidth * 2, buttonHeight), Color.Gray, Color.White, "GoBack", new LoadWindowCommand(_windowManager, MainApp.GetInstance().SettingsMenu), _font);
            AddElement(_goBackButton);

            // Toggle all mods button
            _toggleAll = new Toggle(_graphicsDevice, new Rectangle(_width - buttonWidth - spacing, spacing, buttonWidth, buttonHeight), Color.Green, Color.Red, Color.Gray, Color.Black, "Toggle All", _font, new ToggleAllModsCommand(this));
            AddElement(_toggleAll);

            // Add text next to Toggle All button
            AddElement(new TextElement(new Rectangle(_width - buttonWidth - spacing * 2 - textOffset, spacing, buttonWidth * 2, buttonHeight), "Toggle All", _font, Color.White));

            // Calculate the start position for the mod checkboxes
            int startX = (_width - buttonWidth * 2) / 3 - textOffset;
            int startY = (_height - (3 * buttonHeight + 2 * spacing)) / 2;

            // Individual mod checkboxes
            for (int i = 0; i < 3; i++)
            {
                var checkbox = new Checkbox(_graphicsDevice, new Rectangle(startX + buttonWidth + spacing, startY + i * (buttonHeight + spacing), buttonWidth, buttonHeight), Color.Green, Color.Red, Color.Gray, Color.Black, $"Mod {i + 1}", _font, new ToggleModCommand(this, i));
                _modCheckboxes.Add(checkbox);
                AddElement(checkbox);
                // Add text next to checkbox
                AddElement(new TextElement(new Rectangle(startX, startY + i * (buttonHeight + spacing), buttonWidth * 2, buttonHeight), $"Mod {i + 1}", _font, Color.White));
            }
        }

        // Load content for the mod menu
        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
            foreach (var element in _uiElements)
            {
                element.LoadContent(graphicsDevice, content);
            }
        }

        public void ToggleAllMods(bool isOn)
        {
            foreach (var checkbox in _modCheckboxes)
            {
                checkbox.IsChecked = isOn;
            }
        }

        public bool[] GetModStates()
        {
            return _modCheckboxes.ConvertAll(cb => cb.IsChecked).ToArray();
        }

        public override void Update(GameTime gameTime)
        {
            // Add debounce logic
            if ((DateTime.Now - lastClickTime).TotalMilliseconds < debounceTime)
            {
                return;
            }

            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                lastClickTime = DateTime.Now;
            }

            base.Update(gameTime);
        }
    }

    public class StartMenu : MenuWindow
    {
        private WindowManager _windowManager;
        private SpriteFont _font;

        public StartMenu(int width, int height, Texture2D background, WindowManager windowManager, SpriteFont font)
            : base(width, height, background)
        {
            _windowManager = windowManager;
            _font = font;
        }

        public override void Setup()
        {
            base.Setup();
            int buttonWidth = 200;
            int buttonHeight = 50;
            int buttonSpacing = 10;

            Vector2 buttonPos = CalcButtonPosition(3, buttonWidth, buttonHeight, buttonSpacing);
            int x = (int)buttonPos.X;
            int y = (int)buttonPos.Y;

            AddElement(new Button(new Rectangle(x, y, buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("NewGame"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().SelectSaveSlotMenu), _font));
            AddElement(new Button(new Rectangle(x, (y + buttonHeight + buttonSpacing), buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("LoadGame"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().LoadSaveMenu), _font));
            AddElement(new Button(new Rectangle(x, (y + 2 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("Back"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().MainMenu), _font)); 
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
        }
    }

    public class SelectSaveSlotMenu : MenuWindow
    {
        private WindowManager _windowManager;
        private SpriteFont _font;

        public SelectSaveSlotMenu(int width, int height, Texture2D background, WindowManager windowManager, SpriteFont font)
            : base(width, height, background)
        {
            _windowManager = windowManager;
            _font = font;
        }

        public override void Setup()
        {
            base.Setup();
            int buttonWidth = 200;
            int buttonHeight = 50;
            int buttonSpacing = 10;

            Vector2 buttonPos = CalcButtonPosition(4, buttonWidth, buttonHeight, buttonSpacing);
            int x = (int)buttonPos.X;
            int y = (int)buttonPos.Y;

            // Adjust button texts based on whether the save slots are empty or not
            AddElement(new Button(new Rectangle(x, y, buttonWidth, buttonHeight), Color.Green, Color.White, SaveLoadManager.SaveSlotsEmpty[0] ? MainApp.GetInstance().LocLoader.GetString("SaveSlotEmpty1") : MainApp.GetInstance().LocLoader.GetString("SaveSlot1"), new StartNewGameCommand(_windowManager, 1), _font));
            AddElement(new Button(new Rectangle(x, (y + buttonHeight + buttonSpacing), buttonWidth, buttonHeight), Color.Green, Color.White, SaveLoadManager.SaveSlotsEmpty[1] ? MainApp.GetInstance().LocLoader.GetString("SaveSlotEmpty2") : MainApp.GetInstance().LocLoader.GetString("SaveSlot2"), new StartNewGameCommand(_windowManager, 2), _font));
            AddElement(new Button(new Rectangle(x, (y + 2 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, SaveLoadManager.SaveSlotsEmpty[2] ? MainApp.GetInstance().LocLoader.GetString("SaveSlotEmpty3") : MainApp.GetInstance().LocLoader.GetString("SaveSlot3"), new StartNewGameCommand(_windowManager, 3), _font));

            // Add "Go Back" button
            AddElement(new Button(new Rectangle(x, (y + 3 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("GoBack"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().StartMenu), _font));
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
        }
    }

    public class LoadSaveMenu : MenuWindow
    {
        private WindowManager _windowManager;
        private SpriteFont _font;

        public LoadSaveMenu(int width, int height, Texture2D background, WindowManager windowManager, SpriteFont font)
            : base(width, height, background)
        {
            _windowManager = windowManager;
            _font = font;
        }

        public override void Setup()
        {
            base.Setup();
            int buttonWidth = 200;
            int buttonHeight = 50;
            int buttonSpacing = 10;

            Vector2 buttonPos = CalcButtonPosition(4, buttonWidth, buttonHeight, buttonSpacing);
            int x = (int)buttonPos.X;
            int y = (int)buttonPos.Y;

            // Adjust button texts based on whether the save slots are empty or not
            AddElement(new Button(new Rectangle(x, y, buttonWidth, buttonHeight), Color.Green, Color.White, SaveLoadManager.SaveSlotsEmpty[0] ? MainApp.GetInstance().LocLoader.GetString("SaveSlotEmpty1") : MainApp.GetInstance().LocLoader.GetString("SaveSlot1"), new LoadGameCommand(_windowManager, 1), _font));
            AddElement(new Button(new Rectangle(x, (y + buttonHeight + buttonSpacing), buttonWidth, buttonHeight), Color.Green, Color.White, SaveLoadManager.SaveSlotsEmpty[1] ? MainApp.GetInstance().LocLoader.GetString("SaveSlotEmpty2") : MainApp.GetInstance().LocLoader.GetString("SaveSlot2"), new LoadGameCommand(_windowManager, 2), _font));
            AddElement(new Button(new Rectangle(x, (y + 2 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, SaveLoadManager.SaveSlotsEmpty[2] ? MainApp.GetInstance().LocLoader.GetString("SaveSlotEmpty3") : MainApp.GetInstance().LocLoader.GetString("SaveSlot3"), new LoadGameCommand(_windowManager, 3), _font));

            // Add delete buttons
            AddElement(new Button(new Rectangle(x + buttonWidth + buttonSpacing, y, buttonWidth / 2, buttonHeight), Color.Red, Color.White, MainApp.GetInstance().LocLoader.GetString("Delete"), new DeleteSaveCommand(1), _font));
            AddElement(new Button(new Rectangle(x + buttonWidth + buttonSpacing, (y + buttonHeight + buttonSpacing), buttonWidth / 2, buttonHeight), Color.Red, Color.White, MainApp.GetInstance().LocLoader.GetString("Delete"), new DeleteSaveCommand(2), _font));
            AddElement(new Button(new Rectangle(x + buttonWidth + buttonSpacing, (y + 2 * (buttonHeight + buttonSpacing)), buttonWidth / 2, buttonHeight), Color.Red, Color.White, MainApp.GetInstance().LocLoader.GetString("Delete"), new DeleteSaveCommand(3), _font));

            // Add "Go Back" button
            AddElement(new Button(new Rectangle(x, (y + 3 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("GoBack"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().StartMenu), _font));
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
        }
    }
}