using MediTrack.Domain.Core;
using MediTrack.Domain.Enums;

namespace MediTrack.Domain.Entities;

public class Appointment : BaseEntity
{
    public DateTime AppointmentDateTime { get; set; }
    public int DurationMinutes { get; set; } = 30;
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    public string Reason { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? CancelledAt { get; set; }

    public int PatientId { get; set; }
    public int DoctorId { get; set; }

    public virtual Patient Patient { get; set; } = null!;
    public virtual Doctor Doctor { get; set; } = null!;
}
