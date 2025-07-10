// Importa a classe Task do namespace definido no diretório especificado.
// Permite que a classe Task seja usada diretamente sem precisar do caminho completo.
using LiteratureReviewApi.Models;

// Define um namespace para agrupar classes relacionadas e evitar conflitos de nomes.
// Aqui, a classe TaskService pertence ao namespace LiteratureReviewAPI.src.services.
namespace LiteratureReviewAPI.Services {
    // Declaração da classe pública TaskService.
    // Essa classe oferece funcionalidades para gerenciar tarefas (CRUD em memória).

    // INTERFACE CRIADA PARA DESACOPLAR A LÓGICA DO SERVIÇO DO ARMAZENAMENTO
    public interface ITaskRepository
    {
        List<UserTask> GetAll();
        UserTask? GetById(int id); // pode ser nulo
        UserTask Create(UserTask task);
        bool Update(int id, UserTask updated);
        bool Delete(int id);
    }
    

    // IMPLEMENTAÇÃO EM MEMÓRIA DO REPOSITÓRIO
    public class InMemoryTaskRepository : ITaskRepository
    {
        // REMOVIDO O 'static' — AGORA É UM CAMPO DE INSTÂNCIA (BOA PRÁTICA DE ENCAPSULAMENTO)
        private readonly List<UserTask> _tasks = new();
        private int _nextId = 1; // CADA INSTÂNCIA DO REPOSITÓRIO TEM SEU PRÓPRIO CONTADOR
        
        public List<UserTask> GetAll() => _tasks;

        public UserTask? GetById(int id) =>
            _tasks.FirstOrDefault(t => t.Id == id);

        public UserTask Create(UserTask task)
        {
            task.Id = _nextId++;
            _tasks.Add(task);
            return task;
        }

        public bool Update(int id, UserTask updated)
        {
            var existing = _tasks.FirstOrDefault(t => t.Id == id);
            if (existing == null) return false;

            existing.Title = updated.Title;
            existing.StartDate = updated.StartDate;
            existing.EndDate = updated.EndDate;

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

    public class TaskService {
        // Campo privado e somente leitura (readonly): a lista que armazena todas as tarefas.
        // 'static' significa que pertence à classe e não a uma instância específica.

        //Esse modificador significa que a variável _tasks não pode ser reassociada após a inicialização.
        //Mas o conteúdo da lista pode ser alterado (adicionar/remover elementos). O que não pode é fazer _tasks = outraLista
        //private static readonly List<UserTask> _tasks = new(); RUIM

        private readonly ITaskRepository _repository;

        // INJEÇÃO DE DEPENDÊNCIA PELO CONSTRUTOR — MELHOR PRÁTICA PARA DESACOPLAR COMPONENTES
        public TaskService(ITaskRepository repository) {
            _repository = repository;
        }

        // TODA LÓGICA DELEGA PARA O REPOSITÓRIO — SERVIÇO NÃO SABE COMO OS DADOS SÃO GUARDADOS
        public List<UserTask> GetAll() => _repository.GetAll();
        public UserTask? GetById(int id) => _repository.GetById(id);
        public UserTask Create(UserTask task) => _repository.Create(task);
        public bool Update(int id, UserTask updated) => _repository.Update(id, updated);
        public bool Delete(int id) => _repository.Delete(id);
    }
}
