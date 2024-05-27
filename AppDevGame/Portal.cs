using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Portal : Entity
    {
        private bool _isActive;
        private float _scale; // Add a scale factor

        public Portal(LevelWindow level, Texture2D texture, Vector2 position, bool isActive, float scale = 1.0f) 
            : base(level, texture, position)
        {
            _isActive = isActive;
            _scale = scale; // Initialize the scale
        }

        public bool IsActive => _isActive;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_isActive && Hitbox.Intersects(_level.Player.Hitbox))
            {
                // Send the player back to the main menu
                new LoadWindowCommand(WindowManager.GetInstance(), MainApp.GetInstance().MainMenu).Execute();
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Vector2 drawPosition = _position - offset;
            spriteBatch.Draw(_texture, drawPosition, null, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f); // Apply scaling here
        }
    }
}