using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using TechShop_API_backend_.Data;
using TechShop_API_backend_.Data.Context;
using TechShop_API_backend_.Models;
using TechShop_API_backend_.Service;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using TechShop_API_backend_.Data.Authenticate;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using TechShop.API.Repositories;
using TechShop_API_backend_.Helpers;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using TechShopApi.Helpers;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.WebHost.UseUrls("http://*:8080");


builder.Services.AddControllers();

// Add HttpClient factory
builder.Services.AddHttpClient();

// Add custom services
builder.Services.AddScoped<SecurityHelper>();
builder.Services.AddScoped<ConverterHelper>();
builder.Services.AddScoped<VersionHelper>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<AuthenticationRepository>();
builder.Services.AddScoped<UserDetailRepository>();
builder.Services.AddSingleton<ProductRepository>();
builder.Services.AddScoped<ReviewRepository>();
builder.Services.AddSingleton<OrderRepository>();
builder.Services.AddScoped<VerificationCodeRepository>();
builder.Services.AddScoped<AuthProviderRepository>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<AdminRepository>();
builder.Services.AddScoped<MongoMetricsService>();
builder.Services.AddScoped<UserFcmRepository>();
builder.Services.AddScoped<NotificationRepository>();
builder.Services.AddScoped<FcmService>();
builder.Services.AddSingleton<RecommendationService>();

// Add service configurations
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Register IMongoClient as Singleton
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sqlConnectionString = builder.Configuration.GetConnectionString("UserDatabase")
    ?? builder.Configuration["ConnectionString__UserDatabase"]
    ?? Environment.GetEnvironmentVariable("ConnectionString__UserDatabase");

builder.Services.AddDbContext<AuthenticateDbContext>(options =>
    options.UseSqlServer(sqlConnectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var jwtKey = builder.Configuration["JWT:Key"]
    ?? builder.Configuration["JWT__Key"]
    ?? "ThisIsASuperStrongSecretKey123456!";
var jwtIssuer = builder.Configuration["JwtConfig:Issuer"] ?? "TechShopApi";
var jwtAudience = builder.Configuration["JwtConfig:Audience"] ?? "TechShopApiUser";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.SaveToken = true;
    option.RequireHttpsMetadata = false;
    option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidAudience = jwtAudience,
        ValidIssuer = jwtIssuer,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidateIssuer = true,
        ValidateAudience = true,
    };
});



// ADD SWAGGER WITH JWT AUTHENTICATE
builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});



// ADD RATE LIMITER


builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("authenticate", opt =>
    {
        opt.PermitLimit = 4;
        opt.Window = TimeSpan.FromSeconds(12);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });
});





var app = builder.Build();
//using (var scope = app.Services.CreateScope())
//{
//    var rec = scope.ServiceProvider.GetRequiredService<RecommendationService>();
//    await RecommendationService.BuildMatrix();
//    Console.WriteLine("🔥 Matrix built at startup");
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();



// Enable rate limiting 
app.UseRateLimiter();
//authenticated rate limited endpoint 
app.MapGet("/api/Authenticate", () => "This endpoint is rate limited")
   .RequireRateLimiting("authenticate"); // Apply specific policy to an endpoint



app.Run();