using AssessmentProject.Repository.Interfaces;
using AssessmentProject.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace AssessmentProject.Repository.Repositories;

public class ConcertRepository : IConcertRepository
{
    private readonly AssessmentProjectContext _context;

    public ConcertRepository(AssessmentProjectContext context)
    {
        _context = context;
    }

    public async Task<List<Concerts>> GetEveryConcerts()
    {
        return await _context.Concerts.OrderBy(c => c.ConcertID).Where(c => !c.Isdeleted).ToListAsync();
    }
    public int GetPriceByConcertId(int concertId)
    {
        return  _context.Concerts.Where(c => c.ConcertID == concertId && !c.Isdeleted).Select(c => c.Price).FirstOrDefault();
    }
    public int? GetRequiredTicketsByConcertId(int concertId)
    {
        return  _context.Concerts.Where(c => c.ConcertID == concertId && !c.Isdeleted).Select(c => c.RequiredTicketsForDiscount).FirstOrDefault();
    }
    public int? GetDiscountByConcertId(int concertId)
    {
        return  _context.Concerts.Where(c => c.ConcertID == concertId && !c.Isdeleted).Select(c => c.Discount).FirstOrDefault();
    }

    public async Task<List<Concerts>> GetAllConcerts(string? searchKeyword = null)
    {
        IQueryable<Concerts>? concerts = _context.Concerts.Where(c => !c.Isdeleted);

        if (!string.IsNullOrEmpty(searchKeyword))
        {
            concerts = concerts.Where(c => EF.Functions.Like(c.Title.ToLower(), $"%{searchKeyword.ToLower()}%") || EF.Functions.Like(c.ArtistName.ToLower(), $"%{searchKeyword.ToLower()}%"));
        }
        return await concerts.ToListAsync();
    }
    public async Task<bool> CreateConcert(Concerts concert)
    {
        try
        {
            await _context.Concerts.AddAsync(concert);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public async Task<bool> UpdateConcert(Concerts concert)
    {
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Concerts?> GetConcertById(int concertId)
    {
        return await _context.Concerts.FirstOrDefaultAsync(c => c.ConcertID == concertId);
    }
    public void DeleteConcert(int concertId)
    {
        Concerts? concert = _context.Concerts.Find(concertId);
        if (concert != null)
        {
            concert.Isdeleted = true;
            _context.SaveChanges();
        }
    }
}
