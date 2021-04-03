using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Models
{
    public class Player : LivingEntity
    {
        private string _charclass;
        private int _exp;
        private int _lvl;

        public string CHARCLASS
        {
            get { return _charclass; }
            set
            {
                _charclass = value;
                OnPropertyChanged(nameof(CHARCLASS));
            }
        }
        public int EXP
        {
            get { return _exp; }
            set
            {
                _exp = value;
                OnPropertyChanged(nameof(EXP));
            }
        }
        public int LVL
        {
            get { return _lvl; }
            set
            {
                _lvl = value;
                OnPropertyChanged(nameof(LVL));
            }
        }

        public ObservableCollection<QuestStatus> QUESTS { get; set; }
        
        public Player(string _name, string _charclass, int _exp, int _maxhp, int _currhp, int _gold) : base(_name, _maxhp, _currhp, _gold)
        {
            CHARCLASS = _charclass;
            EXP = _exp;

            QUESTS = new ObservableCollection<QuestStatus>();
        }

        public bool HasAllTheseItems(List<ItemQuantity> items)
        {
            foreach(ItemQuantity item in items)
            {
                if(INV.Count(i => i.ITEMTYPEID == item.ITEMID) < item.QUANTITY)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
