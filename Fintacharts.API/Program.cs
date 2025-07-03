using Fintacharts.API.Transformers;
using Fintacharts.Application.Features.Forex;
using Fintacharts.Infrastructure.Options;
using Fintacharts.Infrastructure.Persistence;
using Fintacharts.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Net.Mime;
using System.Text.Json.Serialization;
using Fintacharts.Application.Common.Interfaces;
using Serilog;

namespace Fintacharts.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        builder.Host.UseSerilog(logger);

        builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerGen(c =>
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fintcharts.API", Version = "v1" }));

        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();

        // Налаштування зв'язку з секцією конфігурації
        builder.Services.Configure<ConnectionOptions>(
            builder.Configuration.GetSection(ConnectionOptions.SectionName));
        builder.Services.Configure<FintaOptions>(
            builder.Configuration.GetSection(FintaOptions.SectionName));

        // Реєстрація контексту бази даних
        builder.Services.AddDbContext<DatabaseContext>(opts =>
            opts.UseNpgsql(
                builder.Configuration.GetConnectionString("ApiDatabase")
            )
        );

        builder.Services.AddCors(o => o.AddPolicy("AllowAny", corsPolicyBuilder =>
        {
            corsPolicyBuilder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        }));

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllQuery).Assembly));

        builder.Services.AddScoped<IForex, ForexService>();
        builder.Services.AddScoped<IMarket, MarketService>();

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services
            .AddControllers(options =>
            {
                options.Filters.Add(new ProducesAttribute(MediaTypeNames.Application.Json));
                options.Conventions.Add(new RouteTokenTransformerConvention(new ToKebabParameterTransformer()));
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) app.MapOpenApi();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseCors("AllowAny");
        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}