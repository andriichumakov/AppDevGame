using Microsoft.Xna.Framework;

namespace AppDevGame
{
    public interface IClickable
    {
        void HandleClick(Point mousePosition, GameTime gameTime);
    }
}