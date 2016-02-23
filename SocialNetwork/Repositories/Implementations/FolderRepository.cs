using SocialNetwork.Models;
using SocialNetwork.Repositories.Interfaces;

namespace SocialNetwork.Repositories.Implementations
{
    public class FolderRepository :
        Repository<FilesStorageFolder, long>, IFolderRepository
    {
        public FolderRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}