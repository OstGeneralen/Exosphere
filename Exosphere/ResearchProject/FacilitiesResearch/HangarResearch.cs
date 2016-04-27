using Exosphere.Src.Basebuilding.Facilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.FacilitiesResearch
{
    class HangarResearch : Research
    {
        public HangarResearch(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "HangarResearch";

            name = "Enhanced Hangar";

            this.timesResearched = timesResearched + 2;
        }

        public override string Information()
        {
            return "Study new ways of keeping and maintaining ships in the hangar. This allows all garages to be upgraded to level " + timesResearched.ToString();
        }
    }
}
