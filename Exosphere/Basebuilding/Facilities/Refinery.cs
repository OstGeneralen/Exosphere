using Exosphere.Src.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Basebuilding.Facilities
{
    class Refinery : Facility
    {

        
        #region Load/Save

        public override void LoadFacility(RefinerySave load)
        {
            this.position = load.position;

            this.colonyID = load.colonyID;

            this.level = load.level;

            this.storageLevel = load.storageLevel;

            this.housingLimit = load.housingLimit;

            this.finished = load.finished;

            this.timeUnderConstruction = load.timeUnderConstruction;

            this.ID = load.ID;

            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/RefineryPH");

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
            refinerySave.position = position;
            refinerySave.colonyID = colonyID;
            refinerySave.level = level;
            refinerySave.storageLevel = storageLevel;
            refinerySave.housingLimit = housingLimit;
            refinerySave.finished = finished;
            refinerySave.timeUnderConstruction = timeUnderConstruction;
            refinerySave.ID = ID;
        }
        #endregion

        public Refinery(Vector2 position, Colony colony, int ID)
            : base(position, colony, ID)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/RefineryPH");

            facilityType = "Refinery";

            CreateFacilityButton();
        }

        public override string GetFacilityType()
        {
            return base.GetFacilityType();
        }
    }

    public struct RefinerySave
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