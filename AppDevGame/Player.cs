using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace AppDevGame
{
    public class Player : Entity
    {
        private float _speed;
        private int _maxHealth;
        private int _currentHealth;
        private int _coinsCollected = 0;
        private int _attackDamage = 2; // Damage dealt to enemies when attacking
        private int _attackRange = 120; // Range of the player's attack
        private Texture2D _healthFullTexture;
        private Texture2D _healthEmptyTexture;

        public Player(LevelWindow level, Texture2D texture, Vector2 position, float speed = 200f, int maxHealth = 100)
            : base(level, texture, position, true)
        {
            _speed = speed;
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;

            _healthFullTexture = MainApp.GetInstance()._imageLoader.GetResource("Health_full");
            _healthEmptyTexture = MainApp.GetInstance()._imageLoader.GetResource("Health_empty");
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

        public void Heal(int amount)
        {
            _currentHealth = Math.Min(_currentHealth + amount, _maxHealth);
        }

        public override void Update(GameTime gameTime)
        {
            try
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

                // Check for collision with hearts
                var hearts = _level.GetEntitiesInRange(_position, _hitbox.Width).OfType<Heart>().ToList();
                foreach (var heart in hearts)
                {
                    Heal((int)(_maxHealth * 0.33));
                    _level.RemoveEntity(heart);
                    ((Level1)_level).DecrementHeartCount(); // Use the method to decrement heart count
                    MainApp.Log("Heart collected and removed");
                }
            }
            catch (Exception ex)
            {
                MainApp.Log($"Error during Player.Update: {ex.Message}");
                throw;
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
            try
            {
                // Draw the player texture
                base.Draw(spriteBatch, offset);

                // Draw health hearts in the top right corner
                int heartWidth = _healthFullTexture.Width;
                int heartHeight = _healthFullTexture.Height;
                int spacing = 5;
                int totalHearts = 3;
                int heartsToDraw = (int)Math.Ceiling((_currentHealth / (float)_maxHealth) * totalHearts);

                for (int i = 0; i < totalHearts; i++)
                {
                    Texture2D texture = i < heartsToDraw ? _healthFullTexture : _healthEmptyTexture;
                    Vector2 position = new Vector2(
                        MainApp.GetInstance().GetGraphicsManager().PreferredBackBufferWidth - (heartWidth + spacing) * (totalHearts - i),
                        spacing);

                    spriteBatch.Draw(texture, position, Color.White);
                }
            }
            catch (Exception ex)
            {
                MainApp.Log($"Error during Player.Draw: {ex.Message}");
                throw;
            }
        }

        public void ResolveCollision(Entity other)
        {
            try
            {
                // Handle collision with hearts separately
                if (other is Heart)
                {
                    Heal((int)(_maxHealth * 0.33));
                    _level.RemoveEntity(other);
                    ((Level1)_level).DecrementHeartCount();
                    MainApp.Log("Heart collected and removed during collision");
                    return;
                }
                base.OnCollision(other);
            }
            catch (Exception ex)
            {
                MainApp.Log($"Error during Player.OnCollision: {ex.Message}");
                throw;
            }
        }
    }
}
