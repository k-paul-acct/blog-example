using Blog.Data;
using Blog.Data.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
builder.Services.AddDbContext<BlogDbContext>(
    options => options.UseNpgsql(builder.Configuration["ConnectionString"]));

builder.Services.AddScoped<IRepository, Repository>();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.MapDefaultControllerRoute();

app.Run();