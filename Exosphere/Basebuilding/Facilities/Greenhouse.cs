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
    class Greenhouse : Facility
    {
        //An int telling how big the water demand is for the Greenhouse
        int waterDemand;


        #region Load/Save

        public override void LoadFacility(GreenhouseSave load)
        {
            this.position = load.position;

            this.colonyID = load.colonyID;

            this.level = load.level;

            this.storageLevel = load.storageLevel;

            this.housingLimit = load.housingLimit;

            this.finished = load.finished;

            this.timeUnderConstruction = load.timeUnderConstruction;

            this.waterDemand = load.waterDemand;

            this.ID = load.ID;

            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/GreenhousePH");

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
            greenhouseSave.position = position;

            greenhouseSave.colonyID = colonyID;

            greenhouseSave.level = level;

            greenhouseSave.storageLevel = storageLevel;

            greenhouseSave.housingLimit = housingLimit;

            greenhouseSave.finished = finished;

            greenhouseSave.timeUnderConstruction = timeUnderConstruction;

            greenhouseSave.waterDemand = waterDemand;

            greenhouseSave.ID = ID;
        }
        #endregion

        /// <summary>
        /// Creates a new Greenhouse
        /// </summary>
        /// <param name="position">The position of the greenhouse</param>
        /// <param name="colony">The colony in which the greenhouse is located</param>
        public Greenhouse(Vector2 position, Colony colony, int ID)
            : base(position, colony, ID)
        {
            //Loads the texture
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/GreenhousePH");

            //Sets the facility type
            facilityType = "Greenhouse";

            //Creates a facility button
            CreateFacilityButton();

            costCopper = 500;
            costIron = 75;
            costCarbon = 150;

            copperValue = 3f;
            carbonValue = 3.75f;
            ironValue = 2.25f;

            requiredEnergy = 5000;

            //TODO: Greenhouse should not be finished as standard
            finished = true;

            //Set the task screen to be a GreenhouseTaskScreen
            taskScreen = new GreenHouseTaskScreen(this);

            maxLevel = 3;

            //Set the standard water demand to 0
            waterDemand = 0;

            SetDebugValues();
        }

        /// <summary>
        /// Returns the facility type
        /// </summary>
        /// <returns>A string the facility type</returns>
        public override string GetFacilityType()
        {
            return base.GetFacilityType();
        }

        /// <summary>
        /// Changes the water demand
        /// </summary>
        /// <param name="change">An int with the change value</param>
        public void SetWaterDemand(int change)
        {

            waterDemand += change;

            //If the water demand is under 0
            if (waterDemand < 0)
            {
                //Set water deman to 0
                waterDemand = 0;
            }

            

        }

        /// <summary>
        /// Returns the water demand
        /// </summary>
        /// <returns>An int with the water demand</returns>
        public int GetWaterDemand()
        {
            return waterDemand;
        }

        /// <summary>
        /// The Greenhouse's task
        /// </summary>
        /// <param name="grid">The grid in which the greenhouse is located</param>
        public override void Task(Grid grid)
        {
            //If the waterDemand is higher than the base can afford, loop with lowering the water demand by ten until the bas can afford id
            while (waterDemand > grid.resourceManager.clearwater)
            {
                SetWaterDemand(-10);
            }

            //If the waterDemand is less than or equal to the amount of clear water the base contains
            if (waterDemand <= grid.resourceManager.clearwater)
            {
                //Remove the demanded water from the stores
                grid.resourceManager.clearwater -= waterDemand;
                //Produce 1 food unit per 10 water units
                grid.resourceManager.food += (int)(waterDemand / 10);
            }

        }

    }
    /*
    class Hunter
    {

        Biome huntingArea;
        Colonist hunter;
        int animalsPerDay;
        bool checkIfHunterIsKilled;

        public Hunter(Colonist colonist)
        {
            hunter = colonist;
        }

        public void AddHuntingArea()
        {

        }

        private void SetAnimalsPerDay()
        {

            animalsPerDay = (int)(hunter.efficiency / 10);
       
        }

        /// <summary>
        /// Makes the hunter go out hunting
        /// </summary>
        /// <returns>The amount of food brought back to the colony</returns>
        public int Hunt()
        {
            return 0;
            SetAnimalsPerDay();
            string[] possiblePrey = new string[animalsPerDay];
            int food = 0;

            for (int i = 0; i < possiblePrey.Length; i++)
            {
                possiblePrey[i] = huntingArea.HuntResult(hunter);
            }

            for (int i = 0; i < possiblePrey.Length; i++)
            {
                food += Kill(possiblePrey[i]);
            }



        }

        private int Kill(string animalSize)
        {
            return 0;
            //if the animal is killed return food
            int animalStr = 0;
            int animalInt = 0;
            int animalChance = 0;
            int hunterChance = 0;
            int foodHold = 0;

            //Set intelligence, strength and food of animals based on size
            #region Animal size
            if (animalSize == "humongous")
            {
                animalStr = Settings.RANDOM.Next(50, 71);
                animalInt = Settings.RANDOM.Next(20, 61);
                foodHold = Settings.RANDOM.Next(10000, 15001);
            }

            if (animalSize == "huge")
            {
                animalStr = Settings.RANDOM.Next(20, 31);
                animalInt = Settings.RANDOM.Next(10, 31);
                foodHold = Settings.RANDOM.Next(1000, 5001);
            }

            if (animalSize == "large")
            {
                animalStr = Settings.RANDOM.Next(10, 21);
                animalInt = Settings.RANDOM.Next(10, 41);
                foodHold = Settings.RANDOM.Next(500, 1001);
            }

            if (animalSize == "medium")
            {
                animalStr = Settings.RANDOM.Next(10, 16);
                animalInt = Settings.RANDOM.Next(5, 31);
                foodHold = Settings.RANDOM.Next(100, 201);
            }

            if (animalSize == "small")
            {
                animalStr = Settings.RANDOM.Next(5, 11);
                animalInt = Settings.RANDOM.Next(5, 21);
                foodHold = Settings.RANDOM.Next(20, 51);
            }

            if (animalSize == "microscopic")
            {
                animalStr = Settings.RANDOM.Next(1, 3);
                animalInt = Settings.RANDOM.Next(1, 3);
                foodHold = Settings.RANDOM.Next(1, 11);
            }
            #endregion

            //Run the difference check 
            #region Animal better than Hunter
            if (animalInt >= hunter.intelligence)
            {
                animalChance += 1;

                if((animalInt - hunter.intelligence) >= 5)
                    animalChance += 1;

                if ((animalInt - hunter.intelligence) >= 10)
                    animalChance += 1;

                if ((animalInt - hunter.intelligence) >= 20)
                    animalChance += 1;
            }

            if(animalStr >= hunter.strength)
            {
                animalChance += 1;

                if ((animalStr - hunter.strength) >= 5)
                    animalChance += 1;

                if ((animalStr - hunter.strength) >= 10)
                    animalChance += 1;

                if ((animalStr - hunter.strength) >= 20)
                    animalChance += 1;

            }
            #endregion

            #region Hunter better than Animal
            if (animalInt < hunter.intelligence)
            {
                hunterChance += 1;

                if ((hunter.intelligence - animalInt) >= 5)
                    hunterChance += 1;

                if ((hunter.intelligence - animalInt) >= 10)
                    hunterChance += 1;

                if ((hunter.intelligence - animalInt) >= 20)
                    hunterChance += 1;
            }

            if (animalStr < hunter.strength)
            {
                hunterChance += 1;

                if ((hunter.strength - animalStr) >= 5)
                    hunterChance += 1;

                if ((hunter.strength - animalStr) >= 10)
                    hunterChance += 1;

                if ((hunter.strength - animalStr) >= 20)
                    hunterChance += 1;

            }
            #endregion

            int hunterLuck = Settings.RANDOM.Next(0, 6);
            int animalLuck = Settings.RANDOM.Next(0, 6);

            if(hunterLuck + hunterChance > animalLuck + animalChance)
            {
                return foodHold;
            }
            else if(hunterLuck + hunterChance < animalLuck + animalChance)
            {
                if((animalLuck + animalChance) - (hunterLuck + hunterChance) >= 5)
                    checkIfHunterIsKilled = true;

                return 0;
            }

        }

    }
*/
    class GreenHouseTaskScreen : FacilityTaskScreen
    {

        //The greenhouse the task screen represents
        Greenhouse greenhouse;

        #region Farm variables
        //Buttons for adding to the water demand and removing from it
        Button add;
        Button remove;

        //A sprite font used to write information
        SpriteFont font;

        //The position of the water demand string
        Vector2 waterDemandStringPosition;

        //A string containing the water demand
        string waterDemand;
        #endregion

        List<Colonist> hunters;

        /// <summary>
        /// Creates a new Greenhouse tas screen
        /// </summary>
        /// <param name="greenhouse">The greenhouse the task screen should represent</param>
        public GreenHouseTaskScreen(Greenhouse greenhouse)
            : base()
        {
            //Sets the local variable for Greenhouse to the one sent in
            this.greenhouse = greenhouse;

            #region Farm
            //Load the font
            font = Game1.INSTANCE.Content.Load<SpriteFont>("Res/Fonts/Message");

            //Create the buttons
            add = new Button("Res/PH/HUD/Buttons/Standard/AddPH", Vector2.Zero);
            remove = new Button("Res/PH/HUD/Buttons/Standard/RemovePH", Vector2.Zero);

            //Sets the position of the buttons to be in the middle of the left side of the screen and middle in Y axis
            add.SetPosition(new Vector2(
                ((Settings.GetScreenRes.X * 0.5f) * 0.75f) - (add.GetTexture().Width / 2),
                (Settings.GetScreenRes.Y * 0.5f) - HUD.HUD.informationList.GetTexture().Height + ((add.GetTexture().Height / 2))));

            remove.SetPosition(new Vector2(
                ((Settings.GetScreenRes.X * 0.5f) * 0.25f) - (remove.GetTexture().Width / 2),
                ((Settings.GetScreenRes.Y * 0.5f) - HUD.HUD.informationList.GetTexture().Height + (remove.GetTexture().Height / 2))));
            #endregion



        }

        /// <summary>
        /// Updates the Task screen
        /// </summary>
        public override void Update()
        {

            //Updates the farming
            FarmUpdate();
            

            base.Update();
        }

        /// <summary>
        /// Updates the farming functions
        /// </summary>
        private void FarmUpdate()
        {
            //Update the button
            add.Update();
            remove.Update();

            //If add is pressed add 10 to the water demand
            if (add.Collision())
            {
                greenhouse.SetWaterDemand(10);
            }
            //If remove is pressed remove 10 from the water demand
            else if (remove.Collision())
            {
                greenhouse.SetWaterDemand(-10);
            }


            //Sets the Water demand screen to equal the water demand of the Greenhouse
            waterDemand = greenhouse.GetWaterDemand().ToString();

            //Sets the water demand string's position to be between the arrows
            waterDemandStringPosition = new Vector2((((add.GetPosition().X - remove.GetPosition().X) * 0.5f) + remove.GetPosition().X) + (font.MeasureString(waterDemand).X / 2), add.GetPosition().Y);
        }



        /// <summary>
        /// Draws the facility screen
        /// </summary>
        /// <param name="spriteBatch">The spritebatch used for drawing</param>
        public override void Draw(SpriteBatch spriteBatch)
        {

            FarmDraw(spriteBatch);

            base.Draw(spriteBatch);
        }

        private void FarmDraw(SpriteBatch spriteBatch)
        {
            //Draws the buttons
            add.Draw(spriteBatch);
            remove.Draw(spriteBatch);

            //Draw the information string
            //TODO: Fixe so that the position is based on the screen rather than locked to 400,400
            if (waterDemand != null)
                spriteBatch.DrawString(font, waterDemand, waterDemandStringPosition, Color.White);
        }

    }

    public struct GreenhouseSave
    {
        public Vector2 position;

        public int colonyID;

        public int level;

        public int storageLevel;

        public int housingLimit;

        public bool finished;

        public int timeUnderConstruction;

        public int waterDemand;
        
        public int ID;
    }
}