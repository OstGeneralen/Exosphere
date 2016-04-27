using Exosphere.Src.Basebuilding;
using Exosphere.Src.Basebuilding.Facilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject
{
    class FacilityResearch : Research
    {
        public FacilityResearch(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "FacilityResearch";

            name = "Human Storage";
            researchPoints = 0;
            intelligenceRequirement = 0;
            this.timesResearched = timesResearched;
            cost = 1;
            maximumLevel = 5;
            costValue = 20;
            labRequirement = 0;
        }
    }
}
