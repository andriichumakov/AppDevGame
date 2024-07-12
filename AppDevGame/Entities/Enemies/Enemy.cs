using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AppDevGame
{
    public abstract class Enemy : LiveEntity
    {
        protected Texture2D _healthBarTexture;
        private Dictionary<string, double> _lastSoundTimes = new Dictionary<string, double>();
        protected float _scale;

        protected int visionRange;
        protected int attackRange;

        protected LiveEntity target = null;

        public Enemy(LevelWindow level, Vector2 position, EntityType type, int maxHealth, int damage, float scale = 1.5f)
            : base(level, position, type, maxHealth, damage)
        {
            _scale = scale;
            _healthBarTexture = new Texture2D(MainApp.GetInstance().GraphicsDevice, 1, 1);
            _healthBarTexture.SetData(new[] { Color.White });

            // make sure that the enemies cannot go through the walls and through the player
            AddCollidableType(EntityType.Player);
            AddCollidableType(EntityType.Obstacle);

            // uncomment this to allow the enemies to collide with each other
            //SetCollidableTypes(EntityType.Enemy);

            // Initialize the hitbox to match the scaled size of the enemy image
            GetNewHitbox();
        }

        public static new Rectangle GetNewHitbox()
        {
            return new Rectangle(0, 0, 0, 0);
        }

        public override void AddSprite(Sprite sprite)
        {
            sprite.SetScale(_scale);
            base.AddSprite(sprite);
        }

        public override void TakeDamage(int damage)
        {
            PlaySoundWithDelay("enemy_damage");
            base.TakeDamage(damage);
        }

        public void DropCoin()
        {
            Texture2D coinTexture = MainApp.GetInstance()._imageLoader.GetResource("coin1");
            if (coinTexture != null)
            {
                _level.AddEntity(new Coin(_level, _position));
                AudioManager.GetInstance(MainApp.GetInstance().Content).PlaySoundEffect("coin_collect");
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (IsDead())
            {
                DropCoin();
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Vector2 drawPosition = _position - offset;
            DrawHealthBar(spriteBatch, drawPosition);
            //spriteBatch.Draw(_texture, drawPosition, null, Color.White, 0f, new Vector2(_texture.Width / 2, _texture.Height / 2), _scale, SpriteEffects.None, 0f);
            base.Draw(spriteBatch, offset);
        }

        protected virtual void DrawHealthBar(SpriteBatch spriteBatch, Vector2 drawPosition)
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

        public virtual void Attack(Entity target)
        {
            if (target is LiveEntity attackTarget && _hitbox.Intersects(target.GetHitbox()))
            {
                attackTarget.TakeDamage(_damage);
            }
        }

        protected void PlaySoundWithDelay(string soundName)
        {
            double currentTime = MainApp.GetInstance().TotalGameTime.TotalSeconds;
            if (!_lastSoundTimes.ContainsKey(soundName) || currentTime - _lastSoundTimes[soundName] >= 1)
            {
                AudioManager.GetInstance(MainApp.GetInstance().Content).PlaySoundEffect(soundName);
                _lastSoundTimes[soundName] = currentTime;
            }
        }

        public virtual void MoveTowardsPlayer(GameTime gameTime)
        {
            Vector2 direction = _level.Player.GetPosition() - _position;
            direction.Normalize();
            _position += direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void OnCollision(Entity other)
        {
            base.OnCollision(other);
        }

        public override void ResolveCollision(Entity other)
        {
            if (other is Player player)
            {
                Attack(player);
            }
            base.ResolveCollision(other);
        }
    }
}
