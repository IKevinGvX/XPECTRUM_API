using Microsoft.EntityFrameworkCore;
using xpectrum_api.data;

var builder = WebApplication.CreateBuilder(args);

// Agregar DbContext con cadena de conexión
builder.Services.AddDbContext<xpectrumContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("XpectrumDB")));

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

app.UseAuthorization();

app.MapControllers();

app.Run();
