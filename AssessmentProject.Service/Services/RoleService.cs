using AssessmentProject.Repository.Interfaces;
using AssessmentProject.Repository.Models;
using AssessmentProject.Service.Interfaces;

namespace AssessmentProject.Service.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService (IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<List<Roles>> GetAllRoles()
    {
        return await _roleRepository.GetAllRoles();
    }
}
