using Exosphere.Src.Basebuilding.Facilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.FacilitiesResearch
{
    class SolarPanelsResearch : Research
    {
        public SolarPanelsResearch(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "SolarPanelsResearch";

            name = "Boost Solar Panels";

            this.timesResearched = timesResearched + 2;
        }

        public override string Information()
        {
            return "Research ways to boost the energy output from the solar panels. Allows upgrading Solar Panels to level " + timesResearched.ToString();
        }
    }
}
