using Exosphere.Src.Basebuilding;
using Exosphere.Src.Generators;
using Exosphere.Src.Handlers;
using Exosphere.Src.HUD;
using Exosphere.Src.Items;
using Exosphere.Src.Items.Vehicles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Exploring
{
    public class Explorer
    {

        ResourceArea findings;

        //A disease holder to contain possible diseases that can be brought home from exploring
        Disease travelDisease;

        bool haveReturnedHome;

        //A bool to tell if the program should check if the colonist get's killed or not
        bool checkIfKilled;

        //A bool to tell if the colonist have reached the goal of the journey
        bool haveReachedGoal;

        //The waypoint that the explorer should go to from its current position
        Waypoint nextWaypoint;

        //The explorer
        Colonist explorer;

        //The collision of the explorer
        Rectangle collision;

        //The collision for if the player presses the explorer
        public Rectangle cursorCollision;

        //The marker on the map for the Explorer
        Texture2D markerTexture;

        //The current position on the planet of the Explorer
        Vector2 currentPosition;
        Vector2 newPosition;

        //The current backpack
        int backpack;

        //The maximum amount in backpack
        int backpackLimit;

        //The speed to use
        float speed;

        #region Vehicle variables
        //A bool that is true if the explorer uses a vehicle
        bool useVehicle;
        //The vehicle the explorer can use
        Vehicle vehicle;
        #endregion

        Vector2 findingsPosition;

        //An int to divide the speed with
        int speedDivider = 500;

        List<Waypoint> waypoints;

        #region Load/Save
        public ExplorerSave save;

        public void LoadExplorer(ExplorerSave load, Colony explorerHome)
        {
            backpackLimit = load.backPackLimitSave;
            backpack = load.backPackSave;

            explorer = new Colonist(null, 0);
            explorer.LoadColonist(load.colonistSave, explorerHome);

            waypoints = new List<Waypoint>();
            for (int i = 0; i < load.waypointSaves.Count; i++)
            {
                waypoints.Add(new Waypoint(Vector2.Zero));

                waypoints[i].LoadWaypoint(load.waypointSaves[i]);
            }

            if(load.hasVehicle)
            {
                vehicle = new BasicVehicle();
                vehicle.LoadVehicle(load.vehicleSave);
            }

            speed = load.speed;

            findings = new ResourceArea(load.findingsSave);

            markerTexture = Game1.INSTANCE.Content.Load<Texture2D>(load.assetName);

            nextWaypoint = new Waypoint(Vector2.Zero);
            nextWaypoint.LoadWaypoint(load.nextWaypointSave);

            currentPosition = load.currentPositionSave;
            newPosition = load.currentPositionSave;

            
        }

        public void SaveExplorer()
        {
            save.waypointSaves = new List<WaypointSave>();

            explorer.SaveColonist();
            save.colonistSave = explorer.save;

            foreach(var waypoint in waypoints)
            {
                waypoint.SaveWaypoint();

                save.waypointSaves.Add(waypoint.save);
            }

            if(findings != null)
            {
                findings.SaveResourceArea();

                save.findingsSave = findings.save;
            }

            save.backPackLimitSave = backpackLimit;
            save.backPackSave = backpack;

            save.hasVehicle = false;
            if(vehicle != null)
            {
                save.hasVehicle = true;
                vehicle.SaveVehicle();

                save.vehicleSave = vehicle.save; 
            }

            nextWaypoint.SaveWaypoint();
            save.nextWaypointSave = nextWaypoint.save;

            save.currentPositionSave = currentPosition;

            save.haveReachedGoalSave = haveReachedGoal;

            save.speed = speed;

            save.assetName = markerTexture.Name;
        }
        #endregion

        public Explorer()
        {

        }

        /// <summary>
        /// Creates a new explorer
        /// </summary>
        /// <param name="explorer">The colonist that should become an explorer</param>
        /// <param name="startingWaypoint">The first waypoint the explorer should go to</param>
        public Explorer(Colonist explorer, Vehicle vehicle = null)
        {

            waypoints = new List<Waypoint>();

            //Load the colonist into the local variable for the explorer/colonist
            this.explorer = explorer;

            if (vehicle != null)
                useVehicle = true;
            else if (vehicle == null)
                useVehicle = false;

            //Set the speed of the explorer
            //If the explorer is not using a vehicle the speed should be the one of the colonist alone
            if(!useVehicle)
                speed = (float)explorer.strength * (float)explorer.efficiency;
            //If the explorer is using a vehicle the speed should be the one of the vehicle
            if(useVehicle)
            {
                this.vehicle = vehicle;
                speed = (float)vehicle.GetSpeed();

            }

            speed /= speedDivider;

            //Load the texture for the marker on the planet screen for the explorer
            string assetName = "Res/PH/Planet View/explorerMarkerPH";
            markerTexture = Game1.INSTANCE.Content.Load<Texture2D>(assetName);
            markerTexture.Name = assetName;


            //Set the starting position to be the colony's
            currentPosition = explorer.GetHome().position;
            newPosition = currentPosition;

            //Set both bools to false
            checkIfKilled = false;
            haveReachedGoal = false;
            haveReturnedHome = false;

            //Set the backpacklimit if the explorer does use a vehicle
            if(vehicle != null)
                backpackLimit = vehicle.GetStorageLimit();

            //Does the same thing but bearing in mind that the colonist now doesn't use a vehicle
            if (vehicle == null)
                backpackLimit = 10;


            backpack = 10;

            explorer.GetHome().GetGrid().resourceManager.food -= 10;
            explorer.GetHome().GetGrid().resourceManager.clearwater -= 10;
        }

        public Colonist GetColonist()
        {
            return explorer;
        }

        #region Travel
        /// <summary>
        /// Adds a new waypoint to the Explorer's route
        /// </summary>
        /// <param name="position">The position of the waypoint</param>
        public void AddWaypoint(Vector2 position)
        {
            waypoints.Add(new Waypoint(position));
        }

        public void CreateRoute()
        {
            if (waypoints.Count > 0)
            {
                nextWaypoint = waypoints[0];


                for (int i = 0; i < waypoints.Count; i++)
                {
                    if (i != 0 && i != waypoints.Count - 1)
                    {
                        waypoints[i].SetPreviousWayPoint(waypoints[i - 1]);
                        waypoints[i].SetFollowingWaypoint(waypoints[i + 1]);
                    }
                    else if (i == waypoints.Count - 1 && waypoints.Count != 1)
                    {
                        waypoints[i].SetPreviousWayPoint(waypoints[i - 1]);
                    }
                    else if (i == 0 && waypoints.Count != 1)
                    {
                        waypoints[i].SetFollowingWaypoint(waypoints[i + 1]);
                        waypoints[i].SetPreviousWayPoint(new Waypoint(explorer.GetHome().position, true));
                    }
                    else if (waypoints.Count == 1)
                    {
                        waypoints[i].SetPreviousWayPoint(new Waypoint(explorer.GetHome().position, true));
                    }
                }
            }

            else if(waypoints.Count <= 0)
            {
                haveReachedGoal = true;
                haveReturnedHome = true;
                string message = "No waypoints assigned to " + explorer.name + ". The colonist will return home immediately the coming day.";

                Core.currentMessageBox = new MessageBox(2, message);

            }
        }
        public void Travel()
        {
            if (TimeHandler.newTurn)
            {
                #region Start of day

                if (travelDisease == null)
                {
                    int basicRisk = 20;
                    int diseaseRisk = Settings.RANDOM.Next(basicRisk, explorer.GetHome().GetPlanet().GetBiomes().BiomeColiding(collision).GetDiseaseRisk() + basicRisk);
                    if (diseaseRisk > explorer.immuneSystem)
                    {
                        travelDisease = new Disease();
                    }
                }

                //Load whether the explorer finds food or not into a foundFood bool
                bool foundFood = FindFood();

                //If the colonist found food
                if (foundFood)
                {
                    //If the backpack is full 
                    if (backpack > backpackLimit)
                    {
                        //Keep the package att full and no more
                        backpack = backpackLimit;
                    }
                }
                //If the explorer doesn't find food and the backpack contains less than its food consumption
                else if (!foundFood && backpack <= explorer.GetFoodConsumption())
                {
                    //Remove health from the colonist
                    explorer.health -= explorer.GetFoodConsumption() * 5;
                }
                //If the colonist doesnät find food and the backpack contains enough food
                else if (!foundFood && backpack > 0)
                {
                    //Let the explorer eat from its backpack
                    backpack -= explorer.GetFoodConsumption();

                    //If the backpack gets under 0 set it to 0
                    if (backpack < 0)
                        backpack = 0;
                }


                //If the explorer should run a kill-check
                if (checkIfKilled)
                {
                    //Randomize a number from 1-100
                    int isKilled = Settings.RANDOM.Next(1, 101);

                    //If the randomized number is less or equal to 25 kill the explorer
                    if (vehicle == null)
                    {
                        if (isKilled <= 25)
                        {
                            explorer.health = 0;
                        }

                        else if (isKilled > 25)
                            checkIfKilled = false;
                    }

                    if(vehicle != null)
                    {
                        if (isKilled <= 1)
                        {
                            explorer.health = 0;
                        }

                        else if (isKilled > 5)
                            checkIfKilled = false;
                    }
                }

                //If the explorer's health is less than or equal to zero
                if (explorer.health <= 0)
                  {
                    string temp = "";

                    if (explorer.gender == "Female")
                        temp = "her";
                    else if(explorer.gender == "Male")
                        temp = "his";

                    MessageBox mb = new MessageBox(1, explorer.name + " died during " + temp + " expedition. ");
                    explorer.GetHome().GetPlanet().explorersRemoveList.Add(this);

                    Core.currentMessageBox = mb;
                }

                #endregion

                #region During day
                if (nextWaypoint != null)
                    Move();

                if (nextWaypoint == null) // nextWaypoint.isHome && haveReachedGoal && collision.Intersects(nextWaypoint.GetCollision()) ||
                {
                     haveReturnedHome = true;
                }

                if (!haveReturnedHome)
                {
                    //If the explorer have reached a waypoint
                    if (nextWaypoint.HaveReached(cursorCollision))
                    {


                        if (!haveReturnedHome)
                        {
                            //Check if the waypoint is the goal of the journey or not
                            //If it is not get the next waypoint


                            if (nextWaypoint.IsGoal() && !haveReachedGoal)
                            {

                                for (int i = 0; i < explorer.GetHome().GetPlanet().GetResourceArea().Length; i++)
                                {
                                    if(cursorCollision.Intersects(explorer.GetHome().GetPlanet().GetResourceArea()[i].GetArea()))
                                    {
                                        explorer.GetHome().GetPlanet().GetResourceArea()[i].AddResources(GetCurrentBiome());
                                        findings = explorer.GetHome().GetPlanet().GetResourceArea()[i];
                                        findingsPosition = nextWaypoint.GetPosition();
                                    }
                                }

                                haveReachedGoal = true;

                            }

                            nextWaypoint = nextWaypoint.GetFollowingWaypoint(haveReachedGoal);

                            //If it is: set the bool telling if the explorer have reached the goal to true

                        }


                    }
                }

                if (haveReturnedHome)
                {

                    if (findings == null)
                    {
                        MessageBox mb = new MessageBox(1, explorer.name + " found no suitable mining area. ");
                        Core.currentMessageBox = mb;
                    }
                    else if (findings.GetCopper() == 0 && findings.GetIron() == 0 && findings.GetCarbon() == 0)
                    {
                        MessageBox mb = new MessageBox(1, explorer.name + " found no suitable mining area. ");
                        Core.currentMessageBox = mb;
                    }
                    else if (findings.GetCopper() > 0 && findings.GetIron() > 0 && findings.GetCarbon() > 0)
                    {
                        string copperWealth = "";
                        string ironWealth = "";
                        string carbonWealth = "";

                        if (findings.GetCopper() <= 200000)
                            copperWealth = "Poor";
                        else if (findings.GetCopper() > 200000 && findings.GetCopper() <= 100000)
                            copperWealth = "Normal";
                        else if (findings.GetCopper() > 100000 && findings.GetCopper() <= 1500000)
                            copperWealth = "Rich";
                        else if (findings.GetCopper() > 1500000)
                            copperWealth = "Plentiful";

                        if (findings.GetIron() <= 500000)
                            ironWealth = "Poor";
                        else if (findings.GetIron() > 500000 && findings.GetIron() <= 1000000)
                            ironWealth = "Normal";
                        else if (findings.GetIron() > 1000000 && findings.GetIron() <= 2000000)
                            ironWealth = "Rich";
                        else if (findings.GetIron() > 2000000)
                            ironWealth = "Plentiful";

                        if (findings.GetCarbon() <= 500000)
                            carbonWealth = "Poor";
                        else if (findings.GetCarbon() > 500000 && findings.GetCarbon() <= 1000000)
                            carbonWealth = "Normal";
                        else if (findings.GetCarbon() > 1000000 && findings.GetCarbon() <= 2000000)
                            carbonWealth = "Rich";
                        else if (findings.GetCarbon() > 2000000)
                            carbonWealth = "Plentiful";


                        string message = explorer.name + " found a suitable mining area and reports the following about it: \nEstimated wealth (copper): " + copperWealth + "\nEstimated wealth (iron): " + ironWealth + "\nEstimated wealth (carbon): " + carbonWealth;

                        MessageBox mb = new MessageBox(3, message);
                        Core.currentMessageBox = mb;

                        explorer.GetHome().GetPlanet().possibleMiningPoints.Add(new PossibleMiningPoint(findingsPosition, copperWealth, ironWealth, carbonWealth, findings.GetCopper(), findings.GetCarbon(), findings.GetIron()));
                    }

                    explorer.GetHome().AddColonist(explorer);
                    explorer.GetHome().GetPlanet().explorersRemoveList.Add(this);
                    if (travelDisease != null)
                        explorer.GetHome().diseases.Add(travelDisease);
                }
                

                #endregion
            }

            if (Core.currentScreen == Core.planetScreen)
            {
                currentPosition.X = MathHelper.Lerp(currentPosition.X, newPosition.X, 0.1f);
                currentPosition.Y = MathHelper.Lerp(currentPosition.Y, newPosition.Y, 0.1f);
            }
            else if(Core.currentScreen != Core.planetScreen || Core.planetScreen.GetPlanet() != explorer.GetHome().GetPlanet())
            {
                currentPosition = newPosition;
            }


        }
        
        private void Move()
        {
            newPosition = currentPosition;

            for (int i = 0; i < speed; i++)
            {
                //If the X position of the waypoint is bigger than the current position's X value, raise the current X by the speed
                if (newPosition.X < nextWaypoint.GetPosition().X)
                    newPosition.X += 1;
                //Do the opposite if the X position of the waypoint is lower instead
                else if (newPosition.X > nextWaypoint.GetPosition().X)
                    newPosition.X -=  1;

                //Do the same thing for the Y axis
                if (newPosition.Y < nextWaypoint.GetPosition().Y)
                    newPosition.Y += 1;
                else if (newPosition.Y > nextWaypoint.GetPosition().Y)
                    newPosition.Y -= 1;
            }

            collision = new Rectangle((int)(currentPosition.X + markerTexture.Width / 2), (int)(currentPosition.Y + markerTexture.Height / 2), 1, 1);

        }
        #endregion

        #region Food
        public bool FindFood()
        {
            int hunt = Hunt();

            if (hunt > 0)
            {
                backpack += hunt;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Makes the hunter go out hunting
        /// </summary>
        /// <returns>The amount of food brought back to the colony</returns>
        public int Hunt()
        {
            string prey;
            int food = 0;

            prey = GetCurrentBiome().HuntResult(explorer);

            if (prey != null)
            {
                food += Kill(prey);
            }

            return food;
        }

        private int Kill(string animalSize)
        {
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
                animalStr = Settings.RANDOM.Next(1, 6);
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
            if (animalInt >= explorer.intelligence)
            {
                animalChance += 1;

                if ((animalInt - explorer.intelligence) >= 5)
                    animalChance += 1;

                if ((animalInt - explorer.intelligence) >= 10)
                    animalChance += 1;

                if ((animalInt - explorer.intelligence) >= 20)
                    animalChance += 1;
            }

            if (animalStr >= explorer.strength)
            {
                animalChance += 1;

                if ((animalStr - explorer.strength) >= 5)
                    animalChance += 1;

                if ((animalStr - explorer.strength) >= 10)
                    animalChance += 1;

                if ((animalStr - explorer.strength) >= 20)
                    animalChance += 1;

            }
            #endregion

            #region Hunter better than Animal
            if (animalInt < explorer.intelligence)
            {
                hunterChance += 1;

                if ((explorer.intelligence - animalInt) >= 5)
                    hunterChance += 1;

                if ((explorer.intelligence - animalInt) >= 10)
                    hunterChance += 1;

                if ((explorer.intelligence - animalInt) >= 20)
                    hunterChance += 1;
            }

            if (animalStr < explorer.strength)
            {
                hunterChance += 1;

                if ((explorer.strength - animalStr) >= 5)
                    hunterChance += 1;

                if ((explorer.strength - animalStr) >= 10)
                    hunterChance += 1;

                if ((explorer.strength - animalStr) >= 20)
                    hunterChance += 1;

            }
            #endregion

            int hunterLuck = Settings.RANDOM.Next(0, 6);
            int animalLuck = Settings.RANDOM.Next(0, 6);

            if (hunterLuck + hunterChance > animalLuck + animalChance)
            {
                return foodHold;
            }
            else if (hunterLuck + hunterChance < animalLuck + animalChance)
            {
                if ((animalLuck + animalChance) - (hunterLuck + hunterChance) >= 5)
                    checkIfKilled = true;

                return 0;
            }

            //throw new Exception("Explorer did not kill or fail to kill an animal");
            return 0;
        }
        #endregion

        /// <summary>
        /// Gets the biome the explorer is currently on
        /// </summary>
        /// <returns>Returns the biome the explorer is on</returns>
        public Biome GetCurrentBiome()
        {
            return explorer.GetHome().GetPlanet().GetBiomes().BiomeColiding(collision);
        }

        #region Basic
        public void Update()
        {

            cursorCollision = new Rectangle((int)(currentPosition.X + markerTexture.Width * 0.5f), (int)(currentPosition.Y + markerTexture.Height * 0.5f), 5, 5);

            //if (waypoints.Count > 0)
                Travel();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (explorer.health > 0)
            {
                spriteBatch.Draw(markerTexture, new Vector2(currentPosition.X - markerTexture.Width / 2, currentPosition.Y - markerTexture.Height / 2), Color.White);
            }
        }
        #endregion
    }

    public struct ExplorerSave
    {
        public ColonistSave colonistSave;
        public List<WaypointSave> waypointSaves;
        public ResourceAreaSave findingsSave;
        public int backPackSave;
        public int backPackLimitSave;
        public VehicleSave vehicleSave;
        public WaypointSave nextWaypointSave;
        public Vector2 currentPositionSave;
        public bool haveReachedGoalSave;
        public bool hasVehicle;
        public float speed;
        public string assetName;

    }
}


