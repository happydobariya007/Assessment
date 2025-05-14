namespace AssessmentProject.Repository.Models;

public class Countries
{
    public int Id { get; set; }

    public string CountryName { get; set; } = null!;

    public virtual ICollection<States> States { get; set; } = new List<States>();
}
