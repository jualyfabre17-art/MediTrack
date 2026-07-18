using System.ComponentModel.DataAnnotations;

namespace MediTrack.BlazorWASM.Models;

public class DoctorUpdateModel
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "El género es obligatorio")]
    public string Gender { get; set; } = string.Empty;

    [Required(ErrorMessage = "El número de licencia es obligatorio")]
    [MaxLength(50)]
    public string LicenseNumber { get; set; } = string.Empty;

    [Range(0, 50, ErrorMessage = "Años de experiencia entre 0 y 50")]
    public int YearsOfExperience { get; set; }

    public string OfficeLocation { get; set; } = string.Empty;

    [Range(0, 999999, ErrorMessage = "Honorario debe ser un valor positivo")]
    public decimal ConsultationFee { get; set; }

    [Required(ErrorMessage = "La hora de inicio es obligatoria")]
    public string StartTime { get; set; } = "08:00";

    [Required(ErrorMessage = "La hora de fin es obligatoria")]
    public string EndTime { get; set; } = "17:00";

    [Required(ErrorMessage = "El ID del departamento es obligatorio")]
    public int DepartmentId { get; set; }

    [Required(ErrorMessage = "El ID de la especialidad es obligatorio")]
    public int SpecialtyId { get; set; }

    public AddressModel Address { get; set; } = new();
}
