using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject.Buffs
{
    class QuickerAssembling : Buff
    {
        public QuickerAssembling(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "QuickerAssembling";

            name = "Increased Assembling Speed";
        }

        public override string Information()
        {
            return "Streamline your machinery to extract higher manufacturing efficiency that reduces the time needed to assemble an item by 10%.";
        }
    }
}
