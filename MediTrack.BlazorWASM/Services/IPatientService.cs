using MediTrack.BlazorWASM.Models;

namespace MediTrack.BlazorWASM.Services;

public interface IPatientService
{
    Task<List<PatientModel>> GetPatientsAsync();
    Task<PatientModel?> GetPatientAsync(int id);
    Task<PatientModel?> CreatePatientAsync(PatientModel patient);
    Task<bool> UpdatePatientAsync(PatientModel patient);
    Task<bool> DeletePatientAsync(int id);
}
