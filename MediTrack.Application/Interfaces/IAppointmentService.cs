using MediTrack.Application.Core;
using MediTrack.Application.Dtos.Appointment;

namespace MediTrack.Application.Interfaces;

public interface IAppointmentService
{
    Task<ServiceResult<IEnumerable<AppointmentResponseDto>>> GetAllAsync();
    Task<ServiceResult<AppointmentResponseDto>> GetByIdAsync(int id);
    Task<ServiceResult<IEnumerable<AppointmentResponseDto>>> GetByPatientAsync(int patientId);
    Task<ServiceResult<IEnumerable<AppointmentResponseDto>>> GetByDoctorAsync(int doctorId);
    Task<ServiceResult<bool>> CheckAvailabilityAsync(int doctorId, DateTime dateTime, int durationMinutes);
    Task<ServiceResult<AppointmentResponseDto>> CreateAsync(AppointmentCreateDto createDto);
    Task<ServiceResult<AppointmentResponseDto>> UpdateStatusAsync(int id, string status);
    Task<ServiceResult<bool>> CancelAsync(int id);
}