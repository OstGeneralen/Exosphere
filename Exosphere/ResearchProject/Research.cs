using Exosphere.Src.Basebuilding.Facilities;
using Exosphere.Src.Generators;
using Exosphere.Src.ResearchProject.Buffs;
using Exosphere.Src.ResearchProject.FacilitiesResearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject
{
    public abstract class Research
    {
        #region Variables All

        #region Requirement

        //The intelligence needed to research this research
        protected int intelligenceRequirement;

        //Represents the intelligenceRequirement-increase in percentage for research projects
        protected float intelligenceRequirementValue;

        //An int containing the value which the level of the lab must equal or exceed in order to perform the research
        protected int labRequirement;

        //Represents the labRequirement-increase in percentage for research projects
        protected int labRequirementValue;

        #endregion

        #region Level

        //Represents the amount of times you have researched a certain button
        protected int timesResearched;

        //Represents max-level
        protected int maximumLevel;

        #endregion

        #region Type

        //A string that represents the research-type
        protected string researchType;

        //A string containing the name of the research
        protected string name;

        //An int that identifies the research
        protected int identification;

        //A key used to identify the category for the research
        protected string researchCategory;

        #endregion

        #region Cost

        //The amount of research points needed to research this research
        public int researchPoints;

        //The amount of research points needed to finish a project
        protected int cost;

        //Represents the cost-increase in percentage for research projects
        protected float costValue;

        #endregion

        #region ID

        public int colonyID;

        #endregion

        #endregion

        #region Save/Load

        public ResearchSave save;

        #region Load Research

        public void LoadResearch(ResearchSave load)
        {
            cost = load.cost;
            researchPoints = load.researchPoints;
            identification = load.identification;
            name = load.name;
            timesResearched = load.timesResearched;
            colonyID = load.colonyID;
            researchType = load.researchType;
        }

        #endregion

        public void SaveResearch()
        {
            save.cost = cost;
            save.researchPoints = researchPoints;
            save.identification = identification;
            save.name = name;
            save.timesResearched = timesResearched;
            save.colonyID = colonyID;
            save.researchType = researchType;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Start a new research
        /// </summary>
        public Research(int colonyID, int timesResearched = 0)
        {
            //Sets the standard value for all variables
            SetStandardValues(colonyID);
            this.timesResearched = timesResearched;
        }

        /// <summary>
        /// Sets the variables standard values
        /// </summary>
        private void SetStandardValues(int colonyID)
        {
            //Sets the standard values
            researchPoints = 0;
            intelligenceRequirementValue = 0;
            intelligenceRequirement = 0;
            labRequirement = 1;
            labRequirementValue = 1;

            cost = 0;
            costValue = 20;
            this.colonyID = colonyID;
            name = "Standard Research";
        }

        #endregion

        #region Task

        /// <summary>
        /// Checks if the research project is finished
        /// </summary>
        /// <returns>Returns true for finished projects, false for unfinished projects</returns>
        public virtual bool Task()
        {
            bool researchDone = false;
            if (researchPoints >= GetCost())
            {
                researchDone = true;
                researchPoints = 0;
            }
            return researchDone;
        }

        /// <summary>
        /// Checks if you have researched all projects
        /// </summary>
        /// <returns>Returns a list containing potential research choices</returns>
        public List<Research> AddResearch(Research research, List<Research> canResearch, int colonyID)
        {
            //Checks if you have enough intelligence, if your lab has the sufficient level to research the next facilitylevel
            //There will be an exception if you have already reached max level
            if (maximumLevel >= timesResearched)
            {
                //Adds the next possible research-project in the list of possible research-projects
                switch (research.GetResearchType())
                {
                    #region Facilities

                    #region Stores

                    case "FacilityResearch":
                        canResearch.Add(new FacilityResearch(colonyID, (research.GetTimesResearched() + 1)));
                        break;

                    case "MineralStoreResearch":
                        canResearch.Add(new MineralStoreResearch(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "FoodStoreResearch":
                        canResearch.Add(new FoodStoreResearch(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "EquipmentStoreResearch":
                        canResearch.Add(new EquipmentStoreResearch(colonyID, research.GetTimesResearched() + 1));
                        break;

                    #endregion

                    #region Stats

                    case "LibraryResearch":
                        canResearch.Add(new LibraryResearch(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "GymResearch":
                        canResearch.Add(new GymResearch(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "MedBayResearch":
                        canResearch.Add(new MedBayResearch(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "EvaluationFacilityResearch":
                        canResearch.Add(new EvaluationFacilityResearch(colonyID, research.GetTimesResearched() + 1));
                        break;

                    #endregion

                    #region Worker Facilities

                    case "GreenhouseResearch":
                        canResearch.Add(new GreenhouseResearch(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "MineResearch":
                        canResearch.Add(new MineResearch(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "HangarResearch":
                        canResearch.Add(new HangarResearch(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "LabResearch":
                        canResearch.Add(new LabResearch(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "RefineryResearch":
                        canResearch.Add(new RefineryResearch(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "WorkshopResearch":
                        canResearch.Add(new FoodStoreResearch(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "GarageResearch":
                        canResearch.Add(new GarageResearch(colonyID, research.GetTimesResearched() + 1));
                        break;

                    #endregion

                    #region Misc

                    case "LivingQuartersResearch":
                        canResearch.Add(new LivingQuartersResearch(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "SolarPanelsResearch":
                        canResearch.Add(new SolarPanelsResearch(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "ComArrayResearch":
                        canResearch.Add(new ComArrayResearch(colonyID, research.GetTimesResearched() + 1));
                        break;

                    #endregion

                    #endregion

                    #region Buffs

                    #region Stores

                    case "LargerEquipmentStorage":
                        canResearch.Add(new LargerEquipmentStorage(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "LargerFoodStorage":
                        canResearch.Add(new LargerFoodStorage(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "LargerMineralStorage":
                        canResearch.Add(new LargerMineralStorage(colonyID, research.GetTimesResearched() + 1));
                        break;

                    #endregion

                    #region More Productivity

                    case "HigherEnergyOutput":
                        canResearch.Add(new HigherEnergyOutput(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "MoreEfficientExtraction":
                        canResearch.Add(new MoreEfficientExtraction(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "MoreEfficientResearch":
                        canResearch.Add(new MoreEfficientResearch(colonyID, research.GetTimesResearched() + 1));
                        break;

                    #endregion

                    #region Less Costs

                    case "LessConstructionCost":
                        canResearch.Add(new LessConstructionCost(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "LessEnergyConsumption":
                        canResearch.Add(new LessEnergyConsumption(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "LessFoodConsumption":
                        canResearch.Add(new LessFoodConsumption(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "LessMaintenanceCost":
                        canResearch.Add(new LessMaintenanceCost(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "LessWaterConsumption":
                        canResearch.Add(new LessWaterConsumption(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "ReducedAssemblingCost":
                        canResearch.Add(new ReducedAssemblingCost(colonyID, research.GetTimesResearched() + 1));
                        break;

                    #endregion

                    #region More Efficiency

                    case "MoreEfficientIntelligenceTraining":
                        canResearch.Add(new MoreEfficientIntelligenceTraining(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "MoreEfficientStrengthTraining":
                        canResearch.Add(new MoreEfficientStrengthTraining(colonyID, research.GetTimesResearched() + 1));
                        break;

                    case "QuickerAssembling":
                        canResearch.Add(new QuickerAssembling(colonyID, research.GetTimesResearched() + 1));
                        break;

                    #endregion

                    #endregion

                    default:
                        break;
                }
            }
            //Returns the list with all possible research-options
            return canResearch;
        }

        /// <summary>
        /// Checks if the lab and it's workers meets the requirements for researching a specific research
        /// </summary>
        /// <param name="lab"></param>
        /// <returns></returns>
        public bool CanResearch(Lab lab, Research research)
        {
            if (lab.level >= research.GetLabRequirement())
            {
                foreach (var colonist in lab.GetWorkers())
                {
                    if (colonist.intelligence >= research.GetIntelligenceRequirement())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region GetInformation

        /// <summary>
        /// Gets the cost in research points to comlete the research project
        /// </summary>
        /// <returns>Returns the cost</returns>
        private int GetCost()
        {
            return (int)(cost * Math.Pow(costValue, timesResearched));
        }

        /// <summary>
        /// Checks if you meet the requirements needed to research a certain project
        /// </summary>
        /// <returns>Returns true if you meet the requirements, else false</returns>
        private int GetIntelligenceRequirement()
        {
            return (int)(intelligenceRequirement * Math.Pow(intelligenceRequirementValue, timesResearched));
        }

        /// <summary>
        /// Checks if you meet the requirements needed to research a certain project
        /// </summary>
        /// <returns>Returns true if you meet the requirements, else false</returns>
        private int GetLabRequirement()
        {
            return (int)(labRequirement + (labRequirementValue * timesResearched));
        }

        /// <summary>
        /// Gets the amount of times you have researched a certain area
        /// </summary>
        /// <returns>Returns the amount of times you have researched a certain area</returns>
        public int GetTimesResearched()
        {
            return timesResearched;
        }

        /// <summary>
        /// Gets the research-type
        /// </summary>
        /// <returns>The research-type</returns>
        public string GetResearchType()
        {
            return researchType;
        }

        public virtual string Information()
        {
            return "This information is pointless";
        }

        #endregion

        #region Buff

        /// <summary>
        /// Gets the percentage change factor
        /// </summary>
        /// <returns>Returns the percentage change factor</returns>
        public virtual float GetNewPercentage()
        {
            return 0;
        }

        #endregion

        #region Disease

        /// <summary>
        /// Gets the name of the research
        /// </summary>
        /// <returns>Returns the name of the research</returns>
        public virtual string GetResearchName()
        {
            return name;
        }

        /// <summary>
        /// Gets the value that represents the research
        /// </summary>
        /// <returns>Returns the value that represents the research</returns>
        public virtual int GetIdentification()
        {
            return identification;
        }

        #endregion

        #region CategoryCheck

        /// <summary>
        /// Checks if the current research is under the vehicle category
        /// </summary>
        /// <returns>Returns true the current research is under the vehicle category</returns>
        public bool CheckIfVehicle()
        {
            if (researchCategory == "Vehicle")
                return true;
            return false;
        }

        #endregion
    }

    public struct ResearchSave
    {
        public int timesResearched;

        public string name;

        public int identification;

        public int researchPoints;

        public int cost;

        public int colonyID;

        public string researchType;
    }
}
