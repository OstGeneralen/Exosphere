using Exosphere.Src.ResearchProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Generators
{
    public class Disease
    {
        #region Variables

        //The resistancy should be checked against the immune system of a colonist. If resisitancy > immune system the disease can affect the colonist
        int resistancy;

        //A value deciding how big the chance is that the disease will spread to its victim
        int spread;

        //An int that identifies the disease
        int identification;

        //Determines how much the disease affects the colonist
        int affect;

        //Determines how long the colonist will be diseased
        int timeDiseased;

        //Determines how long the colonist have been diseased
        float timeUntilCured;

        //A string containing the name of the disease
        string name;

        //The amount of days the disease stays active
        int daysActive;

        //The amount of days the disease have been active
        int daysActivated;

        #endregion

        #region Save/Load

        public DiseaseSave save;

        #region Load Disease

        public void LoadDisease(DiseaseSave load)
        {
            identification = load.identification;
            timeUntilCured = load.timeUntilCured;
            resistancy = load.resistancy;
            spread = load.spread;
            affect = load.affect;
            name = load.name;
            daysActivated = load.daysActivated;
            daysActive = load.daysActive;
        }

        #endregion

        public void SaveDisease()
        {
            save.identification = identification;
            save.timeUntilCured = timeUntilCured;
            save.resistancy = resistancy;
            save.spread = spread;
            save.affect = affect;
            save.name = name;
            save.daysActivated = daysActivated;
            save.daysActive = daysActive;
        }

        #endregion

        /// <summary>
        /// Creates the disease
        /// </summary>
        public Disease()
        {
            Create();
        }

        public Disease(Disease disease)
        {
            this.affect = disease.affect;
            this.daysActivated = disease.daysActivated;
            this.daysActive = disease.daysActive;
            this.identification = disease.identification;
            this.name = disease.name;
            this.resistancy = disease.resistancy;
            this.spread = disease.spread;
            this.timeDiseased = disease.timeDiseased;
            this.timeUntilCured = disease.timeUntilCured;
        }

        /// <summary>
        /// Gets the disease's name
        /// </summary>
        /// <returns>Returns the disease's name</returns>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Gets the disease's ID
        /// </summary>
        /// <returns>Returns the disease's ID</returns>
        public int GetIdentification()
        {
            return identification;
        }

        /// <summary>
        /// Gets the disease's resistancy
        /// </summary>
        /// <returns>Returns the disease's resistancy</returns>
        public int GetResistancy()
        {
            return resistancy;
        }

        /// <summary>
        /// Gets the disease's spread
        /// </summary>
        /// <returns>Returns the disease's spread</returns>
        public int GetSpread()
        {
            return spread;
        }

        /// <summary>
        /// Gets the disease's affect
        /// </summary>
        /// <returns>Returns the disease's affect</returns>
        public int GetAffect()
        {
            return affect;
        }

        /// <summary>
        /// Creates the disease
        /// </summary>
        private void Create()
        {
            resistancy = Settings.RANDOM.Next(1, 100);
            spread = Settings.RANDOM.Next(1, 31);
            identification = Settings.RANDOM.Next(-2000000, 2000001);
            daysActive = Settings.RANDOM.Next(15, 61);

            timeDiseased = resistancy;
            timeUntilCured = 0;
            int weakDiseases = 75;
            int normalDiseases = 90;
            int strongDiseases = 99;

            name = "";

            int random;

            random = Settings.RANDOM.Next(1, 101);

            if (random <= weakDiseases)
            {
                affect = Settings.RANDOM.Next(0, 3);
            }
            else if (random <= normalDiseases)
            {
                affect = Settings.RANDOM.Next(3, 6);
            }
            else if (random <= strongDiseases)
            {
                affect = Settings.RANDOM.Next(6, 16);
            }
            else
            {
                affect = Settings.RANDOM.Next(30, 61);
            }

            #region Generate name

            random = Settings.RANDOM.Next(1, 6);

            switch (random)
            {
                case 1:
                    name = name.Insert(name.Length, "Hyper");
                    break;
                case 2:
                    name = name.Insert(name.Length, "Mal");
                    break;
                case 3:
                    name = name.Insert(name.Length, "Mix");
                    break;
                case 4:
                    name = name.Insert(name.Length, "Anti");
                    break;
                case 5:
                    name = name.Insert(name.Length, "Cas");
                    break;
                default:
                    name = "";
                    break;
            }

            random = Settings.RANDOM.Next(1, 6);

            switch (random)
            {
                case 1:
                    name = name.Insert(name.Length, "condens");
                    break;
                case 2:
                    name = name.Insert(name.Length, "cellular");
                    break;
                case 3:
                    name = name.Insert(name.Length, "fluid");
                    break;
                case 4:
                    name = name.Insert(name.Length, "muscular");
                    break;
                case 5:
                    name = name.Insert(name.Length, "bone");
                    break;
                default:
                    name = "";
                    break;
            }

            random = Settings.RANDOM.Next(1, 6);

            switch (random)
            {
                case 1:
                    name = name.Insert(name.Length, " flu");
                    break;
                case 2:
                    name = name.Insert(name.Length, " bacteria");
                    break;
                case 3:
                    name = name.Insert(name.Length, " virus");
                    break;
                case 4:
                    name = name.Insert(name.Length, " parasite");
                    break;
                case 5:
                    name = name.Insert(name.Length, " cold");
                    break;
                default:
                    name = "";
                    break;
            }

            #endregion
        }

        /// <summary>
        /// Check if a colonist gets infected with the disease
        /// </summary>
        /// <param name="victim">The colonist that should run the check</param>
        /// <returns>Returns true if the disese infects the colonist</returns>
        public bool Infect(Colonist victim, List<Research> canResearch)
        {

            //Only run the check if the victim's immune system is lower than the disease's resistancy
            if (victim.immuneSystem < resistancy)
            {
                //The value to check difference against
                int differenceCheck = 5;

                //Gets the difference 
                int diffrence = resistancy - victim.immuneSystem;

                //Standardly set the risk of infection to 0
                int infectionRisk = 0;

                #region Differnece check
                if (diffrence <= differenceCheck)
                    infectionRisk = Settings.RANDOM.Next(1, 31);


                if (diffrence > differenceCheck && diffrence <= differenceCheck * 5)
                    infectionRisk = Settings.RANDOM.Next(1, 21);


                if (diffrence > differenceCheck * 5 && diffrence <= differenceCheck * 10)
                    infectionRisk = Settings.RANDOM.Next(1, 11);


                if (diffrence > differenceCheck * 10)
                    infectionRisk = Settings.RANDOM.Next(1, 6);
                #endregion

                //Subtracts the spread from infection risk 
                infectionRisk -= spread;

                //Infect the colonist if infectionRisk is lower or equal to 1
                if (infectionRisk <= 1)
                {
                    bool notAdded = false;
                    if (canResearch.Count != 0)
                        foreach (var research in canResearch)
                        {
                            if (research.GetIdentification() != this.identification || research.GetResearchName() != this.name)
                            {
                                notAdded = true;
                                break;
                            }
                        }
                    else
                    {
                        notAdded = true;
                    }
                    //Adds the disease into a list of possible research options
                    if (notAdded)
                        canResearch.Add(new Diseases(victim.colonyID, 0, this));
                    return true;
                }
            }

            //Standardly return false
            return false;
        }

        /// <summary>
        /// Drains the victim's health
        /// </summary>
        /// <param name="victim">The colonist infected by the disease</param>
        public bool DrainHealth(Colonist victim)
        {
            victim.health -= affect;
            if (affect <= 5)
                timeUntilCured += (float)(victim.immuneSystem) / 5;

            if (timeUntilCured > timeDiseased)
            {
                return true;
            }
            return false;
        }

        public bool TimeToRemove()
        {
            daysActivated++;

            if (daysActivated >= daysActive)
            {
                return true;
            }
            return false;
        }
    }

    public struct DiseaseSave
    {
        public int resistancy;
        public int spread;
        public int identification;
        public int affect;
        public int daysActive;
        public int daysActivated;
        public float timeUntilCured;
        public string name;
    }
}
