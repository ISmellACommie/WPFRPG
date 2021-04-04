namespace Engine.Models
{
    public class GameItem
    {
        public enum ItemCategory
        {
            Miscellaneous,
            Weapon
        }

        public ItemCategory CATEGORY { get; }
        public int ITEMTYPEID { get; }
        public string NAME { get; }
        public int PRICE { get; }
        public bool ISUNIQUE { get; }
        public int MINDMG { get; }
        public int MAXDMG { get; }

        public GameItem(ItemCategory _category, int _itemtypeid, string _name, int _price, bool _isunique = false, int _mindmg = 0, int _maxdmg = 0)
        {
            CATEGORY = _category;
            ITEMTYPEID = _itemtypeid;
            NAME = _name;
            PRICE = _price;
            ISUNIQUE = _isunique;
            MINDMG = _mindmg;
            MAXDMG = _maxdmg;
        }
        public GameItem Clone()
        {
            return new GameItem(CATEGORY, ITEMTYPEID, NAME, PRICE, ISUNIQUE, MINDMG, MAXDMG);
        }
    }
}
