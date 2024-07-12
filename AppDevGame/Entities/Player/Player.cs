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
        Right,
        UpRight,
        UpLeft,
        DownRight,
        DownLeft
    }

    public class Player : LiveEntity
    {
        private Texture2D _idleTexture = MainApp.GetInstance()._imageLoader.GetResource("Gunner_Blue_Idle");
        private Texture2D _runTexture = MainApp.GetInstance()._imageLoader.GetResource("Gunner_Blue_Run");

        private Texture2D _healthFullTexture = MainApp.GetInstance()._imageLoader.GetResource("Health_full");
        private Texture2D _healthEmptyTexture = MainApp.GetInstance()._imageLoader.GetResource("Health_empty");
        private Texture2D _projectileTexture = MainApp.GetInstance()._imageLoader.GetResource("Projectile");

        protected int _coinCount = 0;

        private Dictionary<string, double> _lastSoundTimes = new Dictionary<string, double>();

        private float _heartScale = 2.0f; // Scale factor for the heart
        private float _playerScale = 2.0f; // Scale factor for the player

        private Direction _lastDirection;
        private double _lastShotTime;
        private AnimatedSprite _runningAnimation;
        private AnimatedSprite _idleAnimation;
        private bool _isRunning;

        public Player(LevelWindow level, Texture2D runningTexture, Texture2D idleTexture, Vector2 position, float speed = 200f, int maxHealth = 3)
            : base(level, position, EntityType.Player, maxHealth, 2, speed)
        {
            _speed = speed;
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            
            AddCollidableType(EntityType.Item);
            AddCollidableType(EntityType.Obstacle);
            AddCollidableType(EntityType.Enemy);
            AddCollidableType(EntityType.Lantern);

            int hitboxWidth = (int)(runningTexture.Width * _playerScale / 6);
            int hitboxHeight = (int)(runningTexture.Height * _playerScale);
            _hitbox = new Rectangle((int)position.X, (int)position.Y, hitboxWidth, hitboxHeight);

            _lastShotTime = -1; // Initialize to -1 so the player can shoot immediately at the start

            _runningAnimation = new AnimatedSprite(_runTexture, 6, 0.2);
            _idleAnimation = new AnimatedSprite(_idleTexture, 5, 0.25);
            AddSprite(_runningAnimation);
            AddSprite(_idleAnimation);
            _isRunning = false;
        }

        public void CollectCoin()
        {
            _coinCount++;
            MainApp.Log("Coin collected. Total coins: " + _coinCount);
            AudioManager.GetInstance(MainApp.GetInstance().Content).PlaySoundEffect("coin_collect");
        }

        public override void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            PlaySoundWithDelay("player_damage");

            if (_currentHealth <= 0)
            {
                _currentHealth = 0; // Ensure health doesn't go below zero
                PlaySoundWithDelay("player_die");
                MainApp.GetInstance().ShowGameOverScreen(_level); // Pass the current level
            }
        }

        public override void Heal(int amount)
        {
            _currentHealth = Math.Min(_currentHealth + amount, _maxHealth);
            AudioManager.GetInstance(MainApp.GetInstance().Content).PlaySoundEffect("heart_collect");
        }

        public override void Update(GameTime gameTime)
        {
            if (!MainApp.GetInstance().IsPaused)
            {
                base.Update(gameTime);

                Vector2 movement = Vector2.Zero;
                KeyboardState state = Keyboard.GetState();
                _isRunning = false;
                _currentSprite = 0;

                if (state.IsKeyDown(Keys.W))
                {
                    movement.Y -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _lastDirection = Direction.Up;
                    _isRunning = true;
                    _currentSprite = 1;
                }
                if (state.IsKeyDown(Keys.S))
                {
                    movement.Y += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _lastDirection = Direction.Down;
                    _isRunning = true;
                    _currentSprite = 1;
                }
                if (state.IsKeyDown(Keys.A))
                {
                    movement.X -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _lastDirection = Direction.Left;
                    _isRunning = true;
                    _currentSprite = 1;
                }
                if (state.IsKeyDown(Keys.D))
                {
                    movement.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _lastDirection = Direction.Right;
                    _isRunning = true;
                    _currentSprite = 1;
                }
                
                // Check for diagonal movement
                if (state.IsKeyDown(Keys.W) && state.IsKeyDown(Keys.D))
                {
                    _lastDirection = Direction.UpRight;
                }
                if (state.IsKeyDown(Keys.W) && state.IsKeyDown(Keys.A))
                {
                    _lastDirection = Direction.UpLeft;
                }
                if (state.IsKeyDown(Keys.S) && state.IsKeyDown(Keys.D))
                {
                    _lastDirection = Direction.DownRight;
                }
                if (state.IsKeyDown(Keys.S) && state.IsKeyDown(Keys.A))
                {
                    _lastDirection = Direction.DownLeft;
                }

                _position += movement;
                _hitbox.Location = new Point((int)_position.X, (int)_position.Y);

                _position.X = Math.Clamp(_position.X, 0, _level.ActualSize.Width - _hitbox.Width);
                _position.Y = Math.Clamp(_position.Y, 0, _level.ActualSize.Height - _hitbox.Height);

                if (state.IsKeyDown(Keys.Space))
                {
                    AttackEnemies();
                }

                // Handle shooting projectiles
                if (state.IsKeyDown(Keys.Q) && gameTime.TotalGameTime.TotalSeconds - _lastShotTime >= 0.75)
                {
                    ShootProjectile();
                    _lastShotTime = gameTime.TotalGameTime.TotalSeconds;
                }

                base.Update(gameTime);
            }
        }

        private void AttackEnemies()
        {
            PlaySoundWithDelay("player_attack");
            var entitiesInRange = _level.GetEntitiesInRange(_position, 120);

            foreach (var entity in entitiesInRange)
            {
                if (entity is Enemy enemy && IsInAttackDirection(enemy))
                {
                    enemy.TakeDamage(_damage);
                }
            }
        }

        private void PlaySoundWithDelay(string soundName)
        {
            double currentTime = MainApp.GetInstance().TotalGameTime.TotalSeconds;
            if (!_lastSoundTimes.ContainsKey(soundName) || currentTime - _lastSoundTimes[soundName] >= 1)
            {
                AudioManager.GetInstance(MainApp.GetInstance().Content).PlaySoundEffect(soundName);
                _lastSoundTimes[soundName] = currentTime;
            }
        }

        private void ShootProjectile()
        {
            Vector2 direction = Vector2.Zero;

            //MainApp.Log(this._hitbox.X + ", " + this._hitbox.Y);

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
                case Direction.UpRight:
                    direction = new Vector2(1, -1);
                    break;
                case Direction.UpLeft:
                    direction = new Vector2(-1, -1);
                    break;
                case Direction.DownRight:
                    direction = new Vector2(1, 1);
                    break;
                case Direction.DownLeft:
                    direction = new Vector2(-1, 1);
                    break;
            }

            direction.Normalize(); // Normalize the direction to ensure consistent speed

            var projectile = new Projectile(_level, _projectileTexture, _position, direction);
            _level.AddEntity(projectile);
        }

        private bool IsInAttackDirection(Entity entity)
        {
            switch (_lastDirection)
            {
                case Direction.Up:
                    return entity.GetPosition().Y < _position.Y;
                case Direction.Down:
                    return entity.GetPosition().Y > _position.Y;
                case Direction.Left:
                    return entity.GetPosition().X < _position.X;
                case Direction.Right:
                    return entity.GetPosition().X > _position.X;
                case Direction.UpRight:
                    return entity.GetPosition().X > _position.X && entity.GetPosition().Y < _position.Y;
                case Direction.UpLeft:
                    return entity.GetPosition().X < _position.X && entity.GetPosition().Y < _position.Y;
                case Direction.DownRight:
                    return entity.GetPosition().X > _position.X && entity.GetPosition().Y > _position.Y;
                case Direction.DownLeft:
                    return entity.GetPosition().X < _position.X && entity.GetPosition().Y > _position.Y;
                default:
                    return false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            try
            {
                base.Draw(spriteBatch, offset);
                MainApp.Log("No crash yet");
                //spriteBatch.Draw(_texture, _position - offset, null, Color.White, 0f, Vector2.Zero, _playerScale, SpriteEffects.None, 0f);

                int heartWidth = (int)(_healthFullTexture.Width * _heartScale);
                int heartHeight = (int)(_healthFullTexture.Height * _heartScale);
                int spacing = 10;
                int totalHearts = 3;
                int heartsToDraw = (int)Math.Ceiling((_currentHealth / (float)_maxHealth) * totalHearts);

                for (int i = 0; i < totalHearts; i++)
                {
                    MainApp.Log("No crash yet 2");
                    Texture2D texture = i < heartsToDraw ? _healthFullTexture : _healthEmptyTexture;
                    Vector2 position = new Vector2(
                        MainApp.GetInstance().GetGraphicsManager().PreferredBackBufferWidth - (heartWidth + spacing) * (totalHearts - i),
                        spacing);

                    spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, _heartScale, SpriteEffects.None, 0f);
                }
                MainApp.Log("No crash yet 3");
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
            catch (Exception ex) {
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
            _coinCount = coins;
        }

        public int GetCoins()
        {
            return _coinCount;
        }
    }
}
