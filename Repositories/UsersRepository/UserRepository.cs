﻿using Microsoft.EntityFrameworkCore;
using Web_Social_network_BE.Handle;
using Web_Social_network_BE.Models;
using Web_Social_network_BE.RequestModel;

namespace Web_Social_network_BE.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly SocialNetworkN01Ver2Context _context;

        public UserRepository(SocialNetworkN01Ver2Context context)
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
                throw new Exception($"An error occurred while getting all users.{ex}");
            }
        }

        public async Task<IEnumerable<User>> FindUserContent(string content)
        {
            try
            {
                var users = _context.Users.Where(u =>
                    u.FullName.Contains(content) || u.UserInfo.Email.Contains(content) ||
                    u.UserInfo.Phone.Contains(content)).ToList();
                return users;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting all users.{ex}");
            }
        }

        public async Task<IEnumerable<User>> GetAllWithInfoAsync()
        {
            try
            {
                var users = _context.Users.Include(u => u.UserInfo).ToList();
                return users;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting all users.{ex}");
            }
        }

		public async Task<User> BanAsync(string key)
		{
			try
			{
				var user = await _context.Users.AsNoTracking().Include(x => x.UserInfo).FirstOrDefaultAsync(x => x.UserId == key);
				if (user == null)
				{
					throw new ArgumentException($"User not found with id {key}");
				}
				user.UserInfo.Status = "BAN";
				_context.Users.Update(user);
				await _context.SaveChangesAsync().ConfigureAwait(false);
				return user;
			}
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while updating user {key}.{ex}");
			}
		}

		public async Task<User> LockAsync(string key)
        {
            try
            {
                var user = await _context.Users.AsNoTracking().Include(x=>x.UserInfo).FirstOrDefaultAsync(x => x.UserId == key);
                if (user == null)
                {
                    throw new ArgumentException($"User not found with id {key}");
                }
                user.UserInfo.Status = "LOCK";
                _context.Users.Update(user);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating user {key}.{ex}");
            }
        }
        
        public async Task<User> UnLockAsync(string key)
        {
            try
            {
                var user = await _context.Users.AsNoTracking().Include(x=>x.UserInfo).FirstOrDefaultAsync(x => x.UserId == key);
                if (user == null)
                {
                    throw new ArgumentException($"User not found with id {key}");
                }
                user.UserInfo.Status = "ACTIVE";
                _context.Users.Update(user);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating user {key}.{ex}");
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
                throw new Exception($"An error occurred while getting user with id {key}.{ex}");
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
                throw new Exception($"An error occurred while adding user {entity.UserId}.{ex}");
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
                throw new Exception($"An error occurred while updating user with id {entity.UserId}.{ex}");
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

                var userInfoId = userToDelete.UserInfoId;
                _context.Users.Remove(userToDelete);
                var UserInfo = await _context.UsersInfos.FindAsync(userInfoId);
                _context.UsersInfos.Remove(UserInfo);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting user with id {key}.{ex}");
            }
        }

        public async Task<UsersInfo> GetUserInfoAsync(string userInfoId)
        {
            var userInfo = await _context.UsersInfos
                .FirstOrDefaultAsync(u => u.UserInfoId == userInfoId).ConfigureAwait(false);

            if (userInfo == null)
            {
                throw new ArgumentException($"User info with id {userInfoId} not found");
            }

            return userInfo;
        }

        public async Task<User?> GetByEmail(string email)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.UserInfo.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting user by email {email}.{ex}");
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
                throw new Exception($"An error occurred while getting user by phone {phone}.{ex}");
            }
        }

        public async Task<User> GetInformationUser(string idUser)
        {
            try
            {
                User user =
                    await _context.Users.Include(u => u.UserInfo).FirstOrDefaultAsync(u => u.UserId == idUser) ??
                    throw new InvalidOperationException();
                if (user == null)
                {
                    throw new Exception($"User with id {idUser} not found");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting user information by id {idUser}.{ex}");
            }
        }

        public async Task<User> Login(LoginModel account)
        {
            try
            {
                var user = await _context.Users.Include(u => u.UserInfo)
                    .FirstOrDefaultAsync(u => u.UserInfo.Email == account.Email);
                if (user == null)
                {
                    throw new ArgumentException($"Account with email {account.Email} does not exist");
                }

                if (user.UserInfo.Password != MD5Hash.GetHashString(account.Password))
                {
                    throw new ArgumentException($"Password is incorrect");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while logging in user with email {account.Email}.{ex}");
            }
        }

        public async Task<User> Register(RegisterModel account)
        {
            try
            {
                var user = await _context.Users.Include(u => u.UserInfo)
                    .FirstOrDefaultAsync(u => u.UserInfo.Email == account.Email);
                if (user != null)
                {
                    throw new ArgumentException($"User with email {account.Email} already exists");
                }

                var userInfoId = Guid.NewGuid().ToString();
                user = new User()
                {
                    UserId = Guid.NewGuid().ToString(),
                    FullName = account.Name,
                    Avatar = "images/astofol.png",
                    UserInfoId = userInfoId,
                    UserInfo = new UsersInfo()
                    {
                        UserInfoId = userInfoId,
                        Password = MD5Hash.GetHashString(account.Password),
                        Email = account.Email,
                        Dob = null,
                        Address = null,
                        Status = "INACTIVE",
                        UserRole = "USER_ROLE",
                        AboutMe = "",
                        CoverImage = "",
                        RegisterAt = DateTime.Today
                    }
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while register user.{ex}");
            }
        }
    }
}
