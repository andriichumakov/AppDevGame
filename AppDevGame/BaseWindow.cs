using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace AppDevGame
{
    // BaseWindow class
    public abstract class BaseWindow 
    {
        protected int _width;
        protected int _height;
        protected Texture2D _background;
        protected List<UIElement> _elements = new List<UIElement>(); 
        protected MouseState _previousMouseState;

        public BaseWindow(int width, int height, Texture2D background = null)
        {
            this._width = width;
            this._height = height;
            this._background = background;
            this._previousMouseState = Mouse.GetState();
        }

        public virtual void Setup()
        {
            MainApp.Log("Setting up window...");
            MainApp.GetInstance().GetGraphicsManager().PreferredBackBufferWidth = this._width;
            MainApp.GetInstance().GetGraphicsManager().PreferredBackBufferHeight = this._height;
            //MainApp.GetInstance().GetGraphicsManager().IsFullScreen = true;  //command this out to make the game go windowed mode
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

        public virtual void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            foreach (var element in _elements)
            {
                element.LoadContent(graphicsDevice, content);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();

            if (currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
            {
                Point mousePosition = new Point(currentMouseState.X, currentMouseState.Y);

                foreach (UIElement element in _elements)
                {
                    if (element is IClickable clickable)
                    {
                        clickable.HandleClick(mousePosition, gameTime);
                    }
                }
            }

            foreach (UIElement element in _elements)
            {
                element.Update(gameTime);
            }

            _previousMouseState = currentMouseState;
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

        public Vector2 CalcButtonPosition(int numOfButtons, int buttonWidth, int buttonHeight, int verticalSpacing) 
        {
            // calculates the position of a button based on the number of buttons, button width, button height, and spacing
            int x = (_width - buttonWidth) / 2;
            int y = (_height - (buttonHeight * numOfButtons + verticalSpacing * (numOfButtons - 1))) / 2;
            return new Vector2(x, y);
        }
    }
}