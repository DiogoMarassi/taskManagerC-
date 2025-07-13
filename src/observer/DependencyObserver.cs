using LiteratureReviewApi.Models;

namespace LiteratureReviewAPI.Observers
{
    public class DependencyObserver : ITaskObserver
    {
        private readonly TaskCompletionService _completionService;

        public DependencyObserver(TaskCompletionService completionService)
        {
            _completionService = completionService;
        }

        public void finishDependencies(UserTask task)
        {
            Console.WriteLine($"Task {task.Id} completed. Notifying dependencies...");
            foreach (var depId in task.Dependencies)
            {
                _completionService.FinishTask(depId);
            }

        }

    }
}      
