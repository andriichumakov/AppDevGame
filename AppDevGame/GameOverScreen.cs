using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppDevGame
{
    public class GameOverScreen : BaseWindow
    {
        private SpriteFont _font;
        private LevelWindow _currentLevel;

        public GameOverScreen(int width, int height, Texture2D backgroundTexture, LevelWindow currentLevel)
            : base(width, height, backgroundTexture)
        {
            _font = MainApp.GetInstance()._fontLoader.GetResource("Default");
            _currentLevel = currentLevel;
            SetupElements();
        }

        private void SetupElements()
        {
            int buttonWidth = 200;
            int buttonHeight = 50;
            int spacing = 20;
            int startX = (_width - buttonWidth * 2 - spacing) / 2; // Center the buttons
            int startY = (_height / 2) + 50; // Position below the "GAME OVER" text

            AddElement(new Button(new Rectangle(startX, startY, buttonWidth, buttonHeight), Color.Gray, Color.White, "Restart Level", new RestartLevelCommand(_currentLevel), _font));
            AddElement(new Button(new Rectangle(startX + buttonWidth + spacing, startY, buttonWidth, buttonHeight), Color.Gray, Color.White, "Main Menu", new LoadWindowCommand(WindowManager.GetInstance(), MainApp.GetInstance().MainMenu), _font));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            string gameOverText = "GAME OVER";
            Vector2 textSize = _font.MeasureString(gameOverText);
            Vector2 textPosition = new Vector2((_width - textSize.X) / 2, (_height - textSize.Y) / 2);
            spriteBatch.DrawString(_font, gameOverText, textPosition, Color.Red);
        }
    }
}
