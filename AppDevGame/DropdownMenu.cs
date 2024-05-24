using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AppDevGame
{
    public class DropdownMenu : UIElement, IClickable
    {
        private List<string> _options;
        private bool _isExpanded;
        private int _selectedIndex;
        private Texture2D _optionTexture;

        public DropdownMenu(Rectangle bounds, Color backgroundColor, Color textColor, List<string> options) 
            : base(bounds, new Texture2D(MainApp.GetInstance().GraphicsDevice, 1, 1), backgroundColor, textColor, options[0])
        {
            _options = options;
            _isExpanded = false;
            _selectedIndex = 0;
            _texture = new Texture2D(MainApp.GetInstance().GraphicsDevice, 1, 1);
            _texture.SetData(new[] { backgroundColor });
            _optionTexture = new Texture2D(MainApp.GetInstance().GraphicsDevice, 1, 1);
            _optionTexture.SetData(new[] { Color.LightGray });
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            var font = MainApp.GetInstance()._fontLoader.GetResource("Default");
            if (font == null)
            {
                MainApp.Log("Error: Font 'Default' not found.");
                return;
            }

            var textSize = font.MeasureString(_text);
            var textPosition = new Vector2(_bounds.X + (_bounds.Width - textSize.X) / 2, _bounds.Y + (_bounds.Height - textSize.Y) / 2);
            spriteBatch.DrawString(font, _text, textPosition, _textColor);

            if (_isExpanded)
            {
                for (int i = 0; i < _options.Count; i++)
                {
                    Rectangle optionBounds = new Rectangle(_bounds.X, _bounds.Y + _bounds.Height * (i + 1), _bounds.Width, _bounds.Height);
                    spriteBatch.Draw(_optionTexture, optionBounds, Color.LightGray);
                    var optionTextSize = font.MeasureString(_options[i]);
                    var optionTextPosition = new Vector2(optionBounds.X + (optionBounds.Width - optionTextSize.X) / 2, optionBounds.Y + (optionBounds.Height - optionTextSize.Y) / 2);
                    spriteBatch.DrawString(font, _options[i], optionTextPosition, _textColor);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Handle updates here if necessary
        }

        public void HandleClick(Point mousePosition, GameTime gameTime)
        {
            if (_bounds.Contains(mousePosition))
            {
                _isExpanded = !_isExpanded;
            }
            else if (_isExpanded)
            {
                for (int i = 0; i < _options.Count; i++)
                {
                    Rectangle optionBounds = new Rectangle(_bounds.X, _bounds.Y + _bounds.Height * (i + 1), _bounds.Width, _bounds.Height);
                    if (optionBounds.Contains(mousePosition))
                    {
                        _selectedIndex = i;
                        _text = _options[i];
                        _isExpanded = false;
                        break;
                    }
                }
            }
        }
    }
}