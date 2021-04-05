using System;
using Engine.Models;

namespace Engine.Actions
{
    public class AttackWithWeapon : BaseAction, IAction
    {
        private readonly int mindmg;
        private readonly int maxdmg;

        public AttackWithWeapon(GameItem _itemInUse, int _mindmg, int _maxdmg) : base(_itemInUse)
        {
            if(_itemInUse.CATEGORY != GameItem.ItemCategory.Weapon)
            {
                throw new ArgumentException($"{_itemInUse.NAME} is not a weapon");
            }

            if(_mindmg < 0)
            {
                throw new ArgumentException("mindmg must be >= 0");
            }

            if(_maxdmg < _mindmg)
            {
                throw new ArgumentException("max dmg must be > mindmg");
            }

            mindmg = _mindmg;
            maxdmg = _maxdmg;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            int damage = RandomNumberGenerator.NumberBetween(mindmg, maxdmg);

            string actorname = (actor is Player) ? "You" : $"The {actor.NAME.ToLower()}";
            string targetname = (target is Player) ? "you" : $"the {target.NAME.ToLower()}";

            if(damage == 0)
            {
                ReportResult($"{actorname} missed {targetname}.");
            }
            else
            {
                ReportResult($"{actorname} hit {targetname} for {damage} point{(damage > 1 ? "s" : "")}.");
                target.TakeDamage(damage);
            }
        }
    }
}
