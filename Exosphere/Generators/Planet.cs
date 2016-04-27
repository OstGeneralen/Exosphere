using Exosphere.Src.Basebuilding;
using Exosphere.Src.Basebuilding.Facilities;
using Exosphere.Src.Exploring;
using Exosphere.Src.Handlers;
using Exosphere.Src.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Generators
{
    public class Planet
    {

        #region Message Boxes
        MessageBox canNotBuildOnGas;
        MessageBox canNotBuildInWater;
        #endregion

        public List<PossibleMiningPoint> possibleMiningPoints;

        ResourceArea[] resourceArea;

        public bool hasColonyShip;

        //A biome defines which biome should be dominant on the planet
        public Biome biome;

        //A string defining the wealth of the planet
        string wealth;

        //A biome generator to generate the biomes on the planet
        BiomesGenerator biomes;

        //The texture for the border of the planet (a black texture with a transparent circle in the middle)
        Texture2D planetBorder;
        //The position of the planet border
        Vector2 planetBorderPosition;

        //Space to be added around the planet
        Space space;

        //A bool telling if the planet is discovered or not
        public bool isDiscovered;

        //A rectangle with the planet's collision
        Rectangle collision;

        //A list of explorers currently on the planet
        public List<Explorer> explorers;
        //A temp list used to remove explorers
        public List<Explorer> explorersRemoveList;

        #region Colony Variables

        //A bool telling if the planet has a colony or not
        public bool hasColony;
        //A planet-colony
        PlanetColonyButton colony;

        #endregion
        //The size of the planet on a scale 1-3 (Small, Medium, Big)
        int size;

        //The position of the planet in the galaxy
        Vector2 position;

        Vector2 tempPositionOfColony;
        Biome tempBiomeHoldOfColony;

        public PlanetSave save;

        public void LoadPlanet(PlanetSave load)
        {
            this.isDiscovered = load.isDiscovered;
            this.position = load.position;
            this.size = load.size;
            this.wealth = load.wealth;

            this.biomes.LoadBiomes(load.biomes);

            resourceArea = new ResourceArea[load.resourceAreaSave.Length];

            for (int i = 0; i < load.resourceAreaSave.Length; i++)
            {
                resourceArea[i] = new ResourceArea(load.resourceAreaSave[i]);
            }

            this.planetBorderPosition = load.planetBorderPosition;

            this.planetBorder = Game1.INSTANCE.Content.Load<Texture2D>(load.planetBorderAssetName);

            this.space.LoadSpace(load.spaceSave);

            this.hasColony = load.hasColony;

            if (colony == null && hasColony)
            {
                colony = new PlanetColonyButton(Vector2.Zero, new Colony(this, Vector2.Zero, 0, new PlanesBiome()));
                colony.GetColony().Update();
            }
            if (hasColony)
                this.colony.LoadPlanetColonyButton(load.colonyButtonSave);

            explorers.Clear();
            for (int i = 0; i < load.explorerSaves.Count; i++)
            {
                explorers.Add(new Explorer());

                explorers[i].LoadExplorer(load.explorerSaves[i], GetColony());
            }

            possibleMiningPoints.Clear();
            for (int i = 0; i < load.possibleMiningPointsSave.Count; i++)
            {
                possibleMiningPoints.Add(new PossibleMiningPoint(Vector2.Zero, "", "", "", 0, 0, 0));

                possibleMiningPoints[i].LoadPossibleMiningPoint(load.possibleMiningPointsSave[i]);
            }
        }

        public void SavePlanet()
        {
            save.isDiscovered = isDiscovered;

            save.position = position;

            save.size = size;

            save.wealth = wealth;

            save.resourceAreaSave = new ResourceAreaSave[resourceArea.Length];

            for (int i = 0; i < resourceArea.Length; i++)
            {
                resourceArea[i].SaveResourceArea();
                save.resourceAreaSave[i] = resourceArea[i].save;
            }

            biomes.SaveBiomes();
            save.biomes = biomes.save;

            save.planetBorderAssetName = planetBorder.Name;

            save.planetBorderPosition = planetBorderPosition;

            space.SaveSpace();

            save.spaceSave = space.save;

            biome.SaveBiome();

            save.baseBiome = biome.save;

            if (hasColony)
            {
                save.hasColony = hasColony;

                colony.SavePlanetColonyButton();
                save.colonyButtonSave = colony.save;
            }

            save.explorerSaves = new List<ExplorerSave>();
            foreach (var explorer in explorers)
            {
                explorer.SaveExplorer();
                save.explorerSaves.Add(explorer.save);
            }

            save.possibleMiningPointsSave = new List<PossibleMiningPointSave>();
            foreach (var possibleMiningPoint in possibleMiningPoints)
            {
                possibleMiningPoint.SavePossibleMiningPoint();

                save.possibleMiningPointsSave.Add(possibleMiningPoint.save);
            }
        }

        /// <summary>
        /// Creates a new planet
        /// </summary>
        /// <param name="biome">The base-biome of the planet</param>
        /// <param name="size">The size of the planet</param>
        /// <param name="galaxyPosition">The position of the planet in the galaxy view</param>
        /// <param name="wealth">The wealth of the planet defined as Depleted, Poor, Normal, Rich or Plentiful</param>
        public Planet(Biome biome, int size, Vector2 position, string wealth)
        {
            hasColonyShip = false;

            #region Message Boxes
            canNotBuildOnGas = new MessageBox(1, "Gas Giants are uninhabitable. Can not build a colony here.");
            canNotBuildInWater = new MessageBox(1, "Colonies can not be built in water.");
            #endregion

            //Load the lists
            explorers = new List<Explorer>();
            explorersRemoveList = new List<Explorer>();

            //Set the local wealth variable to contain the value of the inputed wealth
            this.wealth = wealth;

            isDiscovered = false;
            //Sets the class varibales to the value of the local variables
            this.biome = biome;
            this.size = size;
            this.position = position;

            //Load the correct assets and position the textures depending on the planet size
            switch (size)
            {
                case 1:
                    planetBorder = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Planet View/SmallPlanetBorderPH");
                    planetBorder.Name = "Res/PH/Planet View/SmallPlanetBorderPH";
                    planetBorderPosition = new Vector2((Settings.GetScreenRes.X / 2) - (planetBorder.Width / 2), (Settings.GetScreenRes.Y / 2) - (planetBorder.Height / 2));
                    break;

                case 2:
                    planetBorder = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Planet View/MediumPlanetBorderPH");
                    planetBorder.Name = "Res/PH/Planet View/MediumPlanetBorderPH";
                    planetBorderPosition = new Vector2((Settings.GetScreenRes.X / 2) - (planetBorder.Width / 2), (Settings.GetScreenRes.Y / 2) - (planetBorder.Height / 2));
                    break;

                case 3:
                    planetBorder = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Planet View/BigPlanetBorderPH");
                    planetBorder.Name = "Res/PH/Planet View/BigPlanetBorderPH";
                    planetBorderPosition = new Vector2((Settings.GetScreenRes.X / 2) - (planetBorder.Width / 2), (Settings.GetScreenRes.Y / 2) - (planetBorder.Height / 2));
                    break;

                default:
                    throw new Exception("Planet size not recognized");

            }

            //Create space around the planet
            space = new Space(planetBorder, planetBorderPosition);

            //Set the collision for the planet
            collision = new Rectangle((int)planetBorderPosition.X, (int)planetBorderPosition.Y, planetBorder.Width, planetBorder.Height);

            //Generate the biomes for the planet
            biomes = new BiomesGenerator(size, planetBorder, planetBorderPosition, biome, wealth);

            resourceArea = new ResourceArea[Settings.RANDOM.Next(5, 16)];

            for (int i = 0; i < resourceArea.Length; i++)
            {
                resourceArea[i] = new ResourceArea((int)planetBorderPosition.X, (int)planetBorderPosition.Y, (int)(planetBorderPosition.X + collision.Width), (int)(planetBorderPosition.Y + collision.Height), resourceArea.Length);
            }

            possibleMiningPoints = new List<PossibleMiningPoint>();
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public BiomesGenerator GetBiomes()
        {
            return biomes;
        }

        public ResourceArea[] GetResourceArea()
        {
            return resourceArea;
        }

        public string GetWealth()
        {
            return wealth;
        }

        /// <summary>
        /// Adds a colony to the planet
        /// </summary>
        /// <param name="colony">The colony added</param>
        public void AddColony()
        {
            GetResources(Resource.ResourceType.carbon);
            GetResources(Resource.ResourceType.copper);
            GetResources(Resource.ResourceType.clearWater);
            GetResources(Resource.ResourceType.iron);

            this.colony = new PlanetColonyButton(tempPositionOfColony, new Colony(this, tempPositionOfColony, Core.GetColonies().Count + 1, tempBiomeHoldOfColony));
            this.colony.GetColony().Update();
            hasColony = true;
        }

        /// <summary>
        /// Returns this planet's colony
        /// </summary>
        /// <returns>The planet's colony as a Colony</returns>
        public Colony GetColony()
        {
            return colony.GetColony();
        }

        /// <summary>
        /// Returns the size of the planet
        /// </summary>
        /// <returns>Returns an int of the size</returns>
        public int GetSize()
        {
            return size;
        }

        /// <summary>
        /// Get the total amount of a specific resource on the planet
        /// </summary>
        /// <param name="rt">The resource type you want to get the amount of</param>
        /// <returns>Int - Total amount</returns>
        public int GetResources(Resource.ResourceType rt)
        {
            int total = biomes.CalculateTotalResource(rt);

            return total;
        }

        /// <summary>
        /// Updates the planet
        /// </summary>
        public void Update()
        {

            if(Core.choiceBoxChoice && tempPositionOfColony != Vector2.Zero && tempBiomeHoldOfColony != null)
            {
                AddColony();
                tempPositionOfColony = Vector2.Zero;
                tempBiomeHoldOfColony = null;

                Core.choiceBoxChoice = false;
            }

            if (colony != null)
            {
                if (colony.Collision())
                {
                    Core.SetColonyView(colony.GetColony());
                }

                if (TimeHandler.newTurn)
                {
                    foreach (var deadExplorer in explorersRemoveList)
                    {
                        explorers.Remove(deadExplorer);
                    }

                    explorersRemoveList.Clear();
                }

                foreach (var explorer in explorers)
                {
                    explorer.Update();
                }
                
                colony.Update();
            }

            if (MouseHandler.LMBOnce() && collision.Intersects(Cursor.collision) && biome.GetBiomeType() == "Gas")
            {
                Core.currentMessageBox = canNotBuildOnGas;
            }

            //Check if the LMB is pressed inside the planets collision area and the planet is not a gas giant
            if (MouseHandler.LMBOnce() && !hasColony && collision.Intersects(Cursor.collision) && biome.GetBiomeType() != "Gas")
            {
                //The source rectangle to get the color data from the planet border to prevent players from building bases in 'space'
                Rectangle source = new Rectangle((int)(Cursor.collision.X - planetBorderPosition.X), (int)(Cursor.collision.Y - planetBorderPosition.Y), 1, 1);

                //A new color array to hold the color data of the pressed area
                Color[] colorArray = new Color[source.Width * source.Height];

                //Saves the color data of the pressed area into the array
                planetBorder.GetData<Color>(0, source, colorArray, 0, colorArray.Length);

                //If the color array's color is not space-black and the biome pressed is not water: Add a new colony
                if (colorArray[0] == new Color(0, 0, 0, 0) && biomes.BiomeColiding(Cursor.collision).GetBiomeType() != "Water")
                {
                    if (hasColonyShip || Core.GetColonies().Count == 0)
                    {
                        tempPositionOfColony = Cursor.GetCollisionArea();
                        tempBiomeHoldOfColony = biomes.BiomeColiding(Cursor.collision);
                        Core.currentMessageBox = new ChoiceBox(2, "Are you sure you want to build a colony here?");
                    }
                    else
                    {
                        MessageBox mb = new MessageBox(1, "Can not build a colony without sending a Colony Ship.");
                        Core.currentMessageBox = mb;
                    }
                }

                if (colorArray[0] != new Color(0, 0, 0) && biomes.BiomeColiding(Cursor.collision).GetBiomeType() == "Water")
                    Core.currentMessageBox = canNotBuildInWater;
            }

            foreach (var possibleMP in possibleMiningPoints)
            {
                possibleMP.Update();
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            biomes.Draw(spriteBatch);
            space.Draw(spriteBatch);
            spriteBatch.Draw(planetBorder, planetBorderPosition, Color.White);

            if (colony != null)
            {
                colony.Draw(spriteBatch);

                foreach (var explorer in explorers)
                {
                    explorer.Draw(spriteBatch);
                }

                foreach (var possibleMP in possibleMiningPoints)
                {
                    possibleMP.Draw(spriteBatch);
                }
            }

        }

    }

    public struct PlanetSave
    {
        public string wealth;

        public Vector2 position;

        public int size;

        public bool isDiscovered;

        public ResourceAreaSave[] resourceAreaSave;

        public BiomesSave biomes;

        public BiomeSave baseBiome;

        public string planetBorderAssetName;

        public Vector2 planetBorderPosition;

        public SpaceSave spaceSave;

        public bool hasColony;

        public PlanetColonyButtonSave colonyButtonSave;

        public List<ExplorerSave> explorerSaves;

        public List<PossibleMiningPointSave> possibleMiningPointsSave;

    }

    class Space
    {
        List<Vector2> positions;
        Texture2D texture;

        public SpaceSave save;

        public void SaveSpace()
        {
            save.positions = positions;
        }

        public void LoadSpace(SpaceSave load)
        {
            this.positions = load.positions;
        }

        public Space(Texture2D planetBorder, Vector2 planetBorderPosition)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/SpacePH");

            positions = new List<Vector2>();

            //Add left side space
            positions.Add(new Vector2(planetBorderPosition.X - texture.Width, planetBorderPosition.Y));

            //Add right side space
            positions.Add(new Vector2(planetBorderPosition.X + planetBorder.Width, planetBorderPosition.Y));

            //Add Lower row space
            positions.Add(new Vector2(planetBorderPosition.X - texture.Width, planetBorderPosition.Y + planetBorder.Height));
            positions.Add(new Vector2(planetBorderPosition.X, planetBorderPosition.Y + planetBorder.Height));
            positions.Add(new Vector2(planetBorderPosition.X + planetBorder.Width, planetBorderPosition.Y + planetBorder.Height));

            //Add upper row space
            positions.Add(new Vector2(planetBorderPosition.X - texture.Width, planetBorderPosition.Y - texture.Height));
            positions.Add(new Vector2(planetBorderPosition.X, planetBorderPosition.Y - texture.Height));
            positions.Add(new Vector2(planetBorderPosition.X + planetBorder.Width, planetBorderPosition.Y - texture.Height));


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var position in positions)
            {
                spriteBatch.Draw(texture, position, Color.White);
            }
        }

    }

    public struct SpaceSave
    {
        public List<Vector2> positions;
    }

    public class BiomesGenerator
    {
        //An array containing the different biomes
        Biome[] biomes;

        Biome baseBiome;

        public BiomesSave save;


        public void LoadBiomes(BiomesSave load)
        {
            this.baseBiome.LoadBiome(load.baseBiomeSave);

            if (load.baseBiomeSave.biomeType == "Gas")
                this.baseBiome = new GasBiome();

            if (load.baseBiomeSave.biomeType == "Planes")
                this.baseBiome = new PlanesBiome();

            if (load.baseBiomeSave.biomeType == "Water")
                this.baseBiome = new WaterBiome();

            if (load.baseBiomeSave.biomeType == "Jungle")
                this.baseBiome = new JungleBiome();

            if (load.baseBiomeSave.biomeType == "Ice")
                this.baseBiome = new IceBiome();

            if (load.baseBiomeSave.biomeType == "Mountains")
                this.baseBiome = new MountainBiome();

            baseBiome.LoadBiome(load.baseBiomeSave);


            if (load.baseBiomeSave.biomeType != "Gas")
            {

                biomes = new Biome[load.biomeSave.Length];

                for (int i = 0; i < load.biomeSave.Length; i++)
                {
                    if (load.biomeSave[i].biomeType == "Planes")
                        biomes[i] = new PlanesBiome();

                    if (load.biomeSave[i].biomeType == "Water")
                        biomes[i] = new WaterBiome();

                    if (load.biomeSave[i].biomeType == "Jungle")
                        biomes[i] = new JungleBiome();

                    if (load.biomeSave[i].biomeType == "Ice")
                        biomes[i] = new IceBiome();

                    if (load.biomeSave[i].biomeType == "Mountains")
                        biomes[i] = new MountainBiome();


                    biomes[i].LoadBiome(load.biomeSave[i]);
                }
            }

        }

        public void SaveBiomes()
        {

            baseBiome.SaveBiome();
            save.baseBiomeSave = baseBiome.save;


            if (baseBiome.GetBiomeType() != "Gas")
            {

                save.biomeSave = new BiomeSave[biomes.Length];

                for (int i = 0; i < biomes.Length; i++)
                {
                    biomes[i].SaveBiome();
                    save.biomeSave[i] = biomes[i].save;
                }
            }


        }

        /// <summary>
        /// Creates a new planet surface
        /// </summary>
        /// <param name="size">The size of the planet</param>
        public BiomesGenerator(int size, Texture2D planetBorder, Vector2 planetBorderPosition, Biome baseBiome, string planetWealth)
        {
            //An int containting the amount of different biomes that should exist on the planet

            this.baseBiome = baseBiome;

            if (baseBiome.GetBiomeType() != "Gas")
            {
                GenerateBiomes(size, planetBorder, planetBorderPosition, planetWealth);
            }

        }

        public Biome GetBaseBiome()
        {
            return baseBiome;
        }

        public Biome[] GetBiomes()
        {
            return biomes;
        }

        private void GenerateBiomes(int size, Texture2D planetBorder, Vector2 planetBorderPosition, string planetWealth)
        {

            float layer = 0;

            if (size == 1)
            {
                biomes = new Biome[10];
            }

            if (size == 2)
            {
                biomes = new Biome[20];
            }

            if (size == 3)
            {
                biomes = new Biome[30];
            }

            for (int i = 0; i < biomes.Length; i++)
            {

                biomes[i] = BiomeGenerator(planetBorder, planetBorderPosition, layer, planetWealth);
                layer++;

            }



        }

        private Biome BiomeGenerator(Texture2D planetBorder, Vector2 planetBorderPosition, float layer, string planetWealth)
        {
            Biome biome = new PlanesBiome(planetBorder, planetBorderPosition, layer, planetWealth);

            int r = Settings.RANDOM.Next(1, 7);

            if (baseBiome.GetBiomeType() == "Dessert" && r == 6)
                r = -1;

            if (baseBiome.GetBiomeType() == "Ice" && r == 5 || baseBiome.GetBiomeType() == "Ice" && r == 4 || baseBiome.GetBiomeType() == "Ice" && r == 3)
                r = -1;

            switch (r)
            {
                case 1:
                    biome = new WaterBiome(planetBorder, planetBorderPosition, layer, planetWealth);
                    break;
                case 2:
                    biome = new MountainBiome(planetBorder, planetBorderPosition, layer, planetWealth);
                    break;
                case 3:
                    biome = new JungleBiome(planetBorder, planetBorderPosition, layer, planetWealth);
                    break;
                case 4:
                    biome = new PlanesBiome(planetBorder, planetBorderPosition, layer, planetWealth);
                    break;
                /*case 5:
                    biome = new DessertBiome(planetBorder, planetBorderPosition);
                    break;
                 */
                case 6:
                    biome = new IceBiome(planetBorder, planetBorderPosition, layer, planetWealth);
                    break;
                default:
                    if (baseBiome.GetBiomeType() == "Water")
                        biome = new WaterBiome(planetBorder, planetBorderPosition, layer, planetWealth);
                    if (baseBiome.GetBiomeType() == "Mountain")
                        biome = new MountainBiome(planetBorder, planetBorderPosition, layer, planetWealth);
                    if (baseBiome.GetBiomeType() == "Jungle")
                        biome = new JungleBiome(planetBorder, planetBorderPosition, layer, planetWealth);
                    if (baseBiome.GetBiomeType() == "Planes")
                        biome = new PlanesBiome(planetBorder, planetBorderPosition, layer, planetWealth);
                    /* if (baseBiome.GetBiomeType() == "Dessert")
                         biome = new DessertBiome(planetBorder, planetBorderPosition);*/
                    if (baseBiome.GetBiomeType() == "Ice")
                        biome = new IceBiome(planetBorder, planetBorderPosition, layer, planetWealth);
                    break;
            }

            return biome;
        }

        /// <summary>
        /// Checks which biome that is colliding with the inputed Rectangle
        /// </summary>
        /// <param name="collisionRectangle">The rectangle to check against</param>
        /// <returns>The biome colliding with the rectangle</returns>
        public Biome BiomeColiding(Rectangle collisionRectangle)
        {
            //A temporary list of the biomes on the planet that is within the collision area
            List<Biome> tempBiomes = new List<Biome>();

            //A biome to return
            Biome pressedBiome = null;

            //A float holding the currently highest layer for the biomes
            float highestLayer = 0;

            //Loop through each biome on the planet
            for (int i = 0; i < biomes.Length; i++)
            {

                //If a biome's colored area is pressed add it to tempBiomes list
                if (biomes[i].Collision(collisionRectangle))
                    tempBiomes.Add(biomes[i]);

            }

            //Loop through the tempBiomes list
            foreach (var biome in tempBiomes)
            {
                //If the biome currently in loops has a higher layer value than the currently highest it means that this is over the highest one
                if (biome.layer >= highestLayer)
                {
                    //Set the currently all time high to equal this biome's layer value
                    highestLayer = biome.layer;
                }
            }

            //Loop through the tempBiomes again
            foreach (var biome in tempBiomes)
            {
                //If the biome's layer is equal to the highes one encountered in the previous loop it means this is the topmost one
                if (biome.layer == highestLayer)
                    //Set the pressed biome to be the current biome in loop
                    pressedBiome = biome;
            }

            //If pressedBiome is not null (no biome was pressed) return it
            if (pressedBiome != null)
                return pressedBiome;

            //If the pressedBiome was null, return the baseBiome
            return baseBiome;
        }

        /// <summary>
        /// Calculates the total amount of resources 
        /// </summary>
        /// <param name="rt">The resource to count</param>
        /// <returns>Returns an int of the total amount of the specified resorce</returns>
        public int CalculateTotalResource(Resource.ResourceType rt)
        {
            int total = 0;

            for (int i = 0; i < biomes.Length; i++)
            {
                total = total + biomes[i].GetResourceAmount(rt);
            }

            return total;
        }

        /// <summary>
        /// Draws the biomes
        /// </summary>
        /// <param name="SpriteBatch">The spriteBatch used to draw the biomes</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            baseBiome.DrawBase(spriteBatch);

            if (baseBiome.GetBiomeType() != "Gas")
            {
                for (int i = 0; i < biomes.Length; i++)
                {
                    biomes[i].Draw(spriteBatch);
                }


            }

        }
    }

    public struct BiomesSave
    {
        public BiomeSave[] biomeSave;

        public BiomeSave baseBiomeSave;
    }

    public class GalaxyPlanetButton : Button
    {
        public Planet planet;
        public Texture2D planetBorder;
        public Vector2 planetBorderPosition;
        public Color undiscovered;
        public Color discovered;

        public GalaxyPlanetButtonSave save;

        public void LoadGalaxyPlanetButton(GalaxyPlanetButtonSave load)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>(load.assetName);

            discovered = load.discovered;

            undiscovered = load.undiscovered;

            planet.LoadPlanet(load.planetSave);

            position = load.position;
        }

        public void SaveGalaxyPlanetButton()
        {

            save.assetName = texture.Name;

            save.discovered = discovered;

            planet.SavePlanet();

            save.planetSave = planet.save;

            save.position = position;

            save.undiscovered = undiscovered;

        }

        public GalaxyPlanetButton(int size, Vector2 position)
            : base(position)
        {

            //Defines Size == 1 as a Small planet's texture
            if (size == 1)
            {
                string assetName = "Res/PH/Galaxy View/smallPlanetPH";
                texture = Game1.INSTANCE.Content.Load<Texture2D>(assetName);
                texture.Name = assetName;
                planetBorder = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Planet View/SmallPlanetBorderPH");
                planetBorderPosition = new Vector2((Settings.GetScreenRes.X / 2) - (planetBorder.Width / 2), (Settings.GetScreenRes.Y / 2) - (planetBorder.Height / 2));
            }

            //Defines Size == 2 as a Medium planet's texture
            if (size == 2)
            {
                string assetName = "Res/PH/Galaxy View/mediumPlanetPH";
                texture = Game1.INSTANCE.Content.Load<Texture2D>(assetName);
                texture.Name = assetName;
                planetBorder = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Planet View/MediumPlanetBorderPH");
                planetBorderPosition = new Vector2((Settings.GetScreenRes.X / 2) - (planetBorder.Width / 2), (Settings.GetScreenRes.Y / 2) - (planetBorder.Height / 2));
            }

            //Defines Size == 1 as a Large planet's texture
            if (size == 3)
            {
                string assetName = "Res/PH/Galaxy View/bigPlanetPH";
                texture = Game1.INSTANCE.Content.Load<Texture2D>(assetName);
                texture.Name = assetName;
                planetBorder = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Planet View/BigPlanetBorderPH");
                planetBorderPosition = new Vector2((Settings.GetScreenRes.X / 2) - (planetBorder.Width / 2), (Settings.GetScreenRes.Y / 2) - (planetBorder.Height / 2));
            }

            SetBiome(size);

        }

        public Rectangle GetCollision()
        {
            return collision;
        }

        private void SetBiome(int size)
        {

            int r = Settings.RANDOM.Next(1, 201);

            int wealthRandom = Settings.RANDOM.Next(1, 101);
            string wealth = "Normal";

            if (wealthRandom <= 5)
                wealth = "Depleted";
            if (wealthRandom > 5 && wealthRandom <= 20)
                wealth = "Poor";
            if (wealthRandom > 20 && wealthRandom <= 95)
                wealth = "Normal";
            if (wealthRandom > 95 && wealthRandom <= 99)
                wealth = "Rich";
            if (wealthRandom > 99 && wealthRandom <= 100)
                wealth = "Plentiful";


            if (r <= 2)
            {
                planet = new Planet(new WaterBiome(planetBorder, planetBorderPosition, -1, wealth), size, position, wealth);
                undiscovered = new Color(5, 130, 215);
                discovered = new Color(0, 100, 170);
            }


            if (r > 2 && r <= 8)
            {
                planet = new Planet(new MountainBiome(planetBorder, planetBorderPosition, -1, wealth), size, position, wealth);
                undiscovered = new Color(175, 175, 175);
                discovered = new Color(65, 65, 65);
            }

            if (r > 8 && r <= 13)
            {
                planet = new Planet(new JungleBiome(planetBorder, planetBorderPosition, -1, wealth), size, position, wealth);
                undiscovered = new Color(55, 105, 0);
                discovered = new Color(35, 65, 0);
            }

            if (r > 13 && r <= 28)
            {
                planet = new Planet(new PlanesBiome(planetBorder, planetBorderPosition, -1, wealth), size, position, wealth);
                undiscovered = new Color(24, 235, 0);
                discovered = new Color(16, 165, 0);
            }

            /*if(r > 18 && r <= 38)
            {
                planet = new Planet(new DessertBiome(planetBorder, planetBorderPosition), size);
            }
             */

            if (r > 28 && r <= 30)
            {
                planet = new Planet(new IceBiome(planetBorder, planetBorderPosition, -1, wealth), size, position, wealth);
                undiscovered = new Color(245, 255, 255);
                discovered = new Color(215, 255, 255);
            }


            if (r > 30 && r <= 200)
            {
                planet = new Planet(new GasBiome(planetBorder, planetBorderPosition, -1, wealth), size, position, wealth);
                undiscovered = Color.Brown * 0.5f;
                discovered = Color.Brown;
            }

        }

        public Planet GetPlanet()
        {
            return planet;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!planet.isDiscovered)
                spriteBatch.Draw(texture, position, undiscovered);

            if (planet.isDiscovered)
                spriteBatch.Draw(texture, position, discovered);
        }
    }

    public struct GalaxyPlanetButtonSave
    {
        public PlanetSave planetSave;
        public Vector2 position;
        public string assetName;
        public Color undiscovered;
        public Color discovered;
    }

    public class ResourceArea
    {
        public Vector2 position;
        public Rectangle collsion;

        public bool hasResources;

        int copper;
        int iron;
        int carbon;
        int clearWater;
        int amount;

        public ResourceAreaSave save;

        public ResourceArea(ResourceAreaSave load)
        {
            this.carbon = load.carbon;
            this.copper = load.copper;
            this.clearWater = load.clearWater;
            this.iron = load.iron;
            this.position = load.position;
            this.collsion = load.collision;
            this.hasResources = load.hasResources;
            this.amount = load.amount;
        }

        public void SaveResourceArea()
        {
            save.carbon = carbon;
            save.clearWater = clearWater;
            save.iron = iron;
            save.copper = copper;
            save.collision = collsion;
            save.position = position;
            save.hasResources = hasResources;
            save.amount = amount;
        }

        public ResourceArea(int minimumX, int minimumY, int maximumX, int maximumY, int amount)
        {
            int x = Settings.RANDOM.Next(minimumX, maximumX);
            int y = Settings.RANDOM.Next(minimumY, maximumY);
            position = new Vector2(x, y);
            collsion = new Rectangle(x, y, 100, 100);

            this.amount = amount;

            hasResources = false;

        }

        public Rectangle GetArea()
        {
            return collsion;
        }

        public void AddResources(Biome biome)
        {
            if (!hasResources)
            {
                copper = (int)(biome.GetResourceAmount(Resource.ResourceType.copper) / amount);
                iron = (int)(biome.GetResourceAmount(Resource.ResourceType.iron) / amount);
                carbon = (int)(biome.GetResourceAmount(Resource.ResourceType.carbon) / amount);
                clearWater = (int)(biome.GetResourceAmount(Resource.ResourceType.clearWater) / amount);

                hasResources = true;
            }
        }

        #region GetResources
        public int GetCopper()
        {
            return copper;
        }

        public int GetIron()
        {
            return iron;
        }

        public int GetCarbon()
        {
            return carbon;
        }

        public int GetClearWater()
        {
            return clearWater;
        }
        #endregion

        #region Subtract Resources
        public void SubtractCopper(int value)
        {
            copper -= value;
        }

        public void SubtractIron(int value)
        {
            iron -= value;
        }

        public void SubtractCarbon(int value)
        {
            carbon -= value;
        }

        public void SubtractClearWater(int value)
        {
            clearWater -= value;
        }
        #endregion
    }

    public struct ResourceAreaSave
    {
        public Vector2 position;

        public Rectangle collision;

        public int copper;

        public int iron;

        public int carbon;

        public int clearWater;

        public bool hasResources;

        public int amount;
    }

}
