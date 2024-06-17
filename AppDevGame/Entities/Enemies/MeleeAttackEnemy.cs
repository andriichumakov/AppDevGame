using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AppDevGame
{
    public class MeleeAttackEnemy : Enemy
    {
        private Random _random;
        private float _speed;
        private float _randomMoveTimer;
        private Vector2 _randomMoveDirection;
        private const float RandomMoveInterval = 2f; // Time in seconds before changing direction
        private bool _selfDestruct; // Flag to indicate if the enemy should self-destruct on contact
        private int _selfDestructDamage = 33; // Damage dealt by self-destruct

        public MeleeAttackEnemy(LevelWindow level, Texture2D texture, Vector2 position, int maxHealth, int damage, float speed = 100f, float scale = 1.5f, bool selfDestruct = false)
            : base(level, texture, position, maxHealth, damage, scale)
        {
            _speed = speed;
            _random = new Random();
            _randomMoveTimer = RandomMoveInterval;
            _selfDestruct = selfDestruct;
            SetRandomDirection();
        }

        public override void OnCollision(Entity other)
        {
            if (other is Player player)
            {
                if (_selfDestruct)
                {
                    // Self-destruct logic
                    player.TakeDamage(_selfDestructDamage);
                    _level.RemoveEntity(this); // Remove the enemy from the level
                }
                else
                {
                    // Melee attack logic
                    Attack(player);
                }
            }
            base.OnCollision(other);
        }

        public override void Attack(Entity target)
        {
            // Implement melee attack logic
            if (Hitbox.Intersects(target.Hitbox))
            {
                if (target is Player player)
                {
                    player.TakeDamage(Damage);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float distanceToPlayer = Vector2.Distance(_position, _level.Player.Position);

            if (distanceToPlayer < 300)
            {
                MoveTowardsPlayer(gameTime);
            }
            else
            {
                RandomMove(gameTime);
            }

            // Ensure the enemy does not move out of the map bounds
            _position.X = Math.Clamp(_position.X, 0, _level.ActualSize.Width - _hitbox.Width);
            _position.Y = Math.Clamp(_position.Y, 0, _level.ActualSize.Height - _hitbox.Height);
            _hitbox.Location = _position.ToPoint();
        }

        private void MoveTowardsPlayer(GameTime gameTime)
        {
            Vector2 direction = _level.Player.Position - _position;
            direction.Normalize();
            _position += direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void RandomMove(GameTime gameTime)
        {
            _randomMoveTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_randomMoveTimer <= 0)
            {
                SetRandomDirection();
                _randomMoveTimer = RandomMoveInterval;
            }
            _position += _randomMoveDirection * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void SetRandomDirection()
        {
            float angle = (float)(_random.NextDouble() * Math.PI * 2);
            _randomMoveDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
    }
}