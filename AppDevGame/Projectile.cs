using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Projectile : Entity
    {
        private Vector2 _direction;
        private float _speed = 500f;

        public Projectile(LevelWindow level, Texture2D texture, Vector2 position, Vector2 direction)
            : base(level, texture, position, EntityType.Projectile)
        {
            _direction = direction;
            SetCollidableTypes(EntityType.Enemy);
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _position += _direction * _speed * deltaTime;
            _hitbox.Location = _position.ToPoint();

            // Remove the projectile if it is out of bounds
            if (_position.X < 0 || _position.X > _level.ActualSize.Width ||
                _position.Y < 0 || _position.Y > _level.ActualSize.Height)
            {
                _level.RemoveEntity(this);
            }

            base.Update(gameTime);
        }

        public override void OnCollision(Entity other)
        {
            if (other is Enemy enemy)
            {
                enemy.TakeDamage(100); // Deal 100 damage to the enemy
                _level.RemoveEntity(this); // Remove the projectile on collision
            }
            base.OnCollision(other);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.Draw(_texture, _position - offset, Color.White);
        }
    }
}
