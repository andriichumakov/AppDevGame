using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Obstacle : Entity
    {
        public static bool DebugShowBounds = true; // Static flag for debugging
        private bool _isHidden;

        public Obstacle(LevelWindow level, Vector2 position, Vector2 size)
            : base(level, new Texture2D(MainApp.GetInstance().GraphicsDevice, (int) size.X, (int) size.Y), position, EntityType.HiddenObstacle, false)
        {
            _isHidden = true;
            _hitbox = new Rectangle(position.ToPoint(), size.ToPoint());
            SetCollidableTypes(EntityType.Player, EntityType.Enemy);
        }

        public Obstacle(LevelWindow level, Texture2D texture, Vector2 position)
            : base(level, texture, position, EntityType.Obstacle, false)
        {
            _isHidden = false;
            _hitbox = new Rectangle(position.ToPoint(), new Point(texture.Width, texture.Height));
            SetCollidableTypes(EntityType.Player, EntityType.Enemy);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (!_isHidden)
            {
                base.Draw(spriteBatch, offset);
            }

            if (DebugShowBounds)
            {
                DrawDebugBounds(spriteBatch, offset);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void DrawDebugBounds(SpriteBatch spriteBatch, Vector2 offset)
        {
            Texture2D rectangleTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            rectangleTexture.SetData(new[] { Color.Red });

            Vector2 drawPosition = _position - offset;
            Rectangle drawRectangle = new Rectangle(drawPosition.ToPoint(), _hitbox.Size);

            // Draw top border
            spriteBatch.Draw(rectangleTexture, new Rectangle(drawRectangle.Left, drawRectangle.Top, drawRectangle.Width, 1), Color.Red);
            // Draw bottom border
            spriteBatch.Draw(rectangleTexture, new Rectangle(drawRectangle.Left, drawRectangle.Bottom - 1, drawRectangle.Width, 1), Color.Red);
            // Draw left border
            spriteBatch.Draw(rectangleTexture, new Rectangle(drawRectangle.Left, drawRectangle.Top, 1, drawRectangle.Height), Color.Red);
            // Draw right border
            spriteBatch.Draw(rectangleTexture, new Rectangle(drawRectangle.Right - 1, drawRectangle.Top, 1, drawRectangle.Height), Color.Red);

            rectangleTexture.Dispose();
        }
    }
}
