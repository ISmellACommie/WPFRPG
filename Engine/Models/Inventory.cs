using System.Collections.Generic;
using System.Linq;
using Engine.Services;

namespace Engine.Models
{
    public class Inventory
    {
        private readonly List<GameItem> _backingInventory = new List<GameItem>();
        private readonly List<GroupedInventoryItem> _backingGroupedInventoryItems = new List<GroupedInventoryItem>();

        public IReadOnlyList<GameItem> ITEMS => _backingInventory.AsReadOnly();
        public IReadOnlyList<GroupedInventoryItem> GROUPEDINV => _backingGroupedInventoryItems.AsReadOnly();
        public IReadOnlyList<GameItem> WEAPONS => _backingInventory.ItemsThatAre(GameItem.ItemCategory.Weapon).AsReadOnly();
        public IReadOnlyList<GameItem> CONSUMABLES => _backingInventory.ItemsThatAre(GameItem.ItemCategory.Consumable).AsReadOnly();
        public bool HASCONSUMABLE => CONSUMABLES.Any();

        public Inventory(IEnumerable<GameItem> items = null)
        {
            if(items == null)
            {
                return;
            }

            foreach(GameItem item in items)
            {
                _backingInventory.Add(item);
                AddItemToGroupedInventory(item);
            }
        }

        public bool HasAllTheseItems(IEnumerable<ItemQuantity> items)
        {
            return items.All(item => items.Count(i => i.ITEMID == item.ITEMID) >= item.QUANTITY);
        }

        public void AddItemToGroupedInventory(GameItem item)
        {
            if (item.ISUNIQUE)
            {
                _backingGroupedInventoryItems.Add(new GroupedInventoryItem(item, 1));
            }
            else
            {
                if(_backingGroupedInventoryItems.All(gi => gi.ITEM.ITEMTYPEID != item.ITEMTYPEID))
                {
                    _backingGroupedInventoryItems.Add(new GroupedInventoryItem(item, 0));
                }

                _backingGroupedInventoryItems.First(gi => gi.ITEM.ITEMTYPEID == item.ITEMTYPEID).QUANTITY++;
            }
        }
    }
}
