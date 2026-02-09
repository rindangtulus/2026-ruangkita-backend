using Microsoft.EntityFrameworkCore;
using _2026_ruangkita_backend.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlite("Data Source=ruangkita.db"));

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin() // Mengizinkan dari alamat mana saja (cocok untuk tahap belajar)
                  .AllowAnyMethod() // Mengizinkan GET, POST, PUT, DELETE, PATCH
                  .AllowAnyHeader(); // Mengizinkan header apa saja
        });
});

var app = builder.Build();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run();