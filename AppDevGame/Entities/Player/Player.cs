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
        private int _attackDamage = 2;
        private int _attackRange = 120;
        private Texture2D _healthFullTexture;
        private Texture2D _healthEmptyTexture;

        private float _heartScale = 2.0f; // Scale factor for the heart
        private float _playerScale = 2.0f; // Scale factor for the player
        private Texture2D _backgroundTexture; // Background texture

        private Direction _lastDirection;
        private string _currentLevel;

        // Cooldown for shooting projectiles
        private TimeSpan _shootCooldown = TimeSpan.FromSeconds(0.75);
        private TimeSpan _lastShootTime;

        public Player(LevelWindow level, Texture2D texture, Vector2 position, Texture2D backgroundTexture, float speed = 200f, int maxHealth = 100)
         : base(level, texture, position, EntityType.Player)
        {
            _speed = speed;
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            _backgroundTexture = backgroundTexture;

            _healthFullTexture = MainApp.GetInstance()._imageLoader.GetResource("Health_full");
            _healthEmptyTexture = MainApp.GetInstance()._imageLoader.GetResource("Health_empty");
            SetCollidableTypes(EntityType.Item, EntityType.Obstacle, EntityType.Enemy, EntityType.Lantern);

            int hitboxWidth = (int)(texture.Width * _playerScale);
            int hitboxHeight = (int)(texture.Height * _playerScale);
            _hitbox = new Rectangle((int)position.X, (int)position.Y, hitboxWidth, hitboxHeight);

            _currentLevel = "Level1";
            _lastShootTime = TimeSpan.Zero;
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
            if (!MainApp.GetInstance().IsPaused)
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

                _position.X = Math.Clamp(_position.X, 0, _level.ActualSize.Width - _hitbox.Width);
                _position.Y = Math.Clamp(_position.Y, 0, _level.ActualSize.Height - _hitbox.Height);

                // Handle shooting
                if (state.IsKeyDown(Keys.Q) && gameTime.TotalGameTime - _lastShootTime > _shootCooldown)
                {
                    ShootProjectile();
                    _lastShootTime = gameTime.TotalGameTime;
                }

                if (state.IsKeyDown(Keys.Space))
                {
                    AttackEnemies();
                }

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
        }

        private void ShootProjectile()
        {
            var direction = Vector2.Zero;
            switch (_lastDirection)
            {
                case Direction.Up:
                    direction = new Vector2(0, -1);
                    break;
                case Direction.Down:
                    direction = new Vector2(0, 1);
                    break;
                case Direction.Left:
                    direction = new Vector2(-1, 0);
                    break;
                case Direction.Right:
                    direction = new Vector2(1, 0);
                    break;
            }

            if (direction != Vector2.Zero)
            {
                var projectile = new Projectile(_level, MainApp.GetInstance()._imageLoader.GetResource("Projectile"), _position, direction);
                _level.AddEntity(projectile);
                MainApp.Log("Projectile shot.");
            }
        }

        private void AttackEnemies()
        {
            MainApp.GetInstance().PlayAttackSound();
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
                spriteBatch.Draw(_texture, _position - offset, null, Color.White, 0f, Vector2.Zero, _playerScale, SpriteEffects.None, 0f);

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

                    spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, _heartScale, SpriteEffects.None, 0f);
                }
            }
            catch (Exception ex)
            {
                MainApp.Log($"Error during Player.Draw: {ex.Message}");
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
                MainApp.Log($"Error during Player.ResolveCollision: {ex.Message}");
            }
        }

        public List<EntityState> GetEntityStates()
        {
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
