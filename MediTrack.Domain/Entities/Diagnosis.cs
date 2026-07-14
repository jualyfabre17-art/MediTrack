using MediTrack.Domain.Core;

namespace MediTrack.Domain.Entities;

public class Diagnosis : BaseEntity
{
    public string Code { get; set; } = string.Empty; 
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}
