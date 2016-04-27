using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exosphere.Src;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exosphere.Src.Basebuilding.Facilities;
using Exosphere.Src.Handlers;
using System.Xml.Serialization;
using Exosphere.Src.Transferring;
using Exosphere.Src.Items.Vehicles;

namespace Exosphere.Src.Generators
{
    public class Galaxy
    {
        //An array containing all the planets in the genrated galaxy
        public GalaxyPlanetButton[] planetArray;
        public Vector2 galaxySizeMin, galaxySizeMax;
        public List<Transmission> transmissions;

        public GalaxySave save;

        public void LoadGalaxy(GalaxySave load)
        {
            planetArray = new GalaxyPlanetButton[load.galaxyPlanetButtons.Length];

            for (int i = 0; i < load.galaxyPlanetButtons.Length; i++)
            {
                planetArray[i] = new GalaxyPlanetButton(1, Vector2.Zero);
                planetArray[i].LoadGalaxyPlanetButton(load.galaxyPlanetButtons[i]);
            }

            transmissions = new List<Transmission>();

            for (int i = 0; i < load.transmissionSaves.Count; i++)
            {
                transmissions.Add(new Transmission(load.transmissionSaves[i].waypointSave.position,
                    load.transmissionSaves[i].transmissionShipSave.position, new VehicleC60()));
                transmissions[i].LoadTransmission(load.transmissionSaves[i]);
            }
        }

        public void SaveGalaxy()
        {
            save.galaxyPlanetButtons = new GalaxyPlanetButtonSave[planetArray.Length];

            for (int i = 0; i < planetArray.Length; i++)
            {
                planetArray[i].SaveGalaxyPlanetButton();
                save.galaxyPlanetButtons[i] = planetArray[i].save;
            }

            save.transmissionSaves = new List<TransmissionSave>();

            for (int i = 0; i < transmissions.Count; i++)
            {
                transmissions[i].SaveTransmission();
                save.transmissionSaves.Add(transmissions[i].save);
            }
        }

        /// <summary>
        /// Creates a new galaxy with randomly generated planets
        /// </summary>
        public Galaxy()
        {
            int amount;
            amount = Settings.RANDOM.Next(800, 2000);
            planetArray = new GalaxyPlanetButton[amount];

            transmissions = new List<Transmission>();

            AddPlanets();

        }

        public GalaxyPlanetButton[] GetPlanets()
        {
            return planetArray;
        }

        /// <summary>
        /// Creates new planets.
        /// </summary>
        private void AddPlanets()
        {

            //The values to be randomized.
            //Defines which main biome the planet will have
            int biome;

            //Defines the size of the planet (Small, Medium or Big)
            int size;

            //The vector2 position in two ints for X and Y
            int positionX;
            int positionY;

            //Loops through the array of planets and fills each spot in it with a newly generated planet
            for (int i = 0; i < planetArray.Length; i++)
            {
                //Randomizes the previously described variables
                biome = Settings.RANDOM.Next(1, 6);
                size = Settings.RANDOM.Next(1, 4);

                positionX = Settings.RANDOM.Next(-10000, (int)(Settings.GetScreenRes.X + 10000));
                positionY = Settings.RANDOM.Next(-10000, (int)(Settings.GetScreenRes.Y + 10000));

                planetArray[i] = new GalaxyPlanetButton(size, new Vector2(positionX, positionY));



            }



            galaxySizeMin = new Vector2(-10000, -10000);
            galaxySizeMax = new Vector2(Settings.GetScreenRes.X + 10000, Settings.GetScreenRes.Y + 10000);

        }

        /// <summary>
        /// Gets the planet at the specified rectangle
        /// </summary>
        /// <param name="collision">The rectangle in which you want to check for planets</param>
        /// <returns>A planet if there is any. Else Null</returns>
        public Planet GetPlanetAt(Rectangle collision)
        {
            for (int i = 0; i < planetArray.Length; i++)
            {
                if (planetArray[i].GetCollision().Intersects(collision))
                {
                    return planetArray[i].GetPlanet();
                }
            }
            return null;
        }

        /// <summary>
        /// Updates the planets
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update()
        {
            for (int i = 0; i < planetArray.Length; i++)
            {
                planetArray[i].Update();
                if (planetArray[i].Collision())
                {
                    planetArray[i].GetPlanet().isDiscovered = true;
                    Core.SetPlanetView(planetArray[i].GetPlanet());

                }
            }
            if (TimeHandler.newTurn)
            {
                List<Transmission> removeTransmission = new List<Transmission>();
                foreach (var transmission in transmissions)
                {
                    transmission.Movement();
                    if (transmission.ReachedGoal())
                    {
                        removeTransmission.Add(transmission);
                    }
                }
                foreach (var remove in removeTransmission)
                {
                    transmissions.Remove(remove);
                }
            }

        }

        /// <summary>
        /// Draws the galaxy
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch used to draw</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //Loops through the planets and output them all in a draw
            for (int i = 0; i < planetArray.Length; i++)
            {
                planetArray[i].Draw(spriteBatch);
            }

            foreach (var transmission in transmissions)
            {
                transmission.Draw(spriteBatch);
            }


        }

    }

    public struct GalaxySave
    {
        public GalaxyPlanetButtonSave[] galaxyPlanetButtons;
        public List<TransmissionSave> transmissionSaves;
    }
}
