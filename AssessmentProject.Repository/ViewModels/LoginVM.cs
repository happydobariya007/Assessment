using System.ComponentModel.DataAnnotations;

namespace AssessmentProject.Repository.ViewModels;

public class LoginVM
{
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format! Please enter a valid email.")]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
