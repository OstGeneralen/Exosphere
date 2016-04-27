using Exosphere.Src.Basebuilding.Facilities;
using Exosphere.Src.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exosphere.Src.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exosphere.Src.Screens;
using Exosphere.Src.Generators;
using Exosphere.Src.HUD;

namespace Exosphere.Src.Basebuilding
{
    public class Grid
    {
        #region Variables

        //An array containing the cells in the grid
        Cell[] cellArray;

        //The position of the cell targeted
        Point cellPosition;

        public int width;
        public int height;

        //Handles resource management
        public ResourceManager resourceManager;
        public EnergyManager energyManager;

        //A dicitionary that keeps track of the amount of all the different facilities.
        SerializableDictionary<string, int> facilityCount;

        public Colony colony;

        public int facilityID;

        #region Free Start Facilities
        public bool freeSolarPanels;
        public bool freeLivingQuarters;
        public bool freePump;
        public bool freeGreenHouse;
        public bool freeMine;
        public bool freeFoodStore;
        public int storedUsedEnergy;
        #endregion

        public int dailyWaterRevenue;
        public int dailyFoodRevenue;

        #endregion

        #region Save/Load

        public GridSave save;

        #region Load Grid

        public void LoadGrid(GridSave load)
        {
            resourceManager = new ResourceManager();
            resourceManager.LoadResourceManager(load.resourceManagerSave);

            energyManager = new EnergyManager();
            energyManager.LoadEnergyManager(load.energyManagerSave);

            Texture2D tempCellTexture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/gridPH");

            facilityID = load.facilityID;

            for (int i = 0; i < load.cellSaveArray.Length; i++)
            {
                cellArray[i] = new Cell(
                    new Point((int)(load.cellSaveArray[i].position.X / tempCellTexture.Width),
                    (int)(load.cellSaveArray[i].position.Y / tempCellTexture.Height)), this);
                cellArray[i].LoadCell(load.cellSaveArray[i]);
            }

            facilityCount = load.facilityCountSave;
        }

        #endregion

        public void SaveGrid()
        {
            resourceManager.SaveResourceManager();
            save.resourceManagerSave = resourceManager.save;

            energyManager.SaveEnergyManager();
            save.energyManagerSave = energyManager.save;

            save.height = height;
            save.width = width;

            save.cellSaveArray = new CellSave[height * width];
            save.facilityID = facilityID;

            //Cell
            for (int i = 0; i < cellArray.Length; i++)
            {
                cellArray[i].SaveCell();
                save.cellSaveArray[i] = cellArray[i].save;
            }

            save.facilityCountSave = facilityCount;
        }

        #endregion

        /// <summary>
        /// Creates a new grid of the defined size
        /// </summary>
        /// <param name="width">The width of the grid</param>
        /// <param name="height">The height of the grid</param>
        public Grid(int width, int height, Colony colony)
        {

            freeSolarPanels = false;
            freeLivingQuarters = false;
            freePump = false;
            freeGreenHouse = false;
            freeMine = false;
            freeFoodStore = false;

            //Check if this is the first colony
            if (Core.GetColonies().Count == 0 && colony.ID == 1)
            {
                freeGreenHouse = true;
                freeLivingQuarters = true;
                freePump = true;
                freeSolarPanels = true;
                freeMine = true;
                freeFoodStore = true;
                storedUsedEnergy = 0;
            }

            facilityID = 0;
            //Creates the dictionary for facility counting
            facilityCount = new SerializableDictionary<string, int>();

            this.height = height;
            this.width = width;
            //Sets the size of the grid in 2D to be widht * height
            int size = width * height;

            //Sets the size of the cell array to the size
            cellArray = new Cell[size];

            //Starts of with the position (0,0)
            cellPosition = new Point(0, 0);

            resourceManager = new ResourceManager();
            energyManager = new EnergyManager();
            this.colony = colony;

            //Loops through the array of cells to create a new cell in each spot
            for (int i = 0; i < cellArray.Length; i++)
            {
                //Adds a new cell
                cellArray[i] = new Cell(cellPosition, this);

                //Adds one to the X position of the cell for each loop
                cellPosition.X++;

                //Resets the X position when maximum is reached and adds one to Y
                if (cellPosition.X == width)
                {
                    cellPosition.Y++;
                    cellPosition.X = 0;
                }
            }
        }

        /// <summary>
        /// Returns the amount of the specified facility on the colony
        /// </summary>
        /// <param name="key">The name of the facility type you want to count</param>
        /// <returns>Returns an int of how many of the specified facility there is</returns>
        public int GetFacilityAmount(string key)
        {
            //Checks so that the key exists
            if (facilityCount.ContainsKey(key))
                return facilityCount[key];

            //Returns 0 if the facility is not counted or does not exist
            return 0;
        }

        /// <summary>
        /// Updates the grid
        /// </summary>
        /// <param name="cursor">The cursor used in the game</param>
        public void Update()
        {

            dailyWaterRevenue = 0;
            dailyFoodRevenue = 0;

            //Loops through each cell
            for (int i = 0; i < cellArray.Length; i++)
            {
                //Updates the cell currently active in the loop
                cellArray[i].Update(resourceManager, energyManager);

                CountFacilities(cellArray[i]);

                energyManager.SetTotalEnergy(this, cellArray[i].GetFacility());

                if (cellArray[i].GetFacility() != null)
                {
                    if (cellArray[i].GetFacility().GetType() == typeof(Greenhouse))
                    {
                        dailyFoodRevenue += cellArray[i].GetFacility().Task(dailyFoodRevenue);
                    }

                    if (cellArray[i].GetFacility().GetType() == typeof(Pump))
                    {
                        dailyWaterRevenue += cellArray[i].GetFacility().Task(dailyWaterRevenue);
                    }
                }

            }

            energyManager.AddUsedPower(storedUsedEnergy);
            storedUsedEnergy = 0;

            if (TimeHandler.newTurn)
            {
                resourceManager.FoodConsumption(colony);
            }
        }

        /// <summary>
        /// Adds the facility in the cell to the facility counter
        /// </summary>
        /// <param name="cell">The current cell</param>
        private void CountFacilities(Cell cell)
        {

            //A string to contain the name of the facility type
            string facilityType;

            //Sets the name of the facility type to be the name of the facility in the active cell
            facilityType = cell.GetFacilityType();

            //Checks so that the facility has not been counted before
            if (!cell.isCounted)
            {
                //Checks so that there is a facility on the cell
                if (facilityType != null)
                {
                    //Checks so that the facility type exists as a key in the dictionary
                    if (facilityCount.ContainsKey(facilityType))
                    {
                        //Checks if there have been none of this facility until the current loop
                        if (facilityCount[facilityType] == 0)
                        {
                            //Sets facility to equal one
                            facilityCount[facilityType] = 1;

                            //Sets the isCounted to true
                            cell.isCounted = true;

                        }
                        //Checks if the facility type have been counted earlier
                        else if (facilityCount[facilityType] != 0)
                        {
                            //Adds one to the facility count
                            facilityCount[facilityType] += 1;

                            //Sets the isCounted to true
                            cell.isCounted = true;
                        }
                    }

                    //Checks if the facility type name has not been counted before
                    if (!facilityCount.ContainsKey(facilityType))
                    {
                        //Creates the key for the facility type name
                        facilityCount.Add(facilityType, 1);

                        //Sets the isCounted to true
                        cell.isCounted = true;
                    }
                }
            }
        }

        /// <summary>
        /// Draw the grid
        /// </summary>
        /// <param name="spriteBatch">The spritebatch used to draw</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //Loops through and draws each cell
            for (int i = 0; i < cellArray.Length; i++)
            {
                cellArray[i].Draw(spriteBatch);
            }
        }
    }

    public struct GridSave
    {
        public ResourceManagerSave resourceManagerSave;
        public EnergyManagerSave energyManagerSave;
        public int width;
        public int height;

        public SerializableDictionary<string, int> facilityCountSave;

        public CellSave[] cellSaveArray;

        public int facilityID;
    }

    class Cell
    {
        #region Variables

        //A bool to check if the area is used
        bool isUsed;

        public bool isCounted;

        //A variable to contain the facility placed at the cell
        Facility facility;

        Grid grid;

        //The texture of the cell
        Texture2D texture;

        //The position in pixels of the cell
        Vector2 position;

        Rectangle collision;

        int layer;

        #endregion

        #region Save/Load

        public CellSave save;

        #region Load Cell

        public void LoadCell(CellSave load)
        {
            position = load.position;
            isCounted = load.isCounted;
            isUsed = load.isUsed;

            if (load.isUsed)
            {
                switch (load.facilityType)
                {
                    case "ComArray":
                        facility = new ComArray(position, grid.colony, grid.colony.ID);
                        facility.LoadFacility(load.comArraySave);
                        break;
                    case "EquipmentStores":
                        facility = new EquipmentStores(position, grid.colony, grid.colony.ID);
                        facility.LoadFacility(load.equipmentStoresSave);
                        break;
                    case "Evaluation":
                        facility = new Evaluation(position, grid.colony, grid.colony.ID);
                        facility.LoadFacility(load.evaluationFacilitySave);
                        break;
                    case "FoodStores":
                        facility = new FoodStores(position, grid.colony, grid.colony.ID);
                        facility.LoadFacility(load.foodStoresSave);
                        break;
                    case "Garage":
                        facility = new Garage(position, grid.colony, grid.colony.ID);
                        facility.LoadFacility(load.garageSave);
                        break;
                    case "Greenhouse":
                        facility = new Greenhouse(position, grid.colony, grid.colony.ID);
                        facility.LoadFacility(load.greenhouseSave);
                        break;
                    case "Gym":
                        facility = new Gym(position, grid.colony, grid.colony.ID);
                        facility.LoadFacility(load.gymSave);
                        break;
                    case "Hangar":
                        facility = new Hangar(position, grid.colony, grid.colony.ID);
                        facility.LoadFacility(load.hangarSave);
                        break;
                    case "Lab":
                        facility = new Lab(position, grid.colony, grid.colony.ID);
                        facility.LoadFacility(load.labSave);
                        break;
                    case "Library":
                        facility = new Library(position, grid.colony, grid.colony.ID);
                        facility.LoadFacility(load.librarySave);
                        break;
                    case "LivingQuarters":
                        facility = new LivingQuarters(position, grid.colony, grid.colony.ID);
                        facility.LoadFacility(load.livingQuartersSave);
                        break;
                    case "MedBay":
                        facility = new MedBay(position, grid.colony, grid.colony.ID);
                        facility.LoadFacility(load.medBaySave);
                        break;
                    case "Mine":
                        facility = new Mine(position, grid.colony, grid.colony.ID);
                        facility.LoadFacility(load.mineSave);
                        break;
                    case "MineralStores":
                        facility = new MineralStores(position, grid.colony, grid.colony.ID);
                        facility.LoadFacility(load.mineralStoresSave);
                        break;
                    case "Refinery":
                        facility = new Refinery(position, grid.colony, grid.colony.ID);
                        facility.LoadFacility(load.refinerySave);
                        break;
                    case "SolarPanels":
                        facility = new SolarPanels(position, grid.colony, grid.colony.ID);
                        facility.LoadFacility(load.solarPanelsSave);
                        break;
                    case "Workshop":
                        facility = new Workshop(position, grid.colony, grid.colony.ID);
                        facility.LoadFacility(load.workshopSave);
                        break;
                    case "Pump":
                        facility = new Pump(position, grid.colony, grid.colony.ID);
                        facility.LoadFacility(load.pumpSave);
                        break;
                    default: throw new Exception("Couldn't load the facility");
                }
            }

            SetValues();
        }

        #endregion

        public void SaveCell()
        {
            save.position = position;
            save.isCounted = isCounted;
            save.isUsed = isUsed;

            if (isUsed)
            {
                save.facilityType = facility.GetFacilityType();
                facility.SaveFacility();
                switch (facility.GetFacilityType())
                {
                    case "ComArray":
                        save.comArraySave = facility.comArraySave;
                        break;
                    case "EquipmentStores":
                        save.equipmentStoresSave = facility.equipmentStoresSave;
                        break;
                    case "Evaluation":
                        save.evaluationFacilitySave = facility.evaluationFacilitySave;
                        break;
                    case "FoodStores":
                        save.foodStoresSave = facility.foodStoresSave;
                        break;
                    case "Garage":
                        save.garageSave = facility.garageSave;
                        break;
                    case "Greenhouse":
                        save.greenhouseSave = facility.greenhouseSave;
                        break;
                    case "Gym":
                        save.gymSave = facility.gymSave;
                        break;
                    case "Hangar":
                        save.hangarSave = facility.hangarSave;
                        break;
                    case "Lab":
                        save.labSave = facility.labSave;
                        break;
                    case "Library":
                        save.librarySave = facility.librarySave;
                        break;
                    case "LivingQuarters":
                        save.livingQuartersSave = facility.livingQuartersSave;
                        break;
                    case "MedBay":
                        save.medBaySave = facility.medBaySave;
                        break;
                    case "Mine":
                        save.mineSave = facility.mineSave;
                        break;
                    case "MineralStores":
                        save.mineralStoresSave = facility.mineralStoresSave;
                        break;
                    case "Refinery":
                        save.refinerySave = facility.refinerySave;
                        break;
                    case "SolarPanels":
                        save.solarPanelsSave = facility.solarPanelsSave;
                        break;
                    case "Workshop":
                        save.workshopSave = facility.workshopSave;
                        break;
                    case "Pump":
                        save.pumpSave = facility.pumpSave;
                        break;
                    default: throw new Exception("Couldn't save the facility");
                }
            }
        }

        #endregion

        /// <summary>
        /// Creates a new cell
        /// </summary>
        /// <param name="gridPosition">The position in the grid of the cell</param>
        public Cell(Point gridPosition, Grid grid, int layer = -1)
        {
            //Sets the facility to null in a new cell
            facility = null;

            //Sets that grid has no facility from the start
            isUsed = false;

            isCounted = false;

            //Sets the texture
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/gridPH");

            this.layer = layer;

            //Calculates the pixelposition of the grid position
            position.X = gridPosition.X * texture.Width + (Settings.GetScreenRes.X - (grid.width * texture.Width)) / 2;
            position.Y = gridPosition.Y * texture.Height + (HUD.HUD.informationList.GetTexture().Height * 1.5f);

            SetValues();

            this.grid = grid;

        }

        private void SetValues()
        {
            collision = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public Facility GetFacility()
        {
            return facility;
        }

        /// <summary>
        /// Updates the cell
        /// </summary>
        /// <param name="cursor">The cursor used in the game</param>
        public void Update(ResourceManager resourceManager, EnergyManager energyManager)
        {
            //If the cell is used the facility will be updated instead of the cell
            if (isUsed)
            {
                facility.Update();
            }

            if (ColonyHUD.showFacilityChoices)
            {
                if (ColonyHUD.GetFacilityChoiceCategory() != null)
                    ColonyHUD.GetFacilityChoiceCategory().FacilityChoiceUpdate();

                //If the cell is not used the cell is updated
                if (Cursor.collision.Intersects(collision) && MouseHandler.LMBOnce() && !isUsed && !ColonyHUD.GetFacilityChoiceCategory().GetFacilitychoise().Collision())
                {
                    #region Add facilities
                    if (ColonyScreen.facilityHold == "Lab")
                    {
                        AddFacility(new Lab(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "Gym")
                    {
                        AddFacility(new Gym(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "ComArray" && grid.GetFacilityAmount("ComArray") < 1)
                    {
                        AddFacility(new ComArray(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "EquipmentStores")
                    {
                        AddFacility(new EquipmentStores(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "Evaluation")
                    {
                        AddFacility(new Evaluation(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "FoodStores")
                    {
                        AddFacility(new FoodStores(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "Garage")
                    {
                        AddFacility(new Garage(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "Greenhouse")
                    {
                        AddFacility(new Greenhouse(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "Hangar")
                    {
                        AddFacility(new Hangar(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "Library")
                    {
                        AddFacility(new Library(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "LivingQuarters")
                    {
                        AddFacility(new LivingQuarters(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "MedBay")
                    {
                        AddFacility(new MedBay(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "Mine")
                    {
                        AddFacility(new Mine(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "MineralStores")
                    {
                        AddFacility(new MineralStores(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "Refinery")
                    {
                        AddFacility(new Refinery(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "SolarPanels")
                    {
                        AddFacility(new SolarPanels(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "Workshop")
                    {
                        AddFacility(new Workshop(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }
                    if (ColonyScreen.facilityHold == "Pump")
                    {
                        AddFacility(new Pump(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }
                    #endregion

                    grid.facilityID++;

                    //Set the facility hold to null to prevent double building of facilities
                    ColonyScreen.facilityHold = null;

                    ColonyHUD.showFacilityChoices = false;
                    ActionButtons.isUsed = false;
                }
            }
            else
            {
                //If the cell is not used the cell is updated
                if (Cursor.collision.Intersects(collision) && MouseHandler.LMBOnce() && !isUsed)
                {
                    #region Add facilities
                    if (ColonyScreen.facilityHold == "Lab")
                    {
                        AddFacility(new Lab(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "Gym")
                    {
                        AddFacility(new Gym(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "ComArray" && grid.GetFacilityAmount("ComArray") < 1)
                    {
                        AddFacility(new ComArray(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "EquipmentStores")
                    {
                        AddFacility(new EquipmentStores(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "Evaluation")
                    {
                        AddFacility(new Evaluation(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "FoodStores")
                    {
                        AddFacility(new FoodStores(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "Garage")
                    {
                        AddFacility(new Garage(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "Greenhouse")
                    {
                        AddFacility(new Greenhouse(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "Hangar")
                    {
                        AddFacility(new Hangar(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "Library")
                    {
                        AddFacility(new Library(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "LivingQuarters")
                    {
                        AddFacility(new LivingQuarters(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "MedBay")
                    {
                        AddFacility(new MedBay(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "Mine")
                    {
                        AddFacility(new Mine(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "MineralStores")
                    {
                        AddFacility(new MineralStores(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "Refinery")
                    {
                        AddFacility(new Refinery(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "SolarPanels")
                    {
                        AddFacility(new SolarPanels(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }

                    if (ColonyScreen.facilityHold == "Workshop")
                    {
                        AddFacility(new Workshop(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }
                    if (ColonyScreen.facilityHold == "Pump")
                    {
                        AddFacility(new Pump(position, grid.colony, grid.facilityID), resourceManager, energyManager);
                    }
                    #endregion

                    grid.facilityID++;

                    //Set the facility hold to null to prevent double building of facilities
                    ColonyScreen.facilityHold = null;
                }
            }

            //Runs the extraction function of the resource manger
            if (TimeHandler.newTurn)
            {
                resourceManager.Extraction(facility);
                resourceManager.Storageroom(grid, facility);
                energyManager.SetTotalEnergy(grid, facility);
                if (facility != null)
                {
                    facility.Maintenance(resourceManager.iron, resourceManager.copper, resourceManager.carbon);
                    if (facility.GetFacilityType() == "Gym" || facility.GetFacilityType() == "Library")
                        //Uses value that represents null
                        facility.Task("");
                    if (facility.GetFacilityType() == "Lab")
                        facility.Task("");
                    if (facility.GetFacilityType() == "Evaluation")
                        facility.Task("");
                    if (facility.GetFacilityType() == "Greenhouse")
                        facility.Task(this.grid);
                    if (facility.GetFacilityType() == "ComArray")
                        facility.Task("");
                    if (facility.GetFacilityType() == "Workshop")
                        facility.Task("");
                    if (facility.GetFacilityType() == "Hangar")
                        facility.Task("");
                }
            }
        }

        /// <summary>
        /// Adds a facility to the cell
        /// </summary>
        /// <param name="facility">The facility type that should be added</param>
        public void AddFacility(Facility facility, ResourceManager resourceManager, EnergyManager energyManager)
        {
            if (facility.GetType() == typeof(SolarPanels) && grid.freeSolarPanels)
            {
                this.facility = facility;

                grid.facilityID++;

                isUsed = true;

                grid.freeSolarPanels = false;

                SolarPanels tempSolarPanel = new SolarPanels(Vector2.Zero, null, 0);

                grid.storedUsedEnergy += tempSolarPanel.GetCostEnergy();
            }
            else if (facility.GetType() == typeof(LivingQuarters) && grid.freeLivingQuarters)
            {
                this.facility = facility;

                grid.facilityID++;

                isUsed = true;

                grid.freeLivingQuarters = false;

                LivingQuarters tempLivingQuarters = new LivingQuarters(Vector2.Zero, null, 0);

                grid.storedUsedEnergy += tempLivingQuarters.GetCostEnergy();
            }
            else if (facility.GetType() == typeof(Pump) && grid.freePump)
            {
                this.facility = facility;

                grid.facilityID++;

                isUsed = true;

                grid.freePump = false;

                Pump tempPump = new Pump(Vector2.Zero, null, 0);

                grid.storedUsedEnergy += tempPump.GetCostEnergy();

            }
            else if (facility.GetType() == typeof(Greenhouse) && grid.freeGreenHouse)
            {
                this.facility = facility;

                grid.facilityID++;

                isUsed = true;

                grid.freeGreenHouse = false;

                Greenhouse tempGreenhouse = new Greenhouse(Vector2.Zero, null, 0);
                grid.storedUsedEnergy += tempGreenhouse.GetCostEnergy();
            }
            else if (facility.GetType() == typeof(Mine) && grid.freeMine)
            {
                this.facility = facility;

                grid.facilityID++;

                isUsed = true;

                grid.freeMine = false;

                Mine tempMine = new Mine(Vector2.Zero, null, 0);
                grid.storedUsedEnergy += tempMine.GetCostEnergy();
            }
            else if (facility.GetType() == typeof(FoodStores) && grid.freeFoodStore)
            {
                this.facility = facility;

                grid.facilityID++;

                isUsed = true;

                grid.freeFoodStore = false;

                FoodStores tempFoodStores = new FoodStores(Vector2.Zero, null, 0);
                grid.storedUsedEnergy += tempFoodStores.GetCostEnergy();
            }
            //Returns a bool to check if you can biuld your requested facility
            else if (resourceManager.BuildFacility(facility) && energyManager.BuildFacility(facility))
            {
                //Sets the class facility variable to the local one
                this.facility = facility;

                grid.facilityID++;

                //Sets that the facility is no used
                isUsed = true;
            }
        }

        /// <summary>
        /// Returns the facility name of the facility placed on the cell
        /// </summary>
        /// <returns>the name of the facility as a string. Returns null if no facility is placed on the cell</returns>
        public string GetFacilityType()
        {
            if (facility != null)
            {
                return facility.GetFacilityType();

            }

            return null;
        }

        /// <summary>
        /// Draws the cell
        /// </summary>
        /// <param name="spriteBatch">The spritebatch that should be used for drawin</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //If there is no facility on the cell the cell will be drawn
            if (!isUsed)
            {
                spriteBatch.Draw(texture, position, Color.White);
            }

            //If there is a facility on the cell the facility will be drawn
            if (isUsed)
            {
                facility.Draw(spriteBatch);
            }
        }
    }

    public struct CellSave
    {
        public Vector2 position;
        public bool isUsed;
        public bool isCounted;
        public string facilityType;

        public ComArraySave comArraySave;
        public EquipmentStoresSave equipmentStoresSave;
        public EvaluationFacilitySave evaluationFacilitySave;
        public FoodStoresSave foodStoresSave;
        public GarageSave garageSave;
        public GreenhouseSave greenhouseSave;
        public GymSave gymSave;
        public HangarSave hangarSave;
        public LabSave labSave;
        public LibrarySave librarySave;
        public LivingQuartersSave livingQuartersSave;
        public MedBaySave medBaySave;
        public MineSave mineSave;
        public MineralStoresSave mineralStoresSave;
        public RefinerySave refinerySave;
        public SolarPanelsSave solarPanelsSave;
        public WorkshopSave workshopSave;
        public PumpSave pumpSave;
    }
}
