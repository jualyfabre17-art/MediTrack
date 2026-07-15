using Microsoft.EntityFrameworkCore;
using MediTrack.Domain.Entities;
using MediTrack.Domain.Interfaces;
using MediTrack.Infrastructure.Context;
using MediTrack.Infrastructure.Core;

namespace MediTrack.Infrastructure.Repositories;

public interface IPatientRepository : IRepository<Patient>
{
    Task<Patient?> GetPatientWithDetailsAsync(int id);
    Task<IEnumerable<Patient>> GetPatientsByMedicalRecordNumberAsync(string medicalRecordNumber);
}

public class PatientRepository : BaseRepository<Patient>, IPatientRepository
{
    public PatientRepository(MediTrackDbContext context) : base(context)
    {
    }

    public async Task<Patient?> GetPatientWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(p => p.Appointments)
            .Include(p => p.MedicalRecords)
            .Include(p => p.Prescriptions)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Patient>> GetPatientsByMedicalRecordNumberAsync(string medicalRecordNumber)
    {
        return await _dbSet
            .Where(p => p.MedicalRecordNumber.Value == medicalRecordNumber)
            .ToListAsync();
    }
}
