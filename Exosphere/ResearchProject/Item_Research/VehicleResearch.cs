using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.Item_Research
{
    abstract class VehicleResearch : ItemResearch
    {
        public VehicleResearch(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "Vehicle";
            researchCategory = "Vehicle";

            researchPoints = 0;
            intelligenceRequirementValue = 0;
            intelligenceRequirement = 0;
            cost = 5;
            this.timesResearched += timesResearched;
            costValue = 1;
            maximumLevel = 5;
        }
    }
}
