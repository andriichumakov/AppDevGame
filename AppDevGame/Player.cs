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
        private int _attackDamage = 2; // Damage dealt to enemies when attacking
        private int _attackRange = 120; // Range of the player's attack
        private Texture2D _healthBarTexture;

        public Player(LevelWindow level, Texture2D texture, Vector2 position, float speed = 200f, int maxHealth = 100)
            : base(level, texture, position)
        {
            _speed = speed;
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            _healthBarTexture = new Texture2D(MainApp.GetInstance().GraphicsDevice, 1, 1);
            _healthBarTexture.SetData(new[] { Color.White });
        }

        public Vector2 Position => _position;
        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                // Implement logic for player death (e.g., load main menu)
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

            // Ensure the player does not move out of the actual level bounds
            _position.X = Math.Clamp(_position.X, 0, _level.ActualSize.Width - _hitbox.Width);
            _position.Y = Math.Clamp(_position.Y, 0, _level.ActualSize.Height - _hitbox.Height);

            // Handle attack logic
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
            // Draw the player texture
            base.Draw(spriteBatch, offset);

            // Health bar parameters
            int barWidth = _hitbox.Width;
            int barHeight = 10; // Thicker health bar
            int barYOffset = 15; // Position the bar above the player

            // Calculate health percentage
            float healthPercentage = (float)_currentHealth / _maxHealth;

            // Health bar positions
            Vector2 healthBarPosition = _position - offset + new Vector2(0, -barYOffset);
            Rectangle healthBarBackground = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, barWidth, barHeight);
            Rectangle healthBarForeground = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, (int)(barWidth * healthPercentage), barHeight);

            // Draw the health bar
            spriteBatch.Draw(_healthBarTexture, healthBarBackground, Color.Red); // Background in red
            spriteBatch.Draw(_healthBarTexture, healthBarForeground, Color.Yellow); // Foreground in yellow
        }

        public override void OnCollision(Entity other)
        {
            base.OnCollision(other);
        }
    }
}
