using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories.MessageRepository;

namespace Web_Social_network_BE.Controller
{
    [Route("v2/api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;
        public MessageController(IMessageRepository messageRepository, IHttpContextAccessor httpContextAccessor)
        {
            _messageRepository = messageRepository;
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }
        [HttpGet("{userTargetId}")] 
        public async Task<IActionResult> GetConservationMessage(string userTargetId)
        {
            try
            {
                var userId = _session.GetString("UserId");
                if (await _messageRepository.CheckConversation(userId,userTargetId) == true)
                    return Ok(await _messageRepository.GetMessageInConversationByUserId(userId,userTargetId));
                else
                    return Ok(await _messageRepository.GetMessageInConversationByUserId(userTargetId, userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting conservation: {ex.Message}");
            }
        }
        [HttpPost("send-message/{userTargetId}/{content}")] 
        public async Task<IActionResult> SendMessage(string userTargetId,string content)
        {
            try
            {
                var userId = _session.GetString("UserId");
                if (await _messageRepository.CheckConversation(userId,userTargetId)==false && await _messageRepository.CheckConversation(userTargetId,userId)==false)
                {
                    Conversation conversation = new Conversation();
                    conversation.ConversationId = Guid.NewGuid().ToString();
                    conversation.UserId = userId;
                    conversation.UserTargetId = userTargetId;
                    await _messageRepository.AddConversation(conversation); 
                }
                Message message = new Message(); 
                message.SendAt = DateTime.Now;
                Conversation conversationOfMessage = await _messageRepository.GetConversation(userId, userTargetId);
                message.ConversationId = conversationOfMessage.ConversationId;
                message.Content = content;
                if (conversationOfMessage.UserId == userId)
                    message.Type = "1";
                else
                    message.Type = "2";
                await _messageRepository.AddAsync(message);
                return Ok(message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while send request: {ex.Message}");
            }
        }
        [HttpPut("{messageId}/undo-message")]
        public async Task<IActionResult> UndoMessage(int messageId)
        {
            try
            {
                var message = await _messageRepository.GetMessageByMessageId(messageId);
                message.Content = "Message has been recovered";
                await _messageRepository.UpdateAsync(message);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while undo message: {ex.Message}");
            }
        }
    }
}
