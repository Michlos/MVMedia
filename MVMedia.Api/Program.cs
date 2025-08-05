using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MVMedia.Api.Context;
using Microsoft.EntityFrameworkCore;
using MVMedia.Api.Repositories.Interfaces;
using MVMedia.Api.Repositories;
using MVMedia.Api.DTOs.Mapping;
using MVMedia.Api.Services;
using MVMedia.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


//CONNECSTION STRING CONFIGURATION
var PostgreSqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseNpgsql(PostgreSqlConnection));

//DEPENDENCE INJECTION FRO REPOSITORIES
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IMediaRepository, MediaRepository>();
builder.Services.AddScoped<IClientService, ClientService>();

//ADD SERVICES AUTOMAPPER
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<EntitiesToDTOMappingProfile>());


//START configuration for JWT authentication

//BUSCANDO DADOS DO appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");


// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MVMedia API", Version = "v1" });

    //configuração para autenticação JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insita o token JWT no formato: Bearer {seu token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
//END of JWT authentication configuration



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
