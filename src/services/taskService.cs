using LiteratureReviewAPI.src.models;

// Um namespace em C# é uma forma de organizar e agrupar tipos relacionados (classes, interfaces, enums, etc.) 
// e evitar conflitos de nomes entre diferentes partes do código.
namespace LiteratureReviewAPI.src.services
{
    public class TaskService
    {
        private static readonly List<Task> _tasks = new();
        private static int _nextId = 1;

        public List<Task> GetAll()
        {
            return _tasks;
        }

        public Task? GetById(int id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public Task Create(Task task)
        {
            task.Id = _nextId++;
            _tasks.Add(task);
            return task;
        }

        public bool Update(int id, Task updated)
        {
            var existing = _tasks.FirstOrDefault(t => t.Id == id);
            if (existing == null) return false;

            existing.Title = updated.Title;
            existing.startDate = updated.startDate;
            existing.endDate = updated.endDate;
            return true;
        }

        public bool Delete(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return false;
            _tasks.Remove(task);
            return true;
        }
    }
}
