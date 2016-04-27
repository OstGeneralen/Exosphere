using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Handlers
{
    static class TimeHandler
    {

        public static TimeHandlerSave save;

        //Variables for day, month and year
        static int day;
        static int month;
        static int year;

        //Variables for how many days there is per month and how many months per year
        static int daysPerMonth;
        static int monthsPerYear;

        //A variable containing the amount of days since last turn
        public static int passedDays;

        //A bool telling if it is a new turn or not
        public static bool newTurn;

        public static void SaveTime()
        {
            save.day = day;
            save.month = month;
            save.year = year;
        }

        public static void CreateTime(TimeHandlerSave load)
        {
            day = load.day;
            month = load.month;
            year = load.year;

            daysPerMonth = 30;
            monthsPerYear = 12;
        }

        /// <summary>
        /// Let there be time
        /// </summary>
        public static void CreateTime()
        {

            day = 1;
            month = 1;
            year = 2050;



            daysPerMonth = 30;
            monthsPerYear = 12;

        }

        /// <summary>
        /// Starts a new turn
        /// </summary>
        public static void StartTurn()
        {
            newTurn = true;
        }

        /// <summary>
        /// Allows mid turn actions
        /// </summary>
        public static void MidTurn()
        {

            newTurn = false;
            //Run all the code that should run each turn



            //What should be done each turn??

        }

        /// <summary>
        /// Ends the turn
        /// </summary>
        /// <param name="daysToPass">The amount of days to pass before starting the next turn</param>
        public static void EndTurn(int daysToPass)
        {
            //Set passed days to equal the amount of days the time handler should pass before starting a new turn
            passedDays = daysToPass;

            day += daysToPass;

            //If day exceeds days per month
            if (day > daysPerMonth)
            {
                //Subtract days per month from day
                day -= daysPerMonth;

                //Add one to month
                month++;
            }

            //If month exceeds months per year
            if (month > monthsPerYear)
            {
                //Subtract months per year from months
                month -= monthsPerYear;

                //Add one to year
                year++;
            }

            //Start the next turn
            StartTurn();


        }

        /// <summary>
        /// Gets the date
        /// </summary>
        /// <returns>Returns a string with the current date</returns>
        public static string GetDate()
        {
            string date = "";

            date = date.Insert(date.Length, day.ToString());

            switch (month)
            {
                case 1:
                    date = date.Insert(date.Length, " Jan ");
                    break;
                case 2:
                    date = date.Insert(date.Length, " Feb ");
                    break;
                case 3:
                    date = date.Insert(date.Length, " Mar ");
                    break;
                case 4:
                    date = date.Insert(date.Length, " Apr ");
                    break;
                case 5:
                    date = date.Insert(date.Length, " May ");
                    break;
                case 6:
                    date = date.Insert(date.Length, " Jun ");
                    break;
                case 7:
                    date = date.Insert(date.Length, " Jul ");
                    break;
                case 8:
                    date = date.Insert(date.Length, " Aug ");
                    break;
                case 9:
                    date = date.Insert(date.Length, " Sep ");
                    break;
                case 10:
                    date = date.Insert(date.Length, " Oct ");
                    break;
                case 11:
                    date = date.Insert(date.Length, " Nov ");
                    break;
                case 12:
                    date = date.Insert(date.Length, " Dec ");
                    break;
                default:
                    date.Insert(date.Length, " Decnovjanfebmaroct ");
                    break;
            }

            date = date.Insert(date.Length, year.ToString());

            return date;
        }




        //Den gamla TimeHandlern är död. MUUUHAHAHAHAHAHAHAHAHAHAH  :D
    }

    public struct TimeHandlerSave
    {
        public int day;
        public int month;
        public int year;
    }
}
