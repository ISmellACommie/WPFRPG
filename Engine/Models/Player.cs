using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Models
{
    public class Player : LivingEntity
    {
        private string _charclass;
        private int _exp;

        public string CHARCLASS
        {
            get { return _charclass; }
            set
            {
                _charclass = value;
                OnPropertyChanged();
            }
        }
        public int EXP
        {
            get { return _exp; }
            private set
            {
                _exp = value;
                OnPropertyChanged();
                SetLevelAndMaximumHitPoints();
            }
        }

        public ObservableCollection<QuestStatus> QUESTS { get; }
        public ObservableCollection<Recipe> RECIPES { get; }

        public EventHandler ONLEVELEDUP;
        
        public Player(string _name, string _charclass, int _exp, int _maxhp, int _currhp, int _gold) : base(_name, _maxhp, _currhp, _gold)
        {
            CHARCLASS = _charclass;
            EXP = _exp;

            QUESTS = new ObservableCollection<QuestStatus>();
            RECIPES = new ObservableCollection<Recipe>();
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

        public void AddExperience(int _exp)
        {
            EXP = _exp;
        }

        public void LearnRecipe(Recipe _recipe)
        {
            if(!RECIPES.Any(r => r.ID == _recipe.ID))
            {
                RECIPES.Add(_recipe);
            }
        }

        private void SetLevelAndMaximumHitPoints()
        {
            int originalLevel = LVL;

            LVL = (EXP / 100) + 1;

            if(LVL != originalLevel)
            {
                MAXHP = LVL * 10;
                ONLEVELEDUP?.Invoke(this, System.EventArgs.Empty);
            }
        }
    }
}
