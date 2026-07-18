using MediTrack.Application.Dtos.Patient;



namespace MediTrack.Application.Dtos.Doctor;



public class DoctorCreateDto

{

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string Gender { get; set; } = string.Empty;

    public string LicenseNumber { get; set; } = string.Empty;

    public int YearsOfExperience { get; set; }

    public string OfficeLocation { get; set; } = string.Empty;

    public decimal ConsultationFee { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int DepartmentId { get; set; }

    public int SpecialtyId { get; set; }

    public AddressDto Address { get; set; } = new();

}

