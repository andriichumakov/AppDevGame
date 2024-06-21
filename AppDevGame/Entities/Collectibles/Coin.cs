using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Coin : Entity
    {
        private float _scale;
        private int _frameWidth;
        private int _frameHeight;
        private int _frameCount;
        private int _currentFrame;
        private double _frameTime;
        private double _elapsedFrameTime;

        public Coin(LevelWindow level, Texture2D texture, Vector2 position, float scale = 2.0f, int frameWidth = 16, int frameHeight = 16, int frameCount = 10, double frameTime = 0.1)
            : base(level, texture, position, EntityType.Item)
        {
            _scale = scale;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _frameCount = frameCount;
            _frameTime = frameTime;
            _currentFrame = 0;
            _elapsedFrameTime = 0;

            _hitbox = new Rectangle((int)position.X, (int)position.Y, (int)(_frameWidth * scale), (int)(_frameHeight * scale));
            SetCollidableTypes(EntityType.Player);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _elapsedFrameTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (_elapsedFrameTime >= _frameTime)
            {
                _currentFrame = (_currentFrame + 1) % _frameCount;
                _elapsedFrameTime = 0;
            }
        }

        public override void OnCollision(Entity other)
        {
            if (other is Player player)
            {
                player.CollectCoin();
                _level.RemoveEntity(this);
                AudioManager.GetInstance(MainApp.GetInstance().Content).PlaySoundEffect("coin_collect");
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            int row = _currentFrame / (_texture.Width / _frameWidth);
            int column = _currentFrame % (_texture.Width / _frameWidth);

            Rectangle sourceRectangle = new Rectangle(column * _frameWidth, row * _frameHeight, _frameWidth, _frameHeight);
            Vector2 origin = new Vector2(_frameWidth / 2.0f, _frameHeight / 2.0f);

            spriteBatch.Draw(_texture, _position - offset, sourceRectangle, Color.White, 0f, origin, _scale, SpriteEffects.None, 0f);
        }
    }
}
