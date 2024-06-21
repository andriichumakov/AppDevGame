using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class PlantBeast : BossEnemy
    {
        private AnimatedSprite _walkAnimation;

        public PlantBeast(LevelWindow level, Texture2D walkTexture, Vector2 position, int maxHealth, int damage, float speed, float scale)
            : base(level, walkTexture, position, maxHealth, damage, speed, scale, "PlantBeast")
        {
            _walkAnimation = new AnimatedSprite(walkTexture, 6, 0.1);
            UpdateHitbox();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _walkAnimation.Update(gameTime);
        }

        protected void UpdateHitbox()
        {
            // Manually adjust hitbox size here
            int hitboxWidth = (int)(_walkAnimation.FrameWidth * _scale * 0.5f);  // 0.5f is an example scaling factor
            int hitboxHeight = (int)(_walkAnimation.FrameHeight * _scale * 0.5f); // 0.5f is an example scaling factor
            _hitbox = new Rectangle((int)(_position.X - hitboxWidth / 2), (int)(_position.Y - hitboxHeight / 2), hitboxWidth, hitboxHeight);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            _walkAnimation.Draw(spriteBatch, _position - offset, _scale, SpriteEffects.None);
            DrawHealthBar(spriteBatch, _position - offset);  // Drawing the health bar at the adjusted size
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
