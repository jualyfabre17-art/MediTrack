using MediTrack.Domain.Entities;

namespace MediTrack.Domain.Interfaces;

public interface IDoctorRepository : IRepository<Doctor>
{
    Task<IEnumerable<Doctor>> GetDoctorsWithDetailsAsync();
}