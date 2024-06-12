using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AppDevGame
{
    public class Button : UIElement, IClickable
    {
        protected ICommand _onClick;
        private TimeSpan _lastClickTime;
        private static readonly TimeSpan DebounceTime = TimeSpan.FromMilliseconds(300);
        private SpriteFont _font;
        private string _textKey;

        public Button(Rectangle bounds, Color backgroundColor, Color textColor, string textKey, ICommand onClick, SpriteFont font) 
            : base(bounds, new Texture2D(MainApp.GetInstance().GraphicsDevice, 1, 1), backgroundColor, textColor, textKey)
        {
            _texture = new Texture2D(MainApp.GetInstance().GraphicsDevice, 1, 1);
            _texture.SetData(new[] { backgroundColor });
            _onClick = onClick;
            _lastClickTime = TimeSpan.Zero;
            _font = font;
            _textKey = textKey;
            UpdateText(); // Get localized string during initialization
        }

        public void UpdateText()
        {
            _text = MainApp.GetInstance().LocLoader.GetString(_textKey); // Update text based on localization
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _bounds, _backgroundColor);
            if (_font != null)
            {
                var textSize = _font.MeasureString(_text);
                var textPosition = new Vector2(_bounds.X + (_bounds.Width - textSize.X) / 2, _bounds.Y + (_bounds.Height - textSize.Y) / 2);
                spriteBatch.DrawString(_font, _text, textPosition, _textColor);
            }
            else
            {
                MainApp.Log("Error: Font not loaded.");
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Handle button updates here
        }

        public void HandleClick(Point mousePosition, GameTime gameTime)
        {
            if (_bounds.Contains(mousePosition))
            {
                var currentTime = gameTime.TotalGameTime;
                if (currentTime - _lastClickTime > DebounceTime && MainApp.CanPerformAction())
                {
                    _lastClickTime = currentTime;
                    MainApp.RecordAction(); // Record the action to enforce delay

                    if (_onClick != null)
                    {
                        try
                        {
                            _onClick.Execute();
                        }
                        catch (Exception ex)
                        {
                            MainApp.Log($"Error executing button command: {ex.Message}");
                        }
                    }
                    else
                    {
                        MainApp.Log("Error: Button click command is null.");
                    }
                }
                else
                {
                    MainApp.Log("Click ignored due to debounce.");
                }
            }
        }
    }
}