using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using SamuraiApp.Domain;

#if PostgreSQL
using SamuraiApp.PostgreSql.Data;
#else
using SamuraiApp.Data;
#endif

using System.Collections.Generic;

namespace SamuraiConsoleUI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //InsertSemurai();
            InsertMultipleSemurais();
        }

        private static void InsertMultipleSemurais()
        {
            var samurai = new Samurai { Name = "Jaiprakash" };
            var samuraiRaksha = new Samurai { Name = "Raksha" };
            using (var context = new SamuraiContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new SamuraiApp.Data.MyLoggerProvider());
                context.Samurais.AddRange(new List<Samurai>() { samurai, samuraiRaksha });
                //context.Samurais.Last
                context.SaveChanges();
            }
        }

        private static void InsertSemurai()
        {
            var samurai = new Samurai { Name = "Jaiprakash" };
            using (var context = new SamuraiContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new SamuraiApp.Data.MyLoggerProvider());
                context.Samurais.Add(samurai);
                //context.Add(samurai);
                context.SaveChanges();
            }
        }
    }
}