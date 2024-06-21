using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public abstract class BossEnemy : Enemy
    {
        protected float _speed; // Add this field
        private int _attackRange = 50; // Example attack range in pixels

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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            FollowPlayer(gameTime);
            AttackIfInRange();
        }

        protected void FollowPlayer(GameTime gameTime)
        {
            Vector2 playerPosition = _level.GetPlayer().Position;
            Vector2 direction = playerPosition - _position;
            direction.Normalize();
            _position += direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected void AttackIfInRange()
        {
            Entity player = _level.GetPlayer();
            if (Vector2.Distance(_position, player.Position) <= _attackRange)
            {
                Attack(player);
            }
        }

        public override void Attack(Entity target)
        {
            if (target is Player player)
            {
                player.TakeDamage(Damage);
                PlaySoundWithDelay("plantbeast_attack");
            }
        }
    }
}
