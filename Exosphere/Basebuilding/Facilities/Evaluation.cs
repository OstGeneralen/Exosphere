using Exosphere.Src.Generators;
using Exosphere.Src.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Basebuilding.Facilities
{
    public class Evaluation : Facility
    {
        int evaluationSpeed;

        #region Load/Save

        public override void LoadFacility(EvaluationFacilitySave load)
        {
            this.position = load.position;

            this.colonyID = load.colonyID;

            this.level = load.level;

            this.storageLevel = load.storageLevel;

            this.housingLimit = load.housingLimit;

            this.finished = load.finished;

            this.timeUnderConstruction = load.timeUnderConstruction;

            this.ID = load.ID;

            evaluationSpeed = 1;

            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/EvaluationFacilityPH");

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
            evaluationFacilitySave.position = position;

            evaluationFacilitySave.colonyID = colonyID;

            evaluationFacilitySave.level = level;

            evaluationFacilitySave.storageLevel = storageLevel;

            evaluationFacilitySave.housingLimit = housingLimit;

            evaluationFacilitySave.finished = finished;

            evaluationFacilitySave.timeUnderConstruction = timeUnderConstruction;

            evaluationFacilitySave.ID = ID;
        }
        #endregion

        public Evaluation(Vector2 position, Colony colony, int ID)
            : base(position, colony, ID)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/EvaluationFacilityPH");

            facilityType = "Evaluation";
            evaluationSpeed = 1;
            level = 1;

            costCopper = 100;
            costIron = 50;
            costCarbon = 100;

            copperValue = 4.5f;
            carbonValue = 4f;
            ironValue = 3.5f;

            requiredEnergy = 250;

            maxLevel = 4;

            CreateFacilityButton();

            SetDebugValues();
        }

        public override void Task(string text)
        {
            foreach (var worker in workers)
            {
                worker.Evaluated(evaluationSpeed, this);
            }
        }

        public override string GetFacilityType()
        {
            return base.GetFacilityType();
        }


    }

    public struct EvaluationFacilitySave
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