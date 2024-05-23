using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using AppDevGame;

namespace AppDevGame
{
    public abstract class UIElement : IDrawable {
        protected Rectangle _bounds { get; set; }
        protected Texture2D _texture { get; set; }
        protected Color _backgroundColor { get; set; }
        protected Color _textColor { get; set; }
        protected string _text { get; set; }

        public UIElement(Rectangle bounds, Texture2D texture, Color backgroundColor, Color textColor, string text)
        {
            _bounds = bounds;
            _texture = texture;
            _backgroundColor = backgroundColor;
            _textColor = textColor;
            _text = text;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _bounds, _backgroundColor);
        }

        public virtual void Update(GameTime gameTime)
        {
            // do nothing
        }

        public virtual void Clear()
        {
            _texture.Dispose();
        }
    }
}