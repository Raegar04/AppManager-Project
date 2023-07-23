using BLL.Abstractions;
using BLL.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC_First.DAL;
using MVC_First.DAL.DAL.Repositories;
using MVC_First.DAL.Repositories;
using MVC_Identity.Attributes;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IToDoTaskService, ToDoTaskService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IProjectUserService, ProjectUserService>();

builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<DisallowSimilarUsernamesAttribute>();

builder.Services.AddDbContext<AppManagerContext>();
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppManagerContext>().AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();
//builder.Services.Configure<IdentityOptions>(opt =>
//{
//    opt.Password.RequiredLength = 5;
//    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(15);
//    opt.Lockout.MaxFailedAccessAttempts = 5;
//    opt.SignIn.RequireConfirmedAccount = true;
//});


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
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
