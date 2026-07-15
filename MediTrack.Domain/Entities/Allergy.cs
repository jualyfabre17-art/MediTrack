using MediTrack.Domain.Core;

namespace MediTrack.Domain.Entities;

public class Allergy : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Symptoms { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;

    public int PatientId { get; set; }

    public virtual Patient Patient { get; set; } = null!;
}