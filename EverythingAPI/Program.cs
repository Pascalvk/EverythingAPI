using System;
using System.Threading.RateLimiting;
using EverythingAPI.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
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

        builder.Services.AddRateLimiter(_ => _
            .AddFixedWindowLimiter(policyName: "fixed", options =>
            {
                options.PermitLimit = 4;
                options.Window = TimeSpan.FromSeconds(12);
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = 2;
            }));

        var app = builder.Build();

        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        

        // Configure the HTTP request pipeline
        app.UseHttpsRedirection();
        app.UseRateLimiter();
        app.UseAuthorization();
        app.MapControllers();



        app.Run();
    }
}

