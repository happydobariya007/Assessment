using System.ComponentModel.DataAnnotations;

namespace AssessmentProject.Repository.ViewModels;

public class BookTicketsVM
{
    [Required]
    public int ConcertID { get; set; }
    public int UserID { get; set; }
    [Required]
    public int BookedSeats { get; set; }
    public int TotalPrice { get; set; }

}
