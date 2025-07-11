using LiteratureReviewAPI.Services;
using LiteratureReviewApi.Models;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LiteratureReviewAPI.Routes;

public static class TaskCrudRoutes
{
    // Esse método estende o tipo WebApplication
    
    public static void MapTaskEndpoints(this WebApplication app)
    {
        app.MapGet("/tasks", ([FromServices] TaskService  repo) =>
        {
            return Results.Ok(repo.GetAll());
        });

        app.MapGet("/tasks/{id}", (int id, [FromServices] TaskService  repo) =>
        {
            var task = repo.GetById(id);
            return task is not null ? Results.Ok(task) : Results.NotFound();
        });

        app.MapPost("/tasks", ([FromBody] UserTaskWithStatus task, [FromServices] TaskService  repo) =>
        {
            var created = repo.Create(task);
            return Results.Created($"/tasks/{created.Id}", created);
        });

        app.MapPut("/tasks/{id}", (int id, [FromBody] UserTaskWithStatus updated, [FromServices] TaskService  repo) =>
        {
            return repo.Update(id, updated) 
                ? Results.Ok() 
                : Results.NotFound();
        });

        app.MapDelete("/tasks/{id}", (int id, [FromServices] TaskService  repo) =>
        {
            return repo.Delete(id) 
                ? Results.Ok() 
                : Results.NotFound();
        });
    }

}
