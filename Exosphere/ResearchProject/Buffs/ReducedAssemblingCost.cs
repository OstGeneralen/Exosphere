using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.Buffs
{
    class ReducedAssemblingCost : Buff
    {
        public ReducedAssemblingCost(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "ReducedAssemblingCost";

            name = "Reduced Assembling Cost";
        }

        public override string Information()
        {
            return "Find more well-adjusted methods to assemble and construct in order to decrease your manufacturing expenses by 10%.";
        }
    }
}
