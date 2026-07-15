using MediTrack.Domain.Core;

namespace MediTrack.Domain.Entities;

public class Medication : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ActiveIngredient { get; set; } = string.Empty;
    public string Presentation { get; set; } = string.Empty;
    public string Strength { get; set; } = string.Empty;
    public string RecommendedDosage { get; set; } = string.Empty;
    public string SideEffects { get; set; } = string.Empty;

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
