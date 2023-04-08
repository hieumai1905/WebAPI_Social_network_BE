using Web_Social_network_BE.Models;
using System.Collections;
using Web_Social_network_BE.Repositories;
namespace Web_Social_network_BE.Repositories.RelationRepository
{
    public interface IRelationRepository : IGeneralRepository<Relation, String>
    {
        Task<IEnumerable> GetAllRelationByUserId(string key);
        Task<IEnumerable> GetFriendByUserId(string key);
        Task<IEnumerable> GetBlockByUserId(string key);
        Task<IEnumerable> GetFollowByUserId(string key);
        Task<IEnumerable> GetWaitingUserById(string key);
        Task<IEnumerable> GetRequestByUserId(string key);
        Task<IEnumerable> GetAnyUserBlockMe(string key);
        Task<IEnumerable> GetRelationByUserIdAndUserTargetId(string userId, string userTargetId);
        Relation GetRequestByUserIdAndUserTargetId(string userId, string userTargetId);
        Relation GetWaitingByUserIdAndUserTargetId(string userId, string userTargetId);
        bool CheckFriendRelation(string userId, string userTargetId);
        bool CheckRequestRelation(string userId, string userTargetId);
        bool CheckWaitingRelation(string userId, string userTargetId);
        bool CheckFollowRelation(string userId, string userTargetId);
        bool CheckBlockRelation(string userId, string userTargetId);
        Task RejectFriendRequestByUserId(string userId, string userTargetId);
        Task DeleteFriendByUserId(string userId, string userTargetId);
        Task DeleteBlockByUserId(string userId, string userTargetId);
        Task DeleteFollowByUserId(string userId, string userTargetId);
        Task DeleteRequestAndWaitingByUserId(string userId, string userTargetId);
    }
}
