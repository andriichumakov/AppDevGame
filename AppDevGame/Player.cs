using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace AppDevGame
{
    public class Player : Entity
    {
        private float _speed;
        private int _maxHealth;
        private int _currentHealth;
        private int _coinsCollected;
        private int _attackDamage = 2;
        private int _attackRange = 120;
        private Texture2D _healthBarTexture;

        public Player(LevelWindow level, Texture2D texture, Vector2 position, float speed = 200f, int maxHealth = 100)
            : base(level, texture, position)
        {
            _speed = speed;
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            _coinsCollected = 0;
            _healthBarTexture = new Texture2D(MainApp.GetInstance().GraphicsDevice, 1, 1);
            _healthBarTexture.SetData(new[] { Color.White });
        }

        public int CoinsCollected => _coinsCollected;

        public void CollectCoin()
        {
            _coinsCollected++;
            MainApp.Log("Coin collected. Total coins: " + _coinsCollected);
        }

        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                new LoadWindowCommand(WindowManager.GetInstance(), MainApp.GetInstance().MainMenu).Execute();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Vector2 movement = Vector2.Zero;
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.W))
            {
                movement.Y -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (state.IsKeyDown(Keys.S))
            {
                movement.Y += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (state.IsKeyDown(Keys.A))
            {
                movement.X -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (state.IsKeyDown(Keys.D))
            {
                movement.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            _position += movement;
            _hitbox.Location = _position.ToPoint();

            _position.X = Math.Clamp(_position.X, 0, _level.ActualSize.Width - _hitbox.Width);
            _position.Y = Math.Clamp(_position.Y, 0, _level.ActualSize.Height - _hitbox.Height);

            if (state.IsKeyDown(Keys.Space))
            {
                AttackEnemies();
            }
        }

        private void AttackEnemies()
        {
            var entitiesInRange = _level.GetEntitiesInRange(_position, _attackRange);

            foreach (var entity in entitiesInRange)
            {
                if (entity is Enemy enemy)
                {
                    enemy.TakeDamage(_attackDamage);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            base.Draw(spriteBatch, offset);

            int barWidth = _hitbox.Width;
            int barHeight = 10;
            int barYOffset = 15;

            float healthPercentage = (float)_currentHealth / _maxHealth;

            Vector2 healthBarPosition = _position - offset + new Vector2(0, -barYOffset);
            Rectangle healthBarBackground = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, barWidth, barHeight);
            Rectangle healthBarForeground = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, (int)(barWidth * healthPercentage), barHeight);

            spriteBatch.Draw(_healthBarTexture, healthBarBackground, Color.Red);
            spriteBatch.Draw(_healthBarTexture, healthBarForeground, Color.Yellow);
        }

        public void ResolveCollision(Entity other)
        {
            Rectangle intersection = Rectangle.Intersect(_hitbox, other.Hitbox);

            if (intersection.Width > intersection.Height)
            {
                // Vertical collision
                if (_hitbox.Top < other.Hitbox.Top)
                {
                    _position.Y -= intersection.Height;
                }
                else
                {
                    _position.Y += intersection.Height;
                }
            }
            else
            {
                // Horizontal collision
                if (_hitbox.Left < other.Hitbox.Left)
                {
                    _position.X -= intersection.Width;
                }
                else
                {
                    _position.X += intersection.Width;
                }
            }

            // Update the hitbox location after resolving collision
            _hitbox.Location = _position.ToPoint();
        }
    }
}
