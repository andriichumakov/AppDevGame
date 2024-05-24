using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Level1 : TopDownLevel
    {
        public Level1(int frameWidth, int frameHeight, int actualWidth, int actualHeight, Texture2D background = null)
            : base(frameWidth, frameHeight, actualWidth, actualHeight, background)
        {
        }

        public override void Setup()
        {
            base.Setup();
            // Initialize entities and background specific to Level1

            // Example of adding entities to the level
            Texture2D entityTexture = MainApp.GetInstance()._imageLoader.GetResource("Frog");
            Texture2D playerTexture = MainApp.GetInstance()._imageLoader.GetResource("character");

            if (entityTexture != null)
            {
                // Add entities at specified positions
               AddEntity(new MeleeAttackEnemy(this, entityTexture, new Vector2(500, 100), maxHealth: 100, damage: 1));
                AddEntity(new MeleeAttackEnemy(this, entityTexture, new Vector2(300, 300), maxHealth: 100, damage: 1));
                AddEntity(new MeleeAttackEnemy(this, entityTexture, new Vector2(700, 450), maxHealth: 100, damage: 1));
            }

            if (playerTexture != null)
            {
                // Add player at the starting position
                SetPlayer(new Player(this, playerTexture, new Vector2(50, 50)));
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Update Level1 specific logic here
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            // Draw Level1 specific elements here
        }
    }
}
