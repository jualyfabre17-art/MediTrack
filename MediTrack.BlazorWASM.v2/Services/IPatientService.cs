using MediTrack.BlazorWASM.v2.Models;

namespace MediTrack.BlazorWASM.v2.Services;

public interface IPatientService
{
    Task<List<PatientModel>> GetPatientsAsync();
    Task<PatientModel?> GetPatientAsync(int id);
    Task<PatientModel?> CreatePatientAsync(PatientCreateModel patient);
    Task<bool> UpdatePatientAsync(PatientModel patient);
    Task<bool> DeletePatientAsync(int id);
}
