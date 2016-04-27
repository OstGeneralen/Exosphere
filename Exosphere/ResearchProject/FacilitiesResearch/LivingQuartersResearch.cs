using Exosphere.Src.Basebuilding.Facilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.FacilitiesResearch
{
    class LivingQuartersResearch : Research
    {
        public LivingQuartersResearch(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "LivingQuartersResearch";

            this.timesResearched = timesResearched + 2;

            name = "Extended Living Quarters";
        }

        public override string Information()
        {
            return "Research ways to extend the living quarters to allow housing more colonists. Allows upgrading of Living Quarters to level " + timesResearched.ToString();
        }
    }
}
