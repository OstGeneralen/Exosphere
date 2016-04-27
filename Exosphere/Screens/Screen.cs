using Exosphere.Src.Generators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Screens
{
    abstract class Screen
    {
        //The screen name variable used for setting and getting the screen name
        protected string screenName;

        public Screen()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual string GetScreen()
        {
            return screenName;
        }

    }
}
