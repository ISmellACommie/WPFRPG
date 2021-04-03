namespace Engine.Models
{
    public class Monster : LivingEntity
    {
        public string IMGNAME { get; }
        public int MINDMG { get; }
        public int MAXDMG { get;}
        public int REWARDEXP { get; }

        public Monster(string _name, string _imgname, int _maxhp, int _currhp, int _mindmg, int _maxdmg, int _rewardexp, int _gold) : base(_name, _maxhp, _currhp, _gold)
        {
            IMGNAME = $"/Engine;component/Images/Monsters/{_imgname}";
            MINDMG = _mindmg;
            MAXDMG = _maxdmg;
            REWARDEXP = _rewardexp;
        }
    }
}
