using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.Models
{
    public abstract class LivingEntity: BaseNotificationClass
    {
        private string _name;
        private int _currenthp;
        private int _maxhp;
        private int _gold;

        public string NAME
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(NAME));
            }
        }
        public int CURRENTHP
        {
            get { return _currenthp; }
            set
            {
                _currenthp = value;
                OnPropertyChanged(nameof(CURRENTHP));
            }
        }
        public int MAXHP
        {
            get { return _maxhp; }
            set
            {
                _maxhp = value;
                OnPropertyChanged(nameof(MAXHP));
            }
        }
        public int GOLD
        {
            get { return _gold; }
            set
            {
                _gold = value;
                OnPropertyChanged(nameof(GOLD));
            }
        }
        public ObservableCollection<GameItem> INV { get; set; }
        public ObservableCollection<GroupedInventoryItem> GROUPEDINV { get; set; }
        public List<GameItem> WEAPONS => INV.Where(i => i is Weapon).ToList();

        protected LivingEntity()
        {
            INV = new ObservableCollection<GameItem>();
            GROUPEDINV = new ObservableCollection<GroupedInventoryItem>();
        }

        public void AddItemToInventory(GameItem _item)
        {
            INV.Add(_item);

            if (_item.ISUNIQUE)
            {
                GROUPEDINV.Add(new GroupedInventoryItem(_item, 1));
            }
            else
            {
                if(!GROUPEDINV.Any(gi => gi.ITEM.ITEMTYPEID == _item.ITEMTYPEID))
                {
                    GROUPEDINV.Add(new GroupedInventoryItem(_item, 0));
                }

                GROUPEDINV.First(gi => gi.ITEM.ITEMTYPEID == _item.ITEMTYPEID).QUANTITY++;
            }

            OnPropertyChanged(nameof(WEAPONS));
        }

        public void RemoveItemFromInventory(GameItem _item)
        {
            INV.Remove(_item);

            GroupedInventoryItem groupedInventoryItemToRemove = GROUPEDINV.FirstOrDefault(gi => gi.ITEM == _item);

            if(groupedInventoryItemToRemove != null)
            {
                if(groupedInventoryItemToRemove.QUANTITY == 1)
                {
                    GROUPEDINV.Remove(groupedInventoryItemToRemove);
                }
                else
                {
                    groupedInventoryItemToRemove.QUANTITY--;
                }
            }

            OnPropertyChanged(nameof(WEAPONS));
        }
    }
}
