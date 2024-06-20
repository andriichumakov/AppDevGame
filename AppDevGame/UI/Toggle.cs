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
        private ICommand _onClick;
        private SpriteFont _font;

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
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _texture = _isToggled ? _toggledTexture : _untoggledTexture;
            base.Draw(spriteBatch);
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

        public void ToggleState()
        {
            _isToggled = !_isToggled;
            _texture = _isToggled ? _toggledTexture : _untoggledTexture;
            _onClick.Execute();
        }
    }
}