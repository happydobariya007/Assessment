using AssessmentProject.Repository.Interfaces;
using AssessmentProject.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace AssessmentProject.Repository.Repositories;

public class CityRepository : ICityRepository
{
    private readonly AssessmentProjectContext _context;

    public CityRepository(AssessmentProjectContext context)
    {
        _context = context;
    }

    public async Task<List<Cities>> GetAllCities()
    {
        return await _context.Cities.ToListAsync();
    }

    public async Task<List<Cities>> GetCitiesByStateId(int stateId)
    {
        return await _context.Cities.Where(c => c.StateId == stateId).ToListAsync();
    }
}
