using Web_Social_network_BE.Models;
using System.Collections;
using Web_Social_network_BE.Repositories;
namespace Web_Social_network_BE.Repositories.RelationRepository
{
    public interface IRelationRepository : IGeneralRepository<Relation,String>
    {
        Task<IEnumerable> GetAllRelationByUserId(string key);
        Task<IEnumerable> GetFriendByUserId(string key);
        Task<IEnumerable> GetBlockByUserId(string key);
        Task<IEnumerable> GetFollowByUserId(string key);
        Task<IEnumerable> GetWaitingUserById(string key);
        Task<IEnumerable> GetRequestByUserId(string key);
        Task<IEnumerable> GetRelationByUserIdAndUserTargetId(string UserId, string UserTargetId);
        Relation GetRequestByUserIdAndUserTargetId(string UserId, string UserTargetId);
        Relation GetWaitingByUserIdAndUserTargetId(string UserId, string UserTargetId);
        Task RejectFriendRequestByUserId(string UserId, string UserTargetId);
        Task DeleteFriendByUserId(string UserId, string UserTargetId);
        Task DeleteBlockByUserId(string UserId, string UserTargetId); 
        Task DeleteFollowByUserId (string UserId, string UserTargetId);
    }
}
