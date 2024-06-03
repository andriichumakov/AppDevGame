using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Heart : Entity
    {
        public Heart(LevelWindow level, Texture2D texture, Vector2 position)
            : base(level, texture, position)
        {
            // Disable the hitbox for collision detection
            _hitbox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public override void Update(GameTime gameTime)
        {
            // No update logic needed for the heart pickup
        }

        public override void OnCollision(Entity other)
        {
            // No collision logic needed for the heart pickup
        }
    }
}
