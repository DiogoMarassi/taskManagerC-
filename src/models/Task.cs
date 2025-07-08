namespace LiteratureReviewApi.Models;

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Descritption { get; set; } = "";
    public string Status { get; set; } = "In progress";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
