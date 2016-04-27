using Exosphere.Src.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Basebuilding.Facilities
{
    class Library : Facility
    {

        #region Load/Save

        public override void LoadFacility(LibrarySave load)
        {
            this.position = load.position;

            this.colonyID = load.colonyID;

            this.level = load.level;

            this.storageLevel = load.storageLevel;

            this.housingLimit = load.housingLimit;

            this.finished = load.finished;

            this.timeUnderConstruction = load.timeUnderConstruction;

            this.ID = load.ID;

            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/LibraryPH");

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
            librarySave.position = position;
            librarySave.colonyID = colonyID;
            librarySave.level = level;
            librarySave.storageLevel = storageLevel;
            librarySave.housingLimit = housingLimit;
            librarySave.finished = finished;
            librarySave.timeUnderConstruction = timeUnderConstruction;
            librarySave.ID = ID;
        }
        #endregion

        public Library(Vector2 position, Colony colony, int ID)
            : base(position, colony, ID)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/LibraryPH");

            costCopper = 50;
            costIron = 100;
            costCarbon = 100;

            requiredEnergy = 500;

            facilityType = "Library";

            maxLevel = 1;

            CreateFacilityButton();

            SetDebugValues();
        }

        public override void Task(string text)
        {
            if (TimeHandler.newTurn)
                foreach (var worker in workers)
                {
                    worker.IntelligenceTraining();
                }
        }

        public override string GetFacilityType()
        {
            return base.GetFacilityType();
        }


    }

    public struct LibrarySave
    {
        public Vector2 position;

        public int colonyID;

        public int ID;

        public int level;

        public int storageLevel;

        public int housingLimit;

        public bool finished;

        public int timeUnderConstruction;
    }
}
