namespace MediTrack.Domain.Core;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    protected BaseEntity()
    {
        CreatedAt = DateTime.UtcNow;
    }
}
