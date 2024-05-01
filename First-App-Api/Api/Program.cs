using Api.Context;
using Api.Interfaces;
using Api.Middlewares;
using Api.Models.DTOs.CardDTOs;
using Api.Models.DTOs.TaskListDTOs;
using Api.Repositories;
using Api.Services;
using Api.Validations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IValidator<CreateUpdateCardDto>, CreateUpdateCardValidator>();
builder.Services.AddScoped<IValidator<CreateTaskListDto>, CreateUpdateTaskListValidator>();

builder.Services.AddScoped<ICardRepository, CardRepository>(); 
builder.Services.AddScoped<IHistoryRepository, HistoryRepository>();
builder.Services.AddScoped<ITaskListRepository, TaskListRepository>();

builder.Services.AddTransient<IHistoryService, HistoryService>();


builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
});

app.Run();
