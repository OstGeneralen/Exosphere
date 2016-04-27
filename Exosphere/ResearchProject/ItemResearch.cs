using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject
{
    abstract class ItemResearch : Research
    {
        public ItemResearch(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "Item";

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
