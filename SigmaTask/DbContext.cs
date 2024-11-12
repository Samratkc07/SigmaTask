using Microsoft.EntityFrameworkCore;

namespace SigmaTask
{
   public class CandidateDbContext : DbContext
{
    public CandidateDbContext(DbContextOptions<CandidateDbContext> options) : base(options) { }

    public DbSet<Candidate> Candidates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Candidate>()
            .HasIndex(c => c.Email)
            .IsUnique(); 
    }
}

}
