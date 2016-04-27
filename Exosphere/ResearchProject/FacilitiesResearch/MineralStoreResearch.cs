using Exosphere.Src.Basebuilding.Facilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.FacilitiesResearch
{
    class MineralStoreResearch : Research
    {
        public MineralStoreResearch(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "MineralStoreResearch";

            name = "More Mineral Storage";

            this.timesResearched = timesResearched + 2;
        }

        public override string Information()
        {
            return "Study new ways of storing minerals to greatly extend the storage area. Allows upgrading of Mineral Stores to level " + timesResearched.ToString();
        }
    }
}
