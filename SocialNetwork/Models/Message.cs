using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Models
{
    public class Message
    {
        [Key]
        public long Id { get; set; }
        public string Content { get; set; }
        public DateTime DepartureTime { get; set; }
        public string SenderName { get; set; }

        [ForeignKey("Conversation")]
        public int ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; }
    }
}