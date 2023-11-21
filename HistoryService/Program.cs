using HistoryService.AsyncDataService;
using HistoryService.Data;
using HistoryService.Data.Repositories;
using HistoryService.Entities;
using HistoryService.EventProcessing;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// --> DbContext
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("HistoryService"));

// --> Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// --> httpcontextAccesor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// --> Repositories
builder.Services.AddScoped<IBaseRepo<User>, UserRepo>();

// --> Event Processor
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddHostedService<MessageBusSubscriber>();


builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
