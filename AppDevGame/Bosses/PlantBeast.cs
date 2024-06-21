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
            FollowPlayer(gameTime);
            UpdateHitbox();  // Ensure the hitbox is updated based on the current position
        }

        private void FollowPlayer(GameTime gameTime)
        {
            Vector2 playerPosition = _level.GetPlayer().Position;
            Vector2 direction = playerPosition - _position;
            direction.Normalize();
            _position += direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void UpdateHitbox()
        {
            // Set hitbox dimensions
            int hitboxWidth = 58; // Width of the hitbox in pixels
            int hitboxHeight = 44; // Height of the hitbox in pixels
            
            // Center the hitbox based on the current position
            _hitbox = new Rectangle(
                (int)(_position.X - hitboxWidth / 2), 
                (int)(_position.Y - hitboxHeight / 2), 
                hitboxWidth, 
                hitboxHeight
            );
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            _walkAnimation.Draw(spriteBatch, _position - offset, _scale, SpriteEffects.None);
            DrawHealthBar(spriteBatch, _position - offset);  // Drawing the health bar at the adjusted size
        }

        public override void Attack(Entity target)
        {
            AudioManager.GetInstance(MainApp.GetInstance().Content).PlaySoundEffect("plantbeast_attack");
            if (target is Player player)
            {
                player.TakeDamage(Damage);
            }
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
