
namespace Engine.Models
{
    public class GroupedInventoryItem : BaseNotificationClass
    {
        private GameItem _item;
        private int _quantity;

        public GameItem ITEM
        {
            get { return _item; }
            set
            {
                _item = value;
                OnPropertyChanged(nameof(ITEM));
            }
        }
        public int QUANTITY
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(QUANTITY));
            }
        }

        public GroupedInventoryItem(GameItem _item, int _quantity)
        {
            ITEM = _item;
            QUANTITY = _quantity;
        }
    }
}
