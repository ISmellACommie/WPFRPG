﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Factories;
using Engine.Models;
using Engine.Services;

namespace TestEngine.Models
{
    [TestClass]
    public class TestInventory
    {
        [TestMethod]
        public void Test_Instantiate()
        {
            Inventory inventory = new Inventory();
            Assert.AreEqual(0, inventory.ITEMS.Count);
        }

        [TestMethod]
        public void Test_AddItem()
        {
            Inventory inventory = new Inventory();
            Inventory inventory1 = inventory.AddItemFromFactory(3001);
            Assert.AreEqual(1, inventory1.ITEMS.Count);
        }

        [TestMethod]
        public void Test_AddItems()
        {
            Inventory inventory = new Inventory();
            List<GameItem> itemsToAdd = new List<GameItem>();
            itemsToAdd.Add(ItemFactory.CreateGameItem(3001));
            itemsToAdd.Add(ItemFactory.CreateGameItem(3002));

            Inventory inventory1 = inventory.AddItems(itemsToAdd);
            Assert.AreEqual(2, inventory1.ITEMS.Count);

            Inventory inventory2 = inventory1.AddItemFromFactory(3001).AddItemFromFactory(3002);
            Assert.AreEqual(4, inventory2.ITEMS.Count);
        }

        [TestMethod]
        public void Test_AddItemQuantities()
        {
            Inventory inventory = new Inventory();
            Inventory inventory1 = inventory.AddItems(new List<ItemQuantity> { new ItemQuantity(1001, 3) });
            Assert.AreEqual(3, inventory1.ITEMS.Count(i => i.ITEMTYPEID == 1001));

            Inventory inventory2 = inventory1.AddItemFromFactory(1001);
            Assert.AreEqual(4, inventory2.ITEMS.Count(i => i.ITEMTYPEID == 1001));

            Inventory inventory3 = inventory2.AddItems(new List<ItemQuantity> { new ItemQuantity(1002, 1) });
            Assert.AreEqual(4, inventory3.ITEMS.Count(i => i.ITEMTYPEID == 1001));
            Assert.AreEqual(1, inventory3.ITEMS.Count(i => i.ITEMTYPEID == 1002));
        }

        [TestMethod]
        public void Test_RemoveItem()
        {
            Inventory inventory = new Inventory();
            GameItem item1 = ItemFactory.CreateGameItem(3001);
            GameItem item2 = ItemFactory.CreateGameItem(3002);
            Inventory inventory1 = inventory.AddItems(new List<GameItem> { item1, item2 });
            Inventory inventory2 = inventory1.RemoveItem(item1);
            Assert.AreEqual(1, inventory2.ITEMS.Count);
        }

        [TestMethod]
        public void Test_RemoveItems()
        {
            Inventory inventory = new Inventory();
            GameItem item1 = ItemFactory.CreateGameItem(3001);
            GameItem item2 = ItemFactory.CreateGameItem(3002);
            GameItem item3 = ItemFactory.CreateGameItem(3003);
            Inventory inventory1 = inventory.AddItems(new List<GameItem> { item1, item2, item3 });
            Inventory inventory2 = inventory1.RemoveItems(new List<GameItem> { item2, item3 });
            Assert.AreEqual(1, inventory2.ITEMS.Count);
        }

        [TestMethod]
        public void Test_CategorisedItemProperties()
        {
            Inventory inventory = new Inventory();
            Assert.AreEqual(0, inventory.WEAPONS.Count);
            Assert.AreEqual(0, inventory.CONSUMABLES.Count);

            Inventory inventory1 = inventory.AddItemFromFactory(1001);
            Assert.AreEqual(1, inventory1.WEAPONS.Count);
            Assert.AreEqual(0, inventory1.CONSUMABLES.Count);

            Inventory inventory2 = inventory1.AddItemFromFactory(3001);
            Assert.AreEqual(1, inventory2.WEAPONS.Count);
            Assert.AreEqual(0, inventory2.CONSUMABLES.Count);

            Inventory inventory3 = inventory2.AddItemFromFactory(1002);
            Assert.AreEqual(2, inventory3.WEAPONS.Count);
            Assert.AreEqual(0, inventory3.CONSUMABLES.Count);

            Inventory inventory4 = inventory.AddItemFromFactory(2001);
            Assert.AreEqual(2, inventory4.WEAPONS.Count);
            Assert.AreEqual(1, inventory4.CONSUMABLES.Count);
        }

        [TestMethod]
        public void Test_RemoveItemQuantities()
        {
            Inventory inventory = new Inventory();
            Assert.AreEqual(0, inventory.WEAPONS.Count);
            Assert.AreEqual(0, inventory.CONSUMABLES.Count);

            Inventory inventory1 =
                inventory
                    .AddItemFromFactory(1001)
                    .AddItemFromFactory(1002)
                    .AddItemFromFactory(1002)
                    .AddItemFromFactory(1002)
                    .AddItemFromFactory(1002)
                    .AddItemFromFactory(3001)
                    .AddItemFromFactory(3001);
            Assert.AreEqual(1, inventory1.ITEMS.Count(i => i.ITEMTYPEID == 1001));
            Assert.AreEqual(4, inventory1.ITEMS.Count(i => i.ITEMTYPEID == 1002));
            Assert.AreEqual(2, inventory1.ITEMS.Count(i => i.ITEMTYPEID == 3001));

            Inventory inventory2 = inventory1.RemoveItems(new List<ItemQuantity> { new ItemQuantity(1001, 2) });
            Assert.AreEqual(1, inventory1.ITEMS.Count(i => i.ITEMTYPEID == 1001));
            Assert.AreEqual(2, inventory1.ITEMS.Count(i => i.ITEMTYPEID == 1002));
            Assert.AreEqual(2, inventory1.ITEMS.Count(i => i.ITEMTYPEID == 3001));

            Inventory inventory3 = inventory2.RemoveItems(new List<ItemQuantity> { new ItemQuantity(1002, 1) });
            Assert.AreEqual(1, inventory1.ITEMS.Count(i => i.ITEMTYPEID == 1001));
            Assert.AreEqual(1, inventory1.ITEMS.Count(i => i.ITEMTYPEID == 1002));
            Assert.AreEqual(2, inventory1.ITEMS.Count(i => i.ITEMTYPEID == 3001));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_RemoveItemQuantities_RemoveTooMany()
        {
            Inventory inventory = new Inventory();
            Assert.AreEqual(0, inventory.WEAPONS.Count);
            Assert.AreEqual(0, inventory.CONSUMABLES.Count);

            Inventory inventory1 =
                inventory
                    .AddItemFromFactory(1001)
                    .AddItemFromFactory(1002)
                    .AddItemFromFactory(1002)
                    .AddItemFromFactory(1002)
                    .AddItemFromFactory(3001)
                    .AddItemFromFactory(3001);
            Assert.AreEqual(1, inventory1.ITEMS.Count(i => i.ITEMTYPEID == 1001));
            Assert.AreEqual(4, inventory1.ITEMS.Count(i => i.ITEMTYPEID == 1002));
            Assert.AreEqual(2, inventory1.ITEMS.Count(i => i.ITEMTYPEID == 3001));

            Inventory inventory2 =
                inventory
                    .RemoveItems(new List<ItemQuantity> { new ItemQuantity(1002, 999) });
        }
    }
}
