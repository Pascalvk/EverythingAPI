using System;
using EverythingAPI.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MijnAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Haal de connection string op uit de environment variables
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

        builder.Services.AddSingleton(new DatabaseConfig(connectionString));


        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
        builder.Services.AddScoped<UserDAL>();
        builder.Services.AddScoped<BoardDAL>();
        builder.Services.AddScoped<ItemDAL>();
        builder.Services.AddScoped<StatusItemDAL>();

        // Add services to the container
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        /*
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        */

        // Configure the HTTP request pipeline
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}

