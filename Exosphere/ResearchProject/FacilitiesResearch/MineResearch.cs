using Exosphere.Src.Basebuilding.Facilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.FacilitiesResearch
{
    class MineResearch : Research
    {
        public MineResearch(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "MineResearch";

            this.timesResearched = timesResearched + 2;

            name = "Extended Mining";
        }

        public override string Information()
        {
            return "Research the structure of the planet to construct better mines. Allows upgrading of mines to level " + timesResearched.ToString();
        }
    }
}
