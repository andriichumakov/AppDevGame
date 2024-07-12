using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Heart : Entity
    {
        private Sprite _heartSprite = new Sprite(MainApp.GetInstance()._imageLoader.GetResource("Heart"), 'R', 2.0f);

        public Heart(LevelWindow level, Texture2D texture, Vector2 position)
            : base(level, position, EntityType.Item)
        {
            AddSprite(_heartSprite);
            Vector2 spriteSize = _heartSprite.GetSize();
            _hitbox = new Rectangle(0, 0, (int) spriteSize.X, (int) spriteSize.Y);

            AddCollidableType(EntityType.Player);
        }

        public override void OnCollision(Entity other)
        {
            if (other is Player player)
            {
                player.Heal(1);
                _level.RemoveEntity(this);
                AudioManager.GetInstance(MainApp.GetInstance().Content).PlaySoundEffect("heart_collect");
            }
        }
    }
}