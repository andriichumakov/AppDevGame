using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppDevGame
{
    public class Slider : UIElement, IClickable
    {
        private float _value;
        private Texture2D _knobTexture;
        private bool _isDragging;

        public Slider(Rectangle bounds, Color backgroundColor, Color textColor, string text, float initialValue) 
            : base(bounds, new Texture2D(MainApp.GetInstance().GraphicsDevice, 1, 1), backgroundColor, textColor, text)
        {
            _texture = new Texture2D(MainApp.GetInstance().GraphicsDevice, 1, 1);
            _texture.SetData(new[] { backgroundColor });
            _knobTexture = new Texture2D(MainApp.GetInstance().GraphicsDevice, 1, 1);
            _knobTexture.SetData(new[] { Color.Black });
            _value = initialValue;
        }

        public float Value
        {
            get { return _value; }
            set { _value = MathHelper.Clamp(value, 0f, 1f); }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _bounds, _backgroundColor);
            var knobPosition = new Vector2(_bounds.X + _value * _bounds.Width, _bounds.Y);
            spriteBatch.Draw(_knobTexture, new Rectangle((int)knobPosition.X - 5, (int)knobPosition.Y - 5, 10, _bounds.Height + 10), Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
            if (_isDragging)
            {
                MouseState mouseState = Mouse.GetState();
                float relativeX = mouseState.X - _bounds.X;
                Value = relativeX / _bounds.Width;

                if (mouseState.LeftButton == ButtonState.Released)
                {
                    _isDragging = false;
                }
            }
        }

        public void HandleClick(Point mousePosition, GameTime gameTime)
        {
            if (_bounds.Contains(mousePosition))
            {
                _isDragging = true;
            }
        }
    }
}