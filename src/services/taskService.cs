using LiteratureReviewApi.Models;
using System.Text.Json;

namespace LiteratureReviewAPI.Services {
    // Declaração da classe pública TaskService.
    // Essa classe oferece funcionalidades para gerenciar tarefas (CRUD em memória).

    // INTERFACE CRIADA PARA DESACOPLAR A LÓGICA DO SERVIÇO DO ARMAZENAMENTO (PERSISTENCIA)
    // FUNÇÕES DE ACESSO (Toda interface é abstrata por definição)
    public interface ITaskRepository
    {
        abstract List<UserTaskWithStatus> GetAll();
        abstract UserTaskWithStatus? GetById(int id); // pode ser nulo
        abstract UserTaskWithStatus Create(UserTaskWithStatus task);
        abstract bool Update(int id, UserTaskWithStatus updated);
        abstract bool Delete(int id);
    }

    public class FileTaskRepository : ITaskRepository
    {
        private readonly string _filePath;

        public FileTaskRepository(string filePath)
        {
            _filePath = filePath;
        }

        private List<UserTaskWithStatus> Load()
        {
            if (!File.Exists(_filePath)) return new();
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<UserTaskWithStatus>>(json) ?? new();
        }

        private void Save(List<UserTaskWithStatus> tasks)
        {
            var json = JsonSerializer.Serialize(tasks);
            File.WriteAllText(_filePath, json);
        }

        public List<UserTaskWithStatus> GetAll() => Load();

        public UserTaskWithStatus? GetById(int id) =>
            Load().FirstOrDefault(t => t.Id == id);

        public UserTaskWithStatus Create(UserTaskWithStatus task)
        {
            var tasks = Load();
            tasks.Add(task);
            Save(tasks);
            return task;
        }

        public bool Update(int id, UserTaskWithStatus updated)
        {
            var tasks = Load();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return false;

            task.Title = updated.Title;
            task.Dependencies = updated.Dependencies;
            task.Descritption = updated.Descritption;
            task.Status = updated.Status;
            task.StartDate = updated.StartDate;
            task.EndDate = updated.EndDate;

            Save(tasks);
            return true;
        }

        public bool Delete(int id)
        {
            var tasks = Load();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return false;

            tasks.Remove(task);
            Save(tasks);
            return true;
        }
    }



    public class TaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public List<UserTaskWithStatus> GetAll() => _repository.GetAll();

        public UserTaskWithStatus? GetById(int id) => _repository.GetById(id);

        public UserTaskWithStatus Create(UserTaskWithStatus task)
        {
            var tasks = _repository.GetAll();
            task.Id = tasks.Count == 0 ? 1 : tasks.Max(t => t.Id) + 1;

            foreach (var dep in task.Dependencies)
            {
                if (dep >= task.Id)
                    throw new InvalidOperationException("Task dependencies must refer to already created tasks.");
            }

            tasks.Add(task);

            return _repository.Create(task);
        }

        public bool Update(int id, UserTaskWithStatus updated)
        {
            var tasks = _repository.GetAll();
            var clone = tasks
                .Select(t => new UserTaskWithStatus
                {
                    Id = t.Id,
                    Title = t.Title,
                    Descritption = t.Descritption,
                    Status = t.Status,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    Dependencies = new List<int>(t.Dependencies)
                }).ToList();

            var task = clone.FirstOrDefault(t => t.Id == id);
            if (task == null) return false;

            task.Title = updated.Title;
            task.Dependencies = updated.Dependencies;
            task.Descritption = updated.Descritption;
            task.Status = updated.Status;
            task.StartDate = updated.StartDate;
            task.EndDate = updated.EndDate;

            var graph = clone.ToDictionary(t => t.Id, t => t.Dependencies);
            if (HasCycle(graph))
                throw new InvalidOperationException("Update would create cyclic dependencies.");

            return _repository.Update(id, updated);
        }

        public bool Delete(int id) => _repository.Delete(id);

        // ---------- Lógica de detecção de ciclos ----------
        private bool HasCycle(Dictionary<int, List<int>> graph)
        {
            var visited = new HashSet<int>();
            var visiting = new HashSet<int>();

            foreach (var node in graph.Keys)
                if (DetectCycle(node, graph, visited, visiting))
                    return true;

            return false;
        }

        private bool DetectCycle(int node, Dictionary<int, List<int>> graph, HashSet<int> visited, HashSet<int> visiting)
        {
            if (visited.Contains(node)) return false;
            if (visiting.Contains(node)) return true;

            visiting.Add(node);
            foreach (var neighbor in graph[node])
                if (DetectCycle(neighbor, graph, visited, visiting))
                    return true;

            visiting.Remove(node);
            visited.Add(node);
            return false;
        }
    }

}
