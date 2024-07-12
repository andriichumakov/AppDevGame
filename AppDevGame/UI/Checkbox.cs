using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppDevGame
{
    public class Checkbox : UIElement
    {
        private bool _isChecked;
        private Texture2D _checkedTexture;
        private Texture2D _uncheckedTexture;
        private SpriteFont _font;
        private ICommand _onClick;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                _texture = _isChecked ? _checkedTexture : _uncheckedTexture;
            }
        }

        public Checkbox(GraphicsDevice graphicsDevice, Rectangle bounds, Color checkedColor, Color uncheckedColor, Color backgroundColor, Color textColor, string text, SpriteFont font, ICommand onClick)
            : base(bounds, new Texture2D(graphicsDevice, 1, 1), backgroundColor, textColor, text)
        {
            _checkedTexture = new Texture2D(graphicsDevice, 1, 1);
            _checkedTexture.SetData(new[] { checkedColor });

            _uncheckedTexture = new Texture2D(graphicsDevice, 1, 1);
            _uncheckedTexture.SetData(new[] { uncheckedColor });

            _texture = _uncheckedTexture;
            _font = font;
            _onClick = onClick;
            IsChecked = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && _bounds.Contains(Mouse.GetState().Position))
            {
                IsChecked = !IsChecked;
                _onClick?.Execute();
                Console.WriteLine($"Checkbox state changed to: {_isChecked}");
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_texture == null)
            {
                Console.WriteLine("Error: Checkbox texture is null.");
                return;
            }

            spriteBatch.Draw(_texture, _bounds, Color.White);

            if (_font != null)
            {
                string displayText = IsChecked ? "On" : "Off";
                var textSize = _font.MeasureString(displayText);
                var textPosition = new Vector2(_bounds.X + (_bounds.Width - textSize.X) / 2, _bounds.Y + (_bounds.Height - textSize.Y) / 2);
                spriteBatch.DrawString(_font, displayText, textPosition, _textColor);
                Console.WriteLine("Drawing Checkbox");
            }
            else
            {
                MainApp.Log("Error: Font not loaded.");
            }
        }
    }
}