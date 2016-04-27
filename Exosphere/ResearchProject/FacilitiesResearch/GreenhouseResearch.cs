using Exosphere.Src.Basebuilding.Facilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.FacilitiesResearch
{
    class GreenhouseResearch : Research
    {
        public GreenhouseResearch(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "GreenhouseResearch";

            name = "Improved plantations";

            this.timesResearched = timesResearched + 2;
        }

        public override string Information()
        {
            return "Research better ways of harvesting in the Greenhouse. This allows all Greehouses to be upgraded to level " + timesResearched.ToString();
        }
    }
}
