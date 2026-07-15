namespace MediTrack.BlazorWASM.Models;

public class DoctorModel
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public string OfficeLocation { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string SpecialtyName { get; set; } = string.Empty;
    public AddressModel Address { get; set; } = new();
}