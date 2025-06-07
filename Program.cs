using Microsoft.EntityFrameworkCore;
using xpectrum_api.data;

var builder = WebApplication.CreateBuilder(args);

// Agregar DbContext con cadena de conexión
builder.Services.AddDbContext<xpectrumContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("XpectrumDB")));

// Configurar política CORS (puedes cambiar AllowAnyOrigin por dominios específicos)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", builder =>
    {
        builder
            .AllowAnyOrigin()    // Permite cualquier origen (para desarrollo)
            .AllowAnyMethod()    // Permite todos los métodos HTTP (GET, POST, etc)
            .AllowAnyHeader();   // Permite todas las cabeceras
    });
});

// Add services to the container.
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilitar CORS antes de MapControllers
app.UseCors("PermitirTodo");

app.UseAuthorization();

app.MapControllers();

app.Run();
