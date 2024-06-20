using System;
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
        private ICommand _onClick;
        private SpriteFont _font;

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
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _texture = _isChecked ? _checkedTexture : _uncheckedTexture;
            base.Draw(spriteBatch);
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

        public void ToggleState()
        {
            _isChecked = !_isChecked;
            _texture = _isChecked ? _checkedTexture : _uncheckedTexture;
            _onClick.Execute();
        }
    }
}