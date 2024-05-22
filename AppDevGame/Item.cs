using System;

namespace AppDevGame
{
    public class Item
    {
        public bool IsPickedUp { get; set; }

        public virtual void PickUp(Character character)
        {
            IsPickedUp = true;
            Console.WriteLine("Item picked up by character");
        }

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

public class Weapon : Item
{
    public string Type { get; set; }
    public int AmmoCount { get; set; }
    public int Durability { get; set; }

    public override void Use(Character character)
    {
        base.Use(character);
        Console.WriteLine($"Weapon used with {DamageOutput} damage and {FireRate} fire rate");
    }

    public void Reload(int ammoToAdd)
    {
        AmmoCount += ammoToAdd;
        Console.WriteLine($"Reloaded weapon with {ammoToAdd} ammo");
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

public class Heart : Item
{
    public int HealAmount { get; set; }
    public int UsesRemaining { get; set; }
    public TimeSpan Cooldown { get; set; }

    public override void Use(Character character)
    {
        if (UsesRemaining > 0)
        {
            base.Use(character);
            Console.WriteLine($"Heart used to heal character by {HealAmount} health");
            UsesRemaining--;
        }
        else
        {
            Console.WriteLine("Heart is exhausted and cannot be used.");
        }
    }

    public override void Draw()
    {
        base.Draw();
        Console.WriteLine("Drawing heart");
    }

    public override void Update()
    {
        base.Update();
        Console.WriteLine("Updating heart");
    }
}

public class Coin : Item
{
    public int Value { get; set; }

    public override void Use(Character character)
    {
        base.Use(character);
        Console.WriteLine($"Coin worth {Value} used to increase character's wealth");
    }

    public override void Draw()
    {
        base.Draw();
        Console.WriteLine("Drawing coin");
    }

    public override void Update()
    {
        base.Update();
        Console.WriteLine("Updating coin");
    }
}
