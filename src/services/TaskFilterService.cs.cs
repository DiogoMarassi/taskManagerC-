

public class TaskFilterService
{
    private readonly ITaskRepository _repository;

    public TaskFilterService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public bool filterTaskByDate(DateTime startDate, DateTime endDate)
    {
        var tasks = _repository.GetAll();

        if (startDate > endDate) throw new ArgumentException("Start date must be before end date.");
        if (startDate == endDate) return false;
        return tasks.Any(t => t.StartDate >= startDate && t.EndDate <= endDate);
    }

    public bool filterTaskByStatus(string status)
    {
        var tasks = _repository.GetAll();
        return tasks.Any(t => t.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
    }

    public bool filterTaskByDifficulty(string difficulty)
    {
        var tasks = _repository.GetAll();
        return tasks.Any(t => t.Difficulty.Equals(difficulty, StringComparison.OrdinalIgnoreCase));
    }
}