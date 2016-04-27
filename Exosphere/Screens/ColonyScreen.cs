using Exosphere.Src.Basebuilding;
using Exosphere.Src.Generators;
using Exosphere.Src.Handlers;
using Exosphere.Src.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Screens
{
    class ColonyScreen : Screen
    {
        //The Facility name of the facility currently held in the cursor in line to be built
        public static string facilityHold;

        //The colony that this colony screen is representing
        Colony colony;

        /// <summary>
        /// Creates a new colony screen
        /// </summary>
        public ColonyScreen()
        {
            screenName = "Colony Screen";
        }

        /// <summary>
        /// Sets the colony that is currently active
        /// </summary>
        /// <param name="colony">The colony that should be currently drawn and active</param>
        public void SetColony(Colony colony)
        {
            this.colony = colony;
        }

        /// <summary>
        /// Updates the Colony Screen
        /// </summary>
        public override void Update()
        {
            if(colony.ID == 1 && Core.firstColonyFirstTime)
            {
                string message = "";

                message = message.Insert(message.Length, "Welcome to the colony view.\n\n");
                message = message.Insert(message.Length, "This screen will help you in many ways as the Exosphere operation progresses.\n");
                message = message.Insert(message.Length, "Here you can read through the data concerning the entire colony as well as view the stats ");
                message = message.Insert(message.Length, "for each induvidual colonist.\n");
                message = message.Insert(message.Length, "The ship you flew with from earth already have you supplied with sufficient resources to build: \n");
                message = message.Insert(message.Length, "One mine, One pump, One Living Quarters, One Food Store, One Greenhouse and One Solar Panel\n\n");
                message = message.Insert(message.Length, "Once these facilities have been built there will be no more free constructions for you.\n");
                message = message.Insert(message.Length, "From here on you are on your own.");

                MessageBox mb = new MessageBox(3, message);
                Core.currentMessageBox = mb;
                Core.firstColonyFirstTime = false;               
            }

            //Updates the colony if colony is not null
            if (colony != null)
                colony.Update();
            //Throws exception if colony is null
            else if (colony == null)
                throw new Exception("Colony can not be null");

            //Make the back button return you to planet screen
            if (HUD.ActionButtons.backButton.Collision())
                Core.SetPlanetView(colony.GetPlanet());

            if(HUD.ActionButtons.infoButton.Collision())
            {
                string message = "";

                message = message.Insert(message.Length, "Current Colony Data: \n");
                message = message.Insert(message.Length, "Inhabitants: " + colony.GetInhabitants().Count.ToString() + "\n\n");
                message = message.Insert(message.Length, "Carbon stores: " + colony.GetGrid().resourceManager.carbon + "\n");
                message = message.Insert(message.Length, "Copper stores: " + colony.GetGrid().resourceManager.copper + "\n");
                message = message.Insert(message.Length, "Iron stores: " + colony.GetGrid().resourceManager.iron + "\n\n");
                message = message.Insert(message.Length, "Food stores: " + colony.GetGrid().resourceManager.food + "\n");
                message = message.Insert(message.Length, "Water stores: " + colony.GetGrid().resourceManager.clearwater + "\n");
                message = message.Insert(message.Length, "Expected food expenditure (daily): " + colony.GetGrid().resourceManager.GetFoodConsumptionPerDay(colony) + "\n");
                message = message.Insert(message.Length, "Expected food revenue (daily): " + colony.GetGrid().dailyFoodRevenue.ToString() + "\n");
                message = message.Insert(message.Length, "Expected water expenditure (daily): " + colony.GetGrid().resourceManager.GetWaterConsumptionPerDay(colony) + "\n");
                message = message.Insert(message.Length, "Expected water revenue (daily): " + colony.GetGrid().dailyWaterRevenue.ToString() + "\n");

                MessageBox journal = new MessageBox(3, message);
                Core.currentMessageBox = journal;
            }
        }

        /// <summary>
        /// Draw the Colony Screen
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch used for drawing</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            //Draw the colony
            colony.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
