using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.Buffs
{
    class LargerMineralStorage : Buff
    {
        public LargerMineralStorage(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "LargerMineralStorage";

            name = "Improved Mineral Storage";
        }

        public override string Information()
        {
            return "Develop new stockpile methods that streamlines your 'mineral stores' which results in a 10% increase in mineral storage capacity.";
        }
    }
}
