using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AppDevGame
{
    public class Frog : Enemy
    {
        private AnimatedSprite _jumpingAnimation;
        private float _speed;
        private Random _random;
        private SpriteEffects _spriteEffect;
        private double _attackCooldown;
        private const double AttackCooldownDuration = 1.0; // 1 second cooldown

        public Frog(LevelWindow level, Texture2D texture, Vector2 position, int maxHealth, int damage, float speed = 100f, float scale = 1.5f)
            : base(level, texture, position, maxHealth, damage, scale)
        {
            _jumpingAnimation = new AnimatedSprite(texture, 4, 0.1); // 4 frames, 0.1 seconds per frame
            _speed = speed;
            _random = new Random();
            _spriteEffect = SpriteEffects.None;
            _attackCooldown = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MoveTowardsPlayer(gameTime);

            _jumpingAnimation.Update(gameTime);
            _attackCooldown -= gameTime.ElapsedGameTime.TotalSeconds;

            if (_attackCooldown <= 0 && Hitbox.Intersects(_level.Player.Hitbox))
            {
                Attack(_level.Player);
                _attackCooldown = AttackCooldownDuration;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Vector2 drawPosition = _position - offset;
            _jumpingAnimation.Draw(spriteBatch, drawPosition, _scale, _spriteEffect);
        }

        public override void Attack(Entity target)
        {
            // Implement attack logic
            if (target is Player player)
            {
                player.TakeDamage(Damage);
                PlaySoundWithDelay("frog_attack"); // Make sure the sound is defined and loaded
            }
        }

        private void MoveTowardsPlayer(GameTime gameTime)
        {
            Player player = _level.Player;
            if (player != null)
            {
                Vector2 direction = player.Position - _position;
                direction.Normalize();
                _position += direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Update sprite effect based on direction
                if (direction.X < 0)
                {
                    _spriteEffect = SpriteEffects.FlipHorizontally;
                }
                else
                {
                    _spriteEffect = SpriteEffects.None;
                }
            }
        }
    }
}
