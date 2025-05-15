using System.ComponentModel.DataAnnotations;

namespace AssessmentProject.Repository.ViewModels;

public class CreateUserVM
{
    public int UserId { get; set; }
    [Required]
    [RegularExpression(@"^[A-Za-z]+(?: [A-Za-z]+)*$", ErrorMessage = "Only letters allowed in name field")]
    public string? FirstName { get; set; }
    [RegularExpression(@"^[A-Za-z]+(?: [A-Za-z]+)*$", ErrorMessage = "Only letters allowed in name field")]
    public string? LastName { get; set; }
    [Required]
    public string EmailId { get; set; } = null!;
    [Required]
    public string? Address { get; set; }
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "The ContactNumber is required")]
    [RegularExpression(@"^\d{1,10}$", ErrorMessage = "ContactNumber should be maximum 10 digit long")]
    public long? ContactNumber { get; set; }
    [Required(ErrorMessage = "The CountryName is required")]
    public int? CountryId { get; set; }
    [Required(ErrorMessage = "The StateName is required")]
    public int? StateId { get; set; }
    [Required(ErrorMessage = "The CityName is required")]
    public int? CityId { get; set; }
    public int RoleId { get; set; }

}
