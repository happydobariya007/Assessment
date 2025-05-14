using System.ComponentModel.DataAnnotations;

namespace AssessmentProject.Repository.Models;

public class Users
{
    [Key]
    public int UserID { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public long? ContactNumber { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }


    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool Isdeleted { get; set; }

    public int? CountryId { get; set; }

    public int? StateId { get; set; }

    public int? CityId { get; set; }

    public string EmailId { get; set; } = null!;

    public string Password { get; set; } = null!;
    public Roles Roles { get; set; } = null!;

}
