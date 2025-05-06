using FieldExpenseManager.FieldExpense.Application.Cqrs.ExpenseCategories.Commands;
using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories;
using FieldExpenseManager.FieldExpense.Application.Interfaces.Repositories.UnitOfWork;
using FieldExpenseManager.FieldExpense.Application.Interfaces.ServiceInterface;
using FieldExpenseManager.FieldExpense.Application.Mapping;
using FieldExpenseManager.FieldExpense.Application.Services;
using FieldExpenseManager.FieldExpense.Application.Token;
using FieldExpenseManager.FieldExpense.Application.Validation.ExpenseCategory;
using FieldExpenseManager.FieldExpense.Infrastructure.Persistence.DbContext;
using FieldExpenseManager.FieldExpense.Infrastructure.Persistence.SeedData; 
using FieldExpenseManager.FieldExpense.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Http;
using FieldExpenseManager.FieldExpense.Infrastructure.Services;
using FieldExpenseManager.FieldExpense.Api.Middlewares;


var builder = WebApplication.CreateBuilder(args);

// --- Veritabaný Baðlantýsý ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// --- DbContext Servisi ---
builder.Services.AddDbContext<FieldExpenseDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- Diðer Servisler ---

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserDataSeeder, UserDataSeeder>();

builder.Services.AddScoped<IReportingRepository, ReportingRepository>();

builder.Services.AddValidatorsFromAssembly(typeof(CreateCategoryDtoValidator).Assembly);
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssembly(typeof(CreateCategoryCommand).Assembly));

builder.Services.AddLogging();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IPaymentService, FakeBankPaymentService>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Field Expense Manager API", Version = "v1" });
    options.MapType<IFormFile>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter JWT Bearer token **_only_**",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",

    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
               Reference=new OpenApiReference{Type=ReferenceType.SecurityScheme,Id="Bearer"},
               Scheme="oauth2",
               Name="Bearer",
               In=ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero 
        };
    });

builder.Services.AddAuthorization();



//app.UseMiddleware<ErrorHandlingMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c=>c.SwaggerEndpoint("/swagger/v1/swagger.json","Field Expense Manager API v1"));
}


app.UseErrorHandlerMiddleware();

app.UseHttpsRedirection();

app.UseStaticFiles();   

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        
        var context = services.GetRequiredService<FieldExpenseDbContext>();

      
        await context.Database.MigrateAsync();

       
        var seeder = services.GetRequiredService<IUserDataSeeder>();

       
        await seeder.SeedUserDataAsync();

        Console.WriteLine("Database migration and seeding completed successfully.");
    }
    catch (Exception ex)
    {

        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database migration or seeding.");

    }
}

app.Run();