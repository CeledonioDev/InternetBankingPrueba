using Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.EntityConfigurations;
using System.Linq;

namespace Persistence.Context
{
    public class InternetBankingContext : DbContext
    {
        /// <summary>
        /// Default connection string name
        /// </summary>
        public static string ConnectionString => "name=InternetBankingDatabase";

        /// <summary>
        /// Builds a new connection string that provides for <code></code>
        /// </summary>
        /// <param name="options"></param>
        public InternetBankingContext(DbContextOptions<InternetBankingContext> options)
            : base(options) { }

        internal DbSet<User> Users { get; set; }
        internal DbSet<UserAccount> UserAccounts { get; set; }
        internal DbSet<AccountLog> AccountLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var pb in modelBuilder.Model
               .GetEntityTypes()
               .SelectMany(t => t.GetProperties())
               .Where(p => p.ClrType == typeof(string))
               .Select(p => modelBuilder.Entity(p.DeclaringEntityType.ClrType).Property(p.Name)))
            {
                pb.HasColumnType("varchar(400)");
                pb.IsRequired();
            }

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserAccountConfiguration());
            modelBuilder.ApplyConfiguration(new AccountLogConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
