using System.Numerics;
using MediTrack.Domain.Core;

namespace MediTrack.Domain.Entities;

public class Department : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}