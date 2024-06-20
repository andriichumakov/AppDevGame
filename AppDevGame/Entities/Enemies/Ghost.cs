using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AppDevGame
{
    public class Ghost : Enemy
    {
        private Texture2D _texture;
        private int _frame;
        private double _frameTime;
        private const double FrameDuration = 0.1; // Adjust the speed of the animation
        private int _frameCount = 6;
        private int _frameWidth = 16; // Adjust based on the sprite sheet frame width
        private int _frameHeight = 16; // Adjust based on the sprite sheet frame height
        private Random _random;
        private float _speed;
        private float _randomMoveTimer;
        private Vector2 _randomMoveDirection;
        private const float RandomMoveInterval = 2f; // Time in seconds before changing direction
        private bool _selfDestruct; // Flag to indicate if the enemy should self-destruct on contact
        private int _selfDestructDamage = 33; // Damage dealt by self-destruct

        public Ghost(LevelWindow level, Texture2D texture, Vector2 position, int maxHealth, int damage, float speed = 100f, float scale = 2.0f, bool selfDestruct = true)
            : base(level, texture, position, maxHealth, damage, scale)
        {
            _texture = texture;
            _frame = 0;
            _frameTime = 0;
            _random = new Random();
            _speed = speed;
            _randomMoveTimer = RandomMoveInterval;
            _selfDestruct = selfDestruct;
            SetRandomDirection();
            UpdateHitbox();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _frameTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (_frameTime >= FrameDuration)
            {
                _frame = (_frame + 1) % _frameCount;
                _frameTime = 0;
            }

            float distanceToPlayer = Vector2.Distance(_position, _level.Player.Position);

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
            int frameX = _frame * _frameWidth;
            Rectangle sourceRectangle = new Rectangle(frameX, 0, _frameWidth, _frameHeight);
            Vector2 drawPosition = _position - offset;

            spriteBatch.Draw(_texture, drawPosition, sourceRectangle, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
            DrawHealthBar(spriteBatch, drawPosition);
        }

        protected override void DrawHealthBar(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            int barWidth = (int)(_hitbox.Width * 0.5f); // Adjusted to make the health bar smaller
            int barHeight = 3; // Adjusted height for the health bar
            int barYOffset = 10;
            float healthPercentage = (float)CurrentHealth / MaxHealth;

            Vector2 healthBarPosition = drawPosition + new Vector2(-barWidth / 2, -(_hitbox.Height / 2 + barYOffset));
            Rectangle healthBarBackground = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, barWidth, barHeight);
            Rectangle healthBarForeground = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, (int)(barWidth * healthPercentage), barHeight);

            spriteBatch.Draw(_healthBarTexture, healthBarBackground, Color.Red);
            spriteBatch.Draw(_healthBarTexture, healthBarForeground, Color.Yellow);
        }

        private void UpdateHitbox()
        {
            int hitboxWidth = (int)(_frameWidth * _scale);
            int hitboxHeight = (int)(_frameHeight * _scale);
            _hitbox = new Rectangle((int)(_position.X - hitboxWidth / 2), (int)(_position.Y - hitboxHeight / 2), hitboxWidth, hitboxHeight);
        }

        public override void Attack(Entity target)
        {
            if (Hitbox.Intersects(target.Hitbox))
            {
                if (target is Player player)
                {
                    player.TakeDamage(Damage);
                    PlaySoundWithDelay("enemy_attack");
                }
            }
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
