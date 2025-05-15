using AssessmentProject.Repository.Models;

namespace AssessmentProject.Repository.Interfaces;

public interface IStateRepository
{
    Task<List<States>> GetAllStates();
    Task<List<States>> GetStatesByCountryId(int countryId);
}
