using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Toggle : UIElement
    {
        private bool _isToggled;
        private Texture2D _toggledTexture;
        private Texture2D _untoggledTexture;
        private SpriteFont _font;
        private ICommand _onClick;

        public Toggle(Rectangle bounds, Texture2D toggledTexture, Texture2D untoggledTexture, Color backgroundColor, Color textColor, string text, SpriteFont font, ICommand onClick)
            : base(bounds, untoggledTexture, backgroundColor, textColor, text)
        {
            _toggledTexture = toggledTexture;
            _untoggledTexture = untoggledTexture;
            _isToggled = false;
            _font = font;
            _onClick = onClick;
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);

            if (_toggledTexture == null)
            {
                _toggledTexture = new Texture2D(graphicsDevice, 1, 1);
                _toggledTexture.SetData(new[] { Color.Green });
            }

            if (_untoggledTexture == null)
            {
                _untoggledTexture = new Texture2D(graphicsDevice, 1, 1);
                _untoggledTexture.SetData(new[] { Color.Red });
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (/* Check for input to toggle */ false) // Replace with your input handling logic
            {
                _isToggled = !_isToggled;
                _texture = _isToggled ? _toggledTexture : _untoggledTexture;
                _onClick.Execute();
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (_font != null)
            {
                var textSize = _font.MeasureString(_text);
                var textPosition = new Vector2(_bounds.X + (_bounds.Width - textSize.X) / 2, _bounds.Y + (_bounds.Height - textSize.Y) / 2);
                spriteBatch.DrawString(_font, _text, textPosition, _textColor);
            }
            else
            {
                MainApp.Log("Error: Font not loaded.");
            }
        }

        public bool IsToggled
        {
            get { return _isToggled; }
            set
            {
                _isToggled = value;
                _texture = _isToggled ? _toggledTexture : _untoggledTexture;
            }
        }
    }
}