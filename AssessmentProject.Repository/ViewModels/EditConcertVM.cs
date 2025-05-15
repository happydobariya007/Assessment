namespace AssessmentProject.Repository.ViewModels;

public class EditConcertVM
{
    public int ConcertID { get; set; }
    public string Title { get; set; } = null!;
    public string ArtistName { get; set; } = null!;
    public string Venue { get; set; } = null!;
    public DateTime DateAndTime { get; set; }
    public int Price { get; set; }
    public int TotalSeats { get; set; }
}
