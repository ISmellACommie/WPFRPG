using System;
using Engine.Models;

namespace Engine.Actions
{
    public class Heal : IAction
    {
        private readonly GameItem item;
        private readonly int hptoheal;

        public event EventHandler<string> ONACTIONPERFORMED;

        public Heal(GameItem _item, int _hptoheal)
        {
            if(item.CATEGORY != GameItem.ItemCategory.Consumable)
            {
                throw new ArgumentException($"{_item.NAME} is not a consumable");
            }

            item = _item;
            hptoheal = _hptoheal;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorname = (actor is Player) ? "You" : $"The {actor.NAME.ToLower()}";
            string targetname = (target is Player) ? "yourself" : $"the {target.NAME.ToLower()}";

            ReportResult($"{actorname} heal {targetname} for {hptoheal} point{(hptoheal > 1 ? "s" : "")}.");
            target.Heal(hptoheal);
        }

        private void ReportResult(string result)
        {
            ONACTIONPERFORMED?.Invoke(this, result);
        }
    }
}
