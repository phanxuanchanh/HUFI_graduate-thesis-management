using GraduateThesis.ApplicationCore.AppSettings;
using GraduateThesis.ApplicationCore.Authorization;
using GraduateThesis.ApplicationCore.Context;
using GraduateThesis.ApplicationCore.Email;
using GraduateThesis.Repository.BLL.Implements;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;

#nullable disable

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connectionString = builder.Configuration.GetConnectionString("DbConnection");

builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContextPool<HufiGraduateThesisContext>(options =>
    options.UseSqlServer(connectionString));

IConfigurationSection smtpSection = builder.Configuration.GetSection("SMTP");

string host = smtpSection.GetValue<string>("Host");
int port = smtpSection.GetValue<int>("Port");
string user = smtpSection.GetValue<string>("User");
string password = smtpSection.GetValue<string>("Password");

builder.Services.AddScoped<IEmailService>(e => new SmtpService(host, port, user, password));
builder.Services.AddScoped(typeof(IAccountManager), typeof(AccountManager));
builder.Services.AddScoped(typeof(IRepository), typeof(Repository));

builder.Services.AddMvc().ConfigureApplicationPartManager(apm =>
{
    ApplicationPart applicationPart = apm.ApplicationParts
        .FirstOrDefault(part => part.Name == "DependentLibrary");

    if (applicationPart != null)
    {
        apm.ApplicationParts.Remove(applicationPart);
    }
});

builder.Services.AddSession();

AppDefaultValue.ConnectionString = connectionString;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
