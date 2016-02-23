using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Models
{
    public class FilesStorageFolder
    {
        [Key, ForeignKey("Resource")]
        public long ResourceId { get; set; }

        public string Path { get; set; }

        public virtual Resource Resource { get; set; }
    }
}