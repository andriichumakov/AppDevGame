using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Character : Entity
    {
        public int Lives { get; set; }
        public Inventory Inventory { get; private set; }
        public Weapon EquippedWeapon { get; set; }

        public Character(Vector2 position, Vector2 velocity, int health, int lives, Animation animation)
            : base(position, velocity, health, animation)
        {
            Lives = lives;
            Inventory = new Inventory(2); // Example with 2 inventory slots
        }

        public override void Update(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Animation.Update(gameTime);
        }

        public void Move(Vector2 direction)
        {
            Velocity = direction;
        }

        public void Attack()
        {
            if (EquippedWeapon != null)
            {
                // Attack logic here using EquippedWeapon.DamageOutput
            }
            else
            {
                Console.WriteLine("No weapon equipped");
            }
        }

        public void PickUp(Item item)
        {
            Inventory.AddItem(item);
            item.PickUp(this);
        }

        public void UseItem(int slot)
        {
            var item = Inventory.GetItem(slot);
            if (!(item is EmptyItem))
            {
                item.Use(this);
                // Optionally, remove the item after use if that's the game design
                // Inventory.RemoveItem(item);
            }
            else
            {
                Console.WriteLine("No usable item in the selected slot");
            }
        }

        public void DropItem(Item item)
        {
            Item droppedItem = Inventory.RemoveItem(item);
            if (!(droppedItem is EmptyItem))
            {
                Console.WriteLine($"Dropped item: {droppedItem}");
                // Handle the dropped item (e.g., place it in the game world)
            }
            else
            {
                Console.WriteLine("Item not found in inventory, cannot drop");
            }
        }
    }
}
