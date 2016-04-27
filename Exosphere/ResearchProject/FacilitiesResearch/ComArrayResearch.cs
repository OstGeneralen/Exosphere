using Exosphere.Src.Basebuilding.Facilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.FacilitiesResearch
{
    class ComArrayResearch : Research
    {
        public ComArrayResearch(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "ComArrayResearch";

            labRequirement = 2;

            name = "Radar Development";

            timesResearched = timesResearched + 2;
        }

        public override string Information()
        {
            return "Research better components that allows the Com-Array to be upgraded to level " + (timesResearched).ToString();
        }
    }
}
