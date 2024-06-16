using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppDevGame
{
    public class EscapeMenu : BaseWindow
    {
        private SpriteFont _font;
        private LevelWindow _currentLevel;

        public EscapeMenu(int width, int height, Texture2D backgroundTexture, LevelWindow currentLevel)
            : base(width, height, backgroundTexture)
        {
            _font = MainApp.GetInstance()._fontLoader.GetResource("Default");
            _currentLevel = currentLevel;
            SetupButtons();
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
            AddElement(new Button(new Rectangle(startX, startY += (buttonHeight + spacing), buttonWidth, buttonHeight), Color.Gray, Color.White, "Placeholder", null, _font));
            AddElement(new Button(new Rectangle(startX, startY += (buttonHeight + spacing), buttonWidth, buttonHeight), Color.Gray, Color.White, "Quit to Menu", new LoadWindowCommand(WindowManager.GetInstance(), MainApp.GetInstance().MainMenu, true), _font));
            AddElement(new Button(new Rectangle(startX, startY += (buttonHeight + spacing), buttonWidth, buttonHeight), Color.Gray, Color.White, "Quit to Desktop", new QuitCommand(), _font));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MouseState currentMouseState = Mouse.GetState();
            if (currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
            {
                Point mousePosition = new Point(currentMouseState.X, currentMouseState.Y);

                foreach (UIElement element in _elements)
                {
                    if (element is IClickable clickable)
                    {
                        clickable.HandleClick(mousePosition, gameTime);
                    }
                }
            }

            _previousMouseState = currentMouseState;
        }
    }

}
