using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Entity : Sprite
    {
        protected Rectangle _hitbox;
        protected LevelWindow _level;

        public Entity(LevelWindow level, Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            _level = level;
            _hitbox = new Rectangle(0, 0, texture.Width, texture.Height);
            _hitbox.Location = _position.ToPoint();
        }

        public Rectangle Hitbox => _hitbox;
        public Vector2 Position => _position;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _hitbox.Location = _position.ToPoint();
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Vector2 drawPosition = _position - offset;
            spriteBatch.Draw(_texture, drawPosition, Color.White);
        }

        public virtual void OnCollision(Entity other)
        {
            // Handle collision logic here
            if (!(other is Heart))
            {
                ResolveCollision(other);
            }
        }

        private void ResolveCollision(Entity other)
        {
            Rectangle intersection = Rectangle.Intersect(_hitbox, other.Hitbox);

            if (intersection.Width > intersection.Height)
            {
                player.ResolveCollision(this);
            }
        }
    }
}
