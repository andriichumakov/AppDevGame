using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AppDevGame
{
    public abstract class BossEnemy : Enemy
    {
        private string _name;
        private float _speed;
        private Random _random;
        private bool _isSpawned;

        public BossEnemy(LevelWindow level, Texture2D texture, Vector2 position, int maxHealth, int damage, float speed, float scale, string name)
            : base(level, texture, position, maxHealth, damage, scale)
        {
            _speed = speed;
            _name = name;
            _random = new Random();
            _isSpawned = false;
        }

        public string Name => _name;

        public void SpawnNearPlayer()
        {
            Vector2 playerPosition = _level.Player.Position;
            float distance = 300; // Distance from the player to spawn the boss

            Vector2 randomOffset = new Vector2(
                _random.Next((int)-distance, (int)distance),
                _random.Next((int)-distance, (int)distance)
            );

            Vector2 spawnPosition = playerPosition + randomOffset;

            // Ensure the boss doesn't spawn outside the level bounds
            spawnPosition.X = Math.Clamp(spawnPosition.X, 0, _level.ActualSize.Width - _hitbox.Width);
            spawnPosition.Y = Math.Clamp(spawnPosition.Y, 0, _level.ActualSize.Height - _hitbox.Height);

            _position = spawnPosition;
            _hitbox.Location = _position.ToPoint();
            _isSpawned = true;

            MainApp.Log($"{_name} spawned at {spawnPosition}");
        }

        public override void Update(GameTime gameTime)
        {
            if (_isSpawned)
            {
                MoveTowardsPlayer(gameTime);
            }

            base.Update(gameTime);
        }

        private void MoveTowardsPlayer(GameTime gameTime)
        {
            Vector2 direction = _level.Player.Position - _position;
            direction.Normalize();
            _position += direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _hitbox.Location = _position.ToPoint();
        }
        public virtual void SpawnNearPortal(Vector2 portalPosition)
        {
            // Default implementation to spawn near the portal
            Vector2 spawnPosition = portalPosition + new Vector2(50, 50); // Adjust the offset as needed
            _position = spawnPosition;
            _hitbox.Location = _position.ToPoint();
            MainApp.Log($"{Name} spawned near the portal.");
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            base.Draw(spriteBatch, offset);

            // Draw boss name above the boss
            SpriteFont font = MainApp.GetInstance()._fontLoader.GetResource("Default");
            if (font != null)
            {
                Vector2 namePosition = _position - offset + new Vector2(0, -20);
                spriteBatch.DrawString(font, _name, namePosition, Color.Red);
            }
        }

        public override void OnCollision(Entity other)
        {
            if (other is Player)
            {
                Attack(other);
            }
            base.OnCollision(other);
        }

        public override void Attack(Entity target)
        {
            // Default attack logic
            if (Hitbox.Intersects(target.Hitbox))
            {
                if (target is Player player)
                {
                    player.TakeDamage(Damage);
                }
            }
        }
    }
}