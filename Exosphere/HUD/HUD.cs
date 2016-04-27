using Exosphere.Src.Basebuilding;
using Exosphere.Src.Basebuilding.Facilities;
using Exosphere.Src.Generators;
using Exosphere.Src.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.HUD
{
    static class HUD
    {


        //The list in the top of the screen telling in which view the player is
        public static HUDList informationList;

        //The list in the bottom of the screen containing the different buttons for choices
        public static HUDList buttonsList;

        //A font used for drawing strings
        static SpriteFont font;

        //The string that should be drawn in the top list
        static string currentScreen;

        //The position of the currentScreen string
        static Vector2 currentScreenPosition;


        /// <summary>
        /// Creates the HUD
        /// </summary>
        public static void CreateHud()
        {

            //Creates the two lists
            informationList = new HUDList("Res/PH/HUD/HUDInfoPH", 1);
            buttonsList = new HUDList("Res/PH/HUD/HUDButtonsListPH", 2);

            //Loads a font into the 'font' variable
            font = Game1.INSTANCE.Content.Load<SpriteFont>("Res/Fonts/Message");

            ActionButtons.CreateActionButtons();


            //Creates a Colony Hud
            ColonyHUD.CreateColonyHUD();
            DateHUD.CreateDateHUD();
            FacilityHUD.CreateFacilityHUD();


        }

        /// <summary>
        /// Returns the decided position of one of the buttons that should be shown in the HUD
        /// </summary>
        /// <param name="button">The button you want to position</param>
        /// <param name="position">1. Slightly to the left of the date 2. Slightly to the right of the date</param>
        /// <returns>A vector2 position</returns>
        public static Vector2 PositionHudButton(Button button, int position)
        {

            if (position == 1)
            {
                return new Vector2(
                (Settings.GetScreenRes.X * 0.25f) - (button.GetTexture().Width / 2),
                HUD.buttonsList.GetPosition().Y + (HUD.buttonsList.GetTexture().Height / 2) - (button.GetTexture().Height / 2));
            }

            return new Vector2(
                (Settings.GetScreenRes.X * 0.75f) - (button.GetTexture().Width / 2),
                HUD.buttonsList.GetPosition().Y + (HUD.buttonsList.GetTexture().Height / 2) - (button.GetTexture().Height / 2));


        }

        /// <summary>
        /// Sets the string in the top of the screen to contain the current screen's name
        /// </summary>
        /// <param name="screenName">The string that the string should contain</param>
        public static void SetScreenString(string screenName)
        {
            //Set the currentScreen string to contain the inputed screenName
            currentScreen = screenName;

            //Set so that the string's position is in the middle of the upper screen list and in the middle of the x-axis of the screen
            currentScreenPosition = new Vector2(
                Settings.GetScreenRes.X / 2 - (font.MeasureString(currentScreen).X / 2),
                0 + informationList.GetTexture().Height / 2 - (font.MeasureString(currentScreen).Y / 2));
        }

        /// <summary>
        /// Updates the HUD
        /// </summary>
        public static void Update()
        {

            if (Core.currentScreen == Core.colonyScreen)
            {

                ActionButtons.buildButtonActive = true;
                ActionButtons.colonistButtonActive = true;
                ActionButtons.explorationButtonActive = false;
                ActionButtons.infoButtonActive = true;
                ActionButtons.probeButtonActive = false;
                ActionButtons.taskButtonActive = false;
                ActionButtons.workerButtonActive = false;

                ColonyHUD.Update();
            }

            if (Core.currentScreen == Core.facilityScreen)
            {

                ActionButtons.buildButtonActive = false;
                ActionButtons.colonistButtonActive = false;
                ActionButtons.explorationButtonActive = false;
                ActionButtons.infoButtonActive = false;
                ActionButtons.probeButtonActive = false;
                ActionButtons.taskButtonActive = true;
                ActionButtons.workerButtonActive = true;

                if (Core.facilityScreen.GetFacility() == "EquipmentStores" ||
                    Core.facilityScreen.GetFacility() == "MineralStores" ||
                    Core.facilityScreen.GetFacility() == "FoodStores" ||
                    Core.facilityScreen.GetFacility() == "SolarPanels" ||
                    Core.facilityScreen.GetFacility() == "ComArray")
                {
                    ActionButtons.taskButtonActive = false;
                    ActionButtons.workerButtonActive = false;
                }

                FacilityHUD.Update();
            }

            if (Core.currentScreen == Core.galaxyScreen)
            {

                ActionButtons.buildButtonActive = false;
                ActionButtons.colonistButtonActive = false;
                ActionButtons.explorationButtonActive = false;
                ActionButtons.infoButtonActive = false;
                ActionButtons.probeButtonActive = true;
                ActionButtons.taskButtonActive = false;
                ActionButtons.workerButtonActive = false;
            }

            if (Core.currentScreen == Core.planetScreen)
            {

                ActionButtons.buildButtonActive = false;
                ActionButtons.colonistButtonActive = false;
                ActionButtons.explorationButtonActive = true;
                ActionButtons.infoButtonActive = false;
                ActionButtons.probeButtonActive = false;
                ActionButtons.taskButtonActive = false;
                ActionButtons.workerButtonActive = false;

                PlanetHUD.Update();
            }

            DateHUD.Update();

            if (Core.currentScreen != Core.facilityScreen)
            {
                FacilityHUD.Reset();
            }

            if (Core.currentScreen != Core.colonyScreen)
            {
                ColonyHUD.Reset(true);
            }

            if (TimeHandler.newTurn)
                ColonyHUD.Reset(false);

            ActionButtons.Update();

        }

        /// <summary>
        /// Draw the HUD
        /// </summary>
        /// <param name="spriteBatch">The Sprite batch used for drawing</param>
        public static void Draw(SpriteBatch spriteBatch)
        {

            //Universal draws
            informationList.Draw(spriteBatch);
            spriteBatch.DrawString(font, currentScreen, currentScreenPosition, Color.SeaShell);
            buttonsList.Draw(spriteBatch);



            //Draws for specific Sub-HUDs

            if (Core.currentScreen == Core.colonyScreen)
            {
                ColonyHUD.Draw(spriteBatch);
            }

            if (Core.currentScreen == Core.facilityScreen)
            {
                FacilityHUD.Draw(spriteBatch);
            }

            DateHUD.Draw(spriteBatch);

            ActionButtons.Draw(spriteBatch);


        }

    }

    static class DateHUD
    {
        //The font that should be used in the Date HUD
        static SpriteFont font;

        //A string containing the current in-game date
        static string date;

        //The position of the date
        static Vector2 position;

        /// <summary>
        /// Creates a new date writer
        /// </summary>
        public static void CreateDateHUD()
        {
            //Loads the font
            font = Game1.INSTANCE.Content.Load<SpriteFont>("Res/Fonts/Clock");
        }

        /// <summary>
        /// Updates the date
        /// </summary>
        public static void Update()
        {
            //Sets the date string to contain the current date the Time handler holds
            date = TimeHandler.GetDate();

            //Sets the position of the date string
            position = new Vector2(
                (Settings.GetScreenRes.X / 2) - (font.MeasureString(date).X / 2),
                HUD.buttonsList.GetPosition().Y + (HUD.buttonsList.GetTexture().Height / 2) - (font.MeasureString(date).Y / 2));

        }

        /// <summary>
        /// Draws the date
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used for drawing the date</param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, date, position, Color.White);

        }


    }

    static class ColonyHUD
    {

        //An array containing the different category-buttons for basebuilding
        static FacilityChoiceCategory[] facilityChoiceCategories;

        static FacilityChoiceCategory facilityChoiceCategory;

        //A list of colonistButtons
        static List<Button> colonistButtons;

        //A bool telling if the HUD should show the facility categories
        public static bool showFacilityChoices;
        static bool showColonists;

        //The colony the hud is represeting
        static Colony colony;

        static ScrollBox colonistScrollBox;

        /// <summary>
        /// Creates the HUD for the Colony view
        /// </summary>
        public static void CreateColonyHUD()
        {
            //Make the array containing facility choice categories have room for all 4 categories (Workplaces, Training, Stores, Misc)
            facilityChoiceCategories = new FacilityChoiceCategory[4];

            colonistButtons = new List<Button>();


            //Loads the different categories into the array of facility choices
            #region Load categories
            facilityChoiceCategories[0] = new FacilityChoiceCategory("Misc", Vector2.Zero);
            facilityChoiceCategories[1] = new FacilityChoiceCategory("Training and Stats", Vector2.Zero);
            facilityChoiceCategories[2] = new FacilityChoiceCategory("Stores", Vector2.Zero);
            facilityChoiceCategories[3] = new FacilityChoiceCategory("Workplaces", Vector2.Zero);

            facilityChoiceCategories[0].SetPosition(PositionCategory(0));
            facilityChoiceCategories[1].SetPosition(PositionCategory(1));
            facilityChoiceCategories[2].SetPosition(PositionCategory(2));
            facilityChoiceCategories[3].SetPosition(PositionCategory(3));

            #endregion

            //Set the starting value of showFacilityChoices to false
            showFacilityChoices = false;
            showColonists = false;
        }

        /*
        public static bool GetShowFacilityChoises()
        {
            return showFacilityChoices;
        }

        public static void SetShowFacilityChoises(bool value)
        {
            showFacilityChoices = value;
        }
        */

        public static FacilityChoiceCategory[] GetFacilityChoiceCategories()
        {
            return facilityChoiceCategories;
        }

        public static FacilityChoiceCategory GetFacilityChoiceCategory()
        {
            return facilityChoiceCategory;
        }

        /// <summary>
        /// Resets all bools and clears the list of colonists
        /// </summary>
        public static void Reset(bool all)
        {

            colonistButtons.Clear();
            showFacilityChoices = false;
            showColonists = false;

            if (!all && Core.GetColonies().Count != 0 && Core.currentScreen == Core.colonyScreen)
                SetColony(colony);

        }

        /// <summary>
        /// Positions the Categorybutton corectly
        /// </summary>
        /// <returns>A vector2 with the position</returns>
        private static Vector2 PositionCategory(int i)
        {
            return new Vector2(
                ActionButtons.buildButton.GetTexture().Width + 1,
                ActionButtons.buildButton.GetPosition().Y + ActionButtons.buildButton.GetTexture().Height - facilityChoiceCategories[0].GetTexture().Height * (i + 1));
        }

        /// <summary>
        /// Positions the Colonist Buttons
        /// </summary>
        /// <param name="i">The amount of colonists that have already been given a button</param>
        /// <returns>Vectoor2 position</returns>
        private static Vector2 PositionColonist(int i)
        {
            return new Vector2(
                ActionButtons.colonistButton.GetTexture().Width + 1,
                HUD.informationList.GetTexture().Height + ActionButtons.colonistButton.GetTexture().Height * (i + 1));
        }

        /// <summary>
        /// Loads the currently active colony into the colony
        /// </summary>
        /// <param name="currentColony">The current colony</param>
        public static void SetColony(Colony currentColony)
        {
            int i = 0;
            colony = currentColony;
            if (Core.GetColonies().Count == 1)
            {
                colony = Core.GetColonies()[0];
            }
            foreach (Colonist colonist in currentColony.GetInhabitants())
            {
                colonistButtons.Add(new ColonistChoice("Res/PH/HUD/Buttons/Standard/ButtonUpPH", PositionColonist(i), colonist));
                i++;
            }

            colonistScrollBox = new ScrollBox(colonistButtons);


        }

        /// <summary>
        /// Updates the colony HUD
        /// </summary>
        public static void Update()
        {

            //If the player presses the build facilities button while it is inactive make it active and all other bools inactive
            if (ActionButtons.buildButton.Collision() && !showFacilityChoices)
            {
                ActionButtons.isUsed = true;
                showFacilityChoices = true;
                showColonists = false;
                MouseHandler.Update();
            }
            //If the player presses the build facilities button while it is active, set it to inactive
            else if (ActionButtons.buildButton.Collision() && showFacilityChoices)
            {
                ActionButtons.isUsed = false;
                showFacilityChoices = false;
            }


            //Same procedure as for the build facility button. Read the two previous comment lines
            if (ActionButtons.colonistButton.Collision() && !showColonists)
            {
                ActionButtons.isUsed = true;
                showColonists = true;
                showFacilityChoices = false;
                MouseHandler.Update();
            }
            else if (ActionButtons.colonistButton.Collision() && showColonists)
            {
                ActionButtons.isUsed = false;
                showColonists = false;
            }

            #region Facility choices
            //If the showFacilityChoices bool is true
            if (showFacilityChoices)
            {
                //Loop through each facility choice categorie
                for (int i = 0; i < facilityChoiceCategories.Length; i++)
                {
                    //Update the facility choice category
                    facilityChoiceCategories[i].Update();

                    //If the facility choice category should show facility choices
                    if (facilityChoiceCategories[i].showFacilityChoices)
                    {
                        facilityChoiceCategory = facilityChoiceCategories[i];
                        //Loop through each facility choice again
                        for (int y = 0; y < facilityChoiceCategories.Length; y++)
                        {
                            if (facilityChoiceCategories[y] != facilityChoiceCategories[i] && facilityChoiceCategories[y].showFacilityChoices)
                            {
                                facilityChoiceCategories[y].showFacilityChoices = false;
                            }
                        }
                    }
                }
            }
            #endregion

            #region Show colonists
            //If the show colonists bool is true
            if (showColonists)
            {

                Core.currentScrollBox = colonistScrollBox;
                showColonists = false;
                ActionButtons.isUsed = false;
            }
            #endregion
        }

        /// <summary>
        /// Draws the colony HUD
        /// </summary>
        /// <param name="spriteBatch">The sprite baych used for drawing</param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            //If showFacilityChoices is true
            if (showFacilityChoices)
            {
                //Draw each facilityChoice category
                for (int i = 0; i < facilityChoiceCategories.Length; i++)
                {
                    facilityChoiceCategories[i].Draw(spriteBatch);
                }
            }
        }
    }

    static class FacilityHUD
    {

        //The Facility that hold the currently active facility type
        static Facility facility;


        /// <summary>
        /// Creates a new FacilityHUD. This should only be done once 
        /// </summary>
        public static void CreateFacilityHUD()
        {

        }

        /// <summary>
        /// Sets the currently active facility
        /// </summary>
        /// <param name="currentFacility">The facility currently being changed/watched/updated</param>
        public static void SetFacility(Facility currentFacility)
        {
            //Load the inputed Facility into the local one
            facility = currentFacility;
        }

        /// <summary>
        /// Resets the facility screen
        /// </summary>
        public static void Reset()
        {
            facility = null;
            Core.facilityScreen.showTask = false;
            Core.facilityScreen.showWorkers = false;
        }

        /// <summary>
        /// Run this if the task is pressed
        /// </summary>
        public static void TaskClicked()
        {
            Core.facilityScreen.showTask = true;
            Core.facilityScreen.showWorkers = false;

        }

        /// <summary>
        /// Run this if the workers are clicked
        /// </summary>
        public static void WorkersClicked()
        {

            Core.facilityScreen.showWorkers = true;
            Core.facilityScreen.showTask = false;

        }

        /// <summary>
        /// Updates the facility HUD
        /// </summary>
        public static void Update()
        {

            if (ActionButtons.workerButton.Collision())
                WorkersClicked();

            if (ActionButtons.taskButton.Collision())
                TaskClicked();

        }

        /// <summary>
        /// Draws the Facility HUD
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used for drawing</param>
        public static void Draw(SpriteBatch spriteBatch)
        {

        }

    }

    static class PlanetHUD
    {

        static Planet planet;

        public static void CreatePlanetHUD()
        {

        }

        public static void SetPlanet(Planet currentPlanet)
        {
            planet = currentPlanet;

        }

        public static void Update()
        {
            if (!planet.hasColony)
                ActionButtons.explorationButtonActive = false;

            if (!ActionButtons.explorationButtonActive)
            {

            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {

        }
    }

    //Under here is specific parts of Huds
    class HUDList
    {
        //The texture of the list
        Texture2D texture;

        //The position of the list
        Vector2 position;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="position">1. Upper area of screen, 2. Lower area of screen</param>
        public HUDList(string assetName, int position)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>(assetName);

            if (position == 1)
                this.position = new Vector2((Settings.GetScreenRes.X / 2) - (texture.Width / 2), 0);

            if (position == 2)
                this.position = new Vector2((Settings.GetScreenRes.X / 2) - (texture.Width / 2), Settings.GetScreenRes.Y - texture.Height);
        }

        public Texture2D GetTexture()
        {
            return texture;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }

    static class ActionButtons
    {
        #region Buttons
        public static Button backButton;
        public static Button buildButton; //Done
        public static Button colonistButton; //Done
        public static Button explorationButton; //Done
        public static Button infoButton; //Done
        public static Button probeButton; //Done
        public static Button taskButton;
        public static Button workerButton;//Done
        #endregion

        public static bool isUsed;

        public static ButtonsList buttonList;

        #region Active-bools
        public static bool backButtonActive;
        public static bool buildButtonActive;
        public static bool colonistButtonActive;
        public static bool explorationButtonActive;
        public static bool infoButtonActive;
        public static bool probeButtonActive;
        public static bool taskButtonActive;
        public static bool workerButtonActive;
        #endregion


        public static void CreateActionButtons()
        {
            buttonList = new ButtonsList(8);

            infoButton = new Button("Res/PH/HUD/Buttons/HUD/InfoButtonPH", buttonList.GetPosition(0), 0, true);
            probeButton = new Button("Res/PH/HUD/Buttons/HUD/ProbeButtonPH", buttonList.GetPosition(1), 0, true);
            explorationButton = new Button("Res/PH/HUD/Buttons/HUD/ExplorationButtonPH", buttonList.GetPosition(2), 0, true);
            colonistButton = new Button("Res/PH/HUD/Buttons/HUD/ColonistButtonPH", buttonList.GetPosition(3), 0, true);
            buildButton = new Button("Res/PH/HUD/Buttons/HUD/BuildButtonPH", buttonList.GetPosition(4), 0, true);
            workerButton = new Button("Res/PH/HUD/Buttons/HUD/WorkerButtonPH", buttonList.GetPosition(5), 0, true);
            taskButton = new Button("Res/PH/HUD/Buttons/HUD/TaskButtonPH", buttonList.GetPosition(6), 0, true);
            backButton = new Button("Res/PH/HUD/Buttons/HUD/BackButtonPH", buttonList.GetPosition(7), 0, true);

            infoButtonActive = false;
            probeButtonActive = false;
            explorationButtonActive = false;
            colonistButtonActive = false;
            buildButtonActive = false;
            workerButtonActive = false;
            taskButtonActive = false;
            backButtonActive = true;
            isUsed = false;

        }

        public static void Update()
        {
            #region If active
            if (infoButtonActive)
            {
                infoButton.SetTexture(Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/HUD/InfoButtonPH"));
                infoButton.Update();

                if (Core.currentScreen == Core.galaxyScreen)
                    infoButton.SetCollision(infoButton.GetPosition() - Camera.position);
            }

            if (probeButtonActive)
            {
                probeButton.SetTexture(Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/HUD/ProbeButtonPH"));
                probeButton.Update();

            }


            if (explorationButtonActive)
            {
                explorationButton.SetTexture(Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/HUD/ExplorationButtonPH"));
                explorationButton.Update();

                if (Core.currentScreen == Core.galaxyScreen)
                    explorationButton.SetCollision(infoButton.GetPosition() - Camera.position);
            }

            if (colonistButtonActive)
            {
                colonistButton.SetTexture(Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/HUD/ColonistButtonPH"));
                colonistButton.Update();

                if (Core.currentScreen == Core.galaxyScreen)
                    colonistButton.SetCollision(infoButton.GetPosition() - Camera.position);
            }

            if (buildButtonActive)
            {
                buildButton.SetTexture(Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/HUD/BuildButtonPH"));
                buildButton.Update();

                if (Core.currentScreen == Core.galaxyScreen)
                    buildButton.SetCollision(infoButton.GetPosition() - Camera.position);
            }

            if (workerButtonActive)
            {
                workerButton.SetTexture(Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/HUD/WorkerButtonPH"));
                workerButton.Update();

                if (Core.currentScreen == Core.galaxyScreen)
                    workerButton.SetCollision(infoButton.GetPosition() - Camera.position);
            }

            if (taskButtonActive)
            {
                taskButton.SetTexture(Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/HUD/TaskButtonPH"));
                taskButton.Update();

                if (Core.currentScreen == Core.galaxyScreen)
                    taskButton.SetCollision(infoButton.GetPosition() - Camera.position);
            }

            if (backButtonActive)
            {
                backButton.SetTexture(Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/HUD/BackButtonPH"));
                backButton.Update();

                if (Core.currentScreen == Core.galaxyScreen)
                    backButton.SetCollision(infoButton.GetPosition() - Camera.position);
            }
            #endregion

            #region If inactive
            if (!infoButtonActive)
            {
                infoButton.SetTexture(Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/HUD/InfoButtonIAPH"));
                //infoButton.Update();
            }

            if (!probeButtonActive)
            {
                probeButton.SetTexture(Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/HUD/ProbeButtonIAPH"));
                //probeButton.Update();
            }

            if (!explorationButtonActive)
            {
                explorationButton.SetTexture(Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/HUD/ExplorationButtonIAPH"));
                //explorationButton.Update();
            }

            if (!colonistButtonActive)
            {
                colonistButton.SetTexture(Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/HUD/ColonistButtonIAPH"));
                //colonistButton.Update();
            }

            if (!buildButtonActive)
            {
                buildButton.SetTexture(Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/HUD/BuildButtonIAPH"));
                //buildButton.Update();
            }

            if (!workerButtonActive)
            {
                workerButton.SetTexture(Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/HUD/WorkerButtonIAPH"));
                //workerButton.Update();
            }

            if (!taskButtonActive)
            {
                taskButton.SetTexture(Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/HUD/TaskButtonIAPH"));
                //taskButton.Update();
            }
            #endregion

            buttonList.Update();



            #region Update Positions
            infoButton.SetPosition(buttonList.GetPosition(0));
            probeButton.SetPosition(buttonList.GetPosition(1));
            explorationButton.SetPosition(buttonList.GetPosition(2));
            colonistButton.SetPosition(buttonList.GetPosition(3));
            buildButton.SetPosition(buttonList.GetPosition(4));
            workerButton.SetPosition(buttonList.GetPosition(5));
            taskButton.SetPosition(buttonList.GetPosition(6));
            backButton.SetPosition(buttonList.GetPosition(7));
            #endregion

        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            buttonList.Draw(spriteBatch);
            infoButton.Draw(spriteBatch);
            probeButton.Draw(spriteBatch);
            explorationButton.Draw(spriteBatch);
            colonistButton.Draw(spriteBatch);
            buildButton.Draw(spriteBatch);
            workerButton.Draw(spriteBatch);
            taskButton.Draw(spriteBatch);
            backButton.Draw(spriteBatch);
        }
    }

    class ButtonsList
    {

        Texture2D top;
        Texture2D bottom;
        Texture2D[] middle;
        Vector2[] position;
        Rectangle collision;
        int height;
        int width;
        bool extended;



        public ButtonsList(int buttonAmount)
        {
            //Load the textures
            top = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/HUD/ButtonListTop");
            bottom = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/HUD/ButtonListBot");

            //Set the array of middle spaces texture to be the amount of middle textures needed
            middle = new Texture2D[buttonAmount];

            //Load the textures for the middle spaces
            for (int i = 0; i < middle.Length; i++)
            {
                middle[i] = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/HUD/ButtonListMid");
            }

            //Set the array of position to be the total amount of spaces (including top and bottom)
            position = new Vector2[buttonAmount + 2];


            //Set the total height and width
            height = (buttonAmount + 2) * top.Height;
            width = top.Width;

            //Extended should be false from the start
            extended = false;

            //Load the starting positions for all
            for (int i = 0; i < position.Length; i++)
            {
                position[i] = positionList(i);
            }

        }

        /// <summary>
        /// Positions the list one position at the time
        /// </summary>
        /// <param name="amount">The current number in loop</param>
        /// <returns>A vector 2 position</returns>
        private Vector2 positionList(int amount)
        {
            //Set the starting x value to be - texture with + 1
            int x = 0 - (top.Width - 1);
            //Set the starting y value to be dependant on the current one in loop (place each one under the previous)
            int y = (int)(Settings.GetScreenRes.Y / 2) - (int)(height / 2) + (amount * top.Height);

            //Return the position vector2
            return new Vector2(x, y);
        }

        /// <summary>
        /// Updates the list
        /// </summary>
        public void Update()
        {
            //Set the collision to not be depending on the camera if not in galaxy screen
            if (Core.currentScreen != Core.galaxyScreen)
                collision = new Rectangle((int)position[0].X, (int)positionList(0).Y, width + 1, height);

            //Set the collision to be depending on the camera if in the galaxy screen
            else if (Core.currentScreen == Core.galaxyScreen)
                collision = new Rectangle((int)(position[0].X - Camera.position.X), (int)(positionList(0).Y - Camera.position.Y), width + 1, height);

            //If the cursor is colliding with the collision rectangle, make the list pop-out
            if (collision.Intersects(Cursor.collision) && !extended)
            {
                for (int i = 0; i < position.Length; i++)
                {
                    position[i].X = MathHelper.Lerp(position[i].X, 0, 0.1f);
                }
            }

            //If the position of the last bit in loop is greater than or equal to zero the list is fully extended
            if (position[position.Length - 1].X >= 0)
            {
                extended = true;
            }
            //If not the list is not either
            else if (position[position.Length - 1].X < 0)
            {
                extended = false;
            }

            //If the cursor is not colliding with the list make it slide in again
            if (!collision.Intersects(Cursor.collision) && !ActionButtons.isUsed)
            {
                if (position[position.Length - 1].X > positionList(0).X)
                {
                    for (int i = 0; i < position.Length; i++)
                    {
                        position[i].X = MathHelper.Lerp(position[i].X, positionList(i).X, 0.1f);
                    }
                }

                if (position[position.Length - 1].X < positionList(0).X)
                {
                    for (int i = 0; i < position.Length; i++)
                    {
                        position[i].X = positionList(i).X;
                    }
                }
            }


        }

        /// <summary>
        /// Returns the position of the current button space
        /// </summary>
        /// <param name="number">The number of the button</param>
        /// <returns>A vector 2 position</returns>
        public Vector2 GetPosition(int number)
        {
            number += 1;
            return position[number];
        }

        /// <summary>
        /// Draws the list
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch used for drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(top, position[0], Color.White);
            spriteBatch.Draw(bottom, position[position.Length - 1], Color.White);
            for (int i = 0; i < middle.Length; i++)
            {
                spriteBatch.Draw(middle[i], position[i + 1], Color.White);
            }


        }


    }
}
