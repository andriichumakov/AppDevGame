using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Sprite
    {
        protected Texture2D _texture;
        protected Vector2 _position;

        public Sprite(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }

        public virtual void Update(GameTime gameTime)
        {
            // Update logic for sprites if needed
        }
    }
}
