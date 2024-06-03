using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Coin : Entity
    {
        private bool _isCollected;
        private float _scale;

        public Coin(LevelWindow level, Texture2D texture, Vector2 position, float scale = 2.0f)
            : base(level, texture, position)
        {
            _isCollected = false;
            _scale = scale;
            _hitbox = new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * scale), (int)(texture.Height * scale));
        }

        public bool IsCollected
        {
            get => _isCollected;
            set => _isCollected = value;
        }

        public override void OnCollision(Entity other)
        {
            if (other is Player player && !_isCollected)
            {
                _isCollected = true;
                player.CollectCoin();
                _level.RemoveEntity(this);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.Draw(_texture, _position - offset, null, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }
    }
}