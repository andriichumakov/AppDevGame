using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace AppDevGame
{
    public class MenuWindow : BaseWindow
    {
        protected List<UIElement> _uiElements;
        private List<UIElement> _elementsToAdd;
        private List<UIElement> _elementsToRemove;

        public MenuWindow(int width, int height, Texture2D background) : base(width, height, background)
        {
            _uiElements = new List<UIElement>();
            _elementsToAdd = new List<UIElement>();
            _elementsToRemove = new List<UIElement>();
        }

        public override void Setup()
        {
            base.Setup();
            // Add initial UI elements if needed
        }

        public new void AddElement(UIElement element)
        {
            _elementsToAdd.Add(element);
        }

        public new void RemoveElement(UIElement element)
        {
            _elementsToRemove.Add(element);
        }

        private void ApplyPendingChanges()
        {
            foreach (var element in _elementsToAdd)
            {
                _uiElements.Add(element);
            }
            _elementsToAdd.Clear();

            foreach (var element in _elementsToRemove)
            {
                _uiElements.Remove(element);
            }
            _elementsToRemove.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            ApplyPendingChanges();

            // Iterate over a copy of the list to avoid modifying it during iteration
            var elementsCopy = new List<UIElement>(_uiElements);
            foreach (var element in elementsCopy)
            {
                element.Update(gameTime);
            }

            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                foreach (var element in elementsCopy)
                {
                    if (element is IClickable clickable)
                    {
                        clickable.HandleClick(mouseState.Position, gameTime);
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach (var element in _uiElements)
            {
                element.Draw(spriteBatch);
            }
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            base.LoadContent(graphicsDevice, content);
            foreach (var element in _uiElements)
            {
                element.LoadContent(graphicsDevice, content);
            }
        }

        public void SetupElements()
        {
            // Clear and re-setup elements to ensure no overlapping text
            ClearElements();
            Setup();
        }

        private void ClearElements()
        {
            // Clear all existing elements
            _uiElements.Clear();
            _elementsToAdd.Clear();
            _elementsToRemove.Clear();
        }

        public void UpdateTexts()
        {
            foreach (var element in _uiElements)
            {
                element.UpdateText();
            }
        }
    }
}