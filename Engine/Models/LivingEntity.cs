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
        public List<GameItem> WEAPONS => INV.Where(i => i is Weapon).ToList();

        protected LivingEntity()
        {
            INV = new ObservableCollection<GameItem>();
        }

        public void AddItemToInventory(GameItem _item)
        {
            INV.Add(_item);
            OnPropertyChanged(nameof(WEAPONS));
        }

        public void RemoveItemFromInventory(GameItem _item)
        {
            INV.Remove(_item);
            OnPropertyChanged(nameof(WEAPONS));
        }
    }
}
