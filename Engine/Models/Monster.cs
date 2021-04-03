using System.Collections.ObjectModel;

namespace Engine.Models
{
    public class Monster : LivingEntity
    {
        public string IMGNAME { get; set; }
        public int MINDMG { get; set; }
        public int MAXDMG { get; set; }
        public int REWARDEXP { get; private set; }

        public Monster(string _name, string _imgname, int _maxhp, int _currhp, int _mindmg, int _maxdmg, int _rewardexp, int _gold) : base(_name, _maxhp, _currhp, _gold)
        {
            IMGNAME = $"/Engine;component/Images/Monsters/{_imgname}";
            MINDMG = _mindmg;
            MAXDMG = _maxdmg;
            REWARDEXP = _rewardexp;
        }
    }
}
