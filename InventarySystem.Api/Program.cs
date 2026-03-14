using DotNetEnv;
using Microsoft.OpenApi;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration["ConnectionStrings:DefaultConnection"] =
    Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? string.Empty;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "InventarySystem API",
        Version = "v1",
        Description = "Multimodular SaaS inventory management API"
    });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "InventarySystem API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseCors();

app.MapGet("/health", () => Results.Ok(new
{
    status = "ok",
    timestamp = DateTime.UtcNow
})).WithTags("Health");

app.Run();