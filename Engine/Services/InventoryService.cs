﻿using System.Collections.Generic;
using System.Linq;
using Engine.Factories;
using Engine.Models;

namespace Engine.Services
{
    public static class InventoryService
    {
        public static Inventory AddItem(this Inventory inventory, GameItem item)
        {
            return inventory.AddItems(new List<GameItem> { item });
        }

        public static Inventory AddItemFromFactory(this Inventory inventory, int itemTypeID)
        {
            return inventory.AddItems(new List<GameItem> { ItemFactory.CreateGameItem(itemTypeID) });
        }

        public static Inventory AddItems(this Inventory inventory, IEnumerable<GameItem> items)
        {
            return new Inventory(inventory.ITEMS.Concat(items));
        }

        public static Inventory AddItems(this Inventory inventory, IEnumerable<ItemQuantity> itemQuantities)
        {
            List<GameItem> itemsToAdd = new List<GameItem>();

            foreach(ItemQuantity itemQuantity in itemQuantities)
            {
                for(int i = 0; i < itemQuantity.QUANTITY; i++)
                {
                    itemsToAdd.Add(ItemFactory.CreateGameItem(itemQuantity.ITEMID));
                }
            }

            return inventory.AddItems(itemsToAdd);
        }

        public static Inventory RemoveItem(this Inventory inventory, GameItem item)
        {
            return inventory.RemoveItems(new List<GameItem> { item });
        }

        public static Inventory RemoveItems(this Inventory inventory, IEnumerable<GameItem> items)
        {
            List<GameItem> workingInventory = inventory.ITEMS.ToList();
            IEnumerable<GameItem> itemsToRemove = items.ToList();

            foreach(GameItem item in itemsToRemove)
            {
                workingInventory.Remove(item);
            }

            return new Inventory(workingInventory);
        }

        public static Inventory RemoveItems(this Inventory inventory, IEnumerable<ItemQuantity> itemQuantities)
        {
            Inventory workingInventory = inventory;

            foreach(ItemQuantity itemQuantity in itemQuantities)
            {
                for(int i = 0; i < itemQuantity.QUANTITY; i++)
                {
                    workingInventory = workingInventory.RemoveItem(workingInventory.ITEMS.First(item => item.ITEMTYPEID == itemQuantity.ITEMID));
                }
            }

            return workingInventory;
        }

        public static List<GameItem> ItemsThatAre(this IEnumerable<GameItem> inventory, GameItem.ItemCategory category)
        {
            return inventory.Where(i => i.CATEGORY == category).ToList();
        }
    }
}
