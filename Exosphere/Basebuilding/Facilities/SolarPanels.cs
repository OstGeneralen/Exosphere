using Exosphere.Src.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Basebuilding.Facilities
{
    class SolarPanels : Facility
    {

        #region Load/Save

        public override void LoadFacility(SolarPanelsSave load)
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
        }

        public override void SaveFacility()
        {
            solarPanelsSave.position = position;
            solarPanelsSave.colonyID = colonyID;
            solarPanelsSave.level = level;
            solarPanelsSave.storageLevel = storageLevel;
            solarPanelsSave.housingLimit = housingLimit;
            solarPanelsSave.finished = finished;
            solarPanelsSave.timeUnderConstruction = timeUnderConstruction;
            solarPanelsSave.ID = ID;
        }
        #endregion

        public SolarPanels(Vector2 position, Colony colony, int ID)
            : base(position, colony, ID)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/SolarPanelsPH");

            facilityType = "SolarPanels";

            costCopper = 250;
            costIron = 50;
            costCarbon = 500;

            copperValue = 3;
            carbonValue = 6.25f;
            ironValue = 2.75f;

            requiredEnergy = 0;

            maxLevel = 4;

            CreateFacilityButton();

            SetDebugValues();
        }

        public override int Task(int value)
        {

            value = 8000;

            return base.Task(value);
        }

        public override string GetFacilityType()
        {
            return base.GetFacilityType();
        }


    }

    public struct SolarPanelsSave
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
