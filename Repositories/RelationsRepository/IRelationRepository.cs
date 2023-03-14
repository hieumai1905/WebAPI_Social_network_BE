using Web_Social_network_BE.Models;
using System.Collections;
using Web_Social_network_BE.Repositories;
namespace Web_Social_network_BE.Repository.RelationRepository
{
    public interface IRelationRepository : IGeneralRepository<Relation,String>
    {
        Task<IEnumerable> GetAllRelationbyId(string key);
        Task<IEnumerable> GetFriendById(string key);
        Task<IEnumerable> GetBlockById(string key);
        Task<IEnumerable> GetFollowById(string key);
        Task<IEnumerable> GetWaitingById(string key);
        Task<IEnumerable> GetRequestById(string key);
        Task DeleteFriendById(String UserId, String UserTargetId);
    }
}
