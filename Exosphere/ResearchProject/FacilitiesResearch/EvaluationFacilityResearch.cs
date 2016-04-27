using Exosphere.Src.Basebuilding.Facilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.FacilitiesResearch
{
    class EvaluationFacilityResearch : Research
    {
        public EvaluationFacilityResearch(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "EvaluationFacilityResearch";

            name = "Behavioural Science";

            this.timesResearched = timesResearched + 2;
        }

        public override string Information()
        {
            return "Research new behavioural patterns to help explain previously unknown characteristic feats. This allows the upgrading of all Evaluation Facilies to level " + timesResearched.ToString();
        }
    }
}
