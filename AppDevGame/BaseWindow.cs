using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

using AppDevGame;

namespace AppDevGame
{
    public abstract class BaseWindow 
    {
        protected int _width;
        protected int _height;
        protected Texture2D _background;
        protected List<UIElement> _elements = new List<UIElement>(); 

        public BaseWindow(int width, int height, Texture2D background=null)
        {
            this._width = width;
            this._height = height;
            this._background = background;
        }

        public virtual void Setup()
        {
            MainApp.Log("Setting up window...");
            MainApp.GetInstance().GetGraphicsManager().PreferredBackBufferWidth = this._width;
            MainApp.GetInstance().GetGraphicsManager().PreferredBackBufferHeight = this._height;
            MainApp.GetInstance().GetGraphicsManager().IsFullScreen = false;
            MainApp.GetInstance().GetGraphicsManager().ApplyChanges();
        }

        public void AddElement(UIElement element)
        {
            this._elements.Add(element);
        }

        public void RemoveElement(UIElement element)
        {
            this._elements.Remove(element);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (UIElement element in this._elements)
            {
                element.Update(gameTime);
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_background != null)
            {
                spriteBatch.Draw(_background, new Rectangle(0, 0, _width, _height), Color.White);
            }

            foreach (UIElement element in this._elements)
            { 
                element.Draw(spriteBatch);
            }
        }

        public void Clear()
        {
            MainApp.Log("Clearing window...");
            MainApp.GetInstance().GraphicsDevice.Clear(Color.Black);
        }
    }
}