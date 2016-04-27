using Exosphere.Src.Handlers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src
{
  
    static class Camera
    {

        public static CameraSave save;

        public static void LoadCamera(CameraSave load)
        {
            position = load.position;
            moveTo = load.position;
        }

        public static void SaveCamera()
        {
            save.position = position;
        }

        //Represents the camera's current position in pixels
        public static Vector2 position;
        //Represents your next waypoint
        static Vector2 moveTo;
        //Represents the total pixels from the screens center
        static Vector2 Sum;
        //Creates a translation from the cameraposition
        public static Matrix matrix { get { return Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0)); } }
        /// <summary>
        /// Moves the cameraposition to a waypoint placed by clicking on a specific point
        /// </summary>
        public static void Update()
        {
            //Calculates how far the cursor is from the center of the screen in the x-axis
            Sum.Y = position.Y + (Settings.GetScreenRes.Y / 2 - MouseHandler.POSITION.Y);
            //Calculates how far the cursor is from the center of the screen in the y-axis
            Sum.X = position.X + (Settings.GetScreenRes.X / 2 - MouseHandler.POSITION.X);
            if (MouseHandler.RMBPressed())
            {
                //Saves the waypoint
                moveTo.X = Sum.X;
                moveTo.Y = Sum.Y;
            }
            //Moves the camera
            position.X = MathHelper.Lerp(position.X, moveTo.X, 0.025f);
            position.Y = MathHelper.Lerp(position.Y, moveTo.Y, 0.025f);

            //Make the galaxy limitless
            #region Controll the camera
            if (position.X > Core.galaxyScreen.galaxy.galaxySizeMax.X + (Settings.GetScreenRes.X * 2))
            {
                position.X = Core.galaxyScreen.galaxy.galaxySizeMin.X - Settings.GetScreenRes.X;
            }

            if(position.X < Core.galaxyScreen.galaxy.galaxySizeMin.X - (Settings.GetScreenRes.X * 2))
            {
                position.X = Core.galaxyScreen.galaxy.galaxySizeMax.X + Settings.GetScreenRes.X;
            }

            if (position.Y > Core.galaxyScreen.galaxy.galaxySizeMax.Y + (Settings.GetScreenRes.Y * 2))
            {
                position.Y = Core.galaxyScreen.galaxy.galaxySizeMin.Y - Settings.GetScreenRes.Y;
            }

            if (position.Y < Core.galaxyScreen.galaxy.galaxySizeMin.Y - (Settings.GetScreenRes.Y * 2))
            {
                position.Y = Core.galaxyScreen.galaxy.galaxySizeMax.Y + Settings.GetScreenRes.Y;
            }
            #endregion

        }
    }

    public struct CameraSave
    {
        public Vector2 position;
    }
}
