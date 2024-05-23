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

            AddElement(new Button(new Rectangle(x, y, buttonWidth, buttonHeight), Color.Green, Color.White, "Start", null));
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
            AddElement(new Button(new Rectangle(x, (y + 2 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, "Mod Menu", null));

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

            int centerX = _width / 2;
            int startY = 150; // Adjusted starting position for the sliders

            Vector2 masterVolumeLabelPos = new Vector2(centerX - sliderWidth / 2 - 100, startY);
            Vector2 masterVolumePos = new Vector2(centerX - sliderWidth / 2, startY);
            masterVolumeSlider = new Slider(new Rectangle((int)masterVolumePos.X, (int)masterVolumePos.Y, sliderWidth, sliderHeight), Color.Gray, Color.Black, "Master Volume", 0.5f);

            Vector2 musicLabelPos = new Vector2(centerX - sliderWidth / 2 - 100, startY + sliderSpacing);
            Vector2 musicPos = new Vector2(centerX - sliderWidth / 2, startY + sliderSpacing);
            musicSlider = new Slider(new Rectangle((int)musicPos.X, (int)musicPos.Y, sliderWidth, sliderHeight), Color.Gray, Color.Black, "Music", 0.5f);

            Vector2 soundEffectsLabelPos = new Vector2(centerX - sliderWidth / 2 - 100, startY + 2 * sliderSpacing);
            Vector2 soundEffectsPos = new Vector2(centerX - sliderWidth / 2, startY + 2 * sliderSpacing);
            soundEffectsSlider = new Slider(new Rectangle((int)soundEffectsPos.X, (int)soundEffectsPos.Y, sliderWidth, sliderHeight), Color.Gray, Color.Black, "Sound Effects", 0.5f);

            AddElement(new Label(new Rectangle((int)masterVolumeLabelPos.X, (int)masterVolumeLabelPos.Y, 0, 0), Color.Transparent, Color.Black, "Master Volume"));
            AddElement(masterVolumeSlider);

            AddElement(new Label(new Rectangle((int)musicLabelPos.X, (int)musicLabelPos.Y, 0, 0), Color.Transparent, Color.Black, "Music"));
            AddElement(musicSlider);

            AddElement(new Label(new Rectangle((int)soundEffectsLabelPos.X, (int)soundEffectsLabelPos.Y, 0, 0), Color.Transparent, Color.Black, "Sound Effects"));
            AddElement(soundEffectsSlider);

            AddElement(backButton);
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
            backButton.LoadContent(graphicsDevice, content);
            masterVolumeSlider.LoadContent(graphicsDevice, content);
            musicSlider.LoadContent(graphicsDevice, content);
            soundEffectsSlider.LoadContent(graphicsDevice, content);
        }
    }
}