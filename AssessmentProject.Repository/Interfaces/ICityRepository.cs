using AssessmentProject.Repository.Models;

namespace AssessmentProject.Repository.Interfaces;

public interface ICityRepository
{
    Task<List<Cities>> GetAllCities();
    Task<List<Cities>> GetCitiesByStateId(int stateId);
}
