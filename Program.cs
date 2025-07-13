using LiteratureReviewAPI.Services;
using LiteratureReviewAPI.Repositories;
using LiteratureReviewAPI.Observers;
using LiteratureReviewAPI.Facade;
using LiteratureReviewAPI.Routes;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // ESSENCIAL — fornece o ISwaggerProvider

// Seus serviços
builder.Services.AddSingleton<ITaskRepository>(new FileTaskRepository("tasks.json"));
builder.Services.AddSingleton<TaskService>();
builder.Services.AddSingleton<TaskCompletionService>();
builder.Services.AddSingleton<TaskFilterService>();
builder.Services.AddSingleton<TaskManagerFacade>();
builder.Services.AddSingleton<DependencyObserver>();

var app = builder.Build();

// Registrar Observers
var completionService = app.Services.GetRequiredService<TaskCompletionService>();
var dependencyObserver = app.Services.GetRequiredService<DependencyObserver>();
completionService.RegisterObserver(dependencyObserver);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();             // deve vir depois do Build e depois dos AddSwaggerGen
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapTaskEndpoints();

app.Run();
