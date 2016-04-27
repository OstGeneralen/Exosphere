using Exosphere.Src.Basebuilding.Facilities;
using Exosphere.Src.Generators;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.HUD
{
    public class PossibleMiningPoint : Button
    {

        string wealthCopper;
        string wealthCarbon;
        string wealthIron;
        int amountCopper;
        int amountCarbon;
        int amountIron;
        int newAmountCopper;
        int newAmountIron;
        int newAmountCarbon;

        public PossibleMiningPointSave save;

        public void LoadPossibleMiningPoint(PossibleMiningPointSave load)
        {
            SetPosition(load.position);
            wealthCarbon = load.wealthCarbonSave;
            wealthCopper = load.wealthCopperSave;
            wealthIron = load.wealthIronSave;
            amountIron = load.amountIronSave;
            amountCopper = load.amountCopperSave;
            amountCarbon = load.amountCarbonSave;
            newAmountIron = load.newAmountIronSave;
            newAmountCopper = load.newAmountCopperSave;
            newAmountCarbon = load.newAmountCarbonSave;
        }

        public void SavePossibleMiningPoint()
        {
            save.position = position;
            save.wealthCarbonSave = wealthCarbon;
            save.wealthCopperSave = wealthCopper;
            save.wealthIronSave = wealthIron;
            save.amountCarbonSave = amountCarbon;
            save.amountCopperSave = amountCopper;
            save.amountIronSave = amountIron;
            save.newAmountCarbonSave = newAmountCarbon;
            save.newAmountCopperSave = newAmountCopper;
            save.newAmountIronSave = newAmountIron;
        }

        public PossibleMiningPoint(Vector2 position, string wealthCopper, string wealthIron, string wealthCarbon, int amountCopper, int amountCarbon, int amountIron, string assetName = "Res/PH/Planet View/PossibleMiningAreaMarker")
            : base(assetName, position)
        {
            this.wealthCopper = wealthCopper;
            this.wealthIron = wealthIron;
            this.wealthCarbon = wealthCarbon;
            this.amountCopper = amountCopper;
            this.amountCarbon = amountCarbon;
            this.amountIron = amountIron;
            newAmountCarbon = amountCarbon;
            newAmountCopper = amountCopper;
            newAmountIron = amountIron;
        }

        public override void Update()
        {
            base.Update();

            if(this.Collision())
            {
                string message = "This point's wealth have been described by our explorers as: \nCopper: " + wealthCopper + "\nIron: " + wealthIron + "\nCarbon: " + wealthCarbon;
                MessageBox mb = new MessageBox(2, message);
                Core.currentMessageBox = mb;
            }

        }

        public int ExtractResources(Colonist colonist, string resourceType, Mine mine)
        {
            float resourceMultiplier = 0.00000725f;
            float strengthMultiplier = colonist.strength / 10;
            int value = 0;

                #region Copper
                if (resourceType == "Copper")
                {

                    value = (int)(amountCopper * resourceMultiplier * strengthMultiplier * colonist.efficiency * colonist.GetProficiency(mine) * colonist.GetHealthBasedEfficiency());
                    if (newAmountCopper >= value)
                    {
                        newAmountCopper -= value;
                    }
                    if (newAmountCopper < value)
                    {
                        value -= newAmountCopper;
                    }
                    return value;

                }
                #endregion

                #region Iron
                if (resourceType == "Iron")
                {
                    value = (int)(amountIron * resourceMultiplier * strengthMultiplier * colonist.efficiency * colonist.GetProficiency(mine) * colonist.GetHealthBasedEfficiency());
                    if (newAmountIron >= value)
                    {
                        newAmountIron -= value;
                    }
                    if (newAmountIron < value)
                    {
                        value -= newAmountIron;
                    }
                    return value;
                }
                #endregion

                #region Carbon
                if (resourceType == "Carbon")
                {
                    value = (int)(amountCarbon * resourceMultiplier * strengthMultiplier * colonist.efficiency * colonist.GetProficiency(mine) * colonist.GetHealthBasedEfficiency());
                    if (newAmountCarbon >= value)
                    {
                        newAmountCarbon -= value;
                    }
                    if (newAmountCarbon < value)
                    {
                        value -= newAmountCarbon;
                    }
                    return value;
                }
                #endregion
            

            return 0;
        }
    }

    public struct PossibleMiningPointSave
    {
        public string wealthCopperSave;
        public string wealthIronSave;
        public string wealthCarbonSave;
        public int amountIronSave;
        public int amountCopperSave;
        public int amountCarbonSave;
        public int newAmountCopperSave;
        public int newAmountCarbonSave;
        public int newAmountIronSave;
        public Vector2 position;
    }
}
