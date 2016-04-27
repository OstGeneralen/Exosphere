using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.Buffs
{
    class LessWaterConsumption : Buff
    {
        public LessWaterConsumption(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "LessWaterConsumption";

            name = "Less Water Consumption";
        }

        public override string Information()
        {
            return "Find more well-adjusted methods to store water in order to decrease your colonies water consumption by 10%.";
        }
    }
}
