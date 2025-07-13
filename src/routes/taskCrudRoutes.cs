using LiteratureReviewAPI.Services;
using LiteratureReviewApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace LiteratureReviewAPI.Routes;

public static class TaskCrudRoutes
{
    // Esse método estende o tipo WebApplication

    public static void MapTaskEndpoints(this WebApplication app)
    {
        app.MapGet("/tasks", ([FromServices] ITaskRepository taskRepo) =>
        {
            var tasks = taskRepo.GetAll();
            return tasks.Any() ? Results.Ok(tasks) : Results.NotFound("No tasks found.");
        });

        app.MapGet("/tasks/{id}", (int id, [FromServices] ITaskRepository taskRepo) =>
        {
            var task = taskRepo.GetById(id);
            return task is not null ? Results.Ok(task) : Results.NotFound();
        });

        app.MapPost("/tasks", ([FromBody] UserTask task, [FromServices] TaskService taskService) =>
        {
            var response = taskService.Create(task);
            return response  ? Results.Ok() : Results.BadRequest("Failed to create task.");
        });

        app.MapPut("/tasks/{id}", (int id, [FromBody] UserTask updated, [FromServices] TaskService taskService) =>
        {
            return taskService.Update(id, updated) ? Results.Ok() : Results.NotFound();
        });

        app.MapDelete("/tasks/{id}", (int id, [FromServices] TaskService taskService) =>
        {
            var response = taskService.Delete(id);
            return response ? Results.Ok() : Results.BadRequest("Failed to delete task.");
        });
        
        app.MapPost("/completeTask/{id}", (int id, [FromServices] TaskCompletionService  taskCompletionService) =>
        {
            var response = taskCompletionService.FinishTask(id);
            return response ? Results.Ok() : Results.BadRequest("Failed to complete task.");
        });
    }

}
