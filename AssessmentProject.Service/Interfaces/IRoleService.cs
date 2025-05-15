using AssessmentProject.Repository.Models;

namespace AssessmentProject.Service.Interfaces;

public interface IRoleService
{
    Task<List<Roles>> GetAllRoles();
}
