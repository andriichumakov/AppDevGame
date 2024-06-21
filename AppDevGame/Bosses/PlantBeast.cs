using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class PlantBeast : BossEnemy
    {
        private AnimatedSprite _walkAnimation;
        private AnimatedSprite _attackAnimation;
        private bool _isAttacking;

        public PlantBeast(LevelWindow level, Texture2D walkTexture, Texture2D attackTexture, Vector2 position, int maxHealth, int damage, float speed, float scale)
            : base(level, walkTexture, position, maxHealth, damage, speed, scale, "PlantBeast")
        {
            _walkAnimation = new AnimatedSprite(walkTexture, 6, 0.1); // Adjust frame count and time per frame as needed
            _attackAnimation = new AnimatedSprite(attackTexture, 6, 0.1); // Adjust frame count and time per frame as needed
            _isAttacking = false;

            UpdateHitbox();
        }

        public override void Attack(Entity target)
        {
            AudioManager.GetInstance(MainApp.GetInstance().Content).PlaySoundEffect("plantbeast_attack");
            _isAttacking = true;
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_isAttacking)
            {
                _attackAnimation.Update(gameTime);
                if (_attackAnimation.IsComplete)
                {
                    _isAttacking = false;
                }
            }
            else
            {
                _walkAnimation.Update(gameTime);
            }

            UpdateHitbox();
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (_isAttacking)
            {
                _attackAnimation.Draw(spriteBatch, _position - offset, _scale, SpriteEffects.None);
            }
            else
            {
                _walkAnimation.Draw(spriteBatch, _position - offset, _scale, SpriteEffects.None);
            }

            DrawHealthBar(spriteBatch, _position - offset); // Adjust as needed
        }

        protected void UpdateHitbox()
        {
            int hitboxWidth = (int)(_texture.Width * _scale / 6); // Assuming 6 frames in the sprite sheet
            int hitboxHeight = (int)(_texture.Height * _scale);
            _hitbox = new Rectangle((int)_position.X - hitboxWidth / 2, (int)_position.Y - hitboxHeight / 2, hitboxWidth, hitboxHeight);
        }

        protected void DrawHealthBar(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            int barWidth = (int)(_hitbox.Width * 0.5f); // Make the health bar half the width of the hitbox
            int barHeight = 5;
            int barYOffset = 10;
            float healthPercentage = (float)CurrentHealth / MaxHealth;

            Vector2 healthBarPosition = drawPosition + new Vector2(-_hitbox.Width / 2, -(_hitbox.Height / 2 + barYOffset));
            Rectangle healthBarBackground = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, barWidth, barHeight);
            Rectangle healthBarForeground = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, (int)(barWidth * healthPercentage), barHeight);

            spriteBatch.Draw(_healthBarTexture, healthBarBackground, Color.Red);
            spriteBatch.Draw(_healthBarTexture, healthBarForeground, Color.Yellow);
        }
    }
}
