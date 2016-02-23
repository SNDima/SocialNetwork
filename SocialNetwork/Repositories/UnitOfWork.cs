using SocialNetwork.Models;
using SocialNetwork.Repositories.Implementations;
using SocialNetwork.Repositories.Interfaces;

namespace SocialNetwork.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            Conversations = new ConversationRepository(this.context);
            Messages = new MessageRepository(this.context);
            Users = new UserRepository(this.context);
            Links = new LinkRepository(this.context);
            Resources = new ResourceRepository(this.context);
            URLs = new URLRepository(this.context);
            Folders = new FolderRepository(this.context);
        }

        public IConversationRepository Conversations { get; private set; }
        public IMessageRepository Messages { get; private set; }
        public IUserRepository Users { get; private set; }
        public ILinkRepository Links { get; private set; }
        public IResourceRepository Resources { get; private set; }
        public IURLRepository URLs { get; private set; }
        public IFolderRepository Folders { get; private set; }

        public int Complete()
        {
            return context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}