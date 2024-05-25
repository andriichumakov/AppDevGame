using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AppDevGame
{
    public class DropdownMenu : UIElement
    {
        private List<string> _items;
        private int _selectedIndex;
        private bool _isOpen;
        private SpriteFont _font;
        private Texture2D _backgroundTexture;

        public DropdownMenu(GraphicsDevice graphicsDevice, Rectangle bounds, Color backgroundColor, Color foregroundColor, string defaultItem, List<string> items, SpriteFont font)
            : base(bounds, null, backgroundColor, foregroundColor, defaultItem)
        {
            _items = items;
            _font = font;
            _selectedIndex = items.IndexOf(defaultItem);
            _isOpen = false;

            _backgroundTexture = new Texture2D(graphicsDevice, 1, 1);
            _backgroundTexture.SetData(new[] { backgroundColor });
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the dropdown background
            spriteBatch.Draw(_backgroundTexture, _bounds, _backgroundColor);

            // Draw the selected item
            if (_font != null && _selectedIndex >= 0)
            {
                string selectedItem = _items[_selectedIndex];
                Vector2 textSize = _font.MeasureString(selectedItem);
                Vector2 textPosition = new Vector2(_bounds.X + 5, _bounds.Y + (_bounds.Height - textSize.Y) / 2);
                spriteBatch.DrawString(_font, selectedItem, textPosition, _textColor);
            }

            // Draw dropdown items if open
            if (_isOpen)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    Rectangle itemBounds = new Rectangle(_bounds.X, _bounds.Y + (i + 1) * _bounds.Height, _bounds.Width, _bounds.Height);
                    spriteBatch.Draw(_backgroundTexture, itemBounds, _backgroundColor);

                    if (_font != null)
                    {
                        Vector2 textSize = _font.MeasureString(_items[i]);
                        Vector2 textPosition = new Vector2(itemBounds.X + 5, itemBounds.Y + (itemBounds.Height - textSize.Y) / 2);
                        spriteBatch.DrawString(_font, _items[i], textPosition, _textColor);
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Update logic if needed
        }

        public void HandleClick(Point clickPosition, GameTime gameTime)
        {
            if (_bounds.Contains(clickPosition))
            {
                _isOpen = !_isOpen;
            }
            else if (_isOpen)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    Rectangle itemBounds = new Rectangle(_bounds.X, _bounds.Y + (i + 1) * _bounds.Height, _bounds.Width, _bounds.Height);
                    if (itemBounds.Contains(clickPosition))
                    {
                        _selectedIndex = i;
                        _isOpen = false;
                        break;
                    }
                }
            }
        }

        public string GetSelectedItem()
        {
            return _items[_selectedIndex];
        }
    }
}