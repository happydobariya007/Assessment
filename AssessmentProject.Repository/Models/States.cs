namespace AssessmentProject.Repository.Models;

public class States
{
    public int Id { get; set; }

    public string StateName { get; set; } = null!;

    public int? CountryId { get; set; }

    public virtual ICollection<Cities> Cities { get; set; } = new List<Cities>();

    public virtual Countries? Country { get; set; }
}
