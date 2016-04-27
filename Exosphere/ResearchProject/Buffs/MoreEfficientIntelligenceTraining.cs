using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.Buffs
{
    class MoreEfficientIntelligenceTraining : Buff
    {
        public MoreEfficientIntelligenceTraining(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "MoreEfficientIntelligenceTraining";

            name = "Mental Training Buff";
        }

        public override string Information()
        {
            return "Find methods to increase each individual's learning speed by 10% in order to facilitate their ability to gain more intelligence.";
        }
    }
}
