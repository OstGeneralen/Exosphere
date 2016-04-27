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
    class GalaxyScreen : Screen
    {
        //The galaxy in which the games take place
        public Galaxy galaxy;

        Probing probing;

        public bool isProbing;

        MessageBox outOfProbes;


        /// <summary>
        /// Creates a new Galaxy screeen
        /// </summary>
        public GalaxyScreen()
        {
            //Sets the screen name
            screenName = "Galaxy Screen";

            //Generates a new galaxy and saves it into the galaxy Galaxy
            galaxy = new Galaxy();

            probing = new Probing(galaxy);

            isProbing = false;

        }

        /// <summary>
        /// Get the screen name
        /// </summary>
        /// <returns>A string containing the screen name</returns>
        public override string GetScreen()
        {
            return "Galaxy Screen";
        }

        /// <summary>
        /// Updates the Galaxy Screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update()
        {

            if(Core.showGalaxyScreenInfo)
            {
                string message = "";

                message = message.Insert(message.Length, "Welcome to the Galaxy View\n\n");
                message = message.Insert(message.Length, "In this view you can navigate through the galaxy by pressing your right mouse button and move your cursor.\n\n");
                message = message.Insert(message.Length, "From here you can also probe the planets to find out whether they are Depleted, Poor, Normal, Rich or Plentiful.");
                message = message.Insert(message.Length, " To probe a planet, press the probe button in your HUD and then click on the planet you want to probe.\n\n");
                message = message.Insert(message.Length, "To view a planet just click on it.");

                MessageBox mb = new MessageBox(3, message);
                Core.currentMessageBox = mb;
                Core.showGalaxyScreenInfo = false;
            }

            //Updates the galaxy
            if(!isProbing)
                galaxy.Update();

            if(HUD.ActionButtons.probeButton.Collision() && probing.probes > 0)
            {
                isProbing = true;
                Cursor.SetCurrentTexture("Probe");
            }

            if(HUD.ActionButtons.probeButton.Collision() && probing.probes <= 0)
            {
                outOfProbes = new MessageBox(1, "You are out of probes");
                Core.currentMessageBox = outOfProbes;
            }

            if(isProbing)
                probing.Update();

            //Updates the camera
            Camera.Update();

        }

        /// <summary>
        /// Draws the Galaxy Screen
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used for drawing</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Add spritebatches and drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Camera.matrix);

            galaxy.Draw(spriteBatch);
            if(isProbing)
                probing.Draw(spriteBatch);
            //Cursor.Draw(spriteBatch);

            spriteBatch.End();
        }
    }

    class Probing
    {
        Texture2D texture;
        public int probes;
        MessageBox planetInfo;
        MessageBox outOfProbes;

        public Probing(Galaxy galaxy)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/ProbeAimPH");
            probes = 500;
        }

        public void Update()
        {
            if (MouseHandler.LMBOnce())
            {
                if (Core.galaxyScreen.galaxy.GetPlanetAt(new Rectangle((int)(Cursor.collision.X), (int)(Cursor.collision.Y), 1, 1)) != null)
                {

                        MouseHandler.Update();
                        probes--;
                        Core.galaxyScreen.isProbing = false;
                        Cursor.SetCurrentTexture("Standard");
                        planetInfo = new MessageBox(1, "This planet is " + Core.galaxyScreen.galaxy.GetPlanetAt(new Rectangle((int)(Cursor.collision.X), (int)(Cursor.collision.Y), 1, 1)).GetWealth() + "\nYou have " + probes.ToString() + " probes left");
                        Core.currentMessageBox = planetInfo;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(((MouseHandler.POSITION.X - texture.Width / 2) - Camera.position.X), (MouseHandler.POSITION.Y - texture.Height / 2) - Camera.position.Y), Color.White);
        }

    }

 
}
