using System.Threading.Tasks;
using AssessmentProject.Repository.Interfaces;
using AssessmentProject.Repository.Models;
using AssessmentProject.Repository.ViewModels;
using AssessmentProject.Service.Interfaces;

namespace AssessmentProject.Service.Services;

public class ConcertService : IConcertService
{
    private readonly IConcertRepository _concertRepository;
    private readonly ICurrentUserService _currentUserService;


    public ConcertService(IConcertRepository concertRepository, ICurrentUserService currentUserService)
    {
        _concertRepository = concertRepository;
        _currentUserService = currentUserService;
    }

    public async Task<PaginationVM<Concerts>> GetAllConcerts(int? CurrentPageIndex = null, int? PageSize = null, string? searchKeyword = null)
    {
        int currentPage = CurrentPageIndex ?? 1;
        int pageSize = PageSize ?? 10;
        var concerts = await _concertRepository.GetAllConcerts(searchKeyword);
        var totalConcerts = concerts.Count();
        var totalPages = (int)Math.Ceiling((double)totalConcerts / pageSize);
        concerts = concerts.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        PaginationVM<Concerts> userListPagination = new PaginationVM<Concerts>();
        userListPagination.Data = concerts;
        userListPagination.CurrentPageIndex = currentPage;
        userListPagination.PageSize = pageSize;
        userListPagination.TotalPages = totalPages;
        userListPagination.TotalUsers = totalConcerts;
        return userListPagination;
    }

    public async Task<List<Concerts>> GetEveryConcerts()
    {
        return await _concertRepository.GetEveryConcerts();
    }

    public async Task<(bool isSuccess, string errMessage)> CreateConcert(CreateConcertVM model)
    {
        Concerts? newConcert = new Concerts
        {
            Title = model.Title,
            ArtistName = model.ArtistName,
            Venue = model.Venue,
            DateAndTime = model.DateAndTime,
            Price = model.Price,
            TotalSeats = model.TotalSeats,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = _currentUserService.UserId ?? 1
        };
        await _concertRepository.CreateConcert(newConcert);
        return (true, "");
    }

    public async Task<Concerts?> GetConcertById(int concertId)
    {
        return await _concertRepository.GetConcertById(concertId);
    }

    public int GetPriceByConcertId(int concertId)
    {
        return _concertRepository.GetPriceByConcertId(concertId);
    }
    public int? GetRequiredTicketsByConcertId(int concertId)
    {
        return _concertRepository.GetRequiredTicketsByConcertId(concertId);
    }
    public int? GetDiscountByConcertId(int concertId)
    {
        return _concertRepository.GetDiscountByConcertId(concertId);
    }

    public async Task<EditConcertVM?> ConcertDataForUpdate(int concertId)
    {
        Concerts? concert = await _concertRepository.GetConcertById(concertId);
        if (concert == null)
        {
            return null;
        }
        return new EditConcertVM
        {
            ConcertID = concert.ConcertID,
            Title = concert.Title,
            ArtistName = concert.ArtistName,
            Venue = concert.Venue,
            DateAndTime = concert.DateAndTime,
            Price = concert.Price,
            TotalSeats = concert.TotalSeats
        };
    }

    public async Task<(bool isSuccess, string errMessage)> UpdateConcert(EditConcertVM model)
    {
        Concerts? concert = await _concertRepository.GetConcertById(model.ConcertID);
        if (concert == null)
        {
            return (false, "");
        }
        concert.ConcertID = model.ConcertID;
        concert.Title = model.Title;
        concert.ArtistName = model.ArtistName;
        concert.Venue = model.Venue;
        concert.Price = model.Price;
        concert.DateAndTime = model.DateAndTime;
        concert.TotalSeats = model.TotalSeats;

        await _concertRepository.UpdateConcert(concert);
        return (true, "");
    }

    public void DeleteConcert(int concertId)
    {
        _concertRepository.DeleteConcert(concertId);
    }
}
