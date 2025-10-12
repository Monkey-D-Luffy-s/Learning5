using Learning5.data;
using Learning5.Models.Account;
using Learning5.services.Account;
using Learning5.services.Payments;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;

var builder = WebApplication.CreateBuilder(args);
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//await DataSeeder.SeedAdminUser(app.Services);
// Configure Serilog
if (!string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine($"Connection string: {connectionString}");
}
else
{
    throw new Exception("DB_CONNECTION_STRING not set");
}
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: connectionString,
        sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
        {
            TableName = "Learning5HRMSLogs",
            AutoCreateSqlTable = true
        })
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, Roles>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IAcount, Acount>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
});

var app = builder.Build();
await Dataseeder.SeedAdminUser(app.Services);
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
