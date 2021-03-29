
namespace Engine.Models
{
    public class Weapon : GameItem
    {
        public int MINDMG { get; set; }
        public int MAXDMG { get; set; }

        public Weapon(int _itemtypeid, string _name, int _price, int _mindmg, int _maxdmg) : base(_itemtypeid, _name, _price)
        {
            MINDMG = _mindmg;
            MAXDMG = _maxdmg;
        }
        public new Weapon Clone()
        {
            return new Weapon(ITEMTYPEID, NAME, PRICE, MINDMG, MAXDMG);
        }
    }
}
