using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject
{
    class Minerals : Research
    {
        public Minerals(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            researchType = "Minerals";
        }
    }
}
