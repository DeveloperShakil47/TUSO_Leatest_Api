using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUSO.Utilities.Constants
{
    public static class Enums
    {
        /// <summary>
        /// This enum is used to determine the location  of the facility/client.
        /// </summary>
        public enum Location : byte
        {
            Urban = 1,

            Rural = 2
        }

        /// <summary>
        /// This enum is used to determine the facility type of the client.
        /// </summary>
        public enum FacilityType : byte
        {
            [Display(Name = "1st Level Hospital")]
            FirstLevelHospital = 1,

            [Display(Name = "2nd Level Hospital")]
            SecondLevelHospital = 2,

            [Display(Name = "3rd Level Hospital")]
            ThirdLevelHospital = 3,

            [Display(Name = "Dental Clinic")]
            DentalClinic = 4,

            [Display(Name = "Diagnostic Centre")]
            DiagnosticCentre = 5,

            [Display(Name = "Eye Clinic")]
            EyeClinic = 6,

            [Display(Name = "Fertility Clinic")]
            FertilityClinic = 7,

            [Display(Name = "First-Aid Stations")]
            FirstAidStations = 8,

            [Display(Name = "General Clinic")]
            GeneralClinic = 9,

            [Display(Name = "Health Centre")]
            HealthCentre = 10,

            [Display(Name = "Health Post")]
            HealthPost = 11,

            Hospice = 12,

            [Display(Name = "Mini Hospital")]
            MiniHospital = 13,

            [Display(Name = "Mobile Clinic")]
            MobileClinic = 14,

            [Display(Name = "Optic Clinics")]
            OpticClinics = 15,

            [Display(Name = "Rehabilitation Centre")]
            RehabilitationCentre = 16,

            [Display(Name = "Specimen Collection Centre")]
            SpecimenCollectionCentre = 17,

            Others = 18
        }

        /// <summary>
        /// This enum is used to determine the facility type of the client.
        /// </summary>
        public enum Ownership : byte
        {
            [Display(Name = "Faith Based")]
            FaithBased = 1,

            [Display(Name = "Ministry Of Health")]
            MinistryOfHealth = 2,

            [Display(Name = "Correctional Service")]
            CorrectionalService = 3,

            Private = 4,

            [Display(Name = "Zambia National Service")]
            ZambiaNationalService = 4,

            [Display(Name = "Zambia Defence Force")]
            ZambiaDefenceForce = 4,

            [Display(Name = "Police Service")]
            PoliceService = 4,
        }
    }
}
