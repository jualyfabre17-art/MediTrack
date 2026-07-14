using MediTrack.Domain.Core;
using MediTrack.Domain.Enums;
using MediTrack.Domain.ValueObjects;

namespace MediTrack.Domain.Entities;

public class Patient : Person
{
    public string IdentificationNumber { get; set; } = string.Empty;
    public MedicalRecordNumber MedicalRecordNumber { get; set; } = new MedicalRecordNumber();
    public BloodType BloodType { get; set; }
    public double Height { get; set; } // in cm
    public double Weight { get; set; } // in kg
    public string Allergies { get; set; } = string.Empty;
    public string MedicalHistory { get; set; } = string.Empty;
    public string EmergencyContactName { get; set; } = string.Empty;
    public string EmergencyContactPhone { get; set; } = string.Empty;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}