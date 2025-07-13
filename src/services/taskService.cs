using LiteratureReviewApi.Models;
using System.Text.Json;

namespace LiteratureReviewAPI.Services
{

    public class TaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public bool Delete(int id)
        {
            var tasks = _repository.GetAll();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                tasks.Remove(task);
                _repository.Save(tasks);
            } else 
            {
                throw new KeyNotFoundException($"Task with ID {id} not found.");
            }

            return true;
        }
        public bool Create(UserTask task)
        {
            Console.WriteLine($"Creating task: {task.Title}");
            var tasks = _repository.GetAll();
            task.Id = tasks.Count == 0 ? 1 : tasks.Max(t => t.Id) + 1;

            foreach (var dep in task.Dependencies)
            {
                if (dep >= task.Id)
                    throw new InvalidOperationException("Task dependencies must refer to already created tasks.");
            }

            tasks.Add(task);
            _repository.Save(tasks);
            Console.WriteLine($"Salvou a task: {task.Id} - {task.Title}");
            return true;
        }

        public bool Update(int id, UserTask updated)
        {
            var tasks = _repository.GetAll();
            var clone = tasks
                .Select(t => new UserTask
                {
                    Id = t.Id,
                    Title = t.Title,
                    Descritption = t.Descritption,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    Dependencies = t.Dependencies
                }).ToList();

            var task = clone.FirstOrDefault(t => t.Id == id);
            if (task == null) return false;

            task.Title = updated.Title;
            task.Dependencies = updated.Dependencies;
            task.Descritption = updated.Descritption;
            task.StartDate = updated.StartDate;
            task.EndDate = updated.EndDate;
            task.Difficulty = updated.Difficulty;

            var graph = clone.ToDictionary(t => t.Id, t => t.Dependencies);
            if (HasCycle(graph))
                throw new InvalidOperationException("Update would create cyclic dependencies.");
                
            _repository.Save(clone);

            return true;
        }

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
