using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exosphere.Src.Handlers;
using Microsoft.Xna.Framework.Input;

namespace Exosphere.Src.Handlers
{
    static class InputHandler
    {

        static string tempString;
        static KeyboardState keyBoardState = new KeyboardState();
        static List<Keys> oldKeys;
        static List<Keys> removeList;

        public static void CreateInputHandler()
        {
            tempString = "";
            oldKeys = new List<Keys>();
            removeList = new List<Keys>();
        }

        public static void Update()
        {
            keyBoardState = Keyboard.GetState();

            #region The alphabet
            if (KeyboardHandler.PressedOnce(Keys.A))
            {
                if(!oldKeys.Contains(Keys.A))
                    oldKeys.Add(Keys.A);
            }

            if (KeyboardHandler.PressedOnce(Keys.B))
            {
                if (!oldKeys.Contains(Keys.B))
                    oldKeys.Add(Keys.B);
            }

            if (KeyboardHandler.PressedOnce(Keys.C))
            {
                if (!oldKeys.Contains(Keys.C))
                    oldKeys.Add(Keys.C);
            }

            if (KeyboardHandler.PressedOnce(Keys.D))
            {
                if (!oldKeys.Contains(Keys.D))
                    oldKeys.Add(Keys.D);
            }

            if (KeyboardHandler.PressedOnce(Keys.E))
            {
                if (!oldKeys.Contains(Keys.E))
                    oldKeys.Add(Keys.E);
            }

            if (KeyboardHandler.PressedOnce(Keys.F))
            {
                if (!oldKeys.Contains(Keys.F))
                    oldKeys.Add(Keys.F);
            }

            if (KeyboardHandler.PressedOnce(Keys.G))
            {
                if (!oldKeys.Contains(Keys.G))
                    oldKeys.Add(Keys.G);
            }

            if (KeyboardHandler.PressedOnce(Keys.H))
            {
                if (!oldKeys.Contains(Keys.H))
                    oldKeys.Add(Keys.H);
            }

            if (KeyboardHandler.PressedOnce(Keys.I))
            {
                if (!oldKeys.Contains(Keys.I))
                    oldKeys.Add(Keys.I);
            }

            if (KeyboardHandler.PressedOnce(Keys.J))
            {
                if (!oldKeys.Contains(Keys.J))
                    oldKeys.Add(Keys.J);
            }

            if (KeyboardHandler.PressedOnce(Keys.K))
            {
                if (!oldKeys.Contains(Keys.K))
                    oldKeys.Add(Keys.K);
            }

            if (KeyboardHandler.PressedOnce(Keys.L))
            {
                if (!oldKeys.Contains(Keys.L))
                    oldKeys.Add(Keys.L);
            }

            if (KeyboardHandler.PressedOnce(Keys.M))
            {
                if (!oldKeys.Contains(Keys.M))
                    oldKeys.Add(Keys.M);
            }

            if (KeyboardHandler.PressedOnce(Keys.N))
            {
                if (!oldKeys.Contains(Keys.N))
                    oldKeys.Add(Keys.N);
            }

            if (KeyboardHandler.PressedOnce(Keys.O))
            {
                if (!oldKeys.Contains(Keys.O))
                    oldKeys.Add(Keys.O);
            }

            if (KeyboardHandler.PressedOnce(Keys.P))
            {
                if (!oldKeys.Contains(Keys.P))
                    oldKeys.Add(Keys.P);
            }

            if (KeyboardHandler.PressedOnce(Keys.Q))
            {
                if (!oldKeys.Contains(Keys.Q))
                    oldKeys.Add(Keys.Q);
            }

            if (KeyboardHandler.PressedOnce(Keys.R))
            {
                if (!oldKeys.Contains(Keys.R))
                    oldKeys.Add(Keys.R);
            }

            if (KeyboardHandler.PressedOnce(Keys.S))
            {
                if (!oldKeys.Contains(Keys.S))
                    oldKeys.Add(Keys.S);
            }

            if (KeyboardHandler.PressedOnce(Keys.T))
            {
                if (!oldKeys.Contains(Keys.T))
                    oldKeys.Add(Keys.T);
            }

            if (KeyboardHandler.PressedOnce(Keys.U))
            {
                if (!oldKeys.Contains(Keys.U))
                    oldKeys.Add(Keys.U);
            }

            if (KeyboardHandler.PressedOnce(Keys.V))
            {
                if (!oldKeys.Contains(Keys.V))
                    oldKeys.Add(Keys.V);
            }

            if (KeyboardHandler.PressedOnce(Keys.W))
            {
                if (!oldKeys.Contains(Keys.W))
                    oldKeys.Add(Keys.W);
            }

            if (KeyboardHandler.PressedOnce(Keys.X))
            {
                if (!oldKeys.Contains(Keys.X))
                    oldKeys.Add(Keys.X);
            }

            if (KeyboardHandler.PressedOnce(Keys.Y))
            {
                if (!oldKeys.Contains(Keys.Y))
                    oldKeys.Add(Keys.Y);
            }

            if (KeyboardHandler.PressedOnce(Keys.Z))
            {
                if (!oldKeys.Contains(Keys.Z))
                    oldKeys.Add(Keys.Z);
            }
            #endregion

            if (KeyboardHandler.PressedOnce(Keys.Space))
            {
                tempString = tempString.Insert(tempString.Length, " ");
            }

            if (KeyboardHandler.PressedOnce(Keys.Back))
            {
                if(tempString.Length != 0)
                tempString = tempString.Remove(tempString.Length - 1);
            }

            foreach (var letter in oldKeys)
            {
                if (keyBoardState.IsKeyUp(letter))
                {
                    tempString = tempString.Insert(tempString.Length, letter.ToString());
                    removeList.Add(letter);
                }
            }

            foreach(var letter in removeList)
            {
                oldKeys.Remove(letter);
            }
            removeList.Clear();

            tempString = tempString.ToLower();
        }
    }
}
