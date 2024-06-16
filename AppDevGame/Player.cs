using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppDevGame
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

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
        private float _heartScale = 2.0f; // Scale factor for the hearts
        private float _playerScale = 0.8f; // Scale factor for the player

        private Direction _lastDirection; // Last movement direction
        private string _currentLevel;

        public Player(LevelWindow level, Texture2D texture, Vector2 position, float speed = 200f, int maxHealth = 100)
            : base(level, texture, position, EntityType.Player)
        {
            _speed = speed;
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;

            _healthFullTexture = MainApp.GetInstance()._imageLoader.GetResource("Health_full");
            _healthEmptyTexture = MainApp.GetInstance()._imageLoader.GetResource("Health_empty");
            SetCollidableTypes(EntityType.Item, EntityType.Obstacle, EntityType.Enemy, EntityType.Lantern);

            // Adjust the hitbox dimensions based on the player's scale
            int hitboxWidth = (int)(texture.Width * _playerScale);
            int hitboxHeight = (int)(texture.Height * _playerScale);
            _hitbox = new Rectangle((int)position.X, (int)position.Y, hitboxWidth, hitboxHeight);

            _currentLevel = "Level1"; // Example current level
        }

        public int CoinsCollected => _coinsCollected;
        public string CurrentLevel => _currentLevel;

        public void SetCurrentLevel(string level)
        {
            _currentLevel = level;
        }

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
                    _lastDirection = Direction.Up;
                }
                if (state.IsKeyDown(Keys.S))
                {
                    movement.Y += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _lastDirection = Direction.Down;
                }
                if (state.IsKeyDown(Keys.A))
                {
                    movement.X -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _lastDirection = Direction.Left;
                }
                if (state.IsKeyDown(Keys.D))
                {
                    movement.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _lastDirection = Direction.Right;
                }

                _position += movement;
                _hitbox.Location = new Point((int)_position.X, (int)_position.Y);

                // Ensure the player does not move out of the actual level bounds
                _position.X = Math.Clamp(_position.X, 0, _level.ActualSize.Width - _hitbox.Width);
                _position.Y = Math.Clamp(_position.Y, 0, _level.ActualSize.Height - _hitbox.Height);

                // Handle attack logic
                if (state.IsKeyDown(Keys.Space))
                {
                    AttackEnemies();
                }

                // Check for collision with hearts and lanterns
                var lanterns = _level.GetEntitiesInRange(_position, _hitbox.Width).OfType<Lantern>().ToList();
                foreach (var lantern in lanterns)
                {
                    if (!lantern.IsLit)
                    {
                        lantern.IsLit = true;
                        ((Level1)_level).IncrementLitLanterns();
                        MainApp.Log("Lantern lit up");
                    }
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
                if (entity is Enemy enemy && IsInAttackDirection(enemy))
                {
                    enemy.TakeDamage(_attackDamage);
                }
            }
        }

        private bool IsInAttackDirection(Entity entity)
        {
            switch (_lastDirection)
            {
                case Direction.Up:
                    return entity.Position.Y < _position.Y;
                case Direction.Down:
                    return entity.Position.Y > _position.Y;
                case Direction.Left:
                    return entity.Position.X < _position.X;
                case Direction.Right:
                    return entity.Position.X > _position.X;
                default:
                    return false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            try
            {
                // Draw the player texture
                spriteBatch.Draw(_texture, _position - offset, null, Color.White, 0f, Vector2.Zero, _playerScale, SpriteEffects.None, 0f);

                // Draw health hearts in the top right corner
                int heartWidth = (int)(_healthFullTexture.Width * _heartScale);
                int heartHeight = (int)(_healthFullTexture.Height * _heartScale);
                int spacing = 10;
                int totalHearts = 3;
                int heartsToDraw = (int)Math.Ceiling((_currentHealth / (float)_maxHealth) * totalHearts);

                for (int i = 0; i < totalHearts; i++)
                {
                    Texture2D texture = i < heartsToDraw ? _healthFullTexture : _healthEmptyTexture;
                    Vector2 position = new Vector2(
                        MainApp.GetInstance().GetGraphicsManager().PreferredBackBufferWidth - (heartWidth + spacing) * (totalHearts - i),
                        spacing);

                    // Draw heart
                    spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, _heartScale, SpriteEffects.None, 0f);
                }
            }
            catch (Exception ex)
            {
                MainApp.Log($"Error during Player.Draw: {ex.Message}");
                throw;
            }
        }

        public override void OnCollision(Entity other) 
        {
            base.OnCollision(other);
        }

        public override void ResolveCollision(Entity other)
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
                base.ResolveCollision(other);
            }
            catch (Exception ex)
            {
                MainApp.Log($"Error during Player.OnCollision: {ex.Message}");
                throw;
            }
        }

        public List<EntityState> GetEntityStates()
        {
            // Implement method to get current entity states
            return new List<EntityState>();
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        public void SetHealth(int health)
        {
            _currentHealth = health;
        }

        public void SetCoins(int coins)
        {
            _coinsCollected = coins;
        }
    }
}