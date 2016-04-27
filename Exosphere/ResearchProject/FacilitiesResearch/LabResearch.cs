using Exosphere.Src.Basebuilding.Facilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.FacilitiesResearch
{
    class LabResearch : Research
    {
        public LabResearch(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "LabResearch";

            name = "Improved Laboratories";

            this.timesResearched = timesResearched + 2;
        }

        public override string Information()
        {
            return "Research new technologies that help making the research process more efficient and less limited. Allows upgradeing of Lab to level "+ timesResearched.ToString();
        }
    }
}
