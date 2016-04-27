using Exosphere.Src.Items.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Items
{
    public abstract class Item
    {
        #region Manufacturing Time

        //An int representing how much progress you have made in the assembling process
        protected int assemblingPoints;

        //Represents the amount of assembling points needed to finish the item
        protected int assemblingPointsNeeded;

        #endregion

        #region Cost

        //The copper expenses for manufacturing the item
        protected int costCopper;

        //The iron expenses for manufacturing the item
        protected int costIron;

        //The carbon expenses for manufacturing the item
        protected int costCarbon;

        #endregion

        #region Item Type

        protected string itemType;

        #endregion

        #region Save/Load

        public ItemSave save;

        #region Load Item

        public void LoadItem(ItemSave load)
        {
            assemblingPoints = load.assemblingPoints;

            itemType = load.itemType;
        }

        #endregion

        public void SaveItem()
        {
            save.assemblingPoints = assemblingPoints;

            save.itemType = itemType;
        }

        #endregion

        /// <summary>
        /// Sets the standard cost and assemblingPoints needed to produce items
        /// </summary>
        public Item()
        {
            assemblingPoints = 0;
            assemblingPointsNeeded = 15;
            costCarbon = 100;
            costCopper = 100;
            costIron = 100;
        }

        #region Cost

        /// <summary>
        /// Gets the manufacturing cost in copper
        /// </summary>
        /// <returns>Returns the manufacturing cost in copper</returns>
        public int GetCostCopper()
        {
            return costCopper;
        }

        /// <summary>
        /// Gets the manufacturing cost in carbon
        /// </summary>
        /// <returns>Returns the manufacturing cost in carbon</returns>
        public int GetCostCarbon()
        {
            return costCarbon;
        }

        /// <summary>
        /// Gets the manufacturing cost in iron
        /// </summary>
        /// <returns>Returns the manufacturing cost in iron</returns>
        public int GetCostIron()
        {
            return costIron;
        }

        #endregion

        #region Manufacturing Time

        /// <summary>
        /// Adds assemblingPoints to the production period
        /// </summary>
        /// <param name="assemblingPoints">The amount of assemblingPoints provided by a workshop</param>
        public void SetAssemblingPoints(int assemblingPoints)
        {
            this.assemblingPoints += assemblingPoints;
        }

        /// <summary>
        /// Checks if the assembling is finished
        /// </summary>
        /// <returns>Returns true when it's done</returns>
        public bool AssemblingFinished()
        {
            if (assemblingPoints >= assemblingPointsNeeded)
            {
                assemblingPoints = 0;
                return true;
            }
            return false;
        }

        #endregion

        #region Item Type

        /// <summary>
        /// Gets the string representing the item's type
        /// </summary>
        /// <returns>Returns the string representing the item's type</returns>
        public string GetItemType()
        {
            return itemType;
        }

        #endregion

        #region Add Item

        /// <summary>
        /// Adds a new item to the list for possible assembles
        /// </summary>
        /// <param name="itemType">The name of the item you want to add to the possible assebles</param>
        /// <param name="canAssemble">The list containing possible assebles</param>
        /// <returns>Returns the list containing possible assebles</returns>
        public List<Item> AddItem(string itemType, List<Item> canAssemble)
        {
            switch (itemType)
            {
                case "VehicleC60":
                    canAssemble.Add(new VehicleC60());
                    break;
                default:
                    break;
            }
            return canAssemble;
        }

        #endregion
    }

    public struct ItemSave
    {
        public int assemblingPoints;

        public string itemType;
    }
}
