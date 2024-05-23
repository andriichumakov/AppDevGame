using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace AppDevGame
{
    public abstract class LevelWindow : BaseWindow
    {
        protected List<Entity> _entities;
        protected Player _player;
        protected Rectangle _frameSize;
        protected Rectangle _actualSize;
        private int _margin = 50; // Distance from the frame border to start moving the frame

        public Rectangle ActualSize => _actualSize;

        public LevelWindow(int frameWidth, int frameHeight, int actualWidth, int actualHeight, Texture2D background = null)
            : base(frameWidth, frameHeight, background)
        {
            _entities = new List<Entity>();
            _frameSize = new Rectangle(0, 0, frameWidth, frameHeight);
            _actualSize = new Rectangle(0, 0, actualWidth, actualHeight);
        }

        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
        }

        public void SetPlayer(Player player)
        {
            _player = player;
            AddEntity(player);
        }

        private void AdjustFrame()
        {
            if (_player != null)
            {
                if (_player.Position.X < _frameSize.X + _margin)
                {
                    _frameSize.X = (int) Math.Max(0, _player.Position.X - _margin);
                }
                else if (_player.Position.X + _player.Hitbox.Width > _frameSize.X + _frameSize.Width - _margin)
                {
                    _frameSize.X = (int) Math.Min(_actualSize.Width - _frameSize.Width, _player.Position.X + _player.Hitbox.Width - _frameSize.Width + _margin);
                }

                if (_player.Position.Y < _frameSize.Y + _margin)
                {
                    _frameSize.Y = (int) Math.Max(0, _player.Position.Y - _margin);
                }
                else if (_player.Position.Y + _player.Hitbox.Height > _frameSize.Y + _frameSize.Height - _margin)
                {
                    _frameSize.Y = (int) Math.Min(_actualSize.Height - _frameSize.Height, _player.Position.Y + _player.Hitbox.Height - _frameSize.Height + _margin);
                }
            }
        }

        public void MoveFrame(int deltaX, int deltaY)
        {
            _frameSize.X = Math.Clamp(_frameSize.X + deltaX, 0, _actualSize.Width - _frameSize.Width);
            _frameSize.Y = Math.Clamp(_frameSize.Y + deltaY, 0, _actualSize.Height - _frameSize.Height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _player?.Update(gameTime);
            AdjustFrame(); // Adjust the frame based on the player's position

            foreach (var entity in _entities)
            {
                if (_frameSize.Contains(entity.Hitbox) || _frameSize.Intersects(entity.Hitbox))
                {
                    entity.Update(gameTime);
                }
            }

            CheckCollisions();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the visible portion of the background
            if (_background != null)
            {
                spriteBatch.Draw(
                    _background,
                    destinationRectangle: new Rectangle(0, 0, _frameSize.Width, _frameSize.Height),
                    sourceRectangle: _frameSize,
                    color: Color.White
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

        private void CheckCollisions()
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                if (!_frameSize.Contains(_entities[i].Hitbox) && !_frameSize.Intersects(_entities[i].Hitbox))
                {
                    continue;
                }

                for (int j = i + 1; j < _entities.Count; j++)
                {
                    if (_entities[i].Hitbox.Intersects(_entities[j].Hitbox))
                    {
                        _entities[i].OnCollision(_entities[j]);
                        _entities[j].OnCollision(_entities[i]);
                    }
                }
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
    }
}
