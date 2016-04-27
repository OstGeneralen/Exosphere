using Exosphere.Src.Generators;
using Exosphere.Src.Handlers;
using Exosphere.Src.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Basebuilding.Facilities
{
    class Workshop : Facility
    {
        //Represents the base production in the workshop
        float productivity;

        //Represents the work every individual worker performs
        int assemblingPoints;

        //The object currently under assembling
        Item currentProject;


        #region Load/Save

        public override void LoadFacility(WorkshopSave load)
        {
            this.position = load.position;

            this.colonyID = load.colonyID;

            this.level = load.level;

            this.storageLevel = load.storageLevel;

            this.housingLimit = load.housingLimit;

            this.finished = load.finished;

            this.timeUnderConstruction = load.timeUnderConstruction;

            this.ID = load.ID;

            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/SolarPanelsPH");

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
            workshopSave.position = position;
            workshopSave.colonyID = colonyID;
            workshopSave.level = level;
            workshopSave.storageLevel = storageLevel;
            workshopSave.housingLimit = housingLimit;
            workshopSave.finished = finished;
            workshopSave.timeUnderConstruction = timeUnderConstruction;
            workshopSave.ID = ID;
        }
        #endregion

        public Workshop(Vector2 position, Colony colony, int ID)
            : base(position, colony, ID)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/WorkshopPH");

            facilityType = "Workshop";

            finished = true;

            productivity = 1.5f;

            costCopper = 900;
            costIron = 1200;
            costCarbon = 800;

            copperValue = 4.3f;
            carbonValue = 3.35f;
            ironValue = 4.35f;

            requiredEnergy = 9500;

            maxLevel = 3;

            CreateFacilityButton();

            taskScreen = new WorkshopTaskScreen(this, colony);

            SetDebugValues();
        }

        public override void Guide()
        {
            base.Guide();
        }

        /// <summary>
        /// Performs the workshops task(assembles equipment)
        /// </summary>
        /// <param name="text">Should contain null or ""</param>
        public override void Task(string text)
        {
            //Loops through all workers
            foreach (var colonist in workers)
            {

                if (currentProject != null)
                {
                    //The worker assembles
                    currentProject.SetAssemblingPoints(Assembling(colonist));

                    //Checks if the assembling is finished
                    if (currentProject.AssemblingFinished())
                    {
                        //Adds the built vehicle in the colony's vehicle list
                        colony.items.Add(currentProject);
                        currentProject = null;
                    }
                }
            }
        }

        /// <summary>
        /// Calculates how much work the individual worker performs
        /// </summary>
        /// <param name="colonist">The person performing the task</param>
        /// <returns>Returns the amount of work performed represented by an int value</returns>
        private int Assembling(Colonist colonist)
        {
            float intelligencePerAssemblingPoint = 5;

            //Checks if the worker's efficiency is less then 10
            if (colonist.efficiency < 10)
            {
                //Sets assemblingpoints to 0(makes sure he does nothing)
                assemblingPoints = 0;
            }
            else
            {
                //Calculates how much work the worker gets done
                assemblingPoints = (int)((float)(colonist.intelligence) * productivity /
                    intelligencePerAssemblingPoint * GetFacilityEfficiency() *
                    colonist.GetProficiency(this) * colonist.GetHealthBasedEfficiency() *
                    colony.researchDictionary["QuickerAssembling"].GetNewPercentage());
            }
            return assemblingPoints;
        }

        /// <summary>
        /// Sets the current project
        /// </summary>
        /// <param name="item">The item you will start to assemble</param>
        public void SetCurrentProject(Item item)
        {
            currentProject = item;
        }

        /// <summary>
        /// Gets the facility type
        /// </summary>
        /// <returns>Returns the facility type</returns>
        public override string GetFacilityType()
        {
            return base.GetFacilityType();
        }
    }

    class WorkshopTaskScreen : FacilityTaskScreen
    {
        Workshop workshop;
        List<AssemblingButton> assemblingButtons;
        Colony colony;

        /// <summary>
        /// Creates a task screen for the workshop
        /// </summary>
        /// <param name="workshop">The workshop which task screen will be shown</param>
        /// <param name="colony">The colony in which the workshop resides</param>
        public WorkshopTaskScreen(Workshop workshop, Colony colony)
        {

            name = "Lab";

            this.workshop = workshop;
            this.colony = colony;

            assemblingButtons = new List<AssemblingButton>();

        }

        /// <summary>
        /// Creates the assembling buttons
        /// </summary>
        private void CreateButtons()
        {

            int amount = 0;

            //Clears the list containing the assembling buttons
            assemblingButtons.Clear();

            foreach (var assembling in colony.canAssemble)
            {
                assemblingButtons.Add(new AssemblingButton(amount, assembling));
                amount++;

            }
        }

        /// <summary>
        /// Updates the Workshop's task screen
        /// </summary>
        public override void Update()
        {
            foreach (var button in assemblingButtons)
            {
                //Updates the button
                button.Update();

                //Checks if you press an assembling button
                if (button.Collision())
                {
                    //Changes the current assembling project
                    workshop.SetCurrentProject(button.GetItem());
                }
            }

            //Creates the assembling buttons
            if (TimeHandler.newTurn)
                CreateButtons();
        }

        /// <summary>
        /// Draws the assembling buttons
        /// </summary>
        /// <param name="spriteBatch">Used to draw stuff on screen</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var button in assemblingButtons)
            {
                button.Draw(spriteBatch);
            }
        }
    }

    class AssemblingButton : Button
    {

        Item item;

        /// <summary>
        /// Creates an assemlingbutton
        /// </summary>
        /// <param name="amount">Contains the amount of buttons you want to add(in this case its representing the number)</param>
        /// <param name="item">The item which production will be represented by the button</param>
        public AssemblingButton(int amount, Item item)
            : base("Res/PH/HUD/Buttons/Standard/ButtonUpPH", Vector2.Zero, "")
        {

            position = SetPosition(amount);

            this.item = item;

            label = item.GetItemType();
        }

        /// <summary>
        /// Sets the button's position
        /// </summary>
        /// <returns>Returns the button's position in pixels</returns>
        private Vector2 SetPosition(int amount)
        {
            int x = 0;
            int y;


            y = HUD.HUD.informationList.GetTexture().Height + texture.Height * amount;
            x = (int)(Settings.GetScreenRes.X / 2 - texture.Width / 2);


            return new Vector2(x, y);

        }

        public Item GetItem()
        {
            return item;
        }

        /// <summary>
        /// Updates the assembling button
        /// </summary>
        public override void Update()
        {
            base.Update();

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }

    public struct WorkshopSave
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
