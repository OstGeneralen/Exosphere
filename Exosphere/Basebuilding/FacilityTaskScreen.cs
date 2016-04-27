using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Basebuilding
{
    public abstract class FacilityTaskScreen
    {
        //The name of the facility the task screen is representing
        protected string name;
        //A bool telling if the facility represented should work or not
        protected bool shouldWork;

        public FacilityTaskScreen()
        {
        }

        public virtual bool ShouldWork()
        {
            return shouldWork;
        }

        public string GetFacilityName()
        {
            return name;
        }

        public virtual Object GetWorkingValue()
        {
            return null;
        }

        public virtual void Update()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

    }
}
