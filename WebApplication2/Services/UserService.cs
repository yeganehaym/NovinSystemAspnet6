using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Data.Entity;

namespace WebApplication2.Services;

public class UserService
{
    private ApplicationDbContext _applicationDbContext;

    public UserService(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<User> LoginAsync(string username, string password)
    {
        return await _applicationDbContext
            .Users
            .FirstOrDefaultAsync(x => x.Username == username 
                                      && x.Password == password);
    }
    public async Task<bool> IsUserExists(string username)
    {
        return await _applicationDbContext
            .Users
            .AnyAsync(u => u.Username == username
                            || u.MobileNumber == username);
    }

    public async Task<User> GetUserAsync(string username)
    {
        return await _applicationDbContext
            .Users
            .FirstOrDefaultAsync(x => x.Username == username);
    }

    public async Task AddUserAsync(User user)
    {
        await _applicationDbContext
            .Users
            .AddAsync(user);
    }

    public async Task<User> FindUserAsync(int id)
    {
        return await _applicationDbContext
            .Users
            .FindAsync(id);
    }

    public async Task<User> FinUserAsync(string username)
    {
        return await _applicationDbContext
            .Users
            .FirstOrDefaultAsync(u => u.Username == username 
                                      || u.MobileNumber==username);
    }

    public async Task AddOtpCode(OtpCode otpCode)
    {
        await _applicationDbContext.OtpCodes.AddAsync(otpCode);
    }

    public async Task<OtpCode> GetOtpCodeAsync(string code)
    {
        return  await _applicationDbContext.OtpCodes
            .Include(x=>x.User)
            .FirstOrDefaultAsync(o => o.Code == code);
    }

    public async Task<List<Role>> GetRolesAsync(int userId)
    {
        return await _applicationDbContext
            .UserRoles
            .Include(x=>x.Role)
            .Where(x => x.UserId == userId)
            .Select(x=>x.Role)
            .ToListAsync();
    }
}