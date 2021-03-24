
namespace Engine.Models
{
    public class GameItem
    {
        public int ITEMTYPEID
        {
            get;
            set;
        }
        public string NAME
        {
            get;
            set;
        }
        public int PRICE
        {
            get;
            set;
        }

        public GameItem(int _itemtypeid, string _name, int _price)
        {
            ITEMTYPEID = _itemtypeid;
            NAME = _name;
            PRICE = _price;
        }
        public GameItem Clone()
        {
            return new GameItem(ITEMTYPEID, NAME, PRICE);
        }
    }
}
