using Exosphere.Src.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.ResearchProject
{
    public class Vaccine
    {
        //The name of the disease
        public string diseaseName;

        //The disease-ID
        int identification;

        #region Save/Load

        public VaccineSave save;

        #region Load Vaccine

        public void LoadVaccine(VaccineSave saveFile)
        {
            diseaseName = saveFile.diseaseName;
            identification = saveFile.identification;
        }

        #endregion

        public void SaveVaccine()
        {
            save.diseaseName = diseaseName;
            save.identification = identification;
        }

        #endregion

        private Vaccine()
        {

        }

        /// <summary>
        /// Create a newe vaccine
        /// </summary>
        /// <param name="research">The research that leads to the vaccine</param>
        public Vaccine(Research research)
        {
            //Creates the vaccine
            CreateVaccine(research);
        }

        public Vaccine(Disease disease)
        {
            diseaseName = disease.GetName();
            identification = disease.GetIdentification();
        }

        /// <summary>
        /// Checks if there is a vaccine for a disease or not
        /// </summary>
        /// <param name="disease">The disease you want to check if the vaccine is against</param>
        /// <returns>True if the vaccine helps agains the disease</returns>
        public bool CureDisease(Disease disease)
        {
            if (disease == null)
                return true;

            //If the id of the vaccine and the name of the vaccine's disease matches
            if (disease.GetIdentification() == identification && disease.GetName() == diseaseName)
            {
                //Return true
                return true;
            }

           

            //Return false
            return false;
        }

        /// <summary>
        /// Creates a vaccine for a disease
        /// </summary>
        /// <param name="research">The research needed to create the vaccine</param>
        private void CreateVaccine(Research research)
        {
            //Sets the vaccine's disease name to be matching the disease it should cure
            diseaseName = research.GetResearchName();

            //Sets the vaccine's ID to match the disease it should cure
            identification = research.GetIdentification();
        }

        /// <summary>
        /// Returns the disease name
        /// </summary>
        /// <returns>A string with the disease name</returns>
        public string GetDiseaseName()
        {
            return diseaseName;
        }

        /// <summary>
        /// Returns the ID of the vaccine and disease
        /// </summary>
        /// <returns>An int with the ID of the vaccine and disease</returns>
        public int GetIdentification()
        {
            return identification;
        }
    }

    public struct VaccineSave
    {
        public string diseaseName;

        public int identification;
    }
}
