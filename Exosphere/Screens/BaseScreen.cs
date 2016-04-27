using Exosphere.Src.Basebuilding;
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
    class BaseScreen : Screen
    {

        public static string facilityHold;


        Colony colony;

        public BaseScreen()
        {


            screenName = "Base Screen";


        }

   


        public void SetColony(Colony colony)
        {
            this.colony = colony;
        }

        public override void Update()
        {
            if(colony != null)
            colony.Update();


            if (HUD.ActionButtons.backButton.Collision())
                Core.SetPlanetView(colony.GetPlanet());


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            colony.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
