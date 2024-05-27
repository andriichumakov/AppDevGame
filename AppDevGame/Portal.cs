using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Portal : Entity
    {
        private float _scale;

        public Portal(LevelWindow level, Texture2D texture, Vector2 position, float scale = 1.5f)
            : base(level, texture, position)
        {
            _scale = scale;
            _hitbox = new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * scale), (int)(texture.Height * scale));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Update hitbox position
            _hitbox.Location = _position.ToPoint();

            // Check for collision with the player
            if (Hitbox.Intersects(_level.Player.Hitbox))
            {
                // Teleport player to main menu
                new LoadWindowCommand(WindowManager.GetInstance(), MainApp.GetInstance().MainMenu).Execute();
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.Draw(_texture, _position - offset, null, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }
    }
}