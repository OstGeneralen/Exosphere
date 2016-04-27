using Exosphere.Src.Basebuilding.Facilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.FacilitiesResearch
{
    class MedBayResearch : Research
    {
        public MedBayResearch(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "MedBayResearch";

            name = "Improved Med-Bay";

            this.timesResearched = timesResearched + 2;
        }

        public override string Information()
        {
            return "Research equipment that helps improve the med-bay and allows it to perform new operations. Allows upgrading Med-Bay to level " + timesResearched.ToString();
        }
    }
}
