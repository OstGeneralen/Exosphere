using Exosphere.Src.Generators;
using Exosphere.Src.Handlers;
using Exosphere.Src.HUD;
using Exosphere.Src.ResearchProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Basebuilding.Facilities
{
    public class Lab : Facility
    {
        //Your current research project
        Exosphere.Src.ResearchProject.Research currentlyResearching;

        #region Load/Save
        public override void LoadFacility(LabSave load)
        {
            this.position = load.position;

            this.colonyID = load.colonyID;

            this.level = load.level;

            this.storageLevel = load.storageLevel;

            this.housingLimit = load.housingLimit;

            this.finished = load.finished;

            this.timeUnderConstruction = load.timeUnderConstruction;

            this.currentlyResearching = load.currentlyResearching;

            this.ID = load.ID;

            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/LabPH");

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
            labSave.position = position;
            labSave.colonyID = colonyID;
            labSave.level = level;
            labSave.storageLevel = storageLevel;
            labSave.housingLimit = housingLimit;
            labSave.finished = finished;
            labSave.timeUnderConstruction = timeUnderConstruction;
            labSave.currentlyResearching = currentlyResearching;
            labSave.ID = ID;
        }
        #endregion


        /// <summary>
        /// Implements a new lab into the game
        /// </summary>
        /// <param name="position">The lab's position in pixels</param>
        /// <param name="colony">The colony the lab recides in</param>
        public Lab(Vector2 position, Colony colony, int ID)
            : base(position, colony, ID)
        {
            //Creates the list that contains possible research projects

            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/LabPH");

            facilityType = "Lab";

            CreateFacilityButton();

            finished = true;

            taskScreen = new LabTaskScreen(this, colony);

            level = 1;

            costCopper = 1000;
            costIron = 800;
            costCarbon = 600;

            copperValue = 5.25f;
            carbonValue = 2.5f;
            ironValue = 4.25f;

            maxLevel = 5;

            requiredEnergy = 10000;

            SetDebugValues();
        }

        /// <summary>
        /// Gets the lab's facility type(in other words lab)
        /// </summary>
        /// <returns>Returns the facility type(Lab)</returns>
        public override string GetFacilityType()
        {
            return base.GetFacilityType();
        }

        /// <summary>
        /// Sets the current research project to a project selected by the player
        /// </summary>
        /// <param name="research">The project you wish to research</param>
        public void SetCurrentResearch(Research research)
        {
            currentlyResearching = research;
        }

        /// <summary>
        /// Performs the lab's task
        /// </summary>
        /// <param name="text">Should be represented by null</param>
        public override void Task(string text)
        {
            //An int containing the amount of researchpoints provided by the entire scientist team
            int researchPoints = 0;
            //A float containing the amount of researchpoints provided by every individual scientist
            float unfinishedResearchPoints = 0;

            //Checks if your currently working with a project
            if (currentlyResearching != null)
            {
                //Loops through all scientists
                foreach (Colonist scientist in workers)
                {
                    //Scientists with efficiency under 10% generates no research points

                    unfinishedResearchPoints += Research(scientist);
                }

                researchPoints += (int)unfinishedResearchPoints;

                Console.WriteLine(researchPoints);
                //Adds research points to the project
                currentlyResearching.researchPoints += researchPoints;

                //Checks if the projects has been finished
                if (currentlyResearching.Task())
                {
                    //Adds new possible projects
                    colony.canResearch = currentlyResearching.AddResearch(currentlyResearching, colony.canResearch, colony.ID);
                    //Adds the finished project to a list over other completed assignments
                    colony.research.Add(currentlyResearching);
                    colony.researchDictionary[currentlyResearching.GetResearchType()] = currentlyResearching;

                    colony.canResearch.Remove(currentlyResearching);
                    currentlyResearching = null;

                    Console.WriteLine("Finished");
                }
            }

        }

        public override void TaskScreenUpdate()
        {
            base.TaskScreenUpdate();
        }

        /// <summary>
        /// Calculates how many research points the scientist produces
        /// </summary>
        /// <param name="scientist">The working scientist</param>
        /// <returns>Returns the amout of research points the scientist produced</returns>
        private float Research(Colonist scientist)
        {
            if (scientist.efficiency > 10)
            {
                //Generates research points
                return (float)scientist.intelligence / 10 * GetFacilityEfficiency() *
                   colony.researchDictionary["MoreEfficientResearch"].GetNewPercentage() *
                   scientist.GetProficiency(this) * scientist.GetHealthBasedEfficiency();
            }
            return 0;
        }

        public override string Information()
        {
            if (currentlyResearching != null)
                return "Currently Researching: " + currentlyResearching.GetResearchName();
            else
                return "No active research project";
        }
    }

    class LabTaskScreen : FacilityTaskScreen
    {
        Lab lab;
        List<Button> researchButtons;
        Colony colony;
        Research research;
        ScrollBox scrollBox;

        public LabTaskScreen(Lab lab, Colony colony)
        {
            name = "Lab";

            this.lab = lab;
            this.colony = colony;

            researchButtons = new List<Button>();
        }

        private void CreateButtons()
        {
            researchButtons.Clear();

            foreach (var research in colony.canResearch)
            {
                if (research.CanResearch(lab, research))
                {
                    researchButtons.Add(new ResearchButton(research, lab));
                }
            }

            scrollBox = new ScrollBox(researchButtons);
        }

        public override void Update()
        {
            if (researchButtons.Count == 0)
            {
                CreateButtons();
            }

            scrollBox.Update();

            foreach (var button in researchButtons)
            {
                ResearchButton tempButton = (ResearchButton)button;
                if (tempButton.Collision())
                {
                    tempButton.ChangeColor();
                    research = tempButton.GetResearch();

                    ChoiceBox choiceBox = new ChoiceBox(2, "Are you sure you want to research " + research.GetResearchName());
                    Core.currentMessageBox = choiceBox;
                }
            }

            if (Core.choiceBoxChoice && research != null)
            {
                lab.SetCurrentResearch(research);
                researchButtons.Clear();
                Core.facilityScreen.showTask = false;
                Core.choiceBoxChoice = false;
                research = null;
            }

            if (HUD.ActionButtons.backButton.Collision())
            {

            }

            if (TimeHandler.newTurn)
                CreateButtons();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            scrollBox.Draw(spriteBatch);
        }
    }

    class ResearchButton : ActivatableButton
    {
        Research research;
        Lab lab;
        InfoBox infoBox;

        public ResearchButton(Research research, Lab lab)
            : base("Res/PH//HUD/Buttons/Standard/ButtonUpPH", Vector2.Zero, research.GetResearchName())
        {
            this.lab = lab;

            this.research = research;

            infoBox = new InfoBox(1, research.Information());
        }

        public Research GetResearch()
        {
            return research;
        }

        public override void Update()
        {
            base.Update();

            if (Cursor.collision.Intersects(collision))
            {
                infoBox.Update();
                infoBox.OverridePosition(new Vector2(position.X + texture.Width, position.Y));
                //infoBox.OverrideMessage(research.Information());
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (Cursor.collision.Intersects(collision))
                infoBox.Draw(spriteBatch);
        }
    }

    public struct LabSave
    {
        public Vector2 position;

        public int colonyID;

        public int level;

        public int storageLevel;

        public int housingLimit;

        public bool finished;

        public int timeUnderConstruction;

        public int ID;

        public Research currentlyResearching;
    }
}
