using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories.RelationRepository;

namespace Web_Social_network_BE.Controller
{
    [Route("v1/api/users")]
    [ApiController]
    public class RelationController : ControllerBase
    {
        private readonly IRelationRepository _relationRepository;
        public RelationController(IRelationRepository relationRepository)
        {
            _relationRepository = relationRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var relation = await _relationRepository.GetAllAsync();
                return Ok(relation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting all relation: {ex.Message}");
            }
        }
        //Lấy ra mối quan hệ của 2 user với nhau
        [HttpGet("{UserId}/relation/{UserTargetId}")]
        public async Task<IActionResult> GetRelationByUserIdAndUserTargetId(string UserId,string UserTargetId)
        {
            try
            {
                var relation = await _relationRepository.GetRelationByUserIdAndUserTargetId(UserId, UserTargetId);
                return Ok(relation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting relation between 2 user: {ex.Message}");
            }
        }
        // Lấy ra toàn bộ mối quan hệ friend của user có id = UserId
        [HttpGet("{UserId}/friends")]
        public async Task<IActionResult> GetFriendById(string UserId)
        {
            var relation = await _relationRepository.GetFriendByUserId(UserId);

            if (relation == null)
            {
                return NotFound($"User with id {UserId} not found");
            }

            return Ok(relation);
        }
        //Lấy ra toàn bộ lời mời kết bạn của user có id = UserId
        [HttpGet("{UserId}/friend-requests")]
        public async Task<IActionResult> GetWaitingById(string UserId)
        {
            var relation = await _relationRepository.GetWaitingUserById(UserId);

            if (relation == null)
            {
                return NotFound($"User with id {UserId} not found");
            }

            return Ok(relation);
        }
        //Lấy ra toàn bộ danh sách gửi lời mời kết bạn của user có id = UserId
        [HttpGet("{UserId}/requests-user")]
        public async Task<IActionResult> GetRequestById(string UserId)
        {
            var relation = await _relationRepository.GetRequestByUserId(UserId);

            if (relation == null)
            {
                return NotFound($"User with id {UserId} not found");
            }

            return Ok(relation);
        }
        //Lấy ra mối quan hệ block của user có id = UserId
        [HttpGet("{UserId}/blocks")]
        public async Task<IActionResult> GetBlockById(string UserId)
        {
            var relation = await _relationRepository.GetBlockByUserId(UserId);

            if (relation == null)
            {
                return NotFound($"User with id {UserId} not found");
            }

            return Ok(relation);
        }
        //Lấy ra mối quan hệ follow của user có id = UserId
        [HttpGet("{UserId}/follows")]
        public async Task<IActionResult> GetFollowById(string UserId)
        {
            var relation = await _relationRepository.GetFollowByUserId(UserId);

            if (relation == null)
            {
                return NotFound($"User with id {UserId} not found");
            }

            return Ok(relation);
        }
        // User có id = UserId gửi lời mời kết bạn đến user có id = UserTargetId
        [HttpPost("{UserId}/friend-request/{UserTargetId}")]
        public async Task<IActionResult> SendFriendRequest(String UserId, String UserTargetId)
        {
            try
            {
                Relation relationUserRequest = new Relation();
                Relation relationUserWaiting = new Relation();
                relationUserRequest.RelationId = Guid.NewGuid().ToString();
                relationUserRequest.TypeRelation = "REQUEST";
                relationUserRequest.UserId = UserId;
                relationUserRequest.UserTargetIduserId = UserTargetId;
                relationUserWaiting.RelationId = Guid.NewGuid().ToString();
                relationUserWaiting.TypeRelation = "WAITING";
                relationUserWaiting.UserId = UserTargetId;
                relationUserWaiting.UserTargetIduserId = UserId;
                await _relationRepository.AddAsync(relationUserRequest);
                await _relationRepository.AddAsync(relationUserWaiting);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while sending friend request : {ex.Message}");
            }
        }
        //User có id = UserId block user có id = UserTargetId
        [HttpPost("{UserId}/block/{UserTargetId}")]  
        public async Task<IActionResult> BlockUser(string UserId, string UserTargetId)
        {
            try
            {
                Relation relationBlock = new Relation();
                relationBlock.RelationId = Guid.NewGuid().ToString();
                relationBlock.TypeRelation = "BLOCK";
                relationBlock.UserId = UserId;
                relationBlock.UserTargetIduserId = UserTargetId;
                await _relationRepository.AddAsync(relationBlock);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while block: {ex.Message}");
            }
        }
        //User có id = UserId Follow User có id = UserTargetId
        [HttpPost("{UserId}/follow/{UserTargetId}")]
        public async Task<IActionResult> FollowUser(string UserId, string UserTargetId)
        {
            try
            {
                Relation relationFollow = new Relation();
                relationFollow.RelationId = Guid.NewGuid().ToString();
                relationFollow.TypeRelation = "FOLLOW";
                relationFollow.UserId = UserId;
                relationFollow.UserTargetIduserId = UserTargetId;
                await _relationRepository.AddAsync(relationFollow);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while follow : {ex.Message}");
            }
        }
        //User có id = UserTargetId chấp nhận lời mời kết bạn của user có id = UserId
        [HttpPut("{UserId}/friend-request/{UserTargetId}/accept")]
        public async Task<IActionResult> AcceptFriendRequest (string UserId, string UserTargetId)
        {
            try
            {
                var relationRequest = _relationRepository.GetRequestByUserIdAndUserTargetId(UserId, UserTargetId);
                var relationWaiting = _relationRepository.GetWaitingByUserIdAndUserTargetId(UserId, UserTargetId);
                relationRequest.TypeRelation = "FRIEND";
                relationWaiting.TypeRelation = "FRIEND";
                await _relationRepository.UpdateAsync(relationRequest);
                await _relationRepository.UpdateAsync(relationWaiting);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //User có id = UserId xóa mối quan hệ bạn bè với user có id = FriendId
        [HttpDelete("{UserId}/friends/{FriendId}")]
        public async Task<IActionResult> UnFriend (string UserId, string FriendId)
        {
            try
            {
                await _relationRepository.DeleteFriendByUserId(UserId, FriendId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while unfriend relation with id : {ex.Message}");
            }
        }
        // User có id = UserTargetId từ chối lời mời kết bạn của user có id = UserId
        [HttpDelete("{UserId}/friend-requests/{UserTargetId}/reject")]
        public async Task<IActionResult> Reject(string UserId,string UserTargetId)
        {
            try
            {
                await _relationRepository.RejectFriendRequestByUserId(UserId,UserTargetId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while reject friend request with id : {ex.Message}");
            }
        }
        //User có id = UserId hủy follow với user có id = UserTargetId
        [HttpDelete("{UserId}/follow/{UserTargetId}")]
        public async Task<IActionResult> UnFollow (string UserId, string UserTargetId)
        {
            try
            {
                await _relationRepository.DeleteFollowByUserId(UserId, UserTargetId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while unfollow with id : {ex.Message}");
            }
        }
        //User có id = UserTargetId hủy block với user có id = UserTargetId
        [HttpDelete("{UserId}/block/{UserTargetId}")]
        public async Task<IActionResult> UnBlock (string UserId, string UserTargetId)
        {
            try
            {
                await _relationRepository.DeleteBlockByUserId(UserId, UserTargetId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while unblock with id : {ex.Message}");
            }
        }
    }
}
