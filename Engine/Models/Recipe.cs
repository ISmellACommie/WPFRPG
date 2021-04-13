using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Models
{
    public class Recipe
    {
        public int ID { get; }
        public string NAME { get; }
        public List<ItemQuantity> INGREDIENTS { get; } = new List<ItemQuantity>();
        public List<ItemQuantity> OUTPUTITEMS { get; } = new List<ItemQuantity>();
        public string TOOLTIPCONTENTS =>
            "Ingredients" + Environment.NewLine +
            "===========" + Environment.NewLine +
            string.Join(Environment.NewLine, INGREDIENTS.Select(i => i.QUANTITYITEMDESCRIPTION)) +
            Environment.NewLine + Environment.NewLine +
            "Creates" + Environment.NewLine +
            "===========" + Environment.NewLine +
            string.Join(Environment.NewLine, OUTPUTITEMS.Select(i => i.QUANTITYITEMDESCRIPTION));

        public Recipe(int _id, string _name)
        {
            ID = _id;
            NAME = _name;
        }

        public void AddIngredient(int _itemID, int _quantity)
        {
            if(!INGREDIENTS.Any(x => x.ITEMID == _itemID))
            {
                INGREDIENTS.Add(new ItemQuantity(_itemID, _quantity));
            }
        }

        public void AddOutputItem(int _itemID, int _quantity)
        {
            if(!OUTPUTITEMS.Any(x => x.ITEMID == _itemID))
            {
                OUTPUTITEMS.Add(new ItemQuantity(_itemID, _quantity));
            }
        }
    }
}
