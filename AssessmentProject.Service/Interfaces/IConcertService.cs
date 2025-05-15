using AssessmentProject.Repository.Models;
using AssessmentProject.Repository.ViewModels;

namespace AssessmentProject.Service.Interfaces;

public interface IConcertService
{
    Task<PaginationVM<Concerts>> GetAllConcerts(int? CurrentPageIndex = null, int? PageSize = null, string? searchKeyword = null);
    Task<(bool isSuccess, string errMessage)> CreateConcert(CreateConcertVM model);
    Task<EditConcertVM?> ConcertDataForUpdate(int concertId);
    Task<Concerts?> GetConcertById(int concertId);
    Task<(bool isSuccess, string errMessage)> UpdateConcert(EditConcertVM model);
    void DeleteConcert(int concertId);
    Task<List<Concerts>> GetEveryConcerts();
    int GetPriceByConcertId(int concertId);
    int? GetRequiredTicketsByConcertId(int concertId);
    int? GetDiscountByConcertId(int concertId);
}
