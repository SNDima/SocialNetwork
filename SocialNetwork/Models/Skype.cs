using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Models
{
    public class Skype
    {
        public int id { get; set; }

        [StringLength(256, ErrorMessage
            = "The Skype value cannot exceed 256 characters.")]
        public string Value { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}