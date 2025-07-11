using LiteratureReviewAPI.Services;
using LiteratureReviewAPI.Routes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<TaskService>();
builder.Services.AddSingleton<ITaskRepository>(new FileTaskRepository("tasks.json"));
builder.Services.AddEndpointsApiExplorer(); // obrigatório
builder.Services.AddSwaggerGen();           // gera o Swagger
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Mapeia a interface gráfica do Swagger para ser acessada via navegador.
    app.MapOpenApi(); // Exibe a documentação interativa da API.
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Adiciona middleware para redirecionar automaticamente requisições HTTP para HTTPS.
// Isso garante conexões seguras (criptografadas).
app.UseHttpsRedirection();
app.MapTaskEndpoints();

app.Run();

// O QUE É RECORD??