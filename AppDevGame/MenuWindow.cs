using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class MenuWindow : BaseWindow
    {
        public MenuWindow(int width, int height, Texture2D background) : base(width, height, background)
        {
        }

        public override void Setup()
        {
            base.Setup();
            // Add initial UI elements if needed
        }

        public void AddButton(Button button)
        {
            AddElement(button);
        }

        public void RemoveButton(Button button)
        {
            RemoveElement(button);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Additional update logic if needed
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            // Additional draw logic if needed
        }
    }
}