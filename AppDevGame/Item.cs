using System;

namespace AppDevGame
{
    public class Item
    {
        public virtual void Use(Character character)
        {
            Console.WriteLine("Item used by character");
        }

        public virtual void Draw()
        {
            Console.WriteLine("Drawing item");
        }

        public virtual void Update()
        {
            Console.WriteLine("Updating item");
        }
    }

    public class InventoryItem : Item
    {
        public bool IsInInventory { get; set; }

        public override void Draw()
        {
            base.Draw();
            Console.WriteLine("Drawing inventory item");
        }
    }

    public class Weapon : InventoryItem
    {
        public int DamageOutput { get; set; }
        public float FireRate { get; set; }

        public override void Use(Character character)
        {
            base.Use(character);
            Console.WriteLine($"Weapon used with {DamageOutput} damage and {FireRate} fire rate");
        }

        public override void Draw()
        {
            base.Draw();
            Console.WriteLine("Drawing weapon");
        }

        public override void Update()
        {
            base.Update();
            Console.WriteLine("Updating weapon");
        }
    }

    public class Potion : InventoryItem
    {
        public int HealAmount { get; set; }

        public override void Use(Character character)
        {
            base.Use(character);
            Console.WriteLine($"Potion used to heal character by {HealAmount} health");
        }

        public override void Draw()
        {
            base.Draw();
            Console.WriteLine("Drawing potion");
        }

        public override void Update()
        {
            base.Update();
            Console.WriteLine("Updating potion");
        }
    }
}
