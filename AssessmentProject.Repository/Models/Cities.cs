namespace AssessmentProject.Repository.Models;

public class Cities
{
    public int Id { get; set; }

    public string CityName { get; set; } = null!;

    public int? StateId { get; set; }

    public virtual States? State { get; set; }
}
