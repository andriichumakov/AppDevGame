using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppDevGame
{
    public abstract class LevelWindow : BaseWindow
    {
        protected List<Entity> _entities;
        protected List<Entity> _entitiesToAdd;
        protected List<Entity> _entitiesToRemove;
        protected Player _player;
        protected Rectangle _frameSize;
        protected Rectangle _actualSize;
        protected int _margin = 50;
        protected Random _random;

        public Rectangle ActualSize => _actualSize;
        public Rectangle FrameSize => _frameSize;

        public List<Entity> Entities => _entities;
        public Player Player => _player;
        public List<Entity> EntitiesToRemove => _entitiesToRemove; // Added property

        public LevelWindow(int frameWidth, int frameHeight, int actualWidth, int actualHeight, Texture2D background = null)
            : base(frameWidth, frameHeight, background)
        {
            _entities = new List<Entity>();
            _entitiesToAdd = new List<Entity>();
            _entitiesToRemove = new List<Entity>();
            _frameSize = new Rectangle(0, 0, frameWidth, frameHeight);
            _actualSize = new Rectangle(0, 0, actualWidth, actualHeight);
            _random = new Random();
        }

        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            if (_entities.Contains(entity))
            {
                _entities.Remove(entity);
            }
        }

        public void SetPlayer(Player player)
        {
            _player = player;
            AddEntity(player);
        }

        protected virtual void AdjustFrame()
        {
            try
            {
                if (_player != null)
                {
                    // Calculate the new frame position based on the player's position
                    int newFrameX = (int)(_player.Position.X + _player.Hitbox.Width / 2 - _frameSize.Width / 2);
                    int newFrameY = (int)(_player.Position.Y + _player.Hitbox.Height / 2 - _frameSize.Height / 2);

                    // Ensure the frame does not move outside of the actual level bounds
                    newFrameX = Math.Clamp(newFrameX, 0, _actualSize.Width - _frameSize.Width);
                    newFrameY = Math.Clamp(newFrameY, 0, _actualSize.Height - _frameSize.Height);

                    // Move the frame by the calculated deltas
                    MoveFrame(newFrameX - _frameSize.X, newFrameY - _frameSize.Y);
                }
            }
            catch (Exception ex)
            {
                MainApp.Log($"Error during AdjustFrame: {ex.Message}");
                throw;
            }
        }


        public void MoveFrame(int deltaX, int deltaY)
        {
            _frameSize.X = Math.Clamp(_frameSize.X + deltaX, 0, _actualSize.Width - _frameSize.Width);
            _frameSize.Y = Math.Clamp(_frameSize.Y + deltaY, 0, _actualSize.Height - _frameSize.Height);
        }

        public override void Update(GameTime gameTime)
        {
            try
            {
                base.Update(gameTime);
                _player?.Update(gameTime);
                AdjustFrame();
                foreach (var entity in _entities.ToList())
                {
                    if (_frameSize.Contains(entity.Hitbox) || _frameSize.Intersects(entity.Hitbox))
                    {
                        entity.Update(gameTime);
                    }
                }

                CheckCollisions();
                _entities.RemoveAll(entity => entity is Enemy enemy && enemy.IsDead());
            }
            catch (Exception ex)
            {
                MainApp.Log($"Error during LevelWindow.Update: {ex.Message}");
                throw;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            try
            {
                if (_background != null)
                {
                    Rectangle sourceRectangle = new Rectangle(
                        _frameSize.X, _frameSize.Y,
                        Math.Min(_frameSize.Width, _background.Width - _frameSize.X),
                        Math.Min(_frameSize.Height, _background.Height - _frameSize.Y)
                    );

                    spriteBatch.Draw(
                        _background,
                        new Rectangle(0, 0, _frameSize.Width, _frameSize.Height),
                        sourceRectangle,
                        Color.White
                    );
                }

                foreach (var entity in _entities)
                {
                    if (_frameSize.Contains(entity.Hitbox) || _frameSize.Intersects(entity.Hitbox))
                    {
                        entity.Draw(spriteBatch, new Vector2(_frameSize.X, _frameSize.Y));
                    }
                }
            }
            catch (Exception ex)
            {
                MainApp.Log($"Error during LevelWindow.Draw: {ex.Message}");
                throw;
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

        private void CheckCollisions()
        {
            try
            {
                var entities = _entities.ToList();
                for (int i = 0; i < entities.Count; i++)
                {
                    if (!_frameSize.Contains(entities[i].Hitbox) && !_frameSize.Intersects(entities[i].Hitbox))
                    {
                        continue;
                    }

                    for (int j = i + 1; j < entities.Count; j++)
                    {
                        if (entities[i].Hitbox.Intersects(entities[j].Hitbox))
                        {
                            entities[i].OnCollision(entities[j]);
                            entities[j].OnCollision(entities[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MainApp.Log($"Error during CheckCollisions: {ex.Message}");
                throw;
            }
        }

        public List<Entity> GetEntitiesInRange(Vector2 position, int range)
        {
            try
            {
                List<Entity> entitiesInRange = new List<Entity>();
                float rangeSquared = range * range;

                foreach (var entity in _entities)
                {
                    float distanceSquared = Vector2.DistanceSquared(position, entity.Position);
                    if (distanceSquared <= rangeSquared)
                    {
                        entitiesInRange.Add(entity);
                    }
                }

                return entitiesInRange;
            }
            catch (Exception ex)
            {
                MainApp.Log($"Error during GetEntitiesInRange: {ex.Message}");
                throw;
            }
        }

        public void SetFrameSize(int width, int height)
        {
            _frameSize.Width = width;
            _frameSize.Height = height;
        }

        public void SetActualSize(int width, int height)
        {
            _actualSize.Width = width;
            _actualSize.Height = height;
        }

        private Vector2 GenerateRandomPosition()
        {
            int x = _random.Next(0, _actualSize.Width);
            int y = _random.Next(0, _actualSize.Height);
            return new Vector2(x, y);
        }

        protected void AddCoins(int numberOfCoins)
        {
            Texture2D coinTexture = MainApp.GetInstance()._imageLoader.GetResource("Coin");
            if (coinTexture != null)
            {
                for (int i = 0; i < numberOfCoins; i++)
                {
                    Vector2 position = GenerateRandomPosition();
                    AddEntity(new Coin(this, coinTexture, position));
                }
            }
        }
    }
}
