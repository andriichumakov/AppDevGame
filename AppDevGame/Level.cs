using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AppDevGame
{
    public class Level
    {
        private Vector2 maxSize; // Maximum level size on the map, used to decide where the map ends
        private Vector2 cameraPos; // Position of the camera to control the visible portion of the level

        private Texture2D background; // Background texture for the level
        private List<GameObj> gameObjects; // List of game objects in the level

        // Properties for accessing private fields
        public Vector2 CameraPosition => cameraPos;

        // Constructor to initialize the level
        public Level(Vector2 maxSize, Texture2D background)
        {
            this.maxSize = maxSize;
            this.background = background;
            gameObjects = new List<GameObj>();
            cameraPos = Vector2.Zero; // Start with camera at the top-left corner of the level
        }

        // Method to update the level
        public virtual void Update(GameTime gameTime)
        {
            // Update game objects, camera position, etc.
        }

        // Method to draw the level
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // Draw background
            spriteBatch.Draw(background, -cameraPos, Color.White);

            // Draw game objects
            foreach (GameObj obj in gameObjects)
            {
                obj.Draw(spriteBatch);
            }
        }

        // Method to move the camera
        public void MoveCamera(Vector2 moveVector)
        {
            cameraPos.X += moveVector.X;

            // Ensure camera stays within the bounds of the level
            ClampCameraToBounds();
        }

        // Method to clamp the camera position to ensure it stays within the bounds of the level
        // Method to clamp the camera position to ensure it stays within the bounds of the level
        private void ClampCameraToBounds()
        {
            cameraPos.X = MathHelper.Clamp(cameraPos.X, 0, maxSize.X - Game1.ScreenWidth);
        }

    }
}
