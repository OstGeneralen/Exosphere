using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Items
{
    class NullItem : Item
    {
        public NullItem()
        {
            itemType = "null";
            costIron = 0;
            costCopper = 0;
            costCarbon = 0;
            assemblingPointsNeeded = 0;
        }
    }
}
