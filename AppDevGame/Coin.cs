using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Coin : Entity
    {
        public Coin(LevelWindow level, Texture2D texture, Vector2 position)
            : base(level, texture, position)
        {
            _hitbox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void OnCollision(Entity other)
        {
            if (other is Player player)
            {
                player.CollectCoin();
                _level.RemoveEntity(this);
            }
            // Do not call base.OnCollision(other) to ignore collisions with other entities
        }
    }
}
