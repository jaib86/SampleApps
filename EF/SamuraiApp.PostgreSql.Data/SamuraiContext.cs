﻿using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;

namespace SamuraiApp.PostgreSQL.Data
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
                optionsBuilder.UseNpgsql("Host=localhost;Database=SamuraiData;Username=postgres;Password=user1", options => options.MaxBatchSize(30));

                // Call SamuraiContextBase.OnConfiguring
                base.OnConfiguring(optionsBuilder);
            }
        }
    }
}