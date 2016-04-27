using Exosphere.Src.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Generators
{
    public abstract class Biome
    {
        //The texture to represent the biome
        public Texture2D texture;

        //The position of the biome
        protected Vector2 position;

        //The layer of the biome
        public float layer;

        public string assetName;

        public Rectangle collision;

        protected Texture2D baseTexture;
        protected Vector2 baseTexturePosition;

        protected int huntingDifficulty;

        string planetWealth;

        #region Animal percentages
        //These shall be used in the way of humongous starting at 0+ the chance of them spawing
        //Huge shall then be humogous number + the chance of them spawning 
        //And so on
        protected int humongousAnimalPercentage;
        protected int hugeAnimalPercentage;
        protected int largeAnimalPercentage;
        protected int mediumAnimalPercentage;
        protected int smallAnimalPercentage;
        protected int microscopicAnimalPercentage;
        #endregion

        #region Resources
        //Spawnchance of resources in percentage
        protected float clearWaterMultiplier;
        protected float copperMultiplier;
        protected float ironMultiplier;
        protected float carbonMultiplier;

        //The total amount of resources for the biome
        protected float clearWaterAmount;
        protected float copperAmount;
        protected float ironAmount;
        protected float carbonAmount;

        #endregion

        protected int diseaseRisk;

        protected string biomeType;

        public BiomeSave save;

        public void LoadBiome(BiomeSave load)
        {
            this.biomeType = load.biomeType;

            this.carbonAmount = load.carbonAmount;

            this.clearWaterAmount = load.clearWaterAmount;

            this.ironAmount = load.ironAmount;

            this.copperAmount = load.copperAmount;

            this.layer = load.layer;

            this.position = load.position;

            this.assetName = load.assetName;

            if(biomeType != "Gas")
            this.texture = Game1.INSTANCE.Content.Load<Texture2D>(load.assetName);

            baseTexture = Game1.INSTANCE.Content.Load<Texture2D>(load.baseTextureAssetName);

            this.baseTexturePosition = load.baseTexturePosition;

            SetValues(true);

            if (biomeType != "Gas")
                this.texture = Game1.INSTANCE.Content.Load<Texture2D>(load.assetName);

            baseTexture = Game1.INSTANCE.Content.Load<Texture2D>(load.baseTextureAssetName);
        }

        public void SaveBiome()
        {
            save.biomeType = biomeType;

            save.carbonAmount = carbonAmount;

            save.clearWaterAmount = clearWaterAmount;

            save.copperAmount = copperAmount;

            save.ironAmount = ironAmount;

            save.layer = layer;

            save.position = position;

            save.assetName = assetName;

            save.baseTexturePosition = baseTexturePosition;

            save.baseTextureAssetName = baseTexture.Name;
        }

        #region Functions

        /// <summary>
        /// Creates a new biome with a randomized position
        /// </summary>
        public Biome(Texture2D planetBorder, Vector2 planetBorderPosition, float layer, string planetWealth)
        {

            position.X = Settings.RANDOM.Next((int)(planetBorderPosition.X - 100), (int)(planetBorderPosition.X + planetBorder.Width));
            position.Y = Settings.RANDOM.Next((int)(planetBorderPosition.Y - 100), (int)(planetBorderPosition.Y + planetBorder.Height));

            this.planetWealth = planetWealth;
            this.layer = layer;

            baseTexturePosition = planetBorderPosition;

        }

        public virtual void SetValues(bool loaded = false)
        {

        }

        public Biome()
        {
            
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public string GetBiomeType()
        {
            return biomeType;
        }

        public int GetDiseaseRisk()
        {
            return diseaseRisk;
        }

        public string AssetNameGenerator(string biomeType)
        {

            string assetName = "Res/PH/Planet View/Biomes/";

            assetName = assetName.Insert(assetName.Length, biomeType + "/" + biomeType);

            int r = Settings.RANDOM.Next(1, 6);
            switch (r)
            {
                case 1:
                    assetName = assetName.Insert(assetName.Length, "BiomeOne");
                    break;
                case 2:
                    assetName = assetName.Insert(assetName.Length, "BiomeTwo");
                    break;
                case 3:
                    assetName = assetName.Insert(assetName.Length, "BiomeThree");
                    break;
                case 4:
                    assetName = assetName.Insert(assetName.Length, "BiomeFour");
                    break;
                case 5:
                    assetName = assetName.Insert(assetName.Length, "BiomeFive");
                    break;
                default:
                    throw new Exception("Asset name not valid");
            }

            return assetName;



        }

        public string HuntResult(Colonist hunter)
        {

            int r = Settings.RANDOM.Next(1, 101);
            int findAnimal = Settings.RANDOM.Next(5, huntingDifficulty);

            if (hunter.intelligence >= findAnimal)
            {
                if (r <= humongousAnimalPercentage)
                {
                    return "humongous";
                }

                if (r > humongousAnimalPercentage && r <= hugeAnimalPercentage)
                {
                    return "huge";
                }

                if (r > hugeAnimalPercentage && r <= largeAnimalPercentage)
                {
                    return "large";
                }

                if (r > largeAnimalPercentage && r <= mediumAnimalPercentage)
                {
                    return "medium";
                }

                if (r > mediumAnimalPercentage && r <= smallAnimalPercentage)
                {
                    return "small";
                }

                if (r > smallAnimalPercentage && r <= microscopicAnimalPercentage)
                {
                    return "microscopic";
                }
            }
            return null;
        }

        public bool Collision(Rectangle collisionRectangle)
        {
            Color[] colorHold = new Color[collisionRectangle.Width * collisionRectangle.Height];


            if (collision != null)
            {
                if (collision.Intersects(collisionRectangle))
                {

                    Rectangle source = new Rectangle();

                    source = new Rectangle((int)(collisionRectangle.X - position.X),
                                           (int)(collisionRectangle.Y - position.Y),
                                           collisionRectangle.Width,
                                           collisionRectangle.Height);


                    texture.GetData<Color>(0, source, colorHold, 0, colorHold.Length);

                    for (int i = 0; i < colorHold.Length; i++)
                    {
                        if (colorHold[i] != Color.Transparent)
                            return true;
                    }

                }
            }

            return false;
        }

        /// <summary>
        /// Adds resources of the specified sort to the biome
        /// </summary>
        /// <param name="rt">The type of resource</param>
        public void AddResources()
        {
            float wealthMultiplier = 0;

            #region Wealth multipliers
            float depletedMultiplier = 0;
            float poorMultiplier = 0.1f;
            float normalMultiplier = 0.2f;
            float richMultiplier = 0.6f;
            float plentifulMultiplier = 2;
            #endregion

            #region Base values
            float baseValueCopper = 20000000; //20 000 000
            float baseValueIron = 20000000; //20 000 000
            float baseValueCarbon = 20000000; //20 000 000
            float baseValueClearWater = 10000000; //10 000 000
            #endregion

            if (planetWealth == "Depleted")
                wealthMultiplier = depletedMultiplier;
            if (planetWealth == "Poor")
                wealthMultiplier = poorMultiplier;
            if (planetWealth == "Normal")
                wealthMultiplier = normalMultiplier;
            if (planetWealth == "Rich")
                wealthMultiplier = richMultiplier;
            if (planetWealth == "Plentiful")
                wealthMultiplier = plentifulMultiplier;

            copperAmount = baseValueCopper * wealthMultiplier * copperMultiplier;
            clearWaterAmount = baseValueClearWater * wealthMultiplier * clearWaterMultiplier;
            ironAmount = baseValueIron * wealthMultiplier * ironMultiplier;
            carbonAmount = baseValueCarbon * wealthMultiplier * carbonMultiplier;
        }


        /// <summary>
        /// Returns the total amount of the specified resource on one biome
        /// </summary>
        /// <param name="rt">The resource type</param>
        /// <returns>The totl amount in an int</returns>
        public int GetResourceAmount(Resource.ResourceType rt)
        {
            if (rt == Resource.ResourceType.carbon)
                return (int)carbonAmount;
            if (rt == Resource.ResourceType.clearWater)
                return (int)clearWaterAmount;
            if (rt == Resource.ResourceType.copper)
                return (int)copperAmount;
            if (rt == Resource.ResourceType.iron)
                return (int)ironAmount;

            return 0;
        }

        /// <summary>
        /// Draws the biome
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch used for drawing the biome</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            
            /*
            for (int i = 0; i < resourceArea.Length; i++)
            {
                spriteBatch.Draw(Game1.BLANK_TEX, new Vector2(resourceArea[i].GetCollision().X, resourceArea[i].GetCollision().Y), resourceArea[i].GetCollision(), Color.Red * 0.5f);
            }
             */
        }

        public virtual void DrawBase(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(baseTexture, baseTexturePosition, Color.White);
        }

        #endregion
    }

    public class PlanesBiome : Biome
    {

        public PlanesBiome()
        {

        }

        public PlanesBiome(Texture2D planetBorder, Vector2 planetBorderPosition, float layer, string planetWealth)
            : base(planetBorder, planetBorderPosition, layer, planetWealth)
        {
            baseTexture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Planet View/Biomes/Planes/PlanesBase");
            baseTexture.Name = "Res/PH/Planet View/Biomes/Planes/PlanesBase";

            biomeType = "Planes";

            assetName = AssetNameGenerator(biomeType);
            texture = Game1.INSTANCE.Content.Load<Texture2D>(assetName);

            SetValues();
        }

        public override void SetValues(bool loaded = false)
        {
            huntingDifficulty = 10;
            humongousAnimalPercentage = 0;
            hugeAnimalPercentage = 0;
            largeAnimalPercentage = 0;
            mediumAnimalPercentage = 2;
            smallAnimalPercentage = 70;
            microscopicAnimalPercentage = 100;

            diseaseRisk = 55;

            carbonMultiplier = 0.02f;
            copperMultiplier = 0.08f;
            ironMultiplier = 0.10f;
            clearWaterMultiplier = 0.2f;

            collision = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            if(!loaded)
                AddResources();
        }


    }

    /*
    class DessertBiome : Biome
    {
        public DessertBiome(Texture2D planetBorder, Vector2 planetBorderPosition) : base( planetBorder, planetBorderPosition)
        {


            baseTexture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Biomes/DessertBase");



            biomeType = "Dessert";


            texture = Game1.INSTANCE.Content.Load<Texture2D>(AssetNameGenerator(biomeType));

            //Sets the percentages
            clearWaterPercentage = 1;
            copperPercentage = 0;
            ironPercentage = 0;
            carbonPercentage = 0;
            
        }
    }
     */

    public class WaterBiome : Biome
    {

        public WaterBiome()
        {

        }

        public WaterBiome(Texture2D planetBorder, Vector2 planetBorderPosition, float layer, string planetWealth)
            : base(planetBorder, planetBorderPosition, layer, planetWealth)
        {
            baseTexture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Planet View/Biomes/Water/WaterBase");
            baseTexture.Name = "Res/PH/Planet View/Biomes/Water/WaterBase";

            biomeType = "Water";


            assetName = AssetNameGenerator(biomeType);
            texture = Game1.INSTANCE.Content.Load<Texture2D>(assetName);

            SetValues();
        }

        public override void SetValues(bool loaded = false)
        {
            huntingDifficulty = 90;
            humongousAnimalPercentage = 20;
            hugeAnimalPercentage = 30;
            largeAnimalPercentage = 40;
            mediumAnimalPercentage = 60;
            smallAnimalPercentage = 80;
            microscopicAnimalPercentage = 100;

            diseaseRisk = 100;
           
            collision = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);


            //Sets the percentages
            clearWaterMultiplier = 0.2f;
            copperMultiplier = 0;
            ironMultiplier = 0;
            carbonMultiplier = 0;

            if(!loaded)
                AddResources();
        }
    }

    public class JungleBiome : Biome
    {

        public JungleBiome()
        {

        }

        public JungleBiome(Texture2D planetBorder, Vector2 planetBorderPosition, float layer, string planetWealth)
            : base(planetBorder, planetBorderPosition, layer, planetWealth)
        {

            baseTexture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Planet View/Biomes/Jungle/JungleBase");
            baseTexture.Name = "Res/PH/Planet View/Biomes/Jungle/JungleBase";


            biomeType = "Jungle";


            assetName = AssetNameGenerator(biomeType);
            texture = Game1.INSTANCE.Content.Load<Texture2D>(assetName);

            SetValues();

        }

        public override void SetValues(bool loaded = false)
        {
            huntingDifficulty = 30;
            humongousAnimalPercentage = 1;
            hugeAnimalPercentage = 5;
            largeAnimalPercentage = 10;
            mediumAnimalPercentage = 30;
            smallAnimalPercentage = 70;
            microscopicAnimalPercentage = 100;

            diseaseRisk = 80;

            collision = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            //Sets the percentages
            clearWaterMultiplier = 0.5f;
            copperMultiplier = 0.06f;
            ironMultiplier = 0.08f;
            carbonMultiplier = 0.03f;

            if(!loaded)
                AddResources();
        }
    }

    public class IceBiome : Biome
    {

        public IceBiome()
        {

        }

        public IceBiome(Texture2D planetBorder, Vector2 planetBorderPosition, float layer, string planetWealth)
            : base(planetBorder, planetBorderPosition, layer, planetWealth)
        {
            baseTexture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Planet View/Biomes/Ice/IceBase");
            baseTexture.Name = "Res/PH/Planet View/Biomes/Ice/IceBase";

            biomeType = "Ice";

            assetName = AssetNameGenerator(biomeType);
            texture = Game1.INSTANCE.Content.Load<Texture2D>(assetName);

            SetValues();
        }

        public override void SetValues(bool loaded = false)
        {
            huntingDifficulty = 60;
            humongousAnimalPercentage = 20;
            hugeAnimalPercentage = 40;
            largeAnimalPercentage = 60;
            mediumAnimalPercentage = 70;
            smallAnimalPercentage = 90;
            microscopicAnimalPercentage = 100;

            clearWaterMultiplier = 0.5f;
            copperMultiplier = 0;
            ironMultiplier = 0;
            carbonMultiplier = 0;

            diseaseRisk = 5;

            collision = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            if(!loaded)
                AddResources();
        }
    }

    public class GasBiome : Biome
    {

        public GasBiome()
        {

        }

        public GasBiome(Texture2D planetBorder, Vector2 planetBorderPosition, float layer, string planetWealth)
            : base(planetBorder, planetBorderPosition, layer, planetWealth)
        {
            baseTexture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Planet View/Biomes/Gas/GasBase");
            baseTexture.Name = "Res/PH/Planet View/Biomes/Gas/GasBase";

            //texture = Game1.INSTANCE.Content.Load<Texture2D>(AssetNameGenerator(biomeType));

            biomeType = "Gas";


        }
    }

    public class MountainBiome : Biome
    {

        public MountainBiome()
        {
               
        }

        public MountainBiome(Texture2D planetBorder, Vector2 planetBorderPosition, float layer, string planetWealth)
            : base(planetBorder, planetBorderPosition, layer, planetWealth)
        {

            baseTexture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Planet View/Biomes/Mountains/MountainsBase");
            baseTexture.Name = "Res/PH/Planet View/Biomes/Mountains/MountainsBase";

            biomeType = "Mountains";

            diseaseRisk = 15;

            assetName = AssetNameGenerator(biomeType);
            texture = Game1.INSTANCE.Content.Load<Texture2D>(assetName);

            SetValues();
        }

        public override void SetValues(bool loaded = false)
        {
                        huntingDifficulty = 70;
            humongousAnimalPercentage = 40;
            hugeAnimalPercentage = 50;
            largeAnimalPercentage = 65;
            mediumAnimalPercentage = 80;
            smallAnimalPercentage = 90;
            microscopicAnimalPercentage = 100;



            collision = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);


            //Sets the percentages
            clearWaterMultiplier = 0.4f;
            copperMultiplier = 0.6f;
            ironMultiplier = 0.8f;
            carbonMultiplier = 0.4f;

            if(!loaded)
                AddResources();
        }
    }

    public struct BiomeSave
    {
        public Vector2 position;

        public Vector2 baseTexturePosition;

        public float layer;

        public string biomeType;

        public string assetName;

        public string baseTextureAssetName;

        public float clearWaterAmount;
        
        public float copperAmount;
        
        public float ironAmount;

        public float carbonAmount;
    }
}