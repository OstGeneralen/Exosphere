using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.Buffs
{
    class LessMaintenanceCost : Buff
    {
        public LessMaintenanceCost(int colonyID, int timesResearched)
            : base(colonyID, timesResearched) 
        {
            researchType = "LessMaintenanceCost";

            name = "Less Maintenance Cost";
        }

        public override string Information()
        {
            return "Find more well-adjusted methods to preserve your facilities in good condition in order to decrease your maintenance expenses by 10%.";
        }
    }
}
