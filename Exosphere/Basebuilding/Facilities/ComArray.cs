using Exosphere.Src.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Basebuilding.Facilities
{
    class ComArray : Facility
    {
        //Represents the com-array's radar range
        double range;

        #region Load/Save

        public override void LoadFacility(ComArraySave load)
        {
            this.position = load.position;

            this.colonyID = load.colonyID;

            this.level = load.level;

            this.storageLevel = load.storageLevel;

            this.housingLimit = load.housingLimit;

            this.finished = load.finished;

            this.timeUnderConstruction = load.timeUnderConstruction;

            this.ID = load.ID;

            range = 5000;

            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/ComArrayPH");
        }

        public override void SaveFacility()
        {
            comArraySave.position = position;

            comArraySave.colonyID = colonyID;

            comArraySave.level = level;

            comArraySave.storageLevel = storageLevel;

            comArraySave.housingLimit = housingLimit;

            comArraySave.finished = finished;

            comArraySave.timeUnderConstruction = timeUnderConstruction;

            comArraySave.ID = ID;
        }
        #endregion

        public ComArray(Vector2 position, Colony colony, int ID)
            : base(position, colony, ID)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/ComArrayPH");

            facilityType = "ComArray";

            finished = true;

            range = 5000;

            costCopper = 625;
            costIron = 1250;
            costCarbon = 275;

            copperValue = 5;
            carbonValue = 4;
            ironValue = 6;

            maxLevel = 5;

            requiredEnergy = 1000;

            CreateFacilityButton();

            SetDebugValues();
        }

        public override void Task(string text)
        {
            //Represents the distance to the other colonies
            double distance;

            
            //Runs through all colonies and checks their distance to the com-array in the current colony
            //Observe the other colony must also have a com array(*?*)
            foreach (var otherColony in Core.GetColonies())
            {
                if (otherColony != colony && otherColony.GetGrid().GetFacilityAmount("ComArray") > 0)
                {
                    //Calculates the distance between the active colony's com array and another colony
                    distance = Math.Sqrt((Math.Pow(MathHelper.Distance(otherColony.GetPlanet().GetPosition().X, colony.GetPlanet().GetPosition().X), 2) +
                        Math.Pow(MathHelper.Distance(otherColony.GetPlanet().GetPosition().Y, colony.GetPlanet().GetPosition().Y), 2)));

                    //Checks if the other colonies is within the com-array's radar
                    //Checks if the com array already has connection with the other colonies
                    if (!colony.colonies.Contains(otherColony) &&
                        range >= distance)
                        //Adds a colony to the list of colonies the com array has contact with
                        colony.colonies.Add(otherColony);
                }
            }
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

    public struct ComArraySave
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
