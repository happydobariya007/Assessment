using AssessmentProject.Repository.Interfaces;
using AssessmentProject.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace AssessmentProject.Repository.Repositories;

public class StateRepository : IStateRepository
{
    private readonly AssessmentProjectContext _context;
     public StateRepository(AssessmentProjectContext context)
    {
        _context = context;
    }

    public async Task<List<States>> GetAllStates()
    {
        return await _context.States.ToListAsync();
    }

    public async Task<List<States>> GetStatesByCountryId(int countryId)
    {
        return await _context.States.Where(s => s.CountryId == countryId).ToListAsync();
    }
}
