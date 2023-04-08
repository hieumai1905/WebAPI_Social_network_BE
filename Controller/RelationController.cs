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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;
        public RelationController(IRelationRepository relationRepository, IHttpContextAccessor httpContextAccessor)
        {
            _relationRepository = relationRepository;
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }
        [HttpGet("relation")]
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
        [HttpGet("relation/{userTargetId}")]
        public async Task<IActionResult> GetRelationByUserIdAndUserTargetId(string userTargetId)
        {
            try
            {
                var userId = _session.GetString("UserId");
                var relation = await _relationRepository.GetRelationByUserIdAndUserTargetId(userId, userTargetId);
                return Ok(relation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting relation between 2 user: {ex.Message}");
            }
        }
        // Lấy ra toàn bộ mối quan hệ friend của user có id = UserId
        [HttpGet("{userId}/friends")]
        public async Task<IActionResult> GetFriendById(string userId)
        {
            var relation = await _relationRepository.GetFriendByUserId(userId);

            if (relation == null)
            {
                return NotFound($"User with id {userId} not found");
            }

            return Ok(relation);
        }
        //Lấy ra toàn bộ lời mời kết bạn của user có id = UserId
        [HttpGet("friend-requests")]
        public async Task<IActionResult> GetWaitingById()
        {
            var userId = _session.GetString("UserId");
            var relation = await _relationRepository.GetWaitingUserById(userId);

            if (relation == null)
            {
                return NotFound($"User with id {userId} not found");
            }

            return Ok(relation);
        }
        //Lấy ra toàn bộ danh sách gửi lời mời kết bạn của user có id = UserId
        [HttpGet("requests-user")]
        public async Task<IActionResult> GetRequestById()
        {
            var userId = _session.GetString("UserId");
            var relation = await _relationRepository.GetRequestByUserId(userId);

            if (relation == null)
            {
                return NotFound($"User with id {userId} not found");
            }

            return Ok(relation);
        }
        //Lấy ra mối quan hệ block của user có id = UserId
        [HttpGet("blocks")]
        public async Task<IActionResult> GetBlockById()
        {
            var userId = _session.GetString("UserId");
            var relation = await _relationRepository.GetBlockByUserId(userId);

            if (relation == null)
            {
                return NotFound($"User with id {userId} not found");
            }

            return Ok(relation);
        }
        [HttpGet("users-blocks-me")]
        public async Task<IActionResult> GetUserBlockMe()
        {
            var userId = _session.GetString("UserId");
            var relation = await _relationRepository.GetAnyUserBlockMe(userId);

            if (relation == null)
            {
                return NotFound($"User with id {userId} not found");
            }

            return Ok(relation);
        }
        //Lấy ra mối quan hệ follow của user có id = UserId
        [HttpGet("follows")]
        public async Task<IActionResult> GetFollowById()
        {
            var userId = _session.GetString("UserId");
            var relation = await _relationRepository.GetFollowByUserId(userId);

            if (relation == null)
            {
                return NotFound($"User with id {userId} not found");
            }

            return Ok(relation);
        }
        // User có id = UserId gửi lời mời kết bạn đến user có id = UserTargetId
        [HttpPost("friend-request/{userTargetId}")]
        public async Task<IActionResult> SendFriendRequest(string userTargetId)
        {
            try
            {
                var userId = _session.GetString("UserId");
                if (_relationRepository.CheckWaitingRelation(userTargetId, userId) == false && _relationRepository.CheckWaitingRelation(userId, userTargetId) == false)
                {
                    if (_relationRepository.CheckFriendRelation(userId, userTargetId) == false)
                    {
                        if (_relationRepository.CheckBlockRelation(userId, userTargetId) == true)
                        {
                            return StatusCode(500, $"An error occurred while sending friend request, unblock after send friend request");
                        }
                        if (_relationRepository.CheckFollowRelation(userId, userTargetId) == false)
                        {
                            Relation relationFollow = new Relation();
                            relationFollow.RelationId = Guid.NewGuid().ToString();
                            relationFollow.TypeRelation = "FOLLOW";
                            relationFollow.UserId = userId;
                            relationFollow.UserTargetIduserId = userTargetId;
                            await _relationRepository.AddAsync(relationFollow);
                        }
                        Relation relationUserRequest = new Relation();
                        Relation relationUserWaiting = new Relation();
                        relationUserRequest.RelationId = Guid.NewGuid().ToString();
                        relationUserRequest.TypeRelation = "REQUEST";
                        relationUserRequest.UserId = userId;
                        relationUserRequest.UserTargetIduserId = userTargetId;
                        relationUserWaiting.RelationId = Guid.NewGuid().ToString();
                        relationUserWaiting.TypeRelation = "WAITING";
                        relationUserWaiting.UserId = userTargetId;
                        relationUserWaiting.UserTargetIduserId = userId;
                        await _relationRepository.AddAsync(relationUserRequest);
                        await _relationRepository.AddAsync(relationUserWaiting);
                        return Ok();
                    }
                    else
                        return StatusCode(500, $"An error occurred while sending friend request");
                }
                else
                {
                    return StatusCode(500, $"Friend request is exist");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while sending friend request : {ex.Message}");
            }
        }
        //User có id = UserId block user có id = UserTargetId
        [HttpPost("block/{userTargetId}")]
        public async Task<IActionResult> BlockUser(string userTargetId)
        {
            try
            {
                var userId = _session.GetString("UserId");
                if (_relationRepository.CheckBlockRelation(userId, userTargetId) == false)
                {
                    if (_relationRepository.CheckFriendRelation(userId, userTargetId) == true)
                    {
                        await UnFriend(userTargetId);
                    }
                    if (_relationRepository.CheckFollowRelation(userId, userTargetId) == true)
                    {
                        await _relationRepository.DeleteFollowByUserId(userId, userTargetId);
                    }
                    if (_relationRepository.CheckFollowRelation(userTargetId, userId) == true)
                    {
                        await _relationRepository.DeleteFollowByUserId(userTargetId, userId);
                    }
                    if (_relationRepository.CheckWaitingRelation(userId, userTargetId) == true)
                    {
                        await Reject(userTargetId);
                    }
                    else if (_relationRepository.CheckRequestRelation(userId, userTargetId) == true)
                    {
                        await CancleFriendRequest(userTargetId);
                    }
                    Relation relationBlock = new Relation();
                    relationBlock.RelationId = Guid.NewGuid().ToString();
                    relationBlock.TypeRelation = "BLOCK";
                    relationBlock.UserId = userId;
                    relationBlock.UserTargetIduserId = userTargetId;
                    await _relationRepository.AddAsync(relationBlock);
                    return Ok();
                }
                else
                    return StatusCode(500, $"An error occurred while block");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while block: {ex.Message}");
            }
        }
        //User có id = UserId Follow User có id = UserTargetId
        [HttpPost("follow/{userTargetId}")]
        public async Task<IActionResult> FollowUser(string userTargetId)
        {
            try
            {
                var userId = _session.GetString("UserId");
                if (_relationRepository.CheckFollowRelation(userId, userTargetId) == false)
                {
                    if (_relationRepository.CheckBlockRelation(userId, userTargetId) == true)
                    {
                        return StatusCode(500, $"An error occurred while sending friend request, unblock after follow");
                    }
                    Relation relationFollow = new Relation();
                    relationFollow.RelationId = Guid.NewGuid().ToString();
                    relationFollow.TypeRelation = "FOLLOW";
                    relationFollow.UserId = userId;
                    relationFollow.UserTargetIduserId = userTargetId;
                    await _relationRepository.AddAsync(relationFollow);
                    return Ok();
                }
                else
                    return StatusCode(500, $"An error occurred while follow");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while follow : {ex.Message}");
            }
        }
        //User có id = userId chấp nhận lời mời kết bạn của user có id = userTargetId
        [HttpPut("friend-request/{userTargetId}/accept")]
        public async Task<IActionResult> AcceptFriendRequest(string userTargetId)
        {
            try
            {
                var userId = _session.GetString("UserId");
                var relationRequest = _relationRepository.GetRequestByUserIdAndUserTargetId(userTargetId, userId);
                var relationWaiting = _relationRepository.GetWaitingByUserIdAndUserTargetId(userId, userTargetId);
                relationRequest.TypeRelation = "FRIEND";
                relationWaiting.TypeRelation = "FRIEND";
                await _relationRepository.UpdateAsync(relationRequest);
                await _relationRepository.UpdateAsync(relationWaiting);
                if (_relationRepository.CheckFollowRelation(userId, userTargetId) == false)
                {
                    Relation relationFollow = new Relation();
                    relationFollow.RelationId = Guid.NewGuid().ToString();
                    relationFollow.TypeRelation = "FOLLOW";
                    relationFollow.UserId = userId;
                    relationFollow.UserTargetIduserId = userTargetId;
                    await _relationRepository.AddAsync(relationFollow);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //User có id = UserId xóa mối quan hệ bạn bè với user có id = UserTargetId
        [HttpDelete("friends/{userTargetId}")]
        public async Task<IActionResult> UnFriend(string userTargetId)
        {
            try
            {
                var userId = _session.GetString("UserId");
                if (_relationRepository.CheckFollowRelation(userId, userTargetId) == true)
                {
                    await _relationRepository.DeleteFollowByUserId(userId, userTargetId);
                }
                if (_relationRepository.CheckFollowRelation(userTargetId, userId) == true)
                {
                    await _relationRepository.DeleteFollowByUserId(userTargetId, userId);
                }
                await _relationRepository.DeleteFriendByUserId(userId, userTargetId);
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
        // User có id = userId từ chối lời mời kết bạn của user có id = userTargetId
        [HttpDelete("friend-requests/{userTargetId}/reject")]
        public async Task<IActionResult> Reject(string userTargetId)
        {
            try
            {
                var userId = _session.GetString("UserId");
                if (_relationRepository.CheckFollowRelation(userId, userTargetId) == true)
                {
                    await _relationRepository.DeleteFollowByUserId(userId, userTargetId);
                }
                if (_relationRepository.CheckFollowRelation(userTargetId, userId) == true)
                {
                    await _relationRepository.DeleteFollowByUserId(userTargetId, userId);
                }
                await _relationRepository.RejectFriendRequestByUserId(userId, userTargetId);
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
        [HttpDelete("follow/{userTargetId}")]
        public async Task<IActionResult> UnFollow(string userTargetId)
        {
            try
            {
                var userId = _session.GetString("UserId");
                await _relationRepository.DeleteFollowByUserId(userId, userTargetId);
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
        //User có id = UserId hủy block với user có id = UserTargetId
        [HttpDelete("block/{userTargetId}")]
        public async Task<IActionResult> UnBlock(string userTargetId)
        {
            try
            {
                var userId = _session.GetString("UserId");
                await _relationRepository.DeleteBlockByUserId(userId, userTargetId);
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
        [HttpDelete("friend-requests/{userTargetId}/cancle")]
        public async Task<IActionResult> CancleFriendRequest(string userTargetId)
        {
            try
            {
                var userId = _session.GetString("UserId");
                if (_relationRepository.CheckFollowRelation(userId, userTargetId) == true)
                {
                    await _relationRepository.DeleteFollowByUserId(userId, userTargetId);
                }
                await _relationRepository.DeleteRequestAndWaitingByUserId(userId, userTargetId);
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
    }
}
