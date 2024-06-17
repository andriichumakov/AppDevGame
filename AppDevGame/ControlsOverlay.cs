using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppDevGame
{
    public class ControlsOverlay : BaseWindow
    {
        private SpriteFont _font;
        private EscapeMenu _escapeMenu;

        public ControlsOverlay(int width, int height, Texture2D backgroundTexture, EscapeMenu escapeMenu)
            : base(width, height, backgroundTexture)
        {
            _font = MainApp.GetInstance()._fontLoader.GetResource("Default");
            _escapeMenu = escapeMenu;
            SetupButtons();
        }

        private void SetupButtons()
        {
            int buttonWidth = 200;
            int buttonHeight = 50;
            int spacing = 20;
            int startX = (_width - buttonWidth) / 2;
            int startY = (_height - buttonHeight) / 2;

            AddElement(new Button(new Rectangle(startX, startY, buttonWidth, buttonHeight), Color.Gray, Color.White, "Go Back", new LoadWindowCommand(WindowManager.GetInstance(), _escapeMenu), _font));
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
