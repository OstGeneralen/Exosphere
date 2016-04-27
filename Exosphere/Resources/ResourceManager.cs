using Exosphere.Src.Basebuilding;
using Exosphere.Src.Basebuilding.Facilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Resources
{
    public class ResourceManager
    {
        #region Variables

        #region Resource Amount
        //Represent your amount of Iron
        public int iron;
        //Represent your amount of Copper
        public int copper;
        //Represent your amount of Carbon
        public int carbon;
        //Represent your amount of Clearwater
        public int clearwater;
        //Represent your amount of Food
        public int food;
        #endregion

        #region Resource Cost
        //Represents the cost in iron to perform an action
        int costIron;
        //Represents the cost in copper to perform an action
        int costCopper;
        //Represents the cost in carbon to perform an action
        int costCarbon;
        #endregion

        #region Storage
        //The maximum storagespace in the base
        int mineralStorageSpace;
        int foodStorageSpace;

        //The basic storage space
        int baseStorageMinerals;
        int baseStorageFoodAndWater;
        #endregion

        #region Consumption
        int waterConsumption;
        int foodConsumption;
        #endregion

        #endregion

        #region Save/Load

        public ResourceManagerSave save;

        #region Load ResourceManager

        public void LoadResourceManager(ResourceManagerSave load)
        {
            carbon = load.carbon;
            clearwater = load.clearwater;
            copper = load.copper;
            food = load.food;
            iron = load.iron;
        }

        #endregion

        public void SaveResourceManager()
        {
            save.carbon = carbon;
            save.clearwater = clearwater;
            save.copper = copper;
            save.food = food;
            save.iron = iron;
        }

        #endregion

        /// <summary>
        /// Implements a resource manager
        /// </summary>
        public ResourceManager()
        {

            iron = 0;
            copper = 0;
            carbon = 0;
            clearwater = 500;
            food = 500;
            baseStorageMinerals = 300;
            baseStorageFoodAndWater = 500;
            mineralStorageSpace = baseStorageMinerals;
            foodStorageSpace = baseStorageFoodAndWater;
        }

        #region Expenditure

        /// <summary>
        /// Checks if you have enough resources to build your choosen facility
        /// </summary>
        /// <param name="facility">Checks what kind of facility your about to construct</param>
        /// <returns>True if you can build the facility, false if you can't</returns>
        public bool BuildFacility(Facility facility)
        {


            //TODO: FIX THESE
            costIron = facility.GetCostIron();
            costCopper = facility.GetCostCopper();
            costCarbon = facility.GetCostCarbon();

            if (CanAfford())
            {
                SubtractResources();
                return true;
            }

            return false;

        }

        /// <summary>
        /// Checks if you have enough resources to afford the current task
        /// </summary>
        /// <returns>Returns true if the amount of resources if sufficient, else false</returns>
        private bool CanAfford()
        {
            if (copper >= costCopper && iron >= costIron && carbon >= costCarbon)
                return true;

            return false;
        }

        /// <summary>
        /// Substracts your expenses from your total amount of resources
        /// </summary>
        private void SubtractResources()
        {
            copper -= costCopper;
            iron -= costIron;
            carbon -= costCarbon;
        }

        /// <summary>
        /// Calculates and subtracts your food/water consumption from your total amount of food/water
        /// </summary>
        /// <param name="colony">The colony containing information of its total consumption each turn</param>
        public void FoodConsumption(Colony colony)
        {
            //Makes sure every colonist consumes food
            foreach (var colonist in colony.GetInhabitants())
            {
                #region Starvation
                //Checks if there is enough food left in the stores to satisfy the colonist
                if (food >= colonist.GetFoodConsumption())
                    //Consumes the food equal to his appetite
                    food -= colonist.GetFoodConsumption();

                else if (food < colonist.GetFoodConsumption() && food > 0)
                {
                    int remainingHunger = food - colonist.GetFoodConsumption();
                    food -= food;
                    //The colonist gets weakened from not getting enough food
                    colonist.health += remainingHunger;
                }

                if (food <= 0)
                    //The colonist starts to starve
                    colonist.health -= colonist.GetFoodConsumption();
                #endregion

                #region Dehydration
                //Checks if there is enough clearwater left in the stores to satisfy the colonist
                if (clearwater >= colonist.GetWaterConsumption())
                    //Consumes the clearwater equal to his thirst
                    clearwater -= colonist.GetWaterConsumption();
                else if (clearwater < colonist.GetWaterConsumption() && clearwater > 0)
                {
                    int remainingThirst = clearwater - colonist.GetWaterConsumption();
                    clearwater -= clearwater;
                    //The colonist gets weakened from not getting enough clearwater
                    colonist.health += remainingThirst;
                }
                if (clearwater <= 0)
                    //The colonist starts to exseccate
                    colonist.health -= colonist.GetWaterConsumption();
                #endregion
            }
        }

        #endregion

        #region Revenues

        /// <summary>
        /// Calculates how much resources the mine extracts that gets stored
        /// </summary>
        /// <param name="facility">The facility that performs the extraction(Mine)</param>
        public void Extraction(Facility facility)
        {
            if (facility != null)
            {
                if (facility.GetFacilityType() == "Mine")
                {
                    iron = facility.Task(iron, "Iron");
                    copper = facility.Task(copper, "Copper");
                    carbon = facility.Task(carbon, "Carbon");
                }
                if (facility.GetFacilityType() == "Pump")
                {
                    clearwater = facility.Task(clearwater);
                }
            }
        }

        #endregion

        public int GetFoodConsumptionPerDay(Colony colony)
        {
            int totalConsumption = 0;

            foreach (var colonist in colony.GetInhabitants())
            {
                totalConsumption += colonist.GetFoodConsumption();
            }

            return totalConsumption;
        }

        public int GetWaterConsumptionPerDay(Colony colony)
        {
            int totalConsumption = 0;

            foreach (var colonist in colony.GetInhabitants())
            {
                totalConsumption += colonist.GetWaterConsumption();
            }

            return totalConsumption;
        }

        #region Storage

        /// <summary>
        /// Checks the colony's storagespace and removes supernumerary resources
        /// </summary>
        /// <param name="grid">The grid containing the storagefacilities</param>
        /// <param name="facility">Holds information of what kind of facility your checking</param>
        public void Storageroom(Grid grid, Facility facility)
        {
            if (facility != null)
                if (facility.GetFacilityType() == "MineralStores")
                    //Checks how much minerals you can store
                    mineralStorageSpace = (int)((float)(grid.GetFacilityAmount("MineralStores") * facility.Task() + baseStorageMinerals) * grid.colony.researchDictionary["LargerMineralStorage"].GetNewPercentage());

            if (facility != null)
                if (facility.GetFacilityType() == "FoodStores")
                    //Checks how much food and clearwater you can store
                    foodStorageSpace = (int)((float)(grid.GetFacilityAmount("FoodStores") * facility.Task() + baseStorageFoodAndWater) * grid.colony.researchDictionary["LargerFoodStorage"].GetNewPercentage());


            RemoveSupernumeraryResources();
        }

        /// <summary>
        /// Removes the supernumerary resources that don't fit in your stores
        /// </summary>
        private void RemoveSupernumeraryResources()
        {
            //Tosses away minerals if you don't have space to store them
            if (iron >= mineralStorageSpace)
                iron = mineralStorageSpace;
            if (copper >= mineralStorageSpace)
                copper = mineralStorageSpace;
            if (carbon >= mineralStorageSpace)
                carbon = mineralStorageSpace;

            //Tosses away food and clearwater if you don't have space to store them
            if (clearwater >= foodStorageSpace)
                clearwater = foodStorageSpace;
            if (food >= foodStorageSpace)
                food = foodStorageSpace;
        }

        #endregion
    }

    public struct ResourceManagerSave
    {
        public int iron;
        public int copper;
        public int carbon;
        public int clearwater;
        public int food;
    }
}
