using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Handlers
{
    static class MouseHandler
    {
        //Represents the mouse state of the current loop
        static MouseState currentMouse;

        //Represents the mouse state of the previous loop
        static MouseState oldMouse;

        //Represents the position in pixels of the cursor
        static Vector2 position;

        /// <summary>
        /// Return the position of the cursor as a Vector2 in pixels
        /// </summary>
        public static Vector2 POSITION
        {
            get { return position; }
        }

        /// <summary>
        /// Updates the mouse. 
        /// </summary>
        public static void Update()
        {
            oldMouse = currentMouse;

            currentMouse = Mouse.GetState();

            position = new Vector2(currentMouse.X, currentMouse.Y);
        }

        /// <summary>
        /// Checks if the Left mouse button is pressed once only
        /// </summary>
        /// <returns>Returns true if the button is pressed only. Returns false if it has been held down since the previous loop or if it's not pressed at all</returns>
        public static bool LMBOnce()
        {
            if (currentMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the Left mouse button is pressed
        /// </summary>
        /// <returns>Returns true if the button is pressed. Returns false if it's not</returns>
        public static bool LMBPressed()
        {
            if (currentMouse.LeftButton == ButtonState.Pressed)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the Right mouse button is pressed once only
        /// </summary>
        /// <returns>Returns true if the button is pressed only. Returns false if it has been held down since the previous loop or if it's not pressed at all</returns>
        public static bool RMBOnce()
        {
            if (currentMouse.RightButton == ButtonState.Pressed && oldMouse.RightButton == ButtonState.Released)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the Right mouse button is pressed
        /// </summary>
        /// <returns>Returns true if the button is pressed. Returns false if it's not</returns>
        public static bool RMBPressed()
        {
            if (currentMouse.RightButton == ButtonState.Pressed)
            {
                return true;
            }

            return false;
        }




    }
}
