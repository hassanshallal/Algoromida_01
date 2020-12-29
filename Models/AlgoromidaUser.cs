using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
// https://www.youtube.com/watch?v=otdYARZQ_0I
// https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-5.0

// We need to give selections for the gender, calendar for DOB, and get country
// from a list of countries, upload a profile picture 
namespace Algoromida_01.Models
{
    public class AlgoromidaUser : IdentityUser
    {
        [PersonalData]
        [Required(ErrorMessage = "Please provide you first name.")]
        public string FirstName { get; set; }

        [PersonalData]
        [Required(ErrorMessage = "Please provide you last name.")]
        public string LastName { get; set; }

        [PersonalData]
        public string Gender { get; set; }

        [PersonalData]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DOB { get; set; }

        [PersonalData]
        public string TimeZone { get; set; }

        [PersonalData]
        public string Country { get; set; }

        [PersonalData]
        public string State { get; set; }

        [PersonalData]
        [Required(ErrorMessage = "Please upload an image that does not exceed 2MB.")]
        public string AvatarPath { get; set; }
    }
}
