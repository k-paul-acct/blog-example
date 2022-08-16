using Blog.Data;
using Blog.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseNpgsql(builder.Configuration["ConnectionString"]));
builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        options.Password.RequiredLength = 4;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredUniqueChars = 1;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BlogDbContext>();

builder.Services.AddScoped<IRepository, Repository>();

var app = builder.Build();

app.UseAuthentication();
app.MapDefaultControllerRoute();

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

app.Run();