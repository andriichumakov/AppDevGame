using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Projectile : Entity
    {
        public int Damage { get; set; }
        public Vector2 Direction { get; set; }
        public float Speed { get; set; }

        public Projectile(Vector2 position, Vector2 velocity, int health, int damage, Vector2 direction, float speed)
            : base(position, velocity, health)
        {
            Damage = damage;
            Direction = direction;
            Speed = speed;
        }

        public override void Update(GameTime gameTime)
        {
            // Update projectile position
            Position += Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
            Console.WriteLine($"Projectile updated position to {Position}");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the projectile
            base.Draw(spriteBatch);
            Console.WriteLine("Drawing projectile");
        }

        public void OnCollision(Character target)
        {
            target.Health -= Damage;
            Console.WriteLine($"Projectile collided with {target} dealing {Damage} damage");
        }
    }
}
