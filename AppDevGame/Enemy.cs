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
        private bool _hasDroppedCoin = false; // Track if coin has been dropped

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
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                if (!_hasDroppedCoin)
                {
                    DropCoin();
                    _hasDroppedCoin = true;
                }
            }
        }

        private void DropCoin()
        {
            Texture2D coinTexture = MainApp.GetInstance()._imageLoader.GetResource("Coin");
            if (coinTexture != null)
            {
                _level.AddEntity(new Coin(_level, coinTexture, _position));
            }
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
            DrawHealthBar(spriteBatch, _position - offset);
        }

        protected void DrawHealthBar(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            int barWidth = _hitbox.Width;
            int barHeight = 5;
            int barYOffset = 10;
            float healthPercentage = (float)_currentHealth / _maxHealth;

            Vector2 healthBarPosition = drawPosition + new Vector2(0, -barYOffset);
            Rectangle healthBarBackground = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, barWidth, barHeight);
            Rectangle healthBarForeground = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, (int)(barWidth * healthPercentage), barHeight);

            spriteBatch.Draw(_healthBarTexture, healthBarBackground, Color.Red);
            spriteBatch.Draw(_healthBarTexture, healthBarForeground, Color.Yellow);
        }

        public abstract void Attack(Entity target);

        public override void OnCollision(Entity other)
        {
            base.OnCollision(other);
        }
    }
}
