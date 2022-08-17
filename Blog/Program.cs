using Blog.Data;
using Blog.Data.FileManager;
using Blog.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseNpgsql(builder.Configuration["ConnectionString"]));
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 4;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredUniqueChars = 1;
    })
    .AddEntityFrameworkStores<BlogDbContext>();
builder.Services.ConfigureApplicationCookie(options => { options.LoginPath = "/Auth/Login"; });
builder.Services.AddMvc();

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IFileManager, FileManager>();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
context.Database.EnsureCreated();
if (!context.Roles.Any())
{
    // Create admin role.
    var adminRole = new IdentityRole("admin");
    roleManager.CreateAsync(adminRole).GetAwaiter().GetResult();
}

if (!context.Users.Any(x => x.UserName == "admin"))
{
    // Create an admin.
    var adminUser = new IdentityUser
    {
        UserName = "admin",
        Email = "admin@admin.com"
    };
    userManager.CreateAsync(adminUser, "admin").GetAwaiter().GetResult();
    userManager.AddToRoleAsync(adminUser, "admin");
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();

app.Run();