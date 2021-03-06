﻿using Exosphere.Src.Basebuilding;
using Exosphere.Src.Basebuilding.Facilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Resources
{
    public class EnergyManager
    {
        //A variable for the total amount of energy that is generated for the colony
        int totalEnergy;

        //A variable for the amount of energy that is not used
        public int freeEnergy;

        //A variable for the amount of energy that is used
        int usedEnergy;

        #region Save/Load

        public EnergyManagerSave save;

        #region Load EnergyManager

        public void LoadEnergyManager(EnergyManagerSave load)
        {
            totalEnergy = load.totalEnergy;
            freeEnergy = load.freeEnergy;
            usedEnergy = load.usedEnergy;
        }

        #endregion

        public void SaveEnergyManager()
        {
            save.totalEnergy = totalEnergy;
            save.freeEnergy = freeEnergy;
            save.usedEnergy = usedEnergy;
        }

        #endregion

        /// <summary>
        /// Creates a new Energy manager. Do this once per colony
        /// </summary>
        public EnergyManager()
        {
            //TODO: Remove this and make the base start with one solar panel
            //totalEnergy = 100;
        }

        /// <summary>
        /// Checks if there is enough free poser in the colony to build the specified facility
        /// </summary>
        /// <param name="facility">The facility that needs checking if it can be built</param>
        /// <returns>Returns true if the colony has enough power</returns>
        public bool BuildFacility(Facility facility)
        {
            //Sets the free energy to equal the total energy minus the used
            freeEnergy = totalEnergy - usedEnergy;

            //Checks if there is enough free energy to build the facility
            if (freeEnergy >= facility.GetCostEnergy())
            {
                //Removes the energy cost from free energy as it is now used
                freeEnergy -= facility.GetCostEnergy();
                //Adds the energy cost to used energy as it is now used
                usedEnergy += facility.GetCostEnergy();

                return true;
            }

            return false;
        }

        public void AddUsedPower(int amount)
        {
            usedEnergy += amount;
        }

        /// <summary>
        /// Sets the total energy based on the amount of power generating facilities built in the colony
        /// </summary>
        /// <param name="grid">The grid of the current colony</param>
        /// <param name="facility">The facility currently in loop</param>
        public void SetTotalEnergy(Grid grid, Facility facility)
        {
            //Checks so that the facility is not null
            if (facility != null)
            {
                //Checks so that the facility is a power generating sort. In this case Solar Panels
                if (facility.GetFacilityType() == "SolarPanels")
                {
                    //Sets total energy to be the total amount of solar panels on the base * the power generated by a single solar panel
                    totalEnergy = grid.GetFacilityAmount("SolarPanels") * facility.Task(0);
                    freeEnergy = totalEnergy - usedEnergy;
                }
            }
        }
    }

    public struct EnergyManagerSave
    {
        public int totalEnergy;
        public int freeEnergy;
        public int usedEnergy;
    }
}
