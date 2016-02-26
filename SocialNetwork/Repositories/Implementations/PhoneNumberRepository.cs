using SocialNetwork.Models;
using SocialNetwork.Repositories.Interfaces;

namespace SocialNetwork.Repositories.Implementations
{
    public class PhoneNumberRepository
        : Repository<AdditionalPhoneNumber, int>, IPhoneNumberRepository
    {
        public PhoneNumberRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}