using Exosphere.Src.Generators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.HUD
{
    class ColonistChoice : Button
    {
        Colonist colonist;
        public bool showStats;

        public ColonistChoice(string assetName, Vector2 position, Colonist colonist)
            : base(assetName, position, colonist.name)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>(assetName);
            this.colonist = colonist;
        }

        public override void Update()
        {
            if (Collision() && !showStats)
            {
                showStats = true;

            }
            else if (Collision() && showStats)
            {
                showStats = false;
            }

            if (!showStats)
                texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/Standard/ButtonUpPH");

            if (showStats)
            {
                MessageBox mb;
                string message = "";
                
                message = message.Insert(message.Length, colonist.name + " \n \n");

                //Determine what should be shown about colonists based on evaluation
                if (colonist.levelOneEvaluated || colonist.levelTwoEvaluated || colonist.levelThreeEvaluated)
                {
                    message = message.Insert(message.Length, "Strength: " + colonist.strength + "\n");
                    message = message.Insert(message.Length, "Intelligence: " + colonist.intelligence + "\n");

                    if (colonist.levelTwoEvaluated || colonist.levelThreeEvaluated)
                    {
                        message = message.Insert(message.Length, "Immune System: " + colonist.immuneSystem + "\n");
                        message = message.Insert(message.Length, "Efficiency: " + colonist.efficiency + "\n");
                    }
                }

                    message = message.Insert(message.Length, "Health: " + colonist.health + "\n");
                

                if (colonist.diseased)
                    message = message.Insert(message.Length, "Disease: " + colonist.diseaseName + "\n");
                if (!colonist.diseased)
                    message = message.Insert(message.Length, "Disease: Currently not diseased" + "\n");

                mb = new MessageBox(3, message);
                Core.currentMessageBox = mb;

                showStats = false;
            }

            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);

        }

    }

}
