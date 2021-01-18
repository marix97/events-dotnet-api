using System.Collections.Generic;

namespace EventAPI.Entities
{
    public class Guest : BaseEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<EventGuest> Events { get; set; }
    }
}
