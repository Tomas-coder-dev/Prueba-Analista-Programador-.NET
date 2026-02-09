using Microsoft.EntityFrameworkCore;
using Myper.Trabajadores.Web.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

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
app.MapGet("/ping", () => "pong");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Trabajadores}/{action=Index}/{id?}"); 

app.Run();