using Backend_Service.Application.Interfaces;
using Backend_Service.Application.Mappers;
using Backend_Service.Application.Services;
using Backend_Service.Infrastructure.Contexts;
using Backend_Service.Infrastructure.Interfaces;
using Backend_Service.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<ISegmentationRepository, SegmentationRepository>();
builder.Services.AddScoped<ISegmentationService, SegmentationService>();
builder.Services.AddScoped<ILabelRepository, LabelRepository>();
builder.Services.AddScoped<ILabelService, LabelService>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddControllers();

if (File.Exists("../.env"))
{
    foreach (var line in File.ReadAllLines("../.env"))
    {
        if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
            continue;

        var parts = line.Split("=");
        if (parts.Length == 2)
        {
            if (parts[0].Contains("POSTGRES_HOST"))
            {
                continue;
            }
            Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
        }
    }
}

string username = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "";
string password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "";
string database = Environment.GetEnvironmentVariable("POSTGRES_DATABASE") ?? "";
string dbHost = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";

builder.Services.AddDbContext<ImageContext>(options =>
    options.UseNpgsql($"Host={dbHost};Database={database};Username={username};Password={password}"));

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();
