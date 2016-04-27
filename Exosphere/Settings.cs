using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src
{
    static class Settings
    {

        public static bool DEBUG;
        private static Vector2 screenRes;

        public static Random RANDOM;

        public static Vector2 GetScreenRes
        {
            get { return screenRes; }
        }

    

        public static void SetValues()
        {

            DEBUG = true;
            screenRes = new Vector2(Game1.INSTANCE.GraphicsDevice.Adapter.CurrentDisplayMode.Width,
                Game1.INSTANCE.GraphicsDevice.Adapter.CurrentDisplayMode.Height);

            RANDOM = new Random();
            
            Debug.WriteLine("Screen resolution: " + screenRes.X + ":" + screenRes.Y);

        }

    }


    static class Debug
    {
        /// <summary>
        /// Outputs the string inputed to console if DEBUG is true
        /// </summary>
        /// <param name="text">The string to output</param>
        public static void WriteLine(string text)
        {
            if (Settings.DEBUG)
            {
                Console.WriteLine(text);
            }

        }

        /// <summary>
        /// Outputs the string inputed to console if DEBUG is true
        /// </summary>
        /// <param name="text">The string to output</param>
        public static void Write(string text)
        {
            if (Settings.DEBUG)
            {
                Console.Write(text);
            }
        }

    }
}
