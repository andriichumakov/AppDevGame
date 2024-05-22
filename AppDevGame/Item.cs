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
        }

        public virtual void Update()
        {
        }
    }

    public class Weapon : Item
    {
        public int DamageOutput { get; set; }
        public float FireRate { get; set; }
    }

    public class Heart : Item
    {
    }

    public class Coin : Item
    {
    }
}
