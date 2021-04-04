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

            if(damage == 0)
            {
                ReportResult($"You missed the {target.NAME.ToLower()}.");
            }
            else
            {
                ReportResult($"You hit the {target.NAME.ToLower()} for {damage} points.");
                target.TakeDamage(damage);
            }
        }

        private void ReportResult(string result)
        {
            ONACTIONPERFORMED?.Invoke(this, result);
        }
    }
}
