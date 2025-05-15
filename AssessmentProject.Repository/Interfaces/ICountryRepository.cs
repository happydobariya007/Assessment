using AssessmentProject.Repository.Models;

namespace AssessmentProject.Repository.Interfaces;

public interface ICountryRepository
{
    Task<List<Countries>> GetAllCountries();
}
