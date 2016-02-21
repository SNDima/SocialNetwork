using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SocialNetwork.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<UserToConversationLink> UserConversations { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<URL> URLs { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}