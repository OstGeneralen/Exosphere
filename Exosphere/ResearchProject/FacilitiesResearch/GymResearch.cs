using Exosphere.Src.Basebuilding.Facilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.FacilitiesResearch
{
    class GymResearch : Research
    {
        public GymResearch(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "GymResearch";

            name = "Better Training Conditions";

            this.timesResearched = timesResearched + 2;
        }

        public override string Information()
        {
            return "Study new ways to plan the layout of the Gym to boost the efficiency further. Allows upgrading Gym to level " + timesResearched;
        }
    }
}
