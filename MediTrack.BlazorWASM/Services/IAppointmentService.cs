using MediTrack.BlazorWASM.Models;

namespace MediTrack.BlazorWASM.Services;

public interface IAppointmentService
{
    Task<List<AppointmentModel>> GetAppointmentsAsync();
    Task<AppointmentModel?> GetAppointmentAsync(int id);
    Task<AppointmentModel?> CreateAppointmentAsync(CreateAppointmentModel appointment);
    Task<bool> CancelAppointmentAsync(int id);
}