using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.Buffs
{
    class LessConstructionCost : Buff
    {
        public LessConstructionCost(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "LessConstructionCost";

            name = "Less Construction Costs";
        }

        public override string Information()
        {
            return "Find more well-adjusted methods to make efficient use of your minerals in order to decrease your facilities construction expenses by 10%.";
        }
    }
}
