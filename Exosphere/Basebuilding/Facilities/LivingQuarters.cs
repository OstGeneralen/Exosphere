using Exosphere.Src.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Basebuilding.Facilities
{



    class LivingQuarters : Facility
    {

        #region Load/Save

        public override void LoadFacility(LivingQuartersSave load)
        {
            this.position = load.position;

            this.colonyID = load.colonyID;

            this.level = load.level;

            this.storageLevel = load.storageLevel;

            this.housingLimit = load.housingLimit;

            this.finished = load.finished;

            this.timeUnderConstruction = load.timeUnderConstruction;

            this.ID = load.ID;

            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/LivingQuartersPH");
        }

        public override void SaveFacility()
        {
            livingQuartersSave.position = position;
            livingQuartersSave.colonyID = colonyID;
            livingQuartersSave.level = level;
            livingQuartersSave.storageLevel = storageLevel;
            livingQuartersSave.housingLimit = housingLimit;
            livingQuartersSave.finished = finished;
            livingQuartersSave.timeUnderConstruction = timeUnderConstruction;
            livingQuartersSave.ID = ID;
        }
        #endregion

        public LivingQuarters(Vector2 position, Colony colony, int ID)
            : base(position, colony, ID)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/LivingQuartersPH");

            facilityType = "LivingQuarters";

            costCopper = 50;
            costIron = 400;
            costCarbon = 75;

            requiredEnergy = 250;

            maxLevel = 1;

            CreateFacilityButton();

            SetDebugValues();
        }

        public override string GetFacilityType()
        {
            return base.GetFacilityType();
        }

    }

    public struct LivingQuartersSave
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