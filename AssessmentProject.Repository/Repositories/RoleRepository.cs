using AssessmentProject.Repository.Interfaces;
using AssessmentProject.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace AssessmentProject.Repository.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AssessmentProjectContext _context;
    public RoleRepository(AssessmentProjectContext context)
    {
        _context = context;
    }
    
    public async Task<List<Roles>> GetAllRoles()
    {
        return await _context.Roles.ToListAsync();
    }

}
