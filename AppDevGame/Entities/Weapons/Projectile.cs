using System.Net.Sockets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Projectile : Entity
    {
        private Vector2 _direction;
        private float _speed = 800f; // Set the desired speed for the projectile

        public Projectile(LevelWindow level, Texture2D texture, Vector2 position, Vector2 direction)
            : base(level, position, EntityType.Projectile)
        {
            _direction = direction;
            _direction.Normalize(); // Normalize the direction vector to ensure it moves correctly in all directions

            // Set the hitbox to be 10 times larger
            Sprite sprite = new Sprite(texture);
            AddSprite(sprite);
            _hitbox = new Rectangle(0, 0, (int) sprite.GetSize().X, (int) sprite.GetSize().Y);

            int hitboxWidth = texture.Width * 5;
            int hitboxHeight = texture.Height * 5;
            _hitbox = new Rectangle((int)position.X, (int)position.Y, hitboxWidth, hitboxHeight);

            AddCollidableType(EntityType.Enemy);
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _position += _direction * _speed * deltaTime;
            _hitbox.Location = _position.ToPoint();

            // Check if the projectile is outside the level bounds and remove it
            if (_position.X < 0 || _position.Y < 0 || _position.X > _level.ActualSize.Width || _position.Y > _level.ActualSize.Height)
            {
                _level.RemoveEntity(this);
            }

            base.Update(gameTime);
        }

        public override void OnCollision(Entity other)
        {
            if (other is Enemy enemy)
            {
                enemy.TakeDamage(100); // Apply damage to the enemy
                _level.RemoveEntity(this); // Remove the projectile after hitting an enemy
            }

            base.OnCollision(other);
        }
    }
}
