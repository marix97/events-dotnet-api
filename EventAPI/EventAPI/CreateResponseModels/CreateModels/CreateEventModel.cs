using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventAPI.CreateResponseModels.CreateModels
{
    public class CreateEventModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(48, MinimumLength = 3,
        ErrorMessage = "Event name should be minimum 3 characters and a maximum of 48 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(25, MinimumLength = 3,
        ErrorMessage = "Host should be minimum 3 characters and a maximum of 25 characters")]
        public string Host { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public DateTime EndDate { get; set; }

        public ICollection<CreateGuestModel> Guests { get; set; }
    }
}
