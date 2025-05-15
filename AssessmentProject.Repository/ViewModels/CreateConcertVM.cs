using System.ComponentModel.DataAnnotations;

namespace AssessmentProject.Repository.ViewModels;

public class CreateConcertVM
{
    public int ConcertID { get; set; }
    [Required]
    public string Title { get; set; } = null!;
    [Required]

    public string ArtistName { get; set; } = null!;
    [Required]

    public string Venue { get; set; } = null!;
    [Required]

    public DateTime DateAndTime { get; set; }
    [Required]

    public int Price { get; set; }
    [Required]

    public int TotalSeats { get; set; }

}
