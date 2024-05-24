using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace AppDevGame
{
    public class MainMenu : MenuWindow
    {
        private WindowManager _windowManager;
        private BaseWindow _settingsWindow;

        public MainMenu(int width, int height, Texture2D background, WindowManager windowManager, BaseWindow settingsWindow) 
            : base(width, height, background)
        {
            _windowManager = windowManager;
            _settingsWindow = settingsWindow;
        }

        public override void Setup()
        {
            base.Setup();
            int buttonWidth = 200;
            int buttonHeight = 50;
            int buttonSpacing = 10;
            // Add buttons to the main menu
            Vector2 buttonPos = CalcButtonPosition(3, buttonWidth, buttonHeight, buttonSpacing); // Update to 3 buttons
            int x = (int)buttonPos.X;
            int y = (int)buttonPos.Y;

            AddElement(new Button(new Rectangle(x, y, buttonWidth, buttonHeight), Color.Green, Color.White, "Start", new LoadWindowCommand(WindowManager.GetInstance(), new Level1(800, 600, 975, 650, MainApp.GetInstance()._imageLoader.GetResource("BackgroundLvl1")))));
            AddElement(new Button(new Rectangle(x, (y + buttonHeight + buttonSpacing), buttonWidth, buttonHeight), Color.Green, Color.White, "Change Settings", new LoadWindowCommand(_windowManager, _settingsWindow)));
            AddElement(new Button(new Rectangle(x, (y + 2 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, "Quit", new QuitCommand()));
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
            // Load content for buttons and other UI elements here
        }
    }

    public class SettingsMenu : MenuWindow
    {
        private WindowManager _windowManager;

        public SettingsMenu(int width, int height, Texture2D background, WindowManager windowManager) : base(width, height, background)
        {
            _windowManager = windowManager;
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

            AddElement(new Button(new Rectangle(x, y, buttonWidth, buttonHeight), Color.Green, Color.White, "Language Menu", new LoadWindowCommand(_windowManager, MainApp.GetInstance().LanguageMenu)));
            AddElement(new Button(new Rectangle(x, (y + buttonHeight + buttonSpacing), buttonWidth, buttonHeight), Color.Green, Color.White, "Sound Menu", new LoadWindowCommand(_windowManager, MainApp.GetInstance().SoundMenu)));
            AddElement(new Button(new Rectangle(x, (y + 2 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, "Mod Menu", new LoadWindowCommand(_windowManager, MainApp.GetInstance().ModMenu)));

            // Position the "Apply Changes" button in the bottom-right corner
            int applyButtonX = _width - buttonWidth - 10;
            int applyButtonY = _height - buttonHeight - 10;
            AddElement(new Button(new Rectangle(applyButtonX, applyButtonY, buttonWidth, buttonHeight), Color.Green, Color.White, "Apply Changes", new LoadWindowCommand(_windowManager, MainApp.GetInstance().MainMenu)));
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
            // Load content for buttons and other UI elements here
        }
    }

    public class LanguageMenu : MenuWindow
    {
        private WindowManager _windowManager;
        private Button backButton;
        private DropdownMenu languageDropdown;

        public LanguageMenu(int width, int height, Texture2D background, WindowManager windowManager) : base(width, height, background)
        {
            _windowManager = windowManager;
        }

        public override void Setup()
        {
            base.Setup();
            int buttonWidth = 200;
            int buttonHeight = 50;

            Vector2 backButtonPos = new Vector2(10, 10);
            backButton = new Button(new Rectangle((int)backButtonPos.X, (int)backButtonPos.Y, buttonWidth, buttonHeight), Color.Green, Color.White, "Go Back", new LoadWindowCommand(_windowManager, MainApp.GetInstance().SettingsMenu));

            Vector2 dropdownPos = new Vector2(300, 200);
            languageDropdown = new DropdownMenu(new Rectangle((int)dropdownPos.X, (int)dropdownPos.Y, buttonWidth, buttonHeight), Color.Green, Color.White, new List<string> { "English", "Dutch" });

            AddElement(backButton);
            AddElement(languageDropdown);
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
            backButton.LoadContent(graphicsDevice, content);
            languageDropdown.LoadContent(graphicsDevice, content);
        }
    }

    public class SoundMenu : MenuWindow
    {
        private WindowManager _windowManager;
        private Button backButton;
        private Slider masterVolumeSlider;
        private Slider musicSlider;
        private Slider soundEffectsSlider;
        private Label masterVolumeLabel;
        private Label musicLabel;
        private Label soundEffectsLabel;

        public SoundMenu(int width, int height, Texture2D background, WindowManager windowManager) : base(width, height, background)
        {
            _windowManager = windowManager;
        }

        public override void Setup()
        {
            base.Setup();
            int buttonWidth = 200;
            int buttonHeight = 50;

            Vector2 backButtonPos = new Vector2(10, 10);
            backButton = new Button(new Rectangle((int)backButtonPos.X, (int)backButtonPos.Y, buttonWidth, buttonHeight), Color.Green, Color.White, "Go Back", new LoadWindowCommand(_windowManager, MainApp.GetInstance().SettingsMenu));

            int sliderWidth = 300;
            int sliderHeight = 20;
            int sliderSpacing = 50;
            int labelOffset = 150;

            Vector2 masterVolumePos = new Vector2(300, 150);
            masterVolumeSlider = new Slider(new Rectangle((int)masterVolumePos.X, (int)masterVolumePos.Y, sliderWidth, sliderHeight), Color.Gray, Color.Black, "Master Volume", 0.5f);
            masterVolumeLabel = new Label(new Rectangle((int)masterVolumePos.X - labelOffset, (int)masterVolumePos.Y, 150, sliderHeight), Color.Transparent, Color.Black, "Master Volume");

            Vector2 musicPos = new Vector2(300, 150 + sliderSpacing);
            musicSlider = new Slider(new Rectangle((int)musicPos.X, (int)musicPos.Y, sliderWidth, sliderHeight), Color.Gray, Color.Black, "Music", 0.5f);
            musicLabel = new Label(new Rectangle((int)musicPos.X - labelOffset, (int)musicPos.Y, 150, sliderHeight), Color.Transparent, Color.Black, "Music");

            Vector2 soundEffectsPos = new Vector2(300, 150 + 2 * sliderSpacing);
            soundEffectsSlider = new Slider(new Rectangle((int)soundEffectsPos.X, (int)soundEffectsPos.Y, sliderWidth, sliderHeight), Color.Gray, Color.Black, "Sound Effects", 0.5f);
            soundEffectsLabel = new Label(new Rectangle((int)soundEffectsPos.X - labelOffset, (int)soundEffectsPos.Y, 150, sliderHeight), Color.Transparent, Color.Black, "Sound Effects");

            AddElement(backButton);
            AddElement(masterVolumeSlider);
            AddElement(masterVolumeLabel);
            AddElement(musicSlider);
            AddElement(musicLabel);
            AddElement(soundEffectsSlider);
            AddElement(soundEffectsLabel);
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
            backButton.LoadContent(graphicsDevice, content);
            masterVolumeSlider.LoadContent(graphicsDevice, content);
            masterVolumeLabel.LoadContent(graphicsDevice, content);
            musicSlider.LoadContent(graphicsDevice, content);
            musicLabel.LoadContent(graphicsDevice, content);
            soundEffectsSlider.LoadContent(graphicsDevice, content);
            soundEffectsLabel.LoadContent(graphicsDevice, content);
        }
    }

    public class ModMenu : MenuWindow
    {
        private WindowManager _windowManager;
        private Button backButton;
        private Button chooseWeaponButton;
        private Button chooseCharacterButton;
        private Button applyButton;
        private Label chooseWeaponLabel;
        private Label chooseCharacterLabel;

        public ModMenu(int width, int height, Texture2D background, WindowManager windowManager) : base(width, height, background)
        {
            _windowManager = windowManager;
        }

        public override void Setup()
        {
            base.Setup();
            int buttonWidth = 200;
            int buttonHeight = 50;
            int buttonSpacing = 20;
            int labelOffset = 150;

            // Move labels left from buttons
            Vector2 backButtonPos = new Vector2(10, 10);
            backButton = new Button(new Rectangle((int)backButtonPos.X, (int)backButtonPos.Y, buttonWidth, buttonHeight), Color.Green, Color.White, "Go Back", new LoadWindowCommand(_windowManager, MainApp.GetInstance().SettingsMenu));

            Vector2 chooseWeaponPos = new Vector2(300, 150);
            chooseWeaponButton = new Button(new Rectangle((int)chooseWeaponPos.X, (int)chooseWeaponPos.Y, buttonWidth, buttonHeight), Color.Gray, Color.White, "Open File", null);
            chooseWeaponLabel = new Label(new Rectangle((int)chooseWeaponPos.X - labelOffset, (int)chooseWeaponPos.Y, 150, buttonHeight), Color.Transparent, Color.Black, "Choose Weapon");

            Vector2 chooseCharacterPos = new Vector2(300, 150 + buttonHeight + buttonSpacing);
            chooseCharacterButton = new Button(new Rectangle((int)chooseCharacterPos.X, (int)chooseCharacterPos.Y, buttonWidth, buttonHeight), Color.Gray, Color.White, "Open File", null);
            chooseCharacterLabel = new Label(new Rectangle((int)chooseCharacterPos.X - labelOffset, (int)chooseCharacterPos.Y, 150, buttonHeight), Color.Transparent, Color.Black, "Choose Main Character");

            // Position the "Apply Changes" button in the bottom-right corner
            int applyButtonX = _width - buttonWidth - 10;
            int applyButtonY = _height - buttonHeight - 10;
            applyButton = new Button(new Rectangle(applyButtonX, applyButtonY, buttonWidth, buttonHeight), Color.Green, Color.White, "Apply Changes", new LoadWindowCommand(_windowManager, MainApp.GetInstance().MainMenu));

            AddElement(backButton);
            AddElement(chooseWeaponButton);
            AddElement(chooseWeaponLabel);
            AddElement(chooseCharacterButton);
            AddElement(chooseCharacterLabel);
            AddElement(applyButton);
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
            backButton.LoadContent(graphicsDevice, content);
            chooseWeaponButton.LoadContent(graphicsDevice, content);
            chooseWeaponLabel.LoadContent(graphicsDevice, content);
            chooseCharacterButton.LoadContent(graphicsDevice, content);
            chooseCharacterLabel.LoadContent(graphicsDevice, content);
            applyButton.LoadContent(graphicsDevice, content);
        }
    }
}