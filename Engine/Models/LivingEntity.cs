using System;
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
        private int _lvl;

        public string NAME
        {
            get { return _name; }
            private set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public int CURRENTHP
        {
            get { return _currenthp; }
            private set
            {
                _currenthp = value;
                OnPropertyChanged();
            }
        }
        public int MAXHP
        {
            get { return _maxhp; }
            protected set
            {
                _maxhp = value;
                OnPropertyChanged();
            }
        }
        public int GOLD
        {
            get { return _gold; }
            private set
            {
                _gold = value;
                OnPropertyChanged();
            }
        }
        public int LVL
        {
            get { return _lvl; }
            set
            {
                _lvl = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<GameItem> INV { get; }
        public ObservableCollection<GroupedInventoryItem> GROUPEDINV { get; }
        public List<GameItem> WEAPONS => INV.Where(i => i.CATEGORY == GameItem.ItemCategory.Weapon).ToList();
        public bool ISDEAD => CURRENTHP <= 0;

        public EventHandler OnKilled;

        protected LivingEntity(string _name, int _maxhp, int _currenthp, int _gold, int _lvl = 1)
        {
            NAME = _name;
            MAXHP = _maxhp;
            CURRENTHP = _currenthp;
            GOLD = _gold;
            LVL = _lvl;

            INV = new ObservableCollection<GameItem>();
            GROUPEDINV = new ObservableCollection<GroupedInventoryItem>();
        }

        public void TakeDamage(int _hpofdmg)
        {
            CURRENTHP -= _hpofdmg;

            if (ISDEAD)
            {
                CURRENTHP = 0;
                RaiseOnKilledEvent();
            }
        }

        public void Heal(int _hptoheal)
        {
            CURRENTHP += _hptoheal;

            if(CURRENTHP > MAXHP)
            {
                CURRENTHP = MAXHP;
            }
        }

        public void CompletelyHeal()
        {
            CURRENTHP = MAXHP;
        }

        public void ReceiveGold(int _goldamt)
        {
            GOLD += _goldamt;
        }

        public void SpendGold(int _goldamt)
        {
            if(_goldamt > GOLD)
            {
                throw new ArgumentOutOfRangeException($"{NAME} only has {GOLD} gold and cannot spend {_goldamt} gold.");
            }

            GOLD -= _goldamt;
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

            GroupedInventoryItem groupedInventoryItemToRemove = _item.ISUNIQUE ? GROUPEDINV.FirstOrDefault(gi => gi.ITEM == _item) : GROUPEDINV.FirstOrDefault(gi => gi.ITEM.ITEMTYPEID == _item.ITEMTYPEID);

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

        private void RaiseOnKilledEvent()
        {
            OnKilled?.Invoke(this, new System.EventArgs());
        }
    }
}
