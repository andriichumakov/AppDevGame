using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public abstract class Entity
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public int Health { get; set; }
        public Animation Animation { get; set; }

        public Entity(Vector2 position, Vector2 velocity, int health, Animation animation)
        {
            Position = position;
            Velocity = velocity;
            Health = health;
            Animation = animation ?? throw new ArgumentNullException(nameof(animation));
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch, Position);
        }

         public bool CheckCollision(Entity otherEntity)
        {
            // Calculate bounding boxes
            Rectangle thisBounds = new Rectangle((int)Position.X, (int)Position.Y, Animation.CurrentFrame.Width, Animation.CurrentFrame.Height);
            Rectangle otherBounds = new Rectangle((int)otherEntity.Position.X, (int)otherEntity.Position.Y, otherEntity.Animation.CurrentFrame.Width, otherEntity.Animation.CurrentFrame.Height);

            // Check for collision
            return thisBounds.Intersects(otherBounds);
        }
    }
}
