using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MediTrack.Infrastructure.Context;

public class MediTrackDbContextFactory : IDesignTimeDbContextFactory<MediTrackDbContext>
{
    public MediTrackDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MediTrackDbContext>();
        optionsBuilder.UseSqlServer("Server=DESKTOP-FL68TIF\\SQLEXPRESS;Database=MediTrackDB;Trusted_Connection=True;TrustServerCertificate=True;");

        return new MediTrackDbContext(optionsBuilder.Options);
    }
}