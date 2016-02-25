using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.ViewModels.Resource
{
    public class CreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.Url)]
        public List<string> URLs { get; set; }

        public List<string> FilesNames { get; set; }
    }
}