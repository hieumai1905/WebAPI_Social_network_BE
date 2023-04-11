using System.Collections;
using Web_Social_network_BE.Models;

namespace Web_Social_network_BE.Repositories.MessageRepository
{
    public interface IMessageRepository:IGeneralRepository<Message,string>
    {
        Task<IEnumerable> GetMessageInConversationByUserId(string userId, string userTargetId);
        Message GetMessageByMessageId(int messageId);
        Conversation GetConversation(string userId, string userTargetId); 
        bool CheckConversation (string userId, string userTargetId);
        Task<Conversation> AddConversation(Conversation conversation);
    }
}
