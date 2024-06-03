using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Heart : Entity
    {
        private bool _isCollected;

        public Heart(LevelWindow level, Texture2D texture, Vector2 position)
            : base(level, texture, position)
        {
            _hitbox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            _isCollected = false;
        }

        public bool IsCollected
        {
            get => _isCollected;
            set => _isCollected = value;
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