using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class OnMapBounds : Entity
    {
        private static bool _debugMode = true;
        private static Texture2D _debugTexture;

        public OnMapBounds(LevelWindow level, Rectangle hitbox)
            : base(level, null, hitbox.Location.ToVector2(), EntityType.Obstacle)
        {
            _hitbox = hitbox;
            _movable = false;

            // Setting collidable types
            SetCollidableTypes(EntityType.Player, EntityType.Enemy, EntityType.Projectile);

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
            if (other.Type == EntityType.Projectile)
            {
                // Destroy the projectile
                _level.RemoveEntity(other);
                return;
            }

            Rectangle intersection = Rectangle.Intersect(_hitbox, other.Hitbox);
            if (intersection.IsEmpty)
                return;

            Vector2 displacement = Vector2.Zero;
            if (intersection.Width < intersection.Height)
            {
                if (_hitbox.Left < other.Hitbox.Left)
                    displacement.X = intersection.Width;
                else
                    displacement.X = -intersection.Width;
            }
            else
            {
                if (_hitbox.Top < other.Hitbox.Top)
                    displacement.Y = intersection.Height;
                else
                    displacement.Y = -intersection.Height;
            }

            other.Move(displacement);

            // Check for further collision and resolve if necessary
            if (Rectangle.Intersect(_hitbox, other.Hitbox) != Rectangle.Empty)
            {
                if (displacement.X != 0)
                    other.Move(new Vector2(displacement.X, 0));
                else
                    other.Move(new Vector2(0, displacement.Y));
            }
        }
    }
}
