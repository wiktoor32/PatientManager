using Microsoft.EntityFrameworkCore;
using PatientManager.Api.Models;

namespace PatientManager.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        :base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();
}