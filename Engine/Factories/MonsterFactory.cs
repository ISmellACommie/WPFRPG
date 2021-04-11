using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Engine.Models;
using Engine.Shared;

namespace Engine.Factories
{
    public static class MonsterFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Monsters.xml";
        private static readonly List<Monster> _baseMonsters = new List<Monster>();

        static MonsterFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                string rootImagePath =
                    data.SelectSingleNode("/Monsters")
                        .AttributeAsString("RootImagePath");

                LoadMonstersFromNodes(data.SelectNodes("/Monsters/Mosnter"), rootImagePath);
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }

        private static void LoadMonstersFromNodes(XmlNodeList nodes, string rootImagePath)
        {
            if(nodes == null)
            {
                return;
            }

            foreach(XmlNode node in nodes)
            {
                Monster monster =
                    new Monster(node.AttributeAsInt("ID"),
                                 node.AttributeAsString("NAME"),
                                 $".{rootImagePath}{node.AttributeAsString("IMGNAME")}",
                                 node.AttributeAsInt("MAXHP"),
                                 ItemFactory.CreateGameItem(node.AttributeAsInt("WEAPONID")),
                                 node.AttributeAsInt("REWARDEXP"),
                                 node.AttributeAsInt("GOLD"));

                XmlNodeList lootItemNodes = node.SelectNodes("./LootItems/LootItem");
                if(lootItemNodes != null)
                {
                    foreach(XmlNode lootItemNode in lootItemNodes)
                    {
                        monster.AddItemToLootTable(lootItemNode.AttributeAsInt("ID"),
                                                   lootItemNode.AttributeAsInt("PERCENTAGE"));
                    }
                }

                _baseMonsters.Add(monster);
            }
        }

        public static Monster GetMonster(int _id)
        {
            return _baseMonsters.FirstOrDefault(m => m.ID == _id)?.GetNewInstance();
        }
    }
}
