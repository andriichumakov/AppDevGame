using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AppDevGame
{
    public class Character : Entity
    {
        public int Lives { get; set; }
        public Inventory Inventory { get; private set; }
        public Weapon EquippedWeapon { get; private set; }

        public Character(Vector2 position, Vector2 velocity, int health, int lives)
            : base(position, velocity, health)
        {
            Lives = lives;
            Inventory = new Inventory(10); // Example size of inventory
            EquippedWeapon = new Weapon(0, 0.0f); // Default weapon with no damage and fire rate
        }

        public virtual void Move(Vector2 direction)
        {
            Position += direction;
            Console.WriteLine($"Character moved to {Position}");
        }

        public virtual void PickUp(Item item)
        {
            if (Inventory.AddItem(item))
            {
                Console.WriteLine("Character picked up an item");
            }
            else
            {
                Console.WriteLine("Inventory is full, cannot pick up item");
            }
        }

        public virtual void Attack(Character target)
        {
            if (EquippedWeapon != null)
            {
                target.Health -= EquippedWeapon.DamageOutput;
                Console.WriteLine($"Attacked {target} with {EquippedWeapon.DamageOutput} damage");
            }
            else
            {
                Console.WriteLine("No weapon equipped");
            }
        }
    }
}
