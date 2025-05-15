using AssessmentProject.Repository.Models;

namespace AssessmentProject.Repository.Interfaces;

public interface IConcertRepository
{
    Task<List<Concerts>> GetAllConcerts(string? searchKeyword = null);
    Task<bool> CreateConcert(Concerts concert);
    Task<bool> UpdateConcert(Concerts concert);
    Task<Concerts?> GetConcertById(int concertId);
    void DeleteConcert(int concertId);
    Task<List<Concerts>> GetEveryConcerts();
    int GetPriceByConcertId(int concertId);
    int? GetRequiredTicketsByConcertId(int concertId);
    int? GetDiscountByConcertId(int concertId);
}
