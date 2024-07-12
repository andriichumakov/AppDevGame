using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class TextElement : UIElement
    {
        private SpriteFont _font;
        private string _text;

        public TextElement(Rectangle bounds, string text, SpriteFont font, Color textColor)
            : base(bounds, null, Color.Transparent, textColor, text)
        {
            _font = font;
            _text = text;
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_font != null)
            {
                var textSize = _font.MeasureString(_text);
                var textPosition = new Vector2(_bounds.X, _bounds.Y + (_bounds.Height - textSize.Y) / 2);
                spriteBatch.DrawString(_font, _text, textPosition, _textColor);
            }
            else
            {
                MainApp.Log("Error: Font not loaded.");
            }
        }
    }
}
