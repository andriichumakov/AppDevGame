using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Lantern : Entity
    {
        private bool _isLit;
        private Texture2D _litTexture;
        private Texture2D _unlitTexture;

        public Lantern(LevelWindow level, Texture2D litTexture, Texture2D unlitTexture, Vector2 position, bool isLit = false)
            : base(level, unlitTexture, position, EntityType.Lantern)
        {
            _litTexture = litTexture;
            _unlitTexture = unlitTexture;
            _isLit = isLit;
            _hitbox = new Rectangle((int)position.X, (int)position.Y, unlitTexture.Width, unlitTexture.Height);
        }

        public bool IsLit
        {
            get => _isLit;
            set
            {
                _isLit = value;
                _texture = _isLit ? _litTexture : _unlitTexture;
            }
        }

        public override void OnCollision(Entity other)
        {
            if (other is Player && !_isLit)
            {
                IsLit = true;
                ((Level1)_level).IncrementLitLanterns();
                MainApp.Log("Lantern lit up!");
            }
        }

        public override void Update(GameTime gameTime)
        {
            // No additional update logic for the lantern
        }
    }
}