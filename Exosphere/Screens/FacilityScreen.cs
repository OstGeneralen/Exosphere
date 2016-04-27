using Exosphere.Src.Basebuilding;
using Exosphere.Src.Basebuilding.Facilities;
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
    class FacilityScreen : Screen
    {
        //The facility
        Facility facility;

        //A button to press to get a list of all the unoccupied and compatible workers
        Button work;

        //The task screen of the facility
        FacilityTaskScreen taskScreen;

        //A list of buttons used to choose workers in this facility
        public List<Button> workerButtons;

        //A screen containing basic facility information
        BackgroundScreen backgroundScreen;

        //A bool telling if the game should show all the workers of the facility
        public bool showWorkers;

        //A bool telling if the game should show the task screen for the facility
        public bool showTask;

        ScrollBox scrollBox;

        /// <summary>
        /// The screen showing the specific facility choosen
        /// </summary>
        public FacilityScreen()
        {
            screenName = "Facility Screen";

            //The button pressed to show all compatible colonists
            work = new Button("Res/PH/HUD/Buttons/Standard/ButtonUpPH", new Vector2(800, 800));

            //A list of buttons representing all compatible colonists
            workerButtons = new List<Button>();
            workerButtons.Add(new Button("Res/PH/HUD/Buttons/Standard/ButtonUpPH", Vector2.Zero, "KK"));
            backgroundScreen = new BackgroundScreen();

            //Standardly sets the showTask and showWorkers bools to false
            showTask = false;
            showWorkers = false;

            scrollBox = new ScrollBox(workerButtons);
        }

        /// <summary>
        /// Sets which specific facility to look at
        /// </summary>
        /// <param name="facility">The facility</param>
        public void SetFacility(Facility facility)
        {
            //Sets the facility to equal the one told to set it to
            this.facility = facility;

            //A temp int for adding colony buttons in worker-screen
            int i = 0;

            //Loops through each colonist in the facility
            foreach (Colonist colonist in facility.GetColony().GetInhabitants())
            {
                //If the colonist is not already working somewhere or if it is working in this facility
                if (!colonist.occupied || colonist.workPlace == facility)
                    //If the colonist can work in this facility
                    if (facility.CanWork(colonist))
                    {
                        //Add a new workerButton for the colonist
                        workerButtons.Add(new WorkersButton(colonist, i));
                        //Set I to bew plus 1
                        i++;
                    }
            }

            scrollBox.SetButtons(workerButtons);
            backgroundScreen.SetInformationBox(facility);
            this.taskScreen = facility.taskScreen;
        }

        public string GetFacility()
        {
            return facility.GetFacilityType();
        }

        /// <summary>
        /// Updates the Facility screen
        /// </summary>
        public override void Update()
        {
            //If the back button is pressed
            if (HUD.ActionButtons.backButton.Collision())
            {
                //Set the view to be Colony view
                Core.SetColonyView(facility.GetColony());

                //Clear the list of worker buttons
                workerButtons.Clear();
            }

            scrollBox.Update();

            //If both showTask and showWorkers is true throw and exception
            if (showTask && showWorkers)
                throw new Exception("Both showTask and showWorkers was set true");


            //Run the update that should be run
            #region Updates
            if (showWorkers)
            {
                UpdateWorkers();
            }

            if (showTask)
            {
                UpdateTask();
            }
            #endregion

            backgroundScreen.Update(showWorkers, showTask, facility);
            backgroundScreen.SetInformationBox(facility);
        }

        /// <summary>
        /// Updates the worker screen
        /// </summary>
        public void UpdateWorkers()
        {
            //Loops through each worker button
            foreach (var wb in workerButtons)
            {
                WorkersButton tempWb = (WorkersButton)wb;

                //Updates the worker button
                tempWb.Update();

                tempWb.ChangeColor();
                //If the worker button is pressed
                if (tempWb.Collision())
                {
                    //If the colonist is not already working
                    if (tempWb.colonist.occupied)
                    {
                        //Add this colonist to this facility's list of workers
                        facility.AddWorker(tempWb.colonist);

                        //Set the colonist's workplace to be this facility
                        tempWb.colonist.workPlace = facility;
                    }
                    //Else if the colonist is occupied
                    else if (!tempWb.colonist.occupied)
                    {
                        //Remove the worker from this facility's list of workers
                        facility.RemoveWorker(tempWb.colonist);

                        //Set the colonist's workplace to be null
                        tempWb.colonist.workPlace = null;
                    }

                    //Toggle the colonists 'occupied' bool
                    //wb.ToggleWork();
                }
            }
        }

        /// <summary>
        /// Updates the task screen
        /// </summary>
        public void UpdateTask()
        {
            //Run the taskScreen.Update()
            taskScreen.Update();
        }

        /// <summary>
        /// Draw the facility screen
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used for drawing</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            //Draw the section that should be drawn
            if (showWorkers)
            {
                scrollBox.Draw(spriteBatch);
            }

            if (showTask)
                taskScreen.Draw(spriteBatch);

            backgroundScreen.Draw(showWorkers, showTask, spriteBatch);

            spriteBatch.End();
        }
    }

    class BackgroundScreen
    {
        Texture2D texture;
        Vector2 position;
        InfoBox informationBox;
        InfoBox priceBox;

        public BackgroundScreen()
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Facility View/MineScreen");
            position = new Vector2(Settings.GetScreenRes.X / 2 - texture.Width / 2, Settings.GetScreenRes.Y / 2 - texture.Height / 2);
            informationBox = new InfoBox(1, "Hello World", new Button("Res/PH/HUD/Buttons/Standard/ButtonUpPH", Vector2.Zero, "Upgrade"));
            informationBox.OverridePosition(new Vector2(position.X, position.Y));
        }

        public void SetInformationBox(Facility facility)
        {
            if (facility != null)
            {
                informationBox = new InfoBox(1, facility.Information(), new Button("Res/PH/HUD/Buttons/Standard/ButtonUpPH", Vector2.Zero, "Upgrade"));
                informationBox.OverridePosition(new Vector2(position.X, position.Y));
            }
        }

        public void Update(bool showWorkers, bool showTask, Facility facility)
        {
            if (!showTask && !showWorkers)
            {
                informationBox.Update();

                if(informationBox.GetButton().collision.Intersects(Cursor.collision) && facility.level != facility.maxLevel)
                {
                    string message = "";

                    message = message.Insert(message.Length, "Upgrade "+ facility.GetFacilityType() + " to level " + (facility.level + 1).ToString() + "\n\n");
                    message = message.Insert(message.Length, "Required Copper: " + facility.GetUpgradeCostCopper().ToString() + "\n");
                    message = message.Insert(message.Length, "Required Iron: " + facility.GetUpgradeCostIron().ToString() + "\n");
                    message = message.Insert(message.Length, "Required Carbon: " + facility.GetUpgradeCostCarbon().ToString());

                    priceBox = new InfoBox(0, message);
                }
                else if (informationBox.GetButton().collision.Intersects(Cursor.collision) && facility.level == facility.maxLevel)
                {
                    string message = "";

                    message = message.Insert(message.Length, "This " + facility.GetFacilityType() + " is at its max level.\n");

                    priceBox = new InfoBox(0, message);
                }
                else if(!informationBox.GetButton().collision.Intersects(Cursor.collision))
                {
                    priceBox = null;
                }

                if (Cursor.collision.Intersects(informationBox.GetButton().collision) && MouseHandler.LMBOnce() && facility.level != facility.maxLevel)
                {
                    if (facility.Upgrade())
                    {
                        foreach (var colonist in facility.GetWorkers())
                        {
                            colonist.occupied = false;
                        }
                        facility.GetWorkers().Clear();

                        facility.StartBuilding();
                        Core.currentScreen = Core.colonyScreen;
                    }
                }
            }
        }

        public void Draw(bool showWorkers, bool showTask, SpriteBatch spriteBatch)
        {
            if (!showTask && !showWorkers)
            {
                spriteBatch.Draw(texture, position, Color.White);

                informationBox.Draw(spriteBatch);

                if(priceBox != null)
                    priceBox.Draw(spriteBatch);
            }
        }
    }
}
