using LiteratureReviewAPI.Services;
using LiteratureReviewAPI.Routes;

// Cria um *builder* para configurar a aplicação Web antes de ela ser iniciada.
// Esse builder configura serviços, middlewares e opções de ambiente.
// O método `CreateBuilder(args)` analisa os argumentos de linha de comando (se houver) e carrega configurações padrão.
var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao container de injeção de dependência (Dependency Injection).
// Que container é esse? // É o container (ou objeto) padrão do ASP.NET Core, que permite registrar serviços que podem ser injetados em controladores, middlewares, etc.
// Em vez de você mesmo instanciar classes manualmente com new, você registra os tipos no container, e depois o próprio framework os injeta automaticamente onde forem necessários (em controllers, serviços, etc.).

// No caso, está adicionando o serviço de documentação OpenAPI/Swagger.
// O método `AddOpenApi` é uma extensão fornecida por algum pacote instalado (provavelmente `Swashbuckle.AspNetCore` ou `Microsoft.AspNetCore.OpenApi`).
// Isso permite que a API gere automaticamente a documentação Swagger/OpenAPI.
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer(); // obrigatório
builder.Services.AddSwaggerGen();           // gera o Swagger

builder.Services.AddSingleton<TaskService>();

// Constrói a aplicação com base no que foi configurado no `builder`.
// Isso instancia a classe `WebApplication`, que representa o pipeline HTTP da aplicação.
var app = builder.Build();

// Define o comportamento da aplicação quando estiver em ambiente de desenvolvimento (Development).
// O `app.Environment` expõe propriedades como `IsDevelopment`, `IsProduction`, etc.
// Aqui, se o ambiente for "Development", ele ativa o endpoint da documentação Swagger.
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


// Inicia a aplicação Web.
// A partir daqui, o servidor começa a escutar requisições HTTP.
app.Run();

// O QUE É RECORD??