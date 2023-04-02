using System.Security.Claims;
using Blog.Configuration;
using Blog.Data;
using Blog.Data.FileManager;
using Blog.Data.Repository;
using Blog.Enums;
using Blog.Models;
using Blog.Services.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuration.
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Adding DB.
builder.Services.AddDbContext<BlogDbContext>(
    o => o.UseNpgsql(builder.Configuration["ConnectionString"]));

// Adding Identity.
builder.Services.AddIdentityCore<BlogUser>(o =>
    {
        o.Password.RequiredLength = 4;
        o.Password.RequireDigit = false;
        o.Password.RequireUppercase = false;
        o.Password.RequireNonAlphanumeric = false;
        o.Password.RequiredUniqueChars = 1;

        o.SignIn.RequireConfirmedEmail = true;
    })
    .AddDefaultUI()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<BlogDbContext>();

// Adding Authentication.
builder.Services.AddAuthentication(o =>
    {
        o.DefaultScheme = IdentityConstants.ApplicationScheme;
        o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies(_ => { });

// Adding Authorization.
builder.Services.AddAuthorization(o => { o.AddPolicy("Admin", p => p.RequireClaim(ClaimTypes.Role, "admin")); });

// Cookie setup.
builder.Services.ConfigureApplicationCookie(options => { options.LoginPath = "/Auth/SignIn"; });

// Mvc things.
builder.Services.AddMvc();

// Custom services.
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IFileManager, FileManager>();
builder.Services.AddSingleton<IEmailService, EmailService>();

var app = builder.Build();

// Uncomment for Npgsql (PostgreSQL) DB provider.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Adding admin seed in DB.
var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<BlogUser>>();

context.Database.EnsureCreated();

if (!context.Users.Any(x => x.NormalizedUserName == "ADMIN@ADMIN.ADMIN"))
{
    var claims = new List<Claim>
    {
        new(ClaimTypes.Role, "admin"),
        new("FirstName", "admin"),
        new("LastName", "admin"),
        new("Sex", Sex.Unknown.ToString()),
        new("DateOfBirth", DateTime.MinValue.ToString("u"))
    };
    var user = new BlogUser
    {
        UserName = "admin@admin.admin",
        Email = "admin@admin.admin",
        EmailConfirmed = true
    };
    userManager.CreateAsync(user, "admin").GetAwaiter().GetResult();
    userManager.AddClaimsAsync(user, claims).GetAwaiter().GetResult();
}

// app.Use here.
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();

app.Run();