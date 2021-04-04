using System.Collections.Generic;
using System.Linq;
using Engine.Models;
using Engine.Actions;

namespace Engine.Factories
{
    public static class ItemFactory
    {
        private static readonly List<GameItem> _standardgameitems = new List<GameItem>();

        static ItemFactory()
        {
            BuildWeapon(1001, "Pointy Stick", 1, 1, 2);
            BuildWeapon(1002, "Rusty Sword", 5, 1, 3);

            BuildWeapon(1501, "Snake fangs", 0, 0, 2);
            BuildWeapon(1502, "Rat claws", 0, 0, 2);
            BuildWeapon(1503, "Spider fangs", 0, 0, 4);

            BuildMiscellaneousItem(9001, "Snake fang", 1);
            BuildMiscellaneousItem(9002, "Snakeskin", 2);
            BuildMiscellaneousItem(9003, "Rat tail", 1);
            BuildMiscellaneousItem(9004, "Rat fur", 2);
            BuildMiscellaneousItem(9005, "Spider fang", 1);
            BuildMiscellaneousItem(9006, "Spider silk", 2);
        }

        public static GameItem CreateGameItem(int _itemtypeid)
        {
            return _standardgameitems.FirstOrDefault(item => item.ITEMTYPEID == _itemtypeid)?.Clone();
        }

        private static void BuildMiscellaneousItem(int _id, string _name, int _price)
        {
            _standardgameitems.Add(new GameItem(GameItem.ItemCategory.Miscellaneous, _id, _name, _price));
        }

        private static void BuildWeapon(int _id, string _name, int _price, int _mindmg, int _maxdmg)
        {
            GameItem weapon = new GameItem(GameItem.ItemCategory.Weapon, _id, _name, _price, true);
            weapon.ACTION = new AttackWithWeapon(weapon, _mindmg, _maxdmg);
            _standardgameitems.Add(weapon);
        }
    }
}
