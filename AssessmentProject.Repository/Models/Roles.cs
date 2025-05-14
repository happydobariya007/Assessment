using System.ComponentModel.DataAnnotations;

namespace AssessmentProject.Repository.Models;

public class Roles
{
    [Key]
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public bool Isdeleted { get; set; }

    public virtual ICollection<Users> Users { get; set; } = new List<Users>();
}
