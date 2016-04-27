using Exosphere.Src.Basebuilding.Facilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.FacilitiesResearch
{
    class WorkshopResearch : Research
    {
        public WorkshopResearch(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "WorkshopResearch";

            name = "Improved Assembling";

            this.timesResearched = timesResearched + 2;
        }

        public override string Information()
        {
            return "Study new ways to use the workshop for assembling items and vehicles. Allows upgrading Workshop to leve " + timesResearched.ToString();
        }
    }
}
