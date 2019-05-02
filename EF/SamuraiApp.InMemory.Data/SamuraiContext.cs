using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;

namespace SamuraiApp.InMemory.Data
{
    public class SamuraiContext : SamuraiContextBase
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            InMemoryDbContextOptionsExtensions.UseInMemoryDatabase(optionsBuilder, "SamuraiData");

            base.OnConfiguring(optionsBuilder);
        }
    }
}