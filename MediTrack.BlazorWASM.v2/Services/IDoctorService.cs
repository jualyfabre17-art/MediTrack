using MediTrack.BlazorWASM.v2.Models;

namespace MediTrack.BlazorWASM.v2.Services;

public interface IDoctorService
{
    Task<List<DoctorModel>> GetDoctorsAsync();
    Task<DoctorModel?> GetDoctorAsync(int id);
    Task<DoctorModel?> CreateDoctorAsync(DoctorCreateModel doctor);
    Task<bool> UpdateDoctorAsync(DoctorModel doctor);
    Task<bool> DeleteDoctorAsync(int id);
}
