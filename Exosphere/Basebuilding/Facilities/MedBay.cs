using Exosphere.Src.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Basebuilding.Facilities
{
    class MedBay : Facility
    {


        #region Load/Save

        public override void LoadFacility(MedBaySave load)
        {
            this.position = load.position;

            this.colonyID = load.colonyID;

            this.level = load.level;

            this.storageLevel = load.storageLevel;

            this.housingLimit = load.housingLimit;

            this.finished = load.finished;

            this.timeUnderConstruction = load.timeUnderConstruction;

            this.ID = load.ID;

            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/MedBayPH");

            for (int i = 0; i < colony.inhabitants.Count; i++)
            {
                if (colony.inhabitants[i].occupied && colony.inhabitants[i].facilityID == ID)
                {
                    workers.Add(colony.inhabitants[i]);
                    colony.inhabitants[i].workPlace = this;
                }
            }
        }

        public override void SaveFacility()
        {
            medBaySave.position = position;
            medBaySave.colonyID = colonyID;
            medBaySave.level = level;
            medBaySave.storageLevel = storageLevel;
            medBaySave.housingLimit = housingLimit;
            medBaySave.finished = finished;
            medBaySave.timeUnderConstruction = timeUnderConstruction;
            medBaySave.ID = ID;
        }
        #endregion

        public MedBay(Vector2 position, Colony colony, int ID)
            : base(position, colony, ID)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/MedBayPH");

            facilityType = "MedBay";

            costCopper = 100;
            costIron = 100;
            costCarbon = 100;

            copperValue = 3.5f;
            carbonValue = 3.5f;
            ironValue = 3.5f;

            requiredEnergy = 250;

            maxLevel = 3;

            finished = true;

            CreateFacilityButton();

            SetDebugValues();
        }

        public override void Task(string text)
        {
            foreach (var worker in workers)
            {
                foreach (var vaccine in colony.vaccines)
                {
                    worker.AddVaccine(vaccine);
                }
            }
        }

        public override string GetFacilityType()
        {
            return base.GetFacilityType();
        }
    }

    public struct MedBaySave
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
