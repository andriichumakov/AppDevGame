using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public abstract class BossEnemy : Enemy
    {
        protected float _speed; // Add this field

        public string Name { get; private set; }

        public BossEnemy(LevelWindow level, Texture2D texture, Vector2 position, int maxHealth, int damage, float speed, float scale, string name)
            : base(level, texture, position, maxHealth, damage, scale)
        {
            _speed = speed; // Initialize the speed
            Name = name;
        }

        public void SpawnNearPortal(Vector2 portalPosition)
        {
            _position = portalPosition + new Vector2(100, 0);  // Example offset from portal
            UpdateHitbox();  // Ensure hitbox is updated based on the new position
        }
    }
}
