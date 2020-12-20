using System;
using System.Collections.Generic;

namespace jolugbokb.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public bool UserValidated { get; set; }
        public virtual ICollection<UserPass> Password { get; set; }
        public User()
        {
        }
    }
}
