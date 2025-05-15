using AssessmentProject.Repository.Interfaces;
using AssessmentProject.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace AssessmentProject.Repository.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AssessmentProjectContext _context;

    public UserRepository(AssessmentProjectContext context)
    {
        _context = context;
    }

    public async Task<Users?> GetUserByEmailAndPassword(string email, string password)
    {
        Users? user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.EmailId == email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return null;
        }
        return user;
    }

    public async Task<List<Users>> GetAllUsers(string sortBy, string sortOrder, string? searchKeyword = null)
    {
        IQueryable<Users>? users = _context.Users.Include(u => u.Role).Where(u => !u.Isdeleted);

        if (!string.IsNullOrEmpty(searchKeyword))
        {
            users = users.Where(u => EF.Functions.Like(u.FirstName.ToLower(), $"%{searchKeyword.ToLower()}%") || EF.Functions.Like(u.LastName.ToLower(), $"%{searchKeyword.ToLower()}%"));
        }

        switch (sortBy.ToLower())
        {
            case "firstname":
                users = (sortOrder.ToLower() == "desc") ? users.OrderByDescending(u => u.FirstName) : users.OrderBy(u => u.FirstName);
                break;

            case "role":
                users = (sortOrder.ToLower() == "desc") ? users.OrderByDescending(u => u.Role.RoleName) : users.OrderBy(u => u.Role.RoleName);
                break;

            default:
                users = (sortOrder.ToLower() == "desc") ? users.OrderByDescending(u => u.UserID) : users.OrderBy(u => u.UserID);
                break;
        }
        return await users.ToListAsync();
    }
    public async Task<bool> CreateUser(Users user)
    {
        try
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> CheckDuplicateContactNumber(string? contactNumber)
    {
        if (string.IsNullOrEmpty(contactNumber))
            return false;

        return await _context.Users.AnyAsync(u => u.ContactNumber.ToString() == contactNumber);
    }
    public async Task<bool> CheckDuplicateEmail(string email)
    {
        bool user = await _context.Users.AnyAsync(u => u.EmailId == email);
        if (user)
        {
            return true;
        }
        return false;
    }
}
