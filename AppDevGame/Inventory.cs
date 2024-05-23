using System;
using System.Collections.Generic;

namespace AppDevGame
{
    public class Inventory
    {
        public List<Item> Slots { get; set; }
        private int maxSlots;

        public Inventory(int size)
        {
            maxSlots = size;
            Slots = new List<Item>();
        }

        public void AddItem(Item item)
        {
            if (CountItems() < maxSlots)
            {
                    Slots.Add(item);
            }
            else
            {
                Console.WriteLine("Inventory is full, cannot add more items");
            }
        }

        public Item RemoveItem(Item item)
        {
            int slotIndex = Slots.IndexOf(item);
            if (slotIndex >= 0)
            {
                Slots[slotIndex] = emptyItem;
                Console.WriteLine($"Item removed from slot {slotIndex}");
                return item;
            }
            else
            {
                Console.WriteLine("Item not found in inventory");
                return item;  // return the item even if it's not found, no nulls
            }
        }

        public Item GetItem(int slot)
        {
            if (slot >= 0 && slot < Slots.Count)
            {
                return Slots[slot];
            }
            Console.WriteLine("Invalid slot index");
            return null;
        }

        private int CountItems()
        {
            return Slots.Count;
        }
    }
}