using System;
using System.Collections.Generic;
namespace jolugbokb.Models
{
    public partial class SessionKey
    {
        public int Id { get; set; }
        public string Userid { get; set; }
        public string SecKey { get; set; }
        public bool IsValid { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ExpiredOn { get; set; }
    }
}