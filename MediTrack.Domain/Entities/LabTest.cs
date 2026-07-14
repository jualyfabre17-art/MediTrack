using MediTrack.Domain.Core;
using MediTrack.Domain.Enums;

namespace MediTrack.Domain.Entities;

public class LabTest : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public LabTestStatus Status { get; set; } = LabTestStatus.Pending;
    public string Notes { get; set; } = string.Empty;

    public int PatientId { get; set; }
    public int? DoctorId { get; set; }

    public virtual Patient Patient { get; set; } = null!;
    public virtual Doctor? Doctor { get; set; }
    public virtual ICollection<LabResult> Results { get; set; } = new List<LabResult>();
}
