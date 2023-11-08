using DoctorService.AsyncDataService;
using DoctorService.Data;
using DoctorService.Data.Repositories;
using DoctorService.Entities;
using DoctorService.EventProcessing;
using DoctorService.Helppers;
using DoctorService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// --> DbContext
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("DoctorService"));

// --> Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// --> httpcontextAccesor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// --> Repositories
builder.Services.AddScoped<IBaseRepository<Speciality>, SpecialityRepository>();
builder.Services.AddScoped<IBaseRepository<Doctor>, DoctorRepository>();
builder.Services.AddScoped<IBaseRepository<User>, UserRepository>();
// --> Data services
builder.Services.AddScoped<IUserDataClient, UserDataClient>();

// --> Event Processor
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddHostedService<MessageBusSubscriber>();

builder.Services.AddHttpClient<AuthorizedFilter>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UsePathBase("/doctorService");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
PrepDb.PrepPoupulation(app, app.Environment.IsProduction());
//app.UseHttpsRedirection();

app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseAuthorization();



app.MapControllers();

app.Run();
