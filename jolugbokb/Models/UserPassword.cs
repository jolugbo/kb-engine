using System;
namespace jolugbokb.Models
{

    public class UserPass
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Pass { get; set; }
        public DateTime DateCreated { get; set; }
        public string RecCreSource { get; set; }
        public bool valid { get; set; }

        public virtual User User { get; set; }
    }
}
