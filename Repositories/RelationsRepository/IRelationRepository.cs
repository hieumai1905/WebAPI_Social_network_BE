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
        Task DeleteFriendByUserId(String UserId, String UserTargetId);
    }
}
