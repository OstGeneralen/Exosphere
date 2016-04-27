using Exosphere.Src.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src
{
   
    /// <summary>
    /// Implements the cursor as an object in the game
    /// </summary>
    static class Cursor
    {
        //Represents the graphic-file of the cursor
        static Texture2D standardTexture;

        //Represents the graphic-file of tje probing cursor
        static Texture2D probingTexture;

        static Texture2D currentTexture;

        //Represents the cursors collisionrectangle
        public static Rectangle collision;


        /// <summary>
        /// Saves the cursors texture and saves the position to implement it in the game
        /// </summary>
        public static void CreateCursor()
        {
            //Saves the texture of the file "assetName" = users input
            standardTexture = Game1.INSTANCE.Content.Load<Texture2D>("Res/Cursor");

            //Saves the texture
            probingTexture = Game1.INSTANCE.Content.Load<Texture2D>("Res/ProbeAimPH");

            currentTexture = standardTexture;

            //Calculates the collisionrectangles position, width and height
            collision = new Rectangle((int)MouseHandler.POSITION.X, (int)MouseHandler.POSITION.Y, 1, 1);
        }

        public static Vector2 GetCollisionArea()
        {
            return new Vector2(collision.X, collision.Y);
        }

        /// <summary>
        /// Updates the cursor
        /// </summary>
        public static void Update()
        {

            collision = new Rectangle((int)(MouseHandler.POSITION.X), (int)(MouseHandler.POSITION.Y), 1, 1);

            if (Core.currentScreen == Core.galaxyScreen)
            {
                //Updates the cursor's collisionrctangle(moves it to the cursors current position
                collision = new Rectangle((int)(MouseHandler.POSITION.X - Camera.position.X), (int)(MouseHandler.POSITION.Y - Camera.position.Y), 1, 1);
            }


        }

        /// <summary>
        /// Sets the texture of the curson
        /// </summary>
        /// <param name="name">Standard for standard, Probe for Probing</param>
        public static void SetCurrentTexture(string name)
        {
            if (name == "Standard")
                currentTexture = standardTexture;
            if (name == "Probe")
                currentTexture = probingTexture;
        }

        /// <summary>
        /// Draws the cursor on screen to a specified position
        /// </summary>
        /// <param name="spriteBatch">Used to draw stuff on screen</param>
        public static void Draw(SpriteBatch spriteBatch)
        {
                //Draws the cursor
            if(currentTexture == standardTexture)
                spriteBatch.Draw(currentTexture, MouseHandler.POSITION, Color.White);

            if (currentTexture == probingTexture)
                spriteBatch.Draw(currentTexture, new Vector2(MouseHandler.POSITION.X - currentTexture.Width / 2, MouseHandler.POSITION.Y - currentTexture.Height / 2), Color.White);

            
        }
    }
}
