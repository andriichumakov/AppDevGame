using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
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

            // Add buttons to the main menu
            Vector2 buttonPos = CalcButtonPosition(3, buttonWidth, buttonHeight, buttonSpacing);
            int x = (int)buttonPos.X;
            int y = (int)buttonPos.Y;

            AddElement(new Button(new Rectangle(x, y, buttonWidth, buttonHeight), Color.Green, Color.White, "Start", new LoadWindowCommand(WindowManager.GetInstance(), new Level1(800, 600, 975, 650, MainApp.GetInstance()._imageLoader.GetResource("BackgroundLvl1"))), _font));
            AddElement(new Button(new Rectangle(x, (y + buttonHeight + buttonSpacing), buttonWidth, buttonHeight), Color.Green, Color.White, "Change Settings", new LoadWindowCommand(_windowManager, _settingsWindow), _font));
            AddElement(new Button(new Rectangle(x, (y + 2 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, "Quit", new QuitCommand(), _font));
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

            AddElement(new Button(new Rectangle(x, y, buttonWidth, buttonHeight), Color.Green, Color.White, "Language Menu", new LoadWindowCommand(_windowManager, MainApp.GetInstance().LanguageMenu), _font));
            AddElement(new Button(new Rectangle(x, (y + buttonHeight + buttonSpacing), buttonWidth, buttonHeight), Color.Green, Color.White, "Sound Menu", new LoadWindowCommand(_windowManager, MainApp.GetInstance().SoundMenu), _font));
            AddElement(new Button(new Rectangle(x, (y + 2 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, "Mod Menu", new LoadWindowCommand(_windowManager, MainApp.GetInstance().ModMenu), _font));

            // Position the "Apply Changes" button in the bottom-right corner
            int applyButtonX = _width - buttonWidth - 10;
            int applyButtonY = _height - buttonHeight - 10;
            AddElement(new Button(new Rectangle(applyButtonX, applyButtonY, buttonWidth, buttonHeight), Color.Green, Color.White, "Apply Changes", new LoadWindowCommand(_windowManager, MainApp.GetInstance().MainMenu), _font));
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
        private DropdownMenu languageDropdown;
        private SpriteFont _font;
        private GraphicsDevice _graphicsDevice;

        public LanguageMenu(int width, int height, Texture2D background, WindowManager windowManager, SpriteFont font, GraphicsDevice graphicsDevice)
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
            int dropdownWidth = 300;
            int dropdownHeight = 50;

            Vector2 backButtonPos = new Vector2(10, 10);
            backButton = new Button(new Rectangle((int)backButtonPos.X, (int)backButtonPos.Y, buttonWidth, buttonHeight), Color.Green, Color.White, "Go Back", new LoadWindowCommand(_windowManager, MainApp.GetInstance().SettingsMenu), _font);

            Vector2 dropdownPos = new Vector2((_width - dropdownWidth) / 2, (_height - dropdownHeight) / 2);
            languageDropdown = new DropdownMenu(_graphicsDevice, new Rectangle((int)dropdownPos.X, (int)dropdownPos.Y, dropdownWidth, dropdownHeight), Color.Green, Color.White, "English", new List<string> { "English", "Dutch" }, _font);

            AddElement(backButton);
            AddElement(languageDropdown);
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
            backButton.LoadContent(graphicsDevice, content);
            languageDropdown.LoadContent(graphicsDevice, content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                languageDropdown.HandleClick(new Point(mouseState.X, mouseState.Y), gameTime);
            }
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

            Vector2 backButtonPos = new Vector2(10, 10);
            backButton = new Button(new Rectangle((int)backButtonPos.X, (int)backButtonPos.Y, buttonWidth, buttonHeight), Color.Green, Color.White, "Go Back", new LoadWindowCommand(_windowManager, MainApp.GetInstance().SettingsMenu), _font);

            int centerX = _width / 2;
            int startY = _height / 2 - sliderSpacing;

            Vector2 masterVolumePos = new Vector2(centerX - sliderWidth / 2, startY);
            masterVolumeSlider = new Slider(_graphicsDevice, new Rectangle((int)masterVolumePos.X, (int)masterVolumePos.Y, sliderWidth, sliderHeight), Color.Gray, Color.Black, "Master Volume", 0.5f, _font);

            Vector2 musicPos = new Vector2(centerX - sliderWidth / 2, startY + sliderSpacing);
            musicSlider = new Slider(_graphicsDevice, new Rectangle((int)musicPos.X, (int)musicPos.Y, sliderWidth, sliderHeight), Color.Gray, Color.Black, "Music", 0.5f, _font);

            Vector2 soundEffectsPos = new Vector2(centerX - sliderWidth / 2, startY + 2 * sliderSpacing);
            soundEffectsSlider = new Slider(_graphicsDevice, new Rectangle((int)soundEffectsPos.X, (int)soundEffectsPos.Y, sliderWidth, sliderHeight), Color.Gray, Color.Black, "Sound Effects", 0.5f, _font);

            AddElement(backButton);
            AddElement(masterVolumeSlider);
            AddElement(musicSlider);
            AddElement(soundEffectsSlider);
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
            backButton.LoadContent(graphicsDevice, content);
            masterVolumeSlider.LoadContent(graphicsDevice, content);
            musicSlider.LoadContent(graphicsDevice, content);
            soundEffectsSlider.LoadContent(graphicsDevice, content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float masterVolume = masterVolumeSlider.Value;
            musicSlider.Value = Math.Min(musicSlider.Value, masterVolume);
            soundEffectsSlider.Value = Math.Min(soundEffectsSlider.Value, masterVolume);
        }
    }

    public class ModMenu : MenuWindow
    {
        private WindowManager _windowManager;
        private Button backButton;
        private Button chooseWeaponButton;
        private Button chooseCharacterButton;
        private Button applyChangesButton;
        private SpriteFont _font;

        public ModMenu(int width, int height, Texture2D background, WindowManager windowManager, SpriteFont font) 
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

            Vector2 backButtonPos = new Vector2(10, 10);
            backButton = new Button(new Rectangle((int)backButtonPos.X, (int)backButtonPos.Y, buttonWidth, buttonHeight), Color.Green, Color.White, "Go Back", new LoadWindowCommand(_windowManager, MainApp.GetInstance().SettingsMenu), _font);

            Vector2 chooseWeaponPos = new Vector2(300, 200);
            chooseWeaponButton = new Button(new Rectangle((int)chooseWeaponPos.X + 200, (int)chooseWeaponPos.Y, buttonWidth, buttonHeight), Color.Gray, Color.Black, "Open File", null, _font);
            AddElement(new Label(new Rectangle((int)chooseWeaponPos.X - 150, (int)chooseWeaponPos.Y, 150, buttonHeight), Color.Transparent, Color.Black, "Choose Weapon", _font));

            Vector2 chooseCharacterPos = new Vector2(300, 200 + buttonHeight + buttonSpacing);
            chooseCharacterButton = new Button(new Rectangle((int)chooseCharacterPos.X + 200, (int)chooseCharacterPos.Y, buttonWidth, buttonHeight), Color.Gray, Color.Black, "Open File", null, _font);
            AddElement(new Label(new Rectangle((int)chooseCharacterPos.X - 150, (int)chooseCharacterPos.Y, 150, buttonHeight), Color.Transparent, Color.Black, "Choose Main Character", _font));

            Vector2 applyChangesPos = new Vector2(_width - buttonWidth - 10, _height - buttonHeight - 10);
            applyChangesButton = new Button(new Rectangle((int)applyChangesPos.X, (int)applyChangesPos.Y, buttonWidth, buttonHeight), Color.Green, Color.White, "Apply Changes", new LoadWindowCommand(_windowManager, MainApp.GetInstance().MainMenu), _font);

            AddElement(backButton);
            AddElement(chooseWeaponButton);
            AddElement(chooseCharacterButton);
            AddElement(applyChangesButton);
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
            backButton.LoadContent(graphicsDevice, content);
            chooseWeaponButton.LoadContent(graphicsDevice, content);
            chooseCharacterButton.LoadContent(graphicsDevice, content);
            applyChangesButton.LoadContent(graphicsDevice, content);
        }
    }
}