using MediTrack.Domain.Core;

namespace MediTrack.Domain.Entities;

public class MedicalRecord : BaseEntity
{
    public DateTime RecordDate { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public string Observations { get; set; } = string.Empty;

    public int PatientId { get; set; }
    public int? DoctorId { get; set; }

    public virtual Patient Patient { get; set; } = null!;
    public virtual Doctor? Doctor { get; set; }
    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    public virtual ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();
    public virtual ICollection<Symptom> Symptoms { get; set; } = new List<Symptom>();
}