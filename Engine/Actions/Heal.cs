using System;
using Engine.Models;

namespace Engine.Actions
{
    public class Heal : BaseAction, IAction
    {
        private readonly int hptoheal;

        public Heal(GameItem _itemInUse, int _hptoheal) : base(_itemInUse)
        {
            if(_itemInUse.CATEGORY != GameItem.ItemCategory.Consumable)
            {
                throw new ArgumentException($"{_itemInUse.NAME} is not a consumable");
            }

            hptoheal = _hptoheal;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorname = (actor is Player) ? "You" : $"The {actor.NAME.ToLower()}";
            string targetname = (target is Player) ? "yourself" : $"the {target.NAME.ToLower()}";

            ReportResult($"{actorname} heal {targetname} for {hptoheal} point{(hptoheal > 1 ? "s" : "")}.");
            target.Heal(hptoheal);
        }
    }
}
