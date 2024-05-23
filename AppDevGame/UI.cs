using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppDevGame
{
    public interface IDrawable
    {
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
        void Clear();
    }

    public interface IClickable
    {
        ICommand GetLeftClickAction();
        ICommand GetRightClickAction();
    }

    public abstract class BaseButton : IClickable, IDrawable
    {
        protected Rectangle _bounds;
        protected ICommand _onLeftClick;
        protected ICommand _onRightClick;
        protected BaseWindow _parentWindow;

        public BaseButton(Rectangle bounds, ICommand onLeftClick, ICommand onRightClick, BaseWindow parentWindow)
        {
            _bounds = bounds;
            _onLeftClick = onLeftClick;
            _onRightClick = onRightClick;
            _parentWindow = parentWindow;
        }

        public ICommand GetLeftClickAction()
        {
            return _onLeftClick;
        }

        public ICommand GetRightClickAction()
        {
            return _onRightClick;
        }

        public bool IsClicked(Vector2 mousePosition)
        {
            return _bounds.Contains(mousePosition.X, mousePosition.Y);
        }

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
    }

    public class TextButton : BaseButton
    {
        private string _buttonText;
        private SpriteFont _font;
        private Color _textColor;
        private Texture2D _backgroundTexture;

        public TextButton(Rectangle bounds, ICommand onLeftClick, ICommand onRightClick, BaseWindow parentWindow, string buttonText, SpriteFont font, Color textColor, Color buttonColor)
            : base(bounds, onLeftClick, onRightClick, parentWindow)
        {
            _buttonText = buttonText;
            _font = font;
            _textColor = textColor;
            _backgroundTexture = new Texture2D(MainApp.GetInstance().GraphicsDevice, 1, 1);
            _backgroundTexture.SetData(new Color[] { buttonColor });
        }

        public TextButton(Rectangle bounds, ICommand onLeftClick, ICommand onRightClick, BaseWindow parentWindow, string buttonText, SpriteFont font, Color textColor, Texture2D backgroundImg)
            : base(bounds, onLeftClick, onRightClick, parentWindow)
        {
            _buttonText = buttonText;
            _font = font;
            _textColor = textColor;
            _backgroundTexture = backgroundImg;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_backgroundTexture, _bounds, Color.White);

            var textSize = _font.MeasureString(_buttonText);
            var textPosition = new Vector2(
                _bounds.X + (_bounds.Width - textSize.X) / 2,
                _bounds.Y + (_bounds.Height - textSize.Y) / 2
            );
            spriteBatch.DrawString(_font, _buttonText, textPosition, _textColor);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

            if (IsClicked(mousePosition))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    _onLeftClick?.Execute();
                }
                else if (mouseState.RightButton == ButtonState.Pressed)
                {
                    _onRightClick?.Execute();
                }
            }
        }
    }

    public class IconButton : BaseButton
    {
        private Texture2D _icon;

        public IconButton(Rectangle bounds, ICommand onLeftClick, ICommand onRightClick, BaseWindow parentWindow, Texture2D icon)
            : base(bounds, onLeftClick, onRightClick, parentWindow)
        {
            _icon = icon;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_icon, _bounds, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

            if (IsClicked(mousePosition))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    _onLeftClick?.Execute();
                }
                else if (mouseState.RightButton == ButtonState.Pressed)
                {
                    _onRightClick?.Execute();
                }
            }
        }
    }
}
