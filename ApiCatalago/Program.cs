using ApiCatalago.Context;
using ApiCatalago.DTOS.Mappings;
using ApiCatalago.Extensions;
using ApiCatalago.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
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

        //conex�o com banco de dados
        builder.Services.AddDbContext<ApiCatalagoDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ApiCatagoDbContext"), builder => builder.MigrationsAssembly("ApiCatalago")));

        var app = builder.Build();

        // adicionado servi�o de logger
        var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();

        // adcionar o a funcionalidade de logging de fomar personaliza como nivel infomation.
        loggerFactory.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
        {
            LogLevel = LogLevel.Information
        }));

        
  
        // configurando servi�o do automapper
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        IMapper mapper = mappingConfig.CreateMapper();

        // adiciona servi�o do AutoMapper
        builder.Services.AddSingleton(mapper);


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

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}