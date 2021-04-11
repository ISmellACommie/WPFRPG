using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using Engine.Models;
using Engine.Actions;

namespace Engine.Factories
{
    public static class ItemFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\GameItems.xml";
        private static readonly List<GameItem> _standardgameitems = new List<GameItem>();

        static ItemFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                LoadItemsFromNodes(data.SelectNodes("/GameItems/Weapons/Weapon"));
                LoadItemsFromNodes(data.SelectNodes("/GameItems/HealingItems/HealingItem"));
                LoadItemsFromNodes(data.SelectNodes("/GameItems/MiscellaneousItems/MiscellaneousItem"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
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

        private static void BuildHealingItem(int _id, string _name, int _price, int _hptoheal)
        {
            GameItem item = new GameItem(GameItem.ItemCategory.Consumable, _id, _name, _price);
            item.ACTION = new Heal(item, _hptoheal);
            _standardgameitems.Add(item);
        }

        public static string ItemName(int _itemtypeid)
        {
            return _standardgameitems.FirstOrDefault(i => i.ITEMTYPEID == _itemtypeid)?.NAME ?? "";
        }

        private static void LoadItemsFromNodes(XmlNodeList nodes)
        {
            if(nodes == null)
            {
                return;
            }

            foreach(XmlNode node in nodes)
            {
                GameItem.ItemCategory itemCategory = DetermineItemCategory(node.Name);

                GameItem gameItem =
                    new GameItem(itemCategory,
                                 GetXmlAttributeAsInt(node, "ID"),
                                 GetXmlAttributeAsString(node, "NAME"),
                                 GetXmlAttributeAsInt(node, "PRICE"),
                                 itemCategory == GameItem.ItemCategory.Weapon);

                if(itemCategory == GameItem.ItemCategory.Weapon)
                {
                    gameItem.ACTION =
                        new AttackWithWeapon(gameItem,
                                             GetXmlAttributeAsInt(node, "MINDMG"),
                                             GetXmlAttributeAsInt(node, "MAXDMG"));
                }
                else if(itemCategory == GameItem.ItemCategory.Consumable)
                {
                    gameItem.ACTION =
                        new Heal(gameItem,
                                 GetXmlAttributeAsInt(node, "HPTOHEAL"));
                }

                _standardgameitems.Add(gameItem);
            }
        }

        private static GameItem.ItemCategory DetermineItemCategory(string itemType)
        {
            switch (itemType)
            {
                case "Weapon":
                    {
                        return GameItem.ItemCategory.Weapon;
                    }
                case "HealingItem":
                    {
                        return GameItem.ItemCategory.Consumable;
                    }
                default:
                    {
                        return GameItem.ItemCategory.Miscellaneous;
                    }
            }
        }

        private static int GetXmlAttributeAsInt(XmlNode node, string attributeName)
        {
            return Convert.ToInt32(GetXmlAttribute(node, attributeName));
        }

        private static string GetXmlAttributeAsString(XmlNode node, string attributeName)
        {
            return GetXmlAttribute(node, attributeName);
        }

        private static string GetXmlAttribute(XmlNode node, string attributeName)
        {
            XmlAttribute attribute = node.Attributes?[attributeName];

            if(attribute == null)
            {
                throw new ArgumentException($"The attribute '{attributeName}' does not exist");
            }

            return attribute.Value;
        }
    }
}
