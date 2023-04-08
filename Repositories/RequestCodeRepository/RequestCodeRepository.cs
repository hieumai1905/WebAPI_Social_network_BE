using Microsoft.EntityFrameworkCore;
using Web_Social_network_BE.Models;

namespace Web_Social_network_BE.Repositories.UserRepository;

public class RequestCodeRepository : IRequestCodeRepository
{
    private readonly SocialNetworkN01Context _context;

    public RequestCodeRepository(SocialNetworkN01Context context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Request>> GetAllAsync()
    {
        try
        {
            return await _context.Requests.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while getting all requests code.{ex.Message}");
        }
    }

    public async Task<Request> GetByIdAsync(long key)
    {
        try
        {
            return await _context.Requests.FindAsync(key);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while getting request code with id {key}.{ex.Message}");
        }
    }

    public async Task<Request> AddAsync(Request entity)
    {
        try
        {
            _context.Requests.Add(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while adding user {entity.RegisterId}.{ex.Message}");
        }
    }

    public async Task UpdateAsync(Request entity)
    {
        try
        {
            var requestToUpdate = await _context.Requests.AsNoTracking()
                .FirstOrDefaultAsync(x => x.RegisterId == entity.RegisterId);
            if (requestToUpdate == null)
                throw new Exception($"Request code with id {entity.RegisterId} don't exist.");
            _context.Requests.Update(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"An error occurred while updating request code with id {entity.RegisterId}.{ex.Message}");
        }
    }

    public async Task DeleteAsync(long key)
    {
        try
        {
            var requestToDelete = await _context.Requests.FindAsync(key);
            if (requestToDelete == null)
            {
                throw new ArgumentException($"Request with id {key} does not exist");
            }

            _context.Requests.Remove(requestToDelete);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while deleting request with id {key}. {ex.Message}");
        }
    }

    public async Task<Request?> GetByEmail(string email)
    {
        try
        {
            var request = await _context.Requests.FirstOrDefaultAsync(u => u.Email == email);
            if (request == null)
                throw new ArgumentException($"Request with email {email} does not exist");
            return request;
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while getting Requests by email {email}. {ex.Message}");
        }
    }

    public async Task CleanRequestCode()
    {
        try
        {
            var requestOutTime =
                await _context.Requests.Where(x => x.RegisterAt.AddMinutes(5) < DateTime.Now).ToListAsync();
            _context.Requests.RemoveRange(requestOutTime);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while cleaning request code.{ex.Message}");
        }
    }

    public async Task<int> GetCountRequest()
    {
        try
        {
            return await _context.Requests.CountAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while getting count request code.{ex.Message}");
        }
    }

    public async Task<Request> RefreshCode(string email)
    {
        try
        {
            var requestSend = _context.Requests.FirstOrDefault(x => x.Email == email);
            if (requestSend == null)
            {
                throw new Exception("Request not found");
            }

            requestSend.RegisterAt = DateTime.Now;
            requestSend.RequestCode = new Random().Next(100000, 999999);
            _context.Requests.Update(requestSend);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return requestSend;
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while refreshing request code.{ex.Message}");
        }
    }
}