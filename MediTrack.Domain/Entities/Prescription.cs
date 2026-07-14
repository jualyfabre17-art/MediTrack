using MediTrack.Domain.Core;

namespace MediTrack.Domain.Entities;

public class Prescription : BaseEntity
{
    public DateTime IssueDate { get; set; }
    public string Indications { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public int DurationDays { get; set; }

    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int MedicalRecordId { get; set; }
    public int MedicationId { get; set; }

    public virtual Patient Patient { get; set; } = null!;
    public virtual Doctor Doctor { get; set; } = null!;
    public virtual MedicalRecord MedicalRecord { get; set; } = null!;
    public virtual Medication Medication { get; set; } = null!;
}