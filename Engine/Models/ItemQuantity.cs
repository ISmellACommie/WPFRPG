using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    class ItemQuantity
    {
        public int ITEMID
        {
            get;
            set;
        }
        public int QUANTITY
        {
            get;
            set;
        }
        public ItemQuantity(int _itemid, int _quantity)
        {
            ITEMID = _itemid;
            QUANTITY = _quantity;
        }
    }
}
