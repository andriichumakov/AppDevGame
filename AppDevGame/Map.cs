using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class BaseMap
    {
        protected Texture2D backgroundTexture;

        public BaseMap(Texture2D backgroundTexture)
        {
            this.backgroundTexture = backgroundTexture;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // Draw the background
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
        }
    }

    public class MapSideScrolling : BaseMap
    {
        private Texture2D platformTexture;

        public MapSideScrolling(Texture2D backgroundTexture, Texture2D platformTexture) : base(backgroundTexture)
        {
            this.platformTexture = platformTexture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // Draw platforms
            // Example: Draw a platform at position (100, 300)
            spriteBatch.Draw(platformTexture, new Vector2(100, 300), Color.White);
            // Draw other platforms as needed
        }
    }

    public class Map2D : BaseMap
    {
        private Texture2D[] layers; // Array of textures for parallax scrolling
        private Vector2[] layerPositions; // Positions of the layers for parallax scrolling
        private const float ParallaxSpeed = 0.5f; // Parallax scrolling speed

        public Map2D(Texture2D backgroundTexture, Texture2D[] layers) : base(backgroundTexture)
        {
            this.layers = layers;
            layerPositions = new Vector2[layers.Length];

            // Initialize layer positions
            for (int i = 0; i < layers.Length; i++)
            {
                layerPositions[i] = Vector2.Zero;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw background
            base.Draw(spriteBatch);

            // Draw parallax layers
            for (int i = 0; i < layers.Length; i++)
            {
                spriteBatch.Draw(layers[i], layerPositions[i], Color.White);
            }
        }

        public void UpdateLayers(float deltaX)
        {
            // Update layer positions for parallax scrolling
            for (int i = 0; i < layers.Length; i++)
            {
                layerPositions[i].X -= deltaX * ParallaxSpeed * (i + 1);
            }
        }
    }
}
