using Exosphere.Src.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject
{
    class Diseases : Research
    {
        public Diseases(int colonyID, int timesResearched, Disease disease)
            : base(colonyID, timesResearched)
        {
            identification = disease.GetIdentification();
            name = disease.GetName();
            researchType = "Disease";
            cost = disease.GetResistancy() / 10;
        }

        public override string GetResearchName()
        {
            return base.GetResearchName();
        }

        public override int GetIdentification()
        {
            return base.GetIdentification();
        }
    }
}
