using Exosphere.Src.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Basebuilding.Facilities
{
    class MineralStores : Facility
    {
        //The amount of food the facility can store
        int mineralStorageSpace;

        #region Load/Save

        public override void LoadFacility(MineralStoresSave load)
        {
            this.position = load.position;

            this.colonyID = load.colonyID;

            this.level = load.level;

            this.storageLevel = load.storageLevel;

            this.housingLimit = load.housingLimit;

            this.finished = load.finished;

            this.timeUnderConstruction = load.timeUnderConstruction;

            this.ID = load.ID;

            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/MineralStoresPH");
        }

        public override void SaveFacility()
        {
            mineralStoresSave.position = position;
            mineralStoresSave.colonyID = colonyID;
            mineralStoresSave.level = level;
            mineralStoresSave.storageLevel = storageLevel;
            mineralStoresSave.housingLimit = housingLimit;
            mineralStoresSave.finished = finished;
            mineralStoresSave.timeUnderConstruction = timeUnderConstruction;
            mineralStoresSave.ID = ID;
        }
        #endregion

        public MineralStores(Vector2 position, Colony colony, int ID)
            : base(position, colony, ID)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/MineralStoresPH");

            mineralStorageSpace = 10000;

            costCopper = 50;
            costIron = 300;
            costCarbon = 25;

            copperValue = 3;
            carbonValue = 3;
            ironValue = 4.5f;

            requiredEnergy = 250;

            maxLevel = 1;

            facilityType = "MineralStores";

            CreateFacilityButton();

            SetDebugValues();
        }

        public override int Task()
        {
            return mineralStorageSpace;
        }

        public override string GetFacilityType()
        {
            return base.GetFacilityType();
        }

    }

    public struct MineralStoresSave
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
