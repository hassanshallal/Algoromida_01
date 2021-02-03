using System;
using System.ComponentModel.DataAnnotations;

namespace Algoromida_01.Models
{
    public class Contact
    {
	[Key]
        public Guid Id { get; set; }
	
        [Required(ErrorMessage = "Please provide your full name.")]
        [DataType(DataType.Text)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please provide your email."), EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please provide a message.")]
        [DataType(DataType.Text)]
        public string Message { get; set; }
    }
}

