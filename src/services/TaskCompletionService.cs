public class TaskCompletionService
{
    private readonly ITaskRepository _repository;
    private readonly List<ITaskObserver> _observers = new();

    public TaskCompletionService(ITaskRepository repository)
    {
        _repository = repository;
    }

    // O método FinishTask marca uma tarefa como concluída e notifica os observadores.
    // Ele conclui uma tarefa e dispara eventos para os observadores registrados.
    public void RegisterObserver(ITaskObserver observer)
    {
        _observers.Add(observer);
    }

    public bool FinishTask(int id)
    {
        var tasks = _repository.GetAll();
        var task = tasks.FirstOrDefault(t => t.Id == id);
        if (task == null) return false;

        if (task.Status == "Finished") return true;

        task.Status = "Finished";
        _repository.Save(tasks);

        foreach (var observer in _observers)
        {
            observer.finishDependencies(task);
        }

        return true;
    }
}
