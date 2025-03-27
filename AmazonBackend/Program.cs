using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AmazonBackend.Data;
using System.Text;
using AmazonBackend.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    // Define a policy for refunds, requiring the Admin role
    options.AddPolicy("AllowRefunds", policy => policy.RequireRole("Admin"));

    // Define a policy for accessing orders via a claim
    options.AddPolicy("CanAccessOrders", policy =>
        policy.RequireClaim("CanAccessOrders", "true"));

    // Define another refund policy (if needed) for demonstration
    options.AddPolicy("RefundPolicy", policy =>
        policy.RequireRole("Admin"));  // Only Admins can process refunds
});

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();


var app = builder.Build();

// Enable authentication & authorization
app.UseAuthentication();
app.UseAuthorization();

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
