using ApplicationCore.Interfaces;
using ApplicationCore.Module;
using Infrastructure.Memory;
using Infrastructure.Memory.Repositories;
using ApplicationCore.Commons.Repository;
using BackendLab01;
using Infrastructure;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddContactsModule(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddContactsEfModule(builder.Configuration);

builder.Services.AddSingleton<JwtSettings>();
builder.Services.AddJwt(new JwtSettings(builder.Configuration));

builder.Services.AddScoped<IPersonService, MemoryPersonService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();

builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();    
builder.Services.AddProblemDetails();

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseExceptionHandler();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    
    // Automatyczna migracja bazy danych przy starcie
    var dbContext = scope.ServiceProvider.GetRequiredService<Infrastructure.EF.Context.ContactsDbContext>();
    dbContext.Database.Migrate();

    var seeders = scope.ServiceProvider.GetServices<IDbSeeder>().OrderBy(s => s.Order);
    foreach (var seeder in seeders)
    {
        await seeder.SeedAsync();
    }
}

app.Run();

namespace BackendLab01
{
    public partial class Program { }
}