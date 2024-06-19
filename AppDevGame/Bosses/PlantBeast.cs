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
            AudioManager.GetInstance(MainApp.GetInstance().Content).PlaySoundEffect("plantbeast_attack");
            // Implement specific attack logic for PlantBeast
            base.Attack(target);
        }

        public override void TakeDamage(int damage)
        {
            AudioManager.GetInstance(MainApp.GetInstance().Content).PlaySoundEffect("plantbeast_damage");
            base.TakeDamage(damage);
        }

        public override void ResolveCollision(Entity other)
        {
            if (CurrentHealth <= 0)
            {
                AudioManager.GetInstance(MainApp.GetInstance().Content).PlaySoundEffect("plantbeast_die");
            }
            base.ResolveCollision(other);
        }
    }
}