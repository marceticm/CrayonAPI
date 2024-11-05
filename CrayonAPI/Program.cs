using CrayonAPI.Extensions;
using CrayonAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServicesToContainer(builder.Configuration);

// Configure the HTTP request pipeline.
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
