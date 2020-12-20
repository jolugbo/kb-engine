using System;
namespace jolugbokb.Models
{
    public class OTP
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public string Otp { get; set; }
        public string SessionId { get; set; }
        public string Details { get; set; }
        public DateTime RecCreDate { get; set; }
        public string RecCreSource { get; set; }
        public virtual User AppUser { get; set; }
    }
}
