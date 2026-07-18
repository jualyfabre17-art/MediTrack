using System;
using System.Collections.Generic;
using System.Text;
using MediTrack.Domain.Entities;
using MediTrack.Domain.Interfaces;
using MediTrack.Infrastructure.Context;
using MediTrack.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;

namespace MediTrack.Infrastructure.Repositories
{
    public class DoctorRepository : BaseRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(MediTrackDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Doctor>> GetDoctorsWithDetailsAsync()
        {
            return await _dbSet
                .Include(d => d.Department)
                .Include(d => d.Specialty)
                .ToListAsync();
        }
    }
}
