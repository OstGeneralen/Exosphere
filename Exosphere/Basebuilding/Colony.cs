using Exosphere.Src.Exploring;
using Exosphere.Src.Generators;
using Exosphere.Src.Handlers;
using Exosphere.Src.Items;
using Exosphere.Src.Items.Vehicles;
using Exosphere.Src.ResearchProject;
using Exosphere.Src.ResearchProject.Buffs;
using Exosphere.Src.ResearchProject.FacilitiesResearch;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Exosphere.Src.Basebuilding
{
    public class Colony
    {
        #region Variables

        #region Basic

        public string name;

        #endregion

        #region Size

        //Adds a grid to the colony
        public Grid grid;

        #endregion

        #region Housing

        //The maximum amount of residents in the colony
        public int housing;

        //TODO: The livingQuarterHousing should be set in the living quarters
        //The maximum amount of colonists that can be stacked in a living quarters
        public int livingQuarterHousing = 25;

        #endregion

        #region Colonists

        //A list containing all inhabitants in the colony
        public List<Colonist> inhabitants;
        //A list used to remove dead colonists
        public List<Colonist> removeInhabitant;



        #endregion

        #region Research

        //A list containing finished research projects
        public List<Research> research;
        //A dictionary containing the most renect discoveries that can be accesed by using a key(the name of the research project)
        public SerializableDictionary<string, Research> researchDictionary;
        //A list containing possible research options
        public List<Research> canResearch;

        #endregion

        #region Diseases

        //A list containing diseases currently running freely in the colony
        public List<Disease> diseases;

        public List<Disease> removeDisease;

        //A list of produced vaccines against diseases the researchers have studied
        public List<Vaccine> vaccines;

        int diseaseRisk;

        int diseaseMaxRandom;

        #endregion

        #region Location

        //The planet which surface the colony is built on
        public Planet planet;
        public Vector2 position;
        private Biome underlyingBiome;
        private Texture2D underlyingBiomeTexture;
        private bool hasUnderlyingBiome;
        private Vector2[] underLyingBiomePosition;

        #endregion

        #region Com-Array

        //A list containing colonies the com array has stable connection with
        public List<Colony> colonies;

        #endregion

        #region Manufactured Stuff

        public List<Item> canAssemble;
        public List<Item> items;
        public List<Vehicle> vehicles;

        #endregion

        //Save variables
        public int ID;

        #endregion

        #region Save/Load

        public ColonySave save;

        #region Load Colony

        public void LoadColony(ColonySave load)
        {
            hasUnderlyingBiome = false;

            this.underlyingBiome.LoadBiome(load.underLyingBiomeSave);

            canAssemble.Clear();
            for (int i = 0; i < load.canAssembleSaves.Count; i++)
            {
                canAssemble.Add(new NullItem());

                canAssemble[i].LoadItem(load.canAssembleSaves[i]);
            }

            canResearch.Clear();
            for (int i = 0; i < load.canResearchSaves.Count; i++)
            {
                canResearch.Add(new FacilityResearch(0, 0));

                canResearch[i].LoadResearch(load.canResearchSaves[i]);
            }

            diseases.Clear();
            for (int i = 0; i < load.diseaseSaves.Count; i++)
            {
                diseases.Add(new Disease());

                diseases[i].LoadDisease(load.diseaseSaves[i]);
            }

            inhabitants.Clear();

            for (int i = 0; i < load.inhabitantSaves.Count; i++)
            {
                inhabitants.Add(new Colonist(this, ID));

                inhabitants[i].LoadColonist(load.inhabitantSaves[i], this);
            }

            for (int i = 0; i < load.itemSaves.Count; i++)
            {
                if (items[i] == null)
                    items.Add(new NullItem());

                items[i].LoadItem(load.itemSaves[i]);
            }

            List<string> keys = new List<string>();

            foreach (var project in load.researchDictionarySaves)
            {
                keys.Add(project.Key);
            }

            for (int i = 0; i < keys.Count; i++)
            {
                if (researchDictionary[keys[i]] == null)
                    researchDictionary.Add(keys[i], new FacilityResearch(0, 0));

                researchDictionary[keys[1]].LoadResearch(load.researchDictionarySaves[keys[i]]);
            }

            //Clear the research list to allow the loading of saved researches
            research.Clear();

            for (int i = 0; i < load.researchSaves.Count; i++)
            {
                //Add a new BasicResearch to fill the empty spot for the moment 
                research.Add(new BasicResearch(this.ID));

                //Load the research save into the BasicResearch
                research[i].LoadResearch(load.researchSaves[i]);
            }

            for (int i = 0; i < load.vaccineSaves.Count; i++)
            {
                if (vaccines[i] == null)
                    vaccines.Add(new Vaccine(new Diseases(0, 0, new Disease())));

                vaccines[i].LoadVaccine(load.vaccineSaves[i]);
            }

            for (int i = 0; i < load.vehicleSaves.Count; i++)
            {
                if (vehicles[i] == null)
                    vehicles.Add(new BasicVehicle());

                vehicles[i].LoadVehicle(load.vehicleSaves[i]);
            }

            ID = load.ID;
            position = load.position;
            name = load.name;

            //Loads the grid
            grid = new Grid(load.gridSave.width, load.gridSave.height, this);
            grid.LoadGrid(load.gridSave);
        }

        #endregion

        public void SaveColony()
        {
            save.position = position;
            save.name = name;
            save.ID = ID;

            underlyingBiome.SaveBiome();
            save.underLyingBiomeSave = underlyingBiome.save;

            //Saves the grid
            grid.SaveGrid();
            save.gridSave = grid.save;

            save.canAssembleSaves = new List<ItemSave>();
            save.canResearchSaves = new List<ResearchSave>();
            save.diseaseSaves = new List<DiseaseSave>();
            save.inhabitantSaves = new List<ColonistSave>();
            save.itemSaves = new List<ItemSave>();
            save.researchDictionarySaves = new SerializableDictionary<string, ResearchSave>();
            save.researchSaves = new List<ResearchSave>();
            save.vaccineSaves = new List<VaccineSave>();
            save.vehicleSaves = new List<VehicleSave>();

            //Vaccine
            for (int i = 0; i < vaccines.Count; i++)
            {
                vaccines[i].SaveVaccine();
                save.vaccineSaves.Add(vaccines[i].save);
            }

            //Disease
            for (int i = 0; i < diseases.Count; i++)
            {
                diseases[i].SaveDisease();
                save.diseaseSaves.Add(diseases[i].save);
            }

            //Vehicle
            for (int i = 0; i < vehicles.Count; i++)
            {
                vehicles[i].SaveVehicle();
                save.vehicleSaves.Add(vehicles[i].save);
            }


            //Item
            for (int i = 0; i < items.Count; i++)
            {
                items[i].SaveItem();
                save.itemSaves[i] = items[i].save;
            }

            //Constructable Items
            for (int i = 0; i < canAssemble.Count; i++)
            {

                canAssemble[i].SaveItem();
                save.canAssembleSaves.Add(canAssemble[i].save);
            }

            //Inhabitants(Colonists)
            for (int i = 0; i < inhabitants.Count; i++)
            {
                inhabitants[i].SaveColonist();
                save.inhabitantSaves.Add(inhabitants[i].save);
            }


            //Finished Research Projects
            for (int i = 0; i < research.Count; i++)
            {
                research[i].SaveResearch();
                save.researchSaves.Add(research[i].save);
            }

            //Can Research
            for (int i = 0; i < canResearch.Count; i++)
            {
                canResearch[i].SaveResearch();
                save.researchSaves.Add(canResearch[i].save);
            }

            List<string> keys = new List<string>();

            foreach (var project in researchDictionary)
            {
                keys.Add(project.Key);
            }

            //Finished Research Projects Dic.
            for (int i = 0; i < researchDictionary.Count; i++)
            {
                researchDictionary[keys[i]].SaveResearch();
                save.researchDictionarySaves.Add(keys[i], researchDictionary[keys[i]].save);
            }

        }

        #endregion

        /// <summary>
        /// Builds/implements a colony
        /// </summary>
        /// <param name="planet">The planet which surface the colony is built on</param>
        public Colony(Planet planet, Vector2 position, int ID, Biome underlyingBiome)
        {
            hasUnderlyingBiome = false;
            this.position = position;

            //Colonists
            inhabitants = new List<Colonist>();
            removeInhabitant = new List<Colonist>();

            //Research
            research = new List<Research>();
            researchDictionary = new SerializableDictionary<string, Research>();
            canResearch = new List<Research>();

            //Diseases
            diseases = new List<Disease>();
            removeDisease = new List<Disease>();
            vaccines = new List<Vaccine>();
            diseaseRisk = 1;
            diseaseMaxRandom = 200000;

            this.ID = ID;

            //Adds 10 colonists to the colony
            for (int i = 0; i < 10; i++)
            {
                //Adds a colonist
                inhabitants.Add(new Colonist(this, ID)); //TODO: Add this in construct
            }

            //Vehicles
            vehicles = new List<Vehicle>();
            canAssemble = new List<Item>();
            items = new List<Item>();
            canAssemble.Add(new BasicVehicle());

            colonies = new List<Colony>();

            vehicles.Add(new VehicleG60());

            this.planet = planet;

            #region Research
            //Adds starting research
            #region Facilities
            
            research.Add(new FacilityResearch(ID, 0));
            research.Add(new ComArrayResearch(ID, 0));
            research.Add(new EquipmentStoreResearch(ID, 0));
            research.Add(new EvaluationFacilityResearch(ID, 0));
            research.Add(new FoodStoreResearch(ID, 0));
            research.Add(new GarageResearch(ID, 0));
            research.Add(new GreenhouseResearch(ID, 0));
            research.Add(new GymResearch(ID, 0));
            research.Add(new HangarResearch(ID, 0));
            research.Add(new LabResearch(ID, 0));
            research.Add(new LibraryResearch(ID, 0));
            research.Add(new LivingQuartersResearch(ID, 0));
            research.Add(new MedBayResearch(ID, 0));
            research.Add(new MineralStoreResearch(ID, 0));
            research.Add(new MineResearch(ID, 0));
            research.Add(new RefineryResearch(ID, 0));
            research.Add(new SolarPanelsResearch(ID, 0));
            research.Add(new WorkshopResearch(ID, 0));
            
            #endregion

            #region Buffs
            research.Add(new LargerFoodStorage(ID, 0));
            research.Add(new LargerMineralStorage(ID, 0));
            research.Add(new MoreEfficientResearch(ID, 0));          
            research.Add(new HigherEnergyOutput(ID, 0));
            research.Add(new LargerEquipmentStorage(ID, 0));
            research.Add(new LessConstructionCost(ID, 0));
            research.Add(new LessEnergyConsumption(ID, 0));
            research.Add(new LessFoodConsumption(ID, 0));
            research.Add(new LessMaintenanceCost(ID, 0));
            research.Add(new LessWaterConsumption(ID, 0));
            research.Add(new MoreEfficientExtraction(ID, 0));
            research.Add(new MoreEfficientIntelligenceTraining(ID, 0));
            research.Add(new MoreEfficientStrengthTraining(ID, 0));
            research.Add(new QuickerAssembling(ID, 0));
            research.Add(new ReducedAssemblingCost(ID, 0));
            #endregion

            foreach (var project in research)
            {
                canResearch.Add(project);
            }

            foreach (var item in research)
            {
                researchDictionary.Add(item.GetResearchType(), item);
            }
            #endregion

            name = "Colony";

            Game1.colonySaveTest = this;

            //Sets the size of the facility grid in the colony
            grid = new Grid(7, 5, this);

            this.underlyingBiome = underlyingBiome;
        }

        /// <summary>
        /// Gets the list containing inhabitants in the colony
        /// </summary>
        /// <returns>Returns the list containing inhabitants in the colony</returns>
        public List<Colonist> GetInhabitants()
        {
            return inhabitants;
        }

        /// <summary>
        /// Removes a colonist
        /// </summary>
        /// <param name="colonist">The colonist you wish to remove</param>
        public void RemoveColonist(Colonist colonist)
        {
            if (inhabitants.Contains(colonist))
            {
                inhabitants.Remove(colonist);
            }
            else if (!inhabitants.Contains(colonist))
            {
                throw new Exception("Tried to remove unexisting colonist");
            }
        }

        /// <summary>
        /// Adds a colonist
        /// </summary>
        /// <param name="colonist">The colonist you wish to add</param>
        public void AddColonist(Colonist colonist)
        {
            inhabitants.Add(colonist);
        }

        /// <summary>
        /// Updates the colony
        /// </summary>
        public void Update()
        {

            if (!hasUnderlyingBiome)
                SetUnderlyingBiome();

            if (TimeHandler.newTurn)
            {
                #region Checks if inhabitants die
                foreach (var inhabitant in inhabitants)
                {
                    //Kills and removes inhabitants if they're dead(if their healt is less or equals to 0)
                    if (inhabitant.health <= 0)
                    {
                        removeInhabitant.Add(inhabitant);
                    }
                }
                foreach (var inhabitant in removeInhabitant)
                {
                    inhabitants.Remove(inhabitant);
                }
                removeInhabitant.Clear();
                #endregion

                #region Diseases

                #region Disease (Generation)

                int i = Settings.RANDOM.Next(diseaseRisk, diseaseMaxRandom);
                if (i == diseaseRisk)
                    diseases.Add(new Disease());

                foreach (var disease in diseases)
                {
                    if (disease.TimeToRemove())
                        removeDisease.Add(disease);
                }
                foreach (var disease in removeDisease)
                {
                    diseases.Remove(disease);
                }
                removeDisease.Clear();

                #endregion

                foreach (var inhabitant in inhabitants)
                {
                    //Checks if the inhabitant gets infected by a disease
                    foreach (var disease in diseases)
                    {
                        inhabitant.Update(disease, canResearch);
                    }
                }
                #endregion

                #region Vaccines
                //Adds a vaccine for each finished disease research project
                foreach (var r in research)
                {
                    //Checks if the research project investigated a disease
                    if (r.GetResearchType() == "Disease")
                        //Checks if you don't have any vaccines
                        if (vaccines.Count != 0)
                        {
                            foreach (var vaccine in vaccines)
                            {
                                //Checks if the vaccine is already added
                                if (vaccine.GetDiseaseName() != r.GetResearchName() || vaccine.GetIdentification() != r.GetIdentification())
                                    //Adds a vaccine
                                    vaccines.Add(new Vaccine(r));
                            }
                        }
                        else
                        {
                            //Adds a vaccine
                            vaccines.Add(new Vaccine(r));
                        }
                }
                #endregion

                #region Vehicles

                foreach (var finishedProjects in research)
                {
                    if (finishedProjects.CheckIfVehicle())
                    {
                        //Adds one asseble to the list of possible assembles
                        canAssemble = canAssemble[0].AddItem(finishedProjects.GetResearchType(), canAssemble);
                    }
                }

                #endregion
            }
            //Updates the colony's grid
            grid.Update();
        }

        private void SetUnderlyingBiome()
        {

            int yRowAmount = 1;

            if (Settings.GetScreenRes.X < 1000)
                underLyingBiomePosition = new Vector2[1];
            else if (Settings.GetScreenRes.X < 2000)
                underLyingBiomePosition = new Vector2[2];
            else if (Settings.GetScreenRes.X < 3000)
                underLyingBiomePosition = new Vector2[3];

            if (Settings.GetScreenRes.Y > 1000)
                yRowAmount = 2;           
            if (Settings.GetScreenRes.Y > 2000)
                yRowAmount = 3;

            underLyingBiomePosition = new Vector2[underLyingBiomePosition.Length * yRowAmount];


            if(underlyingBiome.GetType() == typeof(PlanesBiome))
            {
                underlyingBiomeTexture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Planet View/Biomes/Planes/PlanesBase");
            }

            if(underlyingBiome.GetType() == typeof(JungleBiome))
            {
                underlyingBiomeTexture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Planet View/Biomes/Jungle/JungleBase");
            }

            if(underlyingBiome.GetType() == typeof(IceBiome))
            {
                underlyingBiomeTexture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Planet View/Biomes/Ice/IceBase");
            }

            if(underlyingBiome.GetType() == typeof(MountainBiome))
            {
                underlyingBiomeTexture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Planet View/Biomes/Mountains/MountainsBase");
            }

            for (int i = 0; i < underLyingBiomePosition.Length; i++)
            {
                int rowLength = underLyingBiomePosition.Length / yRowAmount;

                float x = underlyingBiomeTexture.Width * i;
                x %= rowLength * underlyingBiomeTexture.Width;
                float y = (int)i / yRowAmount;

                underLyingBiomePosition[i] = new Vector2(underlyingBiomeTexture.Width * i, 0);
            }



            hasUnderlyingBiome = true;

        }

        /// <summary>
        /// Checks if the colony can house more colonists
        /// </summary>
        /// <returns>Returns true if the colony can house more colonists, else false</returns>
        private bool CanHouseMore()
        {
            housing = grid.GetFacilityAmount("LivingQuarters") * livingQuarterHousing;

            if (inhabitants.Count < housing)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the amount of inhabitants the colony can house
        /// </summary>
        /// <returns>Returns the amount of inhabitants the colony can house</returns>
        public int GetHousingAmount()
        {
            return housing;
        }

        /// <summary>
        /// Gets the planet which surface the colony is built on
        /// </summary>
        /// <returns>Returns the planet which surface the colony is built on</returns>
        public Planet GetPlanet()
        {
            return planet;
        }

        /// <summary>
        /// Gets the colony's grid
        /// </summary>
        /// <returns>Returns the grid</returns>
        public Grid GetGrid()
        {
            return grid;
        }

        /// <summary>
        /// Gets the list containing colonies the com array has stable connection with
        /// </summary>
        /// <returns>Returns the list containing colonies the com array has stable connection with</returns>
        public List<Colony> GetColonies()
        {
            return colonies;
        }

        /// <summary>
        /// Gets the colony's name
        /// </summary>
        /// <returns>Returns the colony's name</returns>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Draws the colony's grid
        /// </summary>
        /// <param name="spriteBatch">Used to draw stuff on screen</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < underLyingBiomePosition.Length; i++)
            {
                spriteBatch.Draw(underlyingBiomeTexture, underLyingBiomePosition[i], Color.White);
            }
            grid.Draw(spriteBatch);
        }
    }

    class PlanetColonyButton : Button
    {

        public PlanetColonyButtonSave save;

        Colony colony;

        public void LoadPlanetColonyButton(PlanetColonyButtonSave load)
        {
            this.position = load.position;

            this.colony.LoadColony(load.colonySave);

            Core.AddColony(colony);
        }

        public void SavePlanetColonyButton()
        {
            save.position = position;

            colony.SaveColony();
            save.colonySave = colony.save;
        }

        public PlanetColonyButton(Vector2 position, Colony colony)
            : base(position)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Planet View/BaseMarkerPH");

            //Center the markers position so that its middle position is set to be the cursors for a more accurate feel
            this.SetPosition(new Vector2(position.X - texture.Width / 2, position.Y - texture.Height / 2));

            this.colony = colony;

            Core.AddColony(colony);
        }

        /// <summary>
        /// Checks if the planetColonyButton is pressed
        /// </summary>
        /// <returns>If pressed returns true</returns>
        public bool isPressed()
        {
            //Checks if the button collides with the cursor
            if (Collision())
                return true;

            return false;
        }

        /// <summary>
        /// Gets the colony
        /// </summary>
        /// <returns>Returns the colony</returns>
        public Colony GetColony()
        {
            //It only returns the colony if its value is not equal to null
            if (colony != null)
                return colony;

            throw new Exception("Attempted to get a NULL facility from Colony.cs");
        }
    }

    public struct PlanetColonyButtonSave
    {
        public Vector2 position;

        public ColonySave colonySave;
    }

    public struct ColonySave
    {
        //Saves the colony ID
        public int ID;

        //Saves the colony's name
        public string name;

        #region SHOULD YOU SAVE THE GRID?

        //Adds a grid to the colony
        public GridSave gridSave;

        #endregion

        //Saves the colony's inhabitants
        public List<ColonistSave> inhabitantSaves;

        //Saves what you have researched
        public List<ResearchSave> researchSaves;
        public SerializableDictionary<string, ResearchSave> researchDictionarySaves;

        //Saves what you can research
        public List<ResearchSave> canResearchSaves;

        //Saves the diseases that currenly exists in the colony
        public List<DiseaseSave> diseaseSaves;

        //Saves the vaccines the colony have developed
        public List<VaccineSave> vaccineSaves;

        //Saves the colony's location
        public Vector2 position;

        //Saves the constructed items
        public List<ItemSave> itemSaves;

        //Saves the items you can assemble
        public List<ItemSave> canAssembleSaves;

        //Saves the vehicle's the colony have produced
        public List<VehicleSave> vehicleSaves;

        public BiomeSave underLyingBiomeSave;
    }
}
