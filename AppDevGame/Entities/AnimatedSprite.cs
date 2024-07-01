using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class AnimatedSprite : Sprite
    {
        /*
            Represents a slightly more complex, animated version of the Sprite
            An animated texture must contain all the frames in a single image, and they should be in order
            This class slices up the texture into individual frames, and plays 
            them in order with a customizable time delay between the frame change 
        */

        private int _frameCount;
        private int _frameHeight;
        private int _frameWidth;
        private int _currentFrame;
        private double _timePerFrame;
        private double _totalElapsed;

        public AnimatedSprite(Texture2D texture, int frameCount, double frameTime, char facingDirection = 'L') 
            : base(texture, facingDirection)
        {
            _frameCount = frameCount;
            _frameWidth = _texture.Width / frameCount;
            _frameHeight = _texture.Height;
            _currentFrame = 0;
            _timePerFrame = frameTime;
            _totalElapsed = 0;
        }

        public override void Update(GameTime gameTime)
        {
            _totalElapsed += gameTime.ElapsedGameTime.TotalSeconds;

            if (_totalElapsed > _timePerFrame)
            {
                _currentFrame++;
                _currentFrame = _currentFrame % _frameCount;
                _totalElapsed -= _timePerFrame;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, float scale)
        {
            int frameWidth = _texture.Width / _frameCount;
            Rectangle sourceRectangle = new Rectangle(frameWidth * _currentFrame, 0, frameWidth, _texture.Height);
            Vector2 origin = new Vector2(frameWidth / 2f, _texture.Height / 2f);

            spriteBatch.Draw(
                _texture,
                position,
                sourceRectangle,
                Color.White,
                0f,
                origin,
                scale,
                _spriteEffects,
                0f
            );
        }
    }
}