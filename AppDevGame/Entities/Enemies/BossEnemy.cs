using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AppDevGame
{
    public abstract class BossEnemy : Enemy
    {
        private string _name;
        private Random _random;
        private bool _isSpawned;

        private Sprite _bossSprite = new Sprite(MainApp.GetInstance()._imageLoader.GetResource("PlantBeast"));

        public BossEnemy(LevelWindow level, Vector2 position, int maxHealth, int damage, float speed, float scale, string name)
            : base(level, position, EntityType.Enemy, maxHealth, damage, scale)
        {
            _speed = speed;
            _name = name;
            _random = new Random();

            AddSprite(_bossSprite);
            Vector2 spriteSize = _bossSprite.GetSize();
            _hitbox = new Rectangle(0, 0, (int) spriteSize.X, (int) spriteSize.Y);

            AddCollidableType(EntityType.Player);
            AddCollidableType(EntityType.Obstacle);
        }

        public string GetName()
        {
            return _name;
        }

        public virtual void SpawnNearPortal(Vector2 portalPosition)
        {
            float distance = 50; // Distance from the portal to spawn the boss

            Vector2 randomOffset = new Vector2(
                _random.Next((int)-distance, (int)distance),
                _random.Next((int)-distance, (int)distance)
            );

            Vector2 spawnPosition = portalPosition + randomOffset;

            // Ensure the boss doesn't spawn outside the level bounds
            spawnPosition.X = Math.Clamp(spawnPosition.X, 0, _level.ActualSize.Width - _hitbox.Width);
            spawnPosition.Y = Math.Clamp(spawnPosition.Y, 0, _level.ActualSize.Height - _hitbox.Height);

            _position = spawnPosition;
            _hitbox.Location = _position.ToPoint();
            _isSpawned = true;

            MainApp.Log($"{_name} spawned at {spawnPosition}");
        }

        public override void Update(GameTime gameTime)
        {
            if (_isSpawned)
            {
                MoveTowardsPlayer(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            base.Draw(spriteBatch, offset);

            // Draw boss name above the boss
            SpriteFont font = MainApp.GetInstance()._fontLoader.GetResource("Default");
            if (font != null)
            {
                Vector2 namePosition = _position - offset + new Vector2(0, -20);
                spriteBatch.DrawString(font, _name, namePosition, Color.Red);
            }
        }

        public override void OnCollision(Entity other)
        {
            if (other is Player)
            {
                Attack(other);
            }
            base.OnCollision(other);
        }
    }
}