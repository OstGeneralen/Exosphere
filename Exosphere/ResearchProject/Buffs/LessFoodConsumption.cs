using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.FacilitiesResearch
{
    class LessFoodConsumption : Buff
    {
        public LessFoodConsumption(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "LessFoodConsumption";

            name = "Less Food Consumption";
        }

        public override string Information()
        {
            return "Find more well-adjusted methods to preserve your food in good condition in order to decrease your colonies food consumption by 10%.";
        }
    }
}
