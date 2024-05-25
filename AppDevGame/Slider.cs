using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppDevGame
{
    public class Slider : UIElement, IClickable
    {
        private bool _isDragging;
        private float _value;
        private SpriteFont _font;
        private Color _foregroundColor;
        private Texture2D _backgroundTexture;

        public float Value
        {
            get { return _value; }
            set { _value = MathHelper.Clamp(value, 0f, 1f); }
        }

        public Slider(Rectangle bounds, Color backgroundColor, Color foregroundColor, string text, float initialValue, SpriteFont font)
            : base(bounds, null, backgroundColor, foregroundColor, text)
        {
            _value = initialValue;
            _font = font;
            _foregroundColor = foregroundColor;
            _backgroundTexture = new Texture2D(MainApp.GetInstance().GraphicsDevice, 1, 1);
            _backgroundTexture.SetData(new[] { backgroundColor });
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the slider background
            spriteBatch.Draw(_backgroundTexture, _bounds, _backgroundColor);

            // Draw the slider handle based on the value
            int handleX = _bounds.X + (int)(_value * _bounds.Width) - 5;
            Rectangle handleBounds = new Rectangle(handleX, _bounds.Y, 10, _bounds.Height);
            spriteBatch.Draw(_backgroundTexture, handleBounds, _foregroundColor);

            // Draw the text if font is available
            if (_font != null)
            {
                Vector2 textSize = _font.MeasureString(_text);
                Vector2 textPosition = new Vector2(_bounds.X, _bounds.Y - textSize.Y - 5);
                spriteBatch.DrawString(_font, _text, textPosition, _textColor);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_isDragging)
            {
                MouseState mouseState = Mouse.GetState();
                float newValue = (float)(mouseState.X - _bounds.X) / _bounds.Width;
                Value = MathHelper.Clamp(newValue, 0f, 1f);
            }
        }

        public void HandleClick(Point clickPosition, GameTime gameTime)
        {
            if (_bounds.Contains(clickPosition))
            {
                _isDragging = true;
            }
        }

        public void HandleRelease(Point releasePosition, GameTime gameTime)
        {
            _isDragging = false;
        }
    }
}