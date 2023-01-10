using GraduateThesis.ApplicationCore.Authorization;
using GraduateThesis.ApplicationCore.Email;
using GraduateThesis.Repository.BLL.Implements;
using GraduateThesis.Repository.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
IConfigurationSection configurationSection = builder.Configuration.GetSection("SMTP");

string host = configurationSection.GetValue<string>("Host");
int port = configurationSection.GetValue<int>("Port");
string user = configurationSection.GetValue<string>("User");
string password = configurationSection.GetValue<string>("Password");

builder.Services.AddScoped<IEmailService>(e => new SmtpService(host, port, user, password));
builder.Services.AddScoped<IAccountManager>(a => new AccountManager());
builder.Services.AddScoped<IRepository>(r => new Repository(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"))));

builder.Services.AddMvc().ConfigureApplicationPartManager(apm =>
{
    ApplicationPart? applicationPart = apm.ApplicationParts
        .FirstOrDefault(part => part.Name == "DependentLibrary");

    if (applicationPart != null)
    {
        apm.ApplicationParts.Remove(applicationPart);
    }
});

builder.Services.AddSession();

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
