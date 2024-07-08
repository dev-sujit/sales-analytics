using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SalesAnalytics.API.Services;
using SalesAnalytics.Application.Services;
using SalesAnalytics.Core.Interface;
using SalesAnalytics.Core.Interfaces;
using SalesAnalytics.Core.Middlewares;
using SalesAnalytics.Infrastructure;
using SalesAnalytics.Infrastructure.Data;
using SalesAnalytics.Infrastructure.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sales Analytics API", Version = "v1" });

    // Add JWT Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] { }
    }});
});

// Configure Entity Framework and SQL Server
builder.Services.AddDbContext<ISalesDbContext, SalesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddScoped<IApiLogService, ApiLogService>();
// Register repository and unit of work
builder.Services.AddScoped<ISaleRecordRepository, SaleRecordRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register services
builder.Services.AddScoped<SaleRecordService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
//builder.Services.AddScoped<IConfiguration>();
builder.Services.AddScoped<ISalesDbContext, SalesDbContext>();
builder.Services.AddScoped<ICommonResponseService, CommonResponseService>();

// Configure JWT Authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Add controllers
builder.Services.AddControllers();

// Add AutoMapper (optional, for DTOs)
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

//app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sales Analytics API v1");
    });
}

app.UseHttpsRedirection();

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
