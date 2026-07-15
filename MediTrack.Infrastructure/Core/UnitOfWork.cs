using Microsoft.EntityFrameworkCore.Storage;
using MediTrack.Domain.Entities;
using MediTrack.Domain.Interfaces;
using MediTrack.Infrastructure.Context;
using MediTrack.Infrastructure.Core;

namespace MediTrack.Infrastructure.Core;

public class UnitOfWork : IUnitOfWork
{
    private readonly MediTrackDbContext _context;
    private IDbContextTransaction? _transaction;
    private bool _disposed;

    private IRepository<Patient>? _patients;
    private IRepository<Doctor>? _doctors;
    private IRepository<Appointment>? _appointments;
    private IRepository<MedicalRecord>? _medicalRecords;
    private IRepository<Prescription>? _prescriptions;
    private IRepository<Medication>? _medications;
    private IRepository<Diagnosis>? _diagnoses;
    private IRepository<Symptom>? _symptoms;
    private IRepository<Allergy>? _allergies;
    private IRepository<LabTest>? _labTests;
    private IRepository<LabResult>? _labResults;
    private IRepository<Department>? _departments;
    private IRepository<Specialty>? _specialties;

    public UnitOfWork(MediTrackDbContext context)
    {
        _context = context;
    }

    public IRepository<Patient> Patients =>
        _patients ??= new BaseRepository<Patient>(_context);

    public IRepository<Doctor> Doctors =>
        _doctors ??= new BaseRepository<Doctor>(_context);

    public IRepository<Appointment> Appointments =>
        _appointments ??= new BaseRepository<Appointment>(_context);

    public IRepository<MedicalRecord> MedicalRecords =>
        _medicalRecords ??= new BaseRepository<MedicalRecord>(_context);

    public IRepository<Prescription> Prescriptions =>
        _prescriptions ??= new BaseRepository<Prescription>(_context);

    public IRepository<Medication> Medications =>
        _medications ??= new BaseRepository<Medication>(_context);

    public IRepository<Diagnosis> Diagnoses =>
        _diagnoses ??= new BaseRepository<Diagnosis>(_context);

    public IRepository<Symptom> Symptoms =>
        _symptoms ??= new BaseRepository<Symptom>(_context);

    public IRepository<Allergy> Allergies =>
        _allergies ??= new BaseRepository<Allergy>(_context);

    public IRepository<LabTest> LabTests =>
        _labTests ??= new BaseRepository<LabTest>(_context);

    public IRepository<LabResult> LabResults =>
        _labResults ??= new BaseRepository<LabResult>(_context);

    public IRepository<Department> Departments =>
        _departments ??= new BaseRepository<Department>(_context);

    public IRepository<Specialty> Specialties =>
        _specialties ??= new BaseRepository<Specialty>(_context);

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
                _transaction?.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}