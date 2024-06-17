using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public abstract class UIElement
    {
        protected Rectangle _bounds;
        protected Texture2D _texture;
        protected Color _backgroundColor;
        protected Color _textColor;
        protected string _text;
        private bool _isVisible;

        protected UIElement(Rectangle bounds, Texture2D texture, Color backgroundColor, Color textColor, string text)
        {
            _bounds = bounds;
            _texture = texture;
            _backgroundColor = backgroundColor;
            _textColor = textColor;
            _text = text;
            _isVisible = true;
        }

        public virtual void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            // Load content here
        }

        public virtual void UpdateText()
        {
            // Update text logic here
        }

        public virtual void Update(GameTime gameTime)
        {
            // Update logic here
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // Draw logic here
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }
    }
}
