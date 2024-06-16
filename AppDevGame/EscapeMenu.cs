using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppDevGame
{
    public class EscapeMenu : BaseWindow
    {
        private SpriteFont _font;
        private OverlayWindow _controlsOverlay;
        private bool _showingControlsOverlay;
        private LevelWindow _currentLevel;

        public EscapeMenu(int width, int height, Texture2D backgroundTexture, LevelWindow currentLevel)
            : base(width, height, backgroundTexture)
        {
            _font = MainApp.GetInstance()._fontLoader.GetResource("Default");
            _currentLevel = currentLevel;
            SetupButtons();
            SetupControlsOverlay();
        }

        private void SetupButtons()
        {
            int buttonWidth = 200;
            int buttonHeight = 50;
            int spacing = 20;
            int startX = (_width - buttonWidth) / 2;
            int startY = (_height - (5 * (buttonHeight + spacing))) / 2;

            AddElement(new Button(new Rectangle(startX, startY, buttonWidth, buttonHeight), Color.Gray, Color.White, "Unpause", new UnpauseCommand(), _font));
            AddElement(new Button(new Rectangle(startX, startY += (buttonHeight + spacing), buttonWidth, buttonHeight), Color.Gray, Color.White, "Restart Level", new RestartLevelCommand(_currentLevel), _font));
            AddElement(new Button(new Rectangle(startX, startY += (buttonHeight + spacing), buttonWidth, buttonHeight), Color.Gray, Color.White, "Controls", new ShowControlsCommand(this), _font));
            AddElement(new Button(new Rectangle(startX, startY += (buttonHeight + spacing), buttonWidth, buttonHeight), Color.Gray, Color.White, "Quit to Menu", new LoadWindowCommand(WindowManager.GetInstance(), MainApp.GetInstance().MainMenu, true), _font));
            AddElement(new Button(new Rectangle(startX, startY += (buttonHeight + spacing), buttonWidth, buttonHeight), Color.Gray, Color.White, "Quit to Desktop", new QuitCommand(), _font));
        }

        private void SetupControlsOverlay()
        {
            _controlsOverlay = new OverlayWindow(_width, _height, _background);
            int buttonWidth = 200;
            int buttonHeight = 50;
            int startX = (_width - buttonWidth) / 2;
            int startY = (_height - buttonHeight) / 2;

            // Add "Go back" button
            _controlsOverlay.AddElement(new Button(new Rectangle(startX, startY, buttonWidth, buttonHeight), Color.Gray, Color.White, "Go back", new HideControlsCommand(this), _font));
            
            // Add "Hello world" text as a button
            _controlsOverlay.AddElement(new Button(new Rectangle(startX, startY + 100, buttonWidth, buttonHeight), Color.Transparent, Color.White, "Hello world", null, _font));

            MainApp.Log("Controls overlay set up.");
        }

        public override void Update(GameTime gameTime)
        {
            if (_showingControlsOverlay)
            {
                _controlsOverlay.Update(gameTime);
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    HideControlsOverlay();
                }
            }
            else
            {
                base.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_showingControlsOverlay)
            {
                _controlsOverlay.Draw(spriteBatch);
            }
            else
            {
                base.Draw(spriteBatch);
            }
        }

        public void ShowControlsOverlay()
        {
            _showingControlsOverlay = true;
            MainApp.Log("Showing controls overlay.");
        }

        public void HideControlsOverlay()
        {
            _showingControlsOverlay = false;
            MainApp.Log("Hiding controls overlay.");
        }
    }

    public class ShowControlsCommand : ICommand
    {
        private EscapeMenu _escapeMenu;

        public ShowControlsCommand(EscapeMenu escapeMenu)
        {
            _escapeMenu = escapeMenu;
        }

        public void Execute()
        {
            MainApp.Log("Executing ShowControlsCommand.");
            _escapeMenu.ShowControlsOverlay();
        }
    }

    public class HideControlsCommand : ICommand
    {
        private EscapeMenu _escapeMenu;

        public HideControlsCommand(EscapeMenu escapeMenu)
        {
            _escapeMenu = escapeMenu;
        }

        public void Execute()
        {
            MainApp.Log("Executing HideControlsCommand.");
            _escapeMenu.HideControlsOverlay();
        }
    }
}
