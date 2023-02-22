using pvp.Data;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<SystemDbContext>();




var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();

//app.UseHttpsRedirection();
//app.UseAuthorization();

app.MapControllers();

app.Run();
