using System;
using Engine.Models;

namespace Engine.Actions
{
    public abstract class BaseAction
    {
        protected readonly GameItem itemInUse;

        public event EventHandler<string> ONACTIONPERFORMED;

        protected BaseAction(GameItem _iteminuse)
        {
            itemInUse = _iteminuse;
        }

        protected void ReportResult(string result)
        {
            ONACTIONPERFORMED?.Invoke(this, result);
        }
    }
}
