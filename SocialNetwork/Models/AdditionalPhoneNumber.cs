using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Models
{
    public class AdditionalPhoneNumber
    {
        public int id { get; set; }

        [StringLength(13, ErrorMessage
            = "The Phone Number value cannot exceed 13 characters. ")]
        public string PhoneNumber { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}