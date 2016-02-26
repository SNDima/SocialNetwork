using System.Data.Entity;
using SocialNetwork.Models;
using SocialNetwork.Repositories.Interfaces;

namespace SocialNetwork.Repositories.Implementations
{
    public class EmailRepository 
        : Repository<AdditionalEmail, int>, IEmailRepository
    {
        public EmailRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}