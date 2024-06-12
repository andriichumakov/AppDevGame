using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppDevGame
{
    public class Slider : UIElement
    {
        private float _value;
        private bool _isDragging;
        private Color _foregroundColor;
        private Texture2D _backgroundTexture;
        private Texture2D _handleTexture;
        private readonly string _textKey;
        private SpriteFont _font;
        private string _text;
        private int _radius; // The radius within which the mouse can interact with the slider

        public float Value
        {
            get { return _value; }
            set { _value = MathHelper.Clamp(value, 0f, 1f); }
        }

        public Slider(GraphicsDevice graphicsDevice, Rectangle bounds, Color backgroundColor, Color foregroundColor, string textKey, float initialValue, SpriteFont font, int radius = 50)
            : base(bounds, new Texture2D(graphicsDevice, 1, 1), backgroundColor, foregroundColor, textKey)
        {
            _foregroundColor = foregroundColor;

            _backgroundTexture = new Texture2D(graphicsDevice, 1, 1);
            _backgroundTexture.SetData(new[] { backgroundColor });

            _handleTexture = new Texture2D(graphicsDevice, 1, 1);
            _handleTexture.SetData(new[] { foregroundColor });

            _value = initialValue;
            _textKey = textKey;
            _font = font;
            _radius = radius;
            UpdateText();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the background of the slider
            spriteBatch.Draw(_backgroundTexture, _bounds, _backgroundColor);

            // Calculate and draw the fill part of the slider
            int handleX = _bounds.X + (int)(_value * _bounds.Width) - 5;
            Rectangle handleBounds = new Rectangle(handleX, _bounds.Y, 10, _bounds.Height);
            spriteBatch.Draw(_handleTexture, handleBounds, _foregroundColor);

            // Draw the text above the slider
            if (_font != null)
            {
                var textSize = _font.MeasureString(_text);
                var textPosition = new Vector2(_bounds.X + (_bounds.Width - textSize.X) / 2, _bounds.Y - textSize.Y - 5);
                spriteBatch.DrawString(_font, _text, textPosition, _textColor);
            }
            else
            {
                MainApp.Log("Error: Font not loaded.");
            }
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Point mousePosition = mouseState.Position;

            if (_isDragging)
            {
                float newValue = (float)(mouseState.X - _bounds.X) / _bounds.Width;
                Value = MathHelper.Clamp(newValue, 0f, 1f);

                // Stop dragging if the mouse button is released
                if (mouseState.LeftButton == ButtonState.Released)
                {
                    _isDragging = false;
                }
            }
            else if (mouseState.LeftButton == ButtonState.Pressed && IsMouseWithinRadius(mousePosition))
            {
                // Start dragging if the mouse button is pressed within the slider bounds and within radius
                if (_bounds.Contains(mouseState.Position))
                {
                    _isDragging = true;
                }
            }
        }

        private bool IsMouseWithinRadius(Point mousePosition)
        {
            Point sliderCenter = new Point(_bounds.X + _bounds.Width / 2, _bounds.Y + _bounds.Height / 2);
            return Vector2.Distance(new Vector2(mousePosition.X, mousePosition.Y), new Vector2(sliderCenter.X, sliderCenter.Y)) <= _radius;
        }

        public void SetValue(float value)
        {
            _value = value;
        }

        public override void UpdateText()
        {
            _text = MainApp.GetInstance().LocLoader.GetString(_textKey);
        }
    }
}