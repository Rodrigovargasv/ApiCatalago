using ApiCatalago.Context;
using ApiCatalago.DTOS.Mappings;
using ApiCatalago.Extensions;
using ApiCatalago.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Text;
using System.Text.Json.Serialization;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers().AddJsonOptions(option =>
        option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>{
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "APICatalogo", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Header de autorização JWT usando o esquema Bearer. \r\n\r\nInforme 'Bearer' e o seu token. \r\n\r\nExamplo: \'Bearer 1234abcdef\'"

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
                    new string[] { }
                }
            });
        });

        //conexão com banco de dados
        builder.Services.AddDbContext<ApiCatalagoDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ApiCatagoDbContext"), builder => builder.MigrationsAssembly("ApiCatalago")));

        // adicionado serviço do automapper
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // adicionando serviço de autenticação de usuario
        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApiCatalagoDbContext>()
            .AddDefaultTokenProviders();

        //JWT
        // adiciona o manipulador de autenticação e define o 
        // esquema de autenticação usando : Bearer
        // valida o emissor, e audiencia e a chave 
        // usando a chave secreta valida a assinatura
        builder.Services.AddAuthentication
        (
            JwtBearerDefaults.AuthenticationScheme).
            AddJwtBearer(options =>
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience =  true,
                ValidateLifetime = true,
                ValidAudience = builder.Configuration["TokenConfiguration:Audience"],
                ValidIssuer = builder.Configuration["TokenConfiguration:Issuer"],
                ValidateIssuerSigningKey= true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            }
        );

        // adicionando serviço de versionamento
        builder.Services.AddApiVersioning(option =>
        {
            option.AssumeDefaultVersionWhenUnspecified = true;
            option.DefaultApiVersion = new ApiVersion(1,0);
            option.ReportApiVersions = true;
        });
    
        var app = builder.Build();

        // adicionado serviço de logger
        var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();

        // adcionar o a funcionalidade de logging de fomar personaliza como nivel infomation.
        loggerFactory.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
        {
            LogLevel = LogLevel.Information
        }));


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //configura middleware para capturar exceções não tratadas e usar trantamento de erro de forma personalizada
            app.UseExceptionHandler("/Error");

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // adiciona o middleware de tratamento de erros de forma global
        app.ConfigureExceptionHandler();

        app.UseHttpsRedirection();

        app.UseRouting();

        // adiciona o middleware de autenticacao
        app.UseAuthentication();
        app.UseAuthorization();
      
        app.MapControllers();
        app.Run();
    }
}