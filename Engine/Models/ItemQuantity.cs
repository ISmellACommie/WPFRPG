
namespace Engine.Models
{
    public class ItemQuantity
    {
        public int ITEMID
        {
            get;
            set;
        }
        public int QUANTITY
        {
            get;
            set;
        }
        public ItemQuantity(int _itemid, int _quantity)
        {
            ITEMID = _itemid;
            QUANTITY = _quantity;
        }
    }
}
