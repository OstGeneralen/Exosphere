using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.Buffs
{
    class LargerEquipmentStorage : Buff
    {
        public LargerEquipmentStorage(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "LargerEquipmentStorage";

            name = "Improved Equipment Storage";
        }

        public override string Information()
        {
            return "Develop new stockpile methods that streamlines your 'equipment stores' which results in a 10% increase in equipment storage capacity.";
        }
    }
}
