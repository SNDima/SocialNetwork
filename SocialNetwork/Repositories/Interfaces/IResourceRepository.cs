using SocialNetwork.Models;

namespace SocialNetwork.Repositories.Interfaces
{
    public interface IResourceRepository : IRepository<Resource, long>
    {
        void IncreaseViewsNumber(Resource resource);
    }
}
