namespace Engine.Models
{
    public class GameItem
    {
        public int ITEMTYPEID { get; }
        public string NAME { get; }
        public int PRICE { get; }
        public bool ISUNIQUE { get; }

        public GameItem(int _itemtypeid, string _name, int _price, bool _isunique = false)
        {
            ITEMTYPEID = _itemtypeid;
            NAME = _name;
            PRICE = _price;
            ISUNIQUE = _isunique;
        }
        public GameItem Clone()
        {
            return new GameItem(ITEMTYPEID, NAME, PRICE, ISUNIQUE);
        }
    }
}
