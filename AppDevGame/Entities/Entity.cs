using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AppDevGame
{
    public enum EntityType
    {
        Obstacle,
        Wall,
        Player,
        Enemy,
        Lantern,
        Interactable,
        Projectile,
    }


    public class Entity
    {
        protected LevelWindow _level;
        protected Rectangle _hitbox;
        protected Vector2 _position;

        protected EntityType _entityType; // used to categorize different entities in groups for collision detection purposes
        protected List<Sprite> _sprites = new List<Sprite>(); // sprite array allows our entities to use multiple sprites at once
        protected int _currentSprite = -1; // -1 means that there is no sprite to display
        
        protected HashSet<EntityType> _collidesWith = new HashSet<EntityType>();

        protected bool _canMove; // immovable entities should not be pushed away by a collision
        protected int _moveSpeed;

        public static Rectangle GetNewHitbox()
        {
            return new Rectangle(0, 0, 100, 100);
        }

        public Entity(LevelWindow level, Vector2 pos, EntityType type, bool canMove=false, int moveSpeed = 0)
        {
            _level = level;
            _position = pos;
            _entityType = type;
            _canMove = canMove;
            _moveSpeed = moveSpeed;
            _hitbox = GetNewHitbox();
        }

        public void AddSprite(Sprite sprite)
        {
            _sprites.Add(sprite);
            if (_currentSprite == -1)
            {
                _currentSprite = 0; // we are now using a sprite
            }
        }

        public void SetCurrentSprite(int spriteIndex)
        {
            if (spriteIndex < _sprites.Count && spriteIndex >= -1)
            {
                _currentSprite = spriteIndex;
            }
        }

        public Sprite GetCurrentSprite()
        {
            if (_currentSprite == -1)
            {
                return null;
            }
            return _sprites[_currentSprite];
        }

        public EntityType GetEntityType()
        {
            return _entityType;
        }

        public Rectangle GetHitbox()
        {
            return _hitbox;
        }

        public void AddCollidableType(EntityType type)
        {
            this._collidesWith.Add(type);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Sprite drawSprite = GetCurrentSprite();
            if (drawSprite == null)
            {
                return;
            }

            Vector2 drawPosition = _position - offset;
            drawSprite.Draw(spriteBatch, drawPosition);
        }

        public virtual void Update(GameTime gameTime)
        {
            GetCurrentSprite().Update(gameTime);
            _hitbox.Location = _position.ToPoint();
        }
        
        public void UpdateSpriteDirections(Direction newDirection)
        {
            char val;
            // TODO Ensure that the Sprite uses the common direction system
            switch (newDirection)
            {
                case Direction.Left:
                    val = 'L';
                    break;
                case Direction.Right:
                    val = 'R';
                    break;
                default:
                    return;
            }            
            foreach (Sprite sprite in _sprites)
            {
                sprite.SetDirection(val);
            }
        }

        public void SelfDestruct()
        // removes the entity from the level
        {
            _level.RemoveEntity(this);
        }

        public void MoveTo(Vector2 pos)
        {
            _position = pos;
        }
        public void MoveBy(Vector2 delta)
        {
            // physically move a sprite to a new position
            _position += delta;
            // update the all the sprites to make sure they are facing towards the correct direction
            if (delta.X < 0) 
            {
                UpdateSpriteDirections(Direction.Left);
            }
            else if (delta.X > 0)
            {
                UpdateSpriteDirections(Direction.Right);
            }
        }

        public virtual void OnCollision(Entity other)
        {
            if (_collidesWith.Contains(other.GetEntityType()))
            {
                ResolveCollision(other);
            }
        }

        public virtual void ResolveCollision(Entity other)
        // base collision resolving method, don't forget to call base.ResolveCollision() in subclasses methods
        // only takes care of preventing an entity from moving into another entity
        // any additional functionality has to be implemented via overrides
        {
            Rectangle intersection = Rectangle.Intersect(_hitbox, other.GetHitbox());
            if (!_canMove)
            {
                return;
            }
            if (intersection.Width > intersection.Height)
            {
                if (_hitbox.Top < other.GetHitbox().Top)
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
                if (_hitbox.Left < other.GetHitbox().Left)
                {
                    _position.X -= intersection.Width;
                }
                else
                {
                    _position.X += intersection.Width;
                }
            }
        }

        public class LiveEntity : Entity
        {
            //differs from the normal entity with the fact that it has health and can be killed
            protected int _maxHealth;
            protected int _currentHealth;
            protected int _damage;

            public LiveEntity(LevelWindow level, Vector2 pos, EntityType type, int maxHealth, int damage=1) : base(level, pos, type)
            {
                _maxHealth = maxHealth;
                _currentHealth = maxHealth;
                _damage = damage;
            }

            public void RestoreHealth()
            {
                _currentHealth = _maxHealth;
            }

            public void Heal(int hearts)
            {
                _currentHealth += hearts;
                if (_currentHealth > _maxHealth)
                {
                    RestoreHealth();
                }
            }

            public void TakeDamage(int damage)
            {
                _currentHealth -= damage;
            }

            public bool isDead()
            {
                return _currentHealth <= 0;
            }

            public override void Update(GameTime gameTime)
            {
                if (isDead())
                {
                    _level.RemoveEntity(this);
                }
                base.Update(gameTime);
            }
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
