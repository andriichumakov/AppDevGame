using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class OverlayWindow : BaseWindow
    {
        public OverlayWindow(int width, int height, Texture2D background = null)
            : base(width, height, background)
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var element in _elements)
            {
                element.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_background != null)
            {
                spriteBatch.Draw(_background, new Rectangle(0, 0, _width, _height), Color.White);
            }

            foreach (var element in _elements)
            {
                element.Draw(spriteBatch);
            }
        }
    }
}
