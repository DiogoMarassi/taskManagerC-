using UserTask = LiteratureReviewApi.Models.UserTask;

public interface ITaskObserver
{
    void finishDependencies(UserTask task);
}