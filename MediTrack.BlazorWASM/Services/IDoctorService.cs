using MediTrack.BlazorWASM.Models;

namespace MediTrack.BlazorWASM.Services;

public interface IDoctorService
{
    Task<List<DoctorModel>> GetDoctorsAsync();
    Task<DoctorModel?> GetDoctorAsync(int id);
}
