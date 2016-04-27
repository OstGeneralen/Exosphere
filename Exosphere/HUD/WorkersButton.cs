using Exosphere.Src.Generators;
using Exosphere.Src.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.HUD
{
    class WorkersButton : ActivatableButton
    {
        //The colonist the button represents
        public Colonist colonist;

        /// <summary>
        /// Create a new WorkersButton
        /// </summary>
        /// <param name="colonist">The colonist the button should represent</param>
        /// <param name="position">The inactive position of the button</param>
        public WorkersButton(Colonist colonist, int i)
            : base("Res/PH/HUD/Buttons/Standard/ButtonUpPH", Vector2.Zero, "")
        {
            //Loads the imported colonist to the local colonist-variable
            this.colonist = colonist;

            //Sets the position for the button
            position = new Vector2(Settings.GetScreenRes.X * 0.75f - texture.Width / 2, 0 + HUD.informationList.GetTexture().Height + texture.Height * i);

            label = colonist.name;
        }

        /// <summary>
        /// Updates the workers button
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (colonist.occupied)
                activation = colonist.occupied;
            else
                activation = colonist.occupied;
        }

        public override void ChangeColor()
        {
            if (collision.Intersects(Cursor.collision) && MouseHandler.LMBOnce() && !colonist.occupied)
                colonist.occupied = true;
            else if (collision.Intersects(Cursor.collision) && MouseHandler.LMBOnce() && colonist.occupied)
                colonist.occupied = false;
        }
    }
}
