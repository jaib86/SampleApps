using System.Linq;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;
using Xunit;
using Xunit.Abstractions;

namespace SamuraiApp.Test
{
    public class SamuraiTests
    {
        private readonly ITestOutputHelper output;

        public SamuraiTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void CanInsertSamuraiIntoSQLiteDb()
        {
            using (var context = new SQLite.Data.SamuraiContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                var samurai = new Samurai { Name = "Jaiprakash" };
                this.output.WriteLine($"Default samurai id: {samurai.Id}");
                context.Samurais.Add(samurai);
                var efDefaultId = samurai.Id;
                this.output.WriteLine($"EF default samurai id: {efDefaultId}");
                context.SaveChanges();
                this.output.WriteLine($"DB assigned samurai id: {samurai.Id}");
                Assert.NotEqual(efDefaultId, samurai.Id);
            }
        }

        [Fact]
        public void CanInsertSamuraiIntoPostgreSQLDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseNpgsql("Host=localhost;Database=TestDb;Username=postgres;Password=user1");
            using (var context = new PostgreSQL.Data.SamuraiContext(optionsBuilder.Options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                var samurai = new Samurai { Name = "Jaiprakash" };
                this.output.WriteLine($"Default samurai id: {samurai.Id}");
                context.Samurais.Add(samurai);
                var efDefaultId = samurai.Id;
                this.output.WriteLine($"EF default samurai id: {efDefaultId}");
                context.SaveChanges();
                this.output.WriteLine($"DB assigned samurai id: {samurai.Id}");
                Assert.NotEqual(efDefaultId, samurai.Id);
            }
        }

        [Fact]
        public void CanInsertSamuraiIntoSQLServerDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TestDb;Integrated Security=True");
            using (var context = new SQLServer.Data.SamuraiContext(optionsBuilder.Options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                var samurai = new Samurai { Name = "Jaiprakash" };
                this.output.WriteLine($"Default samurai id: {samurai.Id}");
                context.Samurais.Add(samurai);
                var efDefaultId = samurai.Id;
                this.output.WriteLine($"EF default samurai id: {efDefaultId}");
                context.SaveChanges();
                this.output.WriteLine($"DB assigned samurai id: {samurai.Id}");
                Assert.NotEqual(efDefaultId, samurai.Id);
            }
        }

        [Fact]
        public void CanInsertSamuraiInInMemoryDatabase()
        {
            using (var context = new InMemory.Data.SamuraiContext())
            {
                var samurai = new Samurai();
                this.output.WriteLine($"Default samurai id: {samurai.Id}");
                context.Samurais.Add(samurai);
                var efDefaultId = samurai.Id;
                this.output.WriteLine($"EF default samurai id: {efDefaultId}");
                context.SaveChanges();
                this.output.WriteLine($"DB assigned samurai id: {samurai.Id}");
                Assert.Equal(efDefaultId, samurai.Id);
            }
        }

        [Fact]
        public void CanInsertSamuraiWithSaveChanges()
        {
            // SQL Server
            var optionsBuilderSQLServer = new DbContextOptionsBuilder();
            InMemoryDbContextOptionsExtensions.UseInMemoryDatabase(optionsBuilderSQLServer, "SamuraiSQLData");
            using(var context = new SQLServer.Data.SamuraiContext(optionsBuilderSQLServer.Options))
            {
                var samurai = new Samurai { Name = "Julie" };
                context.Samurais.Add(samurai);
                context.SaveChanges();
            }
            using (var context = new SQLServer.Data.SamuraiContext(optionsBuilderSQLServer.Options))
            {
                Assert.Equal(1, context.Samurais.Count());
            }

            // Postgre SQL
            var optionsBuilderPostgreSQL = new DbContextOptionsBuilder();
            InMemoryDbContextOptionsExtensions.UseInMemoryDatabase(optionsBuilderPostgreSQL, "SamuraiPostgreSQL");
            using (var context = new PostgreSQL.Data.SamuraiContext(optionsBuilderPostgreSQL.Options))
            {
                var samurai = new Samurai { Name = "Julie" };
                context.Samurais.Add(samurai);
                context.SaveChanges();
            }
            using (var context = new PostgreSQL.Data.SamuraiContext(optionsBuilderPostgreSQL.Options))
            {
                Assert.Equal(1, context.Samurais.Count());
            }


            // SQLite
            var optionsBuilderSQLite = new DbContextOptionsBuilder();
            InMemoryDbContextOptionsExtensions.UseInMemoryDatabase(optionsBuilderSQLite, "SamuraiSQLite");
            using (var context = new SQLite.Data.SamuraiContext(optionsBuilderSQLite.Options))
            {
                var samurai = new Samurai { Name = "Julie" };
                context.Samurais.Add(samurai);
                context.SaveChanges();
            }
            using (var context = new SQLite.Data.SamuraiContext(optionsBuilderSQLite.Options))
            {
                Assert.Equal(1, context.Samurais.Count());
            }
        }
    }
}