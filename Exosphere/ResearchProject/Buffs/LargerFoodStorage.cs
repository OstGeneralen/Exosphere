using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.Buffs
{
    class LargerFoodStorage : Buff
    {
        public LargerFoodStorage(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "LargerFoodStorage";

            name = "Improved Food Storage";
        }

        public override string Information()
        {
            return "Develop new stockpile methods that streamlines your 'food stores' which results in a 10% increase in food and water storage capacity.";
        }
    }
}
