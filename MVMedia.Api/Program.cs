using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MVMedia.Api.Context;
using MVMedia.Api.DTOs.Mapping;
using MVMedia.Api.Identity;
using MVMedia.Api.Videos;
using MVMedia.Api.Repositories;
using MVMedia.Api.Repositories.Interfaces;
using MVMedia.Api.Services;
using MVMedia.Api.Services.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


//CONNECSTION STRING CONFIGURATION
var PostgreSqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseNpgsql(PostgreSqlConnection));

//DEPENDENCE INJECTION FRO REPOSITORIES AND SERVICES
//REPOSITORIES
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IMediaRepository, MediaRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMediaFileRepository, MediaFileRepository>();

//SERVICES
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IMediaSerivce, MediaService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthtenticate, AuthenticateService>();
builder.Services.AddScoped<IMediaFileService, MediaFileService>();

//ADD SERVICES AUTOMAPPER
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<EntitiesToDTOMappingProfile>());

////CONFIGURATION FOR FILE PATH UPLOAD
//builder.Services.Configure<VideoSettings>(builder.Configuration.GetSection("VideoSettings"));


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

// Carregar VideoSettings do appsettings.json
var videoSettings = builder.Configuration.GetSection("VideoSettings").Get<VideoSettings>();

builder.Services.AddSingleton(videoSettings);

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
