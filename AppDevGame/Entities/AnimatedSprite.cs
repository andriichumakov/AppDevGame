using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class AnimatedSprite
    {
        private Texture2D _texture;
        private int _frameCount;
        private int _frameHeight;
        private int _frameWidth;
        private int _currentFrame;
        private double _timePerFrame;
        private double _totalElapsed;

        public AnimatedSprite(Texture2D texture, int frameCount, double frameTime)
        {
            _texture = texture;
            _frameCount = frameCount;
            _frameWidth = _texture.Width / frameCount;
            _frameHeight = _texture.Height;
            _currentFrame = 0;
            _timePerFrame = frameTime;
            _totalElapsed = 0;
        }

        public void Update(GameTime gameTime)
        {
            _totalElapsed += gameTime.ElapsedGameTime.TotalSeconds;

            if (_totalElapsed > _timePerFrame)
            {
                _currentFrame++;
                _currentFrame = _currentFrame % _frameCount;
                _totalElapsed -= _timePerFrame;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float scale, SpriteEffects spriteEffects)
        {
            int frameWidth = _texture.Width / _frameCount;
            Rectangle sourceRectangle = new Rectangle(frameWidth * _currentFrame, 0, frameWidth, _texture.Height);
            Vector2 origin = new Vector2(frameWidth / 2f, _texture.Height / 2f);

            spriteBatch.Draw(_texture, position, sourceRectangle, Color.White, 0f, origin, scale, spriteEffects, 0f);
        }
    }
}
