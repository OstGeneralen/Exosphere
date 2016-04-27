using Exosphere.Src.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Basebuilding.Facilities
{
    class EquipmentStores : Facility
    {
        #region Load/Save

        public override void LoadFacility(EquipmentStoresSave load)
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
            equipmentStoresSave.position = position;

            equipmentStoresSave.colonyID = colonyID;

            equipmentStoresSave.level = level;

            equipmentStoresSave.storageLevel = storageLevel;

            equipmentStoresSave.housingLimit = housingLimit;

            equipmentStoresSave.finished = finished;

            equipmentStoresSave.timeUnderConstruction = timeUnderConstruction;

            equipmentStoresSave.ID = ID;
        }
        #endregion

        public EquipmentStores(Vector2 position, Colony colony, int ID)
            : base(position, colony, ID)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/EquipmentStoresPH");

            facilityType = "EquipmentStores";

            costCopper = 50;
            costIron = 150;
            costCarbon = 25;

            copperValue = 3.5f;
            carbonValue = 4.5f;
            ironValue = 2.5f;

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

    public struct EquipmentStoresSave
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