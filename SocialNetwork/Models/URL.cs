using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Models
{
    public class URL
    {
        public long Id { get; set; }
        public string Content { get; set; }

        [ForeignKey("Resource")]
        public long ResourceId { get; set; }
        public virtual Resource Resource { get; set; }
    }
}