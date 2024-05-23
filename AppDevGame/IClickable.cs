using Microsoft.Xna.Framework;

namespace AppDevGame
{
    public interface IClickable 
    {
        public void HandleClick(Point mousePosition);
    }
}