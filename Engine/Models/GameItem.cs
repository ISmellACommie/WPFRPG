using Engine.Actions;

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
        public IAction ACTION { get; set; }

        public GameItem(ItemCategory _category, int _itemtypeid, string _name, int _price, bool _isunique = false, IAction _action = null)
        {
            CATEGORY = _category;
            ITEMTYPEID = _itemtypeid;
            NAME = _name;
            PRICE = _price;
            ISUNIQUE = _isunique;
            ACTION = _action;
        }

        public void PerformAction(LivingEntity actor, LivingEntity target)
        {
            ACTION?.Execute(actor, target);
        }

        public GameItem Clone()
        {
            return new GameItem(CATEGORY, ITEMTYPEID, NAME, PRICE, ISUNIQUE, ACTION);
        }
    }
}
