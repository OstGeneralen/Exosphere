using Exosphere.Src.Basebuilding.Facilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.FacilitiesResearch
{
    class EquipmentStoreResearch : Research
    {
        public EquipmentStoreResearch(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "EquipmentStoreResearch";

            name = "More Equipment Storage";

            this.timesResearched = timesResearched + 2;
        }

        public override string Information()
        {
            return "Study new ways of expanding the storage area of the Equipment Storage. This allows Equipment Stores to be upgraded to level " + timesResearched.ToString() + ".";
        }
    }
}
