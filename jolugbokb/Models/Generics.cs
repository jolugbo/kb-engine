using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace jolugbokb.middleware
{
    public class GenericResponseMsg
    {
        public bool a { get; set; }
        public string b { get; set; }
        public bool InUse { get; set; }
        public int DataID { get; set; }
    }

    public class SendOTPData
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string RecipientEmail { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Not Valid, See Validation Summary in Documentation")]
        [StringLength(100, ErrorMessage = "The Address value cannot exceed 100 characters.")]
        public string Subject { get; set; }
        [Required]
        public string Reason { get; set; }
        [Required]
        [RegularExpression(@"^[0-9\s]*$", ErrorMessage = "Not Valid, See Validation Summary in Documentation")]
        [StringLength(6, ErrorMessage = "The Address value cannot exceed 6 characters.")]
        public string Token { get; set; }
        [Required]
        [RegularExpression(@"^((0)[0-9]{10})$", ErrorMessage = "Not Valid, See Validation Summary in Documentation")]
        [StringLength(11, ErrorMessage = "The Phonenumber value cannot exceed 11 characters. ")]
        public string Phonenumber { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Not Valid, See Validation Summary in Documentation")]
        [StringLength(200, ErrorMessage = "The CustomerName value cannot exceed 200 characters. ")]
        public string CustomerName { get; set; }
    }

    public class Contactus
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Not Valid, See Validation Summary in Documentation")]
        [StringLength(200, ErrorMessage = "The Firstname value cannot exceed 200 characters. ")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Not Valid, See Validation Summary in Documentation")]
        [StringLength(200, ErrorMessage = "The LastName value cannot exceed 200 characters. ")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^((0)[0-9]{10})$", ErrorMessage = "Not Valid, See Validation Summary in Documentation")]
        [StringLength(11, ErrorMessage = "The Phonenumber value cannot exceed 11 characters. ")]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Complaints { get; set; }
        public string FeedBackType { get; set; }
        [Required]
        public string Source { get; set; }
        [Required]
        public bool IsLead { get; set; }
    }

    public class OutputMessage
    {
        public string Message { get; set; }
        public dynamic OutputObject { get; set; }
        public IList<dynamic> OutputObjectList { get; set; }
        public int HttpStatusCode { get; set; }
    }

}

