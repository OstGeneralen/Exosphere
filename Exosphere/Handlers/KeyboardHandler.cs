using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Handlers
{
    static class KeyboardHandler
    {
        //Represents the currently pressed key
        static KeyboardState currentKey;

        //Represents the key that was pressed during the previous loop
        static KeyboardState oldkey;

        /// <summary>
        /// Updates the key-variables
        /// </summary>
        public static void Update()
        {
            oldkey = currentKey;

            currentKey = Keyboard.GetState();
        }

        /// <summary>
        /// Checks if a key is pressed once only. Will return false if the key is held down
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>Returns true if the key is pressed and false if the key is pressed and have been held down since the last loop. Will also return false if the key is not pressed</returns>
        public static bool PressedOnce(Keys key)
        {
            if (currentKey.IsKeyDown(key) && oldkey != currentKey)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if a key is pressed
        /// </summary>
        /// <param name="checkKey">The key to chekc</param>
        /// <returns>Returns true if the key is pressed and false if it is not</returns>
        public static bool Pressed(Keys checkKey)
        {
            if (currentKey.IsKeyDown(checkKey))
            {
                return true;
            }

            return false;
        }
    }
}
