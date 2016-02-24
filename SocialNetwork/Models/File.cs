using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Models
{
    public class File
    {
        public long Id { get; set; }
        public string Path { get; set; }

        [ForeignKey("Resource")]
        public long? ResourceId { get; set; }
        public virtual Resource Resource { get; set; }
    }
}