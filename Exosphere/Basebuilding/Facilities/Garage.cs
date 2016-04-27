using Exosphere.Src.Handlers;
using Exosphere.Src.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Basebuilding.Facilities
{
    class Garage : Facility
    {

        #region Load/Save

        public override void LoadFacility(GarageSave load)
        {
            this.position = load.position;

            this.colonyID = load.colonyID;

            this.level = load.level;

            this.storageLevel = load.storageLevel;

            this.housingLimit = load.housingLimit;

            this.finished = load.finished;

            this.timeUnderConstruction = load.timeUnderConstruction;

            this.ID = load.ID;

            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/GaragePH");

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
            garageSave.position = position;

            garageSave.colonyID = colonyID;

            garageSave.level = level;

            garageSave.storageLevel = storageLevel;

            garageSave.housingLimit = housingLimit;

            garageSave.finished = finished;

            garageSave.timeUnderConstruction = timeUnderConstruction;

            garageSave.ID = ID;
        }
        #endregion

        public Garage(Vector2 position, Colony colony, int ID)
            : base(position, colony, ID)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/GaragePH");

            facilityType = "Garage";

            finished = true;

            costCopper = 300;
            costIron = 150;
            costCarbon = 50;

            copperValue = 3.75f;
            carbonValue = 4f;
            ironValue = 2.75f;

            requiredEnergy = 1500;

            maxLevel = 3;

            CreateFacilityButton();

            taskScreen = new GarageTaskScreen(this);

            SetDebugValues();
        }

        public override string GetFacilityType()
        {
            return base.GetFacilityType();
        }
     
    }

    class GarageTaskScreen : FacilityTaskScreen
    {
        Garage garage;
        List<VehicleButton> buttons;
        bool showExplorationScreen;

        public GarageTaskScreen(Garage garage)
        {
            buttons = new List<VehicleButton>();


            this.garage = garage;

            showExplorationScreen = false;
        }

        public override void Update()
        {

            if(buttons.Count == 0)
            {
                int amount = 0;
                foreach (var vehicle in garage.GetColony().vehicles)
                {
                    if (vehicle.GetVehicleType() == "Ground")
                    {
                        buttons.Add(new VehicleButton(amount, vehicle));
                        amount++;
                    }
                }
            }

            foreach (var button in buttons)
            {
                button.Update();

                if(button.Collision())
                {
                    showExplorationScreen = true;
                }

                if(showExplorationScreen)
                {
                    Core.planetScreen.explorationScreen.Update(button.GetVehicle());
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(showExplorationScreen)
            {
                Core.planetScreen.explorationScreen.Draw(spriteBatch);
            }
            if(!showExplorationScreen)
            {
                foreach(var button in buttons)
                {
                    button.Draw(spriteBatch);
                }
            }
        }

    }

    public struct GarageSave
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
