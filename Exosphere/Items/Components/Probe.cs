using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Items.Components
{
    class Probe : Item
    {
        public Probe()
        {

            itemType = "Probe";
            costIron = 0;
            costCopper = 0;
            costCarbon = 0;
            assemblingPointsNeeded = 0;
        }
    }
}
