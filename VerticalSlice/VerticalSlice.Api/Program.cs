using FluentValidation;
using Microsoft.EntityFrameworkCore;
using VerticalSlice.Api.Behaviors;
using VerticalSlice.Api.Common;
using VerticalSlice.Api.Extensions;
using VerticalSlice.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AddressDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddEndpoints(typeof(Program).Assembly);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "Vertical Slice Architecture API", 
        Version = "v1",
        Description = "A Vertical Slice Architecture implementation with CQRS using Minimal APIs and FluentValidation"
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vertical Slice API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.Run();
