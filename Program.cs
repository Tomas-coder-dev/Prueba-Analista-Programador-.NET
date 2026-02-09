using Microsoft.EntityFrameworkCore;
using Myper.Trabajadores.Web.Data;

var builder = WebApplication.CreateBuilder(args);

// MVC con vistas
builder.Services.AddControllersWithViews();

// Registro del DbContext apuntando a SQL Server
builder.Services.AddDbContext<TrabajadoresContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Endpoint simple de salud
app.MapGet("/ping", () => "pong");

// Ruta por defecto hacia el módulo de Trabajadores
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Trabajadores}/{action=Index}/{id?}");

app.Run();