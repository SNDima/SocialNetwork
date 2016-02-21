using System.Collections.Generic;
using SocialNetwork.Models;

namespace SocialNetwork.Repositories.Interfaces
{
    public interface IURLRepository : IRepository<URL, long>
    {
        void CreateURLs(IEnumerable<string> urls, long resourceId);
    }
}
