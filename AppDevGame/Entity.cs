using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AppDevGame
{
    public enum EntityType
    {
        Obstacle,
        Player,
        Enemy,
        HiddenObstacle,
        Item,
        Lantern,
    }

    public class Entity : Sprite
    {
        protected Rectangle _hitbox;
        protected LevelWindow _level;

        protected bool _movable;
        protected EntityType _entityType;
        protected HashSet<EntityType> _collidableTypes;

        public Entity(LevelWindow level, Texture2D texture, Vector2 position, EntityType entityType, bool movable = false)
            : base(texture, position)
        {
            _level = level;
            _hitbox = new Rectangle(0, 0, texture.Width, texture.Height);
            _hitbox.Location = _position.ToPoint();
            _movable = movable;
            _entityType = entityType;
            _collidableTypes = new HashSet<EntityType>();
        }

        public Rectangle Hitbox => _hitbox;
        public Vector2 Position => _position;
        public EntityType Type => _entityType;

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
            if (_collidableTypes.Contains(other.Type))
            {
                ResolveCollision(other);
            }
        }

        public virtual void ResolveCollision(Entity other)
        {
            Rectangle intersection = Rectangle.Intersect(_hitbox, other.Hitbox);
            if (!_movable) {
                return;
            }
            if (intersection.Width > intersection.Height)
            {
                // Vertical collision
                if (_hitbox.Top < other.Hitbox.Top)
                {
                    _position.Y -= intersection.Height;
                }
                else
                {
                    _position.Y += intersection.Height;
                }
            }
            else
            {
                // Horizontal collision
                if (_hitbox.Left < other.Hitbox.Left)
                {
                    _position.X -= intersection.Width;
                }
                else
                {
                    _position.X += intersection.Width;
                }
            }

            // Update the hitbox location after resolving collision
            _hitbox.Location = _position.ToPoint();
        }

        public void MoveTo(Vector2 newPosition)
        {
            _position = newPosition;
            _hitbox.Location = _position.ToPoint();
        }

        public void SetCollidableTypes(params EntityType[] types)
        {
            _collidableTypes = new HashSet<EntityType>(types);
        }
    }
}
