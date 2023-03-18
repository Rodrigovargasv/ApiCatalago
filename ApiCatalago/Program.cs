using ApiCatalago.Context;
using ApiCatalago.DTOS.Mappings;
using ApiCatalago.Extensions;
using ApiCatalago.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        builder.Services.AddSwaggerGen();

        //conex�o com banco de dados
        builder.Services.AddDbContext<ApiCatalagoDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ApiCatagoDbContext"), builder => builder.MigrationsAssembly("ApiCatalago")));

        // adicionado servi�o do automapper
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // adicionando servi�o de autentica��o de usuario
        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApiCatalagoDbContext>()
            .AddDefaultTokenProviders();

        //JWT
        // adiciona o manipulador de autentica��o e define o 
        // esquema de autentica��o usando : Bearer
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

  

        var app = builder.Build();

        // adicionado servi�o de logger
        var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();

        // adcionar o a funcionalidade de logging de fomar personaliza como nivel infomation.
        loggerFactory.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
        {
            LogLevel = LogLevel.Information
        }));


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //configura middleware para capturar exce��es n�o tratadas e usar trantamento de erro de forma personalizada
            app.UseExceptionHandler("/Error");

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // adiciona o middleware de tratamento de erros de forma global
        app.ConfigureExceptionHandler();

        app.UseHttpsRedirection();

        // adiciona o middleware de autenticacao
        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}