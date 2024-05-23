using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Entity : Sprite
    {
        protected Rectangle _hitbox;
        protected LevelWindow _level;

        public Entity(LevelWindow level, Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            _level = level;
            _hitbox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public Rectangle Hitbox => _hitbox;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _hitbox.Location = _position.ToPoint();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Vector2 drawPosition = _position - offset;
            spriteBatch.Draw(_texture, drawPosition, Color.White);
            // Draw hitbox for debugging purposes (optional)
            // spriteBatch.Draw(_texture, _hitbox, Color.Red * 0.5f);
        }

        public virtual void OnCollision(Entity other)
        {
            // Handle collision logic here
        }
    }
}
