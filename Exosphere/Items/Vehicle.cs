using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Items
{
    public abstract class Vehicle : Item
    {
        #region Maintenance

        //Represents the vehicles's maintenance cost in copper
        protected int maintenanceCopper;

        //Represents the vehicles's maintenance cost in iron
        protected int maintenanceIron;

        //Represents the vehicles's maintenance cost in carbon
        protected int maintenanceCarbon;

        #endregion

        #region Vehicle Type

        //Represents the vehicle's type(ground or air)
        protected string vehicleType;

        //Represents the size of the vehicle
        protected string vehicleSize;

        #endregion

        #region Speed & Storage

        //Represents the vehicle's speed
        protected int speed;

        #region Storage Limit

        //Represents the vehicle's storage limit
        protected int storageLimit;
        //Represents the vehicle's passenger limit
        protected int passengerLimit;

        #endregion

        #region Storage

        //Represents the amount of iron in the storage
        int storedIron;
        //Represents the amount of copper in the storage
        int storedCopper;
        //Represents the amount of carbon in the storage
        int storedCarbon;

        //Represents the amount of water in the storage
        int storedWater;
        //Represents the amount of food in the storage
        int storedFood;

        #endregion

        #endregion

        #region Save/Load

        public VehicleSave save;

        #region Load Vehicle

        public void LoadVehicle(VehicleSave load)
        {
            assemblingPoints = load.assemblingPoints;
            storedWater = load.storedWater;
            storedIron = load.storedIron;
            storedFood = load.storedFood;
            storedCopper = load.storedCopper;
            storedCarbon = load.storedCarbon;
            itemType = load.itemType;
        }

        #endregion

        public void SaveVehicle()
        {
            save.storedCarbon = storedCarbon;
            save.storedCopper = storedCopper;
            save.storedFood = storedFood;
            save.storedIron = storedIron;
            save.storedWater = storedWater;
            save.itemType = itemType;
            save.assemblingPoints = assemblingPoints;
        }

        #endregion

        /// <summary>
        /// Sets the standard values for vehicles
        /// </summary>
        public Vehicle()
        {
            assemblingPointsNeeded = 15;
            costCarbon = 100;
            costCopper = 100;
            costIron = 100;
            maintenanceCopper = 3;
            maintenanceCarbon = 3;
            maintenanceIron = 3;
            speed = 5;
            storageLimit = 1000;
            passengerLimit = 2;

            storedCarbon = 0;
            storedCopper = 0;
            storedFood = 0;
            storedIron = 0;
            storedWater = 0;

            vehicleType = "Ground";
            vehicleSize = "Normal";
        }

        #region Get speed & storageLimit

        /// <summary>
        /// Gets the vehicle's speed
        /// </summary>
        /// <returns>Returns the vehicle's speed</returns>
        public int GetSpeed()
        {
            return speed;
        }

        /// <summary>
        /// Gets the vehicle's storage limit
        /// </summary>
        /// <returns>Returns the vehicle's storage limit</returns>
        public int GetStorageLimit()
        {
            return storageLimit;
        }

        /// <summary>
        /// Gets the vehicle's passenger limit
        /// </summary>
        /// <returns>Returns the vehicle's passenger limit</returns>
        public int PassengerLimit()
        {
            return passengerLimit;
        }

        #endregion

        #region Vehicle

        /// <summary>
        /// Gets the vehicle type
        /// </summary>
        /// <returns>Returns the vehicle type</returns>
        public string GetVehicleType()
        {
            return vehicleType;
        }

        /// <summary>
        /// Gets the size of the vehicle
        /// </summary>
        /// <returns>Returns the size of the vehicle</returns>
        public string GetVehicleSize()
        {
            return vehicleSize;
        }

        #endregion

        #region Actual Storage

        public void SetStoredResources(int resourceAmount, string resourceType)
        {
            switch (resourceType)
            {
                case "food":
                    storedFood = resourceAmount;
                    break;
                case "water":
                    storedWater = resourceAmount;
                    break;
                case "iron":
                    storedIron = resourceAmount;
                    break;
                case "copper":
                    storedCopper = resourceAmount;
                    break;
                case "carbon":
                    storedCarbon = resourceAmount;
                    break;
                default: throw new Exception("There is no resource that is identified as " + resourceType);
            }
        }

        public int GetStoredResources(string resourceType)
        {
            switch (resourceType)
            {
                case "food":
                    return storedFood;
                case "water":
                    return storedWater;
                case "iron":
                    return storedIron;
                case "copper":
                    return storedCopper;
                case "carbon":
                    return storedCarbon;
                default:
                    return storedIron;
            }
        }

        #endregion

        //o(>_<)o
    }

    public struct VehicleSave
    {
        public int assemblingPoints;

        public int storedIron;

        public int storedCopper;

        public int storedCarbon;

        public int storedWater;

        public int storedFood;

        public string itemType;
    }
}
