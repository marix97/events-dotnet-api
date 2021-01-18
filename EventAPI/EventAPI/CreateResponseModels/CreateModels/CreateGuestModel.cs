using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.CreateResponseModels.CreateModels
{
    public class CreateGuestModel
    {

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(25, MinimumLength = 3,
        ErrorMessage = "First Name should be minimum 2 characters and a maximum of 25 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(25, MinimumLength = 3,
        ErrorMessage = "Last Name should be minimum 2 characters and a maximum of 25 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
