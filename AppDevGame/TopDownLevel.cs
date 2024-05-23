using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class TopDownLevel : LevelWindow
    {
        public TopDownLevel(int frameWidth, int frameHeight, int actualWidth, int actualHeight, Texture2D background = null)
            : base(frameWidth, frameHeight, actualWidth, actualHeight, background)
        {
        }

        public override void Setup()
        {
            base.Setup();
            // Initialize top-down specific elements here
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Update top-down specific logic here
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            // Draw top-down specific elements here
        }
    }
}
