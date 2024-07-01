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
        Projectile
    }

    public class Entity
    {
        protected Rectangle _hitbox;
        protected Vector2 _position;

        protected EntityType _entityType; // used to categorize different entities in groups for collision detection purposes
        protected Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
        protected HashSet<EntityType> _collidesWith = new HashSet<EntityType>();

        protected bool _canMove; // immovable entities should not be pushed away by a collision

        public Entity()
        {
            
        }



    }

    /*
    public class Entity
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
            _hitbox = new Rectangle(0, 0, texture?.Width ?? 0, texture?.Height ?? 0);
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
            if (_texture == null || IsTextureFullyTransparent(_texture))
            {
                return;
            }

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
            if (!_movable)
            {
                return;
            }
            if (intersection.Width > intersection.Height)
            {
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
                if (_hitbox.Left < other.Hitbox.Left)
                {
                    _position.X -= intersection.Width;
                }
                else
                {
                    _position.X += intersection.Width;
                }
            }
        }

        private bool IsTextureFullyTransparent(Texture2D texture)
        {
            Color[] textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);

            foreach (Color color in textureData)
            {
                if (color.A != 0)
                {
                    return false;
                }
            }

            return true;
        }

        public void SetCollidableTypes(params EntityType[] collidableTypes)
        {
            _collidableTypes = new HashSet<EntityType>(collidableTypes);
        }

        public void Move(Vector2 delta)
        {
            _position += delta;
            _hitbox.Location = _position.ToPoint();
        }
    }
    */
}
