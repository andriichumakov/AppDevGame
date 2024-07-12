using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppDevGame
{
    public class Toggle : UIElement
    {
        private bool _isToggled;
        private Texture2D _toggledTexture;
        private Texture2D _untoggledTexture;
        private SpriteFont _font;
        private ICommand _onClick;

        public bool IsToggled
        {
            get { return _isToggled; }
            set
            {
                _isToggled = value;
                _texture = _isToggled ? _toggledTexture : _untoggledTexture;
            }
        }

        public Toggle(GraphicsDevice graphicsDevice, Rectangle bounds, Color toggledColor, Color untoggledColor, Color backgroundColor, Color textColor, string text, SpriteFont font, ICommand onClick)
            : base(bounds, new Texture2D(graphicsDevice, 1, 1), backgroundColor, textColor, text)
        {
            _toggledTexture = new Texture2D(graphicsDevice, 1, 1);
            _toggledTexture.SetData(new[] { toggledColor });

            _untoggledTexture = new Texture2D(graphicsDevice, 1, 1);
            _untoggledTexture.SetData(new[] { untoggledColor });

            _texture = _untoggledTexture;
            _font = font;
            _onClick = onClick;
            IsToggled = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && _bounds.Contains(Mouse.GetState().Position))
            {
                IsToggled = !IsToggled;
                _onClick?.Execute();
                Console.WriteLine($"Toggle state changed to: {_isToggled}");
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_texture == null)
            {
                Console.WriteLine("Error: Toggle texture is null.");
                return;
            }

            spriteBatch.Draw(_texture, _bounds, Color.White);

            if (_font != null)
            {
                string displayText = IsToggled ? "On" : "Off";
                var textSize = _font.MeasureString(displayText);
                var textPosition = new Vector2(_bounds.X + (_bounds.Width - textSize.X) / 2, _bounds.Y + (_bounds.Height - textSize.Y) / 2);
                spriteBatch.DrawString(_font, displayText, textPosition, _textColor);
                Console.WriteLine("Drawing Toggle");
            }
            else
            {
                MainApp.Log("Error: Font not loaded.");
            }
        }
    }
}