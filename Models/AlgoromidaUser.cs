using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Algoromida_01.Models
{
    public class AlgoromidaUser : IdentityUser
    {
        [Required]
        [PersonalData]
        public string FirstName { get; set; }

        [Required]
        [PersonalData]
        public string LastName { get; set; }

        [PersonalData]
        public string Gender { get; set; }

        [PersonalData]
        public DateTime DOB { get; set; }

        [PersonalData]
        public String Location { get; set; }
    }
}
