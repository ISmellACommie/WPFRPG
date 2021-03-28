using System;
using Engine.Models;

namespace Engine.Factories
{
    public static class MonsterFactory
    {
        public static Monster GetMonster(int _monsterID)
        {
            switch (_monsterID)
            {
                case 1:
                    {
                        Monster snake = new Monster("Snake", "Snake.png", 4, 4, 5, 1);

                        AddLootItem(snake, 9001, 25);
                        AddLootItem(snake, 9002, 75);

                        return snake;
                    }

                case 2:
                    {
                        Monster rat = new Monster("Rat", "Rat.png", 5, 5, 5, 1);

                        AddLootItem(rat, 9003, 25);
                        AddLootItem(rat, 9004, 75);

                        return rat;
                    }

                case 3:
                    {
                        Monster giantSpider = new Monster("Giant Spider", "GiantSpider.png", 10, 10, 10, 3);

                        AddLootItem(giantSpider, 9005, 25);
                        AddLootItem(giantSpider, 9006, 75);

                        return giantSpider;
                    }

                default:
                    {
                        throw new AggregateException(string.Format("MonsterType '{0}' does not exist", _monsterID));
                    }
            }
        }
        private static void AddLootItem(Monster _monster, int _itemID, int _percentage)
        {
            if (RandomNumberGenerator.NumberBetween(1, 100) <= _percentage)
            {
                _monster.Inventory.Add(new ItemQuantity(_itemID, 1));
            }
        }
    }
}
