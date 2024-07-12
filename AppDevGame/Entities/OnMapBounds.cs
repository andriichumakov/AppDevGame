using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    // TODO: Test new functionality
    public class OnMapBounds : Entity
    {
        private static bool _debugMode = false;
        private static Texture2D _debugTexture;

        public OnMapBounds(LevelWindow level, Rectangle hitbox)
            : base(level, hitbox.Location.ToVector2(), EntityType.Obstacle, false)
        {
            _hitbox = hitbox;
            _canMove = false; // Set movability from the base class

            // Setting collidable types
            AddCollidableType(EntityType.Player);
            AddCollidableType(EntityType.Enemy);
            AddCollidableType(EntityType.Projectile);

            // Create a debug texture if it doesn't exist
            if (_debugTexture == null)
            {
                _debugTexture = new Texture2D(MainApp.GetInstance().GraphicsDevice, 1, 1);
                _debugTexture.SetData(new[] { Color.Red });
            }
        }

        public static bool DebugMode
        {
            get { return _debugMode; }
            set { _debugMode = value; }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (_debugMode)
            {
                Vector2 drawPosition = _position - offset;
                spriteBatch.Draw(_debugTexture, new Rectangle(drawPosition.ToPoint(), _hitbox.Size), Color.White * 0.5f);
            }
        }

        public override void ResolveCollision(Entity other)
        {
            if (other.GetEntityType() == EntityType.Projectile)
            {
                // Destroy the projectile
                _level.RemoveEntity(other);
                return;
            }

            Rectangle intersection = Rectangle.Intersect(_hitbox, other.GetHitbox());
            if (intersection.IsEmpty)
                return;

            Vector2 displacement = Vector2.Zero;
            if (intersection.Width < intersection.Height)
            {
                if (_hitbox.Left < other.GetHitbox().Left)
                    displacement.X = intersection.Width;
                else
                    displacement.X = -intersection.Width;
            }
            else
            {
                if (_hitbox.Top < other.GetHitbox().Top)
                    displacement.Y = intersection.Height;
                else
                    displacement.Y = -intersection.Height;
            }

            other.MoveBy(displacement);

            // Check for further collision and resolve if necessary
            if (Rectangle.Intersect(_hitbox, other.GetHitbox()) != Rectangle.Empty)
            {
                if (displacement.X != 0)
                    other.MoveBy(new Vector2(displacement.X, 0));
                else
                    other.MoveBy(new Vector2(0, displacement.Y));
            }
        }
    }
}
