using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;

namespace SamuraiApp.SQLServer.Data
{
    public class SamuraiContext : SamuraiContextBase
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SamuraiData;Integrated Security=True",
                options => options.MaxBatchSize(30));

            // Call SamuraiContextBase.OnConfiguring
            base.OnConfiguring(optionsBuilder);
        }
    }
}