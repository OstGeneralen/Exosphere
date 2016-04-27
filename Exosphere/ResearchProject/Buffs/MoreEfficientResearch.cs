using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.Buffs
{
    class MoreEfficientResearch : Buff
    {
        public MoreEfficientResearch(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "MoreEfficientResearch";
            researchPoints = 0;
            intelligenceRequirement = 0;
            this.timesResearched += timesResearched;
            maximumLevel = 80;
            labRequirement = 0;
            name = "Scientific Development";
        }

        public override string Information()
        {
            return "Develop new research methods that grants understanding within new scientific areas which results in a 10% increase in research efficiency";
        }
    }
}
