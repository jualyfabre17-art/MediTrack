using Microsoft.EntityFrameworkCore;
using MediTrack.Domain.Entities;
using MediTrack.Domain.ValueObjects;

namespace MediTrack.Infrastructure.Context;

public class MediTrackDbContext : DbContext
{
    public MediTrackDbContext(DbContextOptions<MediTrackDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Medication> Medications { get; set; }
    public DbSet<Diagnosis> Diagnoses { get; set; }
    public DbSet<Symptom> Symptoms { get; set; }
    public DbSet<Allergy> Allergies { get; set; }
    public DbSet<LabTest> LabTests { get; set; }
    public DbSet<LabResult> LabResults { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Specialty> Specialties { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigurePatient(modelBuilder);
        ConfigureDoctor(modelBuilder);
        ConfigureAppointment(modelBuilder);
        ConfigureMedicalRecord(modelBuilder);
        ConfigurePrescription(modelBuilder);
        ConfigureMedication(modelBuilder);
        ConfigureDepartment(modelBuilder);
        ConfigureSpecialty(modelBuilder);
        ConfigureLabTest(modelBuilder);
    }

    private void ConfigurePatient(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(p => p.LastName).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Email).HasMaxLength(200);
            entity.Property(p => p.PhoneNumber).HasMaxLength(20);
            entity.Property(p => p.IdentificationNumber).IsRequired().HasMaxLength(20);
            entity.HasIndex(p => p.IdentificationNumber).IsUnique();
            entity.Property(p => p.Gender).HasMaxLength(20);

            entity.OwnsOne(p => p.Address, address =>
            {
                address.Property(a => a.Street).HasColumnName("Street").HasMaxLength(200);
                address.Property(a => a.City).HasColumnName("City").HasMaxLength(100);
                address.Property(a => a.State).HasColumnName("State").HasMaxLength(50);
                address.Property(a => a.PostalCode).HasColumnName("PostalCode").HasMaxLength(20);
                address.Property(a => a.Country).HasColumnName("Country").HasMaxLength(50);
            });

            entity.OwnsOne(p => p.MedicalRecordNumber, mrn =>
            {
                mrn.Property(m => m.Value)
                    .HasColumnName("MedicalRecordNumber")
                    .IsRequired()
                    .HasMaxLength(20);
            });

            entity.Property(p => p.BloodType).HasConversion<int>();
            entity.Property(p => p.Height).HasPrecision(5, 2);
            entity.Property(p => p.Weight).HasPrecision(5, 2);
        });
    }

    private void ConfigureDoctor(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(d => d.LastName).IsRequired().HasMaxLength(100);
            entity.Property(d => d.Email).HasMaxLength(200);
            entity.Property(d => d.PhoneNumber).HasMaxLength(20);
            entity.Property(d => d.LicenseNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(d => d.LicenseNumber).IsUnique();
            entity.Property(d => d.Gender).HasMaxLength(20);
            entity.Property(d => d.ConsultationFee).HasPrecision(10, 2);

            entity.OwnsOne(d => d.Address, address =>
            {
                address.Property(a => a.Street).HasColumnName("Street").HasMaxLength(200);
                address.Property(a => a.City).HasColumnName("City").HasMaxLength(100);
                address.Property(a => a.State).HasColumnName("State").HasMaxLength(50);
                address.Property(a => a.PostalCode).HasColumnName("PostalCode").HasMaxLength(20);
                address.Property(a => a.Country).HasColumnName("Country").HasMaxLength(50);
            });

            entity.HasOne(d => d.Department)
                .WithMany(dep => dep.Doctors)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Specialty)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecialtyId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureAppointment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Reason).HasMaxLength(500);
            entity.Property(a => a.Notes).HasMaxLength(1000);
            entity.Property(a => a.Status).HasConversion<int>();

            entity.HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(a => new { a.DoctorId, a.AppointmentDateTime })
                .IsUnique();
        });
    }

    private void ConfigureMedicalRecord(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(mr => mr.Id);
            entity.Property(mr => mr.Diagnosis).HasMaxLength(1000);
            entity.Property(mr => mr.Treatment).HasMaxLength(1000);
            entity.Property(mr => mr.Observations).HasMaxLength(2000);

            entity.HasOne(mr => mr.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(mr => mr.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(mr => mr.Doctor)
                .WithMany()
                .HasForeignKey(mr => mr.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(mr => mr.Diagnoses)
                .WithMany(d => d.MedicalRecords);
        });
    }

    private void ConfigurePrescription(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Indications).HasMaxLength(500);
            entity.Property(p => p.Dosage).HasMaxLength(100);

            entity.HasOne(p => p.Patient)
                .WithMany(pat => pat.Prescriptions)
                .HasForeignKey(p => p.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Doctor)
                .WithMany()
                .HasForeignKey(p => p.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.MedicalRecord)
                .WithMany(mr => mr.Prescriptions)
                .HasForeignKey(p => p.MedicalRecordId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Medication)
                .WithMany(m => m.Prescriptions)
                .HasForeignKey(p => p.MedicationId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureMedication(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Medication>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Name).IsRequired().HasMaxLength(200);
            entity.Property(m => m.ActiveIngredient).HasMaxLength(200);
            entity.Property(m => m.Presentation).HasMaxLength(100);
            entity.Property(m => m.Strength).HasMaxLength(50);
            entity.Property(m => m.RecommendedDosage).HasMaxLength(200);
            entity.Property(m => m.SideEffects).HasMaxLength(500);
            entity.HasIndex(m => m.Name).IsUnique();
        });
    }

    private void ConfigureDepartment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(200);
            entity.Property(d => d.Description).HasMaxLength(500);
            entity.Property(d => d.Location).HasMaxLength(200);
            entity.Property(d => d.PhoneNumber).HasMaxLength(20);
            entity.HasIndex(d => d.Name).IsUnique();
        });
    }

    private void ConfigureSpecialty(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Specialty>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(200);
            entity.Property(s => s.Description).HasMaxLength(500);
            entity.Property(s => s.Code).HasMaxLength(50);
            entity.HasIndex(s => s.Name).IsUnique();
            entity.HasIndex(s => s.Code).IsUnique();
        });
    }

    private void ConfigureLabTest(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LabTest>(entity =>
        {
            entity.HasKey(lt => lt.Id);
            entity.Property(lt => lt.Name).IsRequired().HasMaxLength(200);
            entity.Property(lt => lt.Type).HasMaxLength(100);
            entity.Property(lt => lt.Status).HasConversion<int>();
            entity.Property(lt => lt.Notes).HasMaxLength(500);

            entity.HasOne(lt => lt.Patient)
                .WithMany()
                .HasForeignKey(lt => lt.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(lt => lt.Doctor)
                .WithMany()
                .HasForeignKey(lt => lt.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
