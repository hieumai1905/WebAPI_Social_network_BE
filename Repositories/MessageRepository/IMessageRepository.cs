using System.Collections;
using Web_Social_network_BE.Models;

namespace Web_Social_network_BE.Repositories.MessageRepository
{
    public interface IMessageRepository:IGeneralRepository<Message,string>
    {
        Task<IEnumerable> GetMessageInConversationByUserId(string userId, string userTargetId);
        Task<Message> GetMessageByMessageId(int messageId);
        Task <Conversation> GetConversation(string userId, string userTargetId); 
        Task <bool> CheckConversation (string userId, string userTargetId);
        Task<Conversation> AddConversation(Conversation conversation);
    }
}
