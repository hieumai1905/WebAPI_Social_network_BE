using Web_Social_network_BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using System.Collections.Concurrent;

namespace Web_Social_network_BE.Repositories.RelationRepository
{
    public class RelationRepository:IRelationRepository
    {
        private readonly SocialNetworkN01Context _context; 
        public RelationRepository (SocialNetworkN01Context context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Relation>> GetAllAsync()
        {
            try
            {
                return await _context.Relations.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting all relation for user.", ex); 
            }
        }
        public async Task<IEnumerable> GetRelationByUserIdAndUserTargetId(string UserId, string UserTargetId)
        {
            try
            {
                return await _context.Relations.Where(x => x.UserId == UserId && x.UserTargetIduserId==UserTargetId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("an error occurred while getting all relation friend for user by id", ex);
            }
        }
        public async Task<IEnumerable> GetFriendByUserId(string key)
        {
            try
            {
                return await _context.Relations.Where(x => x.UserId == key).Where(x => x.TypeRelation == "FRIEND").ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("an error occurred while getting all relation friend for user by id", ex);
            }
        }
        public async Task<IEnumerable> GetBlockByUserId(string key)
        {
            try
            {
                return await _context.Relations.Where(x => x.UserId == key).Where(x => x.TypeRelation == "BLOCK").ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("an error occurred while getting all relation block for user by id", ex);
            }
        }
        public async Task<IEnumerable> GetFollowByUserId(string key)
        {
            try
            {
                return await _context.Relations.Where(x => x.UserId == key).Where(x => x.TypeRelation == "FOLLOW").ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("an error occurred while getting all relation follow for user by id", ex);
            }
        }
        public async Task<IEnumerable> GetWaitingUserById(string key)
        {
            try
            {
                return await _context.Relations.Where(x => x.UserId == key).Where(x => x.TypeRelation == "WAITING").ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("an error occurred while getting all relation waiting for user by id", ex);
            }
        }
        public async Task<IEnumerable> GetRequestByUserId(string key)
        {
            try
            {
                return await _context.Relations.Where(x => x.UserId == key).Where(x => x.TypeRelation == "REQUEST").ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("an error occurred while getting all relation request for user by id", ex);
            }
        }
        public Relation GetRequestByUserIdAndUserTargetId(string UserId, string UserTargetId)
        {
            try
            {
                var relationRequest = _context.Relations.Where(x => x.UserId == UserId && x.UserTargetIduserId == UserTargetId && x.TypeRelation == "REQUEST").ToList();
                return relationRequest[0];
            }
            catch (Exception ex)
            {
                throw new Exception("an error occurred while getting relation request", ex);
            }
        }

        public Relation GetWaitingByUserIdAndUserTargetId(string UserId, string UserTargetId)
        {
            try
            {
                var relationWaiting = _context.Relations.Where(x => x.UserId == UserTargetId && x.UserTargetIduserId == UserId && x.TypeRelation == "WAITING").ToList();
                return relationWaiting[0];
            }
            catch (Exception ex)
            {
                throw new Exception("an error occurred while getting all relation request for user by id", ex);
            }
        }
        public async Task<IEnumerable> GetAllRelationByUserId(string key)
        {
            try
            {
                var r = await _context.Relations.Where(x => x.UserId == key).ToListAsync();
                return r;
            }
            catch (Exception ex)
            {
                throw new Exception("an error occurred while getting all relation for user by id", ex);
            }
        }
        public async Task<Relation> AddAsync(Relation entity)
        {
            try
            {
                await _context.Relations.AddAsync(entity);
                await _context.SaveChangesAsync();  
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding relation", ex);
            }
        }
        public async Task UpdateAsync(Relation entity)
        {
            try
            {
                var relationtoUpdate = await _context.Relations.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == entity.UserId&&x.UserTargetIduserId==entity.UserTargetIduserId);
                if (relationtoUpdate == null)
                {
                    throw new ArgumentException("relation does not exist");
                }
                _context.Relations.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("an error occurred while update relation", ex);
            }
        }
        public async Task DeleteFriendByUserId(string UserId, string UserTargetId)
        {
            try
            {
                var relationtoDelete = await _context.Relations.Where(x => x.UserId == UserId || x.UserId == UserTargetId).Where(x => x.UserTargetIduserId == UserTargetId || x.UserTargetIduserId == UserId).Where(x=>x.TypeRelation=="FRIEND").ToListAsync();

                if (relationtoDelete == null)
                {
                    throw new ArgumentException("Relation does not exist");
                }
                foreach (var item in relationtoDelete)
                {
                    _context.Relations.Remove(item);
                }
                await _context.SaveChangesAsync();               
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting relation", ex);
            }
        }
        public async Task RejectFriendRequestByUserId(string UserId, string UserTargetId)
        {
            try
            {
                var relationRequest = await _context.Relations.Where(x => x.UserId == UserId && x.UserTargetIduserId == UserTargetId && x.TypeRelation == "REQUEST").ToListAsync();
                var relationWaiting = await _context.Relations.Where(x => x.UserId == UserTargetId && x.UserTargetIduserId == UserId && x.TypeRelation == "WAITING").ToListAsync();
                if (relationRequest == null)
                {
                    throw new ArgumentException("Relation does not exist");
                }
                if (relationWaiting == null)
                {
                    throw new ArgumentException("Relation does not exist");
                }
                _context.Relations.Remove(relationRequest[0]);
                _context.Relations.Remove(relationWaiting[0]);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting relation", ex);
            }
        }
        public async Task DeleteBlockByUserId(string UserId, string UserTargetId)
        {
            try
            {
                var relationtoDelete = await _context.Relations.Where(x => x.UserId == UserId && x.UserTargetIduserId == UserTargetId && x.TypeRelation == "BLOCK").ToListAsync();
                if (relationtoDelete == null)
                {
                    throw new ArgumentException("Relation not exist");
                }
                _context.Relations.Remove(relationtoDelete[0]);
                await _context.SaveChangesAsync();  
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleteing relation", ex);
            }
        }

        public async Task DeleteFollowByUserId(string UserId, string UserTargetId)
        {
            try
            {
                var relationtoDelete = await _context.Relations.Where(x => x.UserId == UserId && x.UserTargetIduserId == UserTargetId && x.TypeRelation == "FOLLOW").ToListAsync();
                if (relationtoDelete == null)
                {
                    throw new ArgumentException("Relation not exist");
                }
                _context.Relations.Remove(relationtoDelete[0]);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleteing relation", ex);
            }
        }
        public Task<Relation> GetByIdAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string key)
        {
            throw new NotImplementedException();
        }
    }
}
