using SocialNetwork.Models;
using SocialNetwork.Repositories.Interfaces;

namespace SocialNetwork.Repositories.Implementations
{
    public class SkypeRepository : Repository<Skype, int>, ISkypeRepository
    {
        public SkypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}