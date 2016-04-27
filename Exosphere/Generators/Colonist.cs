using Exosphere.Src.Basebuilding;
using Exosphere.Src.Basebuilding.Facilities;
using Exosphere.Src.HUD;
using Exosphere.Src.ResearchProject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Exosphere.Src.Generators
{
    public class Colonist
    {
        #region Variables(Done)

        #region Stats(Done)

        //Represents the colonist's physical strength with a number
        public int strength;
        //Represents the colonist's intelligence with a number
        public int intelligence;
        //Represents the colonist's immunity against unknown bacterias
        public int immuneSystem;
        //Represents the colonist's efficency
        public int efficiency;
        //Represents the colonist's health presented as a percentage-value
        public int health;
        //Represents the normal health value of a healthy colonist
        int normalHealth;

        //Represents the amount of food a civilian needs to eat to survive
        int foodConsumption;
        //Represents the amount of food a civilian needs to eat to survive
        int waterConsumption;

        //Represents the colonist's luck
        public int luck;

        //Represents the colonist's name
        public string name = "";
        //Represents the gender
        public string gender;

        #endregion

        #region Evaluation(Done)

        //A bool that decides if SOME of the colonist's stats will be shown
        public bool levelOneEvaluated;

        //A bool that decides if MOST of the colonist's stats will be shown
        public bool levelTwoEvaluated;

        //A bool that decides if ALL of the colonist's stats will be shown
        public bool levelThreeEvaluated;

        //Represents the amount of days it takes to perform a complete evaluation of the colonist
        int evaluationTime;

        //Contains the amount of days the colonist have been under evaluation
        public int daysEvaluated;

        #endregion

        #region StatTraining(Done)

        #region Training(Done)

        //Represents how much you increase your intelligence has increased
        public float intelligenceTraining;
        //Represents how much you increase your strength has increased
        public float strengthTraining;

        #endregion

        #region TrainingSpeed(Done)

        //Represents the training speed
        float trainingSpeed;

        //Represents your inborn ability to build muscles
        float genetics;
        //Represents your inborn ability to store information
        float learningSpeed;

        //Represents the average colonist's ability to build muscles
        float normalGenetics;
        //Represents the average colonist's ability to store information
        float normalLearningSpeed;

        #endregion

        #endregion

        #region Diseased(Done)

        //Tells if colonist is diseased or not
        public bool diseased;

        //Contains the colonists current disease
        Disease disease; //Save info 

        //The vaccines the colonist have been injected with
        List<Vaccine> vaccines; //Save info

        //Contains the name of the disease
        public string diseaseName;

        #endregion

        #region Work & home(Done)

        //Checks if the collonist have a current profession
        public bool occupied;

        //The facility the colonist currently works in
        public Facility workPlace;

        //The home colony of the colonist
        Colony home;
        #endregion

        #region Proficiency(Done)

        //A dictionary containing the colonist's skill within different occupations
        SerializableDictionary<string, float> proficiency;
        //Represents the time it takes to increase your proficiency within a certain profession
        SerializableDictionary<string, float> proficiencyValue;
        //Represents the colonists starting proficiency
        float proficiencyStartValue;

        #endregion

        #endregion

        //Save variables
        public int colonyID;
        public int facilityID;
        public ColonistSave save;

        #region Load Colonist

        public void LoadColonist(ColonistSave load, Colony home)
        {
            strength = load.strength;
            intelligence = load.intelligence;
            immuneSystem = load.immuneSystem;
            efficiency = load.efficiency;
            health = load.health;
            luck = load.luck;
            name = load.name;
            gender = load.gender;
            levelOneEvaluated = load.levelOneEvaluated;
            levelTwoEvaluated = load.levelTwoEvaluated;
            levelThreeEvaluated = load.levelThreeEvaluated;
            daysEvaluated = load.daysEvaluated;
            intelligenceTraining = load.intelligenceTraining;
            strengthTraining = load.strengthTraining;
            diseased = load.diseased;
            diseaseName = load.diseaseName;
            occupied = load.occupied;
            facilityID = load.facilityID;
            proficiency = load.proficiency;
            proficiencyValue = load.proficiencyValue;

            if (diseased)
                disease.LoadDisease(load.diseaseSave);

            for (int i = 0; i < vaccines.Count; i++)
            {
                if (vaccines[i] == null)
                    vaccines.Add(new Vaccine(new Diseases(0, 0, new Disease())));

                vaccines[i].LoadVaccine(load.vaccineSaves[i]);
            }

            this.home = home;
        }

        #endregion

        public void SaveColonist()
        {
            save.vaccineSaves = new List<VaccineSave>();

            save.strength = strength;
            save.intelligence = intelligence;
            save.immuneSystem = immuneSystem;
            save.efficiency = efficiency;
            save.health = health;
            save.luck = luck;
            save.name = name;
            save.gender = gender;
            save.levelOneEvaluated = levelOneEvaluated;
            save.levelTwoEvaluated = levelTwoEvaluated;
            save.levelThreeEvaluated = levelThreeEvaluated;
            save.daysEvaluated = daysEvaluated;
            save.intelligenceTraining = intelligenceTraining;
            save.strengthTraining = strengthTraining;
            save.diseased = diseased;
            save.diseaseName = diseaseName;
            save.occupied = occupied;
            save.colonyID = colonyID;
            save.facilityID = facilityID;
            save.proficiency = proficiency;
            save.proficiencyValue = proficiencyValue;

            if (disease != null)
            {
                disease.SaveDisease();
                save.diseaseSave = disease.save;
            }

            for (int i = 0; i < vaccines.Count; i++)
            {
                vaccines[i].SaveVaccine();
                save.vaccineSaves.Add(vaccines[i].save);
            }
        }

        #region Generation(Done)


        public Colonist(Colony home, int colonyID)
        {

            this.home = home;
            save = new ColonistSave();
            this.colonyID = colonyID;

            #region Status-Generation
            //Randomizes a value between 5 and 15 to represent intelligence
            intelligence = Settings.RANDOM.Next(5, 16);
            //Randomizes a value between 5 and 15 to represent strength
            strength = Settings.RANDOM.Next(5, 16);
            //Randomizes a value between 20 and 100 to represent the colonist's immunity to diseases
            immuneSystem = Settings.RANDOM.Next(20, 101);
            //Randomizes a value between 1 and 100 to represent the colonist's efficency
            efficiency = Settings.RANDOM.Next(20, 101);

            luck = Settings.RANDOM.Next(-20, 26);

            //Sets a value that represent the colonist's ability to build muscles
            genetics = strength * 5 + luck;
            //Sets a value that represent the colonist's learning speed
            learningSpeed = intelligence * 5 + luck;
            #endregion

            #region Specified stats

            #region Stats
            foodConsumption = strength / 5;
            waterConsumption = strength / 5;
            health = 100;
            normalHealth = 75;
            #endregion

            #region Training
            intelligenceTraining = 0;
            strengthTraining = 0;
            normalGenetics = 50;
            normalLearningSpeed = 50;
            trainingSpeed = 0.6f;
            #endregion

            #region Evaluation
            levelOneEvaluated = false;
            levelTwoEvaluated = false;
            levelThreeEvaluated = false;
            evaluationTime = 7;
            daysEvaluated = 0;
            #endregion

            #endregion

            #region Diseases

            vaccines = new List<Vaccine>();

            #endregion

            #region Gender-Generaration
            int randomGeneration = Settings.RANDOM.Next(1, 3);
            switch (randomGeneration)
            {
                case 1:
                    gender = "Female";
                    break;
                case 2:
                    gender = "Male";
                    break;
                default:
                    break;
            }
            #endregion

            #region Proficiency

            proficiency = new SerializableDictionary<string, float>();
            proficiencyValue = new SerializableDictionary<string, float>();
            proficiencyStartValue = 0.25f;

            #endregion

            GenerateName();
        }

        /// <summary>
        /// Generates a name for the colonist
        /// </summary>
        private void GenerateName()
        {
            int randomGeneration;

            #region Forename-Generation
            randomGeneration = Settings.RANDOM.Next(1, 31);
            switch (randomGeneration)
            {
                case 1:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Mary ");
                    }
                    else
                        name = name.Insert(name.Length, "James ");
                    break;
                case 2:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Patricia ");
                    }
                    else
                        name = name.Insert(name.Length, "John ");
                    break;
                case 3:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Linda ");
                    }
                    else
                        name = name.Insert(name.Length, "Robert ");
                    break;
                case 4:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Barbara ");
                    }
                    else
                        name = name.Insert(name.Length, "Michael ");
                    break;
                case 5:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Elizabeth ");
                    }
                    else
                        name = name.Insert(name.Length, "William ");
                    break;
                case 6:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Jennifer ");
                    }
                    else
                        name = name.Insert(name.Length, "David ");
                    break;
                case 7:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Maria ");
                    }
                    else
                        name = name.Insert(name.Length, "Richard ");
                    break;
                case 8:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Susan ");
                    }
                    else
                        name = name.Insert(name.Length, "Charles ");
                    break;
                case 9:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Lisa ");
                    }
                    else
                        name = name.Insert(name.Length, "Joseph ");
                    break;
                case 10:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Namcy ");
                    }
                    else
                        name = name.Insert(name.Length, "Christopher ");
                    break;
                case 11:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Karen ");
                    }
                    else
                        name = name.Insert(name.Length, "Daniel ");
                    break;
                case 12:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Carol ");
                    }
                    else
                        name = name.Insert(name.Length, "Paul ");
                    break;
                case 13:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Sharon ");
                    }
                    else
                        name = name.Insert(name.Length, "Mark ");
                    break;
                case 14:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Michelle ");
                    }
                    else
                        name = name.Insert(name.Length, "Donald ");
                    break;
                case 15:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Jessica ");
                    }
                    else
                        name = name.Insert(name.Length, "George ");
                    break;
                case 16:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Amy ");
                    }
                    else
                        name = name.Insert(name.Length, "Kenneth ");
                    break;
                case 17:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Anna ");
                    }
                    else
                        name = name.Insert(name.Length, "Steven ");
                    break;
                case 18:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Katherine ");
                    }
                    else
                        name = name.Insert(name.Length, "Edward ");
                    break;
                case 19:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Ashley ");
                    }
                    else
                        name = name.Insert(name.Length, "Brian ");
                    break;
                case 20:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Irene ");
                    }
                    else
                        name = name.Insert(name.Length, "Gary ");
                    break;
                case 21:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Jane ");
                    }
                    else
                        name = name.Insert(name.Length, "Matthew ");
                    break;
                case 22:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Lori ");
                    }
                    else
                        name = name.Insert(name.Length, "Jeffery ");
                    break;
                case 23:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Rachel ");
                    }
                    else
                        name = name.Insert(name.Length, "Eric ");
                    break;
                case 24:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Andrea ");
                    }
                    else
                        name = name.Insert(name.Length, "Stephen ");
                    break;
                case 25:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Louise ");
                    }
                    else
                        name = name.Insert(name.Length, "Raymond ");
                    break;
                case 26:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Ellen ");
                    }
                    else
                        name = name.Insert(name.Length, "Dennis ");
                    break;
                case 27:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Zoe ");
                    }
                    else
                        name = name.Insert(name.Length, "Simon ");
                    break;
                case 28:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Carrie ");
                    }
                    else
                        name = name.Insert(name.Length, "Peter ");
                    break;
                case 29:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Elaine ");
                    }
                    else
                        name = name.Insert(name.Length, "Patrick ");
                    break;
                case 30:
                    if (gender == "Female")
                    {
                        name = name.Insert(name.Length, "Ida ");
                    }
                    else
                        name = name.Insert(name.Length, "Douglas ");
                    break;
                default:
                    break;
            }
            #endregion

            #region Surname-Generation
            randomGeneration = Settings.RANDOM.Next(1, 32);
            switch (randomGeneration)
            {
                case 1:
                    name = name.Insert(name.Length, "Smith");
                    break;
                case 2:
                    name = name.Insert(name.Length, "Johnson");
                    break;
                case 3:
                    name = name.Insert(name.Length, "Williams");
                    break;
                case 4:
                    name = name.Insert(name.Length, "Jones");
                    break;
                case 5:
                    name = name.Insert(name.Length, "Brown");
                    break;
                case 6:
                    name = name.Insert(name.Length, "Davis");
                    break;
                case 7:
                    name = name.Insert(name.Length, "Miller");
                    break;
                case 8:
                    name = name.Insert(name.Length, "Wilson");
                    break;
                case 9:
                    name = name.Insert(name.Length, "Taylor");
                    break;
                case 10:
                    name = name.Insert(name.Length, "White");
                    break;
                case 11:
                    name = name.Insert(name.Length, "Harris");
                    break;
                case 12:
                    name = name.Insert(name.Length, "Thompson");
                    break;
                case 13:
                    name = name.Insert(name.Length, "Lewis");
                    break;
                case 14:
                    name = name.Insert(name.Length, "West");
                    break;
                case 15:
                    name = name.Insert(name.Length, "Fisher");
                    break;
                case 16:
                    name = name.Insert(name.Length, "Reynolds");
                    break;
                case 17:
                    name = name.Insert(name.Length, "Owens");
                    break;
                case 18:
                    name = name.Insert(name.Length, "Burns");
                    break;
                case 19:
                    name = name.Insert(name.Length, "Peters");
                    break;
                case 20:
                    name = name.Insert(name.Length, "Austin");
                    break;
                case 21:
                    name = name.Insert(name.Length, "Lawson");
                    break;
                case 22:
                    name = name.Insert(name.Length, "Jensen");
                    break;
                case 23:
                    name = name.Insert(name.Length, "Davidson");
                    break;
                case 24:
                    name = name.Insert(name.Length, "Reeves");
                    break;
                case 25:
                    name = name.Insert(name.Length, "Robbins");
                    break;
                case 26:
                    name = name.Insert(name.Length, "Higgins");
                    break;
                case 27:
                    name = name.Insert(name.Length, "Reese");
                    break;
                case 28:
                    name = name.Insert(name.Length, "Pratt");
                    break;
                case 29:
                    name = name.Insert(name.Length, "Larsen");
                    break;
                case 30:
                    name = name.Insert(name.Length, "Petersen");
                    break;
                case 31:
                    name = name.Insert(name.Length, "Shepard");
                    break;
                default:
                    break;
            }
            #endregion
        }

        #endregion

        #region Update(Done)

        /// <summary>
        /// Updates the colonist
        /// </summary>
        /// <param name="colonyDisease">A disease that resides in the colony</param>
        /// <param name="canResearch">A list containing which research projects the colony can take on</param>
        public void Update(Disease colonyDisease, List<Research> canResearch)
        {

            home = Core.GetColonies()[0];
            waterConsumption = strength / 5;
            foodConsumption = strength / 5;

            //Sets a value that represent the colonist's ability to build muscles
            genetics = strength * 5 + luck;
            //Sets a value that represent the colonist's learning speed
            learningSpeed = intelligence * 5 + luck;

            Infect(colonyDisease, canResearch);
            if (disease != null)
            {
                diseased = true;
                diseaseName = disease.GetName();
            }

            Cure();

            Affect();
        }

        #endregion

        #region Consumption(Done)

        /// <summary>
        /// Gets the colonist's food consumption
        /// </summary>
        /// <returns>Returns food consumption</returns>
        public int GetFoodConsumption()
        {
            return foodConsumption;
        }

        /// <summary>
        /// Gets the colonist's water consumption
        /// </summary>
        /// <returns>Returns water consumption</returns>
        public int GetWaterConsumption()
        {
            return waterConsumption;
        }

        #endregion

        #region Employ/Unemploy(Done)

        /// <summary>
        /// Sets the colonist to work
        /// </summary>
        public void SetToWork()
        {
            occupied = true;
        }

        /// <summary>
        /// Removes the colonist from work
        /// </summary>
        public void RemoveFromWork()
        {
            occupied = false;
        }

        #endregion

        #region Training(Done)

        /// <summary>
        /// Used to calculate your stat-increase within intelligence
        /// </summary>
        public void IntelligenceTraining()
        {
            intelligenceTraining += (float)(intelligence * Math.Pow(trainingSpeed, intelligence) * GetLearningSpeed());
            if (intelligenceTraining >= 1)
            {
                intelligence++;
                intelligenceTraining--;
            }
        }

        /// <summary>
        /// Used to calculate your stat-increase within strength
        /// </summary>
        public void StrengthTraining()
        {
            strengthTraining += (float)(strength * Math.Pow(trainingSpeed, strength) * GetGenetics());
            if (strengthTraining >= 1)
            {
                strength++;
                strengthTraining--;
            }
        }

        /// <summary>
        /// Calculates how much lesser or greater the colonist's learning speed is compared to the average colonist
        /// </summary>
        /// <returns>Returns the difference in learning speed</returns>
        private float GetLearningSpeed()
        {
            return learningSpeed / normalLearningSpeed;
        }

        /// <summary>
        /// Calculates how much lesser or greater the colonist's genetics is compared to the average colonist
        /// </summary>
        /// <returns>Returns the difference in genetical structure</returns>
        private float GetGenetics()
        {
            return genetics / normalGenetics;
        }

        #endregion

        #region Disease(Done)

        /// <summary>
        /// Calculates how the disease affects the colonist
        /// </summary>
        public void Affect()
        {
            if (disease != null)
                if (disease.DrainHealth(this))
                {
                    AddVaccine(new Vaccine(disease));
                    disease = null;
                }
        }

        /// <summary>
        /// Checks if the colonist gets infected by a disease in the colony
        /// </summary>
        /// <param name="colonyDisease">A disease that resides in the colony</param>
        /// <param name="canResearch">A list containing which research projects the colony can take on</param>
        public void Infect(Disease colonyDisease, List<Research> canResearch)
        {
            bool foundVaccine = false;
            if (disease == null)
            {
                if (colonyDisease.Infect(this, canResearch))
                {
                    if (vaccines.Count == 0)
                        disease = new Disease(colonyDisease);

                    foreach (var vaccine in vaccines)
                    {
                        if (vaccine.CureDisease(colonyDisease))
                            foundVaccine = true;
                    }

                    if (!foundVaccine)
                        disease = new Disease(colonyDisease);
                }

                if(disease != null)
                {
                    MessageBox mb = new MessageBox(1, name + " have fallen ill with: \n" + disease.GetName());
                    Core.currentMessageBox = mb;
                }
            }
        }

        /// <summary>
        /// Injects the colonist with vaccine
        /// </summary>
        /// <param name="vaccine">The vaccine the colonist is injected with</param>
        public void AddVaccine(Vaccine vaccine)
        {
            if (!vaccines.Contains(vaccine))
                vaccines.Add(vaccine);
        }

        /// <summary>
        /// Cures the disease with a vaccine against it 
        /// </summary>
        private void Cure()
        {
            if (disease != null)
            {
                foreach (var vaccine in vaccines)
                {
                    if (vaccine.CureDisease(disease))
                        disease = null;
                }
            }
        }

        #endregion

        #region Evaluation(Done)

        /// <summary>
        /// Evaluates the the colonist
        /// </summary>
        /// <param name="evaluationEfficiency">Affects the evaluation speed</param>
        /// <param name="evaluation">The evaluationfacility the colonists are being evaluated in</param>
        public void Evaluated(int evaluationEfficiency, Evaluation evaluation)
        {
            // Checks the evaluationfacility's level
            if (evaluation.level < 4)
            {
                //Adds one day to the evaluation process
                daysEvaluated += evaluationEfficiency;

                //Checks if the evaluation process is finished
                if (daysEvaluated >= evaluationTime)
                {
                    //Checks which evaluation level that was performed
                    switch (evaluation.level)
                    {
                        case 1:
                            //Sets the bool that shows some stats if "true" to "true"
                            levelOneEvaluated = true;
                            break;
                        case 2:
                            //Sets the bool that shows most stats if "true" to "true"
                            levelTwoEvaluated = true;
                            break;
                        case 3:
                            //Sets the bool that shows all stats if "true" to "true"
                            levelThreeEvaluated = true;
                            break;
                        default:
                            break;
                    }
                    //Resets daysEvaluated to make sure he doesn't get evaluated every time the turn ends
                    daysEvaluated = 0;
                }
            }
            else
            {
                //Sets the bool that shows all stats if "true" to "true"
                levelThreeEvaluated = true;
            }
        }

        #endregion

        #region Work(Done)

        /// <summary>
        /// Calculates how much greater or lesser the colonist's current health is compared to the average
        /// </summary>
        /// <returns>Returns the efficiency of colonist based on its health</returns>
        public float GetHealthBasedEfficiency()
        {
            float f_health = health;
            float f_normalHealth = normalHealth;
            return f_health / f_normalHealth;
        }

        public Colony GetHome()
        {
            return home;
        }

        #endregion

        #region Proficiency(Done)


        /// <summary>
        /// Gets the value representing the colonist's proficiency
        /// </summary>
        /// <param name="facility">The current location where the colonist works</param>
        /// <returns>Returns the colonist's proficiency</returns>
        public float GetProficiency(Facility facility)
        {

            //Increases proficiency if the colonist already have some skills within his current profession
            if (proficiency.ContainsKey(facility.GetFacilityType()))
            {
                ImproveProficiency(facility);
            }
            //Adds a new proficiency when the colonist first encounters a new challenge in the form of a new profession
            else
            {
                int tempProficiencyValue = Settings.RANDOM.Next(5, 101);
                proficiency.Add(facility.GetFacilityType(), proficiencyStartValue);
                proficiencyValue.Add(facility.GetFacilityType(), (float)((float)(Settings.RANDOM.Next(tempProficiencyValue, tempProficiencyValue * 5)) / 10000));
            }

            return proficiency[facility.GetFacilityType()];
        }

        /// <summary>
        /// Calculates the colonist's proficiency improvement
        /// </summary>
        /// <param name="facility">The current location where the colonist works</param>
        private void ImproveProficiency(Facility facility)
        {
            //Improves the colonist's proficiency within his current profession
            proficiency[facility.GetFacilityType()] += (float)(GetLearningSpeed() *
                Math.Pow(proficiencyValue[facility.GetFacilityType()], proficiency[facility.GetFacilityType()]));
        }

        #endregion
    }

    public struct ColonistSave
    {
        public int strength;

        public int intelligence;

        public int immuneSystem;

        public int efficiency;

        public int health;

        public int luck;

        public string name;

        public string gender;

        public bool levelOneEvaluated;

        public bool levelTwoEvaluated;

        public bool levelThreeEvaluated;

        public int daysEvaluated;

        public float intelligenceTraining;

        public float strengthTraining;

        public bool diseased;

        public int colonyID;

        public int facilityID;

        //Save info about disease and vaccine somehow
        public DiseaseSave diseaseSave;
        public List<VaccineSave> vaccineSaves;

        public string diseaseName;

        public bool occupied;

        //Workplace ID

        public SerializableDictionary<string, float> proficiency;

        public SerializableDictionary<string, float> proficiencyValue;
    }
}

