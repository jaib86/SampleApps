using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;

namespace SamuraiApp.SQLite.Data
{
    public class SamuraiContext : SamuraiContextBase
    {
        public SamuraiContext(DbContextOptions options)
               : base(options) { }

        public SamuraiContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Filename=SamuraiData.db");

                // Call SamuraiContextBase.OnConfiguring
                base.OnConfiguring(optionsBuilder);
            }
        }
    }
}