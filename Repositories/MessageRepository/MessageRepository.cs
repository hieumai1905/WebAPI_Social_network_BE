using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using Web_Social_network_BE.Models;

namespace Web_Social_network_BE.Repositories.MessageRepository
{
    public class MessageRepository:IMessageRepository
    {
        private readonly SocialNetworkN01Ver2Context _context;
        public MessageRepository(SocialNetworkN01Ver2Context context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetMessageInConversationByUserId(string userId, string userTargetId)
        {
            try
            {

                return await (from c in _context.Conversations join m in _context.Messages on c.ConversationId equals m.ConversationId where c.UserId == userId && c.UserTargetId==userTargetId orderby m.SendAt ascending select new { m.ConversationId, c.UserId, c.UserTargetId, m.SendAt, m.Type, m.Content }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting conservation.", ex);
            }
        }
        public Message GetMessageByMessageId(int messageId)
        {
            try
            {
                var message = _context.Messages.AsNoTracking().FirstOrDefault(x=>x.MessageId==messageId);
                return message;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while find message by messageId.", ex);
            }
        }
        public async Task<Conversation> AddConversation(Conversation conversation)
        {
            try
            {
                await _context.Conversations.AddAsync(conversation);
                await _context.SaveChangesAsync();
                return conversation;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while add conservation.", ex);
            }
        }
        public bool CheckConversation(string userId, string userTargetId)
        {
            try
            {
                if (_context.Conversations.Where(x => x.UserId == userId && x.UserTargetId == userTargetId).ToList().Count!=0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while check conservation.", ex);
            }
        }
        public Conversation GetConversation(string userId, string userTargetId)
        {
            try
            {
                var conversation = _context.Conversations.Where(x => (x.UserId == userId && x.UserTargetId == userTargetId) || (x.UserId == userTargetId && x.UserTargetId == userId)).ToList();
                return conversation[0];
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting conservationId.", ex);
            }
        }
        public async Task<Message> AddAsync(Message entity)
        {
            try
            {
                await _context.Messages.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while add message.", ex);
            }
        }
        public async Task UpdateAsync(Message entity)
        {
            try
            {
                var message = await _context.Messages.AsNoTracking().FirstOrDefaultAsync(x => x.MessageId==entity.MessageId);
                if (message == null)
                {
                    throw new ArgumentException("message does not exist");
                }
                _context.Messages.Update(entity);
                _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while update message.", ex);
            }
        }
        public Task DeleteAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Message>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Message> GetByIdAsync(string key)
        {
            throw new NotImplementedException();
        }
    }
}
