using SocialNetwork.Models;
using SocialNetwork.Repositories.Interfaces;

namespace SocialNetwork.Repositories.Implementations
{
    public class FileRepository :
        Repository<File, long>, IFileRepository
    {
        public FileRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}