using Microsoft.Xna.Framework;

namespace AppDevGame
{
    public class NPC : Character
    {
        public NPC(Vector2 position, Vector2 velocity, int health)
            : base(position, velocity, health)
        {
        }

        public void Interact(Character character)
        {
            Console.WriteLine($"NPC interacted with {character}");

            // Example: Dialogue
            Console.WriteLine("NPC: Hello traveler! How can I assist you today?");

            // Example: Quest
            Console.WriteLine("NPC: I have a quest for you! Can you retrieve the lost artifact from the dungeon?");

            // Example: Trading
            if (character is Player player)
            {
                Console.WriteLine("NPC: Would you like to trade?");
                Console.WriteLine("NPC: I have some rare items for sale.");
                Console.WriteLine("NPC: What would you like to buy?");
                // Implement trading logic here, such as displaying available items and handling transactions
            }
        }
    }
}
