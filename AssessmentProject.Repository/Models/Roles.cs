using System.ComponentModel.DataAnnotations;

namespace AssessmentProject.Repository.Models;

public class Roles
{
    [Key]
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Users> Users { get; set; } = new List<Users>();
}
