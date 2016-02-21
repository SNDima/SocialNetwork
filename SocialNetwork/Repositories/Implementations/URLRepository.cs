using System.Collections.Generic;
using System.Data.Entity;
using System.Web.WebPages;
using SocialNetwork.Models;
using SocialNetwork.Repositories.Interfaces;

namespace SocialNetwork.Repositories.Implementations
{
    public class URLRepository : Repository<URL, long>, IURLRepository
    {
        public URLRepository(DbContext context) : base(context)
        {
        }

        public void CreateURLs(IEnumerable<string> urls, long resourceId)
        {
            if (urls == null)
            {
                return;
            }
            List<URL> URLs = new List<URL>();
            foreach (string url in urls)
            {
                if (!url.IsEmpty())
                {
                    URLs.Add(new URL
                    {
                        Content = url,
                        ResourceId = resourceId
                    });
                }
            }
            AddRange(URLs);
            Context.SaveChanges();
        }
    }
}