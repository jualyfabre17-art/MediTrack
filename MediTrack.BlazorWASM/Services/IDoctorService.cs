using MediTrack.BlazorWASM.Models;

namespace MediTrack.BlazorWASM.Services;

public interface IDoctorService
{
    Task<List<DoctorModel>> GetDoctorsAsync();
    Task<DoctorModel?> GetDoctorAsync(int id);
    Task<DoctorModel?> CreateDoctorAsync(DoctorCreateModel doctor);
    Task<bool> UpdateDoctorAsync(DoctorUpdateModel doctor);
}
