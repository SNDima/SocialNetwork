using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Models
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsPublic { get; set; }

        public virtual List<UserToConversationLink> Links { get; set; }

        public virtual List<Message> Messages { get; set; }
    }
}