using ApiCatalago.Context;
using ApiCatalago.DTOS.Mappings;
using ApiCatalago.Extensions;
using ApiCatalago.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        //conexão com banco de dados
        builder.Services.AddDbContext<ApiCatalagoDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ApiCatagoDbContext"), builder => builder.MigrationsAssembly("ApiCatalago")));

        // adicionado serviço do automapper
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // adicionando serviço de autenticação de usuario
         builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApiCatalagoDbContext>()
            .AddDefaultTokenProviders();

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

        // adiciona o middleware de autenticacao
        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}