using Exosphere.Src.Basebuilding.Facilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.FacilitiesResearch
{
    class FoodStoreResearch : Research
    {
        public FoodStoreResearch(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "FoodStoreResearch";

            name = "More Food Storage";

            this.timesResearched = timesResearched + 2;
        }

        public override string Information()
        {
            return "Explore more efficient ways of storing food and water. This allows all Food Stores to be upgraded to level " + timesResearched.ToString();
        }
    }
}
