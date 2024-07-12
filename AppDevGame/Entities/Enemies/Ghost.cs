using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AppDevGame
{
    public class Ghost : Enemy
    {
        private Random _random;
        private float _randomMoveTimer;
        private Vector2 _randomMoveDirection;
        private const float RandomMoveInterval = 2f; // Time in seconds before changing direction
        private bool _selfDestruct = true; // Flag to indicate if the enemy should self-destruct on contact
        private int _selfDestructDamage = 1; // Damage dealt by self-destruct

        private AnimatedSprite _ghostSprite = new AnimatedSprite(MainApp.GetInstance()._imageLoader.GetResource("Ghost"), 6, 0.1, 'R', 2.0f);

        public Ghost(LevelWindow level, Vector2 position, int maxHealth, int damage, float speed = 100f, float scale = 2.0f)
            : base(level, position, EntityType.Enemy, maxHealth, damage, scale)
        {
            _random = new Random();
            _speed = speed;
            _randomMoveTimer = RandomMoveInterval;
            SetRandomDirection();
            
            AddSprite(_ghostSprite);
            Vector2 spriteSize = _ghostSprite.GetSize();
            _hitbox = new Rectangle(0, 0, (int) spriteSize.X, (int) spriteSize.Y);

            AddCollidableType(EntityType.Player);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float distanceToPlayer = Vector2.Distance(_position, _level.Player.GetPosition());

            if (distanceToPlayer < 300)
            {
                MoveTowardsPlayer(gameTime);
            }
            else
            {
                RandomMove(gameTime);
            }

            _position.X = Math.Clamp(_position.X, 0, _level.ActualSize.Width - _hitbox.Width);
            _position.Y = Math.Clamp(_position.Y, 0, _level.ActualSize.Height - _hitbox.Height);
            _hitbox.Location = _position.ToPoint();
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Vector2 drawPosition = _position - offset;
            DrawHealthBar(spriteBatch, drawPosition);
            base.Draw(spriteBatch, offset);
        }

        public override void Attack(Entity target)
        {
            PlaySoundWithDelay("enemy_attack");
            base.Attack(target);
        }

        public override void OnCollision(Entity other)
        {
            if (other is Player player)
            {
                if (_selfDestruct)
                {
                    player.TakeDamage(_selfDestructDamage);
                    _level.RemoveEntity(this); // Remove the enemy from the level
                }
                else
                {
                    Attack(player);
                }
            }
            base.OnCollision(other);
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
