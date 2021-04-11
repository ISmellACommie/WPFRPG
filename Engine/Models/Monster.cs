using System.Collections.Generic;
using Engine.Factories;

namespace Engine.Models
{
    public class Monster : LivingEntity
    {
        private readonly List<ItemPercentage> _lootTable = new List<ItemPercentage>();

        public int ID { get; }
        public string IMGNAME { get; }
        public int REWARDEXP { get; }

        public Monster(int _id, string _name, string _imgname, int _maxhp, GameItem _currentWeapon, int _rewardexp, int _gold) : base(_name, _maxhp, _maxhp, _gold)
        {
            ID = _id;
            IMGNAME = $"/Engine;component/Images/Monsters/{_imgname}";
            CURRENTWEAPON = _currentWeapon;
            REWARDEXP = _rewardexp;
        }

        public void AddItemToLootTable(int id, int percentage)
        {
            _lootTable.RemoveAll(ip => ip.ID == id);

            _lootTable.Add(new ItemPercentage(id, percentage));
        }

        public Monster GetNewInstance()
        {
            Monster newMonster = new Monster(ID, NAME, IMGNAME, MAXHP, CURRENTWEAPON, REWARDEXP, GOLD);

            foreach(ItemPercentage itemPercentage in _lootTable)
            {
                newMonster.AddItemToLootTable(itemPercentage.ID, itemPercentage.PERCENTAGE);

                if(RandomNumberGenerator.NumberBetween(1, 100) <= itemPercentage.PERCENTAGE)
                {
                    newMonster.AddItemToInventory(ItemFactory.CreateGameItem(itemPercentage.ID));
                }
            }

            return newMonster;
        }
    }
}
