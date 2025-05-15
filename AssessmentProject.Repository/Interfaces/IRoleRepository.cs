using AssessmentProject.Repository.Models;

namespace AssessmentProject.Repository.Interfaces;

public interface IRoleRepository
{
    Task<List<Roles>> GetAllRoles();
}
