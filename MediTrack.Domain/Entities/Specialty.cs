using MediTrack.Domain.Core;

namespace MediTrack.Domain.Entities;

public class Specialty : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
