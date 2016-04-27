using Exosphere.Src.Basebuilding.Facilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.FacilitiesResearch
{
    class LibraryResearch : Research
    {
        public LibraryResearch(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "LibraryResearch";

            name = "Extended Databases";

            researchPoints = 0;
            intelligenceRequirementValue = 0;
            intelligenceRequirement = 0;
            cost = 5;
            this.timesResearched = timesResearched + 2;
            costValue = 1;
            maximumLevel = 5;
        }

        public override string Information()
        {
            return "Study ways to extend the database containing the books avalible for the colonists. Allows upgrading of Library to level " + timesResearched.ToString()  ;
        }
    }
}
