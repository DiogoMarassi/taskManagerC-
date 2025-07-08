using LiteratureReviewAPI.Services;
using LiteratureReviewApi.Models;

namespace LiteratureReviewAPI.Routes;

public static class TaskCrudRoutes
{
    // Esse método estende o tipo WebApplication
    
    public static void MapTaskEndpoints(this WebApplication app)
    {
        //var taskService = new TaskService(); // Se não fossemos injetar o serviço, instanciaríamos assim. (não recomendado)
        // Repare que eu não instanciei o TaskService aqui, porque ele será injetado pelo ASP.NET Core.

        app.MapGet("/tasks", (TaskService taskService) =>
        {
            return Results.Ok(taskService.GetAll());
        });

        app.MapGet("/tasks/{id}", (int id, TaskService taskService) =>
        {
            var task = taskService.GetById(id);
            return task is not null ? Results.Ok(task) : Results.NotFound();
        });

        app.MapPost("/tasks", (UserTask task, TaskService taskService) =>
        {
            var created = taskService.Create(task);
            return Results.Created($"/tasks/{created.Id}", created);
        });

        app.MapPut("/tasks/{id}", (int id, UserTask updated, TaskService taskService) =>
        {
            return taskService.Update(id, updated) 
                ? Results.Ok() 
                : Results.NotFound();
        });

        app.MapDelete("/tasks/{id}", (int id, TaskService taskService) =>
        {
            return taskService.Delete(id) 
                ? Results.Ok() 
                : Results.NotFound();
        });
    }

}
