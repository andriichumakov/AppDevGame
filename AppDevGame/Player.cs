using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;

namespace AppDevGame
{
    public class Player : Entity
    {
        private float _speed;

        public Player(LevelWindow level, Texture2D texture, Vector2 position, float speed = 200f)
            : base(level, texture, position)
        {
            _speed = speed;
        }

        public Vector2 Position => _position;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Vector2 movement = Vector2.Zero;
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.W))
            {
                movement.Y -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (state.IsKeyDown(Keys.S))
            {
                movement.Y += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (state.IsKeyDown(Keys.A))
            {
                movement.X -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (state.IsKeyDown(Keys.D))
            {
                movement.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            _position += movement;
            _hitbox.Location = _position.ToPoint();

            // Ensure the player does not move out of the actual level bounds
            _position.X = Math.Clamp(_position.X, 0, _level.ActualSize.Width - _hitbox.Width);
            _position.Y = Math.Clamp(_position.Y, 0, _level.ActualSize.Height - _hitbox.Height);
        }
    }
}
