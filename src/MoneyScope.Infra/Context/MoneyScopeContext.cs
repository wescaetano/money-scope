using Microsoft.EntityFrameworkCore;
using MoneyScope.Domain;
using MoneyScope.Domain.AccessProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Infra.Context
{
    public class MoneyScopeContext(DbContextOptions<MoneyScopeContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Profile> Profiles { get; set; } = null!;
        public DbSet<ProfileModule> ProfilesModules { get; set; } = null!;
        public DbSet<ProfileUser> ProfilesUsers { get; set; } = null!;
        public DbSet<Module> Modules { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MoneyScopeContext).Assembly);
            //DateTimeConfig.ConfigDateTime(modelBuilder);
            EntitiesConfigurator.Configure(modelBuilder);
            DatabaseSeeder.Seed(modelBuilder);

        }
    }
}
