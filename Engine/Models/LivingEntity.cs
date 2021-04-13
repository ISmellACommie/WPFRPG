using System;
using System.Collections.Generic;
using Engine.Services;

namespace Engine.Models
{
    public abstract class LivingEntity: BaseNotificationClass
    {
        private string _name;
        private int _currenthp;
        private int _maxhp;
        private int _gold;
        private int _lvl;
        private GameItem _currentWeapon;
        private GameItem _currentConsumable;
        private Inventory _inventory;

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
        public Inventory INV
        {
            get => _inventory;
            private set
            {
                _inventory = value;
                OnPropertyChanged();
            }
        }
        public GameItem CURRENTWEAPON
        {
            get { return _currentWeapon; }
            set
            {
                if(_currentWeapon != null)
                {
                    _currentWeapon.ACTION.ONACTIONPERFORMED -= RaiseActionPerformedEvent;
                }

                _currentWeapon = value;

                if(_currentWeapon != null)
                {
                    _currentWeapon.ACTION.ONACTIONPERFORMED += RaiseActionPerformedEvent;
                }

                OnPropertyChanged();
            }
        }
        public GameItem CURRENTCONSUMABLE
        {
            get => _currentConsumable;
            set
            {
                if(_currentConsumable != null)
                {
                    _currentConsumable.ACTION.ONACTIONPERFORMED -= RaiseActionPerformedEvent;
                }

                _currentConsumable = value;

                if(_currentConsumable != null)
                {
                    _currentConsumable.ACTION.ONACTIONPERFORMED += RaiseActionPerformedEvent;
                }

                OnPropertyChanged();
            }
        }

        public bool ISDEAD => CURRENTHP <= 0;

        public EventHandler ONKILLED;
        public EventHandler<string> ONACTIONPERFORMED;

        protected LivingEntity(string _name, int _maxhp, int _currenthp, int _gold, int _lvl = 1)
        {
            NAME = _name;
            MAXHP = _maxhp;
            CURRENTHP = _currenthp;
            GOLD = _gold;
            LVL = _lvl;
            INV = new Inventory();
        }

        public void UseCurrentWeaponOn(LivingEntity target)
        {
            CURRENTWEAPON.PerformAction(this, target);
        }

        public void UseCurrentConsumable()
        {
            CURRENTCONSUMABLE.PerformAction(this, this);
            RemoveItemFromInventory(CURRENTCONSUMABLE);
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
            INV = INV.AddItem(_item);
        }

        public void RemoveItemFromInventory(GameItem _item)
        {
            INV = INV.RemoveItem(_item);
        }

        public void RemoveItemsFromInventory(List<ItemQuantity> _itemquantities)
        {
            INV = INV.RemoveItems(_itemquantities);
        }

        private void RaiseOnKilledEvent()
        {
            ONKILLED?.Invoke(this, new System.EventArgs());
        }

        private void RaiseActionPerformedEvent(object sender, string result)
        {
            ONACTIONPERFORMED?.Invoke(this, result);
        }
    }
}
