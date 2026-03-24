using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Projecttest.Data;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' não configurada. Defina em appsettings.Development.json ou variável de ambiente.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// wwwroot
app.UseStaticFiles();

// Pasta Sound na raiz do projeto (Projecttest/Sound) servida em /Sound
var soundPath = Path.Combine(builder.Environment.ContentRootPath, "Sound");
if (Directory.Exists(soundPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(soundPath),
        RequestPath = "/Sound"
    });
}

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
