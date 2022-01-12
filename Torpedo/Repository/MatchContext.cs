using Microsoft.EntityFrameworkCore;
using Torpedo.Model;

namespace Torpedo.Repository
{
    public class MatchContext : DbContext
    {
        public DbSet<Match> Matches { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source=(localdb)\\mssqllocaldb;Database=Torpedo;Integrated Security=True;");
        }
    }
}
