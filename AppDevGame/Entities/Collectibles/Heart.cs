using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Heart : Entity
    {
        private float _scale;

        public Heart(LevelWindow level, Texture2D texture, Vector2 position, float scale = 2.0f)
            : base(level, texture, position, EntityType.Item)
        {
            _scale = scale;
            _hitbox = new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * scale), (int)(texture.Height * scale));
            SetCollidableTypes(EntityType.Player);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.Draw(_texture, _position - offset, null, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            // No update logic needed for the heart pickup
        }

        public override void OnCollision(Entity other)
        {
            // No collision logic needed for the heart pickup
        }
    }
}