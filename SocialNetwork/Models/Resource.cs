using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Models
{
    public class Resource
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PostingTime { get; set; }
        public int ViewsNumber { get; set; }

        public virtual List<URL> URLs { get; set; }

        [ForeignKey("Owner")]
        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }

        public virtual FilesStorageFolder Folder { get; set; }
    }
}