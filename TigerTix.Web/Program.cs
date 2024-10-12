using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;
using TigerTix.Web.Data;
using TigerTix.Web.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddDbContext<TigerTixContext>(cfg =>
{
    cfg.UseSqlServer();
});
builder.Services.AddRazorPages();
builder.Services.AddControllers();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.MapGet("/", () => Results.Redirect("/App"));
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=App}/{action=Index}/{id?}");

app.UseDefaultFiles();

app.Run();
