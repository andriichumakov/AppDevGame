using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace AppDevGame
{
    public abstract class UIElement
    {
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

        public virtual void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            // Default implementation to load content for UI elements
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _bounds, _backgroundColor);
        }

        public virtual void Clear()
        {
            _texture.Dispose();
        }
    }

    public class Label : UIElement
    {
        public Label(Rectangle bounds, Color backgroundColor, Color textColor, string text)
            : base(bounds, null, backgroundColor, textColor, text)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var font = MainApp.GetInstance()._fontLoader.GetResource("Default");
            if (font == null)
            {
                MainApp.Log("Error: Font 'Default' not found.");
                return;
            }

            var textSize = font.MeasureString(_text);
            var textPosition = new Vector2(_bounds.X, _bounds.Y);
            spriteBatch.DrawString(font, _text, textPosition, _textColor);
        }

        public override void Update(GameTime gameTime)
        {
            // Labels typically do not need to update
        }
    }
}