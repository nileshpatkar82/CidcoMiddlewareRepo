using Serilog;
using Microsoft.AspNetCore.DataProtection;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Cidco.Middleware.Application;
using Cidco.Middleware.Application.Contracts;
using Cidco.Middleware.API.Services;
using Cidco.Middleware.Persistense;
using Cidco.Middleware.Application.Contracts.Infrastructure;
using Cidco.Middleware.Infrastructure.Token;
using Cidco.Middleware.Application.Features.LogInformation;
using Cidco.Middleware.Domain;


var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

//Test Check In
IConfiguration configurationBuilder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    //.AddJsonFile(
    //    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
    //    optional: true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configurationBuilder)
    .CreateBootstrapLogger().Freeze();

new LoggerConfiguration()
    .ReadFrom.Configuration(configurationBuilder)
    .CreateLogger();

builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

IConfiguration Configuration = builder.Configuration;

var services = builder.Services;

//services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
services.AddApplicationServices();
services.AddScoped<ILoggedInUserService, LoggedInUserService>();
services.AddScoped<ITokenService,TokenService>();
services.AddScoped<ILogInformationService, LogInformationService>();
services.AddPersistenceServices(Configuration);
services.AddControllers();
services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"bin\debug\configuration"));


builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("V1", new OpenApiInfo
    {
        Version = "V1",
        Title = "WebAPI",
        Description = "Cidco Middleware WebAPI"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                }
            },
            new List < string > ()
        }
    });
});

builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["JwtSettings:Issuer"],
        ValidAudience = configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
    };
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<CidcoMiddlewareDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("EODBConnectString"));
});
builder.Services.AddAuthorization();
// Add distributed memory cache (or another distributed cache implementation)
builder.Services.AddDistributedMemoryCache();

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(3); // Set session timeout
    //options.Cookie.HttpOnly = true; // Make the session cookie HTTP-only
    options.Cookie.IsEssential = true; // Ensure the cookie is essential for GDPR compliance
});
services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    // app.UseSwaggerUI();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/V1/swagger.json", "Cidco Middleware WebAPI");
    });
}


app.UseHttpsRedirection();
app.UseSession();
// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.Run();
