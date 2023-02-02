using ApiCatalago.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(option =>
option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//conec��o com banco de dados
builder.Services.AddDbContext<ApiCatalagoDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ApiCatagoDbContext"), builder => builder.MigrationsAssembly("ApiCatalago")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //configura middleware para capturar exce��es n�o tratadas e usar trantamento de erro de forma personalizada
    app.UseExceptionHandler("/Error");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
