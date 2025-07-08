//Um namespace em C# é um agrupador lógico de tipos (classes, interfaces, etc.) 
//que organiza o código e evita conflitos de nomes entre partes diferentes do projeto.
namespace LiteratureReviewApi.Models;

public class UserTask
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Descritption { get; set; } = "";
    public string Status { get; set; } = "In progress";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
