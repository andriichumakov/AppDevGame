using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Portal : Entity
    {
        private float _scale;
        public bool IsActive { get; set; }
        private Texture2D _inactiveTexture;
        private Texture2D _activeTexture;

        public Portal(LevelWindow level, Texture2D activeTexture, Texture2D inactiveTexture, Vector2 position, float scale = 1.5f, bool isActive = false)
            : base(level, inactiveTexture, position)
        {
            _activeTexture = activeTexture;
            _inactiveTexture = inactiveTexture;
            _scale = scale;
            IsActive = isActive;
            _hitbox = new Rectangle((int)position.X, (int)position.Y, (int)(inactiveTexture.Width * scale), (int)(inactiveTexture.Height * scale));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsActive)
            {
                // Update hitbox position
                _hitbox.Location = _position.ToPoint();
                _texture = _activeTexture; // Set the active texture

                // Check for collision with the player
                if (Hitbox.Intersects(_level.Player.Hitbox))
                {
                    // Teleport player to main menu
                    new LoadWindowCommand(WindowManager.GetInstance(), MainApp.GetInstance().MainMenu).Execute();
                }
            }
            else
            {
                _texture = _inactiveTexture; // Set the inactive texture
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.Draw(_texture, _position - offset, null, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }
    }
}