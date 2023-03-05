using pvp.Auth;
using pvp.Data;
using pvp.Data.Repositories;
using pvp.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using pvp.Data.Auth;
using Microsoft.AspNetCore.Identity;
using System.Text;
using pvp.Auth.Models;

var builder = WebApplication.CreateBuilder(args);
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddIdentity<RestUsers, IdentityRole>().AddEntityFrameworkStores<SystemDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(configureOptions: option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters.ValidAudience = builder.Configuration["JWT:ValidAudience"];
    option.TokenValidationParameters.ValidIssuer = builder.Configuration["JWT:ValidIssuer"];
    option.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]));
});


builder.Services.AddDbContext<SystemDbContext>();
//Add repositories





builder.Services.AddTransient<IAdRepository, AdRepository>();
builder.Services.AddTransient<ILoggedRepository, LoggedRepository>();
builder.Services.AddTransient<IResultRepository, ResultRepository>();
builder.Services.AddTransient<ISelectedTaskRepository, SelectedTaskRepository>();
builder.Services.AddTransient<ISolutionRepository, SolutionRepository>();
builder.Services.AddTransient<ITaskRepository, TaskRepository>();
builder.Services.AddTransient<ITypeRepository, TypeRepository>();




builder.Services.AddTransient<IJwtTokenService1, JwtTokenService>();
builder.Services.AddScoped<AuthDbSeeder>();

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy(PolicyNames.ResourceOwner, policy => policy.Requirements.Add(new ResourceOwnerRequirement()));
}
    );
builder.Services.AddSingleton<IAuthorizationHandler, ResourceOwnerAuthorizationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();

//app.UseHttpsRedirection();
//app.UseAuthorization();

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
var dbSedder = app.Services.CreateScope().ServiceProvider.GetRequiredService<AuthDbSeeder>();
await dbSedder.SeedAsync();


app.Run();
