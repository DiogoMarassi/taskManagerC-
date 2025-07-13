
using UserTask = LiteratureReviewApi.Models.UserTask;

public interface ITaskRepository
{
    abstract List<UserTask> GetAll();
    abstract UserTask? GetById(int id); // pode ser nulo
    void Save(List<UserTask> tasks);

}