using System.Diagnostics;
using SamuraiApp.Domain;
using Xunit;

namespace SamuraiApp.Test
{
    public class SamuraiTests
    {
        [Fact]
        public void CanInsertSamuraiIntoPostgreSQLDatabase()
        {
            using (var context = new PostgreSQL.Data.SamuraiContext())
            {
                var samurai = new Samurai();
                Debug.WriteLine($"Default samurai id: {samurai.Id}");
                context.Samurais.Add(samurai);
                var efDefaultId = samurai.Id;
                Debug.WriteLine($"EF default samurai id: {efDefaultId}");
                context.SaveChanges();
                Debug.WriteLine($"DB assigned samurai id: {samurai.Id}");
                Assert.NotEqual(efDefaultId, samurai.Id);
            }
        }

        [Fact]
        public void CanInsertSamuraiIntoSQLServerDatabase()
        {
            using (var context = new SQLServer.Data.SamuraiContext())
            {
                var samurai = new Samurai();
                Debug.WriteLine($"Default samurai id: {samurai.Id}");
                context.Samurais.Add(samurai);
                var efDefaultId = samurai.Id;
                Debug.WriteLine($"EF default samurai id: {efDefaultId}");
                context.SaveChanges();
                Debug.WriteLine($"DB assigned samurai id: {samurai.Id}");
                Assert.NotEqual(efDefaultId, samurai.Id);
            }
        }
    }
}