using MediTrack.Application.Core;
using MediTrack.Application.Dtos.Appointment;
using MediTrack.Application.Interfaces;
using MediTrack.Domain.Entities;
using MediTrack.Domain.Enums;
using MediTrack.Domain.Interfaces;

namespace MediTrack.Application.Services;

public class AppointmentService : BaseService, IAppointmentService
{
    public AppointmentService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<ServiceResult<IEnumerable<AppointmentResponseDto>>> GetAllAsync()
    {
        try
        {
            var appointments = await _unitOfWork.AppointmentRepo.GetAppointmentsWithDetailsAsync();
            var response = appointments.Select(a => MapToResponseDto(a));
            return ServiceResult<IEnumerable<AppointmentResponseDto>>.Success(response);
        }
        catch (Exception ex)
        {
            return HandleError<IEnumerable<AppointmentResponseDto>>($"Error retrieving appointments: {ex.Message}");
        }
    }

    public async Task<ServiceResult<AppointmentResponseDto>> GetByIdAsync(int id)
    {
        try
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
            if (appointment == null)
                return ServiceResult<AppointmentResponseDto>.NotFound("Appointment not found");

            return ServiceResult<AppointmentResponseDto>.Success(MapToResponseDto(appointment));
        }
        catch (Exception ex)
        {
            return HandleError<AppointmentResponseDto>($"Error retrieving appointment: {ex.Message}");
        }
    }

    public async Task<ServiceResult<IEnumerable<AppointmentResponseDto>>> GetByPatientAsync(int patientId)
    {
        try
        {
            var appointments = await _unitOfWork.Appointments
                .FindAsync(a => a.PatientId == patientId);
            var response = appointments.Select(a => MapToResponseDto(a));
            return ServiceResult<IEnumerable<AppointmentResponseDto>>.Success(response);
        }
        catch (Exception ex)
        {
            return HandleError<IEnumerable<AppointmentResponseDto>>($"Error retrieving appointments by patient: {ex.Message}");
        }
    }

    public async Task<ServiceResult<IEnumerable<AppointmentResponseDto>>> GetByDoctorAsync(int doctorId)
    {
        try
        {
            var appointments = await _unitOfWork.Appointments
                .FindAsync(a => a.DoctorId == doctorId);
            var response = appointments.Select(a => MapToResponseDto(a));
            return ServiceResult<IEnumerable<AppointmentResponseDto>>.Success(response);
        }
        catch (Exception ex)
        {
            return HandleError<IEnumerable<AppointmentResponseDto>>($"Error retrieving appointments by doctor: {ex.Message}");
        }
    }

    public async Task<ServiceResult<bool>> CheckAvailabilityAsync(int doctorId, DateTime dateTime, int durationMinutes)
    {
        try
        {
            var appointments = await _unitOfWork.Appointments
                .FindAsync(a => a.DoctorId == doctorId &&
                                a.Status != AppointmentStatus.Cancelled);

            var endTime = dateTime.AddMinutes(durationMinutes);

            var hasConflict = appointments.Any(a =>
                (dateTime >= a.AppointmentDateTime && dateTime < a.AppointmentDateTime.AddMinutes(a.DurationMinutes)) ||
                (endTime > a.AppointmentDateTime && endTime <= a.AppointmentDateTime.AddMinutes(a.DurationMinutes)) ||
                (dateTime <= a.AppointmentDateTime && endTime >= a.AppointmentDateTime.AddMinutes(a.DurationMinutes)));

            return ServiceResult<bool>.Success(!hasConflict);
        }
        catch (Exception ex)
        {
            return HandleError<bool>($"Error checking availability: {ex.Message}");
        }
    }

    public async Task<ServiceResult<AppointmentResponseDto>> CreateAsync(AppointmentCreateDto createDto)
    {
        try
        {
            var availabilityResult = await CheckAvailabilityAsync(createDto.DoctorId, createDto.AppointmentDateTime, createDto.DurationMinutes);
            if (!availabilityResult.Data)
                return ServiceResult<AppointmentResponseDto>.Conflict("Doctor is not available at this time");

            var appointment = new Appointment
            {
                PatientId = createDto.PatientId,
                DoctorId = createDto.DoctorId,
                AppointmentDateTime = createDto.AppointmentDateTime,
                DurationMinutes = createDto.DurationMinutes,
                Reason = createDto.Reason,
                Notes = createDto.Notes,
                Status = AppointmentStatus.Scheduled
            };

            await _unitOfWork.Appointments.AddAsync(appointment);
            await _unitOfWork.CompleteAsync();

            return ServiceResult<AppointmentResponseDto>.Success(MapToResponseDto(appointment), "Appointment created successfully");
        }
        catch (Exception ex)
        {
            return HandleError<AppointmentResponseDto>($"Error creating appointment: {ex.Message}");
        }
    }

    public async Task<ServiceResult<AppointmentResponseDto>> UpdateStatusAsync(int id, string status)
    {
        try
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
            if (appointment == null)
                return ServiceResult<AppointmentResponseDto>.NotFound("Appointment not found");

            if (Enum.TryParse<AppointmentStatus>(status, true, out var newStatus))
            {
                appointment.Status = newStatus;
                appointment.UpdatedAt = DateTime.UtcNow;

                switch (newStatus)
                {
                    case AppointmentStatus.Confirmed:
                        appointment.ConfirmedAt = DateTime.UtcNow;
                        break;
                    case AppointmentStatus.Completed:
                        appointment.CompletedAt = DateTime.UtcNow;
                        break;
                    case AppointmentStatus.Cancelled:
                        appointment.CancelledAt = DateTime.UtcNow;
                        break;
                }

                await _unitOfWork.Appointments.UpdateAsync(appointment);
                await _unitOfWork.CompleteAsync();

                return ServiceResult<AppointmentResponseDto>.Success(MapToResponseDto(appointment), $"Appointment status updated to {status}");
            }

            return ServiceResult<AppointmentResponseDto>.Error("Invalid status value");
        }
        catch (Exception ex)
        {
            return HandleError<AppointmentResponseDto>($"Error updating appointment status: {ex.Message}");
        }
    }

    public async Task<ServiceResult<bool>> CancelAsync(int id)
    {
        try
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
            if (appointment == null)
                return ServiceResult<bool>.NotFound("Appointment not found");

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.CancelledAt = DateTime.UtcNow;
            appointment.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Appointments.UpdateAsync(appointment);
            await _unitOfWork.CompleteAsync();

            return ServiceResult<bool>.Success(true, "Appointment cancelled successfully");
        }
        catch (Exception ex)
        {
            return HandleError<bool>($"Error cancelling appointment: {ex.Message}");
        }
    }


    

    private static AppointmentResponseDto MapToResponseDto(Appointment appointment)
    {
        return new AppointmentResponseDto
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            PatientFullName = appointment.Patient?.FullName ?? string.Empty,
            DoctorId = appointment.DoctorId,
            DoctorFullName = appointment.Doctor?.FullName ?? string.Empty,
            Specialty = appointment.Doctor?.Specialty?.Name ?? string.Empty,
            AppointmentDateTime = appointment.AppointmentDateTime,
            DurationMinutes = appointment.DurationMinutes,
            Status = appointment.Status.ToString(),
            Reason = appointment.Reason,
            Notes = appointment.Notes,
            CreatedAt = appointment.CreatedAt
        };
    }
}