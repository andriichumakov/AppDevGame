using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Button : UIElement
    {
        protected ICommand _onClick;
        
        public Button(Rectangle bounds, Color backgroundColor, Color textColor, string text, ICommand onClick) 
            : base(bounds, new Texture2D(MainApp.GetInstance().GraphicsDevice, 1, 1), backgroundColor, textColor, text)
        {
            _texture = new Texture2D(MainApp.GetInstance().GraphicsDevice, 1, 1);
            _texture.SetData(new[] { backgroundColor });
            _onClick = onClick;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _bounds, _backgroundColor);
            var font = MainApp.GetInstance()._fontLoader.GetResource("Default"); // Assuming a font named Default is loaded
            if (font == null)
            {
                MainApp.Log("Error: Font 'Default' not found.");
                return;
            }
            var textSize = font.MeasureString(_text);
            var textPosition = new Vector2(_bounds.X + (_bounds.Width - textSize.X) / 2, _bounds.Y + (_bounds.Height - textSize.Y) / 2);
            spriteBatch.DrawString(font, _text, textPosition, _textColor);
        }

        public override void Update(GameTime gameTime)
        {
            // Handle button updates here
        }
        
        public void OnClick()
        {
            _onClick.Execute();
        }
    }
}