using Microsoft.EntityFrameworkCore;
using MediTrack.Domain.Entities;
using MediTrack.Domain.Interfaces;
using MediTrack.Infrastructure.Context;
using MediTrack.Infrastructure.Core;

namespace MediTrack.Infrastructure.Repositories;

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(int doctorId);
    Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId);
    Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime dateTime, int durationMinutes);
}

public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(MediTrackDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(int doctorId)
    {
        return await _dbSet
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Where(a => a.DoctorId == doctorId)
            .OrderBy(a => a.AppointmentDateTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId)
    {
        return await _dbSet
            .Include(a => a.Doctor)
            .ThenInclude(d => d.Specialty)
            .Where(a => a.PatientId == patientId)
            .OrderByDescending(a => a.AppointmentDateTime)
            .ToListAsync();
    }

    public async Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime dateTime, int durationMinutes)
    {
        var endTime = dateTime.AddMinutes(durationMinutes);
        var existingAppointments = await _dbSet
            .Where(a => a.DoctorId == doctorId &&
                        a.Status != Domain.Enums.AppointmentStatus.Cancelled)
            .ToListAsync();

        var hasConflict = existingAppointments.Any(a =>
            (dateTime >= a.AppointmentDateTime && dateTime < a.AppointmentDateTime.AddMinutes(a.DurationMinutes)) ||
            (endTime > a.AppointmentDateTime && endTime <= a.AppointmentDateTime.AddMinutes(a.DurationMinutes)) ||
            (dateTime <= a.AppointmentDateTime && endTime >= a.AppointmentDateTime.AddMinutes(a.DurationMinutes)));

        return !hasConflict;
    }
}
