using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Checkbox : UIElement
    {
        private bool _isChecked;
        private Texture2D _checkedTexture;
        private Texture2D _uncheckedTexture;
        private SpriteFont _font;
        private ICommand _onClick;

        public Checkbox(Rectangle bounds, Texture2D checkedTexture, Texture2D uncheckedTexture, Color backgroundColor, Color textColor, string text, SpriteFont font, ICommand onClick)
            : base(bounds, uncheckedTexture, backgroundColor, textColor, text)
        {
            _checkedTexture = checkedTexture;
            _uncheckedTexture = uncheckedTexture;
            _isChecked = false;
            _font = font;
            _onClick = onClick;
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);

            if (_checkedTexture == null)
            {
                _checkedTexture = new Texture2D(graphicsDevice, 1, 1);
                _checkedTexture.SetData(new[] { Color.Green });
            }

            if (_uncheckedTexture == null)
            {
                _uncheckedTexture = new Texture2D(graphicsDevice, 1, 1);
                _uncheckedTexture.SetData(new[] { Color.Red });
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (/* Check for input to toggle */ false) // Replace with your input handling logic
            {
                _isChecked = !_isChecked;
                _texture = _isChecked ? _checkedTexture : _uncheckedTexture;
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

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                _texture = _isChecked ? _checkedTexture : _uncheckedTexture;
            }
        }

        public Texture2D CheckedTexture
        {
            get { return _checkedTexture; }
            set { _checkedTexture = value; }
        }

        public Texture2D UncheckedTexture
        {
            get { return _uncheckedTexture; }
            set { _uncheckedTexture = value; }
        }

        public void SetChecked(bool isChecked)
        {
            IsChecked = isChecked;
        }
    }
}