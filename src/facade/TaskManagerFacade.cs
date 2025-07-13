using LiteratureReviewAPI.Services;

namespace LiteratureReviewAPI.Facade
{
    public class TaskManagerFacade
    {
        private readonly TaskService _taskService;
        private readonly TaskCompletionService _completionService;
        private readonly TaskFilterService _filterService;

        public TaskManagerFacade(TaskService taskService, TaskCompletionService completionService, TaskFilterService filterService)
        {
            _taskService = taskService;
            _completionService = completionService;
            _filterService = filterService;
        }

    }

}
