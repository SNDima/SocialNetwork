﻿using System;
using SocialNetwork.Repositories.Interfaces;

namespace SocialNetwork.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IConversationRepository Conversations { get; }
        IMessageRepository Messages { get; }
        IUserRepository Users { get; }
        ILinkRepository Links { get; }
        IResourceRepository Resources { get; }
        IURLRepository URLs { get; }
        IFileRepository Files { get; }
        IPhoneNumberRepository PhoneNumbers { get; }
        IEmailRepository Emails { get; }
        ISkypeRepository Skypes { get; }
        int Complete();
    }
}
