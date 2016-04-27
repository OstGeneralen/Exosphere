using Exosphere.Src.Exploring;
using Exosphere.Src.Generators;
using Exosphere.Src.Handlers;
using Exosphere.Src.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Screens
{
    class PlanetScreen : Screen
    {
        //A planet which be the active planet in this screen
        Planet planet;

        bool showExplorationScreen;
        public ExplorationScreen explorationScreen;

        MessageBox noColonyWarning;
        MessageBox canNotBuildOnGas;

        /// <summary>
        /// Creates a new planet screen
        /// </summary>
        public PlanetScreen()
        {
            //Sets the screen name
            screenName = "Planet Screen";
            explorationScreen = new ExplorationScreen();

            noColonyWarning = new MessageBox(1, "Exploration is not possible without colonists. Construct a colony first.");

        }

        public Planet GetPlanet()
        {
            return planet;
        }

        /// <summary>
        /// Sets which planet in the galaxy the screen should represent
        /// </summary>
        /// <param name="planet">The planet pressed by the player</param>
        public void SetPlanet(Planet planet)
        {
            this.planet = planet;

            explorationScreen.SetPlanet(planet);

            //If the planet does not exist throw exception
            if (planet == null)
                throw new Exception("Planet was empty");

            showExplorationScreen = false;


        }


        /// <summary>
        /// Updates the planet screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update()
        {

            if(Core.showPlanetScreenInfo)
            {
                string message = "";

                message = message.Insert(message.Length, "Welcome to the Planet View\n\n");
                message = message.Insert(message.Length, "The planet view provides you with an image of the planet and all its biomes.\n");
                message = message.Insert(message.Length, "If you haven't built a colony before or if you have landed a colony ship on it you can press ");
                message = message.Insert(message.Length, "anywhere to construct a colony. Colonies can not be built in water or on gas giants. Each planet can only have one colony ");
                message = message.Insert(message.Length, "so place it wisely.\n\n");
                message = message.Insert(message.Length, "Once you have built a colony you can also send out explorers from here by clicking on the Exploration button");
                Console.WriteLine("Hello World");

                MessageBox mb = new MessageBox(3, message);
                Core.currentMessageBox = mb;
                Core.showPlanetScreenInfo = false;
            }

            if (HUD.ActionButtons.backButton.Collision())
                Core.SetGalaxyView();

            HUD.PlanetHUD.Update();

            if(HUD.ActionButtons.explorationButton.Collision())
            {
                if (planet.hasColony)
                    showExplorationScreen = true;
                else if (!planet.hasColony)
                    Core.currentMessageBox = noColonyWarning;

            }

            if(showExplorationScreen)
            {
                explorationScreen.Update();
            }
            //Updates the planet
            if (planet != null)
                planet.Update();


        }

        /// <summary>
        /// Draws the planet screen
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used for drawing</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if (!showExplorationScreen)
            {
                //Draw the planet
                planet.Draw(spriteBatch);
            }

            if(showExplorationScreen)
            {
                explorationScreen.Draw(spriteBatch);
            }


            spriteBatch.End();
        }
    }
}
