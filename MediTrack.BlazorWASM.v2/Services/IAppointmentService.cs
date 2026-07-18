using MediTrack.BlazorWASM.v2.Models;

namespace MediTrack.BlazorWASM.v2.Services;

public interface IAppointmentService
{
    Task<List<AppointmentModel>> GetAppointmentsAsync();
    Task<AppointmentModel?> GetAppointmentAsync(int id);
    Task<AppointmentModel?> CreateAppointmentAsync(AppointmentCreateModel appointment);
    Task<bool> CancelAppointmentAsync(int id);
}