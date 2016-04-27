using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.Buffs
{
    class LessEnergyConsumption : Buff
    {
        public LessEnergyConsumption(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "LessEnergyConsumption";

            name = "Less Energy Consumption";
        }

        public override string Information()
        {
            return "Find more well-adjusted methods to make efficient use of your electrical energy which decrease's your energy consumption by 10%.";
        }
    }
}
