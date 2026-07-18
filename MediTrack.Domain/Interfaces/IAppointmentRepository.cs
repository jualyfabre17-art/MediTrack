using MediTrack.Domain.Entities;

namespace MediTrack.Domain.Interfaces;

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetAppointmentsWithDetailsAsync();
}
