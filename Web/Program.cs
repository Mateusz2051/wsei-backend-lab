using ApplicationCore.Interfaces;
using ApplicationCore.Module;
using Infrastructure.Memory;
using Infrastructure.Memory.Repositories;
using ApplicationCore.Commons.Repository;
using ApplicationCore.Models;
using ApplicationCore.Models.QuizAggregate;
using BackendLab01;
using Infrastructure;
using Infrastructure.Security;

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

builder.Services.AddSingleton<IGenericRepository<Quiz, int>, MemoryGenericRepository<Quiz, int>>();
builder.Services.AddSingleton<IGenericRepository<QuizItem, int>, MemoryGenericRepository<QuizItem, int>>();
builder.Services.AddSingleton<IGenericRepository<QuizItemUserAnswer, string>, MemoryGenericRepository<QuizItemUserAnswer, string>>();
builder.Services.AddSingleton<IQuizUserService, QuizUserService>();

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

app.Seed();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var seeders = scope.ServiceProvider.GetServices<IDbSeeder>().OrderBy(s => s.Order);
    foreach (var seeder in seeders)
    {
        await seeder.SeedAsync();
    }
}

app.Run();