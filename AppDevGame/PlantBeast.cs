using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class PlantBeast : BossEnemy
    {
        public PlantBeast(LevelWindow level, Texture2D texture, Vector2 position, int maxHealth, int damage, float speed, float scale)
            : base(level, texture, position, maxHealth, damage, speed, scale, "PlantBeast")
        {
        }

        public override void Attack(Entity target)
        {
            // Implement specific attack logic for PlantBeast
            base.Attack(target);
        }
    }
}