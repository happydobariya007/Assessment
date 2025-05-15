using AssessmentProject.Repository.Interfaces;
using AssessmentProject.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace AssessmentProject.Repository.Repositories;

public class CountryRepository : ICountryRepository
{
    private readonly AssessmentProjectContext _context;
    public CountryRepository(AssessmentProjectContext context)
    {
        _context = context;
    }

    public async Task<List<Countries>> GetAllCountries()
    {
        return await _context.Countries.ToListAsync();
    }
}
