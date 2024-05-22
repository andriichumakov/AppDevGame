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
        public bool IsFlipped { get; private set; } // Indicates if the frame is flipped

        public Animation(List<Texture2D> frames, double frameTime)
        {
            Frames = frames ?? throw new ArgumentNullException(nameof(frames));
            FrameTime = frameTime;
            CurrentFrame = 0;
            timeCounter = 0.0;
            IsFlipped = false;
        }

        public void Update(GameTime gameTime, Vector2 velocity)
        {
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (timeCounter >= FrameTime)
            {
                CurrentFrame = (CurrentFrame + 1) % Frames.Count;
                timeCounter -= FrameTime;
            }

            // Check velocity to determine if flipping is needed
            IsFlipped = velocity.X < 0; // Flip if moving left
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Texture2D currentTexture = Frames[CurrentFrame];
            SpriteEffects effects = IsFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(currentTexture, position, null, Color.White, 0f, Vector2.Zero, 1f, effects, 0);
        }


        public void FlipFrame()
        {
            IsFlipped = !IsFlipped;
        }
    }
}
