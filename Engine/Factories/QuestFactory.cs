using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    internal static class QuestFactory
    {
        private static readonly List<Quest> _quests = new List<Quest>();

        static QuestFactory()
        {
            //declare the items needed to complete the quest and reward items
            List<ItemQuantity> itemstocomplete = new List<ItemQuantity>();
            List<ItemQuantity> rewarditems = new List<ItemQuantity>();

            itemstocomplete.Add(new ItemQuantity(9001, 5));
            rewarditems.Add(new ItemQuantity(1002, 1));
            //create the quest
            _quests.Add(new Quest(1,
                "Clear the herb garden",
                "Defeat the snakes in the Herbalist's garden",
                itemstocomplete,
                25, 10,
                rewarditems));
        }
        internal static Quest GetQuestByID(int id)
        {
            return _quests.FirstOrDefault(Quest => Quest.ID == id);
        }
    }
}
