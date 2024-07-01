using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Sprite
    {
        // stores the image and makes it accessible to use for entities
        protected Texture2D _texture;
        protected SpriteEffects _spriteEffects = SpriteEffects.None; // responsible for the horizontal flip of the image
        protected char _originalDirection;

        public Sprite(Texture2D texture, char originalDirection = 'R')
        {
            _texture = texture;
            _originalDirection = originalDirection;
        }

        public virtual void Update(GameTime gameTime)
        {
            // override in subclasses
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, float scale)
        {
            // Default origin to the top-left corner of the texture
            Vector2 origin = new Vector2(0, 0);
            // Drawing with the full texture (sourceRectangle = null), white color, no rotation, specified scale, and layer depth of 0
            spriteBatch.Draw(
                _texture,
                position,
                null, // sourceRectangle (null for full texture)
                Color.White,
                0f, // rotation
                origin,
                scale, // scale
                _spriteEffects,
                0f // layerDepth
            );
        }

        public virtual Texture2D GetTexture()
        {
            return _texture;
        }

        public void setDirection(char desiredDirection)
        {
            _spriteEffects = desiredDirection == _originalDirection ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        }
    }
}
