using MediTrack.Domain.Core;

namespace MediTrack.Domain.Entities;

public class LabResult : BaseEntity
{
    public string Value { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public string NormalRange { get; set; } = string.Empty;
    public string Interpretation { get; set; } = string.Empty;
    public DateTime ResultDate { get; set; }

    public int LabTestId { get; set; }

    public virtual LabTest LabTest { get; set; } = null!;
}
