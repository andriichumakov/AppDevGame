using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public abstract class Enemy : Entity
    {
        private int _maxHealth;
        private int _currentHealth;
        private int _damage;
        private Texture2D _healthBarTexture;

        public Enemy(LevelWindow level, Texture2D texture, Vector2 position, int maxHealth, int damage)
            : base(level, texture, position)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            _damage = damage;
            _healthBarTexture = new Texture2D(MainApp.GetInstance().GraphicsDevice, 1, 1);
            _healthBarTexture.SetData(new[] { Color.White });
        }

        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;
        public int Damage => _damage;

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth < 0) _currentHealth = 0;
        }

        public bool IsDead()
        {
            return _currentHealth <= 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _hitbox.Location = _position.ToPoint();
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            base.Draw(spriteBatch, offset);

            // Draw health bar
            DrawHealthBar(spriteBatch, _position - offset);
        }

        protected void DrawHealthBar(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            // Health bar parameters
            int barWidth = _hitbox.Width;
            int barHeight = 5; // Thicker health bar
            int barYOffset = 10; // Position the bar above the enemy

            // Calculate health percentage
            float healthPercentage = (float)_currentHealth / _maxHealth;

            // Health bar positions
            Vector2 healthBarPosition = drawPosition + new Vector2(0, -barYOffset);
            Rectangle healthBarBackground = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, barWidth, barHeight);
            Rectangle healthBarForeground = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, (int)(barWidth * healthPercentage), barHeight);

            // Draw the health bar
            spriteBatch.Draw(_healthBarTexture, healthBarBackground, Color.Red); // Background in red
            spriteBatch.Draw(_healthBarTexture, healthBarForeground, Color.Yellow); // Foreground in yellow
        }

        public abstract void Attack(Entity target);

        public override void OnCollision(Entity other)
        {
            base.OnCollision(other);
        }
    }
}
