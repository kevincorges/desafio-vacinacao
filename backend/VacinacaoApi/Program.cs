using Microsoft.EntityFrameworkCore;
using VacinacaoApi.Data;
using VacinacaoApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Banco de dados em memória para facilitar o teste do aluno
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("VacinacaoDb"));

// Injeção de Dependência
builder.Services.AddScoped<IAgendamentoService, AgendamentoService>();

// Configuração de CORS para o Frontend React
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReact");

// app.UseHttpsRedirection(); // Removido para simplificar o ambiente de desenvolvimento local

app.UseAuthorization();

app.MapControllers();

app.Run();
