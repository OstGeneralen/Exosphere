using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.Item_Research.Vehicle_Research
{
    class VehicleC60Research : VehicleResearch
    {
        public VehicleC60Research(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "VehicleC60";

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
