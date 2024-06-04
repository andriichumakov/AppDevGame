using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System;

namespace AppDevGame
{
    public class SideScrollerLevel : LevelWindow
    {
        private float _fallSpeed = 100f;
        private float _groundLevel;

        public float GroundLevel => _groundLevel;

        public SideScrollerLevel(int frameWidth, int frameHeight, int actualWidth, int actualHeight, float groundLevel, Texture2D background = null)
            : base(frameWidth, frameHeight, actualWidth, actualHeight, background)
        {
            _groundLevel = groundLevel;
        }

        public override void Setup()
        {
            base.Setup();

            // Example setup
            Texture2D playerTexture = MainApp.GetInstance()._imageLoader.GetResource("character");
            if (playerTexture != null)
            {
                SetPlayer(new Player(this, playerTexture, new Vector2(50, _groundLevel - playerTexture.Height)));
            }

            // Add some obstacles
            AddEntity(new Obstacle(this, new Vector2(200, _groundLevel - 50), new Vector2(100, 50)));
            AddEntity(new Obstacle(this, new Vector2(400, _groundLevel - 100), new Vector2(100, 100)));

            // Enable debug bounds for obstacles
            Obstacle.DebugShowBounds = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (var entity in _entities)
            {
                if (entity.Type == EntityType.Player || entity.Type == EntityType.Enemy)
                {
                    if (!IsStandingOnObstacle(entity))
                    {
                        ApplyGravity(entity, gameTime);
                    }
                }
            }
        }

        public bool IsStandingOnObstacle(Entity entity)
        {
            // Check if the entity is standing on any obstacles
            foreach (var other in _entities)
            {
                if (other.Type == EntityType.Obstacle || other.Type == EntityType.HiddenObstacle)
                {
                    if (entity.Hitbox.Bottom == other.Hitbox.Top && entity.Hitbox.Right > other.Hitbox.Left && entity.Hitbox.Left < other.Hitbox.Right)
                    {
                        return true;
                    }
                }
            }

            // Check if the entity is on the ground level
            if (entity.Hitbox.Bottom >= _groundLevel)
            {
                entity.MoveTo(new Vector2(entity.Position.X, _groundLevel - entity.Hitbox.Height));
                return true;
            }

            return false;
        }

        private void ApplyGravity(Entity entity, GameTime gameTime)
        {
            
            float fallAmount = _fallSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 newPosition = entity.Position + new Vector2(0, fallAmount);

            // Check if the entity hits the ground
            if (newPosition.Y + entity.Hitbox.Height > _groundLevel)
            {
                newPosition.Y = _groundLevel - entity.Hitbox.Height;
            }

            entity.MoveTo(newPosition);
        }

        protected override void AdjustFrame()
        {
            try
            {
                if (_player != null)
                {
                    // Calculate the new frame position based on the player's position
                    int newFrameX = _frameSize.X;

                    // Move frame to the right as the player moves right, but not to the left
                    if (_player.Position.X + _player.Hitbox.Width > _frameSize.X + _frameSize.Width - _margin)
                    {
                        newFrameX = (int)Math.Min(_actualSize.Width - _frameSize.Width, _player.Position.X + _player.Hitbox.Width - _frameSize.Width + _margin);
                    }

                    // Move the frame by the calculated deltas
                    MoveFrame(newFrameX - _frameSize.X, 0);
                }
            }
            catch (Exception ex)
            {
                MainApp.Log($"Error during AdjustFrame: {ex.Message}");
                throw;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                // Draw the static background
                if (_background != null)
                {
                    spriteBatch.Draw(
                        _background,
                        new Rectangle(0, 0, _frameSize.Width, _frameSize.Height),
                        new Rectangle(0, 0, _frameSize.Width, _frameSize.Height),
                        Color.White
                    );
                }

                foreach (var entity in _entities)
                {
                    if (_frameSize.Contains(entity.Hitbox) || _frameSize.Intersects(entity.Hitbox))
                    {
                        // Offset the entity's drawing position by the frame's position
                        entity.Draw(spriteBatch, new Vector2(_frameSize.X, _frameSize.Y));
                    }
                }

                if (_player != null)
                {
                    string coinText = "Coins: " + _player.CoinsCollected;
                    SpriteFont font = MainApp.GetInstance()._fontLoader.GetResource("Default");
                    if (font != null)
                    {
                        spriteBatch.DrawString(font, coinText, new Vector2(10, 10), Color.Yellow);
                    }
                }
            }
            catch (Exception ex)
            {
                MainApp.Log($"Error during SideScrollerLevel.Draw: {ex.Message}");
                throw;
            }
        }
    }
}