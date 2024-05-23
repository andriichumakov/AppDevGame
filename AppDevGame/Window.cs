using Microsoft.Xna.Framework;

namespace AppDevGame
{
    public abstract class BaseWindow : IDrawable
    {
        public abstract void Setup();
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
        public abstract void Clear();
    }

    public class MenuWindow : BaseWindow 
    {
        public override void Setup() 
        {
            // Setup menu window
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw menu window
        }

        public override void Update(GameTime gameTime)
        {
            // Update menu window
        }

        public override void Clear() 
        {
            // Clear menu window
        }
    }

    public class LevelWindow : BaseWindow 
    {
        public override void Setup() 
        {
            // Setup level window
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw level window
        }

        public override void Update(GameTime gameTime)
        {
            // Update level window
        }

        public override void Clear() 
        {
            // Clear level window
        }
    }
}
