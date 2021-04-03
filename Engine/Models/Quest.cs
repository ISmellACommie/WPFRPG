using System.Collections.Generic;

namespace Engine.Models
{
    public class Quest
    {
        public int ID { get; }
        public string NAME { get; }
        public string DESC { get; }
        public List<ItemQuantity> ItemsToComplete { get; }

        public int REWARDEXP { get; }
        public int REWARDGOLD { get; }
        public List<ItemQuantity> RewardItems { get; }

        public Quest(int _id, string _name, string _desc, List<ItemQuantity> _itemstocomplete, int _rewardexp, int _rewardgold, List<ItemQuantity> _rewarditems)
        {
            ID = _id;
            NAME = _name;
            DESC = _desc;
            ItemsToComplete = _itemstocomplete;
            REWARDEXP = _rewardexp;
            REWARDGOLD = _rewardgold;
            RewardItems = _rewarditems;
        }
    }
}
