using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Basebuilding.Facilities
{
    class Pump : Facility
    {

        PumpSave save;

        public override void LoadFacility(PumpSave load)
        {
            this.colonyID = load.colonyID;
            this.finished = load.finished;
            this.housingLimit = load.housingLimit;
            this.ID = load.ID;
            this.level = load.level;
            this.position = load.position;
            this.storageLevel = load.storageLevel;
            this.timeUnderConstruction = load.timeUnderConstruction;
        }

        public override void SaveFacility()
        {
            pumpSave.colonyID = colonyID;
            pumpSave.finished = finished;
            pumpSave.housingLimit = housingLimit;
            pumpSave.ID = ID;
            pumpSave.level = level;
            pumpSave.position = position;
            pumpSave.storageLevel = storageLevel;
            pumpSave.timeUnderConstruction = timeUnderConstruction;
        }

        public Pump(Vector2 position, Colony colony, int ID)
            : base(position, colony, ID)
        {
            taskScreen = new PumpFacilityTaskScreen();

            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/PumpPH");
           
            facilityType = "Pump";

            finished = true;

            constructionTime = 7;

            CreateFacilityButton();

            costCarbon = 25;
            costCopper = 250;
            costIron = 500;
            requiredEnergy = 500;
            ironValue = 3.2f;
            copperValue = 3.7f;
            carbonValue = 3.6f;

            maxLevel = 4;

            SetDebugValues();
        }

        public override string Information()
        {

            int pumpedPerDay = ((int)Math.Pow(colony.GetPlanet().GetResources(Generators.Resource.ResourceType.clearWater) * 0.000005f, level));
            string message = "Pumps: " + pumpedPerDay + " litres per day";
            return message;
        }

        public override int Task(int value)
        {
           
                float waterPumpAmount = 0;

            if(finished)
                waterPumpAmount += (int)Math.Pow(colony.GetPlanet().GetResources(Generators.Resource.ResourceType.clearWater) * 0.000005f, level);

                value += (int)waterPumpAmount;

            
                return value;
            
        }

    }

    public struct PumpSave
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

    class PumpFacilityTaskScreen : FacilityTaskScreen
    {



    }
}
