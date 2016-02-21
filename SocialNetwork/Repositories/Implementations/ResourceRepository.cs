using SocialNetwork.Models;
using SocialNetwork.Repositories.Interfaces;

namespace SocialNetwork.Repositories.Implementations
{
    public class ResourceRepository 
        : Repository<Resource, long>, IResourceRepository
    {
        public ResourceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void IncreaseViewsNumber(Resource resource)
        {
            resource.ViewsNumber++;
            Context.SaveChanges();
        }
    }
}