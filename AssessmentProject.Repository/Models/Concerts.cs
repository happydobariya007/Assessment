using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssessmentProject.Repository.Models;

public class Concerts
{
    [Key]

    public int ConcertID { get; set; }
    public string Title { get; set; } = null!;
    public string ArtistName { get; set; } = null!;
    public string Venue { get; set; } = null!;
    [Column(TypeName ="timestamp without time zone")]
    public DateTime DateAndTime { get; set; }
    public int Price { get; set; }
    public int? RequiredTicketsForDiscount { get; set; }
    public int? Discount { get; set; }
    public int TotalSeats { get; set; }
    public DateTime CreatedOn { get; set; }
    public int CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public bool Isdeleted { get; set; }
    public virtual ICollection<UserConcertsMapping> UserConcertsMapping { get; set; } = new List<UserConcertsMapping>();

}
