using Engine.Factories;

namespace Engine.Models
{
    public class ItemQuantity
    {
        public int ITEMID { get; }
        public int QUANTITY { get; }
        public string QUANTITYITEMDESCRIPTION => $"{QUANTITY} {ItemFactory.ItemName(ITEMID)}";
        public ItemQuantity(int _itemid, int _quantity)
        {
            ITEMID = _itemid;
            QUANTITY = _quantity;
        }
    }
}
