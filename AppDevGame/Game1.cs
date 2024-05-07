using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

using System;

using AppDevGame;

namespace AppDevGame
{
    public class Game1 : Game
    {
        // Screen size constants
        public const int ScreenWidth = 800;
        public const int ScreenHeight = 600;

        // assets and graphics manage
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font; // Declare a SpriteFont variable to hold the font

        private Texture2D background;

        // Declare a Level object
        private Level level;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set window size
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            _graphics.ApplyChanges();

            // Create an instance of Level
            level = new Level(new Vector2(800, 533), background);
            Console.WriteLine("omg");
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the SpriteFont
            _font = Content.Load<SpriteFont>("Default");

            using (FileStream stream = new FileStream("Content/PlaceholderBackground.jpg", FileMode.Open))
            {
                background = Texture2D.FromStream(GraphicsDevice, stream);
            }

            //background = Content.Load<Texture2D>("PlaceholderBackground");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update the level
            level.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Draw the level
            level.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
