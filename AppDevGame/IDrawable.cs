using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using AppDevGame;

namespace AppDevGame
{
    public interface IDrawable {
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
    }
}