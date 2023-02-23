using pvp.Data;
using pvp.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<SystemDbContext>();
//Add repositories
builder.Services.AddTransient<IAdRepository, AdRepository>();
builder.Services.AddTransient<ILoggedRepository, LoggedRepository>();
builder.Services.AddTransient<IResultRepository, ResultRepository>();
builder.Services.AddTransient<ISelectedTaskRepository, SelectedTaskRepository>();
builder.Services.AddTransient<ISolutionRepository, SolutionRepository>();
builder.Services.AddTransient<ITaskRepository, TaskRepository>();
builder.Services.AddTransient<ITypeRepository, TypeRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();

//app.UseHttpsRedirection();
//app.UseAuthorization();

app.MapControllers();

app.Run();
