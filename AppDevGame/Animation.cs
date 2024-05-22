using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Animation
    {
        public List<Texture2D> Frames { get; private set; }
        public int CurrentFrame { get; private set; }
        public double FrameTime { get; private set; } // Time each frame is displayed
        private double timeCounter; // Time since last frame update

        public Animation(List<Texture2D> frames, double frameTime)
        {
            Frames = frames ?? throw new ArgumentNullException(nameof(frames));
            FrameTime = frameTime;
            CurrentFrame = 0;
            timeCounter = 0.0;
        }

        public void Update(GameTime gameTime)
        {
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (timeCounter >= FrameTime)
            {
                CurrentFrame = (CurrentFrame + 1) % Frames.Count;
                timeCounter -= FrameTime;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(Frames[CurrentFrame], position, Color.White);
        }
    }
}
