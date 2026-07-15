using MediTrack.Application.Core;
using MediTrack.Application.Dtos.Doctor;
using MediTrack.Application.Dtos.Patient;
using MediTrack.Application.Interfaces;
using MediTrack.Domain.Entities;
using MediTrack.Domain.Interfaces;

namespace MediTrack.Application.Services;

public class DoctorService : BaseService, IDoctorService
{
    public DoctorService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<ServiceResult<IEnumerable<DoctorResponseDto>>> GetAllAsync()
    {
        try
        {
            var doctors = await _unitOfWork.Doctors.GetAllAsync();
            var response = doctors.Select(d => MapToResponseDto(d));
            return ServiceResult<IEnumerable<DoctorResponseDto>>.Success(response);
        }
        catch (Exception ex)
        {
            return HandleError<IEnumerable<DoctorResponseDto>>($"Error retrieving doctors: {ex.Message}");
        }
    }

    public async Task<ServiceResult<DoctorResponseDto>> GetByIdAsync(int id)
    {
        try
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (doctor == null)
                return ServiceResult<DoctorResponseDto>.NotFound("Doctor not found");

            return ServiceResult<DoctorResponseDto>.Success(MapToResponseDto(doctor));
        }
        catch (Exception ex)
        {
            return HandleError<DoctorResponseDto>($"Error retrieving doctor: {ex.Message}");
        }
    }

    public async Task<ServiceResult<IEnumerable<DoctorResponseDto>>> GetBySpecialtyAsync(int specialtyId)
    {
        try
        {
            var doctors = await _unitOfWork.Doctors
                .FindAsync(d => d.SpecialtyId == specialtyId);
            var response = doctors.Select(d => MapToResponseDto(d));
            return ServiceResult<IEnumerable<DoctorResponseDto>>.Success(response);
        }
        catch (Exception ex)
        {
            return HandleError<IEnumerable<DoctorResponseDto>>($"Error retrieving doctors by specialty: {ex.Message}");
        }
    }

    public async Task<ServiceResult<IEnumerable<DoctorResponseDto>>> GetByDepartmentAsync(int departmentId)
    {
        try
        {
            var doctors = await _unitOfWork.Doctors
                .FindAsync(d => d.DepartmentId == departmentId);
            var response = doctors.Select(d => MapToResponseDto(d));
            return ServiceResult<IEnumerable<DoctorResponseDto>>.Success(response);
        }
        catch (Exception ex)
        {
            return HandleError<IEnumerable<DoctorResponseDto>>($"Error retrieving doctors by department: {ex.Message}");
        }
    }

    public async Task<ServiceResult<IEnumerable<DoctorResponseDto>>> GetAvailableDoctorsAsync(DateTime dateTime)
    {
        try
        {
            var doctors = await _unitOfWork.Doctors.GetAllAsync();
            var availableDoctors = new List<DoctorResponseDto>();

            foreach (var doctor in doctors)
            {
                var appointments = await _unitOfWork.Appointments
                    .FindAsync(a => a.DoctorId == doctor.Id &&
                                    a.AppointmentDateTime.Date == dateTime.Date &&
                                    a.Status != Domain.Enums.AppointmentStatus.Cancelled);
                if (!appointments.Any())
                {
                    availableDoctors.Add(MapToResponseDto(doctor));
                }
            }

            return ServiceResult<IEnumerable<DoctorResponseDto>>.Success(availableDoctors);
        }
        catch (Exception ex)
        {
            return HandleError<IEnumerable<DoctorResponseDto>>($"Error retrieving available doctors: {ex.Message}");
        }
    }

    public async Task<ServiceResult<DoctorResponseDto>> CreateAsync(DoctorCreateDto createDto)
    {
        try
        {
            var doctor = MapToEntity(createDto);

            await _unitOfWork.Doctors.AddAsync(doctor);
            await _unitOfWork.CompleteAsync();

            return ServiceResult<DoctorResponseDto>.Success(MapToResponseDto(doctor), "Doctor created successfully");
        }
        catch (Exception ex)
        {
            return HandleError<DoctorResponseDto>($"Error creating doctor: {ex.Message}");
        }
    }

    public async Task<ServiceResult<DoctorResponseDto>> UpdateAsync(DoctorUpdateDto updateDto)
    {
        try
        {
            var existingDoctor = await _unitOfWork.Doctors.GetByIdAsync(updateDto.Id);
            if (existingDoctor == null)
                return ServiceResult<DoctorResponseDto>.NotFound("Doctor not found");

            UpdateEntity(existingDoctor, updateDto);
            await _unitOfWork.Doctors.UpdateAsync(existingDoctor);
            await _unitOfWork.CompleteAsync();

            return ServiceResult<DoctorResponseDto>.Success(MapToResponseDto(existingDoctor), "Doctor updated successfully");
        }
        catch (Exception ex)
        {
            return HandleError<DoctorResponseDto>($"Error updating doctor: {ex.Message}");
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        try
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (doctor == null)
                return ServiceResult<bool>.NotFound("Doctor not found");

            doctor.IsActive = false;
            doctor.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Doctors.UpdateAsync(doctor);
            await _unitOfWork.CompleteAsync();

            return ServiceResult<bool>.Success(true, "Doctor deleted successfully");
        }
        catch (Exception ex)
        {
            return HandleError<bool>($"Error deleting doctor: {ex.Message}");
        }
    }

    // ============ MÉTODOS PRIVADOS DE MAPEO ============

    private static Doctor MapToEntity(DoctorCreateDto dto)
    {
        return new Doctor
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            LicenseNumber = dto.LicenseNumber,
            YearsOfExperience = dto.YearsOfExperience,
            OfficeLocation = dto.OfficeLocation,
            ConsultationFee = dto.ConsultationFee,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            DepartmentId = dto.DepartmentId,
            SpecialtyId = dto.SpecialtyId,
            Address = new Domain.ValueObjects.Address
            {
                Street = dto.Address.Street,
                City = dto.Address.City,
                State = dto.Address.State,
                PostalCode = dto.Address.PostalCode,
                Country = dto.Address.Country
            }
        };
    }

    private static void UpdateEntity(Doctor entity, DoctorUpdateDto dto)
    {
        entity.FirstName = dto.FirstName;
        entity.LastName = dto.LastName;
        entity.Email = dto.Email;
        entity.PhoneNumber = dto.PhoneNumber;
        entity.DateOfBirth = dto.DateOfBirth;
        entity.Gender = dto.Gender;
        entity.LicenseNumber = dto.LicenseNumber;
        entity.YearsOfExperience = dto.YearsOfExperience;
        entity.OfficeLocation = dto.OfficeLocation;
        entity.ConsultationFee = dto.ConsultationFee;
        entity.StartTime = dto.StartTime;
        entity.EndTime = dto.EndTime;
        entity.DepartmentId = dto.DepartmentId;
        entity.SpecialtyId = dto.SpecialtyId;
        entity.Address.Street = dto.Address.Street;
        entity.Address.City = dto.Address.City;
        entity.Address.State = dto.Address.State;
        entity.Address.PostalCode = dto.Address.PostalCode;
        entity.Address.Country = dto.Address.Country;
        entity.UpdatedAt = DateTime.UtcNow;
    }

    private static DoctorResponseDto MapToResponseDto(Doctor doctor)
    {
        return new DoctorResponseDto
        {
            Id = doctor.Id,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            FullName = doctor.FullName,
            Email = doctor.Email,
            PhoneNumber = doctor.PhoneNumber,
            DateOfBirth = doctor.DateOfBirth,
            Age = doctor.GetAge(),
            Gender = doctor.Gender,
            LicenseNumber = doctor.LicenseNumber,
            YearsOfExperience = doctor.YearsOfExperience,
            OfficeLocation = doctor.OfficeLocation,
            ConsultationFee = doctor.ConsultationFee,
            StartTime = doctor.StartTime.ToString(),
            EndTime = doctor.EndTime.ToString(),
            DepartmentName = doctor.Department?.Name ?? string.Empty,
            SpecialtyName = doctor.Specialty?.Name ?? string.Empty,
            Address = new AddressDto
            {
                Street = doctor.Address.Street,
                City = doctor.Address.City,
                State = doctor.Address.State,
                PostalCode = doctor.Address.PostalCode,
                Country = doctor.Address.Country
            },
            CreatedAt = doctor.CreatedAt,
            UpdatedAt = doctor.UpdatedAt
        };
    }
}