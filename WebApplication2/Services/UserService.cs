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

    public async Task<bool> IsUserExists(string username)
    {
        return await _applicationDbContext
            .Users
            .AnyAsync(u => u.Username == username
                            || u.MobileNumber == username);
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
}