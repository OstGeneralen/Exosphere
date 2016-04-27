using Exosphere.Src.Basebuilding.Facilities;
using Exosphere.Src.Generators;
using Exosphere.Src.Handlers;
using Exosphere.Src.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Basebuilding
{
    public abstract class Facility
    {
        #region Variables
        #region Basic variables
        //The position of the facility
        protected Vector2 position;

        //The texture of the facility
        protected Texture2D texture;

        //A button created for the facility
        protected FacilityButton facilityButton;
        #endregion

        //List containing potential workers
        protected List<Colonist> workers;

        //The colony the facility resides in
        protected Colony colony;

        public int colonyID;

        public int ID;

        #region Requirements
        //Represents the intelligence-requirement for performing a specific task
        public int intelligenceRequirement;
        //Represents the strength-requirement for performing a specific task
        public int strengthRequirement;
        //Represents the immunesystem-requirement for performing a specific task
        public int immuneSystemRequirement;
        //Represents the health-requirement for performing a specific task
        public int healthRequirement;
        #endregion

        public FacilityTaskScreen taskScreen;

        #region Teamwork
        //The minimum efficiency requirement needed to eaceed good teamwork from the perspective of every single worker
        private int goodTeamworkStandard;
        //The efficiency limit every worker need to fall short of in order to for it to count as bad teamwork
        private int badTeamworkStandard;
        //Represents the amount of workers it takes to form a team
        private int amountOfTeamworkers;
        //Represents the avarage efficiency of a facility
        protected int normalEfficiency;
        //Represents a value that you substract from the facility's efficiency
        private int efficiencyDeduction;
        //Represents a value that you add to the facility's efficiency
        private int efficiencyBonus;
        #endregion

        #region Cost

        #region Construction/Upgrade
        //Represents the facility's cost in copper needed to construct it
        protected int costCopper;
        //Represents the facility's cost in iron needed to construct it
        protected int costIron;
        //Represents the facility's cost in carbon needed to construct it
        protected int costCarbon;

        //Represents the copper cost-increase in percentage
        protected float copperValue;
        //Represents the iron cost-increase in percentage
        protected float ironValue;
        //Represents the carbon cost-increase in percentage
        protected float carbonValue;

        //Represents the facility's energy consumption
        protected int requiredEnergy;
        #endregion

        #region Maintenance
        //Represents the facility's maintenance cost in copper
        protected int copperMaintenance;
        //Represents the facility's maintenance cost in iron
        protected int ironMaintenance;
        //Represents the facility's maintenance cost in carbon
        protected int carbonMaintenance;
        //Checks if your facility is properly maintained
        private bool maintenance;
        #endregion

        #endregion

        #region Level

        //Represents the facility's level
        public int level;
        //Represents the facility's max level
        public int maxLevel;
        //Affects how many workers that fits in a facility
        public int storageLevel;

        //The maximum amount of colonists in every facility
        protected int housingLimit;

        //A string containing the facilityType
        protected string facilityType;

        #endregion

        #region Construction/Upgrade Time

        protected int constructionTime;
        protected int timeUnderConstruction;
        protected float constructionTimeValue;
        protected bool finished;
        protected bool finishedStorage;

        #endregion

        #endregion

        #region Save/Load

        #region Facility Saves

        public ComArraySave comArraySave;
        public EquipmentStoresSave equipmentStoresSave;
        public EvaluationFacilitySave evaluationFacilitySave;
        public FoodStoresSave foodStoresSave;
        public GarageSave garageSave;
        public GreenhouseSave greenhouseSave;
        public GymSave gymSave;
        public HangarSave hangarSave;
        public LabSave labSave;
        public LibrarySave librarySave;
        public LivingQuartersSave livingQuartersSave;
        public MedBaySave medBaySave;
        public MineSave mineSave;
        public MineralStoresSave mineralStoresSave;
        public RefinerySave refinerySave;
        public SolarPanelsSave solarPanelsSave;
        public WorkshopSave workshopSave;
        public PumpSave pumpSave;

        #endregion

        public virtual void LoadFacility(ComArraySave load)
        {

        }

        public virtual void LoadFacility(EquipmentStoresSave load)
        {

        }

        public virtual void LoadFacility(EvaluationFacilitySave load)
        {

        }

        public virtual void LoadFacility(FoodStoresSave load)
        {

        }

        public virtual void LoadFacility(GarageSave load)
        {

        }

        public virtual void LoadFacility(GreenhouseSave load)
        {

        }

        public virtual void LoadFacility(GymSave load)
        {

        }

        public virtual void LoadFacility(HangarSave load)
        {

        }

        public virtual void LoadFacility(LabSave load)
        {

        }

        public virtual void LoadFacility(LibrarySave load)
        {

        }

        public virtual void LoadFacility(LivingQuartersSave load)
        {

        }

        public virtual void LoadFacility(MedBaySave load)
        {

        }

        public virtual void LoadFacility(MineSave load)
        {

        }

        public virtual void LoadFacility(MineralStoresSave load)
        {

        }

        public virtual void LoadFacility(RefinerySave load)
        {

        }

        public virtual void LoadFacility(SolarPanelsSave load)
        {

        }

        public virtual void LoadFacility(WorkshopSave load)
        {

        }

        public virtual void LoadFacility(PumpSave load)
        {

        }

        public virtual void SaveFacility()
        {

        }

        #endregion

        public Facility()
        {

        }

        protected void SetDebugValues()
        {
            if (Game1.DEBUG_MODE)
            {
                costCarbon = 0;
                costCopper = 0;
                costIron = 0;
                requiredEnergy = 0;

                ironValue = 0;
                copperValue = 0;
                carbonValue = 0;
            }
        }
        /// <summary>
        /// Creates a new facility at the given pixel-position
        /// </summary>
        /// <param name="position">The position in pixels</param>
        public Facility(Vector2 position, Colony colony, int ID)
        {
            //Sets the position
            this.position = position;
            //Sets the facility type
            facilityType = "blank";

            timeUnderConstruction = 0;
            constructionTime = 5;
            constructionTimeValue = 1.5f;

            costCarbon = 5;
            costCopper = 5;
            costIron = 5;

            ironValue = 1.2f;
            copperValue = 1.2f;
            carbonValue = 1.2f;

            level = 1;

            #region Teamwork
            //Sets minimum efficiency requirement needed to exceed good teamwork from the perspective of every single worker
            goodTeamworkStandard = 75;
            //Sets fficiency limit every worker need to fall short of in order to for it to count as bad teamwork
            badTeamworkStandard = 50;
            //Sets the amount of workers needed to build a team
            amountOfTeamworkers = 10;
            //Sets the avarage facility efficiency
            normalEfficiency = 50;
            //Sets the deduction provided by bad teamwork
            efficiencyDeduction = -20;
            //Sets the bonus provided by good teamwork
            efficiencyBonus = 25;
            #endregion

            //Sets the housing limit
            housingLimit = 25;

            workers = new List<Colonist>();

            if (colony != null)
            {
                this.colony = colony;

                colonyID = colony.ID;
                this.ID = ID;
            }

            finished = true;
        }

        /// <summary>
        /// Creates a new button for the facility
        /// </summary>
        public void CreateFacilityButton()
        {
            facilityButton = new FacilityButton(position, texture);
        }

        public virtual string Information()
        {
            return "Standard Facility Message";
        }

        public virtual void Guide()
        {

        }

        #region FacilityScreen

        public virtual void TaskScreenUpdate()
        {
            taskScreen.Update();
        }

        public virtual void TaskScreenDraw(SpriteBatch spriteBatch)
        {
            taskScreen.Draw(spriteBatch);
        }

        #endregion

        #region Employ/Unemploy

        public bool CanWork(Colonist colonist)
        {
            if (colonist.strength >= strengthRequirement && colonist.intelligence >= intelligenceRequirement && colonist.immuneSystem >= immuneSystemRequirement && colonist.health >= healthRequirement)
                return true;

            return false;
        }

        public List<Colonist> GetWorkers()
        {
            return workers;
        }

        public void AddWorker(Colonist worker)
        {
            workers.Add(worker);
        }

        public void RemoveWorker(Colonist exWorker)
        {
            workers.Remove(exWorker);
        }

        #endregion

        #region Teamwork
        /// <summary>
        /// Calculates the efficiency of the facility based on its workers and their teamwork
        /// </summary>
        /// <returns>Returns the efficiency of the facility</returns>
        private float FacilityEfficiency()
        {
            //Contains the value of the facility efficiency
            int efficiency = 0;
            //Checks if the facility have actual workers
            if (workers.Count != 0)
            {
                //Checks every individual worker
                foreach (var worker in workers)
                {
                    //Stores all workers efficiency
                    efficiency += worker.efficiency;
                }
                //Calculates the average worker-efficiency of all the workers
                efficiency = efficiency / workers.Count;

                //Returns the overall-efficiency of the facility
                return efficiency + TeamWork();
            }
            else
            {
                //Returns null
                return efficiency + TeamWork();
            }
        }

        /// <summary>
        /// Controls if all workers have high respectively low efficiency and returns a bonus or a deduction if it turns out true
        /// </summary>
        /// <returns>Return a bonus or a deduction in facility's efficiency</returns>
        private float TeamWork()
        {
            //Contains the value of the teamwork's impact on the efficiency
            int bonusEfficiency = 0;
            //Used to check if all workers have good efficiency
            bool goodEfficiencyRequirement = true;
            //Used to check if all workers have bad efficiency
            bool badEfficiencyRequirement = true;

            //Checks if the facility have enough workers to form a team
            if (workers.Count >= amountOfTeamworkers)
            {
                //Checks every individual worker
                foreach (var worker in workers)
                {
                    //Check if the worker have high efficiency
                    if (worker.efficiency <= goodTeamworkStandard)
                        //If ONE worker is lazy, you won't receive a efficiencybonus
                        goodEfficiencyRequirement = false;
                    //Check if the worker have low efficiency
                    else if (worker.efficiency <= badTeamworkStandard)
                        //If ONE worker is active, you won't receive a efficiencydeduction
                        badEfficiencyRequirement = false;

                    if (goodEfficiencyRequirement)
                        //Gets a efficiencybonus
                        bonusEfficiency = efficiencyBonus;
                    else if (badEfficiencyRequirement)
                        //Gets a efficiencydeduction
                        bonusEfficiency = efficiencyDeduction;
                    else
                        break;
                }
                //Returns the efficiency(bonus or deduction) or the value null
                return bonusEfficiency;
            }
            //Returns null
            return bonusEfficiency;
        }

        /// <summary>
        /// Calculates how much more or less efficient the facility is compared to the average
        /// </summary>
        /// <returns>Returns the efficiency difference</returns>
        public float GetFacilityEfficiency()
        {

            float facilityEfficiency = FacilityEfficiency() / normalEfficiency;
            return facilityEfficiency;
        }
        #endregion

        #region Return costs
        public int GetCostIron()
        {
            return costIron;
        }

        public int GetCostCopper()
        {
            return costCopper;
        }

        public int GetCostCarbon()
        {
            return costCarbon;
        }

        public int GetCostEnergy()
        {
            return requiredEnergy;
        }
        #endregion

        #region Facility Basics
        /// <summary>
        /// Updates the facility
        /// </summary>
        /// <param name="cursor">The cursor used in the game</param>
        public void Update()
        {
            if (TimeHandler.newTurn)
            {
                switch (storageLevel)
                {
                    case 1:
                        housingLimit = 25;
                        break;
                    case 2:
                        housingLimit = 50;
                        break;
                    case 3:
                        housingLimit = 100;
                        break;
                    case 4:
                        housingLimit = 150;
                        break;
                    case 5:
                        housingLimit = 250;
                        break;
                    default:
                        break;
                }

                Finished();
            }
            //If the facility button is pressed the program runs Clicked()
            if (facilityButton.isClicked() && finished && !ColonyHUD.showFacilityChoices)
                Clicked();
        }

        /// <summary>
        /// Where is the player taken when he has pressed this specific facility?
        /// </summary>
        protected virtual void Clicked()
        {
            Core.SetFacilityView(this);
        }

        /// <summary>
        /// Represents each facility's individual application area
        /// </summary>
        /// <param name="text">The string should always have a value represented by "null"</param>
        public virtual void Task(string text)
        {
            List<Colonist> removeList = new List<Colonist>();
        }

        public virtual void Task(Grid grid)
        {

        }

        /// <summary>
        /// Represents each facility's individual application area
        /// </summary>
        public virtual int Task()
        {
            return 0;
        }

        public virtual int Task(int value, string name)
        {
            return 0;
        }

        /// <summary>
        /// Represents each facility's individual application area
        /// </summary>
        /// <param name="value">The data needed to perform the task</param>
        /// <returns>The result of the action</returns>
        public virtual int Task(int value)
        {

            List<Colonist> removeList = new List<Colonist>();

            foreach (var worker in workers)
            {
                if (!CanWork(worker))
                    removeList.Add(worker);
            }

            foreach (var exworker in removeList)
            {
                RemoveWorker(exworker);
            }

            return value;
        }

        /// <summary>
        /// Gets the type of the facility
        /// </summary>
        /// <returns>Returns the facility type</returns>
        public virtual string GetFacilityType()
        {
            return facilityType;
        }

        /// <summary>
        /// Draws the facility
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch yo use when drawing the facility</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        /// <summary>
        /// Gets information of which colony the facility resides in
        /// </summary>
        /// <returns>The colony the facility resides in</returns>
        public Colony GetColony()
        {
            return colony;
        }
        #endregion

        #region Maintenance

        /// <summary>
        /// Maintenances your facilities by paying maintenance costs
        /// </summary>
        /// <param name="iron">The amount of iron the colony has in storage</param>
        /// <param name="copper">The amount of copper the colony has in storage</param>
        /// <param name="carbon">The amount of carbon the colony has in storage</param>
        public bool Maintenance(int iron, int copper, int carbon)
        {
            if (EnoughResourcesToMaintain(iron, copper, carbon))
            {
                MaintenanceCost(iron, copper, carbon);
            }
            else
            {
                maintenance = false;
            }
            return maintenance;
        }

        /// <summary>
        /// Checks if you have enough resources to maintain
        /// </summary>
        /// <param name="iron">The amount of iron the colony has in storage</param>
        /// <param name="copper">The amount of copper the colony has in storage</param>
        /// <param name="carbon">The amount of carbon the colony has in storage</param>
        private bool EnoughResourcesToMaintain(int iron, int copper, int carbon)
        {
            if (iron >= ironMaintenance && copper >= copperMaintenance && carbon >= carbonMaintenance)
            {
                maintenance = true;
            }

            return maintenance;
        }

        /// <summary>
        /// Substracts your maintenance costs from your total amount of resources
        /// </summary>
        /// <param name="iron">The amount of iron the colony has in storage</param>
        /// <param name="copper">The amount of copper the colony has in storage</param>
        /// <param name="carbon">The amount of carbon the colony has in storage</param>
        private void MaintenanceCost(int iron, int copper, int carbon)
        {
            iron -= ironMaintenance;
            copper -= copperMaintenance;
            carbon -= carbonMaintenance;
        }

        #endregion

        #region Upgrade

        /// <summary>
        /// Upgrades the facility's storage possibilities
        /// </summary>
        public void UpgradeStorage()
        {
            if (EnoughResourcesToUpgrade())
            {
                UpgradeCost();
                storageLevel++;
            }
        }

        /// <summary>
        /// Upgrades the facility's functionality
        /// </summary>
        public bool Upgrade()
        {
            if (EnoughResourcesToUpgrade())
            {
                UpgradeCost();
                level++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Upgrades the facility's storage possibilities
        /// </summary>
        private bool EnoughResourcesToUpgrade()
        {
            bool enoughResources = false;

            if (GetUpgradeCostIron() <= colony.GetGrid().resourceManager.iron &&
                GetUpgradeCostCopper() <= colony.GetGrid().resourceManager.copper &&
                GetUpgradeCostCarbon() <= colony.GetGrid().resourceManager.carbon)
            {
                enoughResources = true;
            }

            return enoughResources;
        }

        /// <summary>
        /// Substracts your upgrade costs from your total amount of resources
        /// </summary>
        private void UpgradeCost()
        {
            colony.GetGrid().resourceManager.iron -= GetUpgradeCostIron();
            colony.GetGrid().resourceManager.copper -= GetUpgradeCostCopper();
            colony.GetGrid().resourceManager.carbon -= GetUpgradeCostCarbon();
        }

        /// <summary>
        /// Gets the iron storage upgrade cost
        /// </summary>
        /// <returns>Returns the iron upgrade cost</returns>
        public int GetUpgradeCostIron()
        {
            return (int)(costIron * Math.Pow(ironValue, level + 1));
        }

        /// <summary>
        /// Gets the copper storage upgrade cost
        /// </summary>
        /// <returns>Returns the copper upgrade cost</returns>
        public int GetUpgradeCostCopper()
        {
            return (int)(costCopper * Math.Pow(copperValue, level + 1));
        }

        /// <summary>
        /// Gets the copper storage upgrade cost
        /// </summary>
        /// <returns>Returns the copper upgrade cost</returns>
        public int GetUpgradeCostCarbon()
        {
            return (int)(costCarbon * Math.Pow(carbonValue, level + 1));
        }

        #endregion

        #region Construction/Upgrade Time

        /// <summary>
        /// Sets the bool finished to false
        /// </summary>
        public void StartBuilding()
        {
            finished = false;
        }

        /// <summary>
        /// Sets the bool finished to false
        /// </summary>
        public void StartUpgradeStorage()
        {
            finishedStorage = false;
        }

        /// <summary>
        /// Checks if the building is constructed/upgraded
        /// </summary>
        private void Finished()
        {
            if (!finished)
            {
                timeUnderConstruction++;
                if (ConstructionTime() <= timeUnderConstruction)
                {
                    finished = true;
                    timeUnderConstruction = 0;
                }
            }
        }

        /// <summary>
        /// Checks if the building's storage is expanded
        /// </summary>
        private void FinishedStorage()
        {
            if (!finishedStorage)
            {
                if (ConstructionTimeStorage() <= timeUnderConstruction)
                {
                    finished = true;
                    timeUnderConstruction = 0;
                }
            }
        }

        /// <summary>
        /// Calculates the time needed to construct/upgrade the facility
        /// </summary>i
        /// <returns>Returns the amount of days it will take to build the facility</returns>
        private int ConstructionTime()
        {
            return (int)(constructionTime * Math.Pow(constructionTimeValue, level));
        }

        /// <summary>
        /// Calculates the time needed to construct/upgrade the facility
        /// </summary>
        /// <returns>Returns the amount of days it will take to build the facility</returns>
        private int ConstructionTimeStorage()
        {
            return (int)(constructionTime * Math.Pow(constructionTimeValue, storageLevel));
        }

        #endregion
    }

    public class FacilityButton
    {
        //The collision rectangle for the facility
        Rectangle collision;

        /// <summary>
        /// Creates a new facility button
        /// </summary>
        /// <param name="position">The pixel-position of the facility</param>
        /// <param name="texture">The texture of the facility</param>
        public FacilityButton(Vector2 position, Texture2D texture)
        {
            //Sets the collision based on the position and dimensions of the texture
            collision = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        /// <summary>
        /// Checks if the button is pressed once
        /// </summary>
        /// <param name="cursor">The cursor used</param>
        /// <returns>True if the facility is clicked on</returns>
        public bool isClicked()
        {
            if (Cursor.collision.Intersects(collision) && MouseHandler.LMBOnce())
                return true;
            return false;
        }
    }

    public struct FacilitySave
    {

    }
}
