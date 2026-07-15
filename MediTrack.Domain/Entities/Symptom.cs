using MediTrack.Domain.Core;

namespace MediTrack.Domain.Entities;

public class Symptom : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }

    public int MedicalRecordId { get; set; }

    public virtual MedicalRecord MedicalRecord { get; set; } = null!;
}