using Microsoft.AspNetCore.Diagnostics;
using ContactsApp.API.Configurations;
using System.Diagnostics.CodeAnalysis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.ServiceCollection(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
        builder.WithHeaders("Content-Type");
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler(a => a.Run(async context =>
{
    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
    var exception = exceptionHandlerPathFeature.Error;

    await context.Response.WriteAsJsonAsync(new { error = exception.Message });
}));

app.MapControllers();
app.UseCors("EnableCORS");
app.Run();

[ExcludeFromCodeCoverage]
public partial class Program()
{ }