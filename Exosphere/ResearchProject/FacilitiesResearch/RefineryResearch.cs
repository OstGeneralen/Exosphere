using Exosphere.Src.Basebuilding.Facilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.FacilitiesResearch
{
    class RefineryResearch : Research
    {
        public RefineryResearch(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "RefineryResearch";
        }
    }
}
