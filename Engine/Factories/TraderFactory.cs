using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Models;

namespace Engine.Factories
{
    public static class TraderFactory
    {
        private static readonly List<Trader> _traders = new List<Trader>();

        static TraderFactory()
        {
            Trader susan = new Trader("Susan");
            susan.AddItemToInventory(ItemFactory.CreateGameItem(1001));

            Trader farmerted = new Trader("Farmer Ted");
            farmerted.AddItemToInventory(ItemFactory.CreateGameItem(1001));

            Trader petetheherbalist = new Trader("Pete the Herbalist");
            petetheherbalist.AddItemToInventory(ItemFactory.CreateGameItem(1001));

            AddTraderToList(susan);
            AddTraderToList(farmerted);
            AddTraderToList(petetheherbalist);
        }

        public static Trader GetTraderByName(string _name)
        {
            return _traders.FirstOrDefault(t => t.NAME == _name);
        }

        private static void AddTraderToList(Trader _trader)
        {
            if(_traders.Any(t => t.NAME == _trader.NAME))
            {
                throw new ArgumentException($"There is already a trader named '{_trader.NAME}'.");
            }

            _traders.Add(_trader);
        }
    }
}
