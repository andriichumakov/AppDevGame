using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AppDevGame
{
    public abstract class Enemy : Entity
    {
        private int _maxHealth;
        private int _currentHealth;
        private int _damage;
        private Texture2D _healthBarTexture;
        private Dictionary<string, double> _lastSoundTimes = new Dictionary<string, double>();
        private bool _hasDroppedCoin = false; // Track if coin has been dropped
        private float _scale;

        public Enemy(LevelWindow level, Texture2D texture, Vector2 position, int maxHealth, int damage, float scale = 1.5f)
            : base(level, texture, position, EntityType.Enemy)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            _damage = damage;
            _scale = scale;
            _healthBarTexture = new Texture2D(MainApp.GetInstance().GraphicsDevice, 1, 1);
            _healthBarTexture.SetData(new[] { Color.White });
            SetCollidableTypes(EntityType.Player, EntityType.Obstacle);

            // Initialize the hitbox to match the scaled size of the enemy image
            UpdateHitbox();
        }

        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;
        public int Damage => _damage;

        public virtual void TakeDamage(int damage)
        {
            PlaySoundWithDelay("enemy_damage");
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                PlaySoundWithDelay("enemy_die");
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
                AudioManager.GetInstance(MainApp.GetInstance().Content).PlaySoundEffect("coin_collect");
            }
        }

        public bool IsDead()
        {
            return _currentHealth <= 0;
        }

        private void UpdateHitbox()
        {
            int hitboxWidth = (int)(_texture.Width * _scale);
            int hitboxHeight = (int)(_texture.Height * _scale);
            _hitbox = new Rectangle((int)(_position.X - hitboxWidth / 2), (int)(_position.Y - hitboxHeight / 2), hitboxWidth, hitboxHeight);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateHitbox();
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Vector2 drawPosition = _position - offset;
            spriteBatch.Draw(_texture, drawPosition, null, Color.White, 0f, new Vector2(_texture.Width / 2, _texture.Height / 2), _scale, SpriteEffects.None, 0f);
            DrawHealthBar(spriteBatch, drawPosition);
        }

        protected void DrawHealthBar(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            int barWidth = (int)(_hitbox.Width * _scale);
            int barHeight = 5;
            int barYOffset = 10;
            float healthPercentage = (float)_currentHealth / _maxHealth;

            Vector2 healthBarPosition = drawPosition + new Vector2(-_hitbox.Width / 2, -(_hitbox.Height / 2 + barYOffset));
            Rectangle healthBarBackground = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, barWidth, barHeight);
            Rectangle healthBarForeground = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, (int)(barWidth * healthPercentage), barHeight);

            spriteBatch.Draw(_healthBarTexture, healthBarBackground, Color.Red);
            spriteBatch.Draw(_healthBarTexture, healthBarForeground, Color.Yellow);
        }

        public abstract void Attack(Entity target);

        protected void PlaySoundWithDelay(string soundName)
        {
            double currentTime = MainApp.GetInstance().TotalGameTime.TotalSeconds;
            if (!_lastSoundTimes.ContainsKey(soundName) || currentTime - _lastSoundTimes[soundName] >= 1)
            {
                AudioManager.GetInstance(MainApp.GetInstance().Content).PlaySoundEffect(soundName);
                _lastSoundTimes[soundName] = currentTime;
            }
        }

        public override void OnCollision(Entity other)
        {
            base.OnCollision(other);
        }

        public override void ResolveCollision(Entity other)
        {
            base.ResolveCollision(other);
        }
    }
}
