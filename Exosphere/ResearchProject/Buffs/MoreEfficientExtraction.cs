using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.Buffs
{
    class MoreEfficientExtraction : Buff
    {
        public MoreEfficientExtraction(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "MoreEfficientExtraction";

            name = "Improved Mining Buff";
        }

        public override string Information()
        {
            return "Find more efficient methods to extract minerals from the planet's bedrock in order to increase your mines efficiency by 10%.";
        }
    }
}
