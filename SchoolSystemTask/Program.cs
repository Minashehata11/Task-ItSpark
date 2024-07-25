using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SchoolSystemStak.DAL.Context;
using SchoolSystemStak.DAL.Models.Identity;
using SchoolSystemStak.DAL.SeedingData;
using SchoolSystemTask.BAL.Interfaces;
using SchoolSystemTask.BAL.Repositories;
using SchoolSystemTask.PL.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ShcoolDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();  
builder.Services.AddIdentity<ApplicationUser,IdentityRole>(option =>
{
    option.Password.RequireUppercase = true;
    
}).AddEntityFrameworkStores<ShcoolDbContext>()
.AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);
builder.Services.AddAutoMapper(typeof(MappingProfile));
//builder.Services.AddScoped<ILogger,Logger<>();

builder.Services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
{
    ProgressBar = true,
    PositionClass = ToastPositions.TopRight,
    PreventDuplicates = true,
    CloseButton = true

});
builder.Services.ConfigureApplicationCookie(options =>
{

    options.LoginPath = new PathString("/Account/SignIn");
    options.AccessDeniedPath = new PathString("/Account/AccessDenied");
});

builder.Services.AddAuthentication()
    .AddGoogle(options=>
    {
        options.ClientId = builder.Configuration["Auth:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Auth:Google:ClientSecret"];
    });
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
#region SeedData
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var LoggerFactory = services.GetRequiredService<ILoggerFactory>();
try
{
    var userManger = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManger = services.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedRoles.AddRoles(roleManger);
    await SeedAdmin.CreateUser(userManger);
}
catch (Exception ex)
{
    var logger = LoggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "An Error Occurred IN program in Seeding DATA");
}
#endregion
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
