using Microsoft.EntityFrameworkCore;
using MediTrack.Infrastructure.Context;
using MediTrack.Infrastructure.Core;
using MediTrack.Domain.Interfaces;
using MediTrack.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MediTrackDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorWASM",
        builder =>
        {
            builder.WithOrigins("https://localhost:7000", "http://localhost:5000")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorWASM");
app.UseAuthorization();
app.MapControllers();

app.Run();
