// Importa a classe Task do namespace definido no diretório especificado.
// Permite que a classe Task seja usada diretamente sem precisar do caminho completo.
using LiteratureReviewApi.Models;

// Define um namespace para agrupar classes relacionadas e evitar conflitos de nomes.
// Aqui, a classe TaskService pertence ao namespace LiteratureReviewAPI.src.services.
namespace LiteratureReviewAPI.Services
{
    // Declaração da classe pública TaskService.
    // Essa classe oferece funcionalidades para gerenciar tarefas (CRUD em memória).
    public class TaskService
    {
        // Campo privado e somente leitura (readonly): a lista que armazena todas as tarefas.
        // 'static' significa que pertence à classe e não a uma instância específica.

        //Esse modificador significa que a variável _tasks não pode ser reassociada após a inicialização.
        //Mas o conteúdo da lista pode ser alterado (adicionar/remover elementos). O que não pode é fazer _tasks = outraLista
        private static readonly List<UserTask> _tasks = new();

        // Campo privado estático que representa o próximo ID disponível para nova tarefa.
        // Inicia em 1 e é incrementado a cada tarefa criada.
        private static int _nextId = 1;

        // Método público que retorna todas as tarefas cadastradas na lista.
        public List<UserTask> GetAll()
        {
            return _tasks; // Retorna a lista completa (referência direta).
        }

        // Método público que retorna uma tarefa pelo ID informado.
        // O operador '?' indica que pode retornar `null` (tipo anulável).
        public UserTask? GetById(int id)
        {
            // LINQ é Language Integrated Query, uma forma de consultar coleções em C#, usando uma linguagem similar ao SQL.
            // Aqui, usamos o método FirstOrDefault para buscar a primeira tarefa que tenha o ID correspondente ao fornecido.

            // O FirstOrDefault faz um for-loop interno na lista _tasks e retorna o primeiro elemento que satisfaz a condição (t.Id == id).
            return _tasks.FirstOrDefault(t => t.Id == id);

            // LINQ - OUTOS EXEMPLOS
            //var first = lista.First();
            // var match = lista.FirstOrDefault(x => x.Id == 5);
            // var todos = lista.Where(x => x.Status == "Done");
            // var nomes = lista.Select(x => x.Nome);
        }

        // Método para criar uma nova tarefa.
        // Recebe um objeto `Task`, atribui um novo ID e adiciona à lista.
        public UserTask Create(UserTask task)
        {
            // Define o ID da nova tarefa como o próximo disponível e incrementa o contador.
            task.Id = _nextId++;
            // Adiciona a tarefa à lista.
            _tasks.Add(task);
            // Retorna a tarefa criada (com ID atualizado).
            return task;
        }

        // Método para atualizar uma tarefa existente.
        // Recebe o ID da tarefa a ser atualizada e os novos dados.
        // Retorna true se a atualização foi bem-sucedida; false caso contrário.
        public bool Update(int id, UserTask updated)
        {
            // Busca a tarefa com o ID correspondente.
            var existing = _tasks.FirstOrDefault(t => t.Id == id);
            // Se não encontrar, retorna false.
            if (existing == null) return false;

            // Atualiza os campos da tarefa existente com os dados da nova.
            existing.Title = updated.Title;
            existing.StartDate = updated.StartDate;
            existing.EndDate = updated.EndDate;

            return true; // Atualização bem-sucedida.
        }

        // Método para remover uma tarefa da lista com base no ID.
        // Retorna true se a tarefa foi encontrada e removida; false caso contrário.
        public bool Delete(int id)
        {
            // Busca a tarefa com o ID informado.
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            // Se não encontrar, retorna false.
            if (task == null) return false;
            // Remove a tarefa da lista.
            _tasks.Remove(task);
            return true; // Remoção bem-sucedida.
        }
    }
}
