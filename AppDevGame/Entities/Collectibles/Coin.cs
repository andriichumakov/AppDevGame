using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Coin : Entity
    {

        private Sprite _coinSprite = new AnimatedSprite(MainApp.GetInstance()._imageLoader.GetResource("coin1"), 10, 0.1, 'R', 1);

        public Coin(LevelWindow level, Vector2 position, float scale = 2.0f, int frameWidth = 16, int frameHeight = 16, int frameCount = 10, double frameTime = 0.1)
            : base(level, position, EntityType.Item)
        {
            AddSprite(_coinSprite);
            Vector2 spriteSize = _coinSprite.GetSize();
            _hitbox = new Rectangle(0, 0, (int) spriteSize.X, (int) spriteSize.Y);

            AddCollidableType(EntityType.Player);
        }

        public override void OnCollision(Entity other)
        {
            if (other is Player player)
            {
                player.CollectCoin();
                _level.RemoveEntity(this);
                AudioManager.GetInstance(MainApp.GetInstance().Content).PlaySoundEffect("coin_collect");
            }
        }
    }
}
