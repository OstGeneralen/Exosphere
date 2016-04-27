using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.Buffs
{
    class MoreEfficientStrengthTraining : Buff
    {
        public MoreEfficientStrengthTraining(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "MoreEfficientStrengthTraining";

            name = "Physical Training Buff";
        }

        public override string Information()
        {
            return "Find methods to increase each individual's ability to build muscles by 10% in order to facilitate their ability to gain more strength.";
        }
    }
}
