using System.Collections.ObjectModel;

namespace Engine.Models
{
    public class Trader : BaseNotificationClass
    {
        public string NAME { get; set; }
        public ObservableCollection<GameItem> INV { get; set; }

        public Trader(string _name)
        {
            NAME = _name;
            INV = new ObservableCollection<GameItem>();
        }

        public void AddItemToInventory(GameItem _item)
        {
            INV.Add(_item);
        }

        public void RemoveItemFromInventory(GameItem _item)
        {
            INV.Remove(_item);
        }
    }
}
