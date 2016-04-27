using Exosphere.Src.Generators;
using Exosphere.Src.Handlers;
using Exosphere.Src.HUD;
using Exosphere.Src.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Exploring
{
    class ExplorationScreen
    {
        //Misc
        Planet planet;

        //Buttons
        Button assign;
        Button debrief;

        AddExplorer addExplorer;

        //Bools
        //Add new explorer
        bool chooseExplorer;
        bool assignWaypoints;
        bool send;


        bool showStandardScreen;

        Vehicle vehicle;

        //SpriteFont
        SpriteFont font;

        public ExplorationScreen()
        {
            //Misc
            addExplorer = new AddExplorer();

            //Buttons
            assign = new Button("Res/PH/HUD/Buttons/Standard/ButtonUpPH", Vector2.Zero, "Assign Explorers");
            debrief = new Button("Res/PH/HUD/Buttons/Standard/ButtonUpPH", Vector2.Zero, "Debrief");

            int x = (int)((Settings.GetScreenRes.X * 0.25f) - (assign.GetTexture().Width / 2));
            int y = (int)((Settings.GetScreenRes.Y * 0.5f) - (assign.GetTexture().Height / 2));

            assign.SetPosition(new Vector2(x, y));

            x = (int)((Settings.GetScreenRes.X * 0.75f) - (debrief.GetTexture().Width / 2));
            
            debrief.SetPosition(new Vector2(x, y));

            //SpriteFont
            font = Game1.INSTANCE.Content.Load<SpriteFont>("Res/Fonts/Message");

            //Bools
            chooseExplorer = false;
            assignWaypoints = false;
            send = false;
            showStandardScreen = false;
        }

        /// <summary>
        /// Sets the current planet that the player will be able to controll the colonists on
        /// </summary>
        /// <param name="planet">The planet that the explorers will explore</param>
        public void SetPlanet(Planet planet)
        {
            this.planet = planet;
            addExplorer.SetPlanet(planet);
        }

        public void Update(Vehicle vehicle = null)
        {

            addExplorer.Update(vehicle);
        }

        public void CloseExplorationScreen()
        {
            addExplorer.ClearButtonList();
        }

        #region Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            addExplorer.Draw(spriteBatch);
        }

        private void OrdersDraw(SpriteBatch spriteBatch)
        {

        }

        private void AssignExplorersDraw(SpriteBatch spriteBatch)
        {

        }
        #endregion

    }

    class AddExplorer
    {
        bool assignExplorer;
        bool assignWaypoints;
        bool send;

        Explorer explorer;
        List<WorkersButton> buttons;
        List<Waypoint> waypoints;

        Planet planet;

        public AddExplorer()
        {
            buttons = new List<WorkersButton>();
            waypoints = new List<Waypoint>();
            assignExplorer = true;
        }

        public void SetPlanet(Planet planet)
        {
            this.planet = planet;
            buttons.Clear();
        }

        private void AssignExplorer(Vehicle vehicle = null)
        {

           
            //Create buttons to assign colonists to be explorers
            if (buttons.Count == 0)
            {
                int amount = 0;
                foreach (var colonist in planet.GetColony().GetInhabitants())
                {
                    if (!colonist.occupied)
                    {
                        buttons.Add(new WorkersButton(colonist, amount));
                        amount++;
                    }
                }
            }

            //Update the buttons
            if (buttons.Count > 0)
            {
                foreach (var button in buttons)
                {
                    button.Update();
                    if (button.Collision())
                    {
                        explorer = new Explorer(button.colonist, vehicle);
                        MouseHandler.Update();

                        assignExplorer = false;
                        assignWaypoints = true;
                        break;
                    }
                }
            }
        }

        private void AssignWaypoints()
        {
            buttons.Clear();

            if(MouseHandler.LMBOnce())
            {
                waypoints.Add(new Waypoint(Cursor.GetCollisionArea()));
            }

            if(MouseHandler.RMBOnce())
            {
                foreach(var waypoint in waypoints)
                {
                    explorer.AddWaypoint(waypoint.GetPosition());
                }
                explorer.CreateRoute();
                assignWaypoints = false;
                send = true;
            }
        }
        
        private void Send()
        {
            planet.explorers.Add(explorer);
            waypoints.Clear();
            planet.GetColony().RemoveColonist(explorer.GetColonist());
            send = false;
            assignExplorer = true;
        }

        public void Update(Vehicle vehicle = null)
        {
            if (assignExplorer)
                AssignExplorer(vehicle);
            if (assignWaypoints)
                AssignWaypoints();
            if (send)
                Send();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(assignExplorer)
            {
                foreach(var button in buttons)
                {
                    button.Draw(spriteBatch);
                }
            }

            if(assignWaypoints)
            {
                planet.Draw(spriteBatch);
                foreach(var waypoint in waypoints)
                {
                    waypoint.Draw(spriteBatch);
                }
            }
        }

        public void ClearButtonList()
        {
            buttons.Clear();
        }
    }
}
       