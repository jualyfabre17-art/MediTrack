using MediTrack.Application.Core;
using MediTrack.Application.Dtos.Doctor;

namespace MediTrack.Application.Interfaces;

public interface IDoctorService
{
    Task<ServiceResult<IEnumerable<DoctorResponseDto>>> GetAllAsync();
    Task<ServiceResult<DoctorResponseDto>> GetByIdAsync(int id);
    Task<ServiceResult<IEnumerable<DoctorResponseDto>>> GetBySpecialtyAsync(int specialtyId);
    Task<ServiceResult<IEnumerable<DoctorResponseDto>>> GetByDepartmentAsync(int departmentId);
    Task<ServiceResult<IEnumerable<DoctorResponseDto>>> GetAvailableDoctorsAsync(DateTime dateTime);
    Task<ServiceResult<DoctorResponseDto>> CreateAsync(DoctorCreateDto createDto);
    Task<ServiceResult<DoctorResponseDto>> UpdateAsync(DoctorUpdateDto updateDto);
    Task<ServiceResult<bool>> DeleteAsync(int id);
}
