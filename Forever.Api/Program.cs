using Forever.Api.Authentication;
using Forever.Api.Configuration;
using Forever.Api.Extensions;
using Forever.Api.Interfaces.AuthService;
using Forever.Api.Interfaces.Product;
using Forever.Api.Interfaces.User;
using Forever.Api.Middleware;
using Forever.Api.Repositories.User;
using Forever.Api.Services.AuthService;
using Forever.Api.Services.Product;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Identity;
using Forever.Api.Models.User;
using Microsoft.AspNetCore.HttpOverrides;
using AuthDemo.Api.Data;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

#region Serilog
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .MinimumLevel.Information()
        .WriteTo.Console()
        .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day, shared: true);
});
#endregion

#region Services
builder.Services.AddControllers();

// Add FluentValidation auto-validation integration
builder.Services.AddFluentValidationAutoValidation();

// Register validators from this assembly (scans your Validators folder)
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS
builder.Services.AddReactCors(builder.Configuration);

// JWT settings + authentication
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddJwtAuthentication(builder.Configuration);

// Authorization policies
builder.Services.AddPolicyAuthorization();

// Swagger
builder.Services.AddSwaggerDocumentation();

// Rate limiting
builder.Services.AddAppRateLimiting();

// DI registrations
builder.Services.AddScoped<IPasswordHasher<Users>, PasswordHasher<Users>>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
#endregion

var app = builder.Build();

#region Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// If behind proxy/load balancer, enable forwarded headers so rate limiting by IP is accurate
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Rate limiting middleware - early in pipeline
app.UseRateLimiter();

// Security and app middleware
app.UseHttpsRedirection();
app.UseCors(CorsExtension.ReactCorsPolicy);

// Exception handling middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
#endregion

app.Run();