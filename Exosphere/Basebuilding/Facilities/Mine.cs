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

namespace Exosphere.Src.Basebuilding.Facilities
{
    public class Mine : Facility
    {
        //The amount the mine should extract per turn
        public int extraction = 10;

        //A bool telling if the mine is active in a mining area or not
        PossibleMiningPoint activeMiningPoint;
        //bool active;

        #region Load/Save

        public override void LoadFacility(MineSave load)
        {
            this.position = load.position;

            this.colonyID = load.colonyID;

            this.level = load.level;

            this.storageLevel = load.storageLevel;

            this.housingLimit = load.housingLimit;

            this.finished = load.finished;

            this.timeUnderConstruction = load.timeUnderConstruction;

            this.ID = load.ID;

            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/MinePH");

            for (int i = 0; i < colony.inhabitants.Count; i++)
            {
                if (colony.inhabitants[i].occupied && colony.inhabitants[i].facilityID == ID)
                {
                    workers.Add(colony.inhabitants[i]);
                    colony.inhabitants[i].workPlace = this;
                }
            }
        }

        public override void SaveFacility()
        {
            mineSave.position = position;
            mineSave.colonyID = colonyID;
            mineSave.level = level;
            mineSave.storageLevel = storageLevel;
            mineSave.housingLimit = housingLimit;
            mineSave.finished = finished;
            mineSave.timeUnderConstruction = timeUnderConstruction;
            mineSave.ID = ID;
        }
        #endregion

        /// <summary>
        /// Creates a new mine
        /// </summary>
        /// <param name="position"></param>
        /// <param name="colony"></param>
        public Mine(Vector2 position, Colony colony, int ID)
            : base(position, colony, ID)
        {
            //Loads the mine texture into the texture
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/MinePH");

            //Set the facility type to be mine
            facilityType = "Mine";

            //Sets the requirements
            intelligenceRequirement = 7;
            strengthRequirement = 10;
            healthRequirement = 50;
            immuneSystemRequirement = 40;

            costCopper = 200;
            costIron = 300;
            costCarbon = 100;

            copperValue = 4.9f;
            carbonValue = 3.05f;
            ironValue = 4.05f;

            maxLevel = 4;

            requiredEnergy = 3000;

            //Creates a button for the Mine
            CreateFacilityButton();

            //Sets the taskScreen to be the MineTaskScreen
            taskScreen = new MineTaskScreen(this);

            //TODO: Fix this, the mine should not be finished from the start
            finished = true;

            SetDebugValues();
        }

        /// <summary>
        /// Runs the mine's task
        /// </summary>
        /// <param name="value">The amount of the resource you want to add</param>
        /// <returns>The new amount after the mine has mined</returns>
        public override int Task(int value, string resourceType)
        {

            activeMiningPoint = (PossibleMiningPoint)taskScreen.GetWorkingValue();

            //Checks if the mine should do any work
            if (taskScreen.ShouldWork())
                //Set value to be += the extraction * amount of workers * the average efficiency of the workers in the facility
                foreach (var worker in workers)
                {
                    value += activeMiningPoint.ExtractResources(worker, resourceType, this);
                }
            

            return value;
        }

    }

    class MineTaskScreen : FacilityTaskScreen
    {
        //The mine the task screen is for 
        Mine mine;

        PossibleMiningPoint miningPoint;

        Texture2D mineMarker;

        /// <summary>
        /// Creates a new mine task screen
        /// </summary>
        /// <param name="mine">The mine that this should be relevant to</param>
        public MineTaskScreen(Mine mine)
        {
            mineMarker = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Facility View/MineMarkerPH");
            //Sets the local variable for mine to be equal the mine sent in
            this.mine = mine;
        }

        /// <summary>
        /// Checks if the mine should do any work
        /// </summary>
        /// <returns>Returns a bool. True if the facility should work and false if not</returns>
        public override bool ShouldWork()
        {
            if (miningPoint != null)
                return true;

            //Return false if the previous if statement in the for loop is false
            return false;
        }

        public override object GetWorkingValue()
        {
            return miningPoint;
        }

        /// <summary>
        /// Updates the Mine task screen
        /// </summary>
        public override void Update()
        {
            foreach(var possibleMiningArea in mine.GetColony().GetPlanet().possibleMiningPoints)
            {
                possibleMiningArea.Update();

                if(possibleMiningArea.Collision())
                {
                    miningPoint = possibleMiningArea;
                }
            }
        }

        /// <summary>
        /// Draw the mine task screen
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used for drawing</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Draw the planet's surface as the player will be able to choose where to mine
            mine.GetColony().GetPlanet().Draw(spriteBatch);

            if(miningPoint != null)
                spriteBatch.Draw(mineMarker, new Vector2(miningPoint.GetPosition().X - mineMarker.Width / 2, miningPoint.GetPosition().Y - mineMarker.Height / 2), Color.White);
        }
    }


    /* 
    class MiningArea
    {
        #region Basic Variables
        //The position of the mining area in pixels
        Vector2 position;

        //The texture of the mining area
        Texture2D texture;

        //The collision of the mining area
        Rectangle collision;
        #endregion

        #region Activation

        //A bool telling if the mining area should check if it's active or not
        bool showChoice;

        //A bool telling if the mining area is active or not
        public bool active;

        //Creates yes and no button
        Button yes, no;

        #endregion

        Planet planet;

        int startCopper;
        int startIron;
        int startCarbon;
        ResourceArea resourceArea;

        /// <summary>
        /// Creates a new mining area
        /// </summary>
        /// <param name="position">The position at which to create the mining area</param>
        public MiningArea(Vector2 position, Planet planet)
        {
            //Sets the texture
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/MineMarkerPH");

            //Sets the position
            this.position = position;

            //Sets the collision based on the texture and position
            collision = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            this.planet = planet;

            Rectangle middlePointCollision;

            middlePointCollision = new Rectangle((int)(position.X + texture.Width / 2), (int)(position.Y + texture.Height / 2), 1, 1);



            for (int i = 0; i < planet.GetResourceArea().Length; i++)
            {
                if (planet.GetResourceArea()[i].collsion.Intersects(middlePointCollision))
                {
                    if (!planet.GetResourceArea()[i].hasResources)
                        planet.GetResourceArea()[i].AddResources(planet.GetBiomes().BiomeColiding(middlePointCollision));

                    startCarbon = planet.GetResourceArea()[i].GetCarbon();
                    startIron = planet.GetResourceArea()[i].GetIron();
                    startCopper = planet.GetResourceArea()[i].GetCopper();
                    resourceArea = planet.GetResourceArea()[i];
                }

            }

            #region Choice buttons

            //Creates the buttons for yes and no
            yes = new Button("Res/PH/ButtonUpPH", Vector2.Zero, "Yes");
            no = new Button("Res/PH/ButtonUpPH", Vector2.Zero, "No");

            //Sets the position for yes and no 
            yes.SetPosition(new Vector2(
                (Settings.GetScreenRes.X / 2) - (yes.GetTexture().Width / 2),
                (Settings.GetScreenRes.Y * 0.25f) - (yes.GetTexture().Height / 2)
                ));

            no.SetPosition(new Vector2(
                (Settings.GetScreenRes.X / 2) - (no.GetTexture().Width / 2),
                (Settings.GetScreenRes.Y * 0.75f) - (no.GetTexture().Height / 2)
                ));

            #endregion
        }

        /// <summary>
        /// Returns the collision rectangle of the mining area
        /// </summary>
        /// <returns>A rectangle with the collision area</returns>
        public Rectangle GetCollision()
        {
            return collision;
        }

        public int GetResourcesPerColonist(float efficiency, Colonist colonist, string resourceType, float proficiency)
        {
            float resourceMultiplier = 0.00000725f;
            float strengthMultiplier = colonist.strength / 10;
            int value = 0;

            if (resourceArea != null)
            {
                #region Copper
                if (resourceType == "Copper")
                {

                    value = (int)(startCopper * resourceMultiplier * strengthMultiplier * efficiency * proficiency * colonist.GetHealthBasedEfficiency());
                    if (resourceArea.GetCopper() >= value)
                    {
                        resourceArea.SubtractCopper(value);
                    }
                    if (resourceArea.GetCopper() < value)
                    {
                        value -= resourceArea.GetCopper();
                    }
                    return value;

                }
                #endregion

                #region Iron
                if (resourceType == "Iron")
                {
                    value = (int)(startIron * resourceMultiplier * strengthMultiplier * efficiency * proficiency * colonist.GetHealthBasedEfficiency());
                    if (resourceArea.GetIron() >= value)
                    {
                        resourceArea.SubtractIron(value);
                    }
                    if (resourceArea.GetIron() < value)
                    {
                        value -= resourceArea.GetIron();
                    }
                    return value;
                }
                #endregion

                #region Carbon
                if (resourceType == "Carbon")
                {
                    value = (int)(startCarbon * resourceMultiplier * strengthMultiplier * efficiency * proficiency * colonist.GetHealthBasedEfficiency());
                    if (resourceArea.GetCarbon() >= value)
                    {
                        resourceArea.SubtractCarbon(value);
                    }
                    if (resourceArea.GetCarbon() < value)
                    {
                        value -= resourceArea.GetCarbon();
                    }
                    return value;
                }
                #endregion
            }

            return 0;
        }

        /// <summary>
        /// Updates the mining area
        /// </summary>
        public void Update()
        {
            //If the bool showChoice is not true
            if (!showChoice)
            {
                //If the clicks on the mining point
                if (Cursor.collision.Intersects(collision) && MouseHandler.LMBOnce())
                {
                    //Set showChoice to be true
                    showChoice = true;
                }
            }

            //If show choice is true
            if (showChoice)
            {
                //Update the yes and no buttons
                yes.Update();
                no.Update();

                //If the player presses the yes button set active to true and showChoice to false
                if (yes.Collision())
                {
                    active = true;
                    showChoice = false;
                }

                //If the player presses the no button set active to false and showChoice to false
                if (no.Collision())
                {
                    active = false;
                    showChoice = false;
                }

            }
        }

        /// <summary>
        /// Draws the mining area
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //If the mining area is not active
            if (!active)
                //Draw the standard mining area texture
                spriteBatch.Draw(texture, position, Color.White);

            //If the mining area is active
            if (active)
                //Draw the mining area in a diffrent color
                spriteBatch.Draw(texture, position, Color.Lime);

            //If showChoice is true
            if (showChoice)
            {
                //Draw the buttons 
                yes.Draw(spriteBatch);
                no.Draw(spriteBatch);
            }

        }
    }
    */

    public struct MineSave
    {
        public Vector2 position;

        public int colonyID;

        public int level;

        public int storageLevel;

        public int housingLimit;

        public bool finished;

        public int timeUnderConstruction;

        public int ID;
    }

}