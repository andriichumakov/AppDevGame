using Microsoft.Xna.Framework;

namespace AppDevGame
{
    public class Enemy : Character
    {
        public int Damage { get; set; }

        public Enemy(Vector2 position, Vector2 velocity, int health, int damage)
            : base(position, velocity, health)
        {
            Damage = damage;
        }

        public override void Attack(Character target)
        {
            base.Attack(target);
            Console.WriteLine($"Enemy attacked {target} with {Damage} damage");
        }
    }
}
