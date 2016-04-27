using Exosphere.Src.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Basebuilding.Facilities
{
    class Gym : Facility
    {

        #region Load/Save

        public override void LoadFacility(GymSave load)
        {
            this.position = load.position;

            this.colonyID = load.colonyID;

            this.level = load.level;

            this.storageLevel = load.storageLevel;

            this.housingLimit = load.housingLimit;

            this.finished = load.finished;

            this.timeUnderConstruction = load.timeUnderConstruction;

            this.ID = load.ID;

            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/GymPH");

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
            gymSave.position = position;
            gymSave.colonyID = colonyID;
            gymSave.level = level;
            gymSave.storageLevel = storageLevel;
            gymSave.housingLimit = housingLimit;
            gymSave.finished = finished;
            gymSave.timeUnderConstruction = timeUnderConstruction;
            gymSave.ID = ID;
        }
        #endregion

        public Gym(Vector2 position, Colony colony, int ID)
            : base(position, colony, ID)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/GymPH");

            facilityType = "Gym";

            costCopper = 50;
            costIron = 100;
            costCarbon = 50;

            requiredEnergy = 500;

            maxLevel = 1;

            CreateFacilityButton();

            SetDebugValues();
        }

        public override void Task(string text)
        {
            if (TimeHandler.newTurn)
                foreach (var worker in workers)
                {
                    worker.StrengthTraining();
                }
        }

        public override string GetFacilityType()
        {
            return base.GetFacilityType();
        }

    }

    public struct GymSave
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
