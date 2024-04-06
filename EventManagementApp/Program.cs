using EventManagementApp.Services;
using EventManagementApp.Data;
using Microsoft.EntityFrameworkCore;
using Minio;
using Microsoft.AspNetCore.Authentication.Cookies;
using EventManagementApp.Repositories;
var builder = WebApplication.CreateBuilder(args);
//Initialize minio;
builder.Services.AddMinio(client  => MinioServiceBootstrap.BuildDefaultMinioClient(client, builder.Configuration));
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Configuration.AddJsonFile("appsettings.json").AddEnvironmentVariables();
builder.Services.AddDbContext<DefaultDbContext>(options=> options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDb")));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options=>{
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.SlidingExpiration = true;
    options.AccessDeniedPath = "/Forbidden";
    options.LoginPath = "/Admin/Login";
    options.Cookie.Name = "admin_sid";
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
var app = builder.Build();
MinioServiceBootstrap.CreateDefaultBucketAndPolicy(app.Services.GetRequiredService<IMinioClient>(), app.Configuration);
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

app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");
    
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
