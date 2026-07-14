using MediTrack.Domain.Enums;

namespace MediTrack.Application.Dtos.Appointment;

public class AppointmentCreateDto
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    public int DurationMinutes { get; set; } = 30;
    public string Reason { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}