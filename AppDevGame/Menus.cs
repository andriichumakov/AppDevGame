using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace AppDevGame
{
    public class MainMenu : MenuWindow
    {
        private WindowManager _windowManager;
        private BaseWindow _settingsWindow;
        private SpriteFont _font;

        private Button startButton;
        private Button changeSettingsButton;
        private Button quitButton;

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

            startButton = new Button(new Rectangle(x, y, buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("Start"), new LoadWindowCommand(WindowManager.GetInstance(), MainApp.GetInstance().StartMenu), _font);
            changeSettingsButton = new Button(new Rectangle(x, (y + buttonHeight + buttonSpacing), buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("ChangeSettings"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().SettingsMenu), _font);
            quitButton = new Button(new Rectangle(x, (y + 2 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("Quit"), new QuitCommand(), _font);

            AddElement(startButton);
            AddElement(changeSettingsButton);
            AddElement(quitButton);
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
        }

        public void UpdateText()
        {
            startButton.UpdateText();
            changeSettingsButton.UpdateText();
            quitButton.UpdateText();
        }
    }

    public class SettingsMenu : MenuWindow
    {
        private WindowManager _windowManager;
        private SpriteFont _font;

        private Button languageMenuButton;
        private Button soundMenuButton;
        private Button modMenuButton;
        private Button applyChangesButton;

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

            languageMenuButton = new Button(new Rectangle(x, y, buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("LanguageMenu"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().LanguageMenu), _font);
            soundMenuButton = new Button(new Rectangle(x, (y + buttonHeight + buttonSpacing), buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("SoundMenu"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().SoundMenu), _font);
            modMenuButton = new Button(new Rectangle(x, (y + 2 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("ModMenu"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().ModMenu), _font);

            // Position the "Apply Changes" button in the bottom-right corner
            int applyButtonX = _width - buttonWidth - 10;
            int applyButtonY = _height - buttonHeight - 10;
            applyChangesButton = new Button(new Rectangle(applyButtonX, applyButtonY, buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("ApplyChanges"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().MainMenu), _font);

            AddElement(languageMenuButton);
            AddElement(soundMenuButton);
            AddElement(modMenuButton);
            AddElement(applyChangesButton);
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
        }

        public void UpdateText()
        {
            languageMenuButton.UpdateText();
            soundMenuButton.UpdateText();
            modMenuButton.UpdateText();
            applyChangesButton.UpdateText();
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
            backButton = new Button(new Rectangle((int)backButtonPos.X, (int)backButtonPos.Y, buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("GoBack"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().SettingsMenu), _font);

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
            if (masterVolumeSlider.Value < musicSlider.Value)
            {
                musicSlider.SetValue(masterVolumeSlider.Value);
            }

            if (masterVolumeSlider.Value < soundEffectsSlider.Value)
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

        public void UpdateText()
        {
            backButton.UpdateText();
            masterVolumeSlider.UpdateText();
            musicSlider.UpdateText();
            soundEffectsSlider.UpdateText();
        }
    }

    public class ModMenu : MenuWindow
    {
        private WindowManager _windowManager;
        private SpriteFont _font;

        private Button installModButton;
        private Button removeModButton;

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

            Vector2 buttonPos = CalcButtonPosition(2, buttonWidth, buttonHeight, buttonSpacing);
            int x = (int)buttonPos.X;
            int y = (int)buttonPos.Y;

            installModButton = new Button(new Rectangle(x, y, buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("InstallMod"), new PrintCommand("Install Mod"), _font);
            removeModButton = new Button(new Rectangle(x, (y + buttonHeight + buttonSpacing), buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("RemoveMod"), new PrintCommand("Remove Mod"), _font);

            AddElement(installModButton);
            AddElement(removeModButton);
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
        }

        public void UpdateText()
        {
            installModButton.UpdateText();
            removeModButton.UpdateText();
        }
    }

    public class StartMenu : MenuWindow
    {
        private WindowManager _windowManager;
        private SpriteFont _font;

        private Button newGameButton;
        private Button loadGameButton;
        private Button quitButton;

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

            newGameButton = new Button(new Rectangle(x, y, buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("NewGame"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().SelectSaveSlotMenu), _font);
            loadGameButton = new Button(new Rectangle(x, (y + buttonHeight + buttonSpacing), buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("LoadGame"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().LoadSaveMenu), _font);
            quitButton = new Button(new Rectangle(x, (y + 2 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("Quit"), new QuitCommand(), _font);

            AddElement(newGameButton);
            AddElement(loadGameButton);
            AddElement(quitButton);
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
        }

        public void UpdateText()
        {
            newGameButton.UpdateText();
            loadGameButton.UpdateText();
            quitButton.UpdateText();
        }
    }

    public class SelectSaveSlotMenu : MenuWindow
    {
        private WindowManager _windowManager;
        private SpriteFont _font;

        private Button saveSlot1Button;
        private Button saveSlot2Button;
        private Button saveSlot3Button;
        private Button goBackButton;

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
            saveSlot1Button = new Button(new Rectangle(x, y, buttonWidth, buttonHeight), Color.Green, Color.White, SaveLoadManager.SaveSlotsEmpty[0] ? MainApp.GetInstance().LocLoader.GetString("Save1Empty") : MainApp.GetInstance().LocLoader.GetString("Save1"), new StartNewGameCommand(_windowManager, 1), _font);
            saveSlot2Button = new Button(new Rectangle(x, (y + buttonHeight + buttonSpacing), buttonWidth, buttonHeight), Color.Green, Color.White, SaveLoadManager.SaveSlotsEmpty[1] ? MainApp.GetInstance().LocLoader.GetString("Save2Empty") : MainApp.GetInstance().LocLoader.GetString("Save2"), new StartNewGameCommand(_windowManager, 2), _font);
            saveSlot3Button = new Button(new Rectangle(x, (y + 2 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, SaveLoadManager.SaveSlotsEmpty[2] ? MainApp.GetInstance().LocLoader.GetString("Save3Empty") : MainApp.GetInstance().LocLoader.GetString("Save3"), new StartNewGameCommand(_windowManager, 3), _font);

            // Add "Go Back" button
            goBackButton = new Button(new Rectangle(x, (y + 3 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("GoBack"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().StartMenu), _font);

            AddElement(saveSlot1Button);
            AddElement(saveSlot2Button);
            AddElement(saveSlot3Button);
            AddElement(goBackButton);
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
        }

        public void UpdateText()
        {
            saveSlot1Button.UpdateText();
            saveSlot2Button.UpdateText();
            saveSlot3Button.UpdateText();
            goBackButton.UpdateText();
        }
    }

    public class LoadSaveMenu : MenuWindow
    {
        private WindowManager _windowManager;
        private SpriteFont _font;

        private Button saveSlot1Button;
        private Button saveSlot2Button;
        private Button saveSlot3Button;
        private Button deleteSlot1Button;
        private Button deleteSlot2Button;
        private Button deleteSlot3Button;
        private Button goBackButton;

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
            saveSlot1Button = new Button(new Rectangle(x, y, buttonWidth, buttonHeight), Color.Green, Color.White, SaveLoadManager.SaveSlotsEmpty[0] ? MainApp.GetInstance().LocLoader.GetString("Save1Empty") : MainApp.GetInstance().LocLoader.GetString("Save1"), new LoadGameCommand(_windowManager, 1), _font);
            saveSlot2Button = new Button(new Rectangle(x, (y + buttonHeight + buttonSpacing), buttonWidth, buttonHeight), Color.Green, Color.White, SaveLoadManager.SaveSlotsEmpty[1] ? MainApp.GetInstance().LocLoader.GetString("Save2Empty") : MainApp.GetInstance().LocLoader.GetString("Save2"), new LoadGameCommand(_windowManager, 2), _font);
            saveSlot3Button = new Button(new Rectangle(x, (y + 2 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, SaveLoadManager.SaveSlotsEmpty[2] ? MainApp.GetInstance().LocLoader.GetString("Save3Empty") : MainApp.GetInstance().LocLoader.GetString("Save3"), new LoadGameCommand(_windowManager, 3), _font);

            // Add delete buttons
            deleteSlot1Button = new Button(new Rectangle(x + buttonWidth + buttonSpacing, y, buttonWidth / 2, buttonHeight), Color.Red, Color.White, MainApp.GetInstance().LocLoader.GetString("Delete"), new DeleteSaveCommand(1), _font);
            deleteSlot2Button = new Button(new Rectangle(x + buttonWidth + buttonSpacing, (y + buttonHeight + buttonSpacing), buttonWidth / 2, buttonHeight), Color.Red, Color.White, MainApp.GetInstance().LocLoader.GetString("Delete"), new DeleteSaveCommand(2), _font);
            deleteSlot3Button = new Button(new Rectangle(x + buttonWidth + buttonSpacing, (y + 2 * (buttonHeight + buttonSpacing)), buttonWidth / 2, buttonHeight), Color.Red, Color.White, MainApp.GetInstance().LocLoader.GetString("Delete"), new DeleteSaveCommand(3), _font);

            // Add "Go Back" button
            goBackButton = new Button(new Rectangle(x, (y + 3 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, MainApp.GetInstance().LocLoader.GetString("GoBack"), new LoadWindowCommand(_windowManager, MainApp.GetInstance().StartMenu), _font);

            AddElement(saveSlot1Button);
            AddElement(saveSlot2Button);
            AddElement(saveSlot3Button);
            AddElement(deleteSlot1Button);
            AddElement(deleteSlot2Button);
            AddElement(deleteSlot3Button);
            AddElement(goBackButton);
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
        }

        public void UpdateText()
        {
            saveSlot1Button.UpdateText();
            saveSlot2Button.UpdateText();
            saveSlot3Button.UpdateText();
            deleteSlot1Button.UpdateText();
            deleteSlot2Button.UpdateText();
            deleteSlot3Button.UpdateText();
            goBackButton.UpdateText();
        }
    }
}