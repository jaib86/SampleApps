using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using SamuraiApp.Domain;

#if PostgreSQL

using SamuraiApp.PostgreSQL.Data;

#else
using SamuraiApp.Data;
#endif

using System.Collections.Generic;
using System.Linq;

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

                // Disconnected - No Tracking - per Query
                context.Samurais.AsNoTracking().ToList();
                // Disconnected - No Tracking - per instance
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                // Attach disconnected entity
                context.ChangeTracker.TrackGraph(new Samurai { }, e => ApplyStateUsingIsKeySet(e.Entry));
                // Eager loading
                context.Samurais.Include(s => s.SecretIdentity).Include(s => s.Quotes).FirstOrDefault();
                // Delete entity
                samurai = context.Samurais.Find(1);
                context.Entry(samurai).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        private static void ApplyStateUsingIsKeySet(EntityEntry entry)
        {
            if (entry.IsKeySet)
            {
                if (((ClientChangeTracker)entry.Entity).IsDirty)
                {
                    entry.State = EntityState.Modified;
                }
                else
                {
                    entry.State = EntityState.Unchanged;
                }
            }
            else
            {
                entry.State = EntityState.Added;
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