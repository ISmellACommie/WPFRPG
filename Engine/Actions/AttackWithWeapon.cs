using System;
using Engine.Models;

namespace Engine.Actions
{
    public class AttackWithWeapon : IAction
    {
        private readonly GameItem weapon;
        private readonly int mindmg;
        private readonly int maxdmg;

        public event EventHandler<string> ONACTIONPERFORMED;

        public AttackWithWeapon(GameItem _weapon, int _mindmg, int _maxdmg)
        {
            if(_weapon.CATEGORY != GameItem.ItemCategory.Weapon)
            {
                throw new ArgumentException($"{weapon.NAME} is not a weapon");
            }

            if(_mindmg < 0)
            {
                throw new ArgumentException("mindmg must be >= 0");
            }

            if(_maxdmg < _mindmg)
            {
                throw new ArgumentException("max dmg must be > mindmg");
            }

            weapon = _weapon;
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
                ReportResult($"{actorname} hit {targetname} for {damage} points.");
                target.TakeDamage(damage);
            }
        }

        private void ReportResult(string result)
        {
            ONACTIONPERFORMED?.Invoke(this, result);
        }
    }
}
