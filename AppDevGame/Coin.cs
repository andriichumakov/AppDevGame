using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Coin : Entity
    {

        public Coin(LevelWindow level, Texture2D texture, Vector2 position)
            : base(level, texture, position, EntityType.Item)
        {
            SetCollidableTypes(EntityType.Player);
        }

        public override void OnCollision(Entity other)
        {
            if (other is Player player)
            {
                player.CollectCoin();
                _level.RemoveEntity(this);
            }
        }
    }
}
