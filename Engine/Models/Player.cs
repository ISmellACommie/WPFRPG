using System.Collections.ObjectModel;

namespace Engine.Models
{
    public class Player : BaseNotificationClass
    {
        private string _name;
        private string _charclass;
        private int _hp;
        private int _exp;
        private int _lvl;
        private int _gold;

        public string NAME
        {
            get 
            { 
                return _name; 
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(NAME));
            }
        }
        public string CHARCLASS
        {
            get
            {
                return _charclass;
            }
            set
            {
                _charclass = value;
                OnPropertyChanged(nameof(CHARCLASS));
            }
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
        public int EXP
        {
            get
            {
                return _exp;
            }
            set
            {
                _exp = value;
                OnPropertyChanged(nameof(EXP));
            }
        }
        public int LVL
        {
            get
            {
                return _lvl;
            }
            set
            {
                _lvl = value;
                OnPropertyChanged(nameof(LVL));
            }
        }
        public int GOLD
        {
            get
            {
                return _gold;
            }
            set
            {
                _gold = value;
                OnPropertyChanged(nameof(GOLD));
            }
        }
        public ObservableCollection<GameItem> INV
        {
            get;
            set;
        }
        public ObservableCollection<QuestStatus> QUESTS
        {
            get;
            set;
        }
        
        public Player()
        {
            INV = new ObservableCollection<GameItem>();
            QUESTS = new ObservableCollection<QuestStatus>();
        }
    }
}
