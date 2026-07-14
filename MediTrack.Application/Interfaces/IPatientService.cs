using MediTrack.Application.Core;
using MediTrack.Application.Dtos.Patient;

namespace MediTrack.Application.Interfaces;

public interface IPatientService
{
    Task<ServiceResult<IEnumerable<PatientResponseDto>>> GetAllAsync();
    Task<ServiceResult<PatientResponseDto>> GetByIdAsync(int id);
    Task<ServiceResult<PatientResponseDto>> GetByMedicalRecordNumberAsync(string medicalRecordNumber);
    Task<ServiceResult<PatientResponseDto>> CreateAsync(PatientCreateDto createDto);
    Task<ServiceResult<PatientResponseDto>> UpdateAsync(PatientUpdateDto updateDto);
    Task<ServiceResult<bool>> DeleteAsync(int id);
    Task<ServiceResult<IEnumerable<PatientResponseDto>>> SearchAsync(string searchTerm);
}
