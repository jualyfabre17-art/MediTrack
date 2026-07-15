using MediTrack.Domain.Core;

namespace MediTrack.Domain.Entities;

public class Doctor : Person
{
    public string LicenseNumber { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public string OfficeLocation { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public int DepartmentId { get; set; }
    public int SpecialtyId { get; set; }

    public virtual Department Department { get; set; } = null!;
    public virtual Specialty Specialty { get; set; } = null!;
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}