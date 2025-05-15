using System.ComponentModel.DataAnnotations;

namespace AssessmentProject.Repository.Models;

public class UserConcertsMapping
{
    [Key]
    public int UserConcertsMappingID { get; set; }
    public int UserID { get; set; }
    public int ConcertID { get; set; }
    public int BookedSeats { get; set; }
    public int TotalPrice { get; set; }
    public virtual Users User { get; set; } = null!;
    public virtual Concerts Concert { get; set; } = null!;
}
