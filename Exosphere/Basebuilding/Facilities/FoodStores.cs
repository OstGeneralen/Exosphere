using Exosphere.Src.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Basebuilding.Facilities
{
    class FoodStores : Facility
    {
        //The amount of food the facility can store
        int foodStorageSpace;

        #region Load/Save

        public override void LoadFacility(FoodStoresSave load)
        {
            this.position = load.position;

            this.colonyID = load.colonyID;

            this.level = load.level;

            this.storageLevel = load.storageLevel;

            this.housingLimit = load.housingLimit;

            this.finished = load.finished;

            this.timeUnderConstruction = load.timeUnderConstruction;

            this.ID = load.ID;

            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/EquipmentStoresPH");

        }

        public override void SaveFacility()
        {
            foodStoresSave.position = position;

            foodStoresSave.colonyID = colonyID;

            foodStoresSave.level = level;

            foodStoresSave.storageLevel = storageLevel;

            foodStoresSave.housingLimit = housingLimit;

            foodStoresSave.finished = finished;

            foodStoresSave.timeUnderConstruction = timeUnderConstruction;

            foodStoresSave.ID = ID;
        }
        #endregion

        public FoodStores(Vector2 position, Colony colony, int ID)
            : base(position, colony, ID)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/FoodStoresPH");

            foodStorageSpace = 2500;

            costCopper = 50;
            costIron = 150;
            costCarbon = 25;

            copperValue = 4.5f;
            carbonValue = 2.5f;
            ironValue = 3.5f;

            requiredEnergy = 250;

            maxLevel = 1;

            facilityType = "FoodStores";

            CreateFacilityButton();

            SetDebugValues();
        }

        public override int Task()
        {
            return foodStorageSpace;
        }

        public override string GetFacilityType()
        {
            return base.GetFacilityType();
        }

    }

    public struct FoodStoresSave
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
