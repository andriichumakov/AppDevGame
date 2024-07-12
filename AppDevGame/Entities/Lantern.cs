using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Lantern : Entity
    {
        private bool _isLit;
        private Texture2D _litTexture = MainApp.GetInstance()._imageLoader.GetResource("LanternLit");
        private Texture2D _unlitTexture = MainApp.GetInstance()._imageLoader.GetResource("LanternUnlit");

        public Lantern(LevelWindow level, Vector2 position, bool isLit = false)
            : base(level, position, EntityType.Lantern)
        {
            AddSprite(new Sprite(_unlitTexture));
            AddSprite(new Sprite(_litTexture));
            _isLit = isLit;
            Vector2 spriteSize = GetCurrentSprite().GetSize();
            _hitbox = new Rectangle(0, 0, (int) spriteSize.X, (int) spriteSize.Y);

            AddCollidableType(EntityType.Player);
        }

        public bool isLit()
        {
            return _isLit;
        }

        public override void OnCollision(Entity other)
        {
            if (other is Player && !_isLit)
            {
                _isLit = true;
                ((Level1)_level).IncrementLitLanterns();
                SetCurrentSprite(1);
                MainApp.Log("Lantern lit up!");
            }
        }

        public override void Update(GameTime gameTime)
        {
            // No additional update logic for the lantern
        }
    }
}