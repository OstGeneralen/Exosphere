using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Exosphere.Src.Handlers;
using Exosphere.Src;
using Exosphere.Src.Basebuilding;
using Exosphere.Src.Generators;
using Exosphere.Src.Screens;
using Exosphere.Src.HUD;
using Exosphere.Src.Exploring;


namespace Exosphere.Src
{
    static class Core
    {
        #region Tutorial
        #region Facilities

        public static bool showComArrayInfo;
        public static bool showEquipmentStoresInfo;
        public static bool showEvaluationInfo;
        public static bool showFoodStoresInfo;
        public static bool showGarageInfo;
        public static bool showGreenhouseInfo;
        public static bool showGymInfo;
        public static bool showHangarInfo;
        public static bool showLabInfo;
        public static bool showLibraryInfo;
        public static bool showLivingQuartersInfo;
        public static bool showMedBayInfo;
        public static bool showMineInfo;
        public static bool showMineralStoresInfo;
        public static bool showPumpInfo;
        public static bool showSolarPanelsInfo;
        public static bool showWorkshopInfo;

        #endregion

        #region Colony

        public static bool firstColonyFirstTime;

        #endregion

        #region Planet
        public static bool showPlanetScreenInfo;
        #endregion

        #region Galaxy
        public static bool showGalaxyScreenInfo;
        #endregion

        #region Load/Save
        public static TutorialsSave save;

        public static void LoadTutorials(TutorialsSave load)
        {
            showComArrayInfo = load.showComArrayInfoSave;
            showEquipmentStoresInfo = load.showEquipmentStoresInfoSave;
            showEvaluationInfo = load.showEvaluationInfoSave;
            showFoodStoresInfo = load.showFoodStoresInfoSave;
            showGarageInfo = load.showGarageInfoSave;
            showGreenhouseInfo = load.showGreenhouseInfoSave;
            showGymInfo = load.showGymInfoSave;
            showHangarInfo = load.showHangarInfoSave;
            showLabInfo = load.showLabInfoSave;
            showLibraryInfo = load.showLibraryInfoSave;
            showLivingQuartersInfo = load.showLivingQuartersInfoSave;
            showMedBayInfo = load.showMedBayInfoSave;
            showMineInfo = load.showMineInfoSave;
            showMineralStoresInfo = load.showMineralStoresInfoSave;
            showPumpInfo = load.showPumpInfoSave;
            showSolarPanelsInfo = load.showSolarPanelsInfoSave;
            showWorkshopInfo = load.showWorkshopInfoSave;

            firstColonyFirstTime = load.firstColonyFirstTimeSave;
            showGalaxyScreenInfo = load.showGalaxyScreenInfoSave;
            showPlanetScreenInfo = load.showPlanetScreenInfoSave;
        }

        public static void SaveTutorials()
        {
            save.showComArrayInfoSave = showComArrayInfo;
            save.showEquipmentStoresInfoSave = showEquipmentStoresInfo;
            save.showEvaluationInfoSave = showEvaluationInfo;
            save.showFoodStoresInfoSave = showFoodStoresInfo;
            save.showGarageInfoSave = showGarageInfo;
            save.showGreenhouseInfoSave = showGreenhouseInfo;
            save.showGymInfoSave = showGymInfo;
            save.showHangarInfoSave = showHangarInfo;
            save.showLabInfoSave = showLabInfo;
            save.showLibraryInfoSave = showLibraryInfo;
            save.showLivingQuartersInfoSave = showLivingQuartersInfo;
            save.showMedBayInfoSave = showMedBayInfo;
            save.showMineInfoSave = showMineInfo;
            save.showMineralStoresInfoSave = showMineralStoresInfo;
            save.showPumpInfoSave = showPumpInfo;
            save.showSolarPanelsInfoSave = showSolarPanelsInfo;
            save.showWorkshopInfoSave = showWorkshopInfo;

            save.firstColonyFirstTimeSave = firstColonyFirstTime;
            save.showGalaxyScreenInfoSave = showGalaxyScreenInfo;
            save.showPlanetScreenInfoSave = showPlanetScreenInfo;
        }
        #endregion
        #endregion

        public static Screen currentScreen;

        public static PlanetScreen planetScreen;
        public static GalaxyScreen galaxyScreen;
        public static ColonyScreen colonyScreen;
        public static FacilityScreen facilityScreen;

        public static TextBox currentMessageBox;
        public static bool choiceBoxChoice;

        public static ScrollBox currentScrollBox;

        private static List<Colony> colonies;

        public static void CreateCore()
        {
            #region Tutorial
            #region Facilities

            showComArrayInfo = true;
            showEquipmentStoresInfo = true;
            showEvaluationInfo = true;
            showFoodStoresInfo = true;
            showGarageInfo = true;
            showGreenhouseInfo = true;
            showGymInfo = true;
            showHangarInfo = true;
            showLabInfo = true;
            showLibraryInfo = true;
            showLivingQuartersInfo = true;
            showMedBayInfo = true;
            showMineInfo = true;
            showMineralStoresInfo = true;
            showPumpInfo = true;
            showSolarPanelsInfo = true;
            showWorkshopInfo = true;

            #endregion

            #region Colony
            firstColonyFirstTime = true;
            #endregion

            #region Planet
            showPlanetScreenInfo = true;
            #endregion

            #region Galaxy
            showGalaxyScreenInfo = true;
            #endregion
            #endregion

            HUD.HUD.CreateHud();
            planetScreen = new PlanetScreen();
            galaxyScreen = new GalaxyScreen();
            colonyScreen = new ColonyScreen();
            facilityScreen = new FacilityScreen();

            colonies = new List<Colony>();

            SetGalaxyView();
        }

        /// <summary>
        /// Loads a core. Run if the game is loaded
        /// </summary>
        /// <param name="loadGalaxy">The saved galaxy</param>
        /// <param name="loadColonies">The saved list of colonies</param>
        public static void LoadCore(Galaxy loadGalaxy, List<Colony> loadColonies)
        {
            galaxyScreen.galaxy = loadGalaxy;
            colonies = loadColonies;
        }

        /// <summary>
        /// Gets the list containing the colonies
        /// </summary>
        /// <returns>Return the list containing the colonies</returns>
        public static List<Colony> GetColonies()
        {
            List<Colony> tempColonies = colonies;
            return tempColonies;
        }

        public static void AddColony(Colony colony)
        {
            if (!colonies.Contains(colony))
                colonies.Add(colony);
        }

        public static void SetPlanetView(Planet planet)
        {
            MouseHandler.Update();
            currentScreen = planetScreen;

            HUD.HUD.SetScreenString("Planet View");
            PlanetHUD.SetPlanet(planet);

            PlanetHUD.CreatePlanetHUD();

            planetScreen.SetPlanet(planet);

        }

        public static void SetColonyView(Colony colony)
        {
            MouseHandler.Update();
            currentScreen = colonyScreen;

            HUD.HUD.SetScreenString("Colony View");

            ColonyHUD.SetColony(colony);

            colonyScreen.SetColony(colony);

        }

        public static void SetGalaxyView()
        {

            HUD.HUD.SetScreenString("Galaxy View");

            MouseHandler.Update();
            currentScreen = galaxyScreen;

        }

        public static void SetFacilityView(Facility facility)
        {
            MouseHandler.Update();

            HUD.HUD.SetScreenString(facility.GetFacilityType() + " View");

            FacilityHUD.SetFacility(facility);

            facilityScreen.SetFacility(facility);
            currentScreen = facilityScreen;
        }


        public static void Update()
        {

            if (HUD.ActionButtons.backButton.Collision())
                HUD.ActionButtons.isUsed = false;


            if (currentMessageBox != null)
            {
                currentMessageBox.Update();
                MouseHandler.Update();
            }

            if (currentScrollBox != null)
            {
                currentScrollBox.Update();
                MouseHandler.Update();
            }

            if (currentMessageBox == null || currentMessageBox.GetType() == typeof(InfoBox) && currentScrollBox == null)
            {
                currentMessageBox = null;

                if (TimeHandler.newTurn)
                {
                    for (int i = 0; i < TimeHandler.passedDays; i++)
                    {
                        foreach (var colony in colonies)
                        {
                            colony.Update();
                            colony.GetPlanet().Update();
                        }
                        facilityScreen.Update();
                    }
                    galaxyScreen.Update();

                    TimeHandler.MidTurn();
                }

                if (currentScreen == planetScreen)
                {
                    planetScreen.Update();
                }

                if (currentScreen == galaxyScreen)
                {
                    galaxyScreen.Update();
                }

                if (currentScreen == colonyScreen)
                {
                    colonyScreen.Update();
                }

                if (currentScreen == facilityScreen)
                {
                    facilityScreen.Update();
                }

                if (currentScreen != facilityScreen)
                {
                    facilityScreen.workerButtons.Clear();
                }

                if (KeyboardHandler.PressedOnce(Keys.Space))
                {
                    TimeHandler.EndTurn(1);
                }
            }
        }


        public static void Draw(SpriteBatch spriteBatch)
        {

            if (currentScreen == planetScreen)
            {
                planetScreen.Draw(spriteBatch);
            }

            if (currentScreen == galaxyScreen)
            {
                galaxyScreen.Draw(spriteBatch);
            }

            if (currentScreen == colonyScreen)
            {
                colonyScreen.Draw(spriteBatch);
            }

            if (currentScreen == facilityScreen)
            {
                facilityScreen.Draw(spriteBatch);
            }

        }
    }

    public struct TutorialsSave
    {
        public bool showComArrayInfoSave;
        public bool showEquipmentStoresInfoSave;
        public bool showEvaluationInfoSave;
        public bool showFoodStoresInfoSave;
        public bool showGarageInfoSave;
        public bool showGreenhouseInfoSave;
        public bool showGymInfoSave;
        public bool showHangarInfoSave;
        public bool showLabInfoSave;
        public bool showLibraryInfoSave;
        public bool showLivingQuartersInfoSave;
        public bool showMedBayInfoSave;
        public bool showMineInfoSave;
        public bool showMineralStoresInfoSave;
        public bool showPumpInfoSave;
        public bool showSolarPanelsInfoSave;
        public bool showWorkshopInfoSave;

        public bool firstColonyFirstTimeSave;

        public bool showPlanetScreenInfoSave;

        public bool showGalaxyScreenInfoSave;
    }
}
