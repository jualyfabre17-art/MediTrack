using MediTrack.Domain.Entities;

namespace MediTrack.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Patient> Patients { get; }
    IRepository<Doctor> Doctors { get; }
    IRepository<Appointment> Appointments { get; }
    IRepository<MedicalRecord> MedicalRecords { get; }
    IRepository<Prescription> Prescriptions { get; }
    IRepository<Medication> Medications { get; }
    IRepository<Diagnosis> Diagnoses { get; }
    IRepository<Symptom> Symptoms { get; }
    IRepository<Allergy> Allergies { get; }
    IRepository<LabTest> LabTests { get; }
    IRepository<LabResult> LabResults { get; }
    IRepository<Department> Departments { get; }
    IRepository<Specialty> Specialties { get; }

    Task<int> CompleteAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}