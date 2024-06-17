using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Label : UIElement
    {
        private SpriteFont _font;

        public Label(Rectangle bounds, Color backgroundColor, Color textColor, string text, SpriteFont font)
            : base(bounds, null, backgroundColor, textColor, text)
        {
            _font = font;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_font != null)
            {
                Vector2 textSize = _font.MeasureString(_text);
                Vector2 textPosition = new Vector2(_bounds.X, _bounds.Y + (_bounds.Height - textSize.Y) / 2);
                spriteBatch.DrawString(_font, _text, textPosition, _textColor);
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Update logic if needed
        }
    }
}
