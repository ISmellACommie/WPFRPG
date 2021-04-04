using System;
using Engine.Models;

namespace Engine.Actions
{
    public interface IAction
    {
        event EventHandler<string> ONACTIONPERFORMED;
        void Execute(LivingEntity actor, LivingEntity target);
    }
}
