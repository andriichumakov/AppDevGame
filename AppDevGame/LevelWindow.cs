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
        private int _margin = 50;

        public Rectangle ActualSize => _actualSize;
        public List<Entity> Entities => _entities;
        public Player Player => _player;

        public LevelWindow(int frameWidth, int frameHeight, int actualWidth, int actualHeight, Texture2D background = null)
            : base(frameWidth, frameHeight, background)
        {
            _entities = new List<Entity>();
            _entitiesToAdd = new List<Entity>();
            _entitiesToRemove = new List<Entity>();
            _frameSize = new Rectangle(0, 0, frameWidth, frameHeight);
            _actualSize = new Rectangle(0, 0, actualWidth, actualHeight);
        }

        public void AddEntity(Entity entity)
        {
            _entitiesToAdd.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            _entitiesToRemove.Add(entity);
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
                int newFrameX = _frameSize.X;
                int newFrameY = _frameSize.Y;

                if (_player.Position.X < _frameSize.X + _margin)
                {
                    newFrameX = (int)Math.Max(0, _player.Position.X - _margin);
                }
                else if (_player.Position.X + _player.Hitbox.Width > _frameSize.X + _frameSize.Width - _margin)
                {
                    newFrameX = (int)Math.Min(_actualSize.Width - _frameSize.Width, _player.Position.X + _player.Hitbox.Width - _frameSize.Width + _margin);
                }

                if (_player.Position.Y < _frameSize.Y + _margin)
                {
                    newFrameY = (int)Math.Max(0, _player.Position.Y - _margin);
                }
                else if (_player.Position.Y + _player.Hitbox.Height > _frameSize.Y + _frameSize.Height - _margin)
                {
                    newFrameY = (int)Math.Min(_actualSize.Height - _frameSize.Height, _player.Position.Y + _player.Hitbox.Height - _frameSize.Height + _margin);
                }

                MoveFrame(newFrameX - _frameSize.X, newFrameY - _frameSize.Y);
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

            // Add and remove entities
            _entities.AddRange(_entitiesToAdd);
            _entitiesToAdd.Clear();

            foreach (var entity in _entitiesToRemove)
            {
                _entities.Remove(entity);
            }
            _entitiesToRemove.Clear();
        }

        public override void Draw(SpriteBatch spriteBatch)
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
                    destinationRectangle: new Rectangle(0, 0, sourceRectangle.Width, sourceRectangle.Height),
                    sourceRectangle: sourceRectangle,
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
            for (int i = 0; i < _entities.Count; i++)
            {
                if (!_frameSize.Contains(_entities[i].Hitbox) && !_frameSize.Intersects(_entities[i].Hitbox))
                {
                    continue;
                }

                for (int j = i + 1; j < _entities.Count; j++)
                {
                    // Skip collision check if either entity is a coin and the other is not a player
                    if ((_entities[i] is Coin || _entities[j] is Coin) &&
                        !(_entities[i] is Player || _entities[j] is Player))
                    {
                        continue;
                    }

                    if (_entities[i].Hitbox.Intersects(_entities[j].Hitbox))
                    {
                        _entities[i].OnCollision(_entities[j]);
                        _entities[j].OnCollision(_entities[i]);
                    }
                }
            }
        }

        public List<Entity> GetEntitiesInRange(Vector2 position, int range)
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
