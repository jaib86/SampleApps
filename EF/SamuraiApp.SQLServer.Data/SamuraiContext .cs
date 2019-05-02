using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;

namespace SamuraiApp.SQLServer.Data
{
    public class SamuraiContext : SamuraiContextBase
    {
        public SamuraiContext(DbContextOptions options)
            : base(options) { }

        public SamuraiContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SamuraiData;Integrated Security=True",
                    options => options.MaxBatchSize(30));

                // Call SamuraiContextBase.OnConfiguring
                base.OnConfiguring(optionsBuilder);
            }
        }
    }
}