using Microsoft.EntityFrameworkCore;
using Web_Social_network_BE.Models;

namespace Web_Social_network_BE.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly SocialNetworkN01Context _context;

        public UserRepository(SocialNetworkN01Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting all users.", ex);
            }
        }

        public async Task<User> GetByIdAsync(string key)
        {
            try
            {
                return await _context.Users.FindAsync(key);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting user with id {key}.", ex);
            }
        }

        public async Task<User> AddAsync(User entity)
        {
            try
            {
                _context.Users.Add(entity);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding user {entity.UserId}.", ex);
            }
        }

        public async Task UpdateAsync(User entity)
        {
            try
            {
                var userToUpdate =
                    await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == entity.UserId);

                if (userToUpdate == null)
                {
                    throw new ArgumentException($"User with id {entity.UserId} does not exist");
                }

                _context.Users.Update(entity);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating user with id {entity.UserId}.", ex);
            }
        }

        public async Task DeleteAsync(string key)
        {
            try
            {
                var userToDelete = await _context.Users.FindAsync(key);

                if (userToDelete == null)
                {
                    throw new ArgumentException($"User with id {key} does not exist");
                }

                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting user with id {key}.", ex);
            }
        }

        public async Task<UsersInfo> GetUserInfoAsync(string userUserInfoId)
        {
            var userInfo = await _context.UsersInfos
                .FirstOrDefaultAsync(u => u.UserInfoId == userUserInfoId).ConfigureAwait(false);

            if (userInfo == null)
            {
                throw new ArgumentException($"User info with id {userUserInfoId} not found");
            }

            return userInfo;
        }

        public async Task<User> GetByEmail(string email)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.UserInfo.Email == email) ??
                       throw new InvalidOperationException();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting user by email {email}.", ex);
            }
        }

        public async Task<User> GetByPhone(string phone)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.UserInfo.Phone == phone) ??
                       throw new InvalidOperationException();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting user by phone {phone}.", ex);
            }
        }
    }
}
