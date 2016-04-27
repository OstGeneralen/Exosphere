using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject
{
    abstract class Buff : Research
    {
        protected float buffValue;

        public Buff(int colonyID, int timesResearched)
            : base(colonyID, timesResearched)
        {
            buffValue = 1.5f;
            costValue = 1.5f;
            cost = 25;
            intelligenceRequirement = 10;
            intelligenceRequirementValue = 1.2f;
            labRequirementValue = 0;
        }

        public override float GetNewPercentage()
        {
            return (float)Math.Pow(buffValue, timesResearched);
        }
    }
}
