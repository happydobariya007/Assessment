
namespace AssessmentProject.Repository.ViewModels;

public class PaginationVM<T>
{
    public List<T>? Data { get; set; }
    public int CurrentPageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalUsers { get; set; }
    public bool HasPreviousPage => CurrentPageIndex > 1;
    public bool HasNextPage => CurrentPageIndex < TotalPages;

}