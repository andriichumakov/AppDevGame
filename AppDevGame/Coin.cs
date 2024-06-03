using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Coin : Entity
    {
        private bool _isCollected;

        public Coin(LevelWindow level, Texture2D texture, Vector2 position)
            : base(level, texture, position)
        {
            _isCollected = false;
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
    }
}
