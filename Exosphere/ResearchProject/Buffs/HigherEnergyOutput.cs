using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.Buffs
{
    class HigherEnergyOutput : Buff
    {
        public HigherEnergyOutput(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "HigherEnergyOutput";

            name = "Higher Energy Output";
        }

        public override string Information()
        {
            return "Develop new solar cells with higher efficiency due to their improved ability to convert solar energy to electrical energy.";
        }
    }
}
