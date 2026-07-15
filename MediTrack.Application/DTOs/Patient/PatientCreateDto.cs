namespace MediTrack.Application.Dtos.Patient;

public class PatientCreateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty;
    public int BloodType { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public string Allergies { get; set; } = string.Empty;
    public string MedicalHistory { get; set; } = string.Empty;
    public string EmergencyContactName { get; set; } = string.Empty;
    public string EmergencyContactPhone { get; set; } = string.Empty;
    public AddressDto Address { get; set; } = new();
}
