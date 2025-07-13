
using LiteratureReviewApi.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace LiteratureReviewAPI.Repositories
{
    public class FileTaskRepository : ITaskRepository
    {
        private readonly string _filePath;

        public FileTaskRepository(string filePath)
        {
            _filePath = filePath;
        }

        private List<UserTask> Load()
        {
            if (!File.Exists(_filePath)) return new();
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<UserTask>>(json) ?? new();
        }

        public void Save(List<UserTask> tasks)
        {
            var json = JsonSerializer.Serialize(tasks);
            File.WriteAllText(_filePath, json);
        }
        public List<UserTask> GetAll()
        {
            return Load();
        }
        public UserTask? GetById(int id)
        {
            var tasks = Load();
            return tasks.FirstOrDefault(t => t.Id == id);
        }

    }
};
