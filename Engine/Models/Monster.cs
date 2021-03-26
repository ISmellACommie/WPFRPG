using System.Collections.ObjectModel;

namespace Engine.Models
{
    public class Monster : BaseNotificationClass
    {
        private int _hp;

        public string NAME
        {
            get;
            private set;
        }
        public string IMGNAME
        {
            get;
            set;
        }
        public int MAXHP
        {
            get;
            private set;
        }
        public int HP
        {
            get
            {
                return _hp;
            }
            set
            {
                _hp = value;
                OnPropertyChanged(nameof(HP));
            }
        }

        public int REWARDEXP
        {
            get;
            private set;
        }
        public int REWARDGOLD
        {
            get;
            private set;
        }

        public ObservableCollection<ItemQuantity> Inventory
        {
            get;
            set;
        }

        public Monster(string _name, string _imgname, int _maxhp, int _hp, int _rewardexp, int _rewardgold)
        {
            NAME = _name;
            IMGNAME = string.Format("/Engine;component/Images/Monsters/{0}", _imgname);
            MAXHP = _maxhp;
            HP = _hp;
            REWARDEXP = _rewardexp;
            REWARDGOLD = _rewardgold;
        }
    }
}
