using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace AppDevGame
{
    public class Level1 : TopDownLevel
    {
        private Portal _portal;
        private int _maxHearts = 3;
        private int _currentHeartCount = 0;

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
            Texture2D activePortalTexture = MainApp.GetInstance()._imageLoader.GetResource("PortalActive");
            Texture2D inactivePortalTexture = MainApp.GetInstance()._imageLoader.GetResource("PortalInactive");
            Texture2D heartTexture = MainApp.GetInstance()._imageLoader.GetResource("Heart");

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

            if (activePortalTexture != null && inactivePortalTexture != null)
            {
                // Add inactive portal at the desired position (update the coordinates accordingly)
                _portal = new Portal(this, activePortalTexture, inactivePortalTexture, new Vector2(550, 60), scale: 2.0f, isActive: false);
                AddEntity(_portal);
            }

            if (heartTexture != null)
            {
                // Add hearts at specified positions
                AddHeart(heartTexture, new Vector2(400, 200));
                AddHeart(heartTexture, new Vector2(600, 300));
                AddHeart(heartTexture, new Vector2(800, 400));
            }
        }

        private void AddHeart(Texture2D heartTexture, Vector2 position)
        {
            if (_currentHeartCount < _maxHearts)
            {
                AddEntity(new Heart(this, heartTexture, position));
                _currentHeartCount++;
            }
        }

        public void DecrementHeartCount()
        {
            if (_currentHeartCount > 0)
            {
                _currentHeartCount--;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Check if all enemies are defeated
            if (_entities.OfType<Enemy>().All(e => e.IsDead()))
            {
                // Activate the portal
                _portal.IsActive = true;
            }

            // Update Level1 specific logic here
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            // Draw Level1 specific elements here
        }
    }
}
