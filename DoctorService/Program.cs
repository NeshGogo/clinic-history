using DoctorService.AsyncDataService;
using DoctorService.Data;
using DoctorService.Data.Repositories;
using DoctorService.Entities;
using DoctorService.EventProcessing;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// --> DbContext
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("DoctorService"));

// --> Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// --> Repositories
builder.Services.AddScoped<IBaseRepository<Speciality>, SpecialityRepository>();
builder.Services.AddScoped<IBaseRepository<Doctor>, DoctorRepository>();
builder.Services.AddScoped<IBaseRepository<User>, UserRepository>();
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
PrepDb.PrepPoupulation(app, app.Environment.IsProduction());
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
